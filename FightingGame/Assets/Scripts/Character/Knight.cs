using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Knight : ActiveCharacter
{
	private bool isInitialized = false;

	public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 캐릭터를 초기화 시도하였습니다.");
            return;
        }

        isInitialized = true;
		characterType = ENUM_CHARACTER_TYPE.Knight;

        base.Init();
    }
}
