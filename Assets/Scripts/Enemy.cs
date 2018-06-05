using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	Rigidbody2D rb;
    Attacker AT;
    AttackManager AM;
    GameManager GM;
    public GameObject currentCat;
    GameObject wall;
	EnemyStats stats;
    public State estado;
	public bool isQuitting = false;
	public bool isDestroying = false;

	//Info
	//private  float velocity;

	// Use this for initialization
	void Start () {
        AM = AttackManager.Instance;
		stats = GetComponent<EnemyStats>();
        estado = stats.estado;
        AT = Attacker.Instance;
		rb = GetComponent<Rigidbody2D> ();
		if (transform.position.x < 0)
		{
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
		Invoke("AddSpeed", stats.placeCooldown.GetValue());
		GM = GameManager.Instance;
        GM.CatDestroyed += CatIsDead;
        GM.WallDestroyed += WallIsDown;
		GM.GameOver += Stop;
		GM.GameOverTuto += Stop;
	}
	private void OnApplicationQuit()
	{
		isQuitting = true;
	}



	public void OnDisable()
	{
		isDestroying = true;
		AT.DogOut(gameObject, gameObject.transform.parent.name);
        GM.CatDestroyed -= CatIsDead;
		GM.WallDestroyed -= WallIsDown;
		GM.GameOver -= Stop;
		GM.GameOverTuto -= Stop;
		//print("isDestroy "+ isDestroying + " isQuitting: "+ isQuitting);
		if (isQuitting == false && isDestroying== true && stats.estado==State.DIE)
		{
			//print("no deberian de morir");
			GM.DropItem(transform);

		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag("Player") || col.CompareTag("TheWall") || col.CompareTag("Shield") )
		{

            switch (col.tag)
            {
                case "TheWall":
                    StartCoroutine(Ataque());
                    wall = col.gameObject;
                    break;
                case "Player":
                    if (col.gameObject.GetComponent<Cat>().isPlaced == true)
                    {
                        currentCat = col.gameObject;
                        StartCoroutine(Ataque());
                    }
                    break;
                case "Shield":
                    if(col.gameObject.GetComponentInParent<Cat>().isPlaced == true)
                    {
                        currentCat = col.gameObject.transform.parent.gameObject;
                        StartCoroutine(Ataque());
                    }
                    break;
                default:
                    break;
            }
		}
	}

    private void OnTriggerExit2D(Collider2D col)
    {
		if (col.CompareTag("Player") || col.CompareTag("TheWall") || col.CompareTag("Shield"))
		{
            switch (col.tag)
            {
                case "TheWall":
                    wall = null;
                    Invoke("AddSpeed", 0);
                    estado = State.IDLE;
                    StopCoroutine(Ataque());
                    break;
                case "Player":
                    if (col.gameObject.GetComponent<Cat>().isPlaced == true && col.gameObject == currentCat)
                    {
                        currentCat = null;
                        estado = State.IDLE;
                        Invoke("AddSpeed", 0);
                        StopCoroutine(Ataque());
                    }
                    break;
                case "Shield":
                    if (col.gameObject.GetComponentInParent<Cat>().isPlaced == true && col.gameObject.transform.parent.gameObject == currentCat)
                    {
                        currentCat = null;
                        estado = State.IDLE;
                        Invoke("AddSpeed", 0);
                        StopCoroutine(Ataque());
                    }
                    break;
                default:
                    break;
            }
		}
    }

    private void CatIsDead(GameObject cat)
    {
        if ( currentCat == cat)
        {
				currentCat = null;
				Invoke("AddSpeed", 0);
				StopCoroutine(Ataque());
				estado = State.IDLE;
        }
    }
    private void WallIsDown(GameObject _wall)
    {
        if (wall == _wall)
        {
			Invoke("AddSpeed", 0);
			estado = State.IDLE;
			StopCoroutine(Ataque());

		}
    }

/*	public void Attack()
	{
		if(currentCat != null)
		{
			object[] temp = new object[] { stats.GetDamage2Do(), stats.tipoAtk };
            stats.anim.SetBool("isAtk", true);
            currentCat.SendMessage("TakeDamage", temp);
            stats.anim.SetBool("isAtk", false);

        }
        else if (wall != null)
		{
			object[] temp = new object[] { stats.GetDamage2Do(), CharacType.WALL };
			wall.SendMessage("TakeDamage", temp);
		}
	}*/

    public IEnumerator Ataque()
    {
        rb.velocity = Vector2.zero;
        estado = State.SHOOTING;
        stats.anim.SetBool("isAtk", true);
        while (estado == State.SHOOTING)
        {
            yield return new WaitForSeconds(stats.atkRate);
            if (currentCat != null)
            {
                AM.DoDamage(currentCat, stats.GetDamage2Do(), stats.tipoAtk);

                //object[] temp = new object[] { stats.GetDamage2Do(), stats.tipoAtk };
               // currentCat.SendMessage("TakeDamage", temp);
            }
            else if (wall != null)
            {
                AM.DoDamage(wall, stats.GetDamage2Do(), stats.tipoAtk);
                //object[] temp = new object[] { stats.GetDamage2Do(), CharacType.WALL };
                //wall.SendMessage("TakeDamage", temp);
            }
            yield return new WaitForSeconds(stats.atkCoolDown);
        }
        stats.anim.SetBool("isAtk", false);
    }

	public void AddSpeed()
	{
		if (transform.position.x > 0)
		{
			rb.velocity = new Vector2(-stats.speed.GetValue(), 0f);
		}
		else
		{
			rb.velocity = new Vector2(stats.speed.GetValue(), 0f);
		}
	}

	public void DestroyMe()
	{
		Destroy(gameObject);
	}

	public void Stop()
	{
		rb.velocity = Vector2.zero;
	}
}
