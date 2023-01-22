using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 네트워크매니저가 브로드캐스트멀티를 사용하기 위해 존재
/// </summary>
public class UserSyncMediator : MonoBehaviourPhoton
{
    public override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        Managers.Network.Set_UserSyncMediator(this);
    }

    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        // stream.Write(isMasterClearComplete);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        // isMasterClearComplete = stream.Read<bool>();=

        base.OnOtherSerializeView(stream);
    }

    [BroadcastMethod]
    public void GameStart_Preprocessing()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect();
    }
}
