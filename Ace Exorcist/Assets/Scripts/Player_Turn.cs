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

		Debug.Log("It's the exorcist's turn.");
		Debug.Log("Select the cards you want to use.");
		//Debug.Log("Once they're chosen, press A to attack the summoner's deck, H to heal or P to Pass");

	}

	void OnDisable()
	{
		endTurn ();
	}

	// Use this for initialization
	void Start () {
		//cardsChosen = new List<GameObject>();
		hand = AceExorcistGame.instance.exorcistHand;

	}



	void endTurn()//ends the player turn; modifies the exorcistTurn boolean and destroys this component
	{
		AceExorcistGame.instance.isExorcistTurn=false;
	}

	// Update is called once per frame
	void Update () {
		//waits for the player to input a keyboard input

		//TODO: How to get the list of cards to send it to the proper functions?

		/*


		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			AceExorcistGame.instance.toggleCard(1, ref cardsChosen);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			AceExorcistGame.instance.toggleCard(2, ref cardsChosen);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			AceExorcistGame.instance.toggleCard(3, ref cardsChosen);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			AceExorcistGame.instance.toggleCard(4, ref cardsChosen);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			AceExorcistGame.instance.toggleCard(5, ref cardsChosen);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			AceExorcistGame.instance.toggleCard(6, ref cardsChosen);
		}


		//Gets the possible action values given by the player


		else if(Input.GetKeyDown(KeyCode.A))
		{
			//should get the cards and check if an attack is possible
			if(cardsChosen.Count<1)//no cards chosen for attack
			{
				Debug.Log("Can't attack; no cards chosen");
			}
			else
			{
				//checks for the validity of the attack
				if(AceExorcistGame.instance.doExorcistAttack(cardsChosen))//if the attack succeeds, this will return true
					endTurn();//from here, it can just finish the turn
				else
				{
					Debug.Log("Couldn't attack with the chosen cards");
				}
			}
		}
		else if(Input.GetKeyDown(KeyCode.H))
		{
			//should get the cards and see if it's a pair and if you can heal with them
			if(cardsChosen.Count<1)
			{
				Debug.Log("Can't attack; no cards chosen");
			}
			else
			{
				//checks for the validity of the healing
				if(AceExorcistGame.instance.doExorcistHeal(cardsChosen))
				{
					//from here, ends the player's turn
					endTurn();
				}
				else
				{
					Debug.Log("Couldn't heal with the chosen cards");
				}
			}
		}

		else if(Input.GetKeyDown(KeyCode.P))
		{
			//doesn't need to wait for card input, passes automatically
			AceExorcistGame.instance.passTurn();
			endTurn();
		}
		*/

	}
}
