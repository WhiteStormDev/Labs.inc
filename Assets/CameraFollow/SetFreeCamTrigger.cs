using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreeCamTrigger : MonoBehaviour {

    public static GameObject MainCamera;
   
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
            MainCamera.GetComponent<CamFollowExtended>().SetFreeCam();
        }
    }
}
