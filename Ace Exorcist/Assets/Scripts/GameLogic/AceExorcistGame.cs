using System;
using UnityEngine;
using System.Collections.Generic;
using Cards.Collections;

public class AceExorcistGame : MonoBehaviour
{


	public static AceExorcistGame instance;

    public Deck ExorcistLibrary;
    public Deck SummonerLibrary;
    //public Discard ExorcistDiscard;
    //public Discard SummonerDiscard;
    public Card SummonerLibraryCard;
    public GameObject ExorcistHandGO, SummonerHandGO;
    public Hand SummonerHand, ExorcistHand;
	public Hand SummonZone;

    public List<Card> CardsPlayed; //is this needed?

	public GameObject CardGO;//Holds the card prefab to be created, to be place via inspector 

	//general values to be used throughout game
    public int ExorcistHP = 30;
    public int SummonerHP = 60;
    public int MaxHandSize = 6;

	//flags to control game flow
    public bool isExorcistTurn = false;//holds whether it's the exorcist's turn or the summoner's
	public bool isExorcist = true;//used for deck building, hand control, among other things
	public bool exorcistAlive = true;
	public bool summonerAlive = true;


	void Awake()
	{
		instance = this;


		//Create Deck and Hand for Exorcist, and draw 5 cards from Deck to Hand
		ExorcistLibrary = new Deck(isExorcist);
		ExorcistHand = ExorcistHandGO.GetComponent<Hand>();
		SummonerLibrary = new Deck(!isExorcist);
		SummonerHand = SummonerHandGO.GetComponent<Hand>();



		//Create Summon Zone
		//SummonZone = new Hand();

		//Create Discards (as a Hand - but not in this implementation)

	}

	void Start()
	{
		//ExorcistHand.currentCardNumber = 0;
		//SummonerHand.currentCardNumber = 0;
		/*
		//draw 5 cards for each one
		for (int i = 0; i < 5; i++)
		{
			//here, we must grab 5 cards and put them inside the hand. For that, we must first instantiate 5 card GO's and then order them
			GameObject card = Instantiate(CardGO,new Vector3(-3,-2), Quaternion.identity) as GameObject;
			card.GetComponent<CardModel> ().loadCard (ExorcistLibrary.TakeCard ());//pulled a card from deck
			//now we add the pulled card to the hand
			ExorcistHand.addCard( card ,true);//and this runs the display always, so it'll update the card
		}


		//Create Deck and Hand for Exorcist, and draw 5 cards from Deck to Hand
		isExorcist = false;


		for (int i = 0; i < 5; i++)
		{
			//same thing, create card GO, add new Card to it, add it to summoner hand
			GameObject card = Instantiate(CardGO, new Vector3(3,2), Quaternion.identity) as GameObject;
			card.GetComponent<CardModel> ().loadCard (SummonerLibrary.TakeCard ());
			SummonerHand.addCard(card,false);
		}

		//once the card is added to the hand, the display must be updated
		ExorcistHand.displayCards();
		SummonerHand.displayCards ();*/

	}

	/*
	 
	  Ok, so here's the deal: I'll have to pretty much redo ALL of their code(which will pretty much make it all MY code, so...
	  I'll credit Jordan/Mikail for the card idea and Dani/Zoe for the art), so for now I'll comment it all out and fix it
	  little by little. Once I get the cards working properly, I'll focus on fixing those game rules (which FYI, should have
	  been WAAAAAY more commented than they are, idk what half of those methods do just by looking at them).


    public Hand GetCardsInHand() //is this handled in Hand?
    {
        //Check if player is Exorcist
        if (isExorcistTurn)
        {
            return ExorcistHand;
        }
        else
        {
            return SummonerHand;
        }
    }

    public void toggleCard(int index, ref List<Card> CardsPlayed)
    { Hand theHand=null;

        if (isExorcistTurn) theHand = ExorcistHand;
        else theHand = SummonerHand;

        if ((index > 0) && (index <= theHand.getHandCount()))  //if index out of bounds for the hand size
        {
            foreach (Card theCard in CardsPlayed)
            {
                if (theCard == theHand.hand[index - 1])
                    CardsPlayed.Remove(theHand.hand[index - 1]); //remove index card if already played
                return; //break method in this case
            }

            CardsPlayed.Add(theHand.hand[index - 1]); //add index card to CardsPlayed, if everything safe
        }

        else return; //break method if index out of bounds
    }


    public void ToggleMitigate(int index, ref List<Card> MitigateWithCards, ref List<Card> MitigatingWith)
    {

        if ((index > 0) && (index <= MitigateWithCards.Count ))  //if index out of bounds for the hand size
        {
            foreach (Card theCard in MitigatingWith)
            {
                if (theCard.cardValue == MitigateWithCards[index - 1].cardValue)
                { if (theCard.Suit == MitigateWithCards[index - 1].Suit)
                        MitigatingWith.Remove(theCard);
                    //remove index card if exactly same card already played, otherwise

                    return; //break method in any case when same value card played
                }
            }

            MitigatingWith.Add(MitigateWithCards[index - 1]); //add index card to CardsPlayed, if everything safe
        }

        else return; //break method if index out of bounds
    }


    public bool doExorcistAttack(List<Card> AttackWithCards)
    {

        int AttackValue = 0;

        if (!isExorcistTurn)
            return false;

            //int ExorcistAttack = 1; //defaulted
                                    // int ExorcistHeal = 1; //defaulted

        foreach (Card theCard in AttackWithCards) // correct enumeration error for AttackWithCards?
        {            AttackValue = AttackValue + (int)theCard.cardValue;
            if (theCard.Suit != AttackWithCards[0].Suit) //exorcist flush = attack
                return false; //no flush no attack
        }

        //if (ExorcistAttack == 0) return false;
        //Check that all in List are of same suit and that cards are from ExorcistHand 
            

            //Check that there is nothing in Summon Zone
            if (SummonZone.getHandCount() == 0) //if Summon Zone empty, attack library

        {
            SummonerLibraryCard = SummonerLibrary.GetTopCard(); //FirstOrDefault();

            while (AttackValue > 0)
            {
                if ((int)SummonerLibraryCard.cardValue <= AttackValue)
                {
                    SummonerHP = SummonerHP - (int)SummonerLibraryCard.cardValue; //change HP
                    AttackValue = AttackValue - (int)SummonerLibraryCard.cardValue; //remaining attackvalue
                    //SummonerDiscard.Add(SummonerLibraryCard); // (Takes it and removes it)
                    SummonerLibrary.TakeCard();    //REMEMBER TO ADD TO DISCARD PILE
                    // Exorcist must also draw one card
                    ExorcistHand.addCard(ExorcistLibrary.TakeCard());
                }
                else
                {
                    AttackValue = -1; //go out of the loop
                    // keep showing the last card on top of the SummonerLibrary
                }

             }

            }

        else //attack Summon Zone
        {
            int i = 0; //choose a card to be attacked (0,1,2)

            if ( (int)SummonZone.hand[i].cardValue <= AttackValue)
            {
                SummonerHP = SummonerHP - (int)SummonZone.hand[i].cardValue; //change HP
                SummonZone.hand.Remove(SummonZone.hand[i]); // (Takes it and removes it)                                     
                // SummonZone[i].Discard;   //REMEMBER TO ADD TO DISCARD PILE
            }
            // Debug.Log("Not enough attack value to destroy that Summoned Card");
            else return false; //attack too small means no attack
        }

        //finally, take the cards played from the Exorcist's hand, and discard them
        foreach (Card theCard in AttackWithCards)
            ExorcistHand.removeCard(theCard); ;// (Takes it and removes it) 
           // SummonZone[i].Discard;   //REMEMBER TO ADD TO DISCARD PILE

        //Do changes to game state (hit points - done, remove cards from hand - done, etc)

        return true;
        //return true if success; false if failed or illegal move
    }

    public bool doExorcistHeal(List<Card> HealWithCards)
    {
        if (!isExorcistTurn) //Healing only for Exorcist
            return false;

        //First, check if there are exactly two cards.
        if (HealWithCards.Count != 2)
            //message to player - must play exactly two cards (of equal value)
            return false;

        //If so, check that they are of equal value (a pair)
        if (HealWithCards[0].cardValue != HealWithCards[1].cardValue)
            //message to player - not a pair
            return false;

        //If so, add the value of each card in turn to the Exorcist's HP
        foreach (Card theCard in HealWithCards)
        {
            ExorcistHP = ExorcistHP + (int)theCard.cardValue;
        }

        //finally, take the cards played from the Exorcist's hand, and discard them
        foreach (Card theCard in HealWithCards)
            // (Takes it and removes it) 
            ExorcistHand.hand.Remove(theCard);
            //ExorcistDiscard.Add(HealWithCards.TakeCard(theCard));
               // SummonZone[i].Discard;   //REMEMBER TO ADD TO DISCARD PILE

        //Do changes to game state (hit points - done, remove cards from hand - done, etc)

        return true;
        //return true if success; false if failed or illegal move (done)

    }

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
