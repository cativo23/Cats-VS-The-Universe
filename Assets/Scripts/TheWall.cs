using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWall : MonoBehaviour {
    AttackManager AM;
    GameManager GM;
	public int vida;
	bool isDamaged = false;

	public CharacType tipo;

	public Stat armor;

	private void Start()
    {

        AM = AttackManager.Instance;
        AM.OnBulletDamage += TakeDamage;
        GM = GameManager.Instance;
    }

    private void OnDisable()
    {
        AM.OnBulletDamage -= TakeDamage;
        GM.DestroyWall(gameObject);
    }

    public void TakeDamage(GameObject _obj, int _damage, CharacType _type)
    {
        _type = CharacType.WALL;
        if (_obj == gameObject)
        {
            if (_type == CharacType.WALL || _type == CharacType.ALL)
            {
                _damage -= armor.GetValue();
                _damage = Mathf.Clamp(_damage, 0, int.MaxValue);

                vida -= _damage;
                //Debug.Log(transform.name + " takes " + _damage + " damage.");
                if (vida <= 0)
                {
                    Die();
                }
                if (vida <= 50 && isDamaged == false)
                {
                    isDamaged = true;
                    GM.WallIsDamaged();
                }
            }
        }
    }
	public void Die()
	{
		gameObject.SetActive(false);
		Destroy(gameObject, 1f);
	}
}
