using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cards.Collections;

public class AceExorcistGame : MonoBehaviour
{
	//Script holds the main mechanics and rules of the game



	public static AceExorcistGame instance;

    public Deck exorcistLibrary;//Don't really need these decks here, they're in their own scripts
    public Deck summonerLibrary;

    //public Discard ExorcistDiscard;
    //public Discard SummonerDiscard;
    public Card summonerLibraryCard;
    public GameObject exorcistHandGO, summonerHandGO, exorcistDeckGO, summonerDeckGO, summonZoneGO;
	public GameObject cardGO;//Holds the card prefab to be created, to be place via inspector 

    public Hand summonerHand, exorcistHand;
	public Hand summonZone;

    public List<Card> cardsPlayed; //is this needed?



	//general values to be used throughout game
    public int exorcistHP = 30;
    public int summonerHP = 60;
	public int currentExorcistHP, currentSummonerHP;
    public int maxHandSize = 6;

	//flags to control game flow
    public bool isExorcistTurn = false;//holds whether it's the exorcist's turn or the summoner's
	public bool isExorcist = true;//used for deck building, hand control, among other things
	public bool exorcistAlive = true;
	public bool summonerAlive = true;


	void Awake()
	{
		instance = this;
		currentExorcistHP = exorcistHP;
		currentSummonerHP = summonerHP;

		//Create Deck and Hand for Exorcist, and draw 5 cards from Deck to Hand
		exorcistLibrary = new Deck(isExorcist);
		exorcistHand = exorcistHandGO.GetComponent<Hand>();
		summonerLibrary = new Deck(!isExorcist);
		summonerHand = summonerHandGO.GetComponent<Hand>();

		summonZone = summonZoneGO.GetComponent<Hand> ();

		//Create Discards (as a Hand - but not in this implementation)

	}

	void Start()
	{


	}


	public void ResetGame()
	{
		//resets flags and values so game can restart
		currentExorcistHP = exorcistHP;
		currentSummonerHP = summonerHP;
		exorcistAlive = true;
		summonerAlive = true;


		//how to properly reset hand...?(like this I guess)
		//TODO

	}


	public bool ExorcistIsAlive()
	{
		//returns true if exorcist is alive
		if (currentExorcistHP <= 0)
		{
			exorcistAlive = false;
		}
		return exorcistAlive;
	}

	public bool SummonerIsAlive()
	{
		//returns true if summoner is alive
		if (currentSummonerHP <= 0)
		{
			summonerAlive = false;
		}
		return summonerAlive;
	}


	/*
	 
	  Ok, so here's the deal: I'll have to pretty much redo ALL of their code(which will pretty much make it all MY code, so...
	  I'll credit Jordan/Mikail for the card idea and Dani/Zoe for the art), so for now I'll comment it all out and fix it
	  little by little. Once I get the cards working properly, I'll focus on fixing those game rules (which FYI, should have
	  been WAAAAAY more commented than they are, idk what half of those methods do just by looking at them).*/


    public Hand GetCardsInHand() //is this handled in Hand?
    {
        //Check if player is Exorcist
        if (isExorcistTurn)
        {
            return exorcistHand;
        }
        else
        {
            return summonerHand;
        }
    }


	public void currentTurnEnded()
	{
		//can be used for both players, since MainGameLoop controls flow
		//invert the enabled scripts(that will change the boolean accordingly)
		exorcistDeckGO.GetComponent<Player_Turn> ().enabled = !exorcistDeckGO.GetComponent<Player_Turn> ().enabled;
		summonerDeckGO.GetComponent<Enemy_Turn> ().enabled = !summonerDeckGO.GetComponent<Enemy_Turn> ().enabled;

	}

	//==================================================================== SUMMONER METHODS ===========================================================================

	public bool summonedRitual(List<GameObject> ritualSummon)
	{

		//receives list of cards, but it can only have ONE card to be placed. If it has more than 1, or if it's not a face card(value = 8,9,10), returns false
		if (ritualSummon.Count != 1 || (int)ritualSummon [0].GetComponent<CardModel> ().cardValue < 8)
		{
			//card not fit for summon
			Debug.Log("Hand is invalid for a summon");
			return false;
		}
		else
		{
			//card can be used for summon
			summonZone.summonCardToSummonZone(ritualSummon[0]);//GIVING ME SOME ERROR
			summonerHand.Invoke("displayCards",Time.deltaTime);
			return true;//card properly summoned;
		}
	}



	public int summonerAttacked(List<GameObject> summonerAttack)
	{
		//summoner attack must be a straight
		//TODO

		//if hand size == 1, it's an automatic straight
		if (summonerAttack.Count == 1)
		{
			return (int)summonerAttack [0].GetComponent<CardModel> ().cardValue;
		}


		//must arrange hand before a straight can be checked
		summonerAttack = summonerAttack.OrderBy( x => x.GetComponent<CardModel>().cardValue ).ToList();

		Debug.Log ("After sort");
		foreach (GameObject card in summonerAttack)
		{
			Debug.Log ("Current card: " + card.GetComponent<CardModel> ().cardValue + " of " + card.GetComponent<CardModel> ().cardSuit);
		}


		int damage = 0;
		for (int i = 0; i < summonerAttack.Count-1; i++)
		{
			if (summonerAttack [i].GetComponent<CardModel> ().cardValue == summonerAttack [i + 1].GetComponent<CardModel> ().cardValue - 1)//if current is just one lower than next
			{
				damage += summonerAttack [i].GetComponent<CardModel> ().cardValue;
				continue;
			}

			else//not a straight
			{
				return -1;
			}
		}
		//must add last card, left loop before that happened
		damage+=summonerAttack[summonerAttack.Count-1].GetComponent<CardModel>().cardValue;
		//if it got here means attack is valid
		return damage;
	}

	public bool summonerSacrificeDrew(List<GameObject> summonerSacrifice)
	{
		//sacrifices some cards to be able to draw new ones
		//hand of 2 or 3 cards only
		if (summonerSacrifice.Count == 2 && summonerSacrifice[0].GetComponent<CardModel>().cardValue==summonerSacrifice[1].GetComponent<CardModel>().cardValue)
		{
			//run draw method twice
			summonerHand.addSummonerCard ();
			summonerHand.addSummonerCard ();
			return true;
		}
		//if hand has 3 cards of equal value
		else if (summonerSacrifice.Count == 3 && summonerSacrifice[0].GetComponent<CardModel>().cardValue==summonerSacrifice[1].GetComponent<CardModel>().cardValue
				&& summonerSacrifice[0].GetComponent<CardModel>().cardValue==summonerSacrifice[2].GetComponent<CardModel>().cardValue)
		{
			//run draw method three times
			summonerHand.addSummonerCard ();
			summonerHand.addSummonerCard ();
			summonerHand.addSummonerCard ();
			return true;

		}
		else//invalid hand
			return false;
	}

	public void checkSummonerVictory()
	{
		//if exorcist's life is less than 0 or their deck is over or summoner summoned 3 cards, summoner wins
		if (exorcistDeckGO.GetComponent<DeckScript> ().deck.getRemainingCards() <= 0 || currentExorcistHP <= 0 || summonZone.hand.Count>=3)
		{
			summonerWon ();
		}

	}

	public void summonerWon()
	{
		//is called if summoner wins the match
		//TODO
	}


	//===================================================================== EXORCIST METHODS =========================================================================


	public int exorcistAttacked(List<GameObject> exorcistAttack)
	{
		//if the cards chosen are from a valid exorcist attack, returns a value >0; else, returns -1

		//a one-card flush is valid; so, if it only has one card, return it immediately
		if (exorcistAttack.Count == 1)
		{
			return (int)exorcistAttack [0].GetComponent<CardModel> ().cardValue;
		}
		int damage = 0;
		//from here on, it's at least 2 cards
		for(int i = 0; i < exorcistAttack.Count-1;i++)
		{
			//needs to be a flush(all of same suit), so if it suit changes, we can say it's not a flush
			if (exorcistAttack [i].GetComponent<CardModel> ().cardSuit == exorcistAttack [i + 1].GetComponent<CardModel> ().cardSuit)
			{
				damage+=(int)exorcistAttack [i].GetComponent<CardModel> ().cardValue;//gets the damage(won't get the last one, must be gotten outside for loop)
				continue;
			}
			else
			{
				//if it got to this else, means its not flush ,diff suits
				return -1;

			}
			
		}
		return damage+ (int)exorcistAttack[exorcistAttack.Count-1].GetComponent<CardModel>().cardValue;
		//returns damage value; needs to implement actual attack on deck(or summon zone, depends)
	}


	public bool exorcistHealed(List<GameObject> exorcistHeal)
	{
		//must be a pair of equal values, nothing else
		//returns are just to indicate if it worked
		if (exorcistHeal.Count == 2 && exorcistHeal [0].GetComponent<CardModel> ().cardValue == exorcistHeal [1].GetComponent<CardModel> ().cardValue)
		{
			currentExorcistHP+= (int)exorcistHeal [0].GetComponent<CardModel> ().cardValue * 2;//will heal the total value of the pair
			if (currentExorcistHP > exorcistHP)
			{
				currentExorcistHP = exorcistHP;//cap at max
			}
			return true;
		}
		else
			return false;
	}


	public int mitigateSummonerAttack(List<GameObject> summonerAttack, List<GameObject> exorcistDefense)
	{
		//returns resultant damage to be taken if player defense can somehow defend themselves from the summoner's attack
		//if they can't(invalid hand), return -1 or -2 and player must redo their hand
		//player can't "discard" cards by using more cards than necessary; that will lead to an invalid hand

		//Returns:
		//0+: amount of damage the player will take
		//-1: invalid hand(oversized)
		//-2: invalid combination(one or more cards would not make any difference in the calculation)

		//first, check if hand is valid;exorcist should have only used number of cards less than or equal to summoner's
		if (exorcistDefense.Count > summonerAttack.Count)
			return -1;

		//get the total damage that would be dealt
		int damage = 0;
		foreach (GameObject card in summonerAttack)
		{
			damage += card.GetComponent<CardModel> ().cardValue;
		}


		//need to arrange them in order, to make it easier to make the checks
		summonerAttack = summonerAttack.OrderBy( x => x.GetComponent<CardModel>().cardValue ).ToList();
		exorcistDefense = exorcistDefense.OrderBy (x => x.GetComponent<CardModel> ().cardValue).ToList ();


		//3 cases, depending on size of summoner hand
		switch (summonerAttack.Count)
		{
		case 1:
			//easiest, checks if any of the exorcist's cards is either bigger or smaller than the summoner's
			//if so, damage will be mitigated
			foreach (GameObject card in exorcistDefense)
			{
				if (card.GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue - 1
					|| card.GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue + 1)
				{
					return 0;//only card in exorcist hand mitigated only card used by summoner; no damage dealt
				}
			}
			break;
		case 2:
			//here we must check the summoner's pair against the exorcist hand(which may be 1 or 2 cards)

			if (exorcistDefense.Count == 1)
			{
				if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue - 1)
				{
					//mitigates damage from lowest card
					return damage - (int)summonerAttack [0].GetComponent<CardModel> ().cardValue;
				} 
				else if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [1].GetComponent<CardModel> ().cardValue + 1)
				{
					//mitigates damage from highest card
					return damage - (int)summonerAttack [1].GetComponent<CardModel> ().cardValue;
				}
			}

			//only other possible case is exorcist having two cards. 
			//In this case, the two MUST, in order, nullify the summoner's cards
			else
			{
				if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue - 1
					&& exorcistDefense [1].GetComponent<CardModel> ().cardValue == summonerAttack [1].GetComponent<CardModel> ().cardValue + 1)
				{
					return 0;
				}
			}
			break;
		case 3:
		case 4:
		case 5:
		case 6:
			//in this case we either have 1 card mitigating one of the extremity ones, or two cards mitigating both extremities
			if (exorcistDefense.Count == summonerAttack.Count)
			{
				//can't, too many cards
				return -1;
			} else if (exorcistDefense.Count == 1)
			{
				if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue - 1)
				{
					//mitigates damage from lowest card
					return damage - (int)summonerAttack [0].GetComponent<CardModel> ().cardValue;
				} 
				else if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [summonerAttack.Count-1].GetComponent<CardModel> ().cardValue + 1)
				{
					//mitigates damage from highest card
					return damage - (int)summonerAttack [summonerAttack.Count-1].GetComponent<CardModel> ().cardValue;
				}
			}
			else//exorcist count = 2
			{
				//then exorcist cards MUST nullify lower and higher summoner cards 
				if (exorcistDefense [0].GetComponent<CardModel> ().cardValue == summonerAttack [0].GetComponent<CardModel> ().cardValue - 1
					&& exorcistDefense [1].GetComponent<CardModel> ().cardValue == summonerAttack [summonerAttack.Count - 1].GetComponent<CardModel> ().cardValue + 1)
				{
					return damage - (int)summonerAttack[0].GetComponent<CardModel>().cardValue - (int)summonerAttack[summonerAttack.Count-1].GetComponent<CardModel>().cardValue;
				}
			}
			break;
		
		default:
			break;
		}
		//if none of the cases above were successful, means the hand is invalid
		return -2;
	}
		

	public void checkExorcistVictory()
	{
		//if exorcist's life is less than 0 or their deck is over, summoner wins
		if (summonerDeckGO.GetComponent<DeckScript> ().deck.getRemainingCards() <= 0 || currentSummonerHP <= 0)
		{
			exorcistWon ();
		}

	}

	public void exorcistWon()
	{
		//is called if exorcist wins the match
		//TODO
	}

	/* 

    public bool doSummonerAttack(List<Card> AttackWithCards)
    {
        if (isExorcistTurn)
            return false;

        int AttackValue = 0;

        AttackWithCards.Sort();
        //ORDER CARDSPLAYED - DONE

        for (int counter = 0; counter < AttackWithCards.Count; counter++)  //CHECK IF CONSECUTIVE RUN
        {
            AttackValue = AttackValue + (int)AttackWithCards[counter].cardValue;

            if (counter < AttackWithCards.Count - 1)
              if (AttackWithCards[counter + 1].cardValue - AttackWithCards[counter].cardValue != 1)
                    return false; //if it's not a consecutive run => break method
        }

        // ADD MITIGATE
        // If exorcist has (AttackWithCards[0].cardValue - 1) 
        // or (AttackWithCards[AttackWithCards.Count-1].cardValue + 1)
        // prompt exorcist whether he would like to mitigate

        //FIX MITIGATE

        List<Card> MitigateWithCards=null;

        foreach (Card theCard in ExorcistHand.hand)
            if ((theCard.cardValue == AttackWithCards[0].cardValue - 1) || (theCard.cardValue == AttackWithCards[AttackWithCards.Count - 1].cardValue + 1))
            {
                MitigateWithCards.Add(theCard);
            }


        if (MitigateWithCards.Count != 0)
            DoMitigate(ref AttackValue, MitigateWithCards);

        ExorcistHP = ExorcistHP - AttackValue; // adjusted via (potentially mitigated) attack

        return true;
        //Validation
    }

    public bool doSummonerPlaySummon(Card SummonCard)
    {
        if (isExorcistTurn) //Only for Summoner
            return false;

        //Check that it's a Face card, allowed in Summon Zone
        if ((int)SummonCard.cardValue > 1 && (int)SummonCard.cardValue < 8)
            //message to player - not a Face Card so can't be part of Summons
            return false;

        //If it is, remove it from the Summoner's hand and add it to Summon Zone
        SummonZone.hand.Remove(SummonCard); // (Takes it and removes it) 

        //Do changes to game state (hit points - done, remove cards from hand - done, etc)

        return true;
        //return true if success; false if failed or illegal move (done)

    }

    public bool doSummonerDraw(List<Card> DrawWithCards)
    {
        if (isExorcistTurn)
            return false;

        //Validation
        //First, check if there are exactly two or three cards.
        if (DrawWithCards.Count != 2 && DrawWithCards.Count != 3)
            //message to player - must play exactly two or three cards (of equal value)
            return false;

        //If so, check that they are of equal value (a pair or triad)
        for (int i=0; i<2; i++)
        {
            if (DrawWithCards[i].cardValue != DrawWithCards[i + 1].cardValue)
                //message to player - cards played must be of equal value
                return false;
        }

        //Discard the cards played and draw new cards
        foreach (Card theCard in DrawWithCards)
        {
            DrawWithCards.Remove(theCard); //remove the card used to draw
            ExorcistHand.addCard(ExorcistLibrary.TakeCard());
            // draw a new card from ExorcistLibrary and add it to ExorcistHand
        }

        //Finally return true
        return true;

    }

    public bool DoMitigate(ref int AttackValue, List<Card> MitigateWithCards)
    {
        if (SummonZone != null)
            return false; //cannot mitigate when there's an attack on summons

        else
        {
            List<Card> MitigatingWith = null;
            //select which cards to actually mitigate with (out of MitigateWithCards) - DONE in ToggleMitigate

            // ToggleMitigate(input, MitigateWithCards, MitigatingWith)
            // for this version of ToggleCard we need to make sure no two cards have the same cardValue -DONE

            int sum = 0;

            foreach (Card theCard in MitigatingWith)
            {
                ExorcistHand.removeCard(theCard); //removing from exorcist hand
                sum = sum + (int)theCard.cardValue; //total mitigation
            }
                AttackValue = AttackValue - sum; //adjusted attack value

            return true;
        }

        //Validation

        // DO NOT DRAW CARDS WHEN ONE CHOOSES TO MITIGATE

    }

    public bool CheckVictorySummoner()
    {
        if ((SummonZone.hand.Count == 3) || (ExorcistHP <= 0))
            return true;
        else return false;
        //if 3 cards in summon zone or exorcist HP <= 0, win
    }

    public bool CheckVictoryExorcist()
    {
        if (SummonerHP <= 0)
            return true;
        else return false;
        //if summoner HP <= 0, win
    }

    public void passTurn()
    {
        isExorcistTurn = !isExorcistTurn;
        //draw card for current player
        //should be PlayerLibrary.DrawCard(PlayerHand). Or something like that

        //CheckVictoryExorcist();
        //CheckVictorySummoner();
        //Check victory conditions
    }

     public void validatePlay(List<Card> CardsPlayed)
    {
        if (isExorcistTurn)
        { int ExorcistAttack=1; //defaulted
            // int ExorcistHeal = 1; //defaulted

            foreach (Card theCard in CardsPlayed)

                if (theCard.Suit != CardsPlayed[0].Suit) //exorcist flush = attack
                    ExorcistAttack = ExorcistAttack * 0; //no flush no attack

                else if ((CardsPlayed.Count == 2) && (CardsPlayed[0].cardValue == CardsPlayed[1].cardValue)) //pair
                    doExorcistHeal(CardsPlayed); //healed if pair

                    if (ExorcistAttack==1) doExorcistAttack(CardsPlayed) ; //flush => attack

                    //ADD MITIGATE CODE
        }
        else //summoner
        {
            //checking if summoning
            foreach (Card theCard in CardsPlayed)

                if ((CardsPlayed.Count == 1) && (( (int)CardsPlayed[0].cardValue > 7) || ( (int)CardsPlayed[0].cardValue == 1))) //face cards 
                    doSummonerPlaySummon(CardsPlayed[0]); //ONE face card played = summon

                else if ((CardsPlayed.Count == 2) && ( CardsPlayed[0].cardValue == CardsPlayed[1].cardValue)) //pair
                    doSummonerDraw(CardsPlayed); //pair => summoner draws

                else doSummonerAttack(CardsPlayed);
            //REMEMBER TO CHECK IF RUN
                
            //ADD MITIGATE CODE
        }
    }


	*/
}
