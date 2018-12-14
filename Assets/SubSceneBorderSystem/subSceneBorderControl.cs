using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subSceneBorderControl : MonoBehaviour {


    // Use this for initialization
    public static CharCamFollow cam;
    private GameObject currLeftBorder;
    private GameObject currRightBorder;

    void Start()
    {
        if (cam == null)
            cam = FindObjectOfType<CharCamFollow>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetBorders(GameObject leftBorder, GameObject rightBorder)
    {
        if (cam.isBorderedX) return;
        currLeftBorder = leftBorder;
        currRightBorder = rightBorder;

        float cameraHeight = cam.camera.orthographicSize;// Попытки найти коэф. масштабирования
        float screenAspect = (float)Screen.width / Screen.height;


        if (leftBorder != null)
        {
            leftBorder.SetActive(true);
            float width = cameraHeight * screenAspect;
            cam.leftBorderX = leftBorder.transform.position.x + width;
        }
        else
            cam.leftBorderX = -Mathf.Infinity;

        if (rightBorder != null)
        {
            rightBorder.SetActive(true);
            float width = cameraHeight * screenAspect;
            cam.rightBorderX = rightBorder.transform.position.x - width;

        }
        else
            cam.rightBorderX = Mathf.Infinity;

        cam.isBorderedX = true;

    }
    public void DeactivateBorders()
    {
        currLeftBorder.SetActive(false);
        currRightBorder.SetActive(false);
        cam.isBorderedX = false;
    }
    
}
