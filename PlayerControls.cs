using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public PlayerStats stats;
	
	private CharacterMovement2D characterMovement2D;
	private CharacterCombat2D characterCombat2D;
	private CharacterAbilities2D characterAbilities2D;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.Move(-stats.movementSpeed*Time.fixedDeltaTime);
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.DontMove();
			characterMovement2D=null;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.Dodge("Left");
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.Move(stats.movementSpeed*Time.fixedDeltaTime);
		}
		if(Input.GetKeyUp(KeyCode.D))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.DontMove();
			characterMovement2D=null;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			characterMovement2D=GetComponent<CharacterMovement2D>();
			characterMovement2D.Dodge("Right");
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			characterCombat2D=GetComponent<CharacterCombat2D>();
			characterCombat2D.Attack();
		}
		
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			characterCombat2D=GetComponent<CharacterCombat2D>();
			characterCombat2D.Block();
		}
		
		//TESTING ONLY:
		if(Input.GetKeyDown(KeyCode.I))
		{
			characterAbilities2D=GetComponent<CharacterAbilities2D>();
			characterAbilities2D.ability1.Use();
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			characterAbilities2D=GetComponent<CharacterAbilities2D>();
			characterAbilities2D.ability2.Use();
		}
    }
}
