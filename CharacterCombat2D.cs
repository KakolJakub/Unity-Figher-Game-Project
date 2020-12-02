using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeAttack
{
	First, Second, Third
}

public class CharacterCombat2D : MonoBehaviour
{
	public PlayerStats playerStats;
	
	public Animator animate;
	
	public Transform attackPoint;
	public float attackRange = 0.5f;
	public LayerMask enemyLayers;
	
	//public delegate void StopAction();
	//public static event StopAction OnInterrupt;
	
	bool canCombo = false; 
	int clickAmount = 0;
		
	public void DealAttackDamage(MeleeAttack number)
	{
		int damage;
		DamageEffect damageEffect;
		
		switch(number)
		{
			case MeleeAttack.First :
				damage = playerStats.firstAttackDamage;
				damageEffect = DamageEffect.Hurt;
				break;
			case MeleeAttack.Second :
				damage = playerStats.secondAttackDamage;
				damageEffect = DamageEffect.Hurt;
				break;
			case MeleeAttack.Third :
				damage = playerStats.thirdAttackDamage;
				damageEffect = DamageEffect.Knockback;
				ComboEnds();
				break;
			default:
				damage = playerStats.firstAttackDamage;
				damageEffect = DamageEffect.Hit;
				break;
		}
		
		DealCombatDamage(damage, damageEffect);
	}
	
	public void DealCombatDamage(int damage, DamageEffect effect)
	{
		Direction ownerDirection = playerStats.currentDirection;
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach(Collider2D enemy in hitEnemies)
		{
			
			if(/*enemy.GetComponent<PlayerStats>().blocking ||*/enemy.GetComponent<PlayerStats>().dodging)
			{
				return;
			}
			else
			{
				enemy.GetComponent<CharacterCombat2D>().TakeDamage(damage, effect, ownerDirection);
				playerStats.PlayerDealtDamage(damage);
			}
		}
	}
	
	public void TakeDamage(int damage, DamageEffect effect, Direction ownerDirection = Direction.Right)
	{
		if(!playerStats.blocking && !playerStats.dodging)
		{
			switch(effect)
			{
			case DamageEffect.Hit :
				//REMOVED: Spawn hit particle
				//Hurt();
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffect.Hurt :
				Hurt();
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffect.Knockback :
				Knockback(ownerDirection);
				Debug.Log(name + " affected by: " + effect);
				break;
			default:
				Debug.Log("Took damage");
				break;
			}
			
			playerStats.health -= damage;
			playerStats.PlayerTookDamage(damage);
			
			//Debug.Log(name+" health: "+playerStats.health);

		} 
		if(playerStats.health <= 0)
		{
			Die(); //TODO: Determine what happens when a player dies - disable Animation Controller or disable playerControls?
		}
	}
	
	//used via animation events on Block
	public void BlockOff()
	{
		playerStats.blocking = false;
		//Debug.Log("BlockOn: " + playerStats.blocking);
	}
	
	//used via animation events on Block
	public void BlockOn()
	{
		playerStats.blocking = true;
		//Debug.Log("BlockOn: " + playerStats.blocking);
	}
	
	//used via animation events on Attack1, Attack2 and Attack3
	public void AttackNotReady()
	{
		playerStats.canAttack = false;
	}
	
	//used via animation events on IdleAnimation
	public void AttackReady()
	{
		playerStats.canAttack = true;
	}
	
	public void Attack()
	{
		if(playerStats.canAttack)
		{
			playerStats.canMove = false;
			clickAmount++;
			ComboAvailable();
			ComboTimer(playerStats.comboTime);
			ManageAttacks(clickAmount);
		}
		Debug.Log("Był atak: " + clickAmount);
	}
	
	public void Block()
	{
		if(enabled)
		{
			if(playerStats.canAttack)
			{
				playerStats.canMove = false;
				animate.SetTrigger("Block");
			}
		}
	}
	
	void ComboTimer(float time)
	{
		CancelInvoke();
		Invoke("ComboEnds", time);
	}
	
	void ComboAvailable()
	{
		canCombo = true;
	}

	void ComboEnds()
	{
		canCombo = false;
		clickAmount = 0;
	}
	
	void ManageAttacks(int attackNumber)
	{
		if(attackNumber==1)
		{
			FirstAttack();
		}
		if (canCombo&&attackNumber==2)
		{
			SecondAttack();
		}
		if(canCombo&&attackNumber>=3) 
		{
			//playerStats.canMove = false;
			ThirdAttack();
			//ComboEnds();
		}
	}
 
	void FirstAttack()
	{
		animate.SetTrigger("FirstAttack");
		//Debug.Log("attack1");
	}
	
	void SecondAttack()
	{
		animate.SetTrigger("SecondAttack");
		//Debug.Log("attack2");
	}
	
	void ThirdAttack()
	{
		animate.SetTrigger("ThirdAttack");
		//Debug.Log("attack3");
	}
	
	void Hurt()
	{	
		//playerStats.PlayerWasInterrupted(); -> on animation event
		
		//playerStats.canMove = false;
		//playerStats.canAttack = false;
		//playerStats.canCastAbility = false;

		PermitActions();
		
		animate.SetTrigger("Hurt");
	}
	
	void Knockback(Direction ownerDirection)
	{	
		//playerStats.PlayerWasInterrupted(); -> on animation event

		if(ownerDirection == playerStats.currentDirection)
		{
			GetComponent<CharacterMovement2D>().TestFlip();
			GetComponent<CharacterMovement2D>().UpdateCurrentDirection();

		}
		
		//playerStats.canMove = false;
		//playerStats.canAttack = false;
		//playerStats.canCastAbility = false;
		PermitActions();
		
		animate.SetTrigger("Knockback");

	}
	
	void CancelAttacks()
	{
		animate.ResetTrigger("FirstAttack");
		animate.ResetTrigger("SecondAttack");
		animate.ResetTrigger("ThirdAttack");
		ComboEnds();
	}

	void PermitActions()
	{
		playerStats.canMove = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
	}

	void Die()
	{
		animate.SetTrigger("Death");
		playerStats.PlayerDied();
		GetComponent<BoxCollider2D>().enabled = false;
	}
	
	void OnEnable()
	{
		playerStats.OnInterrupt += BlockOff;
		playerStats.OnInterrupt += CancelAttacks;
		playerStats.OnInterrupt += PermitActions;
	}
	
	void OnDisable()
	{
		playerStats.OnInterrupt -= BlockOff;
		playerStats.OnInterrupt -= CancelAttacks;
		playerStats.OnInterrupt -= PermitActions;
	}
	
	void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
			return;
		
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
	
	//TESTING ONLY:
	/*
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			TakeDamage(playerStats.firstAttackDamage, (DamageEffect)0);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			TakeDamage(playerStats.secondAttackDamage, (DamageEffect)1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			TakeDamage(playerStats.thirdAttackDamage, (DamageEffect)2);
		}
	}
	*/
	
	public bool GetComboInfo()
	{
		return canCombo;
	}

	public bool DetectEnemies()
	{
		bool enemiesInRange;

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

		if(hitEnemies.Length > 0)
		{
			if(hitEnemies[0].GetComponent<PlayerStats>())
			{
				enemiesInRange = true;
			}
			else
			{
				enemiesInRange = false;
			}
		}
		else
		{
			enemiesInRange = false;
		}
		
		return enemiesInRange;
	}
}
