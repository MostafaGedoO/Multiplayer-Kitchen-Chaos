using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "kitchenObject" , menuName = "Kitchen/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public string ObjectName;
    public GameObject ObjectPrefab;
    public Sprite ObjectIcon;
}
