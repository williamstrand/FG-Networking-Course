using Unity.Netcode;
using UnityEngine;

namespace ResourcePacks
{
    public class ShieldPack : NetworkBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer) return;

            if (!other.TryGetComponent(out Health health)) return;

            health.AddShield();

            var networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}