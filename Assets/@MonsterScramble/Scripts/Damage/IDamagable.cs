using Fusion;

public interface IDamagable
{
    public int MaxHP { get; set; }
    public float NetworkedHP { get; set; }
    public bool IsSealedMonster { get; set; }
    public int TeamID { get; set; }
    void DealDamageRpc(float damage);
    //void UpdateHPGage();
}
