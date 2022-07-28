using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class AttackObejct : Poolable
{
    public Skill skillValue;
    public ENUM_TEAM_TYPE teamType;

    // 디버그용
    public string attackTarget = null;

    public override void Init()
    {
        ENUM_SKILL_TYPE skill = (ENUM_SKILL_TYPE)Enum.Parse(typeof(ENUM_SKILL_TYPE), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)skill, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }
    }

    public virtual void ActivatingAttackObject(GameObject _target,bool _reverseState)
    {
        // 디버깅용 (클라이언트 1개)
        switch(_target.tag.ToString())
        {
            case "Ally":
                attackTarget = "Enemy";
                break;
            case "Enemy":
                attackTarget = "Ally";
                break;
        }

        transform.localEulerAngles = _reverseState ? new Vector3(0, 180, 0) : Vector3.zero;

        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(skillValue.runTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToString() == attackTarget /*ENUM_TAG_TYPE.Enemy.ToString()*/)
        {
            ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            if (enemyCharacter != null && skillValue != null)
            {
                CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_SKILL_TYPE)skillValue.skillType);

                Vector2 dirPower = new Vector2(skillValue.pushingPower, skillValue.risingPower);

                if (gameObject.transform.position.x > enemyCharacter.transform.position.x)
                    dirPower.x *= -1.0f;

                enemyCharacter.ReverseSprites(dirPower.x * -1f);

                enemyCharacter.Hit(attackParam);

                if(enemyCharacter.jumpState && skillValue.risingPower == 0f)
                {
                    dirPower.y = skillValue.pushingPower * 2;
                    dirPower.x = 1.0f;
                }

                enemyCharacter.rigid2D.velocity = Vector2.zero; // 받고있는 힘 초기화
                enemyCharacter.rigid2D.AddForce(dirPower, ForceMode2D.Impulse);

                Managers.Resource.Destroy(gameObject);
            }
            else
            {
                Debug.Log($"{gameObject.name} 이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
            }
               
        }
    }

    private IEnumerator IAttackRunTimeCheck(float _runTime)
    {
        yield return new WaitForSeconds(_runTime);
        
        if(this.gameObject.activeSelf)
            Managers.Resource.Destroy(gameObject);
    }
}
