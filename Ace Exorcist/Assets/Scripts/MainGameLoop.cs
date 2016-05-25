using UnityEngine;
using System.Collections;

public class MainGameLoop : MonoBehaviour 
{
	//This is the main file, since it has the main game loop. Every important comment or TODO must be placed here
	//things here will be listed in the order which I believe is the best for a decent flow of construction of the mechanics 

	//TODO: fix the click mechanic so that it clicks on a card and just on that(THINK I DID IT)
	//TODO: FIX Sacrifice draw/heal text AND method(just tried to heal player and it didn't work)
	//Next up: Toggling the cards up and down with a click(Done, easier than I thought)

	//Next up: Enhance the UI, create a script just for it, display and update HP of exorcist and summoner in it(DONE)
	//Next up: Make UI prettier, get better fonts for it
	//Next up: Make buttons interactable and uninteractable, add couple more buttons(Done)
	//Next up: Start fixing the main game rules in AceExorcistGame.cs(STARTED)
	//Next up: Start creating buttons that receive all the necessary methods from AceExorcistGame(Started)
	//Next up: Implement Summon Zone, make summoner be able to place cards there(DONE)
	//Next up: Create new canvas with Yes/No buttons to manage decisions like damage mitigation(done)
	//Next up: FIX DAMAGE MITIGATION COROUTINE, SEEMS TO BE ENTERING IN ENDLESS LOOP(Done?)
	//Next up: Make more text for mitigation process
	//Next up: Enhance UI, put number of cards left in deck(DONE)
	//Next up: Enhance Cards.collections script, make method to look at next card from deck but not take it out(DONE)
	//Next up: Build animation for card destruction(DONE)
	//Next up: Build deck-destruction mechanism from exorcist attack
	//Next up: Build a coroutine to work for the exorcist attack's deck-destruction mechanism, destroying one card per second
	//Next up: Make another button, this time to attacḱ the summon zone

	//Idea: divide turns into phases, like Yu-gi-oh(draw phase, action phase, end turn phase)


	public GameObject exorcist, summoner;

	// Use this for initialization
	void Start () 
	{
		AceExorcistGame.instance.isExorcistTurn=false;//the game always starts with the summoner. Deal with it
	}
	
	// Update is called once per frame
	void Update () {
		if (AceExorcistGame.instance.isExorcistTurn && AceExorcistGame.instance.ExorcistIsAlive())//changed to exorcist's turn, activate their turn script
		{
			if (!exorcist.GetComponent<Player_Turn> ().enabled)//exorcist's turn has started, must activate its script and deactivate summoner's
			{
				exorcist.GetComponent<Player_Turn> ().enabled = true;
				summoner.GetComponent<Enemy_Turn> ().enabled = false;
			}
		} else if (!AceExorcistGame.instance.exorcistAlive)//exorcist is dead
		{
			Debug.Log ("Exorcist is dead! Game Over");

		} else if (!AceExorcistGame.instance.isExorcistTurn && AceExorcistGame.instance.SummonerIsAlive())//summoner's turn
		{
			if (!summoner.GetComponent<Enemy_Turn> ().enabled)
			{
				summoner.GetComponent<Enemy_Turn> ().enabled = true;//summoner's turn has started
				exorcist.GetComponent<Player_Turn> ().enabled = false;
			}
		}
		else if (!AceExorcistGame.instance.summonerAlive)//summoner got defeated
		{
			Debug.Log ("Summoner is defeated, you win!");

		}
	}
}
