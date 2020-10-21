using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make adjustments or delete this script
public static class Countdown
{
	
	public static float timer = 0;
	public static bool timerSet = false;
	
	public static void Set(float time)
	{
		timer = time;
		timerSet = true;
	}
	
	//use it in Update()
	public static float CountTime()
	{
		if(timerSet)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			
			}
			if (timer <= 0)
			{
				timer = 0;
			}
			//Debug.Log(timer);
			return timer;
		}
		else
		{
			return 0;
		}
	}
}
