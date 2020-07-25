using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltShockGrenadeAbility : Ability
{
    public Transform projectileSpawnPoint;
	public Transform projectile;
	
	public override void ActivateAbility()
	{
		TestProjectileSpawn();
		Debug.Log("Volt Shock Grenade WIP");
	}
	
	public void TestProjectileSpawn()
	{
		Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
	}
		
	//TODO:
	//Implement animation - add a trigger
	//Create a public ShockGrenadeDealDamage method - used via animation events
	//Create electric particles - used when the grenade detonates
	
	//DONE: Create GameObject which will serve as a projectile spawn point and Attach it to the character
	//DONE: Create a Projectile Logic Script
	
	//TESTING: Create a GameObject that will serve as a projectile (later used as a prefab)
	//TESTING: Create a Projectile Spawn method, it will be used via animation event
	
	

}
