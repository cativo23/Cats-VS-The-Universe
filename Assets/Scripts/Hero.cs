using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Hero : Cat{
    Mover Move;
    SpecialAction SA;
    WaveManager WM;
    new void Start()
    {
        Move = GetComponent<Mover>();
        WM = WaveManager.Instance;
        WM.StartLastWave += ChangeMode;
        base.Start();
        SA = GetComponent<SpecialAction>();
        SA.CatPressShort += Shoot;
    }


    public override bool SeekSlot()
    {
        return false;
    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetButton("Shoot"))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        StartCoroutine(Shoots());
    }
    IEnumerator Shoots()
    {
        if (stats.estado == State.IDLE && stats.anim.GetBool("isAtk") == false)
        {
            stats.anim.SetBool("isAtk", true);
            yield return new WaitForSeconds(stats.atkRate);
            stats.estado = State.SHOOTING;
            GameObject pro = Instantiate(bala, canon.position, Quaternion.identity);
            pro.GetComponent<Bullet>().SetBullet(stats.GetDamage2Do(), stats.tipoAtk, stats.atkRate);
            yield return new WaitForSeconds(stats.atkCoolDown);
            stats.estado = State.IDLE;
            stats.anim.SetBool("isAtk", false);
        }
    }
    public void ChangeMode()
    {
        SA.CatPressShort -= Shoot;
        Move.OnMoveJoystick += MoveAround;
    }

    public void MoveAround(HeroStats stats, float force)
    {
        Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * force;
        stats.rb2D.AddForce(moveVec);
    }


}
