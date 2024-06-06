public interface IDamagable
{
    public float NetworkedHP { get; set; }
    public bool IsMonsterCrystal { get; set; }
    public int TeamID { get; set; }
    public float GetDamagedHP(float power);
    void DealDamageRpc(float damagedHP);
    void UpdateHPGage();
}
