using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public abstract class RageMode : MonoBehaviour
{
    public PlayerStats playerStats;
	public Animator animate;
	public VideoClip rageClip;
	
	public delegate void ReactToRage(RageMode rage);
	public event ReactToRage OnRageCutsceneStart;
	public event ReactToRage OnRageCutsceneEnd;
	public static event ReactToRage OnEveryRageCutsceneEnd;

	bool rageReady;
	
	int rageMeterCap = 100;
	int rageMeter;
	
	float currentRageDuration;
	
	void OnEnable()
	{
		playerStats.OnDamageTaken += IncreaseRageMeter;
		playerStats.OnDamageDealt += IncreaseRageMeterByHalf;
		OnRageCutsceneEnd += ActivateRageMode;
		OnEveryRageCutsceneEnd += ResetBoolAnimation;
	}
	
	void OnDisable()
	{
		playerStats.OnDamageTaken -= IncreaseRageMeter;
		playerStats.OnDamageDealt -= IncreaseRageMeterByHalf;
		OnRageCutsceneEnd += ActivateRageMode;
		OnEveryRageCutsceneEnd -= ResetBoolAnimation;
	}
	
	void Start()
	{
		rageReady = false;
		rageMeter = 0;
	}
	
	protected void Update()
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
	}
	
	void IncreaseRageMeter(int amount)
	{
		rageMeter += amount;
		Debug.Log("rageMeter: " + rageMeter);
	}
	
	void IncreaseRageMeterByHalf(int amount)
	{
		double amountValue = amount;
		amountValue = Math.Round(amountValue / 2);
		rageMeter += (int)amountValue;
		Debug.Log("rageMeter: " + rageMeter);
	}

	void ResetBoolAnimation(RageMode rage)
	{
		if(animate.GetBool("Moving"))
		{
			animate.SetBool("Moving", false);
		}
	}

	public void Use()
	{
		if(!playerStats.rageActive)
		{
			if(rageReady)
			{
				PlayerRequestedCutscene();
			}
		}
	}

	public void PlayerRequestedCutscene()
	{
		if(OnRageCutsceneStart != null)
		{
			OnRageCutsceneStart(this);
		}
	}

	public void PlayerCutsceneEnded()
	{
		if(OnRageCutsceneEnd != null)
		{
			OnRageCutsceneEnd(this);
			OnEveryRageCutsceneEnd(this);
		}
	}
	
	public void ActivateRageMode(RageMode rage)
	{
		playerStats.rageActive = true;
		currentRageDuration = playerStats.rageDuration;
		rageMeter = 0;
		AddBonusEffects();
		playerStats.PlayerActivatedRage();
	}

	public void DeactivateRageMode()
	{
		rageMeter = 0;
		rageReady = false;
		playerStats.rageActive = false;
		RemoveBonusEffects();
		playerStats.PlayerDeactivatedRage();
	}
	
	public virtual void AddBonusEffects()
	{
		Debug.Log("Rage effects added.");
	}
	
	public virtual void RemoveBonusEffects()
	{
		Debug.Log("Rage effects removed.");
	}
	
	//TESTING ONLY:
	public int GetRageMeterInfo()
	{
		return rageMeter;
	}
	
	public float GetRageDuration()
	{
		return currentRageDuration;
	}

}
