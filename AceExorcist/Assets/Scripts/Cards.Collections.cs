using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Cards.Collections
{
	//order 52 cards based on suit and value; to be changed to SUIT our needs(get it?) later
	public enum Suit
	{
		Blades = 1,   
		Blood = 2,   
		Flames = 3,  
		Bells = 4,
        Books = 5,
        Candles = 6,
	}

	public enum cardValue
	{
		Face1 = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Face8 = 8,
		Face9 = 9,
		Face10 = 10,

	}
	
	

	public class Card
	{
		public Card(){
		}
		public Card(cardValue cv, Suit s)
		{
			this.Suit = s;
			this.cardValue = cv;
		}
		public Suit Suit { get; set; }
		public cardValue cardValue { get; set; }
	}
	
  
	public class Deck
	{
		public List<Card> Cards { get; set; } //each card has a suit and value
		
		public Deck(bool isExorcist)
		{
			createDeck(isExorcist);
		}

		public int getRemainingCards()
		{
			return Cards.Count;
		}


		//creates the deck
		public void createDeck(bool isExorcist)
		{
            //if isExorcist, build deck with suits 4-6; if Summoner, use suits 1-3
            int useSuits = 4;
            if (!isExorcist)
                useSuits = 1;


            Cards = new List<Card>();
			for(int i = useSuits; i<useSuits+3; i++)//for each suit...
			{
				for(int j = 1; j<11; j++)//creates a card with this suit and this value
				{
					Card c = new Card();
					c.cardValue=(cardValue)j;
					c.Suit=(Suit)i;
					Cards.Add(c);
				}

			}
			//once deck is created, it must be shuffled
			shuffleDeck();
		}

		public void swap(int x, int y)//to swap cards at positions x and y on the Cards array
		{
			Card tempx = Cards[x];
			Card tempy = Cards[y];
			//remove x and y and reintroduce their replacements

			Cards.RemoveAt(x);
			Cards.Insert(x,tempy);

			Cards.RemoveAt(y);
			Cards.Insert(y,tempx);

		}

		public void shuffleDeck()
		{
			for(int i = 0;i<Cards.Count;i++)//for each card, find a random number and swap positions with that card
			{
				int r =UnityEngine.Random.Range(0,30);
				swap(i,r);//swaps cards at positions i and r
			}

		}
			


        public Card GetTopCard()
        {
            return Cards.FirstOrDefault();
        }
 
		public Card TakeCard()
		{
			if (Cards.Count > 0)
			{
                Card card = GetTopCard(); //Cards.FirstOrDefault(); //Take the first card in the Deck
				Cards.Remove(card); //Remove that card from the Deck
				return card;
			}
			else
			{                               
				return null;
			}
		}
		
		
		
	}
	
}