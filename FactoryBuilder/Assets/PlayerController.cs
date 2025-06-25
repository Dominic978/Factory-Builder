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

    public int buildingId { get; set; } = -1;

    //where is omni-man?
    //where is he!!
    //are you sure
    //where is he!!
    //are you sure
    //you bastard
    //where is he!!
    //are you sure

    private void Update()
    {
        //movement
        transform.position += input.movement3d * speed * Time.deltaTime;

        //rotation
        horizontalRotation += input.rotation.x * Time.deltaTime * horizontalRotationSpeed;
        verticalRotation += input.rotation.y * Time.deltaTime * verticalRotationSpeed;

        transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        _camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //building
        if(buildingId != -1)
        {
            bool pointerOverUI = input.IsPointerOverUIElement();

            if (pointerOverUI)
                BuildingManager.instance.CancelPreviewBuilding(buildingId);
            else
                BuildingManager.instance.HandlePreviewBuilding(buildingId, 0, input.mousePosition3d);

            if (input.leftClicked)
            {
                if (!pointerOverUI)
                    BuildingManager.instance.PlaceBuilding(buildingId, 0, input.mousePosition3d);
                input.leftClicked = false;
            }
            else if(input.rightClicked)
            {
                BuildingManager.instance.CancelPreviewBuilding(buildingId);
                buildingId = -1;
            }
        }
    }
}
