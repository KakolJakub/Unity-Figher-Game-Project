using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMode : MonoBehaviour
{
    public PlayerStats playerStats;
	
	bool rageReady;
	
	int rageMeterCap = 100;
	int rageMeter;
	
	float currentRageDuration;
	
	void Start()
	{
		playerStats.OnDamageTaken += IncreaseRageMeter; //rage meter += 1/1 of the damage taken
		playerStats.OnDamageDealt += IncreaseRageMeterByHalf; //rage meter += 1/2 of the damage dealt
		rageReady = false; 
		rageMeter = 0;
	}
	
	void Update()
	{
		if(rageMeter >= rageMeterCap)
		{
			rageMeter = rageMeterCap;
			rageReady = true;
		}
		
		if(playerStats.rageActive)
		{
			rageMeter = 0;
			
			if(currentRageDuration > 0)
			{
				currentRageDuration -= Time.deltaTime;
				Debug.Log(currentRageDuration);
			}
			if(currentRageDuration <=0)
			{
				currentRageDuration = 0;
				DeactivateRageMode();
			}
		}
		
		//TESTING ONLY:
		if(!playerStats.rageActive)
		{
			if((Input.GetKeyDown(KeyCode.R)) && rageReady)
			{
				ActivateRageMode();
			}
		}
	}
	
	void IncreaseRageMeter(int amount)
	{
		rageMeter += amount;
		Debug.Log("rageMeter: " + rageMeter);
	}
	
	void IncreaseRageMeterByHalf(int amount)
	{
		double _amount = amount;
		_amount = Math.Round(_amount / 2);
		rageMeter += (int)_amount;
		Debug.Log("rageMeter: " + rageMeter);
	}
	
	public void ActivateRageMode()
	{
		currentRageDuration = playerStats.rageDuration;
		rageMeter = 0;
		playerStats.rageActive = true;
		playerStats.PlayerActivatedRage();
	}
	
	public void DeactivateRageMode()
	{
		rageMeter = 0;
		rageReady = false;
		playerStats.rageActive = false;
		playerStats.PlayerDeactivatedRage();
	}
	
	//TESTING ONLY:
	public int GetRageMeterInfo()
	{
		return rageMeter;
	}
	//int RageMeter; //Max amount
	//int RageAmount; //Current amount
	//RageAmount goes up: when player takes damage, when player deals damage,
	//When RageAmount >= RageMeter, RageModeAvailable; playerStats.rageMode = true;
	//ActivateRageMode() { //plays cutscene and freezes the game/playerInput //ActivateCharacterRageBonus()}
	//RageMode grants: damage increase, shorter ability cooldown, higher dodgeRegen speed
}
