using UnityEngine;
using System.Collections;

public class FlowManager : MonoBehaviour {

	//used to hold the coroutines that control the user responses to input(like asking the exorcist if they want to mitigate an attack)
	//also used to control the "pause menu"(whenever I decide to actually do one)

	//as I'm doing this, I think this may not be the best use for a coroutine and that it might be more easily implemented using something else,
	//but I'll try it like that to learn it

	public static FlowManager instance;

	public bool answerChosen = false;//chose whether or not to mitigate
	public bool cardsChosen = false;//chose cards to do the mitigation
	public bool validHandChosen = false;


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


					//TODO: if exorcist does mitigate damage, find a sound for that as well



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

		/*
		if (answerChosen)//means player chose to mitigate, do mitigation
		{
			UIManager.instance.displayNewText ("Choose the cards you will use to mitigate the damage");
			//call mitigation method with chosen cards 
			while (!validHandChosen)
			{
				if (cardsChosen)
				{
					//if cards were chosen, try for a mitigation

				}

				else
				{
					cardsChosen = false;//
					//holds the coroutine here while player chooses cards
					yield return null;
				}
			}
		}*/

		//in the end, reset the flags so they can be reused in the next turn
		answerChosen = false;
		cardsChosen = false;


	}


	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
