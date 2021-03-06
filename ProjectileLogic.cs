﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    [SerializeField] float projectileLifeTime;
	[SerializeField] float projectileSpeed;
	[SerializeField] int projectileDamage;
	[SerializeField] DamageEffect projectileDamageStatusNumber;
	
	[SerializeField] ParticleSystem projectileExplosion;
	
	[SerializeField] PlayerStats ownerStats;
	
	public Rigidbody2D rigidbodyReference;
	public Animator animatorReference;
	
	public bool movesOnSpawn;
	public bool stopsOnImpact;
	public bool explodesOnImpact;
	public bool dealsDamage;
	
	public bool Impact { get; private set; }
	public Collider2D ImpactTarget {get; private set; }
	
	public void SetProjectileStats(int damage, DamageEffect statusNumber, float speed, float lifetime, float gravity, PlayerStats playerStats)
	{
		projectileDamage = damage;
		projectileDamageStatusNumber = statusNumber;
		projectileSpeed = speed;
		projectileLifeTime = lifetime;
		rigidbodyReference.gravityScale = gravity;
		ownerStats = playerStats;
	}
	
	public void SetProjectileEffects(ParticleSystem effect)
	{
		projectileExplosion = effect;
	}
	
	//used via animation events
	public void EraseProjectile()
	{
		Destroy(gameObject);
	}
	
	//used via animation events
	public void AddExplosionEffect()
	{
		Instantiate(projectileExplosion, gameObject.transform.position, gameObject.transform.rotation);
	}
	
	void Start()
	{
		if(movesOnSpawn)
		{
			MoveProjectile();	
		}
		if(explodesOnImpact)
		{
			stopsOnImpact = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D enemy)
	{		
		if(stopsOnImpact)
		{
			Impact = true;
			ImpactTarget = enemy;
			StopProjectile();
		}
		if(explodesOnImpact)
		{
			animatorReference.SetTrigger("Detonate");
		}
		if(dealsDamage)
		{	
			DealProjectileDamage(enemy, projectileDamage, projectileDamageStatusNumber);
		}
	}
	
	void Update()
    {
	   Destroy(gameObject, projectileLifeTime);
    }
	
	void DealProjectileDamage(Collider2D enemy, int damage, DamageEffect statusNumber)
	{
		CharacterCombat2D enemyCombatSystem = enemy.GetComponent<CharacterCombat2D>();
		PlayerStats enemyStats = enemy.GetComponent<PlayerStats>();
		
		if (enemyCombatSystem != null && enemyStats.blocking != true)
		{
			enemyCombatSystem.TakeDamage(damage, statusNumber);
			ownerStats.PlayerDealtDamage(damage);
		}
	}
	
	void MoveProjectile()
	{
		rigidbodyReference.velocity = transform.right * projectileSpeed;
	}
	
	void StopProjectile()
	{
		rigidbodyReference.gravityScale = 0;
		rigidbodyReference.velocity = new Vector2(0,0);
	}
	
	/*
	public void MoveProjectileBackwards()
	{
		if(ProjectileMovesRight())
		{
			rigidbodyReference.velocity = transform.right * projectileSpeed * (-1);
		}
		else if(!ProjectileMovesRight())
		{
			rigidbodyReference.velocity = transform.right * projectileSpeed;
		}
	}
	*/
	
	//TESTING ONLY:
	
	public bool ProjectileMovesRight()
	{
		if(rigidbodyReference.velocity.x >= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	void FlipProjectile()
	{
		if(!ProjectileMovesRight())
		{
			transform.Rotate(0f, 180f, 0f);
			Debug.Log("flip: " + transform.rotation);
		}
		else
		{
			Debug.Log("ProjectileMovesRight");
		}
	}
}
