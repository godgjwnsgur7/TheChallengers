using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class AttackObejct : Poolable
{
    [SerializeField] Vector2 vecPos;

    public void ActivatingAttackObject(CharacterAttackParam _attackParam, bool _reverseState)
    {
        Debug.Log(_attackParam.runTime);

        transform.position = new Vector2(transform.position.x + (_reverseState ? vecPos.x * (-1) : vecPos.x), transform.position.y + vecPos.y);
        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(_attackParam.runTime));

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

    private IEnumerator IAttackRunTimeCheck(float _runTime)
    {
        yield return new WaitForSeconds(_runTime);

        Managers.Resource.Destroy(gameObject);
    }
}
