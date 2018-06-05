using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour {
    GameManager GM;
    public float firstTouch;
    public float lastTouch;
    public float swipeDis;
	Spawner SP;

    public static SwipeManager Instance;

    public delegate void OnSwipeUp();
    public delegate void OnSwipeDown();

    public event OnSwipeDown MoveDown;
    public event OnSwipeUp MoveUp;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
     
        GM = GameManager.Instance;
        GM.Clicked += OnPressed;
        GM.Released += OnReleased;
		SP = Spawner.Instance;
	}

    public  void OnPressed(RaycastHit2D[] hits)
    {
        firstTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    }

    public void OnReleased()
    {
        lastTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        if (Mathf.Abs(lastTouch - firstTouch) > swipeDis && SP.cat2Spawn ==null)
		{
			if (lastTouch - firstTouch > 0){
                if(MoveUp != null)
                    MoveUp();
            }
            else
            {
                if (MoveDown != null)
                    MoveDown();
            }
        }
    }
}
