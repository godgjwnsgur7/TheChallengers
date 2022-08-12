using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] InteractableUI interactableUI;
    [SerializeField] ButtonPanel buttonPanel;

    Transform BTransform;
    bool isPanelShow;

    public void init()
    {
        BTransform = buttonPanel.gameObject.transform;
        isPanelShow = false;
    }

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Open(param);
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Close();
        else Debug.Log("범위 벗어남");
    }

    public void SlidePanel()
    {
        if(isPanelShow)
            BTransform.Translate(-400f,0,0);
        else
            BTransform.Translate(400f, 0, 0);

        isPanelShow = !isPanelShow;
    }
}
