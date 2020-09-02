using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltVoltaicHookAbility : Ability
{
   //TODO: Complete animation
   //TODO: Try Raycast approach
   //OR:
   //Create animation trigger
   //Attach 2 gameObjects to the player (ropeBeginPoint and ropeEndPoint)
   //Create RopeLogic Script (or put that code here):
   //-> the ropeBeginPoint
   //-> when spawned, the rope needs to extend in the direction of ropeEndPoint (change Transform values and SpriteRenderer values accordingly)
   //-> when ropeEndPoint enters another Collider2D it stops extending and "attaches" to the Collider2D (NEEDS SOLVING)
   //-> then the rope shortens in the direction of ropeEndPoint OR ropeBeginPoint (no decision yet)
   //-> Player moves in the same direction
   //-> At the end of animation destroy the rope object and stop moving the player
   //TO SOLVE: Extending the rope only in one direction
   //TO SOLVE: Attaching the ropeEndPoint to the hit Collider2D
   
   //DONE: Animator reference should be put in the base Ability script (since every ability needs an Animator reference)
   
   public Transform ropeSpawnPoint;
   public Transform ropeTip;
   public LineRenderer rope;

   public float pullDistance;
   
   Transform actualRopeTip;
   bool attachRope;
   
   //OLD
   //string ropeTipName;
   
   public override void ActivateAbility()
   {
	   animate.SetTrigger("Ability_VoltaicHook");
	   //testing only:
	   //VoltaicHook_Fire();
   }
   
   //used via animation event
   public void VoltaicHook_Fire()
   {
	   //DONE: Instantiate RopeTip
	   //DONE: LineRenderer follows RopeTip
	   //DONE: When tip hits target it stops moving
	   //SCRAPPED(done by VoltaicHook_Adjust): if it doesn't in X seconds it moves back
	   //DONE: when tip hits target, perform VoltaicHook_Pull()
	   
	   SpawnRope();
	   
	   //IDEA: Make RopeTip a child of Player
	   //Move the RopeTip via this script
	   //By doing so, you avoid the chance of a bug appearing when another RopeTip appears
	   
   }
   
   //used via animation event
   public void VoltaicHook_DealDamage()
   {
	   GetComponent<CharacterCombat2D>().DealCombatDamage(abilityDamage, abilityDamageEffect);
   }
   
   //used via animation event
   public void VoltaicHook_Adjust()
   {
	   if(attachRope)
	   {
		   Debug.Log("VH: Pull");
		   //testing only:
		   VoltaicHook_Pull();
		   //animate.SetTrigger("VH_Pull").....
	   }
	   else
	   {
		   Debug.Log("VH: Return");
		   //testing only:
		   VoltaicHook_Retract();
		   //animate.SetTrigger("VH_Return").....
	   }
   }
   
   //used via animation event
   public void VoltaicHook_Pull()
   {
	   GetComponent<CharacterMovement2D>().MovePlayerForward(pullDistance);
   }
   
   //TODO:
   public void VoltaicHook_Retract()
   {
	   actualRopeTip.GetComponent<ProjectileLogic>().MoveProjectileBackwards();
	   actualRopeTip.GetComponent<ProjectileLogic>().dealsDamage = false;
   }
   
   void FixedUpdate()
   {
	   attachRope = GetRopeTipImpactInfo();
	   
	   if(rope.enabled)
	   {
		   TrackRopeTip();
	   }
	   //Debug.Log(GetRopeTipPosition());
   }
   
   void Start()
   {
	   attachRope = false;
	   rope.enabled = false;
	   //OLD
	   //GetRopeTipName();
   }
   
   void SpawnRope()
   {
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
   
   //OLD
   /*
   void GetRopeTipName()
   {
	   string clonePrefix = "(Clone)";
	   ropeTipName = ropeTip.name + clonePrefix;
   }
   */
   
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
	   
	   //OLD
	   /*
	   bool ropeImpactInfo;
	   
	   if(GameObject.Find(ropeTipName) == null)
	   {
		   ropeImpactInfo = false;
	   }
	   else
	   {
		   ropeImpactInfo = GameObject.Find(ropeTipName).GetComponent<ProjectileLogic>().Impact;
	   }
	   
	   return ropeImpactInfo;
	   */
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
	   
	   //OLD
	   /*
	   Vector3 ropeTipPosition;
	   
	   if(GameObject.Find(ropeTipName) == null)
	   {
		   ropeTipPosition = ropeSpawnPoint.position;
	   }
	   else
	   {
		   ropeTipPosition = GameObject.Find(ropeTipName).transform.position;
	   }
	   
	   return ropeTipPosition;
	   */
   }
}
