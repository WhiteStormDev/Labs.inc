using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subSceneBorderTrigger : MonoBehaviour {
    public static subSceneBorderControl borderControl;
    public GameObject leftBorder;
    public GameObject rightBorder;
    public bool workOnce = true;
    private bool isWorked = false;
    private void Start()
    {
        isWorked = false;
        if (borderControl == null)
            borderControl = FindObjectOfType<subSceneBorderControl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!(isWorked && workOnce) && collision.tag == "Player" && (leftBorder != null || rightBorder != null))
        {
            isWorked = true;
            borderControl.SetBorders(leftBorder, rightBorder);
        }
    }

}
