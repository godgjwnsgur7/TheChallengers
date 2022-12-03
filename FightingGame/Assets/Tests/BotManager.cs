using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


public class BotManager : MonoBehaviourTest<BotController>
{
    [Test]
    public void Run()
    {
        component.Initialize();
        component.Run();
    }

    [Test]
    public void Stop()
    {
        component.Stop();
    }

}
