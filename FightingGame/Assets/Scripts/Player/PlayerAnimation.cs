using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPair
{
    Animator dd;
    SpriteRenderer rr;

}

public enum zzz
{
    BODY,
    WEAPON,
    HAND
}

public class PlayerAnimation : MonoBehaviour
{
    AnimatorPair[] pairs = new AnimatorPair[(int)zzz.HAND + 1];

    public Animator bodyAnim;
    // public Animator weaponAnim;
    // public Animator handAnim;

    public SpriteRenderer bodySprite;
    // public SpriteRenderer weaponSprite;
    // public SpriteRenderer handSprite;


    public void SetFloat(string name, float value)
    {
        bodyAnim.SetFloat(name, value);
    }

    public void SetBool(string str, bool value)
    {
        bodyAnim.SetBool(str, value);
    }

    public bool GetBool(string str)
    {
        // 이거 ㅋㅋㅋ 에바참친데 일단 해ㅋㅋ
        return bodyAnim.GetBool(str);
    }
}
