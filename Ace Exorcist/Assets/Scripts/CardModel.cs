using UnityEngine;
using System.Collections;
using Cards.Collections;

public class CardModel : MonoBehaviour {

	/*
	 * GameObject that saves the list of possible sprites for face, cardValue and Suit of one card.
	 *
	*/



	public Card currentCard;
	public cardValue cardValue;
	public Suit cardSuit;
	public Sprite cardFace, cardBack;//cardBack is different depending on whether it's enemy or player cards
	public SpriteRenderer spriteRenderer;
	public Sprite[] cardFaces;


	public int cardIndex;//do I need this in the end...?
	public float toggleMovement = 3.0f;//how much the card goes up and down when toggled


	//flags to control card state
	public bool faceUp;//stores if the card is face up, i.e., if it's yours
	public bool toggled = false;//goes true if card was clicked and toggled

	// Use this for initialization
	void Start () {
		//cardFaces = Resources.Load<Sprite>("Sprites/cardSheet");
		faceUp=false;//when the card is created, it's face down, just to be safe
		spriteRenderer=gameObject.GetComponent<SpriteRenderer>();
	}

	public void loadCard(Card c)//generates the card sprite based on its value and suit
	{
		cardFace = cardFaces[10*((int)c.Suit-1) + (int)c.cardValue-1];//gets card face from the spritelist of all card faces
		gameObject.GetComponent<SpriteRenderer>().sprite = cardFace;//changes the card face on the gameObject
		cardSuit = c.Suit;
		cardValue = c.cardValue;
		faceUp = true;//if card was loaded, it went to player's hand, so it's face up
	}


	public void toggleCard()
	{
		//marks card(or unmarks it) by pushing it up a little(or back down)
		if (toggled)
		{
			//push back down
			float newY = transform.position.y;
			newY -= toggleMovement;
			transform.position=new Vector2(transform.position.x,newY);
			toggled = false;
		}

		else
		{
			//marks card
			float newY = transform.position.y;
			newY += toggleMovement;
			transform.position=new Vector2(transform.position.x,newY);
			toggled = true;
		}
	}



	public void toggleCardUp(bool cardUp)//turns card up or down
	{
		if (cardUp) {
			spriteRenderer.sprite = cardFaces[cardIndex];
		} 
		else
		{
			spriteRenderer.sprite = cardBack;
		}

	}

}
