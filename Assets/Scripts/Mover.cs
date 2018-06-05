using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float posY;
    public float posX;
	public float speed; ///inherit from stats class
    public float posZ;
    public List<float> SlotHY;
	public List<float> SlotHX;
    public Transform lastPos;
	
	HeroStats stats;
    public float moveForce = 5;

	CameraMove CM;
    WaveManager WM;
    GameManager GM;

	public bool isMoving;

    SwipeManager SM;

    public delegate void MoveChar( HeroStats stats, float force);
    public event MoveChar OnMoveJoystick;

    private void Start()
    {
        GM = GameManager.Instance;
        WM = WaveManager.Instance;
        WM.StartLastWave += ChangeMode;
		SM = SwipeManager.Instance;
		SM.MoveDown += MoverAbajo;
		SM.MoveUp += MoverArriba;
		
		CM = CameraMove.Instance;
		CM.Moved += MoverAPs;
		stats = GetComponent<HeroStats>();
        posY = transform.position.y;
        posX = transform.position.x;
        posZ = transform.position.z;
        foreach (var item in GameObject.FindGameObjectsWithTag("SlotHero"))
        {
            SlotHY.Add(item.transform.position.y);
			SlotHX.Add(item.transform.position.x);

		} //obtiene todos los slots de heroe
        SlotHY.Sort(); //los ordena de menos a mayor
		SlotHX.Sort();
		
    }

    public void MoverArriba()
    {
		if ( !isMoving) {
			if (Camera.main.transform.position.x > 0)
			{
				for (int i = 0; i < SlotHY.Count; i++)
				{
					if (posY == SlotHY[i] && i + 1 <= SlotHY.Count - 1)
					{
						posY = SlotHY[i + 1];
						posX = SlotHX[i + 1];
						StartCoroutine(Move(transform.position, new Vector3(SlotHX[i + 1], SlotHY[i + 1], posZ), 1f / stats.speed.GetValue()));
						return;
					}
				}
			}
			else if (Camera.main.transform.position.x < 0)
			{
				for (int i = 0; i < SlotHY.Count; i++)
				{
					if (posY == SlotHY[i] && i + 1 <= SlotHY.Count - 1)
					{
						posY = SlotHY[i + 1];
						posX = -SlotHX[i + 1];
						StartCoroutine(Move(transform.position, new Vector3(-SlotHX[i + 1], SlotHY[i + 1], posZ), 1f / stats.speed.GetValue()));
						return;
					}
				}
			}
		}
		
    }
    public void MoverAbajo()
    {

		if (isMoving == false)
		{
			if (Camera.main.transform.position.x > 0)
			{
				for (int i = 0; i < SlotHY.Count; i++)
				{
					if (posY == SlotHY[i] && i - 1 >= 0)
					{
						posY = SlotHY[i - 1];
						posX = SlotHX[i - 1];
						StartCoroutine(Move(transform.position, new Vector3(SlotHX[i - 1], SlotHY[i - 1], posZ), 1f / stats.speed.GetValue()));
						return;
					}
				}
			}
			else if (Camera.main.transform.position.x < 0)
			{
				for (int i = 0; i < SlotHY.Count; i++)
				{

					if (posY == SlotHY[i] && i - 1 >= 0)
					{
						posY = SlotHY[i - 1];
						posX = -SlotHX[i - 1];
						StartCoroutine(Move(transform.position, new Vector3(-SlotHX[i - 1], SlotHY[i - 1], posZ), 1f / stats.speed.GetValue()));
						return;
					}
				}
			}
		}
		
    }


    IEnumerator Move(Vector3 pos1, Vector3 pos2, float duration)
    {
		isMoving = true;
		for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2,  t / duration);
            yield return 0;
        }
        transform.position = pos2;
		isMoving = false;//al final la posicion tiene que ser la posicion2
	}

	public void MoverAPs(Vector3 camPos)
	{
        if(camPos != lastPos.position)
        {
            Vector3 position = transform.position;
            StartCoroutine(Move(position, new Vector3(position.x * -1, position.y, posZ), 1f / stats.speed.GetValue()));
            Vector3 catScale = transform.localScale;
            catScale.x *= -1;
            transform.localScale = catScale;
        }
        else
        {
            Vector3 nuipos = new Vector3(lastPos.position.x, lastPos.position.y-1.5f, transform.position.z);
            StartCoroutine(Move(transform.position, nuipos, 5f / stats.speed.GetValue()));
        }
	}

    public void ChangeMode()
    {
        SM.MoveDown -= MoverAbajo;
        SM.MoveUp -= MoverArriba;
        stats.rb2D.bodyType = RigidbodyType2D.Dynamic;
        stats.rb2D.gravityScale = 0;
        stats.rb2D.drag = 1f;
        stats.rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;        
    }

    private void FixedUpdate()
    {
        if (OnMoveJoystick != null)
            OnMoveJoystick(stats, moveForce);
    }
}
