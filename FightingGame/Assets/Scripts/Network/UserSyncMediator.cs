using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSyncMediator : MonoBehaviourPhoton
{
    

    public override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        Managers.Network.Set_UserSyncMediator(this);
    }


}
