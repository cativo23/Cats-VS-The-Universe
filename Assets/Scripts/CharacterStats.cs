using UnityEngine;
using System.Collections;

public enum CharacType
{
	ATK,
	DEF,
	SPL,
	HER,
	ALL,
	WALL,
}

public class CharacterStats : MonoBehaviour {
    //Salud
    AttackManager AM;
    public int maxHealth = 100;
    public State estado = State.IDLE;
	public int currentHealth { get; private set; }

	//Ataque
	public int maxDamage = 100;
	public float atkRate;
    public float atkCoolDown;
	public CharacType tipoAtk;

	//componentes del gameobject
	public AudioSource sound; 
	public Rigidbody2D rb2D;
	public Animator anim;

	//Elixir
	public Stat exilir;

	//Tipo de gato
	public CharacType tipo;

	public Stat damage;
	public Stat armor;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        AM = AttackManager.Instance;
        AM.OnBulletDamage += TakeDamage;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			TakeDamage(gameObject, 10, CharacType.ALL);
		}
	}

    private void OnDisable()
    {
        AM.OnBulletDamage -= TakeDamage;
    }


    public void TakeDamage(GameObject _obj, int _damage, CharacType _type) {
        if (_obj == gameObject)
        {
           
            if (_type == tipo || _type == CharacType.ALL)
            {
                _damage -= armor.GetValue();
                _damage = Mathf.Clamp(_damage, 0, int.MaxValue);

                currentHealth -= _damage;
               // g.Log(transform.name + " takes " + _damage + " damage.");
                if (currentHealth <= 0 && estado != State.DIE)
                {
                    Die();
                }
            }
        }
	}

	public int GetDamage2Do()
	{
		return damage.GetValue();
	}

	public virtual void Die()
	{
        estado = State.DIE;
        if (anim)
        {
            StartCoroutine(ShowDestroy());
        }
        Destroy(gameObject, 1f);
	}

    public IEnumerator ShowDestroy()
    {
        anim.SetBool("isDestroy", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isDestroy", false);
    }
}
