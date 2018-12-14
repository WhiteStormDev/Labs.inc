using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPoint : MonoBehaviour {
    [SerializeField] private float holdDownTimer;
    [SerializeField] public bool HoldDownEnd;
    [SerializeField] private bool alreadyTicking;
    private float startTickingTime;
    public float HoldDownTimer
    {
        get
        {
            return holdDownTimer;
        }

        //set
        //{
        //    holdDownTimer = value;
        //}
    }

    public void StartTick()
    {
        if (alreadyTicking) return;
        alreadyTicking = true;
        startTickingTime = Time.time;
        HoldDownEnd = false;
        Debug.Log("StartTickTime = " + startTickingTime);

    }
    private void Update()
    {
        if (alreadyTicking)
        {
            float now = Time.time;
            if (now - startTickingTime >= holdDownTimer)
            {
                HoldDownEnd = true;
                alreadyTicking = false;
                Debug.Log("Done");
            }
            else
            {
                Debug.Log("Now = " + now);
                Debug.Log("start = " + startTickingTime);
                Debug.Log("HoldTime = " + holdDownTimer);
            }

        }
    }
}
