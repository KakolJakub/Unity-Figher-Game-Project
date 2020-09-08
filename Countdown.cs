using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make adjustments or delete this script
public static class Countdown
{
	
	public static float timer=0;
	
	public static void Set(float time)
	{
		timer=time;
	}
	
	//use it in Update()
	public static float Begin()
	{
		Debug.Log(timer);
		if (timer>0)
		{
			timer-=Time.deltaTime;
		}
		if (timer<=0)
		{
			timer=0;
		}
		return timer;
	}
	
	//use it in Update()
	public static void CountTime()
	{
		if (timer>0)
		{
			timer-=Time.deltaTime;
			
		}
		if (timer<=0)
		{
			timer=0;
			
		}
		Debug.Log(timer);
	}
}
