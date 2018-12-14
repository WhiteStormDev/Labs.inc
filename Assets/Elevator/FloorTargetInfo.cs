using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTargetInfo : MonoBehaviour {

    [SerializeField] private DoorMode doorModeOnThisFloor;

    public DoorMode DoorModeOnThisFloor
    {
        get
        {
            return doorModeOnThisFloor;
        }

        set
        {
            doorModeOnThisFloor = value;
        }
    }
}
