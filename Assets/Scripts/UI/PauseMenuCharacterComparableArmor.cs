public class PauseMenuCharacterComparableArmor : PauseMenuCharacterComparableAbstract
{
    public void Setup(ArmorSO armorSo)
    {
        base.Setup(armorSo);

        SetupBlunt(armorSo.BluntResistance);
        SetupPiercing(armorSo.PierceResistance);
        SetupExplosive(armorSo.ExplosiveResistance);
        SetupFire(armorSo.FireResistance);
        SetupIce(armorSo.IceResistance);
        SetupLightning(armorSo.LightningResistance);
        SetupEarth(armorSo.EarthResistance);
        SetupVoid(armorSo.VoidResistance);
    }
}
