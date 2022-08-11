using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour
{
    [SerializeField] public BoxCollider2D bound;
    [SerializeField] public Transform redTeamSpawnPoint;
    [SerializeField] public Transform blueTeamSpawnPoint;

    public Vector2 minBound;
    public Vector2 maxBound;
    
    private void Awake()
    {
        minBound = new Vector2(bound.bounds.min.x, bound.bounds.min.y);
        maxBound = new Vector2(bound.bounds.max.x, bound.bounds.max.y);
    }
}
