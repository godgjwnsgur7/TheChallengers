using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class BotController : MonoBehaviour, IMonoBehaviourTest
{
    Coroutine botCoroutine = null;
    List<FGBot> botList = new List<FGBot>();

    public bool IsTestFinished => botList.All(bot => bot.IsTestFinished == true);

    public void Initialize()
    {
        botList = new List<FGBot>()
        {
            new LoginBot(this)
        };
    }

    public void Run()
    {
        botCoroutine = StartCoroutine(Process());
    }
    
    public void Stop()
    {
        StopCoroutine(botCoroutine);
    }

    private IEnumerator Process()
    {
        foreach (var bot in botList)
        {
            if(bot.Initialize())
            {
                yield return bot.ProcessMain();
            }
            
            while (!bot.IsTestFinished)
                yield return null;
        }
    }

    public T Find<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }

}
