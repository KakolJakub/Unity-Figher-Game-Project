using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Countdown
{
	
	public static float timer=0;
	
	public static void Set(float time)
	{
		timer=time;
	}
	
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
}
