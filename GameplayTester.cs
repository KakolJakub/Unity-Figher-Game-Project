using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayTester : MonoBehaviour
{	
	public static bool gamePaused;
	
	[SerializeField] private Vector3 player1SpawnPoint;
	[SerializeField] private Vector3 player2SpawnPoint;
	
	[SerializeField] private KeyCode skipRoundCountdownButton;
	[SerializeField] private float roundCountdown;
	private float currentCountdown;
	private bool roundStarted;
	
	[SerializeField] private GameObject[] players;

	[SerializeField] private Text[] hpBars;
	
	[SerializeField] private Text[] dodges;
	
	[SerializeField] private Text[] abilities1;
	[SerializeField] private Text[] abilities2;
	[SerializeField] private Text[] abilities3;
	
	[SerializeField] private Text[] blockInfo;
	
	[SerializeField] private Text[] comboInfo;
	
	[SerializeField] private Text[] rageMeterAmount;
	
	[SerializeField] private Text[] rageInfo;
	
	[SerializeField] private Text[] rageDuration;
	
	[SerializeField] private Text roundCountdownInfo;
	
	int playerMaxHealth;

	[SerializeField] VideoPlayer videoPlayer;

	public void PlayCutscene(VideoClip clip)
	{
		EnablePlayerControls(false);
		PauseGame();
		videoPlayer.clip = clip;
		videoPlayer.Play();
	}

    void Start()
    {
		gamePaused = false;
		
		players[0] = GameObject.Find("Player");
		players[1] = GameObject.Find("TestDummy");
		
		GetPlayerMaxHealth();
		SetPlayersSpawn();
		RoundCountdown();
    }
	
	void OnEnable()
	{
		PlayerStats.OnDeath += RoundEnd;
		RageMode.OnCutscene += PlayCutscene;
		videoPlayer.loopPointReached += EndCutscene;
	}
	
	void OnDisable()
	{
		PlayerStats.OnDeath -= RoundEnd;
		RageMode.OnCutscene -= PlayCutscene;
		videoPlayer.loopPointReached -= EndCutscene;
	}
	
	void Update()
    {
		if(!roundStarted && Input.GetKeyDown(skipRoundCountdownButton))
		{
			RoundStart();
		}
		else if (roundStarted && Input.GetKeyDown(skipRoundCountdownButton))
		{
			RoundRestart();
		}
		
		try 
		{
			for(int i = 0; i < 2; i++)
			{
				hpBars[i].text = players[i].GetComponent<PlayerStats>().health.ToString(); if(players[i].GetComponent<PlayerStats>().health <= 0) { hpBars[i].text = "0"; }
				blockInfo[i].text = players[i].GetComponent<PlayerStats>().blocking.ToString();
				
				dodges[i].text = players[i].GetComponent<CharacterMovement2D>().GetCurrentDodgeAmount().ToString();
				
				abilities1[i].text = players[i].GetComponent<CharacterAbilities2D>().ability1.GetAbilityCooldown().ToString();
				abilities2[i].text = players[i].GetComponent<CharacterAbilities2D>().ability2.GetAbilityCooldown().ToString();
				abilities3[i].text = players[i].GetComponent<CharacterAbilities2D>().ability3.GetAbilityCooldown().ToString();
			
				comboInfo[i].text = players[i].GetComponent<CharacterCombat2D>().GetComboInfo().ToString();
				//TODO: add the same functionality for combo duration
			
				rageMeterAmount[i].text = players[i].GetComponent<CharacterAbilities2D>().rageMode.GetRageMeterInfo().ToString();
				rageInfo[i].text = players[i].GetComponent<PlayerStats>().rageActive.ToString();
				rageDuration[i].text = players[i].GetComponent<CharacterAbilities2D>().rageMode.GetRageDuration().ToString();
				
			}
		}
		catch(Exception e)
		{
			//just to see other Debug Logs
		}

		if(!roundStarted)
		{
			if(currentCountdown > 0)
			{
				currentCountdown -= Time.fixedDeltaTime;
				roundCountdownInfo.text = currentCountdown.ToString();
			}
			else if(currentCountdown < 0)
			{
				currentCountdown = 0;
				RoundStart();
			}
		}	
	}
	
	void SetPlayersSpawn()
	{
		foreach(GameObject player in players)
		{				
			if(player.GetComponent<PlayerStats>().GetPlayerId() == 1)
			{
				player.transform.position = player1SpawnPoint;
				
				Debug.Log("Player 1 ready." + " ID: " + player.GetComponent<PlayerStats>().GetPlayerId());
			}
			else
			{
				if(player.GetComponent<CharacterMovement2D>())
				{
					player.GetComponent<CharacterMovement2D>().TestFlip();
				}
				
				player.transform.position = player2SpawnPoint;
				Debug.Log("Player 2 ready." + " ID: " + player.GetComponent<PlayerStats>().GetPlayerId());
			}
		}
	}
	
	void GetPlayerMaxHealth()
	{
		foreach(GameObject player in players)
		{
			playerMaxHealth += player.GetComponent<PlayerStats>().health;
		}
		
		playerMaxHealth /= players.Length;
	}
	
	void RoundCountdown()
	{
		roundStarted = false;
		
		EnablePlayerControls(false);
		
		currentCountdown = roundCountdown;
	}
	
	void RoundStart()
	{
		roundStarted = true;
		
		EnablePlayerControls(true);
		roundCountdownInfo.text = "Start!";
		
		Invoke("ClearRoundInfo", 0.5f);
	}
	
	void RoundEnd()
	{
		EnablePlayerControls(false);
		roundCountdownInfo.text = "Round ended.";
	}
	
	void RoundRestart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		if(gamePaused)
		{
			ResumeGame();
		}
	}
	
	void PauseGame()
	{
		Time.timeScale = 0;
		gamePaused = true;
	}

	void ResumeGame()
	{
		Time.timeScale = 1;
		gamePaused = false;
	}

	void EndCutscene(VideoPlayer source)
	{
		source = videoPlayer;
		source.Stop();
		ResumeGame();
		EnablePlayerControls(true);
	}

	void EnablePlayerControls(bool setting)
	{
		foreach(GameObject player in players)
		{
			if(player.GetComponent<PlayerControls>())
			{
				player.GetComponent<PlayerControls>().enabled = setting;
			}
		}
	}
	
	void ClearRoundInfo()
	{
		roundCountdownInfo.text = "";
	}
}
