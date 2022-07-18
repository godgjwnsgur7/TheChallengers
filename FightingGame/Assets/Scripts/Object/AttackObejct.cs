using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class AttackObejct : Poolable
{
    public Skill skillValue;

    public override void Init()
    {
        ENUM_SKILL_TYPE skill = (ENUM_SKILL_TYPE)Enum.Parse(typeof(ENUM_SKILL_TYPE), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)skill, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }

        
    }

    public void ActivatingAttackObject(GameObject _target,bool _reverseState)
    {
        transform.localEulerAngles = _reverseState ? new Vector3(0, 180, 0) : Vector3.zero;

        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(skillValue.runTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.ToString() == ENUM_TAG_TYPE.Enemy.ToString())
        {
            ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            if (enemyCharacter != null && skillValue != null)
            {
                CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_SKILL_TYPE)skillValue.skillType);

                float ConversionDir = 1.0f;
                if (gameObject.transform.position.x > enemyCharacter.transform.position.x)
                    ConversionDir = -1.0f;

                enemyCharacter.ReverseSprites(ConversionDir * -1.0f);

                enemyCharacter.Hit(attackParam);
                enemyCharacter.rigid2D.AddForce(new Vector2(skillValue.pushingPower * ConversionDir, skillValue.risingPower), ForceMode2D.Impulse);
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
