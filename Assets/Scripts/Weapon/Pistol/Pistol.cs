using UnityEngine;

public class Pistol : Weapon
{
    protected override GameObject ShootObject { get; } = Resources.Load<GameObject>("Prefabs/Weapons/PistolShoot");
    public override WeaponTypes Type { get; } = WeaponTypes.Pistol;
    public override string NameTag { get; } = "PistolShoot";
    public override string NameAttack { get; } = "Blaster";
    public override float RechargeTime { get; } = 1.3f;
    public override float Damage { get; } = 15f;
}
