using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public static AttackManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public delegate void BulletDamage(GameObject obj, int amount, CharacType type);
    public event BulletDamage OnBulletDamage;

    public void DoDamage(GameObject _obj, int _amount, CharacType _type)
    {
        if (OnBulletDamage != null)
        {
            OnBulletDamage(_obj, _amount, _type);
        }
    }


}
