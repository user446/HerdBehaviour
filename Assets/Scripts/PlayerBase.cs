using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(TeamMate))]
public class PlayerBase : MonoBehaviour
{
    public float movementSpeed;
    public float searchRadius;
    private Rigidbody rb;
    private Health health;
    private Combat combat;
    private Vector3 moveTo; /// vector to hold directon to move towards
    private LineRenderer line;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        combat = GetComponent<Combat>();
        line = GetComponent<LineRenderer>();

        health.onDeath += OnDeath; //set delegate call
    }

    // Update is called once per frame
    void Update()
    {
        if(combat.target != null)
        {
            Vector3 direction = combat.target.position - transform.position;
            direction.Normalize();
            moveTo = direction;
            combat.Attack(); //call for attack each Update
            ShareTarget();
            //draw a line to show which target is attacked
            line.enabled = true;
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, combat.target.position);
        }
        else
        {
            //search for target
            SearchTarget();
            line.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if(combat.target != null)
        {
            MoveTowards(moveTo);
        }
    }

    /// <summary>
    /// Handle death of player
    /// </summary>
    public void OnDeath()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Function to search enemies by team (could be done with just Layers in OverlapSphere, but I decided to go with ScriptableObjects)
    /// </summary>
    public void SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        Transform c_nearest = null;
        float min_distance = Mathf.Infinity;
        foreach(Collider c in colliders)
        {
            if(c.GetComponent<TeamMate>() != null)
            //check if this object is opposing to collider's team
            if(this.GetComponent<TeamMate>().team.enemyTeams.Find(x => x.Equals(c.GetComponent<TeamMate>().team)))
            {
                //validate target
                if(c.GetComponent<PlayerBase>() != null)
                {
                    float currentDistance = Vector3.Distance(this.transform.position, c.transform.position);
                    if(currentDistance < min_distance)
                    {
                        c_nearest = c.transform;
                        min_distance = currentDistance;
                    }
                }
            }
        }
        if(c_nearest != null)
        {
            this.combat.target = c_nearest;
        }
    }

    /// <summary>
    /// Share enemy transform with allies nearby
    /// </summary>
    public void ShareTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach(Collider c in colliders)
        {
            if(c.GetComponent<TeamMate>() != null)
            //check if this object is in the same team with the collider
            if(c.GetComponent<TeamMate>().team.Equals(this.GetComponent<TeamMate>().team))
            {
                var c_base = c.GetComponent<PlayerBase>();
                //validate target
                if(c_base != null && c_base.combat.target == null)
                {
                    c_base.SetTarget(this.combat.target);
                }
            }
        }
    }

    /// <summary>
    /// Function to set a new target to follow by monser
    /// </summary>
    /// <param name="newTarget">New target transform</param>
    public void SetTarget(Transform newTarget)
    {
        combat.target = newTarget;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Current target</returns>
    public Transform GetTarget()
    {
        return combat.target;
    }

    /// <summary>
    /// Moves rigidbody of a monster following given direction
    /// </summary>
    /// <param name="direction">Direction to move on</param>
    void MoveTowards(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * movementSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Makes easier to ajust search radius in the Inspector
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
