using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttacker : MonoBehaviour {
    public CatAttacker Instance;
   public GameManager GM;

    public List<GameObject> Enemies;

	public delegate void OnDogEnterZone (GameObject Dog);
	public delegate void OnDogOut ();
	public event OnDogEnterZone DogOnSight;
	public event OnDogOut NoHayChuchos;

    private void Start()
    {
        Enemies = new List<GameObject>();
        GM = GameManager.Instance;
        GM.DogDestroyed += SalioPerro;
    }

    public void OnEnable(){
		Instance = this;
	}

	public void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("Enemy")){
			if (DogOnSight != null)
			{
				DogOnSight(col.gameObject);
                Enemies.Add(col.gameObject);
            }
        }
	}

   public void SalioPerro(GameObject dog)
    {
        if (Enemies.Contains(dog))
        {
            Enemies.Remove(dog);
            if (Enemies.Count == 0 && NoHayChuchos != null)
            {
                NoHayChuchos();
			}
        }
    }
}
