using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCamFollow : MonoBehaviour {

    public GameObject Background;
    public float AttackImpactFreezTime;
    public bool isBorderedX;
    public bool isZoom = false;
    public float zoomSmooth = 0;
    public float unzoomValue = 0;
    public float zoomValue = 0;
    public float zoomBorder = 0f;
    public bool targetHold = false;
    public bool CIMATICcamNAEZD = false;
    public GameObject targetGameObj;
    public GameObject secondTargetGameObj;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float naezdSpeed = 0.1f;
    public float lookAheadMoveThreshold = 0.1f;
    public float X_Offset = 0.1f;
    public float Y_Offset = 0.06f;
    public float targetXOffset = 0f;
    public float targetYOffset = 0f;
    public float yPosBorder = 0f;
    public float rightBorderX = 0f;
    public float leftBorderX = 0f;
    public float camSizeOnTarget;
    public float camSizeFree;

    private bool isFreeCam = true;

    private float oldYPosBorder = 0f;
    private float oldTargetXOffset = 0f;
    private float oldTargetYOffset = 0f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    private float s_OffsetZ;
    private Vector3 s_LastTargetPosition;
    private Vector3 s_CurrentVelocity;
    private Vector3 s_LookAheadPos;

    private float oldRotY;
    private Transform animTransform;
    private Transform target;
    private Transform secondTarget;

    public Camera camera;
    private bool isAttackImpactFreez;
    private float oldTime;

    private void Start()
    {
        target = targetGameObj.transform;
        animTransform = target.GetChild(0).gameObject.transform;
        oldRotY = animTransform.rotation.y;
        
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
        

        secondTargetSet();
        camera = GetComponent<Camera>();
        camera.orthographicSize = camSizeFree;
        unzoomValue = camera.orthographicSize;
        //GameObject.FindGameObjectWithTag("Background").SetActive(true);
        Background.SetActive(true);

    }
    public void hardSecondTargetSet()
    {
        if (secondTargetGameObj != null /*&& secondTarget == null*/)
        {
            secondTarget = secondTargetGameObj.transform;
            s_LastTargetPosition = secondTarget.position;
            s_OffsetZ = (transform.position - secondTarget.position).z;
            transform.parent = null;
            targetHold = true;
        }
    }
    private void secondTargetSet()
    {
        if (secondTargetGameObj != null && secondTarget == null)
        {
            secondTarget = secondTargetGameObj.transform;
            s_LastTargetPosition = secondTarget.position;
            s_OffsetZ = (transform.position - secondTarget.position).z;
            transform.parent = null;
        }
        //oldTargetXOffset = targetXOffset;
        //oldTargetYOffset = targetYOffset;
    }
    public void StartFreez()
    {
        oldTime = AttackImpactFreezTime;
        StartCoroutine(AttackImpactCor());
    }
    public IEnumerator AttackImpactCor()
    {
        
        isAttackImpactFreez = true;
        while (AttackImpactFreezTime > 0)
        {
            AttackImpactFreezTime--;
            yield return new WaitForEndOfFrame();
        }
        isAttackImpactFreez = false;
        AttackImpactFreezTime = oldTime;
    }


    private void Update()
    {
        if (isAttackImpactFreez) return;
        //Debug.Log(animTransform.rotation.y);
        //targetSet();
        secondTargetSet();
        if (isZoom)
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomValue, Time.deltaTime * zoomSmooth);
        else
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, unzoomValue, Time.deltaTime * zoomSmooth);
        
        if (CIMATICcamNAEZD && secondTargetGameObj != null)
        {
          
            float xMoveDelta = (secondTarget.position - s_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                s_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                s_LookAheadPos = Vector3.MoveTowards(s_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 targetOffset = new Vector3(0/*targetXOffset*/, targetYOffset);
            Vector3 aheadTargetPos = secondTarget.position + s_LookAheadPos + Vector3.forward * s_OffsetZ + targetOffset;


            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref s_CurrentVelocity, naezdSpeed);
            if (isBorderedX)
            {
                newPos = new Vector3(Mathf.Clamp(newPos.x, leftBorderX, rightBorderX), Mathf.Clamp(newPos.y, yPosBorder, Mathf.Infinity), newPos.z);
            }
            else
                newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosBorder, Mathf.Infinity), newPos.z);
               
            transform.position = newPos/* + new Vector3(X_Offset, Y_Offset)*/;

            s_LastTargetPosition = secondTarget.position;
        }
        else
        {
            Vector3 currPos = target.position;
            if (targetHold)
            {
                currPos.x = (target.position.x + secondTarget.position.x) / 2;
                currPos.y = (target.position.y + secondTarget.position.y) / 2;
            }
            
            float xMoveDelta = (currPos - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            
           

            Vector3 targetOffset = new Vector3(targetXOffset, targetYOffset);
            Vector3 aheadTargetPos = currPos + m_LookAheadPos + Vector3.forward * m_OffsetZ + targetOffset;


            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            if (isBorderedX)
            {
                newPos = new Vector3(Mathf.Clamp(newPos.x, leftBorderX, rightBorderX), Mathf.Clamp(newPos.y, yPosBorder, Mathf.Infinity), newPos.z);
            }
            else
                newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosBorder, Mathf.Infinity), newPos.z);

            
            transform.position = newPos + new Vector3(X_Offset, Y_Offset);

            m_LastTargetPosition = currPos;
        }
    }
    private void FixedUpdate()
    {
        if (targetHold)
        {
            isFreeCam = false;
            return;
        }
        float currRotY = animTransform.rotation.y;

        //X_Offset *= (oldRotY != currRotY) ? -1 : 1;
        targetXOffset *= (oldRotY != currRotY) ? -1 : 1;
        //oldTargetXOffset *= (oldRotY != currRotY) ? -1 : 1;
        oldRotY = currRotY;
    }
    public void SetSecondTarget(GameObject sTarget)
    {
        secondTarget = null;
        secondTargetGameObj = sTarget;
        camera.orthographicSize = camSizeOnTarget;

    }
    public void SetFreeCam()
    {

        targetHold = false;
        secondTarget = null;
        if (isFreeCam) return;
        isFreeCam = true;
        camera.orthographicSize = camSizeFree;
    }
    public void onZoom(GameObject sTarget)
    {
        oldYPosBorder = yPosBorder;
        yPosBorder += zoomBorder;
        isZoom = true;
        SetSecondTarget(sTarget);
        targetHold = true;
        oldTargetXOffset = targetXOffset;
        oldTargetYOffset = targetYOffset;
        targetXOffset = 0;
        targetYOffset = 0;
        
    }
    public void offZoom()
    {
        yPosBorder = oldYPosBorder;
        isZoom = false;
        secondTarget = null;
        secondTargetGameObj = null;
        targetHold = false;
        targetXOffset = oldTargetXOffset;
        targetYOffset = oldTargetYOffset;
    }
    
}
