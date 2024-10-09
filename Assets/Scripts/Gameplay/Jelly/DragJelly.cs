using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragJelly : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    private Vector3 offset;
    

    void OnMouseDown()
    {
        startPosition = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        offset = transform.position - mousePos;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
         isDragging = false;

        // Check for snapping to grid cells
        SnapToGrid();
    }

    void SnapToGrid()
    {
        // Define a snapping threshold
        float snapThreshold = 0.5f; // Adjust this value based on your grid size and spacing
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, snapThreshold);

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("GridCell")) // Ensure you're hitting a grid cell
            {
                // Snap the jelly to the center of the grid cell
                Vector3 targetPosition = hit.transform.position;
                targetPosition.z = -0.5f; // Maintain the z position
                transform.position = targetPosition;

                // Trigger check for matching (make sure to adjust this method in your GameManager)
                FindObjectOfType<GameManager>().CheckForMatches(this.gameObject);
                return; // Exit the loop once snapped
            }
        }

        // Return to the start position if not snapped
        transform.position = startPosition;
    }
}
