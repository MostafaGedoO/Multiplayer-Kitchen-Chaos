using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasPrograss
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private FryingRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasPrograss.OnCuttingPrograssChangedEventArgs> OnFryingPrograssChanged;
    public enum State { Idel,Frying,Fried,Burned }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idel);
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSO fryingRecipeSO;


    public event EventHandler<OnStateChangedEventArgs> OnStateChenged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged  += OnFryingTimerValueChanged;
        burningTimer.OnValueChanged += OnBurningTimerValueChanged;
        state.OnValueChanged        += OnStateValueChanged;
    }

    private void OnFryingTimerValueChanged(float previousValue, float newValue)
    {
        float _fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.timer : 1f;

        OnFryingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
        {
            PrograssAmountNormalized = fryingTimer.Value / _fryingTimerMax
        });
    }  
    
    private void OnBurningTimerValueChanged(float previousValue, float newValue)
    {
        float _burningTimerMax = fryingRecipeSO != null ? fryingRecipeSO.timer : 1f;

        OnFryingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
        {
            PrograssAmountNormalized = burningTimer.Value / _burningTimerMax
        });
    }

    private void OnStateValueChanged(State previousState,State newState)
    {
        OnStateChenged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = this.state.Value
        });

        if(state.Value == State.Burned | state.Value == State.Idel)
        {
            OnFryingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
            {
                PrograssAmountNormalized = 0
            });
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        if(HasKitchenObject())
        {
            if(state.Value == State.Frying)
            {
                fryingTimer.Value += Time.deltaTime;

                if(fryingTimer.Value > fryingRecipeSO.timer)
                {
                    //Fried
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpownKitchenObject(fryingRecipeSO.OutputObject, this);

                    state.Value = State.Fried;
                    burningTimer.Value = 0;
                    GetBurningRcipeSOClientRpc(MultiPlayerGameManager.Instance.GetKitchenObjectSOIndexFromList(GetKitchenObject().GetKitchenObjectSO())); 
                }
            }
            else if (state.Value == State.Fried)
            {
                burningTimer.Value += Time.deltaTime;
                
                if (burningTimer.Value > fryingRecipeSO.timer)
                {
                    //Burned
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpownKitchenObject(fryingRecipeSO.OutputObject, this);

                    state.Value = State.Burned;
                }
            }
        }
    }

  

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //nothing on the counter
            if (player.HasKitchenObject())
            {
                //player has a kitchen object
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                { 
                    KitchenObject _kitchenObject = player.GetKitchenObject();
                    _kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicServerRpc(MultiPlayerGameManager.Instance.GetKitchenObjectSOIndexFromList(_kitchenObject.GetKitchenObjectSO()));
                }
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //counter had an object
            if (player.HasKitchenObject())
            {
                //player has an object
                if (player.HasKitchenObject())
                {
                    //player has an object
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject _plateKitchenObject))
                    {
                        //player has a plate
                        if (_plateKitchenObject.TryAddIngrediant(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();

                            SetStateServerRpc(State.Idel);
                        }
                    }
                }
            }
            else
            {
                //player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
                SetStateServerRpc(State.Idel);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateServerRpc(State state)
    {
        this.state.Value = state;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc(int _kitchenObjectSOIndex)
    {
        fryingTimer.Value = 0;
        state.Value = State.Frying;
        GetFryingRecipeSOClientRpc(_kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void GetFryingRecipeSOClientRpc(int _kitchenObjectSOIndex)
    {
        KitchenObjectSO _kitchenObjectSO = MultiPlayerGameManager.Instance.GetKitchenObjectSOFromList(_kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(_kitchenObjectSO);
    } 
    
    [ClientRpc]
    private void GetBurningRcipeSOClientRpc(int _kitchenObjectSOIndex)
    {
        KitchenObjectSO _kitchenObjectSO = MultiPlayerGameManager.Instance.GetKitchenObjectSOFromList(_kitchenObjectSOIndex);
        fryingRecipeSO = GetBurningRecipeSOWithInput(_kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectSO _inputKitchenObjectSO)
    {
        FryingRecipeSO _fryingRecipeSo = GetFryingRecipeSOWithInput(_inputKitchenObjectSO);
        return _fryingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO _inputKitchenObjectSO)
    {
        FryingRecipeSO _fryingRecipeSO = GetFryingRecipeSOWithInput(_inputKitchenObjectSO);
        if (_fryingRecipeSO != null)
        {
            return _fryingRecipeSO.OutputObject;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO _inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO _fryingRecipeSO in fryingRecipeSOArray)
        {
            if(_fryingRecipeSO.InputObject == _inputKitchenObjectSO)
            {
                return _fryingRecipeSO;
            }
        }
        return null;
    }
    
    private FryingRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO _inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO _fryingRecipeSO in burningRecipeSOArray)
        {
            if(_fryingRecipeSO.InputObject == _inputKitchenObjectSO)
            {
                return _fryingRecipeSO;
            }
        }
        return null;
    }

    public bool IsFriedState()
    {
        return state.Value == State.Fried;
    }
}
