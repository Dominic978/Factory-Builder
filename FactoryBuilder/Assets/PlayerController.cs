using UnityEngine;

/// <summary>
///     Basic player controller
///     <list type="bullet">
///         <item>rotatation on X and Y axis's</item>
///         <item>Movement on X and Z axis's</item>
///         <item>right click to stop building</item>
///         <item>
///             Can place buildings
///             <list type="bullet">
///                 if Mouse is hovering over UI while placing buildings, doesn't place
///             </list>
///         </item>
///     </list>
/// </summary>
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
                BuildingManager.instance.HandlePreviewBuilding(buildingId, input.buildingRotation, input.mousePosition3d);

            if (input.leftClicked)
            {
                if (!pointerOverUI)
                    BuildingManager.instance.PlaceBuilding(buildingId, input.buildingRotation, input.mousePosition3d);
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
