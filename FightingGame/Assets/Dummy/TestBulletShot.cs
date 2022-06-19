using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletShot : MonoBehaviour
{
    GameObject bulletGo;
    GameObject effectGo;
    Animator anim;
    Rigidbody2D rigid;
    

    void Awake()
    {
        effectGo = gameObject.transform.parent.Find("Effect").gameObject;
        anim = gameObject.GetComponent<Animator>();
    }

    public void init(string bulletName)
    {
        // 발사 세팅
        effectGo = gameObject.transform.parent.Find("Effect").gameObject;
        anim = gameObject.GetComponent<Animator>();
        bulletGo = Managers.Resource.Instantiate(bulletName, gameObject.transform);
        rigid = bulletGo.GetComponent<Rigidbody2D>();

        SetBulletPosition();

        ShotBullet();
        //Invoke("ShotBullet", 0.1f);

        //Invoke("Destroy", 0.35f);
    }

    private void SetBulletPosition()
    {
        bulletGo.transform.localPosition = new Vector2(effectGo.transform.localPosition.x, effectGo.transform.localPosition.y);
        bulletGo.transform.rotation = effectGo.transform.rotation;
    }

    public void ShotBullet()
    {
        Vector2 vec = new Vector2(anim.GetFloat("DirX"), anim.GetFloat("DirY"));
        vec.Normalize();

        rigid.AddForce(vec * 0.1f, ForceMode2D.Force);

        /*if (anim.GetFloat("DirY") >= 1f)
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
        }*/

        Invoke("Destroy", 0.25f);
    }

    private void Destroy()
    {
        if (bulletGo == null)
            return;

        Managers.Resource.Destroy(bulletGo);
    }
}
