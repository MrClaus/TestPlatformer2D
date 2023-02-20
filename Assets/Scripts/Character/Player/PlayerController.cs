using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    void Start()
    {
        _player = GetComponent<Player>();
        if (_player == null) throw new Exception("'PlayerController' is not connected to the 'Player'");
    }

    void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0)
        {
            _player.Direction(DirectionTypes.Right);
            _player.Move();
        }
        if (horizontal < 0)
        {
            _player.Direction(DirectionTypes.Left);
            _player.Move();
        }

        if (vertical > 0)
            _player.Jump();

        if (Input.GetKey(KeyCode.Space))
            _player.Attack();
    }
}
