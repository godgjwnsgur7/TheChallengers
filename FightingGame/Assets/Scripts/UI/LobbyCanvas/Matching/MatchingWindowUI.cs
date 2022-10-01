using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MatchingWindowUI : MonoBehaviour
{
    public void OnClick_Matching()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(MathingStart);
    }

    public void MathingStart(ENUM_CHARACTER_TYPE charType)
    {
        Debug.Log($"선택한 캐릭터 : {charType}");

        // 매칭 돌려주면서? 
        // 이거 팝업캔버스로 뺄지도 모름
    }
}
