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
    public float speed = 3.0f;
    
    private float runSpeed = 5.0f;
    
    public CharacterMoveParam(Vector3 _inputVec, bool _isRun)
    {
        speed = _isRun ? runSpeed : speed;
        this.inputVec = _inputVec;
    }
}

public class CharacterAttackParam : CharacterParam
{
    int attackType = 0;

    public CharacterAttackParam(int _attackType)
    {
        attackType = _attackType;
    }
}