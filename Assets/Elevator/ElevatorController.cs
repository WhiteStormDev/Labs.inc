using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DoorMode
{
    right,
    left,
    both
}
public class ElevatorController : MonoBehaviour {
    [SerializeField] private GameObject centerPoint; //point that elevator checks when comming to the floor
    [SerializeField] private List<GameObject> targets;
    //[SerializeField] private GameObject upTarget;
    //[SerializeField] private GameObject downTarget;
    [SerializeField] private GameObject platform;
    [SerializeField] private float startElevatorSpeed = -3f;
    [SerializeField] private float acselerationElevatorSpeed = 0.1f;
    [SerializeField] private float elevatorSpeed = 3f;
    [SerializeField] private float _currElevatorSpeed;
    [SerializeField] private float targetEpsilon = 0.1f;

    //[SerializeField] private bool goingDown = false;
    //[SerializeField] private bool goingUp = false;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] DoorMode openingDoorsMode;
    [SerializeField] private GameObject firstGoesFloor;
    private bool isGoing;
    public bool IsGoing
    {
        get
        {
            return IsGoing;
        }
        set
        {
            if (!value)
                openTheDoors();
            isGoing = value;
        }
    }
    //private int currTargetId;
    private GameObject currTargetGO;

    [SerializeField] private Animator leftDoorAnim;
    [SerializeField] private Animator rightDoorAnim;
    [SerializeField] private Animator liftAnimator;
    private static Transform heroTF;
    private void Start()
    {
        if (heroTF == null)
            heroTF = GameObject.FindGameObjectWithTag("Player").transform;

        _currElevatorSpeed = startElevatorSpeed;
        //liftAnimator = GetComponent<Animator>();
        currTargetGO = targets[0];
        //openTheDoors();
        if (firstGoesFloor != null)
        {
            liftAnimator.SetBool("IsMoving", true);
            currTargetGO = firstGoesFloor;
            IsGoing = true;
            openingDoorsMode = currTargetGO.GetComponent<FloorTargetInfo>().DoorModeOnThisFloor;
        }
        
    }
    private void closeTheDoors()
    {
        if (leftDoorAnim != null)
        {
            leftDoorAnim.SetTrigger("closeDoorTrigger");
            
        }
            
        if (rightDoorAnim != null)
            rightDoorAnim.SetTrigger("closeDoorTrigger");
        //leftDoor.SetActive(true);
        //rightDoor.SetActive(true);
    }
    private void openTheDoors()
    {
        switch (openingDoorsMode)
        {
            case DoorMode.both:
                if (leftDoorAnim != null)
                    leftDoorAnim.SetTrigger("openDoorTrigger");
                if (rightDoorAnim != null)
                    rightDoorAnim.SetTrigger("openDoorTrigger");

                //leftDoor.SetActive(false);
                //rightDoor.SetActive(false);
                break;
            case DoorMode.left:
                if (leftDoorAnim != null)
                    leftDoorAnim.SetTrigger("openDoorTrigger");
                //leftDoor.SetActive(false);
                break;
            case DoorMode.right:
                if (rightDoorAnim != null)
                    rightDoorAnim.SetTrigger("openDoorTrigger");
                //rightDoor.SetActive(false);
                break;
        }
    }
    public void GoAtTarget(int targetIndexInList)
    {
        if (currTargetGO != null && currTargetGO == targets[targetIndexInList])
        {
            Debug.Log("already On that Floor");
            return;
        }
        liftAnimator.SetBool("IsMoving", true);
        currTargetGO = targets[targetIndexInList];
        IsGoing = true;
        openingDoorsMode = currTargetGO.GetComponent<FloorTargetInfo>().DoorModeOnThisFloor;
    }
    private void Update()
    {
        
    }
    public void FixedUpdate()
    {
        if (isGoing)
        {
            
            float currY = (centerPoint.transform.position.y < currTargetGO.transform.position.y) ? 1 : (centerPoint.transform.position.y > currTargetGO.transform.position.y) ? -1 : 0;
            
            if (Mathf.Abs(centerPoint.transform.position.y - currTargetGO.transform.position.y) < targetEpsilon)
            {
                _currElevatorSpeed = startElevatorSpeed;
                liftAnimator.SetBool("IsMoving", false);
                currY = 0;
                IsGoing = false;
            }
            platform.transform.Translate((new Vector3(0, currY, 0)) * Time.deltaTime * _currElevatorSpeed);
            if (currY != 0)
            {
                heroTF.Translate((new Vector3(0, currY, 0)) * Time.deltaTime * _currElevatorSpeed);
                _currElevatorSpeed = Mathf.Lerp(_currElevatorSpeed, elevatorSpeed, acselerationElevatorSpeed);
                
            }
            //Debug.Log(currY);
           

            
            //if (Vector3.Distance(centerPoint.transform.localPosition, currTargetGO.transform.localPosition) < targetEpsilon)
            //    isGoing = false;
        }
        if (leftDoorAnim != null)
            leftDoorAnim.SetBool("isGoing", isGoing);
        if (rightDoorAnim != null)
            rightDoorAnim.SetBool("isGoing", isGoing);
        //if (_currElevatorSpeed == startElevatorSpeed) openTheDoors();
    }
}
