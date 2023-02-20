public interface ICharacter
{
    float Health { get; }                   // Default Health
    float CurrentHealth { get; set; }       // Current Health

    void Direction(DirectionTypes type);    // Select the Direction of the Character's gaze
    void Move();                            // Ñharacter movement
    void Jump();                            // Ñharacter jump
    void Attack(WeaponTypes type);          // Shooting from the selected Weapon
    void Attack();                          // Shooting with some active Weapons
    Weapon GetActiveWeapon();               // Returns the current active Weapon
}
