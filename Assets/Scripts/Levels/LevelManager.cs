using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData;
    public GameObject gridCellPrefab;
    public GameObject jellyPrefab;
    private GameObject[,] playGrid;

    public SpawnManager spawnManager;
    public UIManager uiManager;

    private Dictionary<JellyColor, int> collectedJellies; // Tracks collected jellies
    private GameObject currentDraggedJelly;

    public float cellSpacing = 0.1f; // Distance between cells

    void Start()
    {
        SetupGrid();
        InitializeObjectives();
        PlaceStartingJellies();
        SpawnFirstJelly(); 
        uiManager.UpdateLevelUI(currentLevelData.levelNumber); // Update level number
        UpdateJellyObjectiveUI(); // Initialize jelly objectives
    }

    void SetupGrid()
    {
        playGrid = new GameObject[currentLevelData.gridColumns, currentLevelData.gridRows];

        for (int x = 0; x < currentLevelData.gridColumns; x++)
        {
            for (int y = 0; y < currentLevelData.gridRows; y++)
            {
                if (!currentLevelData.IsCellBlocked(x, y))
                {
                    Vector3 spawnPos = new Vector3(x + x * cellSpacing, y + y * cellSpacing, 0);
                    GameObject cell = Instantiate(gridCellPrefab, spawnPos, Quaternion.identity);
                    playGrid[x, y] = cell;
                }
            }
        }
    }

    void SpawnFirstJelly()
    {
        spawnManager.SpawnNextJelly(); // Start spawning the first jelly
    }

    public void OnJellyPlacedOnGrid()
    {
        spawnManager.OnJellyPlaced();
        Debug.Log("Jelly placed on grid, calling SpawnManager to spawn next jelly");
    }

    void PlaceStartingJellies()
    {
        foreach (var jellyData in currentLevelData.startingJellies)
        {
            Vector3 jellyPos = new Vector3(jellyData.gridPosition.x + jellyData.gridPosition.x * cellSpacing,
                                           jellyData.gridPosition.y + jellyData.gridPosition.y * cellSpacing,
                                           -0.5f); 

            GameObject jelly = Instantiate(jellyPrefab, jellyPos, Quaternion.identity);
            Jelly jellyComponent = jelly.GetComponent<Jelly>();
            jellyComponent.SetupJelly(jellyData.quarters); 
            jelly.transform.SetParent(playGrid[jellyData.gridPosition.x, jellyData.gridPosition.y].transform);
        }
    }

    void InitializeObjectives()
    {
        collectedJellies = new Dictionary<JellyColor, int>();

        foreach (var objective in currentLevelData.jellyObjectives)
        {
            collectedJellies[objective.color] = 0;
        }
    }

    public void OnJellyCollected(Dictionary<JellyColor, int> poppedQuarters)
    {
        foreach (var entry in poppedQuarters)
        {
            JellyColor color = entry.Key;
            int quartersPopped = entry.Value;

            if (collectedJellies.ContainsKey(color))
            {
                collectedJellies[color] += quartersPopped;
            }
        }

        // Update UI after jellies are collected
        UpdateJellyObjectiveUI();
        CheckObjectives(); // Check if the objectives are completed
    }

    void CheckObjectives()
    {
        foreach (var objective in currentLevelData.jellyObjectives)
        {
            if (collectedJellies[objective.color] < objective.requiredAmount)
            {
                return; // Exit if any objective isn't completed yet
            }
        }

        // All objectives are completed
        Debug.Log("Level Completed!");
        uiManager.ShowCompleteLevelPanel();
    }

    void UpdateJellyObjectiveUI()
    {
        uiManager.UpdateJellyObjective(collectedJellies, GetObjectiveDictionary()); // Update the UI with jelly collection progress
    }

    // Helper function to get the objective dictionary from the current level data
    Dictionary<JellyColor, int> GetObjectiveDictionary()
    {
        Dictionary<JellyColor, int> totalObjectives = new Dictionary<JellyColor, int>();
        foreach (var objective in currentLevelData.jellyObjectives)
        {
            totalObjectives[objective.color] = objective.requiredAmount;
        }
        return totalObjectives;
    }
}
