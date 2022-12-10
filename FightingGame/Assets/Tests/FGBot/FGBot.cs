using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// 유니티 테스트 자동화 도구로 쓸 봇을 만든다.
/// 모든 것에 대하여 만드는 것은 불가능하므로, 우선순위를 정하여 봇을 구성한다.
/// 
/// 주기적으로 커맨드 라인으로 테스트 봇을 돌릴 예정
/// 리포트는 디스코드로 발송하는 것이 베스트, 불가능할 경우 로컬 파일로 만든다.
/// 
/// 1. 버그가 터졌을 때 크리티컬한 곳을 위주로 작성한다.
/// 2. 자주 발생할 만한 버그를 위주로 작성한다.
/// </summary>

public abstract class FGBot : MonoBehaviourTest<MonoBehaviourTestHelper>
{
    private bool isSuccessed = false;
    private bool isFailed = false;

    public bool IsTestFinished => isSuccessed || isFailed;

    public FGBot()
    {
        isSuccessed = false;
        isFailed = false;
    }

    protected void Success()
    {
        isSuccessed = true;
        isFailed = false;

        End();
    }

    protected void Fail()
    {
        isFailed = true;
        isSuccessed = false;

        End();
    }

    public virtual IEnumerator ProcessMain()
	{
        yield return null;
	}

    public virtual bool Initialize()
    {
        return true;
    }

    private void End()
    {
        Assert.IsFalse(!IsTestFinished || IsFailed());
    }

    public bool IsFailed()
    {
        return !isSuccessed && isFailed;
    }
}
