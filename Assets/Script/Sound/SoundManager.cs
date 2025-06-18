using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip mergeSound;

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayMergeSound()
    {
        audioSource.PlayOneShot(mergeSound);
    }
}
