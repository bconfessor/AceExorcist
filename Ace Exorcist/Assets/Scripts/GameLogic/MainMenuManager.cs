using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	//Manages buttons and UI in main menu

	public static MainMenuManager instance;

	public GameObject playButton, instructionsButton, creditsButton, instructionsCanvas, creditsCanvas;

	public void pressedPlayButton()
	{
		SceneManager.LoadScene ("TestScene");//loads main game
	}

	public void pressedInstructionsButton()
	{
		creditsCanvas.SetActive (false);
		instructionsCanvas.SetActive (!instructionsCanvas.activeInHierarchy);//toggles instructions
	}
	public void pressedCreditsButton()
	{
		instructionsCanvas.SetActive (false);//turn it off always
		creditsCanvas.SetActive (!creditsCanvas.activeInHierarchy);//toggle it
	}

	public void deactivateAllCanvases()
	{
		creditsCanvas.SetActive (false);
		instructionsCanvas.SetActive (false);
	}


	// Use this for initialization
	void Awake () {
		instance = this;
		deactivateAllCanvases ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
