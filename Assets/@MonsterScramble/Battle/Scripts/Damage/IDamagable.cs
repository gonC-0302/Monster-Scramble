using Fusion;

public interface IDamagable
{
    public float NetworkedHP { get; set; }
    public int TeamID { get; set; }
    void DealDamageRpc(float damage, AttackManager attacker);
}
