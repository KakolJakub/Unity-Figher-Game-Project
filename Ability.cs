using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public Animator animate;
	
	public PlayerStats playerStats;
	
	public Sprite abilityIcon;
	public string abilityName;
	
	public int abilityDamage;
	
	public float abilityCooldown;
	float activeCooldown;
	bool abilityReady = true;
	
	protected void Update()
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
		abilityCooldown = 0;
	}
	
}
