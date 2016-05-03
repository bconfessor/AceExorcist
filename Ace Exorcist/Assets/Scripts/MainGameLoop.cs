using UnityEngine;
using System.Collections;

public class MainGameLoop : MonoBehaviour 
{
	//This is the main file, since it has the main game loop. Every important comment or TODO must be placed here
	//things here will be listed in the order which I believe is the best for a decent flow of construction of the mechanics 

	//TODO: fix the click mechanic so that it clicks on a card and just on that(THINK I DID IT)
	//Next up: Toggling the cards up and down with a click(Done, easier than I thought)
	//Next up: make (test)button to remove toggled cards and rearrange deck
	//(button can later be converted into an action button, such as 'attack' or 'sacrifice')(KIND OF DONE, WORKS BUT DOES NOT REARRANGE CARDS YET)

	//Next up: Enhance the UI, create a script just for it, get better fonts, display and update HP of exorcist and summoner in it
	//Next up: Make buttons interactable and uninteractable, add couple more buttons
	//Next up: Start fixing the main game rules in AceExorcistGame.cs




	public GameObject exorcist, summoner;

	// Use this for initialization
	void Start () 
	{
		AceExorcistGame.instance.isExorcistTurn=false;//the game always starts with the summoner. Deal with it
	}
	
	// Update is called once per frame
	void Update () {
		if(AceExorcistGame.instance.isExorcistTurn)//changed to exorcist's turn, activate their turn script
		{
			if(!exorcist.GetComponent<Player_Turn>().enabled)//exorcist's turn has started, must activate its script and deactivate summoner's
			{
				exorcist.GetComponent<Player_Turn> ().enabled = true;
				summoner.GetComponent<Enemy_Turn> ().enabled = false;
			}
		}
		else//it's the summoner's turn
		{
			if(!summoner.GetComponent<Enemy_Turn>().enabled)
			{
				summoner.GetComponent<Enemy_Turn>().enabled = true;//summoner's turn has started
				exorcist.GetComponent<Player_Turn>().enabled = false;
			}
		}
	}
}
