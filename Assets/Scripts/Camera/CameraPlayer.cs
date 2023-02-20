using System;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Border limits of Camera")]
    [SerializeField] private float _leftLimit;
    [SerializeField] private float _rightLimit;
    [SerializeField] private float _upperLimit;
    [SerializeField] private float _bottomLimit;

    private Transform _player;


    void Start()
    {
        _player = Player.instance.transform;

        if (_player == null) throw new Exception("Please add the Player-object to the scene!");
    }

    void Update()
    {
        if (_player != null)
        {
            float xLimit = Mathf.Clamp(_player.position.x, _leftLimit, _rightLimit);
            float yLimit = Mathf.Clamp(_player.position.y, _bottomLimit, _upperLimit);

            transform.position = new Vector3(xLimit, yLimit, transform.position.z);
        }        
    }
}
