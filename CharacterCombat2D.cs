using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat2D : MonoBehaviour
{
	public PlayerStats playerStats;
	
	public Animator animate;
	
	public Transform attackPoint;
	public float attackRange = 0.5f;
	public LayerMask enemyLayers;
	
	bool canCombo=false; 
	int clickAmount=0;
		
	public void DealAttackDamage(int number)
	{
		int damage;
		
		switch(number)
		{
			case 1 :
				damage=playerStats.firstAttackDamage;
				break;
			case 2 :
				damage=playerStats.secondAttackDamage;
				break;
			case 3 :
				damage=playerStats.thirdAttackDamage;
				break;
			default:
				damage=playerStats.firstAttackDamage;
				break;
		}
		
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<CharacterCombat2D>().TakeDamage(damage, number);
		}
	}
	
	public void TakeDamage(int damage, int number)
	{
		if(!playerStats.blocking)
		{
			switch(number)
			{
			case 1 :
				//Spawn hit particle
				Debug.Log("Took first attack damage");
				break;
			case 2 :
				Hurt();
				Debug.Log("Took second attack damage");
				break;
			case 3 :
				Knockback();
				Debug.Log("Took third attack damage"+playerStats.knockbackForce);
				break;
			default:
				Debug.Log("Took damage");
				break;
			}
			playerStats.health-=damage;
			Debug.Log("health"+playerStats.health);
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
		animate.SetTrigger("Hurt");
	}
	
	void Knockback()
	{
		animate.SetTrigger("Knockback");
		
		playerStats.canMove = false;
		playerStats.canAttack = false;
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
			TakeDamage(playerStats.firstAttackDamage,1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			TakeDamage(playerStats.secondAttackDamage,2);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			TakeDamage(playerStats.thirdAttackDamage,3);
		}
	}
}
