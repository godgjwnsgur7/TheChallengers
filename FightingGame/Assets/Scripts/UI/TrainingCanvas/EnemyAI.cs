using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    Transform thisTrans;
    EnemyPlayer aiEnemy;

    float intervalX;
    float intervalY;

    bool isMove = false;
    bool isAttack = false;

    public void Init(ENUM_CHARACTER_TYPE charType)
    {
        aiEnemy = this.transform.parent.GetComponent<EnemyPlayer>();

        thisTrans = GetComponent<Transform>();

        target = GameObject.Find("Player").transform.Find($"{charType}").transform;
    }

    private void FixedUpdate()
    {
        AILogic();
    }

    private void AILogic()
    {
        isMove = false;
        isAttack = false;

        intervalY = target.position.y - thisTrans.position.y;
        intervalX = thisTrans.position.x - target.position.x;

        if (intervalY >= 2.5f && intervalY < 5.0f)
            JumpState();

        if (intervalX <= -2.5f)
        {
            isMove = true;
            StartCoroutine(MoveState(1.0f));
        }
        else if (intervalX >= 2.5f)
        {
            isMove = true;
            StartCoroutine(MoveState(-1.0f));
        }
        else
        {
            isAttack = true;
            StartCoroutine(AttackState());
        }
    }

    private void JumpState()
    {
        aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Jump);
    }

    IEnumerator MoveState(float direction)
    {
        while (isMove)
        {
            aiEnemy.moveDir = direction;
            aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(aiEnemy.moveDir));
            yield return null;
        }
    }

    IEnumerator AttackState()
    {
        while (isAttack)
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, aiEnemy.activeCharacter.reverseState);
            aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            aiEnemy.activeCharacter.Change_AttackState(true);
            yield return null;
        }
    }
}
