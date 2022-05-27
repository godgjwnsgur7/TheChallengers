using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{
    public abstract void Open<T>() where T : UIPopup;
    public abstract void Close<T>() where T : UIPopup;
}
