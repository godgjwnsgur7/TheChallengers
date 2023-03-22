using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class UserInfo_SelectChar : MonoBehaviour
{
    [SerializeField] Text characterNameText;
    [SerializeField] Text characterDescriptionText;

    public void Open()
    {
        characterNameText.text = "-";
        characterDescriptionText.text = "캐릭터 선택중...";

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Set_SelectionCharacter(ENUM_CHARACTER_TYPE _characterType)
    {
        characterNameText.text = _characterType.ToString().ToUpper();
        characterDescriptionText.text = Managers.Data.Get_CharExplanationDict(_characterType);
    }
}
