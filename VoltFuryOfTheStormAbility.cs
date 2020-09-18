using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FuryOfTheStormAttack
{
	FirstFuryAttack,
	SecondFuryAttack,
	ThirdFuryAttack,
	FourthFuryAttack,
	FifthFuryAttack
}

public class VoltFuryOfTheStormAbility : Ability
{
   public override void ActivateAbility()
   {
	   //animate.SetTrigger("Ability_FuryOfTheStorm");
	   Debug.Log("You used: " + abilityName);
   }
   
   //TODO:
   //One method for all 4 attacks
   //One method for the final attack
   //No enums in the process
   
   //used via animation event
   public void FuryOfTheStorm_DealDamage(FuryOfTheStormAttack attackNumber)
   {
	   int dmg;
	   DamageEffect dmgEffect;
	   
	   //TODO: 
	   // -> Determine each attack damage and damageEffect
	   // -> If you want to use one method, stay with enum approach
	   // -> Else you can use two (or more) methods, one for each kind of attack
	   
	   //TESTING ONLY:
	   if(attackNumber == FuryOfTheStormAttack.FifthFuryAttack)
	   {
		   dmg = 20;
		   dmgEffect = DamageEffect.Knockback;
	   }
	   else
	   {
		   dmg = abilityDamage;
		   dmgEffect = abilityDamageEffect;
	   }
	   
	   GetComponent<CharacterCombat2D>().DealCombatDamage(dmg, dmgEffect);
   }  
}
