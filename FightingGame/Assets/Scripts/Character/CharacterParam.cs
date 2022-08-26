using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

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

/// <summary>
/// 기본공격, 점프공격
/// </summary>
public class CharacterAttackParam : CharacterParam
{
    public ENUM_SKILL_TYPE attackType;
    public bool reverseState;

    public CharacterAttackParam(ENUM_SKILL_TYPE _attackType, bool _reverseState)
    {
        attackType = _attackType;
        reverseState = _reverseState;
    }
}

/// <summary>
/// 캐릭터의 스킬 번호 ( 0 ~ 2 )
/// </summary>
public class CharacterSkillParam : CharacterParam
{
    public int skillNum;

    public CharacterSkillParam(int _skillNum)
    {
        skillNum = _skillNum;
    }
}