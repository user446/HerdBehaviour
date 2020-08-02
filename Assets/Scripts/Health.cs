using UnityEngine;
using System;

/// <summary>
/// Health class
/// Holds properties necessary for health bar
/// </summary>
public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public bool isDead;
    public Action onDeath = delegate { };

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0 && !isDead)
        {
            isDead = true;
            onDeath(); // this prefab is now dead
        }
    }

    /// <summary>
    /// Make a delta from current health
    /// Can be used for healing and taking damage on prefab
    /// Values are held between maxHealth and 0
    /// </summary>
    /// <param name="delta">Delta to add to a currentHealth</param>
    public void DoDelta(float delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
    }
}
