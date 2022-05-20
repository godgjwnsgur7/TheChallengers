using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPair
{
    Animator anim;
    SpriteRenderer sprite;
}

public class PlayerAnimation : MonoBehaviour
{
    // 페어로 묶을지도 고민중 아직 사용x
    AnimatorPair bodyPair = new AnimatorPair();

    public Animator bodyAnim;
    // public Animator weaponAnim;
    // public Animator handAnim;

    public SpriteRenderer bodySprite;
    public SpriteRenderer weaponSprite;
    public SpriteRenderer coverSprite;

    private bool reverseState = false;

    public void SetVector(Vector2 vec, bool isRun)
    {
       ReverseSprites(vec.x == 1.0f);

        float f = isRun ? 2.0f : 1.0f;
        bodyAnim.SetFloat("DirX", vec.x * f);
        bodyAnim.SetFloat("DirY", vec.y * f);
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
}
