using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrgrassBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasPrograssGameObject;
    [SerializeField] private Image barImage;
    
    private IHasPrograss hasProgress;

    private void Start()
    {
        hasProgress = hasPrograssGameObject.GetComponent<IHasPrograss>();

        hasProgress.OnCuttingPrograssChanged += HasProgress_OnCuttingPrograssChanged;

        gameObject.SetActive(false);
    }

    private void HasProgress_OnCuttingPrograssChanged(object sender, IHasPrograss.OnCuttingPrograssChangedEventArgs e)
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
