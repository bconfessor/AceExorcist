using UnityEngine;
using System.Collections;

public class CardAnimations : MonoBehaviour {

	//TODO:Fix card animation


	SpriteRenderer spriteRenderer;
	CardModel model;


	public AnimationCurve scaleCurve;
	public float duration = 0.5f;


	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		model = GetComponent<CardModel> ();
	}

	public void FlipCard(Sprite startImg, Sprite endImg, int cardIndex)
	{
		StopCoroutine (Flip (startImg, endImg, cardIndex));
		StartCoroutine (Flip (startImg, endImg, cardIndex));
	}
	public void FlipCard(Sprite startImg, Sprite endImg)
	{
		//card index does not matter
		StopCoroutine (Flip (startImg, endImg));
		StartCoroutine (Flip (startImg, endImg));
	}

	public void DeleteCard()
	{
		//"blinks" card twice before destroying it
		StopCoroutine(Blink());
		StartCoroutine (Blink ());
	}


	IEnumerator Blink()
	{
		//makes card blink twice, then destroys it
		Color tmp = spriteRenderer.color;
		for (int i = 0; i < 2; i++)
		{
			tmp.a = 1.0f;
			spriteRenderer.color = tmp;//make card show
			yield return new WaitForSeconds (0.20f);
			tmp.a = 0f;
			spriteRenderer.color = tmp;//make invisible
			yield return new WaitForSeconds (0.20f);

		}
		//after this, destroy object
		DestroyImmediate(gameObject);
	}

	//flips the card, doesn't modify the card index
	IEnumerator Flip(Sprite startImg, Sprite endImg)
	{
		spriteRenderer.sprite = startImg;
		float time = 0f;

		while (time <= 1f)
		{
			float scale = scaleCurve.Evaluate (time);
			time += Time.deltaTime / duration;

			//changes the x scale of the object, making it seem like it flipped

			Vector3 parentScale = transform.parent.localScale;
			Vector3 localScale = transform.localScale;
			localScale.x = scale/parentScale.x;//apparently this fixed it, by comprising the local scale within the parent scale....don't ask me to explain, idk
			transform.localScale = localScale;

			//when it gets to the middle of the animation, switch to the end image
			if (time >= 0.5f)
			{
				spriteRenderer.sprite = endImg;
			}
			yield return new WaitForFixedUpdate ();

		}

	}



	IEnumerator Flip(Sprite startImg, Sprite endImg, int cardIndex)
	{
		spriteRenderer.sprite = startImg;
		float time = 0f;

		while (time <= 1f) {
			float scale = scaleCurve.Evaluate (time);
			time += Time.deltaTime / duration;

			Vector3 localScale = transform.localScale;
			localScale.x = scale;
			transform.localScale = localScale;

			if (time >= 0.5f) {
				spriteRenderer.sprite = endImg;
			}

			yield return new WaitForFixedUpdate ();

		}

		if (cardIndex == -1) {
			model.toggleCardUp (false);
		}

		else
		{
			model.cardIndex = cardIndex;
			model.toggleCardUp (true);
		}

	}

}
