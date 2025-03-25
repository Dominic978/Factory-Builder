using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    [NonSerialized] public Vector2 movement;

    public Vector3 movement3d
    {
        get
        {
            return transform.forward * movement.y + transform.right * movement.x;
        }
    }

    public Vector3 mousePosition3d
    {
        get
        {
            Physics.Raycast(_camera.ScreenPointToRay(mousePosition), out RaycastHit hit, float.PositiveInfinity, raycastTarget);
            return hit.point;
        }
    }

    [Tooltip("Required for using mousePosition3d")]
    public Camera _camera;
    public LayerMask raycastTarget;

    [NonSerialized] public Vector2 mousePosition;
    [NonSerialized] public Vector2 rotation;
    [NonSerialized] public int buildingRotation;
    [NonSerialized] public bool clicked;

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

    public void Click(CallbackContext context)
    {
        clicked = context.ReadValue<float>() > 0.5f;
    }

    public void MousePosition(CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}
