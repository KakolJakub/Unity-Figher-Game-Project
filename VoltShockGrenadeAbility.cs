﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltShockGrenadeAbility : Ability
{	
	public Transform projectileSpawnPoint;
	public Transform projectile;
	
	public ParticleSystem explosion;
	
	public float grenadeSpeed;
	public float grenadeLifeTime;
	public float grenadeGravity;
	
	void Start()
	{
		AdjustGrenadeStats();
	}
	
	public override void ActivateAbility()
	{
		AdjustGrenadeStats();
		animate.SetTrigger("Ability_ShockGrenade");
	}
	
	public void ShockGrenade_ThrowGrenade()
	{
		Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
	}
	
	void AdjustGrenadeStats()
	{
		projectile.GetComponent<ProjectileLogic>().SetProjectileStats(abilityDamage, abilityDamageEffect, grenadeSpeed, grenadeLifeTime, grenadeGravity, playerStats);
		projectile.GetComponent<ProjectileLogic>().SetProjectileEffects(explosion);
	}
	
	//TODO:
	//Adjust grenade animation to fit the direction
	//Maybe add energyColor to CharacterAbilities Script
	
	//DONE: Make an enum for damageEffects	
	//DONE: Make final touches: Adjust animation (look and speed), Check for bugs
	//DONE: Make the player unable to move or attack when casting this ability
	//DONE: Add gravity to the Shock Grenade Projectile (the gravity value should be set in this script)
	//DONE: Create electric particles - used when the grenade detonates
	//DONE: Implement animation - add an animator reference, add a SetTrigger
	//DONE: Create a DealProjectileDamage method - used on collider collision (ProjectileLogic script)
	//DONE: Create GameObject which will serve as a projectile spawn point and Attach it to the character
	//DONE: Create a Projectile Logic Script
	//DONE: Create a GameObject that will serve as a projectile (later used as a prefab)
	//DONE: Create a Projectile Spawn method, it will be used via animation event
	
	

}
