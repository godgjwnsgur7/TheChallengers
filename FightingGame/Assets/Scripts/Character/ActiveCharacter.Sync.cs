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

	protected override void OnOtherSerializeView(PhotonReadStream stream)
	{
		currState = stream.Read<ENUM_PLAYER_STATE>();

		base.OnOtherSerializeView(stream);
	}


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
