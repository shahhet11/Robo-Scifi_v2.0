using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public Transform Target;
    public float stoppingDistance;
    public float RotationSpeed;
    float distance;
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 5f;
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;

            if(transform.name.Contains("Enemy2"))
            {
                navMeshAgent.speed = GameManager.Instance.RedAISpeed;
            }
            else
            {
                navMeshAgent.speed = GameManager.Instance.NormalAISpeed;
            }
        }

        Target = GameManager.Instance.MainPlayer.transform;
    }

    void Update()
    {
        if(Target == null)
        {
            return;
        }

        distance = Vector3.Distance(Target.position, transform.position);

        //Debug.Log("AIMOVEMENTS "+this.gameObject.name+ "distance from Player" +distance);
        //Debug.Log("NAVMESH POSITION" + navMeshAgent.destination);
        if (!navMeshAgent.isStopped)
        {
            if (distance <= stoppingDistance)
            {
                //Debug.Log("AGENT, STOP IT NOW");
                //Debug.Log("AIMOVEMENTS " + this.gameObject.name + "distance from Player" + distance);
                navMeshAgent.isStopped = true;
            }
            else
            {
                navMeshAgent.destination = Target.position;
                navMeshAgent.isStopped = false;
            }
        }

        if (distance >= stoppingDistance && navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }



            Rotate_Character();   
    }

    //private void Move_Character()
    //{
    //    Vector3 direction_norm = (Target.transform.position - transform.position).normalized;

    //    Vector3 movement = direction_norm * 0.3f;
    //    _movement = movement.normalized * Mathf.Clamp01(movement.magnitude);
    //    _movement.y = 0f;

    //    rb.AddForce(_movement * MovementSpeed);
    //}

    private void Rotate_Character()
    {
        Vector3 direction = Target.transform.position - transform.position;
        direction.y = 0f;

        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, RotationSpeed * Time.deltaTime);
    }
}
