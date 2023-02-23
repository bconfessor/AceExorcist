using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimationsManager : MonoBehaviour {

	//has general animations used by GO's throughout the game

	public static AnimationsManager instance;

	public void floatingAnimation (GameObject g)
	{
		StopCoroutine (Float (g));
		StartCoroutine (Float (g));

	}


	IEnumerator Float(GameObject floatingObj)
	{
		//makes the GO float around its starting position
		//how to call sine here...? Sucks to have no wifi
		float r = 0f;
		while (true)
		{
			Vector3 position = floatingObj.transform.position;
			position.y += Mathf.Sin (r)/2;
			r += 0.05f;
			floatingObj.transform.position = position;
			yield return new WaitForFixedUpdate();
		}


	}


	void Awake()
	{
		instance = this;
	}


}
