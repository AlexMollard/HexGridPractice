using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

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
    public float PCdragSpeed = 2;
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
    public GameObject MainMenuButton;

    public static bool BlockedByUI;
    public GameObject SelectedCell;
    float TimeGone = 0.0f;
    float FadeTimer = 0.0f;
    public Vector3 LastPos;
    public Vector3 LastRot;
    public bool IsViewingInnerCell = false;
    public GameObject ZoomInButton;
    public GameObject BlackFade;
    public GameObject ReturnToMapButton;
    Vector3 SelectedCellPostition;
    Vector3 direction;
    Quaternion toRotation;
    bool IsValidTouch = false;
    bool TouchedOnly = false;
    public GameObject MapParent;
    public GameObject gameController;
    bool FirstUpdate = true;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        layerMask = 1 << 2;
        layerMask = ~layerMask;
        InfoCanvas.SetActive(false);
        ReturnToMapButton.SetActive(false);
        ZoomInButton.GetComponent<Button>().onClick.AddListener(VisitCell);
        ReturnToMapButton.GetComponent<Button>().onClick.AddListener(ReturnToMap);

    }


    // Update is called once per frame
    void Update()
    {
        if (FirstUpdate)
        {
            MapParent = GameObject.Find("Chunk Meshes");
            FirstUpdate = false;
        }

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //Touched
                IsValidTouch = true;
            }


            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //Moved Finger (Now Invalid!)
                IsValidTouch = false;
            }

            if (IsValidTouch && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                TouchedOnly = true;
            }
        }


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

        if (!IsViewingInnerCell)
        {
            mainCamera.transform.position = new Vector3(Mathf.Clamp(mainCamera.transform.position.x, -40, 40), Mathf.Clamp(mainCamera.transform.position.y, MinHeight, 100), Mathf.Clamp(mainCamera.transform.position.z, -36, 30));
        }
        else
        {
            if (TimeGone < 1.2f)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(LastRot), Quaternion.Euler(90,0,0), TimeGone / 1.75f);
                mainCamera.transform.position = Vector3.Lerp(LastPos, SelectedCellPostition, TimeGone);
                BlackFade.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, TimeGone));

                TimeGone += Time.deltaTime * 0.33f;
            }
            else if (FadeTimer < 1.2f)
            {
                if (MapParent)
                    MapParent.SetActive(false);

                mainCamera.transform.position = Vector3.Lerp(new Vector3(400, 35, -35), new Vector3(400, 25, -25), FadeTimer);
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(30f, 50, FadeTimer), 0, 0));
                BlackFade.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, FadeTimer));
                FadeTimer += Time.deltaTime * 0.5f;
                if (FadeTimer > 1f)
                {
                    ReturnToMapButton.SetActive(true);
                    BlackFade.SetActive(false);
                }
            }
        }
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

        

        if ((Input.touchCount <= 0  && Input.GetMouseButtonDown(0)) || (Input.touchCount > 0 && TouchedOnly))
        {
            TouchedOnly = false;
            if (!IsViewingInnerCell)
            {
                if (!InfoCanvas.activeSelf)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100, layerMask))
                    {
                        SelectedCell = hit.transform.gameObject;
                        hit.transform.GetComponent<CellBehaviour>().GenerateTerrain();
                        Graph.cell = hit.transform.GetComponent<CellBehaviour>();
                        InfoCanvas.SetActive(true);
                        Graph.UpdateChart();
                        gameController.GetComponent<SpawnGrid>().IsOnMap = false;
                    }
                }
                else if (!BlockedByUI)
                {
                    InfoCanvas.SetActive(false);
                }
            }
            else
            {
                gameController.GetComponent<SpawnGrid>().IsOnMap = true;
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

    private void VisitCell()
    {
        IsViewingInnerCell = true;
        InfoCanvas.SetActive(false);
        MainMenuButton.SetActive(false);
        BlackFade.SetActive(true);
        LastPos = transform.position;
        LastRot = transform.rotation.eulerAngles;
        TimeGone = 0;
        FadeTimer = 0;

        direction = SelectedCellPostition - transform.position;
        toRotation = Quaternion.FromToRotation(transform.forward, direction);

        SelectedCellPostition = new Vector3(SelectedCell.transform.position.x, SelectedCell.transform.localScale.y / 5, SelectedCell.transform.position.z);
    }

    private void ReturnToMap()
    {
        ReturnToMapButton.SetActive(false);
        MainMenuButton.SetActive(true);
        MapParent.SetActive(true);
        IsViewingInnerCell = false;
        transform.position = LastPos;
        transform.rotation = Quaternion.Euler(LastRot);
    }

}
