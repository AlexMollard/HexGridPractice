using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public int speed = 4;
    public float PCspeed = 1.0f;
    public float MINSCALE = 2.0F;
    public float MAXSCALE = 5.0F;
    public float minPinchSpeed = 5.0F;
    public float varianceInDistances = 5.0F;
    private float touchDelta = 0.0F;
    private Vector2 prevDist = new Vector2(0, 0);
    private Vector2 curDist = new Vector2(0, 0);
    private float speedTouch0 = 0.0F;
    private float speedTouch1 = 0.0F;
    private Vector2 movePrevDist = new Vector2(0, 0);
    public float moveSpeedTouch = 1.0F;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    public float PCdragSpeed = 2;
    private Vector3 PCdragOrigin;
    Rigidbody RB;
    float moveSpeedPC = 0.0f;
    public float ZoomAmount = 0.0f;
    public float ZoomSpeed = 40.0f;
    public float PCZoomSpeed = 40.0f;
    RaycastHit hit;
    public float MinHeight = 10.0f;
    int layerMask;
    public PieGraphManager Graph;
    public GameObject InfoCanvas;

    public static bool BlockedByUI;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        layerMask = 1 << 2;
        layerMask = ~layerMask;
        InfoCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(mainCamera.transform.position, -mainCamera.transform.up, out hit, 20f))
        {
            MinHeight = (hit.transform.localScale.y / 5) + 2;
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            movePrevDist = (Input.GetTouch(0).deltaPosition * -1) * (moveSpeedTouch * mainCamera.transform.position.y);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + movePrevDist.x, mainCamera.transform.position.y, mainCamera.transform.position.z + movePrevDist.y);
        }
        // PINCH ZOOM
        else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
            prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
            touchDelta = curDist.magnitude - prevDist.magnitude;
            speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
            speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
        
            if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {
                mainCamera.transform.Translate(Vector3.forward * -(ZoomSpeed + mainCamera.transform.position.y) * Time.deltaTime);

            }
            if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed) && mainCamera.transform.position.y < 100f && mainCamera.transform.position.y > MinHeight)
            {
                mainCamera.transform.Translate(Vector3.forward * (ZoomSpeed + mainCamera.transform.position.y) * Time.deltaTime);
            }
        }
        else
        {
            GetPCInput();
        }

        mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, -40, 40), Mathf.Clamp(mainCamera.transform.position.y, MinHeight, 100), Mathf.Clamp(mainCamera.transform.position.z, -36, 30));
    }


    void GetPCInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeedPC = PCspeed * 2 * Time.deltaTime;
            PCZoomSpeed = ZoomSpeed * 6;
        }
        else
        {
            PCZoomSpeed = ZoomSpeed;
            moveSpeedPC = PCspeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, RB.velocity.z + moveSpeedPC);
        }

        if (Input.GetKey(KeyCode.S))
        {
            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, RB.velocity.z - moveSpeedPC);
        }

        if (Input.GetKey(KeyCode.D))
        {
            RB.velocity = new Vector3(RB.velocity.x + moveSpeedPC, RB.velocity.y, RB.velocity.z);
        }

        if (Input.GetKey(KeyCode.A))
        {
            RB.velocity = new Vector3(RB.velocity.x - moveSpeedPC, RB.velocity.y, RB.velocity.z) ;
        }

        if (Input.mouseScrollDelta.y > 0 )
        {
            mainCamera.transform.Translate(Vector3.forward * (PCZoomSpeed + mainCamera.transform.position.y ) * Time.deltaTime);
        }
        else if (Input.mouseScrollDelta.y < 0 && mainCamera.transform.position.y < 100f)
        {
            mainCamera.transform.Translate(Vector3.forward * -(PCZoomSpeed + mainCamera.transform.position.y) * Time.deltaTime);
        }



        if (Input.GetMouseButtonDown(0))
        {

            if (!InfoCanvas.activeSelf)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, layerMask))
                {
                    hit.transform.GetComponent<CellBehaviour>().GenerateTerrain();
                    Graph.cell = hit.transform.GetComponent<CellBehaviour>();
                    InfoCanvas.SetActive(true);
                    Graph.UpdateChart();
                }
            }
            else if (!BlockedByUI)
            {
                InfoCanvas.SetActive(false);
            }
        }


    }
    public void EnterUI()
    {
        BlockedByUI = true;
    }
    public void ExitUI()
    {
        BlockedByUI = false;
    }


}
