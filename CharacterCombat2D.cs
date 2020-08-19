using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attacks
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
	
	bool canCombo=false; 
	int clickAmount=0;
		
	public void DealAttackDamage(Attacks number)
	{
		int damage;
		DamageEffects damageEffect;
		
		switch(number)
		{
			case Attacks.First :
				damage = playerStats.firstAttackDamage;
				damageEffect = DamageEffects.Hit;
				break;
			case Attacks.Second :
				damage = playerStats.secondAttackDamage;
				damageEffect = DamageEffects.Hurt;
				break;
			case Attacks.Third :
				damage = playerStats.thirdAttackDamage;
				damageEffect = DamageEffects.Knockback;
				break;
			default:
				damage = playerStats.firstAttackDamage;
				damageEffect = DamageEffects.Hit;
				break;
		}
		
		DealCombatDamage(damage, damageEffect);
	}
	
	public void DealCombatDamage(int damage, DamageEffects effect)
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<CharacterCombat2D>().TakeDamage(damage, effect);
		}
	}
	
	public void TakeDamage(int damage, DamageEffects effect)
	{
		if(!playerStats.blocking)
		{
			switch(effect)
			{
			case DamageEffects.Hit :
				//Spawn hit particle
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffects.Hurt :
				Hurt();
				Debug.Log(name + " affected by: " + effect);
				break;
			case DamageEffects.Knockback :
				Knockback();
				Debug.Log(name + " affected by: " + effect);
				break;
			default:
				Debug.Log("Took damage");
				break;
			}
			playerStats.health-=damage;
			Debug.Log(name+" health: "+playerStats.health);
		}
	}
	
	//used via animation events on Block
	public void BlockOff()
	{
		playerStats.blocking = false;
		Debug.Log("BlockOn: " + playerStats.blocking);
	}
	
	//used via animation events on Block
	public void BlockOn()
	{
		playerStats.blocking = true;
		Debug.Log("BlockOn: " + playerStats.blocking);
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
			AttackManager(clickAmount);
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
	
	void AttackManager(int attackNumber)
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
		playerStats.canMove = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
		animate.SetTrigger("Hurt");
	}
	
	void Knockback()
	{
		animate.SetTrigger("Knockback");
		
		playerStats.canMove = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
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
			TakeDamage(playerStats.firstAttackDamage, (DamageEffects)0);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			TakeDamage(playerStats.secondAttackDamage, (DamageEffects)1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			TakeDamage(playerStats.thirdAttackDamage, (DamageEffects)2);
		}
	}
}
