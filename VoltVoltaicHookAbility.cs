using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltVoltaicHookAbility : Ability
{   
   public Transform ropeSpawnPoint;
   public Transform ropeTip;
   public LineRenderer rope;

   public int ropeTipDamage;
   public DamageEffect ropeTipDamageEffect;
   public float ropeTipSpeed;
   public float ropeTipLifeTime;
   public float ropeTipGravity;
   
   public float pullSpeed;
   public float ropeReturnSpeed;
   
   Transform actualRopeTip;
   bool attachRope;

   Collider2D hookedEnemy;
   bool checkForEnemyDodge;
   
   public override void ActivateAbility()
   {
	   animate.ResetTrigger("Ability_VoltaicHook_Return");
	   animate.ResetTrigger("Ability_VoltaicHook_Pull");
	   animate.SetTrigger("Ability_VoltaicHook");
      checkForEnemyDodge = false;
   }
   
   //used via animation event
   public void VoltaicHook_Fire()
   {   
	   hookedEnemy = null;
      checkForEnemyDodge = true;
      SpawnRope();
   }
   
   //used via animation event
   public void VoltaicHook_DealDamage()
   {
      if(GetComponent<CharacterCombat2D>().DetectEnemies())
      {
         GetComponent<CharacterCombat2D>().DealCombatDamage(abilityDamage, abilityDamageEffect);
         animate.SetTrigger("Ability_VoltaicHook_PullReturn");
         checkForEnemyDodge = false;
      }
      //else
      //{
         //StopPlayerOnEnemyDodge();
      //}
   }
   
   //used via animation event
   public void VoltaicHook_Adjust()
   {
	   attachRope = GetRopeTipImpactInfo();
	   
	   if(attachRope)
	   {
		   animate.ResetTrigger("Ability_VoltaicHook_Return");
		   animate.SetTrigger("Ability_VoltaicHook_Pull");
	   }
	   else
	   {
		   animate.ResetTrigger("Ability_VoltaicHook_Pull");
		   animate.SetTrigger("Ability_VoltaicHook_Return");
	   }
	   
   }
   
   //used via animation event
   public void VoltaicHook_Pull()
   {
	   DisarmRopeTip();
	   GetComponent<CharacterMovement2D>().MovePlayerForward(pullSpeed);
      //StopPlayerOnEnemyDodge();
      
   }
   
   public void VoltaicHook_Retract()
   {
	   DisarmRopeTip();
	   MoveRopeTipBackwards();
   }
   
   public void VoltaicHook_DestroyRopeTip()
   {
	   if(actualRopeTip != null)
	   {
		   actualRopeTip.GetComponent<ProjectileLogic>().EraseProjectile();
		   rope.enabled = false;
         hookedEnemy = null;
         checkForEnemyDodge = false;
		   Debug.Log("RopeTip was erased.");
	   }
   }
   
   void OnEnable()
   {
	   base.OnEnable();
      playerStats.OnInterrupt += VoltaicHook_DestroyRopeTip;
   }
   
   void OnDisable()
   {
	   base.OnDisable();
      playerStats.OnInterrupt -= VoltaicHook_DestroyRopeTip;
   }
   
   void Update()
   {   
	   if(rope.enabled)
	   {
		   TrackRopeTip();
	   }
      if(checkForEnemyDodge)
      {
         StopPlayerOnEnemyDodge();
      }	   
   }
   
   void Start()
   {
	   hookedEnemy = null;
      checkForEnemyDodge = false;
      attachRope = false;
	   rope.enabled = false;
   }
   
   void SpawnRope()
   {
	   ropeTip.GetComponent<ProjectileLogic>().SetProjectileStats(ropeTipDamage, ropeTipDamageEffect, ropeTipSpeed, ropeTipLifeTime, ropeTipGravity, playerStats);
	   actualRopeTip = Instantiate(ropeTip, ropeSpawnPoint.position, ropeSpawnPoint.rotation);
	   TrackRopeTip();
	   rope.enabled = true;
   }
   
   void HideRope()
   {
	   rope.enabled = false;
   }
   
   void TrackRopeTip()
   {
	   rope.SetPosition(0, ropeSpawnPoint.position); 
	   rope.SetPosition(1, GetRopeTipPosition());
   }
   
   void DisarmRopeTip()
   {
	   actualRopeTip.GetComponent<ProjectileLogic>().dealsDamage = false;
   }
   
   void MoveRopeTipBackwards()
   {
		actualRopeTip.GetComponent<ProjectileLogic>().rigidbodyReference.velocity = transform.right * ropeReturnSpeed * (-1);
   }
   
   void StopPlayerOnEnemyDodge()
   {
      if(hookedEnemy == null)
      {
         if(GetRopeTipImpactTarget() != null)
         {
            hookedEnemy = GetRopeTipImpactTarget();
         }
      }

      if(hookedEnemy != null && hookedEnemy.GetComponent<PlayerStats>().dodging)
      {
         checkForEnemyDodge = false;
         animate.SetTrigger("Ability_VoltaicHook_PullReturn");
         hookedEnemy = null;
      }
   }

   bool GetRopeTipImpactInfo()
   {
	   bool ropeImpactInfo;
	   
	   if(actualRopeTip == null)
	   {
		   ropeImpactInfo = false;
	   }
	   else
	   {
		   ropeImpactInfo = actualRopeTip.GetComponent<ProjectileLogic>().Impact;
	   }
	   
	   return ropeImpactInfo;
	   
   }
   
   Vector3 GetRopeTipPosition()
   {
	   Vector3 ropeTipPosition;
	   
	   if(actualRopeTip == null)
	   {
		   ropeTipPosition = ropeSpawnPoint.position;
	   }
	   else
	   {
		   ropeTipPosition = actualRopeTip.transform.position;
	   }
	   
	   return ropeTipPosition;
	   
   }

   Collider2D GetRopeTipImpactTarget()
   {
	   Collider2D ropeImpactTarget;
	   
	   if(actualRopeTip == null)
	   {
		   ropeImpactTarget = null;
	   }
	   else
	   {
		   ropeImpactTarget = actualRopeTip.GetComponent<ProjectileLogic>().ImpactTarget;
	   }
	   
	   return ropeImpactTarget;
	   
   }

   /*
   Direction GetRopeTipDirection()
   {
      Direction direction;
      
      if(GetRopeTipPosition().x > transform.position.x)
      {
         direction = Direction.Right;
         Debug.Log("ROPETIP DIRECTION: " + direction);
      }
      else if(GetRopeTipPosition().x < transform.position.x)
      {
         direction = Direction.Left;
         Debug.Log("ROPETIP DIRECTION: " + direction);
      }
      else
      {
         direction = Direction.Right;
      }

      return direction;
   }
   */
}
