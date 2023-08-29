using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioRefsSO audioRefsSO;
    
    public static SoundManager instance { get; private set; }
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliveryFailed += DeliveryManager_OnDeliveryFailed;
        DeliveryManager.Instance.OnDeliveryCompleted += DeliveryManager_OnDeliveryCompleted;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUpKitchenObject += Player_OnPickUpKitchenObject;
        BaseCounter.OnAnyKitchenObjectDropedOnACounter += BaseCounter_OnAnyKitchenObjectDropedOnACounter;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter _trashCounter = sender as TrashCounter;
        PlaySound(audioRefsSO.trash, _trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyKitchenObjectDropedOnACounter(object sender, System.EventArgs e)
    {
        BaseCounter _baseCounter = sender as BaseCounter;
        PlaySound(audioRefsSO.objectDrop, _baseCounter.transform.position);
    }

    private void Player_OnPickUpKitchenObject(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.objectPickUp, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter _cuttingCounter = sender as CuttingCounter;
        PlaySound(audioRefsSO.chope, _cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnDeliveryCompleted(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.deliverySuccess, DeliveryCounter.Instacne.transform.position);
    }

    private void DeliveryManager_OnDeliveryFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioRefsSO.deliverFail, DeliveryCounter.Instacne.transform.position);
    }

    public void PlaySound(AudioClip[] _audioClip, Vector3 _position, float _volume = 1f)
    {
        AudioSource.PlayClipAtPoint(_audioClip[Random.Range(0,_audioClip.Length)], _position, _volume);
    }

    public void PlaySound(AudioClip _audioClip, Vector3 _position,float _volume = 1f)
    {
        AudioSource.PlayClipAtPoint(_audioClip, _position, _volume);
    }

    public void PlayFootStepSound()
    {
        PlaySound(audioRefsSO.footSteps, Player.Instance.transform.position);
    }
}
