using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class AttackObejct : Poolable
{
    [SerializeField] Vector2 vecPos;

    public Skill skillValue;

    public void Init()
    {

    }

    public void ActivatingAttackObject(CharacterAttackParam _attackParam, bool _reverseState)
    {


        transform.position = new Vector2(transform.position.x + (_reverseState ? vecPos.x * (-1) : vecPos.x), transform.position.y + vecPos.y);
        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(skillValue.runTime));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("아니 왜 안돼 ㅋㅋㄹㅃㅃ");

        if (collision.tag == ENUM_TAG_TYPE.Enemy.ToString())
        {
            ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            if (enemyCharacter != null && skillValue != null)
            {
                CharacterAttackParam attackParam = new CharacterAttackParam(skillValue.skillType);
                enemyCharacter.Hit(attackParam);
            }
            else
                Debug.Log($"{gameObject.name} 이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    private IEnumerator IAttackRunTimeCheck(float _runTime)
    {
        yield return new WaitForSeconds(_runTime);

        skillValue = null;
        Managers.Resource.Destroy(gameObject);
    }
}
