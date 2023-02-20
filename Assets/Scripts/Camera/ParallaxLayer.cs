using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Camera parallax-effect for background")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _paralaxEffect;

    private float _startPos, _length;


    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Background moving
    void FixedUpdate()
    {
        float temp = _camera.position.x * (1 - _paralaxEffect);
        float dist = _camera.position.x * _paralaxEffect;

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if (temp > _startPos + _length)
            _startPos += _length;
        else if (temp < _startPos - _length)
            _startPos -= _length;
    }
}
