using Player;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Ammo ammo;
    [SerializeField] float fireRate = .5f;

    NetworkVariable<float> fireTimer = new();

    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;

        if (!IsServer) return;

        fireTimer.Value = fireRate;
    }

    void Fire(bool isShooting)
    {
        if (isShooting)
        {
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    void ShootBulletServerRpc()
    {
        ammo.UseAmmo();
        fireTimer.Value = 0;

        var bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();
    }

    [ClientRpc]
    void ShootBulletClientRpc()
    {
        if (IsOwner) return;

        var bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    void ShootLocalBullet()
    {
        if (fireTimer.Value < fireRate) return;
        if (!ammo.CanUseAmmo()) return;

        var bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }

    void Update()
    {
        if (!IsServer) return;

        if (fireTimer.Value < fireRate)
        {
            fireTimer.Value += Time.deltaTime;
        }
    }
}