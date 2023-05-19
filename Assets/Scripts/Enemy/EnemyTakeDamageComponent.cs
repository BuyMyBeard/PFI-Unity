using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyTakeDamageComponent : MonoBehaviour
{
    [SerializeField] LayerMask attackerLayer;
    [SerializeField] float launchSpeed = 1;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float TimeBeforeDissapear = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Die()
    {
        transform.parent.GetComponent<MonsterMove>().enabled = false;
        transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
        foreach (var bc in transform.parent.GetComponentsInChildren<BoxCollider2D>())
            bc.enabled = false;
        StartCoroutine(SendFlying());
    }
    IEnumerator SendFlying()
    {
        int angle = Random.Range(0, 360);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        if (Random.Range(0, 2) == 0)
            rotationSpeed *= -1;
        for (float time = 0; time < TimeBeforeDissapear; time += Time.deltaTime)
        {
            transform.parent.transform.Translate(launchSpeed * Time.deltaTime * direction, Space.World);
            transform.parent.Rotate(Vector3.forward ,Time.deltaTime * rotationSpeed, Space.Self);
            yield return null;
        }
        transform.parent.gameObject.SetActive(false);

    }
}
