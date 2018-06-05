using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State 
{
	IDLE, SHOOTING, DIE,
}

public class Cat : MonoBehaviour {

    public GameManager GM;

    public CatAttacker selfAtkr;

    public CatStats stats;

    public GameObject slot;
    public Transform canon;
    public GameObject bala;
    public List<GameObject> slots;
    public bool isPlaced;
    public float dis;

    public void Start() {
        stats = GetComponent<CatStats>();
        GM = GameManager.Instance;
        GM.GameOverTuto += CatStopAttackDog;
        GM.GameOver += CatStopAttackDog;
        slots = new List<GameObject>();
        if (GetComponent<Transform>().childCount>2)
        {
            if (transform.GetChild(2).GetComponent<CatAttacker>() != null)
            {
                selfAtkr = transform.GetChild(2).GetComponent<CatAttacker>().Instance;
                selfAtkr.DogOnSight += CatAttackDog;
                selfAtkr.NoHayChuchos += CatStopAttackDog;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Slot"))
        {
            slots.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Slot")) {
            slots.Remove(col.gameObject);
        }
    }

    public virtual bool SeekSlot()
    {
        isPlaced = false;
        if (GM.sliderelixir.value >= stats.exilir.GetValue() && slots.Count != 0) {
            for (int i = 0; i < slots.Count; i++)
            {
                if (Vector3.Distance(slots[i].transform.position, transform.position) < dis && slots[i].GetComponent<Slot>().isEmpty == true)
                {
					Vector3 slotPos = slots[i].transform.position;
					Vector3 newPos = new Vector3(slotPos.x, slotPos.y, transform.position.z);
					transform.SetParent(slots[i].transform);
					transform.position = newPos;
                    slot = slots[i];
                    slot.GetComponent<Slot>().isEmpty = false;
                    isPlaced = true;
                  	slots.Clear();
                    return isPlaced;
                }
            }
        }
        else if (GM.sliderelixir.value < stats.exilir.GetValue())
        {
            GM.typeSente("Ya no puedes poner gatos :c");
        }
        return isPlaced;
    } //busca un slot donde ponerse

    public void CatAttackDog(GameObject doggo) {
		if(stats.estado == State.IDLE && isPlaced == true){
            //print ("Gato atacando al doggo");
            StartCoroutine(Ataque());
		}
	}/// El gato detecta cuando el perro esta en el rango y ataca al perro

    public void CatStopAttackDog()
    {
        if (isPlaced == true && stats.estado == State.SHOOTING)
        {
            stats.estado = State.IDLE;
            StopCoroutine(Ataque());

        }
    }/// El gato detecta cuando el perro no esta en el rango y deja de disparar


	public virtual void Shoot(){
		GameObject pro = Instantiate(bala, canon.position, Quaternion.identity);
		if (pro.GetComponent<Bullet>() != null)
		{
			pro.GetComponent<Bullet>().damage = stats.GetDamage2Do();
			pro.GetComponent<Bullet>().type2Damage = stats.tipoAtk;
		}
	}

	private void OnDisable()
	{
		if (slot != null)
		{
            slot.GetComponent<Slot>().isEmpty = true;
            GM.RemoverGatos(gameObject, slot.transform.parent.name);
        }
        if(selfAtkr!= null)
        {
            selfAtkr.DogOnSight -= CatAttackDog;
            selfAtkr.NoHayChuchos -= CatStopAttackDog;  
        }
		GM.GameOver -= StopShooting;
		GM.GameOverTuto -= StopShooting;
	}

    public void StopShooting()
    {
        stats.anim.SetBool("isAtk", true);
        StopCoroutine(Ataque());
    }

    public IEnumerator Ataque()
    {
        stats.estado = State.SHOOTING;
        stats.anim.SetBool("isAtk", true);
        while (stats.estado == State.SHOOTING)
        {
            yield return new WaitForSeconds(stats.atkRate);

            GameObject pro = Instantiate(bala, canon.position, Quaternion.identity);
            pro.GetComponent<Bullet>().SetBullet(stats.GetDamage2Do(), stats.tipoAtk, stats.atkRate); //TODO: replace atk rate with speedBull

            yield return new WaitForSeconds(stats.atkCoolDown);
        }
        stats.anim.SetBool("isAtk", false);
    }

}