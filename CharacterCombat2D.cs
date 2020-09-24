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
	
	bool canCombo=false; 
	int clickAmount=0;
		
	public void DealAttackDamage(MeleeAttack number)
	{
		int damage;
		DamageEffect damageEffect;
		
		switch(number)
		{
			case MeleeAttack.First :
				damage = playerStats.firstAttackDamage;
				damageEffect = DamageEffect.Hit;
				break;
			case MeleeAttack.Second :
				damage = playerStats.secondAttackDamage;
				damageEffect = DamageEffect.Hurt;
				break;
			case MeleeAttack.Third :
				damage = playerStats.thirdAttackDamage;
				damageEffect = DamageEffect.Knockback;
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
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<CharacterCombat2D>().TakeDamage(damage, effect);
			playerStats.PlayerDealtDamage(damage);
		}
		
	}
	
	public void TakeDamage(int damage, DamageEffect effect)
	{
		if(!playerStats.blocking)
		{
			switch(effect)
			{
			case DamageEffect.Hit :
				//Spawn hit particle
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffect.Hurt :
				Hurt();
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffect.Knockback :
				Knockback();
				Debug.Log(name + " affected by: " + effect);
				break;
			default:
				Debug.Log("Took damage");
				break;
			}
			
			playerStats.health-=damage;
			playerStats.PlayerTookDamage(damage);
			//Debug.Log(name+" health: "+playerStats.health);
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
		playerStats.canAttack=false;
	}
	
	//used via animation events on IdleAnimation
	public void AttackReady()
	{
		playerStats.canAttack=true;
	}
	
	public void Attack()
	{
		if(playerStats.canAttack)
		{
			playerStats.canMove=false;
			clickAmount++;
			ComboAvailable();
			ComboTimer(playerStats.comboTime);
			ManageAttacks(clickAmount);
		}
	}
	
	public void Block()
	{
		if(playerStats.canAttack)
		{
			playerStats.canMove=false;
			animate.SetTrigger("Block");
		}
	}
	
	void ComboTimer(float time)
	{
		CancelInvoke();
		Invoke("ComboEnds", time);
	}
	
	void ComboAvailable()
	{
		canCombo=true;
	}

	void ComboEnds()
	{
		canCombo=false;
		clickAmount=0;
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
			ThirdAttack();
			ComboEnds();
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
		
		playerStats.canMove = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
		
		animate.SetTrigger("Hurt");
	}
	
	void Knockback()
	{	
		//playerStats.PlayerWasInterrupted(); -> on animation event
		
		playerStats.canMove = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
		
		animate.SetTrigger("Knockback");
	}
	
	void OnEnable()
	{
		playerStats.OnInterrupt += BlockOff;
	}
	
	void OnDisable()
	{
		playerStats.OnInterrupt -= BlockOff;
	}
	
	void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
			return;
		
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
	
	//TESTING ONLY:
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
	
	public bool GetComboInfo()
	{
		return canCombo;
	}
}
