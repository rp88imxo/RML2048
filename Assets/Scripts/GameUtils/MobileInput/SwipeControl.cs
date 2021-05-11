using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    [SerializeField, Range(0f, 30000f)]
    float minDistanceSquaredToSwipe = 10000f;

    [SerializeField, Range(0f, 1f)]
    float thresholdOfMinDistance = 0.85f;

    bool
        tap,
        swipeLeft,
        swipeRight,
        swipeUp,
        swipeDown;

    bool isDragging;

    Vector2 startTouch, swipeDelta;
    Touch t;

    float minDistance;

    public Vector2 SwipeDelta => swipeDelta;
    public bool SwipeLeft => swipeLeft;
    public bool SwipeRight => swipeRight;
    public bool SwipeUp => swipeUp;
    public bool SwipeDown => swipeDown;

    // Start is called before the first frame update
    void Start()
    {
        minDistance = Mathf.Sqrt(minDistanceSquaredToSwipe) * thresholdOfMinDistance;
    }

    // Update is called once per frame
    void Update()
    {
        ResetSwipeState();
        #region PC_INPUT
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            tap = isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetState();
        }
        #endregion

        #region MOBILE_INPUT
#if PLATFORM_ANDROID
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            
            if (t.phase == TouchPhase.Began)
            {
                tap = isDragging = true;
                startTouch = t.position;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                ResetState();
            }
        }
#endif
        #endregion

       // swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touchCount > 0)
            {
                swipeDelta = t.position - startTouch;
            }
            if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        if (swipeDelta.sqrMagnitude > minDistanceSquaredToSwipe)
        {
            float xPos = swipeDelta.x;
            float yPos = swipeDelta.y;
            float xPosAbs = Mathf.Abs(xPos);
            float yPosAbs = Mathf.Abs(yPos);

            if (xPosAbs > yPosAbs)
            {
                if (xPos > 0f)
                {
                    if (xPos - yPosAbs >= minDistance)
                    {
                        swipeRight = true;
                    }
                }
                else
                {
                    if (xPosAbs - yPosAbs >= minDistance)
                    {
                        swipeLeft = true;
                    }
                }
            }
            else if (xPosAbs < yPosAbs)
            {
                if (yPos > 0f)
                {
                    if (yPos - xPosAbs >= minDistance)
                    {
                        swipeUp = true;
                    }
                }
                else
                {
                    if (yPosAbs - xPosAbs >= minDistance)
                    {
                        swipeDown = true;
                    }
                }

            }

            ResetState();
        }
    }

    void ResetSwipeState()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }

    void ResetState()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }
}
