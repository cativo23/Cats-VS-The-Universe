using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour {
	public Text teXt;

	public string[] texto;
	public int actual;
	public GameManager GM;
	public WaveManager WM;
	Coroutine cor;

	private void Awake()
	{
		actual = 0;
	}

	private void Start()
	{
		WM = WaveManager.Instance;
		GM = GameManager.Instance;
		GM.Clicked += NextText;

		


	}

	private void OnDisable()
	{
		if (GM != null)
		{
			GM.Clicked -= NextText;
		}
			

	}
	private void OnEnable()
	{
		if (GM != null)
		GM.Clicked += NextText;
		StartCoroutine(TypeSentence(texto[actual]));
	}


	IEnumerator TypeSentence(string _sentence)
	{
		teXt.text = " ";
		print(teXt.text);
		foreach (char letter in _sentence.ToCharArray())
		{
			teXt.text += letter;
			yield return 0;
		}
	}

	public void NextText(RaycastHit2D[] hits)
	{
		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].collider.CompareTag("PanelTuto"))
			{
				actual += 1;
				if (actual < texto.Length)
				{
					if(cor != null)
						StopCoroutine(cor);
					cor =StartCoroutine(TypeSentence(texto[actual]));
				}
				else
				{
					Destroy(transform.parent.gameObject);
					WM.Starts();
				}
				
			}
		}
	}
}
