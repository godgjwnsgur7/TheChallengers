using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

/// <summary>
/// 캐릭터의 스테이트를 변경하기 위해 사용하는 파라미터 목록
/// </summary>
/// 

public class CharacterMoveParam : CharacterParam<CharacterMoveParam>
{
    public float moveDir;
    
    public CharacterMoveParam(float _moveDir)
    {
        moveDir = _moveDir;
    }
}

/// <summary>
/// 기본공격, 점프공격
/// </summary>
public class CharacterAttackParam : CharacterParam<CharacterAttackParam>
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
public class CharacterSkillParam : CharacterParam<CharacterSkillParam>
{
    public int skillNum;

    public CharacterSkillParam(int _skillNum)
    {
        skillNum = _skillNum;
    }
}