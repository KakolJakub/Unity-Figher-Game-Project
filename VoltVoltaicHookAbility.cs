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
   
   public override void ActivateAbility()
   {
	   animate.ResetTrigger("Ability_VoltaicHook_Return");
	   animate.ResetTrigger("Ability_VoltaicHook_Pull");
	   animate.SetTrigger("Ability_VoltaicHook");
   }
   
   //used via animation event
   public void VoltaicHook_Fire()
   {   
	   SpawnRope();   
   }
   
   //used via animation event
   public void VoltaicHook_DealDamage()
   {
	   GetComponent<CharacterCombat2D>().DealCombatDamage(abilityDamage, abilityDamageEffect);
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
		   Debug.Log("RopeTip was erased.");
	   }
   }
   
   void OnEnable()
   {
	   playerStats.OnInterrupt += VoltaicHook_DestroyRopeTip;
   }
   
   void OnDisable()
   {
	   playerStats.OnInterrupt -= VoltaicHook_DestroyRopeTip;
   }
   
   void Update()
   {   
	   if(rope.enabled)
	   {
		   TrackRopeTip();
	   }
	   
   }
   
   void Start()
   {
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
}
