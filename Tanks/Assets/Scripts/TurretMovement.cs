using UnityEngine;

public class TurretControl : MonoBehaviour
{
    public Transform turningPoint; // Reference to the TurningPoint GameObject

    void Update()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate direction from turningPoint to mouse
        Vector3 direction = mousePosition - turningPoint.position;

        // Calculate the angle and apply rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        turningPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
