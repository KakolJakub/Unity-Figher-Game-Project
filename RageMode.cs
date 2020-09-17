using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RageMode : MonoBehaviour
{
    int rageMeter;
	bool rageReady;
	
	void Start()
	{
		rageReady = false;
		rageMeter = 0;
	}
	
	
	//int RageMeter; //Max amount
	//int RageAmount; //Current amount
	//RageAmount goes up: when player takes damage, when player deals damage,
	//When RageAmount >= RageMeter, RageModeAvailable; playerStats.rageMode = true;
	//ActivateRageMode() { //plays cutscene and freezes the game/playerInput //ActivateCharacterRageBonus()}
	//RageMode grants: damage increase, shorter ability cooldown, higher dodgeRegen speed
}
