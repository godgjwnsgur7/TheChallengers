using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private SpriteRenderer eRenderer;
    private void Awake()
    {
        eRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}
