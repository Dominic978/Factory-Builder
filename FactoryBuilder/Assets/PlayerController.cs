using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public float horizontalRotationSpeed;
    public float verticalRotationSpeed;

    public Camera _camera;

    private InputHandler input;

    private void Awake()
    {
        input = GetComponent<InputHandler>();
    }

    private float horizontalRotation = 0;
    private float verticalRotation = 50;

    private void Update()
    {
        //movement
        transform.position += input.movement3d * speed * Time.deltaTime;

        //rotation
        horizontalRotation += input.rotation.x * Time.deltaTime * horizontalRotationSpeed;
        verticalRotation += input.rotation.y * Time.deltaTime * verticalRotationSpeed;

        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        _camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        BuildingManager.instance.HandlePreviewBuilding(0, 0, input.mousePosition3d);

        if(input.clicked)
        {
            BuildingManager.instance.PlaceBuilding(0, 0, input.mousePosition3d);
            input.clicked = false;
        }
    }
}
