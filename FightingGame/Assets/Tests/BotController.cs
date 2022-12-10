using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MonoBehaviourTestHelper : MonoBehaviour, IMonoBehaviourTest
{
    public bool IsTestFinished
	{
        get
		{
			return true;
		}
	}

	public T Find<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }
}
