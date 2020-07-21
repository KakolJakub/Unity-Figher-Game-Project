using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public PlayerStats stats;
	
	private CharacterMovement2D CharacterMovement2D;
	private CharacterCombat2D CharacterCombat2D;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.Move(-stats.movementSpeed*Time.fixedDeltaTime);
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.DontMove();
			CharacterMovement2D=null;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.Dodge("Left");
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.Move(stats.movementSpeed*Time.fixedDeltaTime);
		}
		if(Input.GetKeyUp(KeyCode.D))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.DontMove();
			CharacterMovement2D=null;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			CharacterMovement2D=GetComponent<CharacterMovement2D>();
			CharacterMovement2D.Dodge("Right");
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CharacterCombat2D=GetComponent<CharacterCombat2D>();
			CharacterCombat2D.Attack();
		}
		
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			CharacterCombat2D=GetComponent<CharacterCombat2D>();
			CharacterCombat2D.Block();
		}
		
    }
}
