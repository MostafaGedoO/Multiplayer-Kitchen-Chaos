using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioRefsSO : ScriptableObject
{
    public AudioClip[] chope;
    public AudioClip[] deliverFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footSteps;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip   panSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
}
