using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class CharPanel : MonoBehaviour
{
    Dictionary<ENUM_CHARACTER_TYPE, CharPanelElement> charPanelElementsDict = new Dictionary<ENUM_CHARACTER_TYPE, CharPanelElement>();

    public void Init(Action<ENUM_CHARACTER_TYPE> _selectedCharCallBack)
    {
        for (int i = 1; i < (int)ENUM_CHARACTER_TYPE.Max; i++)
        {
            ENUM_CHARACTER_TYPE charType = (ENUM_CHARACTER_TYPE)i;
            if (charPanelElementsDict.ContainsKey(charType))
                return;

            CharPanelElement charPanelElement = Managers.Resource.Instantiate("UI/CharPanelElement", this.transform).GetComponent<CharPanelElement>();
            charPanelElement.Init(_selectedCharCallBack, charType);
            charPanelElementsDict.Add(charType, charPanelElement);
        }
    }

    public void Update_SelectCharState(ENUM_CHARACTER_TYPE _selectedCharType)
    {
        foreach(CharPanelElement charPanelElement in charPanelElementsDict.Values)
            charPanelElement.Set_CharAreaImageColor(Color.gray);

        if (charPanelElementsDict.ContainsKey(_selectedCharType))
        {
            charPanelElementsDict.TryGetValue(_selectedCharType, out CharPanelElement charPanelElement);
            charPanelElement.Set_CharAreaImageColor(new Color(1f, 0f, 0f, 0.5f)); // 버건디
        }
    }
}
