﻿using UnityEngine;
using System.Collections;
using Cards.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	/*To be put in the Hand GO of the exorcist and the summoner(and possibly the summonZone). Holds the cards that are in each hand.
	 * Also holds the position of each card.
	 * 
	*/



	public GameObject CardGO;


	public List<GameObject> hand;//saves the hand being used by player or summoner(can also be possibly used by the summon Zone)
	public DeckScript deckScript;//holds the deckscript associated with this hand 
	public int upperLimit;//limits the number of cards in hand
	public int currentCardNumber;//number of cards currently in hand
	public bool deckDepleted = false;



	public float exorcistX;//saves the x position the exorcist's hand will be displayed
	public float summonerX;//same thing for summoner
	public float exorcistY;//saves the height the exorcist's hand will be displayed
	public float summonerY;//same thing for summoner
	public float summonZoneX, summonZoneY;

	float cardWidth = 30.0f;//hard coded value for now, used to put cards apart on the table


	//===================================== card adding methods =============================================





	public void addCard(GameObject c, bool exorcistHand)
	{
		//saves the GO's of each card
		hand.Add(c);
		currentCardNumber++;
		//UIManager.instance.displayNewText("Adding "+c.GetComponent<CardModel>().cardValue+" of "+c.GetComponent<CardModel>().cardSuit);
		//once card is added, we must make its parent be the correct Hand
		if (exorcistHand) {
			c.transform.parent = AceExorcistGame.instance.exorcistHandGO.transform;//make this card a child of the exorcist hand
		}
		else
		{
			c.transform.parent = AceExorcistGame.instance.summonerHandGO.transform;//make this card a child of the summoner hand
		}

		displayCards ();
		//does the card flip animation
		c.GetComponent<CardAnimations> ().FlipCard (c.GetComponent<CardModel> ().cardBack, c.GetComponent<CardModel> ().cardFace);
	}

	public void summonCardToSummonZone(GameObject c)
	{
		//pretty much the same thing as addCard, but with some mods
		//takes card off summoner and puts in summon zone, i.e., changes parent
		hand.Add (c);
		currentCardNumber++;
		//removes it from summoner hand
		AceExorcistGame.instance.summonerHand.removeCardFromHand(c);
		c.transform.parent = AceExorcistGame.instance.summonZoneGO.transform;
		c.GetComponent<CardAnimations> ().FlipCard (c.GetComponent<CardModel> ().cardBack, c.GetComponent<CardModel> ().cardFace);
		Invoke ("displayCards", 1.2f);
	}


	public void addExorcistCard()
	{
		//to be used with button, since that only accepts functions with one parameter

		//CAN ONLY HAPPEN IF THERE ARE CARDS TO BE DRAWN

		if (currentCardNumber < upperLimit && deckScript.deck.getRemainingCards () > 0)
		{
			GameObject card = Instantiate (CardGO);
			card.GetComponent<CardModel> ().loadCard (deckScript.deck.TakeCard ());//pulled a card from deck
			//make button turn off somehow
			//GameObject.FindGameObjectWithTag("drawButton").

			//now we add the pulled card to the hand
			addCard (card, true);
		}
		else if (currentCardNumber >= upperLimit)
		{
			UIManager.instance.displayNewText ("You can't draw anymore cards");
		}
		else//remaining cards <=0
		{
			UIManager.instance.displayNewText ("Your deck is over!");
			deckDepleted = true;
		}
	}

	public void addSummonerCard()
	{
		//to be used with button, since that only accepts functions with one parameter
		//easy, huh?
		if (currentCardNumber < upperLimit && deckScript.deck.getRemainingCards () > 0)
		{
			GameObject card = Instantiate (CardGO);
			card.GetComponent<CardModel> ().loadCard (deckScript.deck.TakeCard ());//pulled a card from deck
			//now we add the pulled card to the hand
			addCard (card, false);
		}
		else if (currentCardNumber >= upperLimit)
		{
			UIManager.instance.displayNewText ("You can't draw anymore cards");
		}
		else//remaining cards <=0
		{
			UIManager.instance.displayNewText ("Your deck is over!");
			deckDepleted = true;
		}
	}


	//===================================== card removal methods =============================================


	public void removeToggledCards()
	{
		//pretty much what it says on the name
		foreach (Transform t in transform)//gets each card
		{
			if (t.GetComponent<CardModel> ().toggled)
			{
				//UIManager.instance.displayNewText.Log (t.GetComponent<CardModel> ().cardValue + " of " + t.GetComponent<CardModel> ().cardSuit + " is toggled");
				removeCard (t.gameObject);
			}
		}
		//after this, rearrange hand
		//need to do this with invoke since, because of Destroy, cards will only get erased on the next update, so calling
		//displayCards before then rearranges them to the same order they already are, and THEN cards dissapear, leaving gaps.
		//so, make a delayed call to the method to give time for the cards to be Destroyed
		Invoke("displayCards", 1.2f);
	}

	public void removeCardFromHand(GameObject c)
	{
		//simply removes card from hand, but does not destroy it
		hand.Remove(c);
		currentCardNumber--;
	}

	public void removeCardAndUpdate(GameObject c)
	{
		//same as "removeCard", but runs a display update right after
		hand.Remove(c);
		c.GetComponent<CardAnimations>().DeleteCard();//deletes card with it's effect
		currentCardNumber--;
		Invoke ("displayCards",1.2f);
	}

	public void removeCard(GameObject c)
	{
		hand.Remove(c);
		c.GetComponent<CardAnimations>().DeleteCard();//deletes card with it's effect
		currentCardNumber--;

	}

	public void emptyHand()
	{
		//empties whole hand
		foreach (Transform t in transform)
		{
			removeCard (t.gameObject);
		}
	}

	//======================================== card use methods ===============================================

	public int getCardsPower()
	{
		//gets the toggled cards, sums their attack power, and returns it
		int sum = 0;//sum of attack power
		foreach (Transform t in transform)
		{
			//gets each card, check if they're toggled
			if (t.GetComponent<CardModel> ().toggled)
			{
				sum += (int)t.GetComponent<CardModel> ().cardValue;

			}

		}
		return sum;

	}


	public List<GameObject> getToggledCards()
	{
		//returns a list of the toggled cards, to be used to validade a move
		List<GameObject> toggledCards = new List<GameObject>();
		foreach (Transform t in transform)
		{
			//looks for(and adds) toggled cards
			if (t.GetComponent<CardModel> ().toggled)
			{
				toggledCards.Add (t.gameObject);
			}
		}
		return toggledCards;

	}

	//========================================= status methods ==========================================================


	public void flipHandUp()
	{
		//flips up all cards in hand, if there are cards in hand
		if (hand.Count > 0)
		{
			StopCoroutine(FlipUp());
			StartCoroutine(FlipUp());
		}
	}

	public void flipHandDown()
	{
		//flips down all cards in hand, if there are cards in hand
		if (hand.Count > 0)
		{
			StopCoroutine (FlipDown ());
			StartCoroutine (FlipDown ());
		}
	}

	public void flipHandUpImmediately()
	{
		for(int i = 0; i <hand.Count;i++)
		{
			//flips each card back up
			CardAnimations cardAnim = hand[i].GetComponent<CardAnimations>();
			cardAnim.StartCoroutine(cardAnim.Flip(hand[i].GetComponent<CardModel>().cardBack, hand[i].GetComponent<CardModel>().cardFace));
		}
	}

	public void flipHandDownImmediately()
	{
		for(int i = 0; i <hand.Count;i++)
		{
			//flips each card back down
			CardAnimations cardAnim = hand[i].GetComponent<CardAnimations>();
			cardAnim.StartCoroutine(cardAnim.Flip(hand[i].GetComponent<CardModel>().cardFace, hand[i].GetComponent<CardModel>().cardBack));
		}
	}


	public void FlipDownUntoggledCards()
	{
		//pretty much what it says on the name
		for(int i = 0; i <hand.Count;i++)
		{
			//flips each untoggled card back down
			if (!hand [i].GetComponent<CardModel> ().toggled)
			{
				CardAnimations cardAnim = hand [i].GetComponent<CardAnimations> ();
				cardAnim.StartCoroutine (cardAnim.Flip (hand [i].GetComponent<CardModel> ().cardFace, hand [i].GetComponent<CardModel> ().cardBack));
			}
		}
	}

	public void FlipUpUntoggledCards()
	{
		//pretty much what it says on the name
		for(int i = 0; i <hand.Count;i++)
		{
			//flips each untoggled card back down
			if (!hand [i].GetComponent<CardModel> ().toggled)
			{
				CardAnimations cardAnim = hand [i].GetComponent<CardAnimations> ();
				cardAnim.StartCoroutine (cardAnim.Flip (hand [i].GetComponent<CardModel> ().cardBack, hand [i].GetComponent<CardModel> ().cardFace));
			}
		}
	}

	IEnumerator FlipUp()
	{
		float delay = 0.2f;

		for(int i = 0; i <hand.Count;i++)
		{
			//flips each card back up
			CardAnimations cardAnim = hand[i].GetComponent<CardAnimations>();
			cardAnim.StartCoroutine(cardAnim.Flip(hand[i].GetComponent<CardModel>().cardBack, hand[i].GetComponent<CardModel>().cardFace));
			yield return new WaitForSeconds (delay);
		}
	}


	IEnumerator FlipDown()
	{
		float delay = 0.2f;

		for(int i = 0; i <hand.Count;i++)
		{
			//flips each card back down
			CardAnimations cardAnim = hand[i].GetComponent<CardAnimations>();
			cardAnim.StartCoroutine(cardAnim.Flip(hand[i].GetComponent<CardModel>().cardFace, hand[i].GetComponent<CardModel>().cardBack));
			yield return new WaitForSeconds (delay);
		}
	}








	public int getHandCount()//returns the amount of cards the hand currently has
	{
		return currentCardNumber;
	}
	
	public void showHand() //lists the hand
	{
		foreach(GameObject c in hand)
		{
			UIManager.instance.displayNewText(c.GetComponent<CardModel>().cardValue + " of " + c.GetComponent<CardModel>().cardSuit);
		}
	}

	public void displayCards()//shows the cards that exist
	{
		
		int counter=0;//to move each card 
		foreach(Transform t in transform)//gets all childs of the hand, i.e., the cards themselves
		{
			if (this.gameObject.tag == "exorcistHand")
				t.position = new Vector2 (exorcistX + (cardWidth * counter), exorcistY);
			else if (this.gameObject.tag == "summonerHand")
				t.position = new Vector2 (summonerX + (cardWidth * counter), summonerY);

			else//summon zone
			{
				t.position = new Vector2 (summonZoneX + (cardWidth * counter), summonZoneY);
			}
			//also need to untoggle them, just to be safe
			t.GetComponent<CardModel> ().toggled = false;
			counter++;

		}
	}

	public void untoggleAllCards()
	{
		foreach (Transform t in transform)
		{
			if (t.GetComponent<CardModel> ().toggled)
			{
				//if it's toggled, untoggle
				t.GetComponent<CardModel>().toggleCard();
			}
		}
	}





	// Use this for initialization
	void Awake () {
		upperLimit = 6;//for now at least
		currentCardNumber=0;

		if (this.gameObject.name == "ExorcistHand")
			deckScript = GameObject.Find ("Player_Deck").GetComponent<DeckScript> ();
		else
			deckScript = GameObject.Find ("Enemy_Deck").GetComponent<DeckScript> ();
		hand = new List<GameObject>();


		//to know where to start creating the cards of each hand
		summonerX = AceExorcistGame.instance.summonerHandGO.transform.position.x;
		exorcistX = AceExorcistGame.instance.exorcistHandGO.transform.position.x;
		summonerY = AceExorcistGame.instance.summonerHandGO.transform.position.y;
		exorcistY = AceExorcistGame.instance.exorcistHandGO.transform.position.y;

		summonZoneX = AceExorcistGame.instance.summonZoneGO.transform.position.x;
		summonZoneY = AceExorcistGame.instance.summonZoneGO.transform.position.y;
	}




}
