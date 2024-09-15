using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class BaseMap : MonoBehaviour
{
    [SerializeField] ENUM_MAP_TYPE mapType;

    public SpriteRenderer backgroundMapSprite;
    public Transform redTeamSpawnPoint;
    public Transform blueTeamSpawnPoint;

    public Vector2 minBound;
    public Vector2 maxBound;
    
    private void Awake()
    {
        minBound = new Vector2(backgroundMapSprite.bounds.min.x, backgroundMapSprite.bounds.min.y);
        maxBound = new Vector2(backgroundMapSprite.bounds.max.x, backgroundMapSprite.bounds.max.y);
    }

    public ENUM_MAP_TYPE Get_MapType() => mapType;
}