using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]

public class UIRaycaster : MonoBehaviour
{
    public static bool isPauseButtonHit = false, isTouchEnabled = true;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Create a new PointerEventData to store the raycast results
            pointerEventData = new PointerEventData(eventSystem);

            // Set the pointer's position to the mouse click position
            pointerEventData.position = Input.mousePosition;

            // Create a list to store the raycast results
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointerEventData, results);

            // Check if any UI element was hit
            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    GameObject clickedObject = result.gameObject;
                    if(clickedObject.name == "Pause")
                    {
                        isPauseButtonHit = true;
                    }

                    if(clickedObject.name == "Restart")
                    {
                        StartCoroutine(EnableTouch());
                    }
                }
            }
        }
    }

    private IEnumerator EnableTouch()
    {
        isTouchEnabled = false;
        yield return new WaitForSeconds(0.1f);
        isTouchEnabled = true;
    }
}
