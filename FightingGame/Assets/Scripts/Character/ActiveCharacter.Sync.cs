using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public partial class ActiveCharacter : Character
{
	[BroadcastMethodAttribute]
    public void Sync_ReverseState(bool _reverseState)
    {
        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    public void Sync_Summon_AttackObjcet(int _attackTypeNum)
    {

    }
}
