using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltModernRageMode : RageMode
{
    public Transform voltPrimaryHand;
    public Transform voltSecondaryHand;

    public ParticleSystem shockParticles;
    ParticleSystem primaryShockParticles;
    ParticleSystem secondaryShockParticles;
    
    public override void AddBonusEffects()
	{
		primaryShockParticles = Instantiate(shockParticles, voltPrimaryHand.position, voltPrimaryHand.rotation);
		secondaryShockParticles = Instantiate(shockParticles, voltSecondaryHand.position, voltSecondaryHand.rotation);
        Debug.Log("VoltModernRage effects added.");
	}
	
	public override void RemoveBonusEffects()
	{
		primaryShockParticles.Stop();
		secondaryShockParticles.Stop();

        Debug.Log("VoltModernRage effects removed.");
	}

    void Update()
    {
        base.Update();

        if(primaryShockParticles != null)
        {
            primaryShockParticles.transform.position = voltPrimaryHand.position;
        }

        if(secondaryShockParticles != null)
        {
            secondaryShockParticles.transform.position = voltSecondaryHand.position;
        }
       
    }
}
