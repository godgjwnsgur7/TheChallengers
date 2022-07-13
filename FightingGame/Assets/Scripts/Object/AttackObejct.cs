using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class AttackObejct : Poolable
{
    [SerializeField] Vector2 vecPos;

    public Skill skillValue;

    public override void Init()
    {
        ENUM_SKILL_TYPE skill = (ENUM_SKILL_TYPE)Enum.Parse(typeof(ENUM_SKILL_TYPE), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)skill, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }

        
    }

    public void ActivatingAttackObject(CharacterAttackParam _attackParam, bool _reverseState)
    {
        transform.position = new Vector2(transform.position.x + (_reverseState ? vecPos.x * (-1) : vecPos.x), transform.position.y + vecPos.y);
        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(skillValue.runTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ENUM_TAG_TYPE.Enemy.ToString())
        {
            ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            if (enemyCharacter != null && skillValue != null)
            {
                CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_SKILL_TYPE)skillValue.skillType);
                enemyCharacter.Hit(attackParam);
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

        Managers.Resource.Destroy(gameObject);
    }
}
