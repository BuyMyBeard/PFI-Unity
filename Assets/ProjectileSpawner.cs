using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] float spawnRange = 5f;
    [SerializeField] float minCooldown;
    [SerializeField] float maxCooldown;

    ObjectPoolComponent associatedPool;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - Vector3.left * spawnRange, transform.position + Vector3.right * spawnRange);
    }

    private void Awake()
    {
        associatedPool = transform.parent.GetComponentInChildren<ObjectPoolComponent>();
        StartCoroutine(SpawnProjectiles());
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
            GameObject projectile = associatedPool.GetElement();
            projectile.transform.position = transform.position + Vector3.forward * Random.Range(-spawnRange, spawnRange);
            projectile.SetActive(true);
        }
    }
}
