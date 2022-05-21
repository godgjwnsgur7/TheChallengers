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
    // public Animator weaponAnim;
    // public Animator handAnim;

    public SpriteRenderer bodySprite;
    public SpriteRenderer weaponSprite;
    public SpriteRenderer coverSprite;

    public FarWeaponSprites farWeaponSprites = new FarWeaponSprites();

    private bool reverseState = false;

    public void Init(ENUM_CHARACTER_TYPE charType)
    {
        farWeaponSprites = Managers.Resource.GetWeaponSprites(charType);
    }
    
    public void SetSprite(ENUM_WEAPON_TYPE weaponType)
    {
        switch(weaponType)
        {
            case ENUM_WEAPON_TYPE.Bow:
                
                break;
            case ENUM_WEAPON_TYPE.Gun:

                break;
            case ENUM_WEAPON_TYPE.Rifle:

                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }

    private void SetSprites()
    {

    }

    public void SetVector(Vector2 vec, bool isRun)
    {
       ReverseSprites(vec.x == 1.0f);

        float f = isRun ? 2.0f : 1.0f;
        bodyAnim.SetFloat("DirX", vec.x * f);
        bodyAnim.SetFloat("DirY", vec.y * f);
    }

    public Vector2 GetVector()
    {
        return new Vector2(bodyAnim.GetFloat("DirX"), bodyAnim.GetFloat("DirY"));
    }

    private void ReverseSprites(bool _reverseState)
    {
        if (reverseState == _reverseState)
            return;

        bodySprite.flipX = _reverseState;
        weaponSprite.flipX = _reverseState;
        coverSprite.flipX = _reverseState;
        reverseState = _reverseState;
    }

    public void SetFloat(string str, float value)
    {
        bodyAnim.SetFloat(str, value);
    }

    public void SetInteger(string str, int value)
    {
        bodyAnim.SetInteger(str, value);
    }

    public void SetBool(string str, bool value)
    {
        bodyAnim.SetBool(str, value);
    }

    public void SetTrigger(string str)
    {
        bodyAnim.SetTrigger(str);
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
