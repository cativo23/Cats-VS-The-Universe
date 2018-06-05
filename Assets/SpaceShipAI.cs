using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Pathfinding;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class SpaceShipAI : MonoBehaviour {

    public Transform target;
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    //The CalculatedPath
    public Path path;

    //The AI spee per second

    public float speed = 300f;

    public ForceMode2D FMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //the max diatance form ai to a way pooint to continue to the next waypoint
    public float nextWayPointDis = 3;

    private int currentWayPoint;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if(target == null)
        {
            print("No hay target");
            GameObject tmp = GameObject.FindGameObjectWithTag("Hero");
            target = tmp.transform;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got path, errores? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

   IEnumerator UpdatePath()
    {
        if (target == null)
        {
            yield return false;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f/updateRate);
        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        //TODO: Look at player?

        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }

            Debug.Log("End of path reach");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;

        dir *= speed * Time.fixedDeltaTime;

        rb.AddForce(dir, FMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if ( dist <  nextWayPointDis)
        {
            currentWayPoint++;
            return;
        }
    }
}
