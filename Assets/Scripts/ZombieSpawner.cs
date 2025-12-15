using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public int maxZombieSpawn = 5;
    public float spawnInterval = 5f;
    float nextSpawnTime = 0f;
    public GameObject ZombiePrefab;
    int zombieSpawned = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if(Time.time >= nextSpawnTime && zombieSpawned < maxZombieSpawn)
        {
            Instantiate(ZombiePrefab, transform.position, Quaternion.identity);
            nextSpawnTime = Time.time + spawnInterval;
            zombieSpawned++;
        }    
    }
}
