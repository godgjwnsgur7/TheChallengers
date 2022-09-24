using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class EnemyAI : MonoBehaviour
{
    RaycastHit2D rayHit;
    public Transform target;
    Transform thisTrans;
    Character thisCharacter;
    EnemyPlayer aiEnemy;
    float charFront;
    Vector2 direction;
    Vector2 rayVec;

    public void Init(ENUM_CHARACTER_TYPE charType)
    {
        aiEnemy = this.transform.parent.GetComponent<EnemyPlayer>();
        thisTrans = GetComponent<Transform>();
        target = GameObject.Find("Player").transform.Find($"{charType}").transform;
        thisCharacter = this.gameObject.GetComponent<Character>();
    }

    private void FixedUpdate()
    {
        charFront = (thisCharacter.reverseState == true) ? -1f : 1f;
        direction = (thisCharacter.reverseState == true) ? Vector2.left : Vector2.right;

        rayVec = new Vector2(thisTrans.position.x + charFront, thisTrans.position.y);
        rayHit = Physics2D.Raycast(rayVec, direction, 5f);

        Debug.DrawRay(rayVec, direction * 5f, Color.red);

        MoveArrow();
    }

    private void MoveArrow()
    {
        if (aiEnemy.activeCharacter.anim.GetBool("AttackState"))
        {
            aiEnemy.activeCharacter.Change_AttackState(false);
        }

        if (thisTrans.position.x - target.position.x <= -3)
        {
            MoveState(1.0f);

            if(thisTrans.position.y < target.position.y)
                aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }
        else if (thisTrans.position.x - target.position.x >= 3)
        {
            MoveState(-1.0f);

            if (thisTrans.position.y < target.position.y)
                aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }
        else
        {
            AttackState();
        }
    }

    private void MoveState(float direction)
    {
        aiEnemy.moveDir = direction;
        aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(aiEnemy.moveDir));
    }

    private void AttackState() 
    {
        CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, aiEnemy.activeCharacter.reverseState);
        aiEnemy.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
        aiEnemy.activeCharacter.Change_AttackState(true);
    }
}
