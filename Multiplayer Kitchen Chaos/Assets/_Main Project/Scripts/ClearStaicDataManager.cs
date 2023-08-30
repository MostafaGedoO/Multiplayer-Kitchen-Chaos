using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearStaicDataManager : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.ClearStaticDate();
        TrashCounter.ClearStaticDate();
        BaseCounter.ClearStaticDate();
    }
}
