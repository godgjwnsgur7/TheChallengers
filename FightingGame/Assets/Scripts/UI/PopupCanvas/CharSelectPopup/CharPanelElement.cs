using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CharPanelElement : MonoBehaviour
{
    Action<ENUM_CHARACTER_TYPE> selectedCharTypeCallBack;

    [SerializeField] Image charAreaImage;
    [SerializeField] Text charText;

    ENUM_CHARACTER_TYPE myCharType;

    bool isInit = false;

    public void Init(Action<ENUM_CHARACTER_TYPE> _charTypeCallBack, ENUM_CHARACTER_TYPE _myCharType)
    {
        if (isInit)
            return;

        isInit = true;
        selectedCharTypeCallBack = _charTypeCallBack;
        myCharType = _myCharType;
        
        gameObject.name = myCharType.ToString();
        Set_CharAreaImageColor(Color.gray);
        charText.text = myCharType.ToString();

        charAreaImage.gameObject.GetComponent<Button>().onClick.AddListener(OnClick_CharType);

        /* // CharIconImage Setting (임시)
        charAreaImage.gameObject.transform.GetChild(0).GetComponent<Image>().sprite
            = Managers.Resource.Load<Sprite>("캐릭터 아이콘 경로 입력");
        */
    }
    public void Set_CharAreaImageColor(Color _color) => charAreaImage.color = _color;
    public void OnClick_CharType() => selectedCharTypeCallBack(myCharType);
}
