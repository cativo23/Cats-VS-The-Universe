using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadSprite : MonoBehaviour
{
    private Renderer rend;//se obtiene el rendered del quad
    public float speed; // velocidad con que se va a mover
	// Use this for initialization
	void Start ()
    {
        rend = gameObject.GetComponent<Renderer>(); //se obtiene el renderer
	}
	
	// Update is called once per frame
	void Update ()
    {
        float offSet = Time.time * speed; //se calcula el offset
        rend.material.SetTextureOffset("_MainTex", new Vector2(offSet, 0)); //se aplica el offset

	}
}
