using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
	public int health = 100;
	
	public float movementSpeed = 35;
	public float dodgeRange = 40;
	public bool canMove = true;
	
	public float comboTime = 3;
	public bool canAttack = true;
	public bool blocking = false;
	
	public int firstAttackDamage = 10;
	public int secondAttackDamage = 20;
	public int thirdAttackDamage = 30;
	
	public float knockbackForce = 10;
	
	public bool canCastAbility = true;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
