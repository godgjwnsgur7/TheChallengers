using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class WeaponObject : InteractableObject
{
    [SerializeField] ENUM_WEAPON_TYPE weaponType;

    private float rotateSpeed = 100.0f;

    public override void Init()
    {
        base.Init();

        interactionType = ENUM_INTERACTION_TYPE.Weapon;
    }

    public override void Interact()
    {
        if (!isInteractableState) return;

        Managers.Resource.Destroy(gameObject);

        base.Interact();

    }

    public override void EndInteract()
    {

    }

    public ENUM_WEAPON_TYPE GetInfo()
    {
        return weaponType;
    }
}
