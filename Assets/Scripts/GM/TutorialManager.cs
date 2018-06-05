using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : GameManager {

	public Text tuText;
	IEnumerator TypeSentenceTuto(string _sentence)
	{
		Info.text = "";
		foreach (char letter in _sentence.ToCharArray())
		{
			tuText.text += letter;
			yield return 0;
		}
		yield return new WaitForSeconds(2f);

		tuText.text = "";
	}

}
