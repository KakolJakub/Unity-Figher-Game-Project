using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControls : MonoBehaviour
{
    public PlayerStats stats;
	
	public KeyCode moveLeft;
	public KeyCode moveRight;
	public KeyCode attack;
	public KeyCode block;
	public KeyCode firstAbility;
	public KeyCode secondAbility;
	public KeyCode thirdAbility;
	
	private CharacterMovement2D characterMovement2D;
	private CharacterCombat2D characterCombat2D;
	private CharacterAbilities2D characterAbilities2D;

    // Update is called once per frame
    void Update()										
    {
		//TODO: Maybe add an interface for gameplay systems ("IControllable" or "IGameplaySystem")
		//TODO: Maybe base the logic on events, not if statements
		
		if(Input.GetKey(moveLeft))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();    			
			characterMovement2D.Move(Direction.Left);
		}
		if(Input.GetKeyUp(moveLeft))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.DontMove();
			characterMovement2D = null;
		}
		if(Input.GetKeyDown(moveLeft))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.Dodge(Direction.Left);
		}
		
		if(Input.GetKey(moveRight))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.Move(Direction.Right);
		}
		if(Input.GetKeyUp(moveRight))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.DontMove();
			characterMovement2D = null;
		}
		if(Input.GetKeyDown(moveRight))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.Dodge(Direction.Right);
		}
		
		if(Input.GetKey(moveLeft) && Input.GetKey(moveRight))
		{
			characterMovement2D = GetComponent<CharacterMovement2D>();
			characterMovement2D.DontMove();
		}
		
		if(Input.GetKeyDown(attack))
		{
			characterCombat2D = GetComponent<CharacterCombat2D>();
			characterCombat2D.Attack();
		}
		
		if(Input.GetKeyDown(block))
		{
			characterCombat2D = GetComponent<CharacterCombat2D>();
			characterCombat2D.Block();
		}
		
		//TESTING ONLY:
		if(Input.GetKeyDown(KeyCode.I))
		{
			characterAbilities2D = GetComponent<CharacterAbilities2D>();
			characterAbilities2D.ability1.Use();
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			characterAbilities2D = GetComponent<CharacterAbilities2D>();
			characterAbilities2D.ability2.Use();
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			characterAbilities2D = GetComponent<CharacterAbilities2D>();
			characterAbilities2D.ability3.Use();
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			characterAbilities2D = GetComponent<CharacterAbilities2D>();
			characterAbilities2D.rageMode.Use();
		}
		
    }
	
	//IDEA:
	//Probably not in this script
	//Instead of returning nothing, return KeyCode
	public void DetectKey()
	{
		foreach(KeyCode code in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(code))
			{  
				Debug.Log("KeyCode down: " + code);
			}
		}
	}
}
