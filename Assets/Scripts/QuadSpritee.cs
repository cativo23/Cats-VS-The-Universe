using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadSpritee : MonoBehaviour {

    private Renderer rend;
    public float speed;
    void Start () {
        rend = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        float offset = Time.time * speed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));//MainTex: nombre por defecto del parametro
    }
}
