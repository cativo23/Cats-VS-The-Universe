using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	//Instance
	public static Spawner Instance;
    public bool isMovingCat;

	/// 
	/// GameManager
	///
	GameManager GM;


	//variables
	public List<GameObject> cats2Spawn;
	public GameObject cat2Spawn;
	public GameObject catCreated;

	//Delegates declarations
	public delegate void OnCatPlaced(GameObject cat, string fila);	
	//Events Declarations
	public event OnCatPlaced CatPlaced;

	private void Awake()
	{
		Instance = this;
	}


	private void Start()
	{
		cat2Spawn = null;
		GM = GameManager.Instance;
		GM.Clicked += OnButtonSelected;
		GM.Pressed += MoverGato;
		GM.Released += PonerGato;
	}

	public void OnButtonSelected( RaycastHit2D[] hits) ///Si se da click en el mismo botton quitar gato
	{
		Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		wp.z = 1;
		
		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].collider.CompareTag("SelectCatBtn"))
			{
				isMovingCat = true;
				switch (hits[i].collider.name)
				{
					case "Cat0":
						cat2Spawn = cats2Spawn[0];
						break;
					case "Cat1":
						cat2Spawn = cats2Spawn[1];
						break;
					case "Cat2":
						cat2Spawn = cats2Spawn[2];
						break;
				}
				if (cat2Spawn)
				{
					catCreated = Instantiate(cat2Spawn, wp, Quaternion.identity);
					if (catCreated.transform.position.x < 0 && catCreated.transform.localScale.x>0)
					{
						Vector3 theScale = catCreated.transform.localScale;
						theScale.x *= -1;
						catCreated.transform.localScale = theScale;
					}
				}
				return;
			}
			else if (cat2Spawn != null && (hits[i].collider.CompareTag("Plataforma") || hits[i].collider.CompareTag("Slot") || hits[i].collider.CompareTag("Fila")))
			{
                isMovingCat = true;
                catCreated = Instantiate(cat2Spawn, wp, Quaternion.identity);
				if (catCreated.transform.position.x < 0 && catCreated.transform.localScale.x > 0)
				{
					Vector3 theScale = catCreated.transform.localScale;
					theScale.x *= -1;
					catCreated.transform.localScale = theScale;
				}
				return;
			}
		}
	}

	public void MoverGato()
	{
		if (catCreated)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = -5;
			catCreated.transform.position = mousePos;
			if (catCreated.transform.position.x < 0 && catCreated.transform.localScale.x > 0)
			{
				Vector3 theScale = catCreated.transform.localScale;
				theScale.x *= -1;
				catCreated.transform.localScale = theScale;
			}
			if (catCreated.transform.position.x >0 && catCreated.transform.localScale.x < 0)
			{
				Vector3 theScale = catCreated.transform.localScale;
				theScale.x *= -1;
				catCreated.transform.localScale = theScale;
			}
		}
	}
	public void PonerGato()
	{
		
		if (catCreated)
		{
			bool res = catCreated.GetComponent<Cat>().SeekSlot();
		//	print (res);
			if (res)
			{
				CatPlaced(catCreated, catCreated.GetComponent<Cat>().slot.transform.parent.name);
				catCreated = null;
				cat2Spawn = null;
				//print("buenas vibras 2 " + Time.time);
			}
			else
			{
				catCreated.SetActive(false);
			//	print("Deberia de morir");
				Destroy (catCreated);
				catCreated = null;
                cat2Spawn = null;
			
			}
		}
		isMovingCat = false;

	}



}
