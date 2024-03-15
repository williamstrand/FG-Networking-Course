using Player;
using Unity.Netcode;
using UnityEngine;

namespace ResourcePacks
{
    public class AmmoPack : NetworkBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer) return;

            if (!other.TryGetComponent(out Ammo ammo)) return;

            ammo.Reload();

            var networkObject = gameObject.GetComponent<NetworkObject>();
            networkObject.Despawn();
        }
    }
}