using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControls : MonoBehaviour
{
    public PlayerStats stats;

	public KeyCode moveLeft;
	public KeyCode moveRight;
	public KeyCode dodge;
	public KeyCode attack;
	public KeyCode block;
	public KeyCode firstAbility;
	public KeyCode secondAbility;
	public KeyCode thirdAbility;
	public KeyCode rageMode;

	[SerializeField] private KeyBinding defaultBinding;
	[SerializeField] private KeyBinding additionalBinding;

	private CharacterMovement2D characterMovement2D;
	private CharacterCombat2D characterCombat2D;
	private CharacterAbilities2D characterAbilities2D;


	void Start()
	{
		GetDefaultBindings();
		SetDefaultBindings();

		characterMovement2D = GetComponent<CharacterMovement2D>();
		characterCombat2D = GetComponent<CharacterCombat2D>();
		characterAbilities2D = GetComponent<CharacterAbilities2D>();
	}

    // Update is called once per frame
    void Update()										
    {
		//TODO: Maybe add an interface for gameplay systems ("IControllable" or "IGameplaySystem")
		//TODO: Maybe base the logic on events, not if statements
		//DONE: Add default bindings (for each player)
		
		if(Input.GetKey(moveLeft))
		{  			
			characterMovement2D.Move(Direction.Left);
		}
		if(Input.GetKeyUp(moveLeft))
		{
			characterMovement2D.DontMove();
		}
		if(Input.GetKey(moveLeft) && Input.GetKey(dodge))
		{
			characterMovement2D.NewDodge(Direction.Left);
		}
		
		if(Input.GetKey(moveRight))
		{
			characterMovement2D.Move(Direction.Right);
		}
		if(Input.GetKeyUp(moveRight))
		{
			characterMovement2D.DontMove();
		}
		if(Input.GetKey(moveRight) && Input.GetKey(dodge))
		{
			characterMovement2D.NewDodge(Direction.Right);
		}
		
		if(Input.GetKey(moveLeft) && Input.GetKey(moveRight))
		{
			characterMovement2D.DontMove();
		}
		
		if(Input.GetKeyDown(attack))
		{
			characterCombat2D.Attack();
		}
		
		if(Input.GetKeyDown(block))
		{
			characterCombat2D.Block();
		}
		
		if(Input.GetKeyDown(firstAbility))
		{
			characterAbilities2D.ability1.Use();
		}
		if(Input.GetKeyDown(secondAbility))
		{
			characterAbilities2D.ability2.Use();
		}
		if(Input.GetKeyDown(thirdAbility))
		{
			characterAbilities2D.ability3.Use();
		}

		if(Input.GetKeyDown(rageMode))
		{
			characterAbilities2D.rageMode.Use();
		}
		
    }
	
	void ChangeKeyBinding(KeyBinding k)
	{
		this.moveLeft = k.GetKey("moveLeft");
        this.moveRight = k.GetKey("moveRight");
		this.dodge = k.GetKey("dodge");
        this.attack = k.GetKey("attack");
        this.block = k.GetKey("block");
        this.firstAbility = k.GetKey("firstAbility");
        this.secondAbility = k.GetKey("secondAbility");
        this.thirdAbility = k.GetKey("thirdAbility");
        this.rageMode = k.GetKey("rageMode");
	}

	void GetDefaultBindings()
	{
		if(GameObject.Find("GameTester"))
		{
			defaultBinding = GameObject.Find("GameTester").GetComponent<GameplayTester>().ShareControls()[0];
			additionalBinding = GameObject.Find("GameTester").GetComponent<GameplayTester>().ShareControls()[1];
		}
	}

	void SetDefaultBindings()
	{
		if(GetComponent<PlayerStats>().GetPlayerId() == 1)
		{
			try
			{
				ChangeKeyBinding(defaultBinding);
			}
			catch
			{
				Debug.Log("Something went wrong with setting binding: " + defaultBinding);
			}

		}
		else
		{
			try
			{
				ChangeKeyBinding(additionalBinding);
			}
			catch
			{
				Debug.Log("Something went wrong with setting binding: " + additionalBinding);
			}
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
