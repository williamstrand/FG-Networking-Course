using Unity.Netcode;
using UnityEngine;

namespace ResourcePacks
{
    public class HealthPack : NetworkBehaviour
    {
        [SerializeField] int healAmount = 25;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            healAmount = 25;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer) return;

            if (!other.TryGetComponent(out Health health)) return;

            health.Heal(healAmount);

            var networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}