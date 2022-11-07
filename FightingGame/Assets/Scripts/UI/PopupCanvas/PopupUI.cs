using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;

    [SerializeField] GameObject popupWindow;

    private void OnEnable() => isUsing = true;

    private void OnDisable() => isUsing = false;
}
