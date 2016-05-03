using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour {


	public GameObject attackButton, drawButton, removeButton;


	public void pressedDrawButton()
	{
		GameObject.Find ("ExorcistHand").GetComponent<Hand> ().addExorcistCard ();
		//once draw is done, it becomes disabled until next turn
		drawButton.GetComponent<Button>().interactable = false;
	}
	public void pressedAttackButton()
	{
		//for now, gets the sum of powers, subtracts it from the summoner's health, discards cards
		AceExorcistGame.instance.SummonerHP-=GameObject.Find ("ExorcistHand").GetComponent<Hand> ().getCardsPower();
		gameObject.GetComponent<UIManager> ().updateHealthUI ();
	}
	public void pressedRemoveButton()
	{
		GameObject.Find ("ExorcistHand").GetComponent<Hand> ().removeToggledCards ();
	}

	public void pressedEndTurnButton()
	{
		//change turns from one player to the other
		AceExorcistGame.instance.isExorcistTurn = !AceExorcistGame.instance.isExorcistTurn;
	}

}
