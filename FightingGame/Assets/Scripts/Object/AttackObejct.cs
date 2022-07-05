using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;


public class AttackObejct : Poolable
{
    public bool isSetting = false;
    [SerializeField] Vector2 vecPos;
    
    public void ActivatingAttackObject(CharacterAttackParam _attackParam)
    {
        StartCoroutine(IAttackRunTimeCheck(_attackParam.runTime));
    }

    private void OnEnable()
    {
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
    
    IEnumerator IAttackRunTimeCheck(float _runTime)
    {
        yield return new WaitForSeconds(_runTime);

        Managers.Resource.Destroy(this.gameObject);
    }
}
