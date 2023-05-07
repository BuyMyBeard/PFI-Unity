using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMechanic : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    private Transform mainCameraTransform;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        previousCameraPosition = mainCameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = mainCameraTransform.position - previousCameraPosition;
        transform.position += deltaMovement * parallaxSpeed;
        previousCameraPosition = mainCameraTransform.position;
    }
}
