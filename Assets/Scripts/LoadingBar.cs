using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour {

    AsyncOperation ao;
    public GameObject loadingScreenBG;
    public Slider porgBar;
    public Text loadingText;
    AudioManager audi;

    public GameObject SelecNivel;
	// Use this for initialization
	void Start () {
        audi = AudioManager.instance;

        audi.PlayMusic("Main");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel(string level)
    {

        SelecNivel.SetActive(false);
        loadingScreenBG.SetActive(true);
        loadingText.text = "Loading...";
        StartCoroutine(LoadLevelWithProgress(level));
    }


    IEnumerator LoadLevelWithProgress(string level)
    {
        yield return new WaitForSeconds(1f);

        ao = SceneManager.LoadSceneAsync(level);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            porgBar.value = ao.progress;
            if (ao.progress == 0.9f)
            {
                porgBar.value = 1f;
                loadingText.text = "Tap to continue...";
                if (Input.anyKeyDown)
                {
                    ao.allowSceneActivation = true;
                    audi.PlaySound("Loaded");
                }
            }

            yield return null;
        }


    }
}
