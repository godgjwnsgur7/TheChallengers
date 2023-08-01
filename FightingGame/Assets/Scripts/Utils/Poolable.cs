using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviourPhoton
{
    public bool isUsing;
    protected bool isServerSyncState = false;

    public override void Init() 
    { 
        base.Init();

        isServerSyncState = Managers.Network.IsServerSyncState;
    }
}