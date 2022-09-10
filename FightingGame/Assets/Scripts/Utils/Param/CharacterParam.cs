using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;
using System.Text;

/// <summary>
/// 캐릭터의 스테이트를 변경하기 위해 사용하는 파라미터 목록
/// </summary>
/// 

[Serializable]
public class CharacterParam : PhotonCustomType { }

[Serializable]
public class CharacterMoveParam : CharacterParam
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
[Serializable]
public class CharacterAttackParam : CharacterParam
{
    public ENUM_SKILL_TYPE attackType;
    public bool reverseState;

    public CharacterAttackParam(ENUM_SKILL_TYPE _attackType, bool _reverseState)
    {
        attackType = _attackType;
        reverseState = _reverseState;
    }

    public new static object Deserialize(byte[] data)
    {
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<CharacterAttackParam>(jsonData);
    }

    public new static byte[] Serialize(object customObject)
    {
        var param = (CharacterAttackParam)customObject;
        string jsonData = JsonUtility.ToJson(param);
        return Encoding.UTF8.GetBytes(jsonData);
    }
}

/// <summary>
/// 캐릭터의 스킬 번호 ( 0 ~ 2 )
/// </summary>
[Serializable]
public class CharacterSkillParam : CharacterParam
{
    public int skillNum;

    public CharacterSkillParam(int _skillNum)
    {
        skillNum = _skillNum;
    }
}