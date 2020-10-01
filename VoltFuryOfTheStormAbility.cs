using System;
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
   public Transform particleSpawnPoint;
   public ParticleSystem particles;	//TODO: Create particles.
   
   [SerializeField] int noOfRegularHits = 4;
   [SerializeField] int finalHitMultiplier;
   
   ParticleSystem actualParticles;
   
   public override void ActivateAbility()
   {
	   animate.SetTrigger("Ability_FuryOfTheStorm");
	   Debug.Log("You used: " + abilityName);
   }
   
   //used via animation event
   public void FuryOfTheStorm_DealDamage(FuryOfTheStormAttack attackNumber)
   {   
	   int dmg;
	   DamageEffect dmgEffect;
	   
	   if(attackNumber == FuryOfTheStormAttack.FifthFuryAttack)
	   {
		   dmg = CalculateDamage(attackNumber);
		   dmgEffect = DamageEffect.Knockback;
	   }
	   else
	   {
		   dmg = CalculateDamage(attackNumber);
		   dmgEffect = abilityDamageEffect;
	   }
	   
	   GetComponent<CharacterCombat2D>().DealCombatDamage(dmg, dmgEffect);
	   Debug.Log("FuryOfTheStorm dealt: " + dmg);
   }
   
   //used via animation event
   public void FuryOfTheStorm_Energize()
   {
	   actualParticles = Instantiate(particles, particleSpawnPoint.position, particleSpawnPoint.rotation);
   }
   
   public void FuryOfTheStorm_EnergizeOff()
   {
	  actualParticles.Stop();
   }
   
   int CalculateDamage(FuryOfTheStormAttack attackNumber)
   {
	   double regularHitDamage = Math.Round((double)(abilityDamage / (noOfRegularHits + finalHitMultiplier)));
	   
	   int finalHitDamage = (int)regularHitDamage * finalHitMultiplier;
	   
	   if((finalHitDamage + (noOfRegularHits * regularHitDamage)) != abilityDamage)
	   {
		   finalHitDamage = abilityDamage - (noOfRegularHits * (int)regularHitDamage);
	   }
	   
	   if(attackNumber == FuryOfTheStormAttack.FifthFuryAttack)
	   {
		  return finalHitDamage;
	   }
	   else
	   {
		   return (int)regularHitDamage;
	   }
   }
   
   void Update()
   {
	   if(actualParticles != null)
	   {
		   actualParticles.transform.position = particleSpawnPoint.position;
	   }
   }
}
