using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    public GameObject slider;
    public AudioMixer mixer;
    private new AudioSource audio;
    public AudioClip buttonClick;
    public GameObject highscoreText;

    private void Start()
    {
        if (highscoreText != null)
        {
            highscoreText.GetComponent<TextMeshProUGUI>().text = "Quickest Win: " + PlayerPrefs.GetInt("Highscore", 30) + " years";
        }
        audio = GetComponent<AudioSource>();
        slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    public void ToggleMenu(GameObject menu)
    {
        audio.PlayOneShot(buttonClick);
        menu.SetActive(!menu.activeSelf);
    }

    public void SwitchScene(int sceneIndex)
    {
        audio.PlayOneShot(buttonClick);
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        audio.PlayOneShot(buttonClick);
        Application.Quit();
    }

    public void ControlVolume(float sliderValue)
    {
        float logValue = slider.GetComponent<Slider>().value;
        logValue = Mathf.Log10(logValue) * 20;
        mixer.SetFloat("MasterVol", logValue);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }
}
