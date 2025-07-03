using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int gridPos;
    public Vector2Int size;   

    public void Init(Vector2Int gridPos, Vector2Int size)
    {
        this.gridPos = gridPos;
        this.size = size;
        UpdateBuilding();
    }

    public virtual void UpdateBuilding()
    {
        //implement within children classes
    }
}
