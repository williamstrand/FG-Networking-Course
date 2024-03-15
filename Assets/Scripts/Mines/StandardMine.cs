using Unity.Netcode;
using UnityEngine;

public class StandardMine : NetworkBehaviour
{
    [SerializeField] GameObject minePrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        var health = other.GetComponent<Health>();
        if (!health) return;

        health.TakeDamage(25);

        var xPosition = Random.Range(-4, 4);
        var yPosition = Random.Range(-2, 2);

        var newMine = Instantiate(minePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        var no = newMine.GetComponent<NetworkObject>();
        no.Spawn();

        var networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}