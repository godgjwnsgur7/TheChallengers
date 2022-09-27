using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;
    [SerializeField] GameObject PopupWindow;

    // 팝업 UI가 어떤 크기든 맞춰서 Popup Window의 등장 이펙트 사라지는 이펙트 구현
    // 등장하고 사라지는데에는 0.5초 ~ 1초 사이의 시간 안에 할 것.
    // 이쁘게 만들어주세엽 (우진)
    // + 활성화 시키는 시점도 여기서 제어할 예정

    public void Open_Effect()
    {
        
    }

    public void Close_Effect()
    {

    }
}
