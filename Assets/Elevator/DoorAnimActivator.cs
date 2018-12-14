using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimActivator : MonoBehaviour {
    [SerializeField] private Animator doorAnimator;
    
    [SerializeField] private bool isOpenedOnStart;
    private void Start()
    {
    //    if (isOpenedOnStart)
    //    {
    //        doorAnimator.SetTrigger("openDoorTrigger");
    //    }
    //    else
    //        doorAnimator.SetTrigger("closeDoorTrigger");
    }
    private void OnEnable()
    {
        if (doorAnimator != null)
        doorAnimator.SetTrigger("closeDoorTrigger");
        //closeDoorAnimation
    }
    private void OnDisable()
    {
        //openDoorAnimation
        if (doorAnimator != null)
        doorAnimator.SetTrigger("openDoorTrigger");
    }
}
