using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTester : MonoBehaviour
{
    [SerializeField]
	private GameObject[] players;
	
	[SerializeField]
	private Text[] hpBars;
	
	[SerializeField]
	private Text[] dodges;
	
	[SerializeField]
	private Text[] abilities1;
	[SerializeField]
	private Text[] abilities2;
	[SerializeField]
	private Text[] abilities3;
	
	[SerializeField]
	private Text[] blockInfo;
	
	[SerializeField]
	private Text[] comboInfo;
	
	[SerializeField]
	private Text[] rageMeterAmount;
	
	[SerializeField]
	private Text[] rageInfo;
	
    void Start()
    {
		players[0] = GameObject.Find("Player");
		players[1] = GameObject.Find("TestDummy");
    }
	
	
	void Update()
    {
        try 
	{
		for(int i = 0; i < 2; i++)
		{
			hpBars[i].text = players[i].GetComponent<PlayerStats>().health.ToString(); if(players[i].GetComponent<PlayerStats>().health <= 0) { hpBars[i].text = "0"; }
			blockInfo[i].text = players[i].GetComponent<PlayerStats>().blocking.ToString();
			
			dodges[i].text = players[i].GetComponent<CharacterMovement2D>().currentDodgeAmount.ToString();
			
			abilities1[i].text = players[i].GetComponent<CharacterAbilities2D>().ability1.GetAbilityCooldown().ToString();
			abilities2[i].text = players[i].GetComponent<CharacterAbilities2D>().ability2.GetAbilityCooldown().ToString();
			abilities3[i].text = players[i].GetComponent<CharacterAbilities2D>().ability3.GetAbilityCooldown().ToString();
			
			comboInfo[i].text = players[i].GetComponent<CharacterCombat2D>().GetComboInfo().ToString();
			//TODO: add the same functionality for combo duration
			
			rageMeterAmount[i].text = players[i].GetComponent<RageMode>().GetRageMeterInfo().ToString();
			//up to change, probably: rageMeterAmount[i].text = players[i].GetComponent<CharacterAbilities2D>().rageMode.GetRageMeterAmount().ToString();
			rageInfo[i].text = players[i].GetComponent<PlayerStats>().rageActive.ToString();
			//TODO: add the same functionality for rageMode duration
			
		}
	}
	catch(Exception e)
	{
		//just to see other Debug Logs
	}
    }
	
}
