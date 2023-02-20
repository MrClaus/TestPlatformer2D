using System;
using UnityEngine;

public abstract class Weapon : IWeapon
{
    public abstract WeaponTypes Type { get; }           // Name of the Shoot-object Tag
    public abstract string NameTag { get; }             // Name of the Shoot-object Tag
    public abstract string NameAttack { get; }          // Name of the Character animation (from Animator) for this Weapon
    public abstract float RechargeTime { get; }         // Weapon recharging time
    public abstract float Damage { get; }               // Shoot-object Damage value

    protected abstract GameObject ShootObject { get; }  // Bullet Prefab (Shoot-object)

    private bool _isActive;    
    private Transform _position;
    private DirectionTypes _direction;
    private float _deltaTime;
    private DateTime _mTime;    


    public Weapon()
    {
        _direction = DirectionTypes.Right;
        _mTime = DateTime.Now;
    }

    // Call ToAttack() - if the weapon has been recharged (RechargeTime)
    public bool Attack()
    {
        _deltaTime += (float)(DateTime.Now - _mTime).TotalMilliseconds / 1000;
        _mTime = DateTime.Now;

        if (_deltaTime > RechargeTime)
        {
            _deltaTime = 0f;
            ToAttack();
            return true;
        }
        else
            return false;
    }

    // Setting Direction of the Weapon
    public void Direction(DirectionTypes type)
    {
        _direction = type;
    }

    // The position from where the bullet (Shoot-object) should fly is set
    public void SetPosition(Transform position)
    {
        _position = position;
    }

    // Updating the weapon status - is the weapon activated or not
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    // Returns 'true' - if the weapon is picked up or initially active in the inventory
    public bool IsActive()
    {
        return _isActive;
    }

    // Attack scenario
    protected void ToAttack()
    {
        float angleÂirection = (GetDirection() == DirectionTypes.Right) ? 0 : 180;
        GameObject shoot = UnityEngine.Object.Instantiate(ShootObject, GetPosition().position, Quaternion.Euler(0, 0, angleÂirection));
        shoot.tag = NameTag;
    }

    // Returns Direction of the Weapon
    protected DirectionTypes GetDirection()
    {
        return _direction;
    }

    // Returns respawn position for bullet (Shoot-object)
    protected Transform GetPosition()
    {
        return _position;
    }    
}
