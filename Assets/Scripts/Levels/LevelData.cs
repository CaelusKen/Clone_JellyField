using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 0)]
public class LevelData : ScriptableObject 
{
    public int levelNumber;
    public int gridRows;
    public int gridColumns;

    // List of blocked positions for special grid designs
    public List<Vector2Int> blockedPositions;

    // This method can be used to check if a position is blocked
    public bool IsCellBlocked(int x, int y)
    {
        return blockedPositions.Contains(new Vector2Int(x, y));
    }

    // New field for controlling initial jelly spawning
    public bool initialSpawn;
    public InitialJellyData[] initialSpawnJellies; // Jellies to spawn initially
    public List<JellyData> startingJellies; // The jellies to be placed in the grid

    // List for jelly objectives (key is the JellyColor, value is the amount needed)
    public List<JellyObjective> jellyObjectives;

    // Add a method to get a random jelly color
    public JellyColor GetRandomJellyColor()
    {
        var colors = new List<JellyColor> { JellyColor.Red, JellyColor.Green, JellyColor.Blue, JellyColor.Yellow, JellyColor.Purple, JellyColor.Pink, JellyColor.Orange };
        return colors[Random.Range(0, colors.Count)];
    }
}

[System.Serializable]
public class JellyData
{
    public Vector2Int gridPosition;
    public JellyColor[] quarters; // 4 quarters of the jelly
}

//This is the initial jellies that will be spawned first.
[System.Serializable]
public class InitialJellyData
{
    public JellyColor[] quarters; // 4 quarters of the jelly
}

[System.Serializable]
public class JellyObjective
{
    public JellyColor color;
    public int requiredAmount;
}

public enum JellyColor { None, Red, Blue, Green, Yellow, Purple, Pink, Orange }
