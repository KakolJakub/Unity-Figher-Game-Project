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
   
   string ropeTipName;
   bool attachRope;
   
   public override void ActivateAbility()
   {
	   VoltaicHook_Fire();
	   //animate.SetTrigger("Ability_VoltaicHook");
   }
   
   public void VoltaicHook_Fire()
   {
	   //DONE: Instantiate RopeTip
	   //DONE: LineRenderer follows RopeTip
	   //DONE: When tip hits target it stops moving
	   //TODO: if it doesn't in X seconds it moves back
	   //TODO: when tip hits target, perform VoltaicHook_Pull()
	   SpawnRope();
	   //if()
	   //{
		//   VoltaicHook_Pull();
	   //}
   }
   
   public void VoltaicHook_DealDamage()
   {
	   GetComponent<CharacterCombat2D>().DealCombatDamage(abilityDamage, abilityDamageEffect);
   }
   
   public void VoltaicHook_Pull()
   {
	   GetComponent<CharacterMovement2D>().MovePlayerForward(pullDistance);
   }
   
   void FixedUpdate()
   {
	   if(rope.enabled)
	   {
		   TrackRopeTip();
	   }
   }
   
   void Start()
   {
	   rope.enabled = false;
	   GetRopeTipName();
   }
   
   void SpawnRope()
   {
	   Instantiate(ropeTip, ropeSpawnPoint.position, ropeSpawnPoint.rotation);
	   TrackRopeTip();
	   rope.enabled = true;
   }
   
   void TrackRopeTip()
   {
	   rope.SetPosition(0, ropeSpawnPoint.position); 
	   rope.SetPosition(1, GetRopeTipPosition());
   }
   
   void GetRopeTipName()
   {
	   string clonePrefix = "(Clone)";
	   ropeTipName = ropeTip.name + clonePrefix;
   }
   
   Vector3 GetRopeTipPosition()
   {
	   return GameObject.Find(ropeTipName).transform.position;
   }
}
