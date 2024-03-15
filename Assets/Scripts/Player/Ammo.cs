using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class Ammo : NetworkBehaviour
    {
        public NetworkVariable<int> CurrentAmmo = new();

        [SerializeField] int maxAmmo = 5;
        [SerializeField] float reloadTime = 1f;

        bool IsReloading => reloadTimer < reloadTime;
        float reloadTimer;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;

            CurrentAmmo.Value = maxAmmo;
            reloadTimer = reloadTime;
        }

        public bool CanUseAmmo()
        {
            return CurrentAmmo.Value > 0;
        }

        public void UseAmmo()
        {
            if (IsReloading) return;

            CurrentAmmo.Value--;

            if (CurrentAmmo.Value <= 0)
            {
                StartReload();
            }
        }

        void StartReload()
        {
            reloadTimer = 0f;
        }

        public void Reload()
        {
            CurrentAmmo.Value = maxAmmo;
            reloadTimer = reloadTime;
        }

        void Update()
        {
            if (!IsServer) return;
            if (!IsReloading) return;

            reloadTimer += Time.deltaTime;
            if (!IsReloading)
            {
                Reload();
            }
        }
    }
}