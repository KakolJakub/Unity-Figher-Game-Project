using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public Sprite abilityIcon;
	public string abilityName;
	
	public int abilityDamage;
	
	public float abilityCooldown;
	float activeCooldown;
	bool abilityReady = true;
	
	protected void Update()
	{
		if(activeCooldown > 0)
		{
			abilityReady = false;
			activeCooldown -= Time.deltaTime;
		}
		if(activeCooldown <= 0)
		{
			activeCooldown = 0;
			abilityReady = true;
		}
		
	}
	
	public void Use()
	{
		if(abilityReady)
		{
			activeCooldown = abilityCooldown;
			ActivateAbility();
		}
		else
		{
			Debug.Log(abilityName + " is not ready. " + activeCooldown);
		}
	}
	
	public virtual void ActivateAbility()
	{
		Debug.Log("You used " + abilityName);
	}
}
