using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class KeyArea : MonoBehaviour
{
    public Image areaImage;
    public bool isSelect = false;
    bool isInit = false;
    public int triggerCount = 0;

    public void Init()
    {
        if (isInit) return;

        isInit = true;

        areaImage = this.transform.Find("AreaImage").GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
            triggerCount++;

        Debug.Log(collision.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(triggerCount > 0)
            Set_AreaColor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerCount--;

        if (triggerCount < 1)
            Set_AreaColor();
    }

    public void Set_AreaColor()
    {
        Color changeColor;
        
        if(triggerCount > 0)
        {
            changeColor = new Color(255, 0, 0, 0.5f);
            areaImage.color = changeColor;
        }
        else if (isSelect && triggerCount < 1)
        {
            changeColor = new Color(0, 255, 0, 0.5f);
            areaImage.color = changeColor;
        }
        else if (!isSelect && triggerCount < 1)
        {
            changeColor = new Color(255, 255, 255, 0f);
            areaImage.color = changeColor;
        }
    }

    public bool Get_Updatable()
    {
        if (triggerCount > 0)
            return false;
        else
            return true;
    }
}
