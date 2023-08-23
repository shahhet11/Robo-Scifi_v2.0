using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosSelect : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        GameObject pa;
        pa = GameObject.Find("AngryGrandPa");

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.gameObject.transform.position, pa.transform.position);
    }
}
