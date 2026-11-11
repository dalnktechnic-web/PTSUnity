using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float respawnDelay = 3f;

    void Start()
    {
        // Hanya memanggil fungsi spawn awal di sini
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
                CreateEnemy(point);
        }
    }

    void CreateEnemy(Transform point)
    {
        if (enemyPrefab == null || point == null)
        {
            Debug.LogError("Enemy Prefab atau Spawn Point belum diisi!");
            return;
        }

        GameObject enemy = Instantiate(
            enemyPrefab,
            point.position,
            point.rotation
        );

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            // Menghubungkan spawner dan titik spawn ke musuh tersebut
            movement.Initialize(this, point);
        }
        else
        {
            Debug.LogError("EnemyMovement tidak ditemukan pada Prefab!");
        }
    }

    // Fungsi ini HANYA dipanggil ketika musuh mati
    public void RespawnEnemy(Transform point)
    {
        StartCoroutine(RespawnCoroutine(point));
    }

    IEnumerator RespawnCoroutine(Transform pointToRespawn)
    {
        // Menunggu beberapa detik sebelum memunculkan musuh baru
        yield return new WaitForSeconds(respawnDelay);

        if (pointToRespawn != null)
        {
            CreateEnemy(pointToRespawn);
        }
        else if (spawnPoints.Length > 0)
        {
            // Fallback ke titik acak jika titik asalnya null
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            CreateEnemy(randomPoint);
        }
    }
}