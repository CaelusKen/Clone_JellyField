using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;

    public void CheckForMatches(GameObject placedJelly)
    {
        Jelly jelly = placedJelly.GetComponent<Jelly>();
        List<Jelly> matchedJellies = new List<Jelly>();

        // Check all adjacent jellies for a match
        Vector2 jellyPos = placedJelly.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(jellyPos, 1f); // Find nearby jellies

        foreach (Collider2D collider in colliders)
        {
            Jelly nearbyJelly = collider.GetComponent<Jelly>();
            if (nearbyJelly != null && jelly.CheckMatch(nearbyJelly))
            {
                matchedJellies.Add(nearbyJelly);
            }
        }

        // If we found at least 4 matches, pop them
        if (matchedJellies.Count >= 4)
        {
            foreach (Jelly matchedJelly in matchedJellies)
            {
                matchedJelly.PopJelly();
            }
            // Extend the jellies around them
            ExtendNearbyJellies(matchedJellies);
        }
    }

    private void ExtendNearbyJellies(List<Jelly> poppedJellies)
    {
        foreach (Jelly jelly in poppedJellies)
        {
            // Find adjacent jellies and extend them
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(jelly.transform.position, 1f);
            foreach (Collider2D collider in nearbyColliders)
            {
                Jelly nearbyJelly = collider.GetComponent<Jelly>();
                if (nearbyJelly != null && !poppedJellies.Contains(nearbyJelly))
                {
                    nearbyJelly.Extend();
                }
            }
        }
    }

    public void GameOver()
    {
        uiManager.ShowGameOver();
    }
}
