using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	public GameObject eHPGO, sHPGO;//exorcist and summoner hp Game Objects
	public Text exorcistHP, summonerHP;

	// Use this for initialization
	void Start () {
		instance = this;
		exorcistHP = eHPGO.GetComponent<Text> ();
		summonerHP = sHPGO.GetComponent<Text> ();
		updateHealthUI ();
	}

	public void updateHealthUI()
	{
		//updates both HP's
		exorcistHP.text = "Exorcist HP: " + AceExorcistGame.instance.ExorcistHP;
		summonerHP.text = "Summoner HP: " + AceExorcistGame.instance.SummonerHP;

	}



	// Update is called once per frame
	void Update () {
	
	}
}
