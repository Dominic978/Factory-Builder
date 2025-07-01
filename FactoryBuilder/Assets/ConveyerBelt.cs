using UnityEngine;

public class ConveyerBelt : Building
{
    public class BeltSpot 
    {
        public WorldItem item;
        public bool moving;
        public float timeMoving;

        public readonly Vector3 position;

        public BeltSpot(Vector3 position)
        {
            this.position = position;
            item = null;
            moving = false;
            timeMoving = 0;
        }
    }

    private void Awake()
    {
        Vector3 rightEdge = transform.right * BuildingManager.BuildingTileSize / 4;
        Vector3 frontEdge = transform.forward * BuildingManager.BuildingTileSize / 4;

        //sets position for where all the conveyer spots are
        conveyerChain[2] = new (-rightEdge - frontEdge + transform.position + Vector3.up);
        conveyerChain[3] = new (-rightEdge + frontEdge + transform.position + Vector3.up);
        conveyerChain[0] = new (rightEdge - frontEdge + transform.position + Vector3.up);
        conveyerChain[1] = new (rightEdge + frontEdge + transform.position + Vector3.up);
    }

    public Building forwardBuilding;

    public override void UpdateBuilding()
    {
        forwardBuilding = BuildingManager.GetBuilding<Building>(gridPos + new Vector2Int((int)transform.forward.x, (int)transform.forward.z));
        BuildingManager.GetBuilding<Storage>(gridPos - new Vector2Int((int)transform.forward.x, (int)transform.forward.z))?.AddConveyerBelt(this);
    }

    // 0 = rightConveyerChain[0]
    // 1 = rightConveyerChain[1]
    // 2 = leftConveyerChain[0]
    // 3 = leftConveyerChain[1]
    [HideInInspector]
    public BeltSpot[] conveyerChain = new BeltSpot[4];

    private const float timeMovingBetweenSpots = 3;

    // TODO :
    //      - make it so when conveyerBelts that are being moved onto one that is rotated 90 Degrees to make is so the left and right lanes stay on the right and left if they is only 1 conveyer moving into it
    //      - when multiple items are moving into 1 spot pick a winner (one already on the conveyerBelt or same direction as the conveyer belt)

    //moves items on conveyerBelt (:
    public void Update()
    {
        HandleMovingBeltSpot(conveyerChain[2], conveyerChain[3]);
        HandleMovingBeltSpot(conveyerChain[0], conveyerChain[1]);

        //I've decided to let the forward conveyerBelt handle the movement
        if(forwardBuilding is ConveyerBelt forwardConveyerBelt)
        {
            forwardConveyerBelt.TransferItemToMyBelt(conveyerChain[3], true, transform.forward);
            forwardConveyerBelt.TransferItemToMyBelt(conveyerChain[1], false, transform.forward);
        }
        else if(forwardBuilding is Storage forwardStorage)
        {
            if (conveyerChain[1].item != null) 
                forwardStorage.AddItem(conveyerChain[1].item);
            if (conveyerChain[3].item != null) 
                forwardStorage.AddItem(conveyerChain[3].item);
        }
    }

    /// <summary>
    /// ONLY CALL IN UPDATE, you know cuz Time.deltaTime is being used
    /// </summary>
    /// <param name="a">from belt spot</param>
    /// <param name="b">to belt spot</param>
    private static void HandleMovingBeltSpot(BeltSpot a, BeltSpot b)
    {
        a.moving = a.item != null && (b.item == null || b.moving);
        if (a.moving)
            a.timeMoving += Time.deltaTime;
        else
            a.timeMoving = 0;
        float lerpTime = a.timeMoving / timeMovingBetweenSpots;
        if(a.item)
            a.item.transform.position = Vector3.Lerp(a.position, b.position, lerpTime);
        if (lerpTime >= 1 && b.item == null)
        {
            b.item = a.item;
            a.item = null;
            a.timeMoving = 0;
            a.moving = false;
        }
    }

    /// <summary>
    /// Called from a conveyerBelt that is points into this conveyerBelt
    /// (I will refer to that conveyerbelt as other)
    /// </summary>
    /// <param name="otherBeltSpot">the other's belts spot that contains the item that will be moving into us</param>
    /// <param name="_isLeftChain"></param>
    /// <param name="otherBeltForward"></param>
    public void TransferItemToMyBelt(BeltSpot otherBeltSpot, bool _isLeftChain, Vector3 otherBeltForward)
    {
        if(otherBeltForward == transform.forward)
        {
            if(_isLeftChain)
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[2]);
            else
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[0]);
        }
        else if(otherBeltForward == transform.right)//belt is on my left
        {
            if (_isLeftChain)
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[3]);
            else
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[2]);
        }
        else if (otherBeltForward == -transform.right)//belt is on my right
        {
            if (_isLeftChain)
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[0]);
            else
                HandleMovingBeltSpot(otherBeltSpot, conveyerChain[1]);
        }
        else if (otherBeltForward == -transform.forward)
            return;//point into eachother no point in moving items
    }
}
