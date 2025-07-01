using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI stuff")]
    public GameObject buildingButtonPrefab;

    public Transform buildingButtonHolder;

    private void Start()
    {
        GenerateBuildingButtons();
    }

    private void GenerateBuildingButtons()
    {
        for (int i = 0; i < BuildingManager.instance.buildings.Length; i++)
        {
            GameObject buttonGameObject = Instantiate(buildingButtonPrefab, buildingButtonHolder);
            buttonGameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = BuildingManager.instance.buildings[i].name;
            int n = i;// I've had to set the variable to another to make the action actually work, so I am just gonna do this. It's only called once anyways
            buttonGameObject.GetComponent<Button>().onClick.AddListener(() => { PlayerController.instance.buildingId = n;});
        }
    }
}
