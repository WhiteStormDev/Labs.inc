using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowExtended : MonoBehaviour {

    [SerializeField] private Transform target;

    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isFreeCam = true;
    [SerializeField] private bool isBordered = false;
    [SerializeField] private bool FocusPointsMode = false;
    [SerializeField] private float freeCamSize = 10f;
    [SerializeField] private GameObject FocusPointAndTriggerParePrefab;
    [SerializeField] private GameObject FocusPointAndTriggerQuadPrefab;
    [SerializeField] private GameObject SetFreeCamPrefab;
    private Vector3 focusTarget;
    private List<GameObject> focusPares;
    private List<GameObject> focusQuads;
    private float oldDesiredCameraSize;
    private float desiredCameraSize;
    private Vector3 desiredPos;
    private Camera cameraComponent;
    private float camLeftBorderX;
    private float camRightBorderX;
    private float camUpBorderY;
    private float camDownBorderY;
    //FocusPoints array where cam will follow if character touched Trigger at appropriate arr index 
    
    private void Start()
    {
        cameraComponent = gameObject.GetComponent<Camera>();
        oldDesiredCameraSize = cameraComponent.orthographicSize;
        desiredCameraSize = cameraComponent.orthographicSize;
        focusTarget = transform.position;
        focusPares = new List<GameObject>(GameObject.FindGameObjectsWithTag("FocusPare"));
        focusQuads = new List<GameObject>(GameObject.FindGameObjectsWithTag("FocusQuad"));
        
    }
    private void Update()
    {
        if (isFreeCam && !FocusPointsMode)
        {
            desiredCameraSize = freeCamSize;
            focusTarget = transform.position;
            desiredPos = target.position + offset;
            //if (target.position.x > camLeftBorderX || target.position.x < camRightBorderX ||
            //    target.position.y > camDownBorderY || target.position.y < camUpBorderY)
            //{
               
            //    FocusPointsMode = true;
            //    desiredCameraSize = oldDesiredCameraSize;
            //}
        }
        if (!isFreeCam && FocusPointsMode)
        {
            desiredPos = focusTarget + new Vector3(0, 0, offset.z);
        }
        if (isFreeCam && FocusPointsMode)
        {
            float cameraHeight = cameraComponent.orthographicSize;// Попытки найти коэф. масштабирования
            float screenAspectX = (float)Screen.width / Screen.height;
            float width = cameraHeight * screenAspectX;
            camRightBorderX = focusTarget.x + width;
            camLeftBorderX = focusTarget.x - width;
            //float screenAspectY = (float)Screen.height / Screen.width;
            camUpBorderY = focusTarget.y + cameraHeight;
            camDownBorderY = focusTarget.y - cameraHeight;
            if (target.position.x < camLeftBorderX || target.position.x > camRightBorderX ||
                target.position.y < camDownBorderY || target.position.y > camUpBorderY)
            {
                oldDesiredCameraSize = desiredCameraSize;
                FocusPointsMode = false;
                desiredCameraSize = freeCamSize;
            }
            else
                desiredPos = focusTarget + new Vector3(0, 0, offset.z);
        }
        
        
        
    }
    void FixedUpdate()
    {
        if (Mathf.Abs(cameraComponent.orthographicSize - desiredCameraSize) > 0.001f)
        {
            float smoothedCamSize = Mathf.Lerp(cameraComponent.orthographicSize, desiredCameraSize, zoomSpeed);
            cameraComponent.orthographicSize = smoothedCamSize;
        }
        
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;

    }
    public void AddFocusPointAndTriggerPare()
    {
        Instantiate(FocusPointAndTriggerParePrefab);
    }
    public void AddFocusPointAndTriggerQuad()
    {
        Instantiate(FocusPointAndTriggerQuadPrefab);
    }
    public void AddSetFreeCamTrigger()
    {
        Instantiate(SetFreeCamPrefab);
    }
    public void SetFreeCam()
    {
        isFreeCam = true;
        FocusPointsMode = false;
    }
    public void LerpCamToFocusPoint(Vector3 focusPointPosition, float cameraSize)
    {
        //if (!FocusPointsMode) return;
        //isFreeCam = false;
        FocusPointsMode = true;
        focusTarget = focusPointPosition;
        desiredCameraSize = cameraSize;
        
    }
}
