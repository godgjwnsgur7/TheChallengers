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

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public override void Interact()
    {
        if (!isInteractableState) return;

        base.Interact();

        Managers.Resource.Destroy(gameObject);
    }

    public override void EndInteract()
    {

    }

    public ENUM_WEAPON_TYPE GetInfo()
    {
        return weaponType;
    }
}
