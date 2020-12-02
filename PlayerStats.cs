using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageEffect
{
	Hit, Hurt, Knockback
}

public class PlayerStats : MonoBehaviour
{
	[SerializeField] private int playerId;
	static int _playerId;
	
	public int health = 200;
	
	public float movementSpeed = 35;
	public int dodgeAmount = 3;
	public float dodgeRange = 15;
	public float dodgeRegenTime = 15;
	public float dodgeTimer = 0.5f;
	public bool dodging = false;
	public bool canMove = true;
	public bool canDodge = true;
	public Direction currentDirection;


	public float comboTime = 1;
	public bool canAttack = true;
	public bool blocking = false;
	
	public int firstAttackDamage = 10;
	public int secondAttackDamage = 20;
	public int thirdAttackDamage = 30;
	
	public float knockbackForce = 10;
	
	public bool canCastAbility = true;
	
	public bool rageActive = false;
	public float rageDuration = 30;
	public int rageMultiplier = 2;
	
	public delegate void TakeAction();
	public event TakeAction OnInterrupt;
	public static event TakeAction OnDeath;
	public event TakeAction OnRageMode;
	public event TakeAction OnRageModeOff;
	
	public delegate void GetDamage(int damage);
	public event GetDamage OnDamageTaken;
	public event GetDamage OnDamageDealt;
	
	void Awake()
	{
		_playerId++;
		playerId = _playerId;
	}
	
	void OnEnable()
	{
		OnRageMode += IncreasePlayerAttackDamage;
		OnRageModeOff += DecreasePlayerAttackDamage;
	}
	
	void OnDisable()
	{
		OnRageMode -= IncreasePlayerAttackDamage;
		OnRageModeOff -= DecreasePlayerAttackDamage;
	}
	
	void OnDestroy()
	{
		_playerId = 0;
	}
	
	//TODO: Create an universal method for these 4 events
	
	public void PlayerTookDamage(int damage)  
	{
		if(OnDamageTaken != null)
		{
			OnDamageTaken(damage);
		}
	}

	public void PlayerDealtDamage(int damage) 
	{
		if(OnDamageDealt != null)
		{
			OnDamageDealt(damage);
		}
	}
	
	public void PlayerWasInterrupted()
	{
		if(OnInterrupt != null)
		{
			OnInterrupt();
		}
	}
	
	public void PlayerDied()
	{
		if(OnDeath != null)
		{
			OnDeath();
		}
	}
	
	public void PlayerActivatedRage()
	{
		if(OnRageMode != null)
		{
			OnRageMode();
		}
	}
	
	public void PlayerDeactivatedRage()
	{
		if(OnRageModeOff != null)
		{
			OnRageModeOff();
		}
	}
	
	void IncreasePlayerAttackDamage()
	{
		firstAttackDamage *= rageMultiplier;
		secondAttackDamage *= rageMultiplier;
		thirdAttackDamage *= rageMultiplier;
	}
	
	void DecreasePlayerAttackDamage()
	{
		firstAttackDamage /= rageMultiplier;
		secondAttackDamage /= rageMultiplier;
		thirdAttackDamage /= rageMultiplier;
	}
	
	//TESTING ONLY:
	public int GetPlayerId()
	{
		return playerId;
	}
}
