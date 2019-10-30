using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Slider ZoomSlider;
    public int speed = 4;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                ZoomSlider.value += (1 * speed);
            }
            if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
            {
                ZoomSlider.value -= (1 * speed);
        
            }
        }
        
        // Slider Zoom
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, ZoomSlider.value, mainCamera.transform.position.z);
    }


}
