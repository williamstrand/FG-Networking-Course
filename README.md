# FG-Networking-Course
 
## Score
Im trying to achieve G with 6 points.

## 1. Limited Ammo and Shot Timer: [Points: 2]
I created an [Ammo](Assets/Scripts/Player/Ammo.cs) class that is similar to the [Health](Assets/Scripts/Player/Health.cs) class.
The Ammo class has a network variable `CurrentAmmo`.

The `ShootLocalBullet()` method checks if the player has ammo and that the player has not shot recently before shooting a bullet.
```csharp
void ShootLocalBullet()
{
    if (!ammo.HasAmmo()) return;
    if (fireTimer.Value < fireRate) return;

    var bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    ShootBulletServerRpc();
}
```
The when shooting the server bullet, the ammo is used and the fire timer is reset.

```csharp
[ServerRpc]
void ShootBulletServerRpc()
{
    ammo.UseAmmo();
    fireTimer.Value = 0;

    var bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
    ShootBulletClientRpc();
}
```
In [Ammo](Assets/Scripts/Player/Ammo.cs)
```csharp
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
```
## 2. Ammo Pickups: [Points: 1]