using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour {

	//TODO: Make Yes/No button to work with damage mitigation

	public static ButtonManager instance;

	public GameObject summonButton, attackButton, attackSummonZoneButton, drawButton, sacrificeButton, passButton, endTurnButton;//most will be used for both players
	public GameObject resetButton;//restarts game

	public GameObject yesButton, noButton, mitigateButton, cancelMitigationButton;//only show up when exorcist is given a choice about mitigating an attack

	public void Awake()
	{
		instance=this;
	}

	//======================================================== NORMAL BUTTON METHODS =======================================================================

	public void pressedDrawButton()
	{
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			AceExorcistGame.instance.exorcistHand.GetComponent<Hand> ().addExorcistCard ();
		} 
		else
			AceExorcistGame.instance.summonerHand.GetComponent<Hand> ().addSummonerCard ();

		//update amount of cards in both decks
		UIManager.instance.Invoke("updateCardsLeftUI",Time.deltaTime);
		//once draw is done, it becomes disabled until next turn
		cardDrawn();
	}

	public void pressedAttackButton()
	{
		//if no cards were selected, give a message to select
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count == 0 && AceExorcistGame.instance.summonerHand.getToggledCards ().Count == 0)
		{
			UIManager.instance.displayNewText ("No cards were selected. Select card(s) before choosing an action");
			return;
		}

		//if it's exorcist turn and there's a summon in the field, a direct attack is not possible
		if (AceExorcistGame.instance.isExorcistTurn && AceExorcistGame.instance.summonZone.hand.Count > 0)
		{
			UIManager.instance.displayNewText ("Cannot do a direct attack; must attack summons first.");
			return;
		}

		//gets toggled cards to check if they form a valid attack
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			AceExorcistGame.instance.callExorcistAttack ();//exorcist can simply attack
		}

		else//summoner turn, call summoner attack, block all other buttons since exorcist might try to mitigate
		{
			AceExorcistGame.instance.callSummonerAttack ();
		}

	}

	public void pressedAttackSummonZoneButton()
	{
		//first, player must choose a hand; if hand is empty/too small or not a flush, does nothing
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count < 2 || !AceExorcistGame.instance.handIsFlush(AceExorcistGame.instance.exorcistHand.getToggledCards()))
		{
			UIManager.instance.displayNewText ("Hand is not appropriate for attacking a summoned monster! Choose a proper hand before attacking.");
			return;
		}
		else
		{
			//hand is good enough for an attack(now to choose the card to be attacked)
			AceExorcistGame.instance.canToggleSummonCards=true;
			deactivateAllButtons();//makes the summonpanel appear as well
			UIManager.instance.displaySummonAttackPanel();

		}
	}

	public void pressedConfirmSummonAttack()
	{
		//TODO: works just by checking if summon was successful
		//can only attack ONE summon, if more than one is selected, complains
		if (AceExorcistGame.instance.summonZone.getToggledCards ().Count != 1)
		{
			UIManager.instance.displayNewText ("Can only attack one summon card, invalid choice.");
			return;
		}
		if (AceExorcistGame.instance.exorcistAttackedSummonCard (AceExorcistGame.instance.exorcistHand.getToggledCards (), AceExorcistGame.instance.summonZone.getToggledCards () [0]))
		{
			//attack was successfull, restart buttons
			AceExorcistGame.instance.canToggleSummonCards = false;

			UIManager.instance.hideSummonAttackPanel ();
			actionCompleted ();


		}


	}

	public void pressedCancelSummonAttackButton()
	{
		//cancels it, makes buttons work again, makes summon panel dissapear
		//cancel has the same effect as drawing card, button online-wise
		UIManager.instance.hideSummonAttackPanel();
		cardDrawn ();
		AceExorcistGame.instance.canToggleSummonCards = false;

	}

	public void pressedSummonButton()
	{
		//Summoner only, only works if there is only one (valid) toggled card 
		List<GameObject> summonerCard = AceExorcistGame.instance.summonerHand.getToggledCards();
		if (summonerCard.Count == 0)
		{
			UIManager.instance.displayNewText ("No cards selected. Select a valid card to summon.");
			return;
		}
		if (AceExorcistGame.instance.summonedRitual (summonerCard))//if it can summon, display this
		{
			UIManager.instance.displayNewText ("Card Summoned!");
			actionCompleted ();
			AceExorcistGame.instance.checkSummonerVictory ();//summoner might have won by summoning three cards

		}
		else
		{
			UIManager.instance.displayNewText ("Cannot use this hand for summon");
		}

	}

	public void pressedPassButton()
	{
		//just draws another card and ends turn
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			AceExorcistGame.instance.exorcistHand.GetComponent<Hand> ().addExorcistCard ();
			SoundManager.instance.playPassSound ("exorcist");
		}
		else
		{
			AceExorcistGame.instance.summonerHand.GetComponent<Hand> ().addSummonerCard ();
			SoundManager.instance.playPassSound ("summoner");
		}
		//after this, has same effect as pressing the End Turn button, so I'll just go ahead and call that to make my life easier
		pressedEndTurnButton();
	}

	public void pressedSacrificeButton()
	{
		//either sacrifices cards to get more cards(in summoner's case) or to heal damage(in exorcist's case)
		//if no cards were selected, give a message to select
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count == 0 && AceExorcistGame.instance.summonerHand.getToggledCards ().Count == 0)
		{
			UIManager.instance.displayNewText ("No cards were selected. Select card(s) before choosing an action");
			return;
		} else if (AceExorcistGame.instance.isExorcistTurn && AceExorcistGame.instance.exorcistHealed (AceExorcistGame.instance.exorcistHand.getToggledCards ()))
		{
			//successfully healed with the cards chosen; give feedback to player that it worked, move on
			UIManager.instance.displayNewText ("Exorcist sacrificed some cards to heal themselves!");
			UIManager.instance.updateHealthUI ();
			actionCompleted ();
		} else if (!AceExorcistGame.instance.isExorcistTurn && AceExorcistGame.instance.summonerSacrificeDrew (AceExorcistGame.instance.summonerHand.getToggledCards ()))
		{
			//successfully sacrificed summoner cards to draw some more; give feedback to player, move on
			UIManager.instance.displayNewText ("Summoner sacrificed cards to draw more!"); 
			actionCompleted ();
		} 
		else
			UIManager.instance.displayNewText ("Cannot use this hand for a sacrifice!");

	}

	public void pressedEndTurnButton()
	{
		//change turns from one player to the other
		AceExorcistGame.instance.currentTurnEnded();
		//end turn = beginning of the other's turn
		turnStarted();
	}

	public void pressedResetButton()
	{
		//TODO:resets game: resets all flags, decks and etc(easier to just reload scene?)

	}

	//========================================================== MITIGATION BUTTON METHODS ===============================================================

	public void pressedYesButton()
	{
		FlowManager.instance.answerChosen = true;
		//allow exorcist to choose cards to mitigate, change yes/no panel to mitigate/cancel 
		UIManager.instance.hideChoicePanel();
		UIManager.instance.displayMitigationPanel ();

		//turns on mitigation state so exorcist cards can be toggled
		AceExorcistGame.instance.exorcistMitigating=true;

		//starts coroutine that waits for exorcist to give mitigation cards
		FlowManager.instance.StartCoroutine ("doExorcistMitigation");

	}

	public void pressedNoButton()
	{
		FlowManager.instance.answerChosen = false;
		UIManager.instance.displayNewText ("Exorcist took the blunt of the attack!");
		AceExorcistGame.instance.exorcistDamaged();
		//exorcist was attacked, so take out the summoner's used cards
		AceExorcistGame.instance.summonerHand.removeToggledCards();
		UIManager.instance.hideChoicePanel ();
		//go ahead with the full summoner attack
		actionCompleted();
	}

	public void pressedMitigateButton()
	{
		//try to mitigate attack with hand chosen(if there is a chosen hand)
		if (AceExorcistGame.instance.exorcistHand.getToggledCards ().Count > 0)
		{
			FlowManager.instance.cardsChosen = true;
		}
	}

	public void pressedCancelButton()
	{
		//go ahead with the full summoner attack
		UIManager.instance.displayNewText ("Exorcist took the blunt of the attack!");
		AceExorcistGame.instance.exorcistDamaged();
		//exorcist was attacked, so take out the summoner's used cards
		AceExorcistGame.instance.summonerHand.removeToggledCards();
		UIManager.instance.hideMitigationPanel();
		AceExorcistGame.instance.exorcistMitigating = false;//gets out of the state
		actionCompleted ();
	}

	//================================================================= BUTTON STATE METHODS =============================================================================

	public void turnStarted()
	{
		//if it's the exorcist's turn, summon button shouldn't show up and attack summon zone should
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			summonButton.SetActive (false);
			attackSummonZoneButton.SetActive (true);
		}
		else
		{
			summonButton.SetActive (true);
			attackSummonZoneButton.SetActive (false);
		}

		//when a turn starts, only the draw button is enabled
		deactivateAllButtons();
		drawButton.GetComponent<Button> ().interactable = true;
		//also, once it starts, change buttons names, depending on current player
		changeTexts();
		AceExorcistGame.instance.cannotClickCards ();//only after card is drawn
	}

	public void cardDrawn()
	{
		//after player draws a card, draw button becomes inactive and other activate(except end turn)
		reactivateAllButtons();
		endTurnButton.GetComponent<Button> ().interactable = false;
		drawButton.GetComponent<Button> ().interactable = false;

		//now player can select cards
		AceExorcistGame.instance.canClickCards();

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
		attackSummonZoneButton.GetComponent<Button> ().interactable = false;
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
		else//is exorcist's turn, activates summon zone attack button
			attackSummonZoneButton.GetComponent<Button> ().interactable = true;
		
	}

	public void activateChoiceButtons()
	{
		yesButton.GetComponent<Button> ().interactable = true;
		noButton.GetComponent<Button> ().interactable = true;
	}

	public void deactivateChoiceButtons()
	{
		yesButton.GetComponent<Button> ().interactable = false;
		noButton.GetComponent<Button> ().interactable = false;
	}

	public void activateMitigationButtons()
	{
		mitigateButton.GetComponent<Button> ().interactable = true;
		cancelMitigationButton.GetComponent<Button> ().interactable = true;
	}

	public void deactivateMitigationButtons()
	{
		mitigateButton.GetComponent<Button> ().interactable = false;
		cancelMitigationButton.GetComponent<Button> ().interactable = false;
	}

	public void changeTexts()
	{
		//Change button texts to fit the current player
		if (AceExorcistGame.instance.isExorcistTurn)
		{
			sacrificeButton.GetComponentInChildren<Text> ().text = "Sacrifice Heal";
			attackButton.GetComponentInChildren<Text>().text = "Direct Attack";
		}
		else
		{
			sacrificeButton.GetComponentInChildren<Text> ().text = "Sacrifice Draw";
			attackButton.GetComponentInChildren<Text> ().text = "Attack";
		}
	}

}
