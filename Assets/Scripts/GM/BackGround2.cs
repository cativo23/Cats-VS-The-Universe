using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround2 : MonoBehaviour {
//    public Transform[] backgrounds;
    public float speed;
 //   public float tilesizeZ;
    public float newPos;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        
        newPos += Time.deltaTime * speed;
        if (transform.position.x >= 30f)
        {
            transform.position = new Vector3(-51.75f, transform.position.y, 0);
            startPos = transform.position;
            newPos = 0;
        }
        else
        {
            transform.position = startPos + Vector3.right * newPos; 
        }
    }
} 
