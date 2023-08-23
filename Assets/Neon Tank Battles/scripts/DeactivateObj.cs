using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObj : MonoBehaviour
{
    public float time;
    public AudioSource JetStop;
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(gameObject, time);
        Invoke("JetStopSound",0.9f);
    }
     void JetStopSound()
    {
        
            JetStop.Play();
     
    }
}
