using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float attackDamage;
    public float attackRadius;
    public float attackSpeed;
    public Transform target;
    private float attackCooldown;

    void Update()
    {
        if(attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Checks if any prefab around is inside circle of attackRange and applies damage
    /// </summary>
    public void Attack()
    {
        if(attackCooldown < 0)
        {
            attackCooldown = 1f/attackSpeed;
            if(target != null)
            if(Vector3.Distance(this.transform.position, target.position) < attackRadius)
            {
                var target_health = target.GetComponent<Health>();
                if(target_health != null)
                    target_health.DoDelta(-attackDamage);
            }
            // List<Collider> colliders = new List<Collider>();
            // colliders.AddRange(Physics.OverlapSphere(transform.position, attackRadius));
            // var c_target = colliders.Find(x => x.transform.Equals(target));
            // if(c_target != null)
            // {
            //     var c_target_health = c_target.GetComponent<Health>();
            //     if(c_target_health != null)
            //     {
            //         c_target_health.DoDelta(-attackDamage);
            //     }
            // }
            //===================
            // foreach(var c in colliders)
            // {
            //     if(c.transform.Equals(target))
            //     {
            //         var c_health = c.GetComponent<Health>();
            //         if(c_health != null)
            //         {
            //             c_health.DoDelta(-attackDamage);
            //             break;
            //         }
            //     }
            // }
        }
    }

    /// <summary>
    /// Makes easier to ajust attack range in the Inspector
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
