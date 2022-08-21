using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public partial class ActiveCharacter : Character
{
    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        stream.Write(currState);

        base.OnMineSerializeView(stream);
    }

    // 변수 동기화가 되지 않고 있..ㅎ
    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        currState = (ENUM_PLAYER_STATE)stream.Read();

        base.OnOtherSerializeView(stream);
    }

    [BroadcastMethodAttribute]
    public void Sync_ReverseState(bool _reverseState)
    {
        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    
}
