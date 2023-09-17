using System;
using UnityEngine;
using Unity.Netcode;


public class Player : NetworkBehaviour,IKitchenObjectParent
{
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [Space]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 7f;
    [SerializeField] private Transform playerHoldPoint;

    public static Player LocalInstance { get; private set; }

    private BaseCounter selectedCounter;
    private bool isWalking;
    private Vector3 lastInteractionDir;
    private KitchenObject kitchenObject;

    //Selected Counter Changed Event
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public event EventHandler OnPickUpKitchenObject;

    //Events for visuals and pickup sounds
    public static event EventHandler OnAnyPlayerSpawnd;
    public static event EventHandler OnAnyPlayerPickedSomething;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
        }
        OnAnyPlayerSpawnd?.Invoke(this,EventArgs.Empty);
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += Interact;
        GameInput.Instance.OnInteractAlternateAction += InteractAlternate;
    }

    private void InteractAlternate(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Interact(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            HandleMovement();
            HandleInteractions();
        }
    }

    private void HandleMovement()
    {
        Vector2 _moveInput = GameInput.Instance.GetMovmentVectorNormalized();
        Vector3 _moveDir = new Vector3(_moveInput.x, 0f, _moveInput.y);

        //Moving into walls
        float _playerRadius = 0.7f;
        float _moveDistance = moveSpeed * Time.deltaTime;
        float _playerHeight = 2f;

        bool _canMove = !Physics.BoxCast(transform.position, Vector3.one * _playerRadius , _moveDir,Quaternion.identity , _moveDistance, collisionsLayerMask);

        if (!_canMove)
        {
            //try moveing on the x only
            Vector3 _moveDirX = new Vector3(_moveDir.x, 0, 0).normalized;
            _canMove = _moveDir.x != 0 & !Physics.BoxCast(transform.position, Vector3.one * _playerRadius, _moveDirX, Quaternion.identity, _moveDistance, collisionsLayerMask);

            if (_canMove)
            {
                //we can move on the x
                _moveDir = _moveDirX;
            }
            else
            {
                //cant move on x lets try z
                Vector3 _moveDirZ = new Vector3(0, 0, _moveDir.z).normalized;
                _canMove = _moveDir.z != 0 & !Physics.BoxCast(transform.position, Vector3.one * _playerRadius, _moveDirZ, Quaternion.identity, _moveDistance, collisionsLayerMask);

                if (_canMove)
                {
                    //we can move on the z
                    _moveDir = _moveDirZ;
                }
                else
                {
                    //we cant move any where
                }
            }
        }

        if (_canMove)
        {
            transform.position += _moveDir * _moveDistance;
        }

        if (_moveDir != Vector3.zero)
        {
             transform.forward = Vector3.Slerp(transform.forward, _moveDir, Time.deltaTime * rotateSpeed);
        }

        //handle is walking used for animation
        if(_moveDir != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

    }

    private void HandleInteractions()
    {
        Vector2 _moveInput = GameInput.Instance.GetMovmentVectorNormalized();
        Vector3 _moveDir = new Vector3(_moveInput.x, 0f, _moveInput.y);
       
        if(_moveDir != Vector3.zero)
        {
            lastInteractionDir = _moveDir;
        }

        float _InteractionDistance = 2f;

        if(Physics.Raycast(transform.position,lastInteractionDir, out RaycastHit raycastHit, _InteractionDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter _baseCounter))
            {
                if(_baseCounter != selectedCounter)
                {
                    SetSelectedCounter(_baseCounter);
                }
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
       
    }

    private void SetSelectedCounter(BaseCounter _baseCounter)
    {
        selectedCounter = _baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { 
            SelectedCounter = _baseCounter
        });;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetFollowTrandormPoint()
    {
        return playerHoldPoint;
    }

    public void SetKitchenObject(KitchenObject _kitchenObject)
    {
        kitchenObject = _kitchenObject;

        if(_kitchenObject != null)
        {
            OnPickUpKitchenObject?.Invoke(this,EventArgs.Empty);
            OnAnyPlayerPickedSomething?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearkitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public static void ClearStaticDate()
    {
        OnAnyPlayerSpawnd = null;
        OnAnyPlayerPickedSomething = null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
