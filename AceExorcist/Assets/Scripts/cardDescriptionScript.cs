using UnityEngine;
using System.Collections;

public class cardDescriptionScript : MonoBehaviour {

	//to be used to show current card clicked
	//TODO: Research a way to fit lots of card descriptions in some sort of DB(.json maybe, or...what Jing is using for her thing?)

	public static cardDescriptionScript instance;//only gonna be one card description, so...

	public GameObject cardDescription;
	SpriteRenderer descriptionSpriteRenderer;


	public Sprite cardBack, currentCardFace;



	public void changeCardFace(Sprite newCardFace)
	{
		//once a card is clicked, it is "zoomed in" and is shown in the big card on the right, along with a description
		currentCardFace = newCardFace;
		descriptionSpriteRenderer.sprite = currentCardFace;
	}


	public void setDefaultDescription()
	{
		//if player clicks on another place that is not a valid card, set current card showing to be a default card back
		if (descriptionSpriteRenderer != null)
			descriptionSpriteRenderer.sprite = cardBack;
		else
			Debug.Log ("null for some reason");
	}


	// Use this for initialization
	void Start () {
		descriptionSpriteRenderer = cardDescription.GetComponent<SpriteRenderer> ();
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
