# FG-Networking-Course
 
## Score
Im trying to achieve G with 6 points.

## 1. Limited Ammo and Shot Timer: [Points: 2(1/1)]
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
## 2. Ammo/Health/Shield Pickups: [Points: 4(1/1/2)]
I created [AmmoPack](Assets/Scripts/ResourcePacks/AmmoPack.cs), [HealthPack](Assets/Scripts/ResourcePacks/HealthPack.cs), and [ShieldPack](Assets/Scripts/ResourcePacks/ShieldPack.cs) classes that are similar to the [Ammo](Assets/Scripts/Player/Ammo.cs) and [Health](Assets/Scripts/Player/Health.cs) classes.
They have a OnTriggerEnter2D method that checks if the player is the one that collided with the pickup.

The ammo pack gets the player's ammo component and reloads the player's ammo.
```csharp
if (!other.TryGetComponent(out Ammo ammo)) return;

ammo.Reload();
```
The health pack get the player's health component and heals the player.
```csharp
if (!other.TryGetComponent(out Health health)) return;

health.Heal(healAmount);
```
For the shield pack I added a currentShield network variable to the player's health component.
When a player collides with the shield pack, the player's shield is set to the max shield value.

When the player takes damage, the shield is checked first and if the shield is greater than 0, the player does not take damage.
```csharp
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
```