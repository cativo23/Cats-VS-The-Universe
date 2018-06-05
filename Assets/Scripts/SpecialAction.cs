using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAction : MonoBehaviour {
	public float first;
	public float second;
	public delegate void OnCatPress();
	public event OnCatPress CatPressShort;
	public event OnCatPress CatPressLong;


	GameManager GM;
	bool hit;

	// Update is called once per frame
	private void Start()
	{
		GM = GameManager.Instance;
		GM.Clicked += ButtonDown;
		GM.Released += ButtonUp;
	}


	private void ButtonDown(RaycastHit2D[] hits)
	{
		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].collider.gameObject == gameObject)
			{
				first = Time.time;
				hit = true;
				return;
			}
		}
	}

	private void ButtonUp()
	{ ///check if is placed
		second = Time.time;
		if ( second - first < 0.5f && hit)
		{
			//print("Click chiquito ");
			if (CatPressShort != null)
				CatPressShort();
		}else if ( second - first > 0.5f && hit)
		{
			//print("Click grande ");
			if(CatPressLong != null)
			CatPressLong();
		}
		hit = false;
	}


}
