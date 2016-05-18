using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource source;
	public AudioClip exorcistLightAttack, exorcistHeavyAttack, exorcistHeal, exorcistPass, exorcistVictory;
	public AudioClip summonerLightAttack, summonerHeavyAttack, summonerDraw, summonerPass, summonerSummoning, summonerVictory;
	public AudioClip gameOverSound;//for test...because I want to

	public void playGameOverSound()
	{
		source.PlayOneShot (gameOverSound, 0.3f);

	}


	public void playLightAttackSound(string player)
	{
		if (player == "exorcist")
		{
			source.PlayOneShot (exorcistLightAttack);
		} 
		else if (player == "summoner")
		{
			source.PlayOneShot (summonerLightAttack);
		}
		else//error, play nothing
		{
			Debug.Log ("Invalid string in parameter");
			return;
		}
	}

	public void playHeavyAttackSound(string player)
	{
		if (player == "exorcist")
		{
			source.PlayOneShot (exorcistHeavyAttack);
		} 
		else if (player == "summoner")
		{
			source.PlayOneShot (summonerHeavyAttack);
		}
		else//error, play nothing
		{
			Debug.Log ("Invalid string in parameter");
			return;
		}

	}
	public void playHealDrawSound(string player)
	{
		if (player == "exorcist")
		{
			source.PlayOneShot (exorcistHeal);
		}
		else if (player == "summoner")
		{
			source.PlayOneShot (summonerDraw);
		}
		else//error, play nothing
		{
			Debug.Log ("Invalid string in parameter");
			return;
		}
	}

	public void playSummonSound()
	{
		//plays summoner's summoning sound
		source.PlayOneShot(summonerSummoning);
	}

	public void playPassSound(string player)
	{
		if (player == "exorcist")
		{
			source.PlayOneShot (exorcistPass);
		}
		else if (player == "summoner")
		{
			source.PlayOneShot (summonerPass);
		}
		else//error, play nothing
		{
			Debug.Log ("Invalid string in parameter");
			return;
		}

	}

	public void playVictorySound(string player)
	{
		if (player == "exorcist")
		{
			source.PlayOneShot (exorcistVictory);
		} 
		else if (player == "summoner")
		{
			source.PlayOneShot (summonerVictory);
		}
		else//error, play nothing
		{
			Debug.Log ("Invalid string in parameter");
			return;
		}

	}




	// Use this for initialization
	void Start () {
		instance = this;
		source = Camera.main.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
