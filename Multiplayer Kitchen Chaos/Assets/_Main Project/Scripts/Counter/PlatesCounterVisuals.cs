using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisuals : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private GameObject plateVisual;

    private List<GameObject> spownedPlates;
    private float plateHight = 0.1f;

    private void Awake()
    {
        spownedPlates = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.OnPlateSpowned += PlateCounter_OnPlateSpowned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }


    private void PlateCounter_OnPlateSpowned(object sender, System.EventArgs e)
    {
        GameObject _plate = Instantiate(plateVisual,counterTopPoint);
        _plate.transform.localPosition = new Vector3(0,spownedPlates.Count * plateHight ,0);
        spownedPlates.Add(_plate);
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject _plate = spownedPlates[spownedPlates.Count - 1];
        spownedPlates.Remove(_plate);
        Destroy(_plate);
    }

}
