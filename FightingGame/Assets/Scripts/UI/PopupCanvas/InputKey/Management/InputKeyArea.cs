using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class InputKeyArea : InputBasicKey
{
    [SerializeField] Image areaImage;

    int collisionCount = 0;
    bool isSelect = false;
    
    public override void EventTrigger_PointerDown()
    {
        isSelect = true;
        Set_AreaColor();

        OnPointDownCallBack?.Invoke((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public override void EventTrigger_PointerUp()
    {
        Set_AreaColor();

        OnPointUpCallBack?.Invoke((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
            collisionCount++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collisionCount > 0)
            Set_AreaColor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI && collisionCount > 0)
            collisionCount--;

        Set_AreaColor();
    }

    public void Set_ScaleSize(float _value)
    {
        float scaleSizeValue = _value / 100f;
        Vector2 tempVec = new Vector2(scaleSizeValue, scaleSizeValue);
        rectTr.localScale = tempVec;
    }

    public void Set_AreaColor()
    {
        Color changeColor;

        if (collisionCount > 0) // 충돌
        {
            changeColor = isSelect? Color.magenta : Color.red;
            changeColor.a = 1.0f;
            areaImage.color = changeColor;
        }
        else if (isSelect) // 선택
        {
            changeColor = new Color(1f, 1f, 1f, 1f);
            areaImage.color = changeColor;
        }
        else // 비활
        {
            changeColor = new Color(1f, 1f, 1f, 0f);
            areaImage.color = changeColor;
        }
    }

    /// <summary>
    /// 겹쳐있는 상태(저장불가상태)면 True
    /// </summary>
    public bool Get_CollisionCheck() => collisionCount > 0;

    public void Deselect_AreaImage()
    {
        isSelect = false;
        Set_AreaColor();
    }
}
