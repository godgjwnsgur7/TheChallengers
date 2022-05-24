using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator bodyAnim;
    public Animator weaponAnim;
    public Animator coverAnim;

    public SpriteRenderer bodySpriteRender;
    public SpriteRenderer weaponSpriteRender;
    public SpriteRenderer coverSpriteRender;

    private bool reverseState = false;

    public void Init(ENUM_CHARACTER_TYPE charType)
    {
        GetObjectInfo();
        SetSpriteOrderLayer(Vector2.zero);
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

    private void SetSpriteOrderLayer(Vector2 vec)
    {
        bool isUpState = (vec.y > 0.9f && vec.x == 0.0f) ? true : false;

        if(isUpState)
        {
            weaponSpriteRender.sortingOrder = 4;
            coverSpriteRender.sortingOrder = 3;
        }
        else
        {
            weaponSpriteRender.sortingOrder = 6;
            coverSpriteRender.sortingOrder = 7;
        }
    }

    public void SetVector(Vector2 vec, bool isRun)
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