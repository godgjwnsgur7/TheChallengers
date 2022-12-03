using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BotManager
{
    FGBot[] bots = new FGBot[]
    {
        new TestBot(),
    };
    
    [Test]
    public void RunAllBot()
    {
        var iter = Process();

        while (iter.MoveNext())
        {

        }
    }

    private IEnumerator Process()
    {
        foreach(var bot in bots)
        {
            bot.Run();

            while (bot.IsProcessing())
                yield return null;
        }
    }
}
