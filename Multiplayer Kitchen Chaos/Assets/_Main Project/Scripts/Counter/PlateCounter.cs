using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    public event EventHandler OnPlateSpowned;
    public event EventHandler OnPlateRemoved;

    private float timeToSpownPlate = 4;
    private float spownPlateMaxNumber = 5;
    private float spownedPlates = 0;
    private float timer;

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            if (spownedPlates < spownPlateMaxNumber)
            {
                timer += Time.deltaTime;
                if (timer >= timeToSpownPlate)
                {
                    //spown plate
                    OnPlateSpowned?.Invoke(this, EventArgs.Empty);

                    timer = 0;
                    spownedPlates++;
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(spownedPlates > 0)
            {
                //remove one plate and give it to the player
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

                KitchenObject.SpownKitchenObject(plateKitchenObjectSO, player);

                spownedPlates--;
            }
        }
    }

}
