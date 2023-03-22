using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MasterProfileUI : BaseProfile
{
    public override void Init()
    {
        if (isInit)
            return;

        base.Init();

        if(PhotonLogicHandler.IsMasterClient)
            isMine = true;
    }
    
    public override void Clear()
    {
        base.Clear();
    }
}
