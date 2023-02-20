using System;
using UnityEngine;

public class Enemy : Character
{   
    [Header("Visibility Distance")]
    [SerializeField] private Transform _eyePosition;
    [SerializeField] private float _visibility;


    private new void Start()
    {
        base.Start();
        UIService.isRestarted = false;

        if (Player.instance == null) throw new Exception("Please add the Player-object to the scene!");
    }

    private void Update()
    {
        if (Player.instance != null)
        {
            float delta = transform.position.x - Player.instance.transform.position.x;
            DirectionTypes type = (delta > 0) ? DirectionTypes.Left : DirectionTypes.Right;
            Direction(type);

            RaycastHit2D hit = Physics2D.Raycast(_eyePosition.position, Vector2.right * (float)type, _visibility);

            if (hit.collider != null && hit.collider.tag.Equals("Player"))
                Attack();
        }
    }

    private void OnDestroy()
    {
        if (UIService.instance != null)
            if (!UIService.isRestarted)
                UIService.instance.EnemyDeath();
    }
}
