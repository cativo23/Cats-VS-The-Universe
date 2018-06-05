using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	GameManager GM;
	public GameObject Panel;
	public GameObject Panel2;
	// Use this for initialization
	void Start () {
		GM = GameManager.Instance;
		GM.GameOver += GameOber;
		GM.GameOverTuto += GameOverTuto;
	}

	public void GameOber()
	{
		Panel.SetActive(true);

	}

	public void GameOverTuto()
	{
		Panel2.SetActive(true);
	}

}
