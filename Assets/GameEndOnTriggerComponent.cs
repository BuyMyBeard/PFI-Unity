using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameEndOnTriggerComponent : MonoBehaviour
{
    [SerializeField] LayerMask canTriggerEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((canTriggerEnd.value & (1 << collision.gameObject.layer)) > 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
