using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrgrassBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnCuttingPrograssChanged += CuttingCounter_OnCuttingPrograssChanged;

        gameObject.SetActive(false);
    }

    private void CuttingCounter_OnCuttingPrograssChanged(object sender, CuttingCounter.OnCuttingPrograssChangedEventArgs e)
    {
        barImage.fillAmount = e.PrograssAmountNormalized;

        if(e.PrograssAmountNormalized == 0 | e.PrograssAmountNormalized == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
