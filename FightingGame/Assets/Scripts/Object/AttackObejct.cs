using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;


public class AttackObejct : Poolable
{
    public bool isSetting = false;

    CharacterAttackParam attackParam = null;

    public void ActivatingAttackObject(CharacterAttackParam _attackParam)
    {
        attackParam = _attackParam;
        
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Coroutine attackCoroutine = StartCoroutine(IAttackRunTimeCheck());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ENUM_TAG_TYPE.Enemy.ToString())
        {
            ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            if(enemyCharacter != null)
            {
                // enemyCharacter.Hit(attackParam);
            }
        }
    }

    IEnumerator IAttackRunTimeCheck()
    {
        yield return null;
    }
}
