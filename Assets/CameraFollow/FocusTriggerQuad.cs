using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusTriggerQuad : MonoBehaviour {

    public static GameObject MainCamera;
    [SerializeField] private GameObject anotherTrigger;
    [SerializeField] private GameObject focusPoint;
    [SerializeField] private float newCameraSize;
    private void Awake()
    {
        if (MainCamera == null)
        {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            Debug.Log("MainCamera found");
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MainCamera.GetComponent<CamFollowExtended>().LerpCamToFocusPoint(focusPoint.transform.position, newCameraSize);
            gameObject.SetActive(false);
            anotherTrigger.SetActive(true);
        }
    }
}
