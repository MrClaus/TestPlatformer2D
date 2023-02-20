using System;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private HealthIndicatorTypes type;

    private Transform _lineId;
    private Vector3 _mScale, _scale, _position;
    private Character _character;
    

    private void Start()
    {
        _mScale = transform.localScale;

        foreach (Transform child in GetComponentsInChildren<Transform>())
            if (child.name.Equals("LiveId"))
            {
                _lineId = child;
                break;
            }

        _character = (type == HealthIndicatorTypes.PlayerUI) ? Player.instance.GetComponent<Character>() : transform.parent.GetComponent<Character>();

        if (_lineId != null)
        {
            _position = _lineId.transform.localPosition;
            _scale = _lineId.transform.localScale;
        }
        else
            throw new Exception("Please add Indicator-object (LiveId) in prefab");
        
        if (_character == null) 
            throw new Exception("Please add the Current-indicator to the existing Character-object");
    }

    private void Update()
    {        
        if (_character != null && _character.CurrentHealth >= 0)
        {
            float value = _character.CurrentHealth / _character.Health;
            float length = -(1 - value) / 2;
            _lineId.transform.localScale = new Vector3(value, _scale.y, _scale.z);
            _lineId.transform.localPosition = new Vector3(length, _position.y, _position.z);

            if (type == HealthIndicatorTypes.Mini)
            {
                float signScale = (_character.transform.localScale.x < 0) ? -Mathf.Abs(_mScale.x) : Mathf.Abs(_mScale.x);
                transform.localScale = new Vector3(signScale, _mScale.y, _mScale.z);
            }            
        }
        else if (type == HealthIndicatorTypes.PlayerUI && Player.instance != null)
            _character = Player.instance.GetComponent<Character>();
    }
}
