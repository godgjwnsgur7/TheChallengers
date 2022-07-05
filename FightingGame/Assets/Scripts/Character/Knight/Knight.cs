using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_KNIGHT_ATTACK_TYPE
{

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

            attackObject = Managers.Resource.Instantiate($"AttackObejcts/{characterType}_Attack", gameObject.transform.position).GetComponent<AttackObejct>();
            attackObject.ActivatingAttackObject(attackParam);                

        }


    }
}
