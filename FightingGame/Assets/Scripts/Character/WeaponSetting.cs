using FGDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetting : MonoBehaviour
{
    private GameObject bulletGo, weaponGo;

    private SpriteRenderer effectSpriteRender;
    private Sprite[] sprites;

    private CircleCollider2D weaponCirCollider;
    private Animator anim;
    private Rigidbody2D rigid;

    // currentSetPosition Value {downX, downY, upX, upY, side}
    private float[] currSetPos = new float[5];
    private float[] gunPosition, riflePosition, bowPosition;

    // offset Value {downY, sideX, upY, sideY,}
    private float[] curOffset = new float[4];
    private float[] hammerOffset, swordOffset, syctheOffset, weaponRadius;

    public void init()
    {
        sprites = Resources.LoadAll<Sprite>("Art/BulletEffect/");
        effectSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        weaponGo = gameObject.transform.parent.Find("Weapon").gameObject;
        anim = weaponGo.GetComponent<Animator>();

        // 원거리무기 Position Value Base
        bowPosition = new float[5] { 0.0f, 0.1f, 0.0f, 1.0f, 0.4f};
        gunPosition = new float[5] { 0.04f, -0.4f, -0.04f, 1.9f, 1.0f };
        riflePosition = new float[5] { 0.05f, -0.5f, -0.05f, 2.0f, 1.25f };

        // 근거리무기 Radius, Offset Value Base
        weaponRadius = new float[3] { 1f, 0.8f, 1f };
        hammerOffset = new float[4] { 0.2f, 0.6f, 1.1f, 0.6f };
        swordOffset = new float[4] { 0.3f, 0.5f, 1f, 0.8f };
        syctheOffset = new float[4] { 0f, 0.85f, 1.4f, 0.6f };
    }

    // 근거리무기 Collider 초기화
    public void SetWeaponCollider2D(ENUM_WEAPON_TYPE weaponType) 
    {
        weaponCirCollider = weaponGo.GetComponent<CircleCollider2D>();

        if (weaponCirCollider == null) { 
            weaponCirCollider = weaponGo.AddComponent<CircleCollider2D>();
        }

        weaponCirCollider.isTrigger = true;
        weaponCirCollider.enabled = false;

        weaponCirCollider.radius = weaponRadius[(int)weaponType - 1];

        if (weaponType == ENUM_WEAPON_TYPE.Hammer)
            curOffset = hammerOffset;
        else if (weaponType == ENUM_WEAPON_TYPE.Sword)
            curOffset = swordOffset;
        else if (weaponType == ENUM_WEAPON_TYPE.Sycthe)
            curOffset = syctheOffset;
    }

    // 근거리무기 Collider Offset 설정 & Enable
    public void enabledWeapon(Vector2 vec)
    {
        vec = SignChk(vec);

        int x = (int)vec.x;
        int y = (int)vec.y;

        if (y != 0)
            weaponCirCollider.offset = new Vector2(0, curOffset[y + 1]);
        else if (x != 0)
            weaponCirCollider.offset = new Vector2(x * curOffset[1], curOffset[3]);

        weaponCirCollider.enabled = true;
    }

    // 총기류 이펙트 설정
    public void SetWeaponEffect(ENUM_WEAPON_TYPE weaponType) 
    {
        if (weaponType == ENUM_WEAPON_TYPE.Gun)
        {
            effectSpriteRender.sprite = sprites[0];
            currSetPos = gunPosition;
        }
        else if (weaponType == ENUM_WEAPON_TYPE.Rifle)
        {
            effectSpriteRender.sprite = sprites[1];
            currSetPos = riflePosition;
        }
        else if (weaponType == ENUM_WEAPON_TYPE.Bow) 
        {
            currSetPos = bowPosition;    
        }
    }

    // 캐릭터 방향에 따른 이펙트 위치와 각도 조정
    public void SetEffectPosition(Vector2 vec) 
    {
        vec = SignChk(vec);

        if (vec.y != 0)
        {
            int y = (int)vec.y;
            float rotateZ = (y <= 0) ? 90 : -90;
            
            this.transform.localPosition = new Vector2(currSetPos[y + 1], currSetPos[y + 2]);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateZ));
        }
        else if (vec.x != 0)
        {
            int x = (int)vec.x;
            float rotateY = (x <= 0) ? 0 : 180;

            this.transform.localPosition = new Vector2(x * currSetPos[4], 0.5f);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, rotateY, 0));
        }
    }

    // 총알 발사
    public void Shot(ENUM_WEAPON_TYPE weaponType) 
    {
        bulletGo = Managers.Resource.Instantiate(weaponType.ToString(), null);
        rigid = bulletGo.GetComponent<Rigidbody2D>();

        bulletGo.transform.position = this.transform.position;
        bulletGo.transform.rotation = this.transform.rotation;

        Vector2 vec = new Vector2(anim.GetFloat("DirX"), anim.GetFloat("DirY"));
        vec = SignChk(vec) * 0.1f;

        if (vec.y != 0f)
        {
            rigid.AddForce(new Vector2(0, vec.y), ForceMode2D.Force);
        }
        else if (vec.x != 0f)
        {
            rigid.AddForce(new Vector2(vec.x, 0), ForceMode2D.Force);
        }
    }

    // 벡터 부호 확인, 벡터 변경
    private Vector2 SignChk(Vector2 vec)
    { 
        vec.x = Math.Sign(vec.x);
        vec.y = Math.Sign(vec.y);
        return vec;
    }
}
