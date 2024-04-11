using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Prefab for the zombie enemy
    public Transform target; // Target to follow
    public int maximumZombies = 5; // Maximum number of zombies allowed at once
    public float spawnInterval = 10f; // Time interval between spawning zombies

    private int currentZombies = 0; // Current number of spawned zombies

    private Transform spawnPoint; // Point where zombies will spawn

    private void Start()
    {
        spawnPoint = GetComponent<Transform>();
        // Start spawning zombies repeatedly
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            // Check if we can spawn more zombies
            if (currentZombies < maximumZombies)
            {
                // Instantiate a new zombie at the spawn point
                GameObject enemy = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

                AIZombieBehaviorScript aiZombieBehaviorScript = enemy.GetComponent<AIZombieBehaviorScript>();

                if (aiZombieBehaviorScript != null) aiZombieBehaviorScript.target = target;

                currentZombies++;
            }

            // Wait for the specified interval before spawning the next zombie
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Call this method when a zombie is killed to decrease the count
    public void ZombieKilled()
    {
        currentZombies--;
        // Ensure the count doesn't go below 0
        currentZombies = Mathf.Max(currentZombies, 0);
    }

}