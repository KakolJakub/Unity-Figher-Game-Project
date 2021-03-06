﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public Animator animate;
	
	public PlayerStats playerStats;
	
	public Sprite abilityIcon;
	public string abilityName;
	
	public int abilityDamage;
	int amplifiedAbilityDamage;
	public DamageEffect abilityDamageEffect;
	
	public float abilityCooldown;
	float activeCooldown;
	bool abilityReady = true;
	
	protected void Awake()
	{
		amplifiedAbilityDamage = abilityDamage * playerStats.rageMultiplier;
	}
	
	protected void OnEnable()
	{
		playerStats.OnRageMode += IncreaseAbilityDamage;
		playerStats.OnRageModeOff += DecreaseAbilityDamage;
	}
	
	protected void OnDisable()
	{
		playerStats.OnRageMode -= IncreaseAbilityDamage;
		playerStats.OnRageModeOff -= DecreaseAbilityDamage;
	}
	
	protected void FixedUpdate()
	{
		if(activeCooldown > 0)
		{
			abilityReady = false;
			activeCooldown -= Time.deltaTime;
		}
		if(activeCooldown <= 0)
		{
			activeCooldown = 0;
			abilityReady = true;
		}
		
	}
	
	protected void IncreaseAbilityDamage()
	{
		abilityDamage = amplifiedAbilityDamage;
		Debug.Log(abilityName + " damage was increased: " + abilityDamage);
	}
	
	protected void DecreaseAbilityDamage()
	{
		abilityDamage = amplifiedAbilityDamage / playerStats.rageMultiplier;
		Debug.Log(abilityName + " damage was lowered: " + abilityDamage);
	}
	
	public void Use()
	{
		if(playerStats.canCastAbility)
		{
			if(abilityReady)
			{
				activeCooldown = abilityCooldown;
				ActivateAbility();
			}
			else
			{
				Debug.Log(abilityName + " is not ready. " + activeCooldown);
			}
		}
	}
	
	public void CastingAbility()
	{
		playerStats.canMove = false;
		playerStats.canAttack = false;
	}
	
	public void CanUseAbility()
	{
		playerStats.canCastAbility = true;
	}
	
	public void CantUseAbility()
	{
		playerStats.canCastAbility = false;
	}
	
	public virtual void ActivateAbility()
	{
		Debug.Log("You used " + abilityName);
	}
	
	public void ResetCooldown()
	{
		activeCooldown = 0;
	}
	
	//testing only:
	public float GetAbilityCooldown()
	{
		return activeCooldown;
	}
	
}
