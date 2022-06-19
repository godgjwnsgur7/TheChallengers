using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletShot
{
    GameObject Bg;
    GameObject effectGo;
    Rigidbody2D rigid;
    Animator anim;

    public void init(string bulletName, GameObject go) 
    {
        // 발사 세팅
        Bg = Managers.Resource.Instantiate(bulletName, go.transform);
        effectGo = go.transform.parent.Find("Effect").gameObject;
        anim = go.GetComponent<Animator>();
        rigid = Bg.GetComponent<Rigidbody2D>();
    }

    public void shotBullet()
    {
        // 총알 Position 세팅
        SetBulletPosition();

        // 발사
        ShotBullet();

        /*await Task.Delay(200);

        Destroy();*/
    }

    private void SetBulletPosition()
    {
        Bg.transform.localPosition = new Vector2(effectGo.transform.localPosition.x, effectGo.transform.localPosition.y);
        Bg.transform.rotation = effectGo.transform.rotation;
    }

    private void ShotBullet()
    {
        if (anim.GetFloat("DirY") == 1f)
        {
            rigid.AddForce(new Vector2(0, 0.1f), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirY") == -1f)
        {
            rigid.AddForce(new Vector2(0, -0.1f), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirX") == 1f)
        {
            rigid.AddForce(new Vector2(0.1f, 0), ForceMode2D.Force);
        }
        else if (anim.GetFloat("DirX") == -1f)
        {
            rigid.AddForce(new Vector2(-0.1f, 0), ForceMode2D.Force);
        }
    }

    public void Destroy() 
    {
        if(Bg == null) 
        {
            return;
        }

        Managers.Resource.Destroy(Bg);
    }
}
