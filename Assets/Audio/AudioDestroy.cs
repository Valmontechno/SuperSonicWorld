using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, GetComponent<AudioSource>().clip.length + 0.5f);
    }
}
