using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public float projectileLifeTime;
	public float projectileSpeed;
	public Rigidbody2D rigidbodyReference;
	
	public bool movesOnSpawn;
	public bool explodesOnImpact;
	
	void Start()
	{
		if(movesOnSpawn)
		{
			rigidbodyReference.velocity = transform.right * projectileSpeed;	
		}
	}
	
	void OnTriggerEnter2D(Collider2D enemy)
	{		
		if(explodesOnImpact)
		{
			//TODO - Explode Animation
			Destroy(gameObject);
		}
	}
	
	void Update()
    {
       Destroy(gameObject, projectileLifeTime);
    }
}
