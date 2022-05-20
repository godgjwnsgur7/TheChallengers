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
    public float speed = 3.0f;
    
    public CharacterMoveParam(Vector3 _inputVec, bool _isRun)
    {
        isRun = _isRun;
        speed = _isRun ? (speed * 1.5f) : speed;
        inputVec = _inputVec;
    }
}

public class CharacterHitParam : CharacterParam
{
    public float damage;

    public CharacterHitParam(float _damage)
    {
        damage = _damage;
    }
}