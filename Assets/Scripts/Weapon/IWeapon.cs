public interface IWeapon
{
    WeaponTypes Type { get; }       // Type of Weapon
    string NameTag { get; }         // Name of the Shoot-object Tag
    string NameAttack { get; }      // Name of the Character animation (from Animator) for this Weapon
    float RechargeTime { get; }     // Weapon recharging time
    float Damage { get; }           // Shoot-object Damage value

    bool Attack();                                      // Call Attack-process - if the weapon has been recharged (RechargeTime)
    void Direction(DirectionTypes type);                // Setting Direction of the Weapon
    void SetPosition(UnityEngine.Transform position);   // The position from where the bullet (Shoot-object) should fly is set
    void SetActive(bool isActive);                      // Updating the weapon status - is the weapon activated or not
    bool IsActive();                                    // Returns 'true' - if the weapon is picked up or initially active in the inventory
}
