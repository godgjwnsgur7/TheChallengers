using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MasterProfileUI : BaseProfile
{
    public override void Init()
    {

        base.Init();
    }

    public override void IsReadyInfoUpdateCallBack(bool _readyState)
    {


        base.IsReadyInfoUpdateCallBack(_readyState);
    
    }

    public override void Set_ReadyState(bool readyState)
    {
        if (readyState && currCharType == ENUM_CHARACTER_TYPE.Default)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }
        

        base.Set_ReadyState(readyState);


    }

    public override void Clear()
    {
        base.Clear();

        Set_ReadyState(false);
    }
}
