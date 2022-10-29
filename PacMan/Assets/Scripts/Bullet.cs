using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float force = 1000;
    void Start()
    {
        // If Rigidbody is attached to bullet, add force in bullet's foward direction (bullet is instantiated with given rotation)
        if (TryGetComponent(out Rigidbody RB))
        {
            RB.AddForce(force * transform.forward);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // If bullet hit enemy, kill the enemy and call KillEnemy function from PacManController, if present on player
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.KillEnemy();
            }
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PacManController PMC))
            {
                PMC.KillEnemy();
            }
        }

        // Destroy bulet
        Destroy(gameObject);
    }
}
