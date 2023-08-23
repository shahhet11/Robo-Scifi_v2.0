using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysGun : MonoBehaviour
{
    
    private Vector3 mOffset;
    private float mZCoord;
    public float minimumY = 2.2f;
    public float maximumY = 18f;
    private void Start()
    {
        this.enabled = false;

    }
    //void Update()
    //{
    //    if (Input.GetMouseButton(1))
    void OnMouseDown()
            {
            mZCoord = Camera.main.WorldToScreenPoint(
            gameObject.transform.position).z;

            // Store offset = gameobject world pos - mouse world pos

            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            }
    //}

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
         Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
           mousePoint.z = mZCoord;

        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    void OnMouseDrag()
    {
        Vector3 temp = GetMouseAsWorldPoint() + mOffset;
        temp.y = Mathf.Clamp(temp.y, minimumY, maximumY);
        transform.position = new Vector3(temp.x, temp.y, temp.z);

        
    }
}
