using UnityEngine;
using System.Collections;
using Cards.Collections;

public class ClickTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			//hit=Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
			Vector2 cameraPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log("Clicked at "+cameraPoint.x+","+cameraPoint.y);
			Collider2D col =Physics2D.OverlapPoint(cameraPoint);
			if(col!=null)
			{
				if (col.gameObject.tag == "card") 
				{
					//if it clicks on a card, it takes it out of the game
					//First, out of the deck...
					Debug.Log ("This is "+col.gameObject.GetComponent<CardModel>().cardValue+" of "+col.gameObject.GetComponent<CardModel>().cardSuit);
					//col.gameObject.transform.parent.GetComponent<Hand> ().removeCard (col.gameObject);

					Hand parentHand = col.gameObject.transform.parent.GetComponent<Hand> ();

					//when clicked, card must be (un)selected
					col.GetComponent<CardModel> ().toggleCard ();

				}
			}
		}
	}
}
