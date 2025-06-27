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
        leftConveyerChain[0] = new (-rightEdge - frontEdge + transform.position + Vector3.up);
        leftConveyerChain[1] = new (-rightEdge + frontEdge + transform.position + Vector3.up);
        rightConveyerChain[0] = new (rightEdge - frontEdge + transform.position + Vector3.up);
        rightConveyerChain[1] = new (rightEdge + frontEdge + transform.position + Vector3.up);
    }

    [HideInInspector]
    public BeltSpot[] leftConveyerChain = new BeltSpot[2];
    [HideInInspector]
    public BeltSpot[] rightConveyerChain = new BeltSpot[2];

    private const float timeMovingBetweenSpots = 3;

    //moves items on conveyerBelt (:
    public void Update()
    {
        HandleMovingBeltSpot(leftConveyerChain[0], leftConveyerChain[1]);
        HandleMovingBeltSpot(rightConveyerChain[0], rightConveyerChain[1]);

        //I've decided to let the forward conveyerBelt handle the movement
        ConveyerBelt forwardBelt = BuildingManager.GetBuilding<ConveyerBelt>(gridPos + new Vector2Int((int)transform.forward.x, (int)transform.forward.z));
        forwardBelt?.TransferItemToMyBelt(leftConveyerChain[1], true, transform.forward);
        forwardBelt?.TransferItemToMyBelt(rightConveyerChain[1], false, transform.forward);
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
                HandleMovingBeltSpot(otherBeltSpot, leftConveyerChain[0]);
            else
                HandleMovingBeltSpot(otherBeltSpot, rightConveyerChain[0]);
        }
        else if(otherBeltForward == transform.right)//belt is on my left
        {
            if (_isLeftChain)
                HandleMovingBeltSpot(otherBeltSpot, leftConveyerChain[1]);
            else
                HandleMovingBeltSpot(otherBeltSpot, leftConveyerChain[0]);
        }
        else if (otherBeltForward == -transform.right)//belt is on my right
        {
            if (_isLeftChain)
                HandleMovingBeltSpot(otherBeltSpot, rightConveyerChain[0]);
            else
                HandleMovingBeltSpot(otherBeltSpot, rightConveyerChain[1]);
        }
        else if (otherBeltForward == -transform.forward)
            return;//point into eachother no point in moving items
    }
}
