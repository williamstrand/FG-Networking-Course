using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new();
    public NetworkVariable<int> currentShield = new();

    [SerializeField] int maxHealth = 100;
    [SerializeField] int maxShield = 2;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        currentHealth.Value = maxHealth;
        currentShield.Value = 0;
    }

    public void AddShield()
    {
        currentShield.Value = maxShield;
    }

    public void TakeDamage(int damage)
    {
        if (currentShield.Value > 0)
        {
            currentShield.Value--;
            return;
        }

        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;
    }

    public void Heal(int heal)
    {
        heal = heal > 0 ? heal : -heal;
        currentHealth.Value += heal;
    }
}