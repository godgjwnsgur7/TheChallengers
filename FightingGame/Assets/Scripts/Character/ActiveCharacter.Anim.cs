using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_ANIMATOR_TYPE
{
    Body = 0,
    Weapon = 1,
    Cover = 2,
}

public partial class ActiveCharacter
{
    private Animator bodyAnim;
    private Animator weaponAnim;
    private Animator coverAnim;

    private SpriteRenderer bodySpriteRender;
    private SpriteRenderer weaponSpriteRender;
    private SpriteRenderer coverSpriteRender;

    private bool reverseState = false;

    private void SetObjectInfo(ENUM_CHARACTER_TYPE charType)
    {
        GameObject g;

        g = gameObject.transform.Find("Body").gameObject;
        bodyAnim = g.GetComponent<Animator>();
        bodyAnim.runtimeAnimatorController = Managers.Resource.GetAnimator(charType, ENUM_ANIMATOR_TYPE.Body);
        bodySpriteRender = g.GetComponent<SpriteRenderer>();

        g = gameObject.transform.Find("Weapon").gameObject;
        weaponAnim = g.GetComponent<Animator>();
        weaponAnim.runtimeAnimatorController = Managers.Resource.GetAnimator(charType, ENUM_ANIMATOR_TYPE.Weapon);
        weaponSpriteRender = g.GetComponent<SpriteRenderer>();

        g = gameObject.transform.Find("Cover").gameObject;
        coverAnim = g.GetComponent <Animator>();
        coverAnim.runtimeAnimatorController = Managers.Resource.GetAnimator(charType, ENUM_ANIMATOR_TYPE.Cover);
        coverSpriteRender = g.GetComponent<SpriteRenderer>();
    }

    private void SetSpriteOrderLayer(Vector2 vec)
    {
        bool isUpState = (vec.y > 0.9f && vec.x == 0.0f) ? true : false;
        bool isDownState = (vec.y <= 0.0f && vec.x == 0.0f) ? true : false;

        if (isUpState)
        {
            weaponSpriteRender.sortingOrder = 4;
            coverSpriteRender.sortingOrder = 3;
        }
        else if (isDownState) 
        {
            weaponSpriteRender.sortingOrder = 7;
            coverSpriteRender.sortingOrder = 6;
        }
        else
        {
            weaponSpriteRender.sortingOrder = 6;
            coverSpriteRender.sortingOrder = 7;
        }
    }

    private void SetVector(Vector2 vec, bool isRun)
    {
        ReverseSprites(vec.x);
        SetSpriteOrderLayer(vec);

        float f = isRun ? 2.0f : 1.0f;
        bodyAnim.SetFloat("DirX", vec.x * f);
        bodyAnim.SetFloat("DirY", vec.y * f);

        weaponAnim.SetFloat("DirX", vec.x);
        weaponAnim.SetFloat("DirY", vec.y);

        coverAnim.SetFloat("DirX", vec.x);
        coverAnim.SetFloat("DirY", vec.y);
    }

    private Vector2 GetVector()
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

    private void SetFloat(string str, float value)
    {
        bodyAnim.SetFloat(str, value);
        weaponAnim.SetFloat(str, value);
        coverAnim.SetFloat(str, value);
    }

    private void SetInteger(string str, int value)
    {
        bodyAnim.SetInteger(str, value);
        weaponAnim.SetInteger(str, value);
        coverAnim.SetInteger(str, value);
    }

    private void SetBool(string str, bool value)
    {
        bodyAnim.SetBool(str, value);
        weaponAnim.SetBool(str, value);
        coverAnim.SetBool(str, value);
    }

    private void SetTrigger(string str)
    {
        bodyAnim.SetTrigger(str);
        weaponAnim.SetTrigger(str);
        coverAnim.SetTrigger(str);
    }

    private bool GetBool(string str)
    {
        return bodyAnim.GetBool(str);
    }

    private int GetInteger(string str)
    {
        return bodyAnim.GetInteger(str);
    }
}