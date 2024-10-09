using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    public JellyColor[] quarters = new JellyColor[4]; // 4 quarters
    public SpriteRenderer[] quarterRenderers; // Reference to quarter visuals
    private bool isMatched = false;
    private LevelManager levelManager;
    private bool isPlaced = false; // New flag to track if the jelly has been placed

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void SetupJelly(JellyColor[] newQuarters)
    {
        quarters = newQuarters;
        for (int i = 0; i < quarters.Length; i++)
        {
            quarterRenderers[i].color = GetColorFromEnum(quarters[i]); // Update visuals
        }
    }

    Color GetColorFromEnum(JellyColor colorEnum)
    {
        switch (colorEnum)
        {
            case JellyColor.Red: return Color.red;
            case JellyColor.Blue: return Color.blue;
            case JellyColor.Green: return Color.green;
            case JellyColor.Yellow: return Color.yellow;
            case JellyColor.Purple: return new Color(0.5f, 0.0f, 0.5f); // Custom RGB for Purple
            case JellyColor.Pink: return new Color(1.0f, 0.75f, 0.8f); // Custom RGB for Pink
            case JellyColor.Orange: return new Color(1.0f, 0.5f, 0.0f); // Custom RGB for Orange
            default: return Color.clear;
        }
    }

    public bool CheckMatch(Jelly otherJelly)
    {
        for (int i = 0; i < 4; i++)
        {
            if (quarters[i] == otherJelly.quarters[i])
                return true;
        }
        return false;
    }

    public void PopJelly()
    {
        isMatched = true;

        // Notify the LevelManager of the quarters that were popped
        Dictionary<JellyColor, int> poppedQuarters = new Dictionary<JellyColor, int>();

        // Count how many quarters of each color exist in the jelly
        foreach (JellyColor color in quarters)
        {
            if (!poppedQuarters.ContainsKey(color))
            {
                poppedQuarters[color] = 0;
            }
            poppedQuarters[color]++;
        }

        // Notify the LevelManager
        levelManager.OnJellyCollected(poppedQuarters);

        // Trigger animation or visual change before destroying
        Destroy(gameObject, 0.5f); // Delay before destruction
    }

    public void Extend()
    {
        // Logic to extend jelly (quarter -> half -> full)
    }

    // Call this method when jelly is successfully placed on the grid
    public void OnPlacedOnGrid()
    {
        if (!isPlaced) // Only trigger once
        {
            isPlaced = true;
            levelManager.OnJellyPlacedOnGrid();
            Debug.Log("Jelly placed on the grid, calling LevelManager to notify placement");
        }
    }

    private void OnMouseUp()
    {
        // Simulate placement when the jelly is dropped
        OnPlacedOnGrid();
    }
}
