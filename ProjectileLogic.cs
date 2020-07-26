using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    [SerializeField] float projectileLifeTime;
	[SerializeField] float projectileSpeed;
	[SerializeField] int projectileDamage;
	[SerializeField] int projectileDamageStatusNumber;
	
	public Rigidbody2D rigidbodyReference;
	public Animator animatorReference;
	
	public bool movesOnSpawn;
	public bool explodesOnImpact;
	public bool dealsDamage;
	
	public void SetProjectileStats(int damage, int statusNumber, float speed, float lifetime)
	{
		projectileDamage = damage;
		projectileDamageStatusNumber = statusNumber;
		projectileSpeed = speed;
		projectileLifeTime = lifetime;
	}
	
	public void EraseProjectile()
	{
		Destroy(gameObject);
	}
	
	void Start()
	{
		if(movesOnSpawn)
		{
			MoveProjectile();	
		}
	}
	
	void OnTriggerEnter2D(Collider2D enemy)
	{		
		if(explodesOnImpact)
		{
			StopProjectile();
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
	
	void DealProjectileDamage(Collider2D enemy, int damage, int statusNumber)
	{
		CharacterCombat2D enemyCombatSystem = enemy.GetComponent<CharacterCombat2D>();
		if (enemy != null)
		{
			enemyCombatSystem.TakeDamage(damage, statusNumber);
		}
	}
	
	void MoveProjectile()
	{
		rigidbodyReference.velocity = transform.right * projectileSpeed;	
	}
	
	void StopProjectile()
	{
		rigidbodyReference.velocity = new Vector2(0,0);
	}
}
