using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cards.Collections;
using System.Collections.Generic;

public class Player_Turn : MonoBehaviour {


	//public List<GameObject> cardsChosen;//cards chosen by the player; get them through the clicking script
	public Hand hand;//stores the player hand to be used

	void OnEnable()
	{

		UIManager.instance.displayNewText ("It's the exorcist's turn.\nSelect the cards you want to use");
		hand.flipHandUp ();
		//Debug.Log("It's the exorcist's turn.");
		//Debug.Log("Select the cards you want to use.");
	}

	void OnDisable()
	{
		hand.flipHandDown ();
		endTurn ();
	}

	// Use this for initialization
	void Start () {
		//cardsChosen = new List<GameObject>();
		hand = AceExorcistGame.instance.exorcistHand;
		hand.flipHandUp ();
	}



	void endTurn()//ends the player turn; modifies the exorcistTurn boolean and destroys this component
	{
		AceExorcistGame.instance.isExorcistTurn=false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
