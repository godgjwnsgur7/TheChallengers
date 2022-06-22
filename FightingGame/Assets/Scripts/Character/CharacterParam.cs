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
    public float moveDir;
    public bool isRun;
    public float speed = 3.0f;
    
    public CharacterMoveParam(float _moveDir, bool _isRun = false)
    {
        isRun = _isRun;
        speed = _isRun ? (speed * 1.5f) : speed;
        moveDir = _moveDir;
    }
}

public class CharacterAttackParam : CharacterParam
{
    public float damage;

    public CharacterAttackParam(float _damage)
    {
        damage = _damage;
    }
}