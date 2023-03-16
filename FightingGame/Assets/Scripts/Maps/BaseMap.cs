using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MonoBehaviour
{
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

    private void Start()
    {
        Managers.Sound.Play_BGM(FGDefine.ENUM_BGM_TYPE.Battle); // 임시
    }
}
