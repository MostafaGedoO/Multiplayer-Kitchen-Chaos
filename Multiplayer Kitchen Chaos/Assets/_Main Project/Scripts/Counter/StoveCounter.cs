using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasPrograss
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private FryingRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasPrograss.OnCuttingPrograssChangedEventArgs> OnCuttingPrograssChanged;
    public enum State { Idel,Frying,Fried,Burned }

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;


    public event EventHandler<OnStateChangedEventArgs> OnStateChenged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }


    private void Start()
    {
        state = State.Idel;
    }

    private void Update()
    {
        if(HasKitchenObject())
        {
            if(state == State.Frying)
            {
                fryingTimer += Time.deltaTime;

                OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                {
                    PrograssAmountNormalized = fryingTimer / fryingRecipeSO.timer
                });

                if(fryingTimer > fryingRecipeSO.timer)
                {
                    //Fried
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpownKitchenObject(fryingRecipeSO.OutputObject, this);

                    state = State.Fried;
                    FireOnSteteChangedEvent();
                    burningTimer = 0;
                    fryingRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                }
            }
            else if (state == State.Fried)
            {
                burningTimer += Time.deltaTime;
                
                OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                {
                    PrograssAmountNormalized = burningTimer / fryingRecipeSO.timer
                });

                if (burningTimer > fryingRecipeSO.timer)
                {
                    //Burned
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpownKitchenObject(fryingRecipeSO.OutputObject, this);

                    state = State.Burned;
                    FireOnSteteChangedEvent();

                    OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                    {
                        PrograssAmountNormalized = 0f
                    });
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
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    FireOnSteteChangedEvent();
                    fryingTimer = 0;
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
            }
            else
            {
                //player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idel;
                FireOnSteteChangedEvent();

                OnCuttingPrograssChanged?.Invoke(this, new IHasPrograss.OnCuttingPrograssChangedEventArgs
                {
                    PrograssAmountNormalized = 0
                });
            }
        }
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

    private void FireOnSteteChangedEvent()
    {
        OnStateChenged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = this.state
        });
    }
}
