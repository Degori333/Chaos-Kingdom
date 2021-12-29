using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour , IPointerEnterHandler
{
    private new AudioSource audio;
    public AudioClip buttonSelect;

    private void Start()
    {
        audio = GameObject.Find("MenuHandler").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.PlayOneShot(buttonSelect);
    }
}
