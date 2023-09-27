using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 10f, throwForce = 100f, lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Rigidbody grabbedRB;

    void Update()
    {
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 100f;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        if (grabbedRB)
        {
            //Vector3 grabbedposition = new Vector3(grabbedRB.position.x, 5f, grabbedRB.position.z);
            //Vector3 objHolderposition = new Vector3(grabbedRB.position.x, 5f, grabbedRB.position.z);

          
            grabbedRB.MovePosition(Vector3.Lerp(grabbedRB.position, objectHolder.position, Time.deltaTime * lerpSpeed));
          

            if (Input.GetMouseButtonDown(1))
            {
                grabbedRB.isKinematic = false;
                Debug.Log(PlayerTransform.forward);
                grabbedRB.AddForce(PlayerTransform.forward * throwForce, ForceMode.Impulse);
                grabbedRB = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (grabbedRB)
            {
                grabbedRB.isKinematic = false;
                grabbedRB = null;
            }
            else
            {
                //Debug.Log("Ray Hit0");

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 100f;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log(camRay+"CamRay");
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("Ray Hit");
                    grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (grabbedRB)
                    {
                        grabbedRB.isKinematic = true;
                    }
                }
            }
        }
    }
}