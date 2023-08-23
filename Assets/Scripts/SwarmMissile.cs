using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmMissile : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float destroyTime;

    private Rigidbody rb;
    private Vector3 myVelocity;
    private float timerT;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (destroyTime > 0f)
        {
            timerT += Time.deltaTime;

            if (timerT > destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        myVelocity = rb.velocity;
        myVelocity.x += Random.Range(-10f, 10f) * MoveSpeed * Time.fixedDeltaTime;
        rb.velocity = myVelocity;
    }
}
