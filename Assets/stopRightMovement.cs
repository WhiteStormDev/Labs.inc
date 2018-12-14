using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopRightMovement : MonoBehaviour {
    [SerializeField] private Character2DControl charCtrl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (string.Equals(collision.tag, "GroundBorders"))
        {
            charCtrl.CanMove = false;
            Debug.Log("cant");
        }
           

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (string.Equals(collision.tag, "GroundBorders"))
            charCtrl.CanMove = true;
    }
}
