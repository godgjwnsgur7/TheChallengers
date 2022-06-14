using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region Test
[Serializable]
public class Test
{
    public int intValue;
    public string stringValue;
}

[Serializable]
public class TestData : ILoader<int, Test>
{
    public List<Test> testList = new List<Test>();

    public Dictionary<int, Test> MakeDict()
    {
        Dictionary<int, Test> testDict = new Dictionary<int, Test>();
        foreach (Test test in testList)
            testDict.Add(test.intValue, test);
        return testDict;
    }
}
#endregion