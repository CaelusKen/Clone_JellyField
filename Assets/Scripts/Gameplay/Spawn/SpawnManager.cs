using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject jellyPrefab;
    public Transform spawnPoint;
    public LevelData levelData;

    private int jellyIndex = 0; // Keep track of which jelly to spawn from initialSpawnJellies
    private bool isWaitingForPlacement = false; // Ensure new jelly spawns only after placement
    private bool allInitialJelliesSpawned = false; // Track if all initial jellies have been spawned

    public void SpawnNextJelly()
    {
        if (isWaitingForPlacement) return; // Don't spawn a new jelly until the previous one is placed

        JellyColor[] jellyColors;

        if (!allInitialJelliesSpawned && jellyIndex < levelData.initialSpawnJellies.Length)
        {
            // Spawn the next jelly from the initial spawn data
            jellyColors = levelData.initialSpawnJellies[jellyIndex].quarters;
            jellyIndex++;

            Debug.Log("Spawning initial jelly " + jellyIndex);

            // Check if we've spawned all initial jellies
            if (jellyIndex >= levelData.initialSpawnJellies.Length)
            {
                allInitialJelliesSpawned = true; // Mark that all initial jellies are now spawned
            }
        }
        else
        {
            // If all initial jellies have been spawned, spawn random jellies
            jellyColors = new JellyColor[4];
            for (int i = 0; i < 4; i++)
            {
                jellyColors[i] = levelData.GetRandomJellyColor();
            }
            Debug.Log("Spawning random jelly");
        }

        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, -0.5f);
        GameObject jelly = Instantiate(jellyPrefab, spawnPosition, Quaternion.identity);
        Jelly jellyComponent = jelly.GetComponent<Jelly>();

        if (jellyComponent != null)
        {
            jellyComponent.SetupJelly(jellyColors);
        }

        isWaitingForPlacement = true; // Wait for this jelly to be placed before spawning the next one
    }

    // This method should be called when the jelly has been successfully placed
    public void OnJellyPlaced()
    {
        isWaitingForPlacement = false; // Allow the next jelly to spawn
        Debug.Log("Jelly placed, ready to spawn next jelly");
        SpawnNextJelly(); // Spawn the next jelly after the current one is placed
    }
}
