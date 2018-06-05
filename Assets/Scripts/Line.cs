using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {
	public Line Instance;


	public delegate void OnEnemyEnter(GameObject go, string name);
	public delegate void OnEnemyOut (GameObject go, string name);
	public OnEnemyEnter EntroEnemigo;
	public OnEnemyOut SalioEnemigo;

	public void Awake(){
		Instance = this;
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("Enemy")){
			if(EntroEnemigo!=null)
			EntroEnemigo (col.gameObject, this.gameObject.name);
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if(col.CompareTag("Enemy")){
			if(SalioEnemigo !=null)
				SalioEnemigo(col.gameObject, this.gameObject.name); // mandar info de la fila tambien
		}
	}
}
