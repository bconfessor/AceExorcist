using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour {


	public static ButtonManager instance;

	public GameObject summonButton, attackButton, drawButton, sacrificeButton, passButton, endTurnButton;//for now, they will be used for both players


	public void Awake()
	{
		instance=this;
	}

	public void pressedDrawButton()
	{
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			AceExorcistGame.instance.exorcistHand.GetComponent<Hand> ().addExorcistCard ();
		} 
		else
			AceExorcistGame.instance.summonerHand.GetComponent<Hand> ().addSummonerCard ();

		//once draw is done, it becomes disabled until next turn
		cardDrawn();
	}
	public void pressedAttackButton()
	{
		//if no cards were selected, give a message to select
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count == 0 && AceExorcistGame.instance.summonerHand.getToggledCards ().Count == 0)
		{
			Debug.Log ("No cards were selected. Select card(s) before choosing an action");
			return;
		}

		//gets toggled cards to check if they form a valid attack
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			//call exorcist attack with exorcist toggled cards
			int damageToSummoner = AceExorcistGame.instance.exorcistAttacked(AceExorcistGame.instance.exorcistHand.getToggledCards());
			//if damage = -1, invalid hand
			if (damageToSummoner < 0)
			{
				Debug.Log ("Invalid hand, attack must be a flush");
			}
			else//valid attack
			{
				Debug.Log ("Exorcist Attacked!");
				//for now, take damage directly to summoner health
				AceExorcistGame.instance.currentSummonerHP-=GameObject.Find ("ExorcistHand").GetComponent<Hand> ().getCardsPower();
				if (AceExorcistGame.instance.currentSummonerHP < 0)
					AceExorcistGame.instance.currentSummonerHP = 0;
				gameObject.GetComponent<UIManager> ().updateHealthUI ();
				AceExorcistGame.instance.checkExorcistVictory ();
				actionCompleted ();
			}
		}
		else//summoner turn, call summoner attack
		{
			//TODO:Summoner attack method still incomplete; complete it
			int damageToExorcist = AceExorcistGame.instance.summonerAttacked(AceExorcistGame.instance.summonerHand.getToggledCards());
			//if hand is invalid, damage = -1
			if (damageToExorcist < 0)
			{
				Debug.Log ("Invalid hand, attack must be a straight.");
			}
			else
			{
				Debug.Log ("Summoner attacked!");
				AceExorcistGame.instance.currentExorcistHP -= GameObject.Find ("SummonerHand").GetComponent<Hand> ().getCardsPower ();
				if (AceExorcistGame.instance.currentExorcistHP < 0)
					AceExorcistGame.instance.currentExorcistHP = 0;
				gameObject.GetComponent<UIManager> ().updateHealthUI ();
				AceExorcistGame.instance.checkSummonerVictory ();
				actionCompleted ();
			}
		}

	}

	public void pressedSummonButton()
	{
		//Summoner only, only works if there is only one (valid) toggled card 
		List<GameObject> summonerCard = AceExorcistGame.instance.summonerHand.getToggledCards();
		if (summonerCard.Count == 0)
		{
			Debug.Log ("No cards selected. Select a valid card to summon.");
			return;
		}
		if (AceExorcistGame.instance.summonedRitual (summonerCard))//if it can summon, display this
		{
			Debug.Log ("Card Summoned!");
			actionCompleted ();
			AceExorcistGame.instance.checkSummonerVictory ();//summoner might have won by summoning three cards

		}
		else
		{
			Debug.Log ("Cannot use this hand for summon");
		}

	}

	public void pressedPassButton()
	{
		//just draws another card and ends turn
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			AceExorcistGame.instance.exorcistHand.GetComponent<Hand> ().addExorcistCard ();
		} 
		else
			AceExorcistGame.instance.summonerHand.GetComponent<Hand> ().addSummonerCard ();

		//after this, has same effect as pressing the End Turn button, so I'll just go ahead and call that to make my life easier
		pressedEndTurnButton();
	}

	public void pressedSacrificeButton()
	{
		//either sacrifices cards to get more cards(in summoner's case) or to heal damage(in exorcist's case)
		//if no cards were selected, give a message to select
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count == 0 && AceExorcistGame.instance.summonerHand.getToggledCards ().Count == 0)
		{
			Debug.Log ("No cards were selected. Select card(s) before choosing an action");
			return;
		}
		//TODO
		GameObject.Find ("ExorcistHand").GetComponent<Hand> ().removeToggledCards ();
		actionCompleted ();
	}

	public void pressedEndTurnButton()
	{
		//change turns from one player to the other
		AceExorcistGame.instance.currentTurnEnded();
		//end turn = beginning of the other's turn
		turnStarted();

	}

	//================================================================= BUTTON STATE METHODS =============================================================================

	public void turnStarted()
	{
		//when a turn starts, only the draw button is enabled
		deactivateAllButtons();
		drawButton.GetComponent<Button> ().interactable = true;
		//also, once it starts, change buttons names, depending on current player
		changeTexts();
	}

	public void cardDrawn()
	{
		//after player draws a card, draw button becomes inactive and other activate(except end turn)
		reactivateAllButtons();
		endTurnButton.GetComponent<Button> ().interactable = false;
		drawButton.GetComponent<Button> ().interactable = false;


	}

	public void actionCompleted()
	{
		//after an action has been successfully completed, disable all buttons other than "end turn", so the player won't do more than one action per turn
		deactivateAllButtons();
		endTurnButton.GetComponent<Button> ().interactable = true;
	}

	public void deactivateAllButtons()
	{
		drawButton.GetComponent<Button>().interactable=false;
		attackButton.GetComponent<Button> ().interactable = false;
		passButton.GetComponent<Button> ().interactable = false;
		sacrificeButton.GetComponent<Button> ().interactable = false;
		summonButton.GetComponent<Button> ().interactable = false;
		endTurnButton.GetComponent<Button> ().interactable = false;

	}

	public void reactivateAllButtons()
	{
		drawButton.GetComponent<Button>().interactable=true;
		attackButton.GetComponent<Button> ().interactable = true;
		sacrificeButton.GetComponent<Button> ().interactable = true;
		passButton.GetComponent<Button> ().interactable = true;
		endTurnButton.GetComponent<Button> ().interactable = true;
		//only reactivates this one if it's summoner's turn
		if (!AceExorcistGame.instance.isExorcistTurn)
		{
			//activate summon button as well
			summonButton.GetComponent<Button>().interactable=true;
		}
	}

	public void changeTexts()
	{
		//Change button texts to fit the current player
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			sacrificeButton.GetComponentInChildren<Text> ().text = "Sacrifice Heal";
		}
		else
		{
			sacrificeButton.GetComponentInChildren<Text> ().text = "Sacrifice Draw";
		}
	}

}
