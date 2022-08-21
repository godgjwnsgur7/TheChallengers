using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public partial class ActiveCharacter : Character
{
    /* 변수 동기화가 안되서 일단 통으로 주석처리
    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        stream.Write(currState);

        base.OnMineSerializeView(stream);
    }
    
    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        currState = (ENUM_PLAYER_STATE)stream.Read();

        base.OnOtherSerializeView(stream);
    }
    */

    [BroadcastMethodAttribute]
    public void Sync_ReverseState(bool _reverseState)
    {
        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    // 일단 시도중
    [BroadcastMethodAttribute]
    public void Sync_PlayerState(ENUM_PLAYER_STATE playerState)
    {
        currState = playerState;
    }
}
