using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

	public GameManager GM;

	public static Attacker Instance;
	public List<Line> InstanciasFilas;

	void Awake(){
		Instance = this;
	}

	void Start(){
		GM = GameManager.Instance;


		foreach (GameObject item in GameObject.FindGameObjectsWithTag ("Fila")) {
			InstanciasFilas.Add (item.GetComponent<Line>().Instance);
		}
		foreach (Line item in InstanciasFilas) {
			item.EntroEnemigo += DogIn;
			item.SalioEnemigo += DogOut;
		}
	}

	public void DogOut(GameObject dog, string fila){
		//print ("El perro: "+dog.name + " salio de la fila: "+ fila + "deja de DISPARAR!!!");
		GM.RemoverPerros (dog, fila);
	}
	public void DogIn(GameObject dog, string nameFila){
      //  print ("El perro: "+dog.name + " entro, en la fila: "+ nameFila + "DISPARAR!!!");
        GM.AgregarPerros(dog, nameFila);
	}
}
