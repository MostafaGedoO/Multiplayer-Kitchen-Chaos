using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        if (!IsServer) return;

        if (GameManager.Instance.IsGamePlaying())
        {
            if (spownedPlates < spownPlateMaxNumber)
            {
                timer += Time.deltaTime;
                if (timer >= timeToSpownPlate)
                {
                    //spown plate
                    timer = 0;
                    SpawnPlateServerRpc();
                }
            }
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        OnPlateSpowned?.Invoke(this, EventArgs.Empty);
        spownedPlates++;
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (spownedPlates > 0)
            {
                //remove one plate and give it to the player
                KitchenObject.SpownKitchenObject(plateKitchenObjectSO, player);
                InteractServerRpc();
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc()
    {
        InteractClientRpc();
    }

    [ClientRpc]
    private void InteractClientRpc()
    {
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        spownedPlates--;
    }
}