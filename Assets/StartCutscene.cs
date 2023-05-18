using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{

    GameObject player;
    CinemachineVirtualCamera vCamera;
    GameObject lava;
    [SerializeField] float speed = 5f;
    [SerializeField] float time = 5f;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().gameObject;
        vCamera = FindObjectOfType<CinemachineVirtualCamera>();
        lava = FindObjectOfType<RisingLava>().gameObject;
        StartCoroutine(BeginCutscene(time));

    }

    private void Update()
    {
        if (!(transform.position.y < -4))
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    public IEnumerator BeginCutscene(float time)
    {
        lava.SetActive(false);
        vCamera.Follow = transform;
        yield return new WaitForSeconds(time);
        vCamera.Follow = player.transform;
        lava.SetActive(true);
        gameObject.SetActive(false);
        
    }
}
