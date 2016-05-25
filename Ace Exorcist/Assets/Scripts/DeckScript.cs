using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cards.Collections;

public class  DeckScript : MonoBehaviour {
	
	
	public int cardAmount;
	public int cardsRemaining;
	public Deck deck;

	public Hand playerHand;

	public GameObject CardGO;


	public bool playerDeck;

	public void startDeckAndDrawCards()
	{

		deck = new Deck(playerDeck);//depends on whose deck it is

		//shuffles deck
		deck.shuffleDeck();

		//empty hands just to be sure, in case this is being called by Reset
		playerHand.emptyHand();

		//draw 5 cards for each one
		for (int i = 0; i < 5; i++)
		{
			//here, we must grab 5 cards and put them inside the hand. For that, we must first instantiate 5 card GO's and then order them
			GameObject card = Instantiate(CardGO);
			card.GetComponent<CardModel> ().loadCard (deck.TakeCard ());//pulled a card from deck
			//now we add the pulled card to the hand
			playerHand.addCard(card ,playerDeck);//and this runs the display always, so it'll update the card
		}

		playerHand.displayCards ();

	}

	public bool canDestroyNextCardInDeck(ref int damage)
	{
		//looks at next card on top of the summoner's deck;if it's equal to or weaker than the damage, it gets destroyed, and damage gets lowered
		if ((int)deck.GetTopCard ().cardValue <= damage)
		{
			//instantiate it on top of deck, then take card out(with destruction animation)
			GameObject card = Instantiate (CardGO, this.gameObject.transform.position, Quaternion.identity) as GameObject;
			card.transform.parent = gameObject.transform;//makes it a child of the deck
			card.GetComponent<CardModel> ().loadCard (deck.TakeCard ());//created it and put it on top of deck
			cardDescriptionScript.instance.changeCardFace(card.GetComponent<CardModel>().cardFace);//shows current card gotten from deck in the card description area
			card.GetComponent<CardModel>().turnCardDown();
			StartCoroutine (card.GetComponent<CardAnimations> ().FlipUpAndDelete ());
			damage -= (int)card.GetComponent<CardModel> ().cardValue;//decreases damage everytime a card is destroyed

			return true;
		} 
		else
			return false;
	}


	// Use this for initialization
	void Start () {

		//create references to the player's Hand component and the enemy's, depending on who's this script parent
		if (playerDeck) {
			playerHand = AceExorcistGame.instance.exorcistHand;
		} 
		else
		{
			playerHand = AceExorcistGame.instance.summonerHand;
		}



		//maybe I overdid it with this method name...?
		startDeckAndDrawCards ();

		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E)&&!playerDeck)//will make the enemy draw a card and show its hand
		{
			if(playerHand.currentCardNumber<AceExorcistGame.instance.maxHandSize)//max number of cards you can have in your hand
			{
				Card cardDrawn = deck.TakeCard();
				//creates the card in the game world with the Card prefab
				GameObject card = Instantiate(CardGO,new Vector3(3,3), Quaternion.identity) as GameObject;

				//once GO is created, fill it up with the info on the card drawn
				card.GetComponent<CardModel> ().loadCard (cardDrawn);
				/*
				card.GetComponent<CardModel>().cardValue = cardDrawn.cardValue;
				card.GetComponent<CardModel>().cardSuit = cardDrawn.Suit;
				Sprite cardSprite = Resources.Load<Sprite>("Sprites/aceSpades");*/

				playerHand.addCard(card,playerDeck);

				//card.GetComponent<SpriteRenderer>().sprite = cardSprite;


				Debug.Log("Enemy drew a "+cardDrawn.cardValue + " of " + cardDrawn.Suit);

				card.transform.parent = GameObject.Find("SummonerHand").transform;//make this card a child of the summoner hand
				Debug.Log("Enemy has "+playerHand.getHandCount());
				playerHand.showHand();

			}
			else
			{
				Debug.Log("Enemy can't draw anymore cards!");
			}

		}
		else if(Input.GetKeyDown(KeyCode.P)&&playerDeck)//will make the player draw a card and show it's current hand
		{
			if(playerHand.currentCardNumber<AceExorcistGame.instance.maxHandSize)//max 
			{
				Card cardDrawn = deck.TakeCard();
				Debug.Log("Player drew a "+cardDrawn.cardValue + " of " + cardDrawn.Suit);
				
				//creates the card in the game world
				GameObject card = Instantiate(CardGO) as GameObject;

				//once card is created, fill it up with the info on the card drawn
				card.GetComponent<CardModel> ().loadCard (cardDrawn);

				//then add it too hand
				playerHand.addCard(card,playerDeck);

				card.transform.parent = GameObject.Find("ExorcistHand").transform;//make this card a child of the exorcist hand

				//shows player hand
				Debug.Log("Player has "+playerHand.getHandCount());
				playerHand.showHand();
			}
			else
			{
				Debug.Log("Player can't draw anymore cards!");
			}
		}
	}
}
