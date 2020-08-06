using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltVoltaicHookAbility : Ability
{
   //TODO: Try Raycast approach
   //OR:
   //Create animation trigger
   //Attach 2 gameObjects to the player (ropeBeginPoint and ropeEndPoint)
   //Create RopeLogic Script (or put that code here):
   //-> the ropeBeginPoint
   //-> when spawned, the rope needs to extend in the direction of ropeEndPoint (change Transform values and SpriteRenderer values accordingly)
   //-> when ropeEndPoint enters another Collider2D it stops extending and "attaches" to the Collider2D (NEEDS SOLVING)
   //-> then the rope shortens in the direction of ropeEndPoint
   //-> Player moves in the same direction
   //-> At the end of animation destroy the rope object and stop moving the player
   
   //TO SOLVE: Extending the rope only in one direction
   //TO SOLVE: Attaching the ropeEndPoint to the hit Collider2D
   
   //DONE: Animator reference should be put in the base Ability script (since every ability needs an Animator reference)
   
   public Transform ropeSpawnPoint;
   public LineRenderer rope;

   bool attachRope;
   
   public override void ActivateAbility()
   {
	   RaycastHit2D ropeImpact = Physics2D.Raycast(ropeSpawnPoint.position, ropeSpawnPoint.right);
	   if(ropeImpact)
	   {
		   Instantiate(rope, ropeSpawnPoint.position, ropeSpawnPoint.rotation);
		   rope.SetPosition(0, ropeSpawnPoint.position);
		   rope.SetPosition(1, ropeImpact.point);
		   attachRope = true;
		   Debug.Log("You attached the rope");
	   }
   }
   
   void Update()
   {
	   if(attachRope)
	   {
		   //TO FIX: Rope needs to update its position properly
		   rope.SetPosition(0, ropeSpawnPoint.position);
		   Debug.Log(ropeSpawnPoint.position);
	   }
   }
}
