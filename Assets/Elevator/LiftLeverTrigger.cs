using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLeverTrigger : MonoBehaviour {
    [SerializeField] private int TargetIdAtList;
    [SerializeField] private AudioClip leverSound;
    [SerializeField] private float buttonDelayTime;
    private float oldTime;
    private AudioSource leverAS;
    private int oldTargetId;
    //private static ElevatorController elevatorController;
    [SerializeField] private ElevatorController elevatorController;
    private void Start()
    {
        oldTime = Time.time;
        leverAS = gameObject.GetComponent<AudioSource>();
        //if (elevatorController == null)
        //{
        //    elevatorController = GetComponentInParent<ElevatorController>();
        //    Debug.Log("Got elevator controller (check static)");
        //}
    }
    public void ChangeTargetId(int newTargetFloorId)
    {
        oldTargetId = TargetIdAtList;
        TargetIdAtList = newTargetFloorId;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        
        if (string.Equals("Player", collision.tag) && Input.GetAxis("Interact") > 0)
        {
            float now = Time.time;
            if ((now - oldTime) < buttonDelayTime) return;
            oldTime = now;
            if (leverAS != null)
                leverAS.PlayOneShot(leverSound);
            elevatorController.GoAtTarget(TargetIdAtList);
        }
    }
}
