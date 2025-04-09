using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private StateMachine stateMachine;
    public Transform player;
    public NavMeshAgent agent;


    //patrolling
    public Transform[] patrolWaypoints;
    public int waypointIndex;
    public float patrolSpeed = 5;
    public float detectionRange = 3;

    //vision
    private bool isPlayerInVisionCone = false;
    public float visionAngle = 120f;

    //hearing
    public float hearingRange = 15f;
    public float hearingThreshold = 10;
    public float playerVolume = 2f; //TEMP MOVE TO PLAYER CONTROLLER WHEN WE HAVE IT

    //how aware of the player the ai is (sound wise)
    private float awarenessLevel = 0f;
    public float awarenessThreshold = 100;
    public float awarenessDecay;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine();
        ChangeState(new StatePatrol(this));
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }


    public void ChangeState(State newState)
    { 
        stateMachine.ChangeState(newState);
    }

    public bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer < visionAngle / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))

            {
                if (hit.transform == player)
                {
                    return true;
                }

            }
            
        }
        return false;


        //COLLIDER METHOD
        /*if (!isPlayerInVisionCone) 
        {
            return false; //player must be inside vision cone
        }
        Debug.Log("canseeplayer firing");

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
        {
            if (hit.transform == player)
            {
                Debug.Log("Hit Player");
                return true;
            }
        }

        return false;*/
        //return Vector3.Distance(transform.position, player.position) < detectionRange;
    }

    public bool CanHearPlayer()
    {
        if (player == null) return false;

        if (Vector3.Distance(transform.position, player.position) < hearingRange && playerVolume > hearingThreshold)
        {
            return true;
        }
        return false;
    }

    public void UpdateAwareness()
    {
        if(CanSeePlayer())
        {
            awarenessLevel += 50 * Time.deltaTime;
        }
        else if (CanHearPlayer())
        {
            awarenessLevel += 20 * Time.deltaTime;
        }
        else
        {
            awarenessLevel -= awarenessDecay * Time.deltaTime;
        }

        //awarenessLevel = Mathf.Clamp(awarenessLevel, 0, awarenessThreshold);
    }

    public void SetPlayerInVisionCone(bool isVisible)
    {
        isPlayerInVisionCone = isVisible;
    }


    public void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * patrolSpeed);

        Vector3 direction = (player.position - transform.position).normalized;

        float rotationSpeed = 3f;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);

    }

    /*public void RotateToObject(Vector3 objectPosition)
    {
        Vector3 direction = (player.position - transform.position).normalized;

        float rotationSpeed = 3f;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }*/

    public void Patrol()
    {
        if (patrolWaypoints.Length == 0) return;

        Transform targetWaypoint = patrolWaypoints[waypointIndex];

        // Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        /*float rotationSpeed = 3f;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Time.deltaTime * patrolSpeed);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            waypointIndex = (waypointIndex + 1) % patrolWaypoints.Length;
        }*/

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            waypointIndex = (waypointIndex + 1) % patrolWaypoints.Length;
        }
            agent.SetDestination(patrolWaypoints[waypointIndex].position);
    }
}
