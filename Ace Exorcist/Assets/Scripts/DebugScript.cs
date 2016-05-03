﻿using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {

	int cardIndex;
	CardModel cardModel;

	CardFlipper flipper;

	// Use this for initialization
	void Start () {
		cardModel = GameObject.Find ("Card").GetComponent<CardModel> ();
		cardIndex = 0;
		flipper = GameObject.Find("Card").GetComponent<CardFlipper> ();
	}


	public void ToggleCard()
	{
		cardModel.cardIndex = cardIndex;
		cardModel.toggleCardUp (true);//changes to next card
		if (cardIndex >= cardModel.cardFaces.Length) {
			cardIndex = 0;
			flipper.FlipCard (cardModel.cardFaces [cardModel.cardFaces.Length - 1], cardModel.cardBack, -1);
		} 
		else 
		{
			if (cardIndex > 0) {
				flipper.FlipCard (cardModel.cardFaces [cardIndex - 1], cardModel.cardFaces [cardIndex], cardIndex);
			}
			else
			{
				flipper.FlipCard (cardModel.cardBack, cardModel.cardFaces [cardIndex], cardIndex);
			}

			cardIndex++;
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
