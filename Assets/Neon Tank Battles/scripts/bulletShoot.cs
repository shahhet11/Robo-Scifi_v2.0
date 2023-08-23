using UnityEngine;
using System.Collections;

public class bulletShoot : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject wallHitParticle;

    public float speed;


    // Use this for initialization
    void Start()
    {
        //transform.rotation = Quaternion.AngleAxis(Vars.angle + 180, Vector3.forward);
        //rb.AddRelativeForce(Vector3.up * 0.12f);

        //rb.velocity = transform.TransformDirection(Vector2.down * speed);

        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("enemy"))
        {
            GameObject p = Instantiate(wallHitParticle, collision.contacts[0].point, Quaternion.identity);
            Destroy(p, 1f);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            GameObject p = Instantiate(wallHitParticle, collision.contacts[0].point, Quaternion.identity);
            Destroy(p, 1f);

            Destroy(gameObject);
        }
    }
}