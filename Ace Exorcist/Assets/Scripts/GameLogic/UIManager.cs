﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	public GameObject eHPGO, sHPGO, textGO, exorcistDeckGO, summonerDeckGO;//exorcist and summoner hp Game Objects, textbox game object
	public Text exorcistHP, summonerHP, summonerDeck, exorcistDeck, textBoxText;


	//game objects for the mitigation panels, need to be turned on/off depending on what is happening
	public GameObject choicePanel, mitigationPanel, summonAttackPanel;

	public GameObject gameOverCanvas;

	public void displayChoicePanel()
	{
		//displays panel and enables its buttons
		choicePanel.SetActive(true);
	}
	public void hideChoicePanel()
	{
		choicePanel.SetActive (false);
	}
	public void displayMitigationPanel()
	{
		mitigationPanel.SetActive (true);

	}
	public void hideMitigationPanel()
	{
		mitigationPanel.SetActive (false);
	}

	public void displaySummonAttackPanel()
	{
		summonAttackPanel.SetActive (true);

	}
	public void hideSummonAttackPanel()
	{
		summonAttackPanel.SetActive (false);
	}



	public void displayNewText(string text)
	{
		//gets the new string and display it in the text box(i.e., will substitute all debugs throughout the program)
		textBoxText.text = text;

	}

	public void updateHealthUI()
	{
		//updates both HP's
		exorcistHP.text = "Exorcist HP: " + AceExorcistGame.instance.currentExorcistHP;
		summonerHP.text = "Summoner HP: " + AceExorcistGame.instance.currentSummonerHP;

	}

	public void updateCardsLeftUI()
	{
		//Debug.Log("Cards left: " + AceExorcistGame.instance.exorcistDeckGO.GetComponent<DeckScript> ().deck.getRemainingCards());
		exorcistDeck.text = "Cards left: " + AceExorcistGame.instance.exorcistDeckGO.GetComponent<DeckScript> ().deck.getRemainingCards();
		summonerDeck.text = "Cards left: " + AceExorcistGame.instance.summonerDeckGO.GetComponent<DeckScript> ().deck.getRemainingCards();
	}

	public void gameOverScreenFadesIn()
	{
		float fadeTime = 3.0f;

		//activate canvas, but not its contents
		gameOverCanvas.SetActive(true);
		//sets button as inactive, text as invisible
		Color tmp = gameOverCanvas.GetComponent<Color>();
		tmp.a = 0.0f;

	}

	IEnumerator ScreenFadeIn(int fadeTime)
	{
		float time = 0f;
		while (time <= fadeTime)
		{
			//gets game over text and slowly make it fade in
			return null;//TODO: finish this TODAY
		}
		return null;
	}

	void Awake()
	{
		textBoxText = textGO.GetComponent<Text> ();

		//panels start hidden
		hideChoicePanel();
		hideMitigationPanel ();
		hideSummonAttackPanel ();
	}

	// Use this for initialization
	void Start () {
		instance = this;
		exorcistHP = eHPGO.GetComponent<Text> ();
		summonerHP = sHPGO.GetComponent<Text> ();
		exorcistDeck = exorcistDeckGO.GetComponent<Text> ();
		summonerDeck = summonerDeckGO.GetComponent<Text> ();
		updateHealthUI ();
		Invoke("updateCardsLeftUI",Time.deltaTime);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
