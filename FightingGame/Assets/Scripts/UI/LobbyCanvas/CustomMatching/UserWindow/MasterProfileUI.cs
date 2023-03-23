using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MasterProfileUI : BaseProfile
{
    public override void Init(Profile_Info _profileInfo)
    {
        IsMine = PhotonLogicHandler.IsMasterClient;

        base.Init(_profileInfo);
    }
    
    public override void Clear()
    {
        base.Clear();
    }
}
