using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCanvas : MonoBehaviour
{
    [SerializeField] BlackOutPopup blackOut;
    [SerializeField] CountDownPopup countDownPopup;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        DontDestroyOnLoad(this);
    }

    public void Open<T>()
    {
        if (typeof(T) == typeof(BlackOutPopup)) blackOut.Open();
        else if (typeof(T) == typeof(CountDownPopup)) countDownPopup.Open();
        else
        {
            Debug.Log("범위 벗어남");
            return;
        }
    }

    public void Close<T>()
    {
        if (typeof(T) == typeof(BlackOutPopup)) blackOut.Close();
        else if (typeof(T) == typeof(CountDownPopup)) countDownPopup.Close();
        else
        {
            Debug.Log("범위 벗어남");
            return;
        }
    }

}
