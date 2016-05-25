using UnityEngine;
using System.Collections;

public class FlowManager : MonoBehaviour {

	//used to hold the coroutines that control the user responses to input(like asking the exorcist if they want to mitigate an attack)
	//also used to control the "pause menu"(whenever I decide to actually do one)

	//as I'm doing this, I think this may not be the best use for a coroutine and that it might be more easily implemented using something else,
	//but I'll try it like that to learn it

	public static FlowManager instance;

	//flags to control whiles inside coroutines
	public bool answerChosen = false;//chose whether or not to mitigate
	public bool cardsChosen = false;//chose cards to do the mitigation

	public bool summonAttackChosen = false;//true when player chooses a summon to attack
	public bool canceledSummonAttack = false;

	IEnumerator doExorcistMitigation()
	{

		//when it gets here, means player chose to mitigate, so wait for their input
		Debug.Log("Started mitigation process");
		while (AceExorcistGame.instance.exorcistMitigating)
		{
			Debug.Log ("Waiting for cards to be chosen...");
			if (cardsChosen)//exorcist chose cards to use for mitigation by clicking "Mitigate" button
			{
				//check for their validity
				Debug.Log("Check for cards validity");
				//seems to break around here...
				int finalDamageToExorcist = AceExorcistGame.instance.mitigateSummonerAttack (AceExorcistGame.instance.summonerHand.getToggledCards (),
					AceExorcistGame.instance.exorcistHand.getToggledCards());
				Debug.Log ("Let's see if cards are good...");
				//if cards aren't valid, get out of this while and wait for new input
				if (finalDamageToExorcist == -1)
				{
					UIManager.instance.displayNewText ("Too many cards chosen; Invalid hand.");
					cardsChosen = false;
					//yield return null;

				} 
				else if (finalDamageToExorcist == -2)
				{
					UIManager.instance.displayNewText ("Invalid combination; choose a new combination.");
					cardsChosen = false;
					//yield return null;
				}
				else//final damage >=0, valid hand chosen
				{
					//take out toggled cards of both players, damage exorcist with what's left of damage
					AceExorcistGame.instance.exorcistDamaged(finalDamageToExorcist);

					//text changes depending on how much damage is mitigated
					if (finalDamageToExorcist == 0)
					{
						UIManager.instance.displayNewText ("Exorcist completely blocked the attack!");
					}
					else
					{
						//took SOME damage
						UIManager.instance.displayNewText("Exorcist partially blocked the attack! Took "+finalDamageToExorcist+ " damage!");
					}
					//TODO: if exorcist does mitigate damage, find a sound for that as well

					//play summoner attack sound; if less than three cards, counts as light attack; if three or more, heavy attack
					if (AceExorcistGame.instance.summonerHand.getToggledCards ().Count >= 3)//heavy attack
					{
						SoundManager.instance.playHeavyAttackSound ("summoner");
					} 
					else
						SoundManager.instance.playLightAttackSound ("summoner");

					//take toggled cards from both
					AceExorcistGame.instance.summonerHand.removeToggledCards();
					AceExorcistGame.instance.exorcistHand.removeToggledCards ();

					//complete action(fix buttons that work), hide mitigation panel, exit mitigation state
					UIManager.instance.hideMitigationPanel();
					ButtonManager.instance.actionCompleted ();
					AceExorcistGame.instance.exorcistMitigating = false;//to be able to leave the while
					yield return null;
				}
			}

			else
			{
				cardsChosen = false;
				yield return null;
			}
		}


		//in the end, reset the flags so they can be reused in the next turn
		answerChosen = false;
		cardsChosen = false;


	}

	public IEnumerator destroySummonerCards(int totalDamageToBeDone)
	{


		//if exorcist chooses to attack summoner deck with a valid hand, call this coroutine, which locks every button while it runs
		ButtonManager.instance.deactivateAllButtons();//so nothing can be done during the animation
		int damage = totalDamageToBeDone;
		int tmp = damage;//to know how much damage to take each round
		//every x seconds, checks if the next card can be destroyed, and while it can, keep doing it
		while (AceExorcistGame.instance.summonerDeckGO.GetComponent<DeckScript> ().canDestroyNextCardInDeck (ref damage))
		{
			//destroyed another card, must damage enemy health as well


			AceExorcistGame.instance.summonerDamaged(tmp-damage);//gets the difference, which is the damage gotten from this card
			tmp = damage;

			UIManager.instance.updateHealthUI ();
			UIManager.instance.updateCardsLeftUI ();
			AceExorcistGame.instance.checkExorcistVictory ();

			yield return new WaitForSeconds(1.2f);
		}

		//if the flush wasn't even able to damage one card, the totalDamageToBeDone and damage should still be the same; give a message to exorcist
		if (totalDamageToBeDone == damage)
		{
			UIManager.instance.displayNewText ("Couldn't damage summoner at all!");
		}

		//when it gets here, means it destroyed as many cards as it could, so just give info to exorcist
		else
		{
			UIManager.instance.displayNewText ("Exorcist damaged summoner for " + (totalDamageToBeDone - damage) + " damage!");
		}
		ButtonManager.instance.actionCompleted ();
	}


	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
