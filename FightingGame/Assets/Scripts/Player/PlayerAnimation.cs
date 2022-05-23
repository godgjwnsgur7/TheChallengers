using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

// 페어로 묶을지 고민중 임시 + 미사용
public class AnimatorPair
{
    Animator anim;
    SpriteRenderer sprite;
}

/// <summary>
/// bow - [0][1](Side), [2][3](Down), [4](Up) / 
/// gun - [0][1](Side), [2](Down), [3](Up) / 
/// rifle - [0][1](Side), [2](Down), [3](Up)
/// </summary>
public class FarWeaponSprites
{
    public Sprite[] bowSprites = new Sprite[5];
    public Sprite[] gunSprites = new Sprite[4];
    public Sprite[] rifleSprites = new Sprite[4];
}

public class PlayerAnimation : MonoBehaviour
{
    // AnimatorPair bodyPair = new AnimatorPair();

    public Animator bodyAnim;
    public Animator weaponAnim;
    public Animator coverAnim;

    public SpriteRenderer bodySpriteRender;
    public SpriteRenderer weaponSpriteRender;
    public SpriteRenderer coverSpriteRender; // 미사용중

    public FarWeaponSprites farWeaponSprites = new FarWeaponSprites();

    private bool reverseState = false;

    public void Init(ENUM_CHARACTER_TYPE charType)
    {
        farWeaponSprites = Managers.Resource.GetWeaponSprites(charType);

        GetObjectInfo();

        weaponSpriteRender.sprite = farWeaponSprites.bowSprites[1];
    }

    private void GetObjectInfo()
    {
        GameObject g;

        g = gameObject.transform.Find("Body").gameObject;
        bodyAnim = g.GetComponent<Animator>();
        bodySpriteRender = g.GetComponent<SpriteRenderer>();

        g = gameObject.transform.Find("Weapon").gameObject;
        weaponAnim = g.GetComponent<Animator>();
        weaponSpriteRender = g.GetComponent<SpriteRenderer>();

        g = gameObject.transform.Find("Cover").gameObject;
        coverAnim = g.GetComponent <Animator>();
        coverSpriteRender = g.GetComponent<SpriteRenderer>();
    }
    
    public void SetSprites(ENUM_WEAPON_TYPE weaponType)
    {
        Vector2 vec = GetVector();

        switch (weaponType)
        {
            case ENUM_WEAPON_TYPE.Bow:
                SetBowSprites(vec);
                break;
            case ENUM_WEAPON_TYPE.Gun:
                SetGunSprites(vec);
                break;
            case ENUM_WEAPON_TYPE.Rifle:
                SetRifleSprites(vec);
                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }

    public void SetIdle()
    {
        if (weaponSpriteRender.sprite != null)
            weaponSpriteRender.sprite = null;
    }

    private void SetBowSprites(Vector2 vec)
    {
        if (vec.x != 0)
        {
            ReverseSprites(vec.x);
            bodySpriteRender.sprite = farWeaponSprites.bowSprites[0];
            weaponSpriteRender.sprite = farWeaponSprites.bowSprites[1];
        }
        else if (vec.y < -0.9f)
        {
            bodySpriteRender.sprite = farWeaponSprites.bowSprites[2];
            weaponSpriteRender.sprite = farWeaponSprites.bowSprites[3];
        }
        else if (vec.y > 0.9f)
            bodySpriteRender.sprite = farWeaponSprites.bowSprites[4];
        else
            Debug.Log("벗어남");
    }

    private void SetGunSprites(Vector2 vec)
    {
        if (vec.x != 0)
        {
            ReverseSprites(vec.x);
            bodySpriteRender.sprite = farWeaponSprites.gunSprites[0];
            weaponSpriteRender.sprite = farWeaponSprites.gunSprites[1];
        }
        else if (vec.y < 0.9f)
            bodySpriteRender.sprite = farWeaponSprites.gunSprites[2];
        else if (vec.y > 0.9f)
            bodySpriteRender.sprite = farWeaponSprites.gunSprites[3];
        else
            Debug.Log("벗어남");
    }

    private void SetRifleSprites(Vector2 vec)
    {
        if (vec.x != 0)
        {
            ReverseSprites(vec.x);
            bodySpriteRender.sprite = farWeaponSprites.rifleSprites[0];
            weaponSpriteRender.sprite = farWeaponSprites.rifleSprites[1];
        }
        else if (vec.y < 0.9f)
            bodySpriteRender.sprite = farWeaponSprites.rifleSprites[2];
        else if (vec.y > 0.9f)
            bodySpriteRender.sprite = farWeaponSprites.rifleSprites[3];
        else
            Debug.Log("벗어남");
    }

    public void SetVector(Vector2 vec, bool isRun)
    {
       ReverseSprites(vec.x);

        float f = isRun ? 2.0f : 1.0f;
        bodyAnim.SetFloat("DirX", vec.x * f);
        bodyAnim.SetFloat("DirY", vec.y * f);

        weaponAnim.SetFloat("DirX", vec.x);
        weaponAnim.SetFloat("DirY", vec.y);

        coverAnim.SetFloat("DirX", vec.x);
        coverAnim.SetFloat("DirY", vec.y);
    }

    public Vector2 GetVector()
    {
        return new Vector2(bodyAnim.GetFloat("DirX"), bodyAnim.GetFloat("DirY"));
    }

    private void ReverseSprites(float vecX)
    {
        bool _reverseState = (vecX > 0.9f);

        if (reverseState == _reverseState)
            return;

        bodySpriteRender.flipX = _reverseState;
        weaponSpriteRender.flipX = _reverseState;
        coverSpriteRender.flipX = _reverseState;
        reverseState = _reverseState;
    }

    public void SetFloat(string str, float value)
    {
        bodyAnim.SetFloat(str, value);
        weaponAnim.SetFloat(str, value);
        coverAnim.SetFloat(str, value);
    }

    public void SetInteger(string str, int value)
    {
        bodyAnim.SetInteger(str, value);
        weaponAnim.SetInteger(str, value);
        coverAnim.SetInteger(str, value);
    }

    public void SetBool(string str, bool value)
    {
        bodyAnim.SetBool(str, value);
        weaponAnim.SetBool(str, value);
        coverAnim.SetBool(str, value);
    }

    public void SetTrigger(string str)
    {
        bodyAnim.SetTrigger(str);
        weaponAnim.SetTrigger(str);
        coverAnim.SetTrigger(str);
    }

    public bool GetBool(string str)
    {
        return bodyAnim.GetBool(str);
    }

    public int GetInteger(string str)
    {
        return bodyAnim.GetInteger(str);
    }
}