using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    AttackManager AM;

	public int damage;
	public CharacType type2Damage;
	public object[] args;
	Rigidbody2D rb;
	public float speed;
	// Use this for initialization
	void Start()
	{
        AM = AttackManager.Instance;
		rb = GetComponent<Rigidbody2D>();
		if(transform.position.x> 0 || transform.position.y > 2.5f)
		{
			rb.velocity = new Vector2(speed, 0);

		}
		else if (transform.position.x < 0 && transform.position.y < 2.5f)
		{
			rb.velocity = new Vector2(-speed, 0);

		}
		Destroy(gameObject, 3f);
	}


	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			args = new object[] { damage, type2Damage };
            AM.DoDamage(col.gameObject, damage, type2Damage);
			Destroy(gameObject);
		}
	}

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Plataforma"))
        {
            Destroy(gameObject, 0.1f);
        }
    }

    public void SetBullet(int _damage, CharacType type, float speed)
    {
        damage = _damage;
        type2Damage = type;
    }
}
