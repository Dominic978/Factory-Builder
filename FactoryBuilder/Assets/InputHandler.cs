using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    // WASD controls mapped to Vector2
    [NonSerialized] public Vector2 movement;

    // movement represented with a Vector3 and points in the direction the attached transform is facing
    public Vector3 movement3d
    {
        get
        {
            return transform.forward * movement.y + transform.right * movement.x;
        }
    }

    // raycasted mousePosition from <see cref="_camera"/>
    public Vector3 mousePosition3d
    {
        get
        {
            Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out RaycastHit hit, float.PositiveInfinity, mouse3dLayerMask);
            return hit.point;
        }
    }

    private void Start()
    {
        uiLayer = LayerMask.NameToLayer("UI");
    }

    private int uiLayer;

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == uiLayer)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    [Tooltip("The camera that mousePosition3d uses for raycasting")]
    public Camera _camera;
    [Tooltip("The Layer Mask that is used when raycasting for mousePosition3d")]
    public LayerMask mouse3dLayerMask;

    [NonSerialized] public Vector2 mousePosition;
    // mouse delta, used for rotation PlayerController
    [NonSerialized] public Vector2 rotation;
    // building rotation for the Y axis, and when applying to Building multipled by 90
    [NonSerialized] public int buildingRotation;
    [NonSerialized] public bool leftClicked;
    [NonSerialized] public bool rightClicked;

    public void BuildingRotatation(CallbackContext context)
    {
        buildingRotation += (int)Mathf.Clamp(context.ReadValue<float>(), -1, 1);
        buildingRotation %= 4;
    }

    public void Move(CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void RotateHorizontal(CallbackContext context)
    {
        rotation.x = context.ReadValue<float>();
    }

    public void RotateVertical(CallbackContext context)
    {
        rotation.y = context.ReadValue<float>();
    }

    public void LeftClick(CallbackContext context)
    {
        leftClicked = context.ReadValue<float>() > 0.5f;
    }

    public void RightClicked(CallbackContext context)
    {
        rightClicked = context.ReadValue<float>() > 0.5f;
    }

    public void MousePosition(CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}
