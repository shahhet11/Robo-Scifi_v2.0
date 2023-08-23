using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).y;

        mOffset = gameObject.transform.position - GetMouseWorldPos();


    }

    void OnMouseDrag()
    {

        //Vector3 move = GetMouseWorldPos() + mOffset;
        //Debug.Log(move + "move ");
        Vector3 move = new Vector3(GetMouseWorldPos().x - mOffset.x, 0f, GetMouseWorldPos().y - mOffset.y);
        transform.position = move;          // GetMouseWorldPos() + mOffset;

        //transform.position = move;


    }

    private Vector3 GetMouseWorldPos()
    {
        // Pixel Coordinate(x,y)
        Vector3 mousepoint = Input.mousePosition;

        // z Coordinate of game object on screen
        mousepoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousepoint);
    }





















    //private void Update()
    //{
    //    Debug.Log(Input.mousePosition.z + "Input.mousePosition.z;");
    //}


    //Vector3 hitPoint;

    //void OnMouseDrag()
    //{

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
    //    Debug.Log(ray);

    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
    //    {

    //        if (hit.transform.gameObject.name == "CameraElasticPoint")
    //        {
    //            return;
    //        }
    //        else
    //        {
    //            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
    //            hitPoint = Input.mousePosition;
    //        }

    //    }
    //}











    //float dragSpeed = 20f;

    //private Vector3 dragOrigin;

    //void OnMouseDown()
    //{


    //    dragOrigin = gameObject.transform.position;

    //}
    //void OnMouseDrag()
    //{
    //    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    //    pos.z = pos.x;
    //    Vector3 move = new Vector3(pos.x * dragSpeed, transform.position.y, pos.z * -dragSpeed);
    //    transform.position = move;

    //}



    //Vector3 dist;
    //Vector3 startPos;
    //float posX;
    //float posZ;
    //float posY;
    //void OnMouseDown()
    //{
    //    startPos = transform.position;
    //    dist = Camera.main.WorldToScreenPoint(transform.position);
    //    posX = Input.mousePosition.x - dist.x;
    //    posY = Input.mousePosition.y - dist.y;
    //    posZ = Input.mousePosition.z - dist.z;
    //}

    //void OnMouseDrag()
    //{
    //    float disX = Input.mousePosition.x - posX;
    //    float disY = Input.mousePosition.y - posY;
    //    float disZ = Input.mousePosition.z - posZ;
    //    Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
    //    transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);
    //}




    //private GameObject _drag;
    //private Vector3 screenPosition;
    //private Vector3 offset;

    //private void Update()
    //{
    //    if (_drag == null && Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit, 100.0f))
    //        {
    //            if (gameObject == hit.transform.gameObject)
    //            {
    //                Debug.Log("hit");
    //                _drag = hit.transform.gameObject;
    //                screenPosition = Camera.main.WorldToScreenPoint(_drag.transform.position);
    //                offset = _drag.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
    //            }
    //        }
    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        _drag = null;
    //    }

    //    if (_drag != null)
    //    {
    //        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
    //        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
    //        _drag.transform.position = new Vector3(currentPosition.x, _drag.transform.position.y, currentPosition.z);
    //    }
    //}





    //Vector3 startPos;
    //Vector3 dist;

    //void OnMouseDown()
    //{
    //    startPos = Camera.main.WorldToScreenPoint(transform.position);
    //    dist = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, startPos.z));
    //}

    //void OnMouseDrag()
    //{
    //    Vector3 lastPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, startPos.z);
    //    transform.position = Camera.main.ScreenToWorldPoint(lastPos) + dist;
    //}
}
