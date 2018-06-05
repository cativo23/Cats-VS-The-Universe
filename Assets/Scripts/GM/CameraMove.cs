using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
	public static CameraMove Instance;
	public GameObject HERO;
	public bool isMoving;
    public Transform lastPos;

    WaveManager WM;

	public delegate void CameraMoved(Vector3 pos);
	public event CameraMoved Moved;

	private void Awake()
	{
		Instance = this;
	}

    public void Start()
    {
        WM = WaveManager.Instance;
        WM.StartLastWave += MoveToLastPos;
    }

	IEnumerator Moves(Vector3 pos1, Vector3 pos2, float duration)
	{
		Moved(pos2);
		isMoving = true;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			transform.position = Vector3.Lerp(pos1, pos2, t / duration);
			yield return 0;
		}
		transform.position = pos2;
		isMoving = false;//al final la posicion tiene que ser la posicion2
	}

	public void MoveCam()
	{
		Vector3 newPos = transform.position;
		newPos.x *= -1;
		if (isMoving == false)
		{
			StartCoroutine(Moves(transform.position, newPos, 0.5f));
			////Vector3 pos = HERO.transform.position;
		//	Vector3 newpos = HERO.transform.position;
		//	newpos.x *= 1;
		}
	}

    public void  MoveToLastPos()
    {
        if (isMoving == false)
        {
            lastPos.position = new Vector3(lastPos.position.x, lastPos.position.y, transform.position.z);
            StartCoroutine(Moves(transform.position, lastPos.position, 1f));
        }
    }

}