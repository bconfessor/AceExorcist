using UnityEngine;
using System.Collections;
using Cards.Collections;

public class ClickTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void updateDescriptionCard(Sprite newCard)
	{
		cardDescriptionScript.instance.changeCardFace(newCard);

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			//hit=Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
			Vector2 cameraPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log("Clicked at "+cameraPoint.x+","+cameraPoint.y);
			Collider2D col =Physics2D.OverlapPoint(cameraPoint);
			if (col != null)
			{
				if (col.gameObject.tag == "card")
				{

					//when clicked, card must be (un)selected(Only if it's the current player's card, or if exorcist is mitigating damage)
					if (AceExorcistGame.instance.isExorcistTurn && col.gameObject.transform.parent.tag == "exorcistHand"
					    || (!AceExorcistGame.instance.isExorcistTurn && col.gameObject.transform.parent.tag == "summonerHand" && !AceExorcistGame.instance.exorcistMitigating)
					    || AceExorcistGame.instance.exorcistMitigating)
					{
						col.GetComponent<CardModel> ().toggleCard ();
						//change card description to current card clicked
						updateDescriptionCard (col.GetComponent<CardModel> ().cardFace);
					}
					else if (col.gameObject.transform.parent.tag == "SummonZone")//if it's a card in the summon zone, also mark it, regardless of whose turn it is
					{
						//TODO: eventually, will only work when exorcist is attacking summon zone, for now, just show card in description
						updateDescriptionCard(col.GetComponent<CardModel>().cardFace);
					}
				}
			}

		}
	}
}
