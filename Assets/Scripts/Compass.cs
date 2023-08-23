using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    private void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert mouse position to world space
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the angle between the mouse direction and the compass needle's forward direction
        Vector3 mouseDir = worldMousePosition - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, mouseDir, Vector3.up);

        // Apply the calculated angle to the compass needle's rotation
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
