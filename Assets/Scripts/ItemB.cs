using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemB : MonoBehaviour
{
    //private AudioManager audi;
	public float prob;
    public int quantity;
    public int temp;
	GameManager GM;
    // Use this for initialization
    void Start()
    {
		GM = GameManager.Instance;
       // audi = AudioManager.instance;
		quantity = Random.Range(0, temp);
		Destroy(gameObject, 2f);
    }


	private void OnDisable ()
    {
		if(GM != null)
		GM.ItemIsDestroyed(quantity);
      //  audi.PlaySound(this.name);
    }
}
