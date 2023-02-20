using UnityEngine;

public class Blaster : Weapon
{
    protected override GameObject ShootObject { get; } = Resources.Load<GameObject>("Prefabs/Weapons/BlasterShoot");
    public override WeaponTypes Type { get; } = WeaponTypes.Blaster;
    public override string NameTag { get; } = "BlasterShoot";
    public override string NameAttack { get; } = "Blaster";
    public override float RechargeTime { get; } = 1f;
    public override float Damage { get; } = 20f;
}
