using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public enum ENUM_INTERACTION_TYPE
{
    Weapon = 0,
}

public class InteractableObject : MonoBehaviour
{
    public ENUM_INTERACTION_TYPE interactionType;

    public bool isInteractableState = false;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        gameObject.layer = (int)ENUM_LAYER_TYPE.Interaction;
        isInteractableState = true;
    }
    
    public virtual void Interact()
    {
        isInteractableState = false;
        Managers.UI.CloseUI<InteractableUI>();
    }

    public virtual void EndInteract()
    {
        // 음 쓸 일이 있을까 싶기는 한데 흠...
    }

    public void SetInteractable()
    {
        Managers.UI.OpenUI<InteractableUI>();
    }

    public void UnsetInteractable()
    {
        Managers.UI.CloseUI<InteractableUI>();
    }
}
