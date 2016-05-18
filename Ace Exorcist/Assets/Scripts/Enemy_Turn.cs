using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cards.Collections;
using System.Collections.Generic;

public class Enemy_Turn : MonoBehaviour {
	
	
	//List<GameObject> cardsChosen;//cards chosen by the player; get them through the clicking script
	public Hand hand;//stores the player hand to be used

	void OnEnable()
	{
		UIManager.instance.displayNewText ("It's the summoner's turn.\nSelect the cards you want to use");

		//Debug.Log ("It's the Summoner's turn");
		//Debug.Log("Select the cards you want to use.");
	}
	void OnDisable()
	{
		endTurn ();
	}

	// Use this for initialization
	void Start () {
		//cardsChosen = new List<GameObject>();
		UIManager.instance.displayNewText ("It's the summoner's turn.\nSelect the cards you want to use");

		ButtonManager.instance.turnStarted ();
		hand = AceExorcistGame.instance.summonerHand;
	}
	
	void endTurn()//ends the player turn; modifies the exorcistTurn boolean and destroys this component
	{
		AceExorcistGame.instance.isExorcistTurn=true;

	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
