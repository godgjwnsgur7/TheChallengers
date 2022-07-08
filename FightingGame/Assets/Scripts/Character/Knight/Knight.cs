using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

/// <summary>
/// 해당 공격 타입의 AttackObject의 이름과 같아야 함
/// </summary>
public enum ENUM_KNIGHT_ATTACK_TYPE
{
    Attack,
}

public class Knight : ActiveCharacter
{

    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Knight;

        base.Init();
    }

    public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack)
            return;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("AttackTrigger");

            string path = characterType.ToString() + "/" + ENUM_KNIGHT_ATTACK_TYPE.Attack.ToString();
            GameObject g = Managers.Resource.GetAttackObject(path);
            attackObject = g.gameObject.GetComponent<AttackObejct>();

            if(attackObject != null)
            {
                StartCoroutine(IAttackDelayTimeCheck(attackParam));
            }
        }
    }
}
