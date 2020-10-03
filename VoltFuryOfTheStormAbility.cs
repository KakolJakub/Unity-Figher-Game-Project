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
   public Transform primaryParticleSpawnPoint;
   public Transform secondaryParticleSpawnPoint;
   public ParticleSystem[] particles;	//TODO: Create particles.
   
   [SerializeField] int noOfRegularHits = 4;
   [SerializeField] int finalHitMultiplier;
   
   ParticleSystem[] actualParticles = new ParticleSystem[5];
   
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
   public void FuryOfTheStorm_Energize(FuryOfTheStormAttack attackNumber)
   {
	   int index;
	   index = (int)attackNumber;
	   
	   if(DeterminePrimarySpawnPoint(attackNumber))
	   {
		   actualParticles[index] = Instantiate(particles[index], primaryParticleSpawnPoint.position, primaryParticleSpawnPoint.rotation);
	   }
	   else
	   {
		   actualParticles[index] = Instantiate(particles[index], secondaryParticleSpawnPoint.position, secondaryParticleSpawnPoint.rotation);
	   }
   }
   
   public void FuryOfTheStorm_EnergizeOff(FuryOfTheStormAttack attackNumber)
   {
	  
	  int index;
	  index = (int)attackNumber;
	  
	  if(actualParticles[index] != null)
	  {
		  actualParticles[index].Stop();
	  }
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
   
   bool DeterminePrimarySpawnPoint(FuryOfTheStormAttack attackNumber)
   {
	   bool isPrimary;
	   
	   switch(attackNumber)
	   {
		   case FuryOfTheStormAttack.SecondFuryAttack :
		   isPrimary = false;
		   break;
		   
		   case FuryOfTheStormAttack.FourthFuryAttack :
		   isPrimary = false;
		   break;
		   
		   default:
		   isPrimary = true;
		   break;
	   }
	   
	   return isPrimary;
   }
   
   void Update()
   {
	   if(actualParticles != null)
	   {
		   for(int i = 0; i < actualParticles.Length; i++)
		   {
			   if(actualParticles[i] != null)
			   {
				   if(DeterminePrimarySpawnPoint((FuryOfTheStormAttack)i))
				   {
					   actualParticles[i].transform.position = primaryParticleSpawnPoint.position;
					}
					else
					{
						actualParticles[i].transform.position = secondaryParticleSpawnPoint.position;
					}
				}
			}
	   }

   }
}
