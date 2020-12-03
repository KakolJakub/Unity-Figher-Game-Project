using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyBinding
{
    [SerializeField] string name;
    
    [SerializeField] KeyCode moveLeft;
	[SerializeField] KeyCode moveRight;
	[SerializeField] KeyCode dodge;
	[SerializeField] KeyCode attack;
	[SerializeField] KeyCode block;
	[SerializeField] KeyCode firstAbility;
	[SerializeField] KeyCode secondAbility;
	[SerializeField] KeyCode thirdAbility;
	[SerializeField] KeyCode rageMode;


    //TODO: Consider adding a dictionary for this:
    public KeyCode GetKey(string keyName)
    {
        KeyCode key;
        switch(keyName)
        {
            case "moveLeft":
            key = moveLeft;
            break;

            case "moveRight":
            key = moveRight;
            break;
            
            case "dodge":
            key = dodge;
            break;

            case "attack":
            key = attack;
            break;

            case "block":
            key = block;
            break;

            case "firstAbility":
            key = firstAbility;
            break;

            case "secondAbility":
            key = secondAbility;
            break;

            case "thirdAbility":
            key = thirdAbility;
            break;

            case "rageMode":
            key = rageMode;
            break;

            default:
            Debug.Log("No key of that name. Binding set to SPACE.");
            key = KeyCode.Space;
            break;
            
        }
        return key;

    }
    /*
    public KeyBinding(KeyCode moveLeft, KeyCode moveRight, KeyCode attack, KeyCode block, KeyCode firstAbility, KeyCode secondAbility, KeyCode thirdAbility, KeyCode rageMode)
    {
        this.moveLeft = moveLeft;
        this.moveRight = moveRight;
        this.attack = attack;
        this.block = block;
        this.firstAbility = firstAbility;
        this.secondAbility = secondAbility;
        this.thirdAbility = thirdAbility;
        this.rageMode = rageMode;
    }
    */
    
}
