using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class CharacterSelectArea : MonoBehaviour
{
    List<CharacterSelectElementUI> characterSelectElementList = new List<CharacterSelectElementUI>();
    public void Init(Action<ENUM_CHARACTER_TYPE> _selectionCharacterCallBack)
    {
        CharacterSelectElementUI characterSelectElementUI;
        characterSelectElementList.Clear();

        for (int i = (int)ENUM_CHARACTER_TYPE.Default + 1; i < (int)ENUM_CHARACTER_TYPE.Max; i++)
        {
            characterSelectElementUI = Managers.Resource.Instantiate("UI/CharacterSelectElement", transform).GetComponent<CharacterSelectElementUI>();

            characterSelectElementUI.Init(_selectionCharacterCallBack, (ENUM_CHARACTER_TYPE)i);

            characterSelectElementList.Add(characterSelectElementUI);
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        for(int i = 0; i < characterSelectElementList.Count; i++)
        {
            if (characterSelectElementList[i] != null)
                Managers.Resource.Destroy(characterSelectElementList[i].gameObject);
        }

        gameObject.SetActive(false);
    }
}
