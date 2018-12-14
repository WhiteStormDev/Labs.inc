using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    [Header("Colls&Sprites: ")]
    [SerializeField] private GameObject doorBorderCollider;
    [SerializeField] private GameObject doorCloseSprite;
    [SerializeField] private GameObject doorOpenSprite;
    [Header("Audio: ")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorCloseAudioClip;
    [SerializeField] private AudioClip doorOpenAudioClip;
    [Header("Door Timer: ")]
    [SerializeField] private float deltaDoorTime = 1f;
    private float doorTime;
    
    private void Start()
    {
        doorTime = Time.time;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetAxis("Interact") > 0 && Mathf.Abs(doorTime - Time.time) > deltaDoorTime)
        {
            doorTime = Time.time;
            if (doorCloseSprite.activeInHierarchy)
                doorAudioSource.PlayOneShot(doorOpenAudioClip);
            else
                doorAudioSource.PlayOneShot(doorCloseAudioClip);

            doorCloseSprite.SetActive(!doorCloseSprite.activeInHierarchy);
            doorOpenSprite.SetActive(!doorOpenSprite.activeInHierarchy);
            doorBorderCollider.SetActive(!doorBorderCollider.activeInHierarchy);

        }
    }
  
}
