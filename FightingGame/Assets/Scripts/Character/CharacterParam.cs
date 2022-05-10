using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터의 스테이트를 변경하기 위해 사용하는 파라미터 목록
/// </summary>
/// 

public class CharacterParam { }

public class CharacterMoveParam : CharacterParam
{
    public Vector2 inputVec;
    public bool isRun;

    public CharacterMoveParam(Vector3 inputVec, bool isRun)
    {
        this.inputVec = inputVec;
        this.isRun = isRun;
    }
}