using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnFirstFloorTrigger : MonoBehaviour {
    [SerializeField] private AudioClip triggerSound;
    private AudioSource triggerAS;
    [SerializeField] float triggerDelay = 2f;
    [SerializeField] private LiftLeverTrigger LiftLeverTrigger;
    [SerializeField] private int changeTargetIdOnThatId = 0;
    private float oldTime;
    private void Start()
    {
        oldTime = Time.time;
        triggerAS = gameObject.GetComponent<AudioSource>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (string.Equals("Player", collision.tag) && Input.GetAxis("Interact") > 0) 
        {
            float now = Time.time;
            if (now - oldTime < triggerDelay) return;
            oldTime = now;
            if (triggerAS != null) triggerAS.PlayOneShot(triggerSound);
            LiftLeverTrigger.ChangeTargetId(changeTargetIdOnThatId);
            Debug.Log("Lift Lever [Floor 2] => [Floor 1]");
        }
    }
}
