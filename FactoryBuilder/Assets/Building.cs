using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int gridPos;//set at init
    public Vector2Int size;   //set at init

    private void Update()
    public void Init(Vector2Int gridPos, Vector2Int size)
    {
        this.gridPos = gridPos;
        this.size = size;
    }
}
