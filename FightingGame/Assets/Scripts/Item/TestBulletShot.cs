using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletShot : MonoBehaviour
{
    GameObject bulletGo;
    GameObject effectGo;
    GameObject weaponGo;
    Animator anim;
    Rigidbody2D rigid;
    

    void Awake()
    {
        weaponGo = gameObject;
        effectGo = gameObject.transform.parent.Find("Effect").gameObject;
        anim = gameObject.GetComponent<Animator>();
    }

    public void init(string bulletName)
    {
        // 발사 세팅
        bulletGo = Managers.Resource.Instantiate(bulletName, weaponGo.transform);
        rigid = bulletGo.GetComponent<Rigidbody2D>();

        SetBulletPosition();

        Invoke("ShotBullet", 0.1f);

        Invoke("Destroy", 0.35f);
    }

    private void SetBulletPosition()
    {
        bulletGo.transform.localPosition = new Vector2(effectGo.transform.localPosition.x, effectGo.transform.localPosition.y);
        bulletGo.transform.rotation = effectGo.transform.rotation;
    }

    private void ShotBullet()
    {
        if (anim.GetFloat("DirY") >= 1f)
        {
            rigid.AddForce(new Vector2(0, 0.1f), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirY") <= -1f)
        {
            rigid.AddForce(new Vector2(0, -0.1f), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirX") >= 1f)
        {
            rigid.AddForce(new Vector2(0.1f, 0), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirX") <= -1f)
        {
            rigid.AddForce(new Vector2(-0.1f, 0), ForceMode2D.Force);
        }
    }

    public void Destroy()
    {
        if (bulletGo == null)
            return;

        Managers.Resource.Destroy(bulletGo);
    }
}
