using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public interface ISubject
{
    void ResisterObserver(IObserver _observer);
    void RemoveObserver(IObserver _observer);
    void NotifyObserver();
}

public interface IObserver
{
    void Update_BGM();
}
