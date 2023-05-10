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
    private Vector2 direction = Vector2.down;
    [SerializeField] float speed = 5f;
    [SerializeField] float time = 5f;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().gameObject;
        vCamera = FindObjectOfType<CinemachineVirtualCamera>();
        lava = FindObjectOfType<RisingLava>().gameObject;
    }

    void Start()
    {
        StartCoroutine(BeginCutscene(time));
    }
    private void Update()
    {
        if (!(transform.position.y < -4))
            transform.Translate(speed * Time.deltaTime * direction);
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
