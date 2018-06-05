using System.Collections;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
	public GameObject Im1;
	public WaveManager WM;
	public GameObject PanelTut;
	public GameObject text;
	Animator ani;
	// Use this for initialization
	void Start()
	{
		PanelTut = GameObject.FindGameObjectWithTag("PanelTuto");
		if (PanelTut != null)
		{
			PanelTut.SetActive(false);
		}
		WM = WaveManager.Instance;
		ani = GetComponent<Animator>();
	    StartCoroutine(startScreen(Im1));

	}

	IEnumerator startScreen(GameObject IMG)
	{
		IMG.SetActive(true);

		//   yield return new WaitForSeconds(1f);
		ani.SetBool("lel", true);
		IMG.GetComponent<Animator>().SetBool("start", true);
		yield return new WaitForSeconds(1f);
		IMG.GetComponent<Animator>().SetBool("start", false);
		text.SetActive(true);
		yield return new WaitForSeconds(2f);
		Destroy(this.gameObject);
	}

	private void OnDestroy()
	{
		if (PanelTut == null)
		{
			WM.Starts();
		}
		else
		{
			PanelTut.SetActive(true);
		}
	}
}
