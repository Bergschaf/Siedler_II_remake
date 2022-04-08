using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{

    GameObject MuteButton;
    GameObject Settings_UI;
    AudioSource audioSrc;
    Slider MusicVolume;

    public Sprite mute;
    public Sprite unmute;


    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Setting_UI_Show()
    {
        Settings_UI.SetActive(true);
    }

    public void Change_Volume()
    {
        audioSrc.volume = MusicVolume.value;
        if (MusicVolume.value == 0.0f)
        {
            audioSrc.mute = true;
            MuteButton.GetComponent<Image>().sprite = mute;
        }
        else if(MusicVolume.value > 0.0f)
        {
            audioSrc.mute = false;
            MuteButton.GetComponent<Image>().sprite = unmute;
        }
    }

    public void Setting()
    {
        if (audioSrc.mute == true)
        {
            audioSrc.mute = false;
            MusicVolume.value = 1.0f;
            MuteButton.GetComponent<Image>().sprite = unmute;
        }
        else
        {
            audioSrc.mute = true;
            MusicVolume.value = 0.0f;
            MuteButton.GetComponent<Image>().sprite = mute;
        }
        
    }

    public void Setting_UI_Close()
    {
        Settings_UI.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }



    private void Awake()
    {
        Settings_UI = GameObject.Find("Settings_UI");
        MuteButton = GameObject.Find("Mute");
        audioSrc = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
        MusicVolume = GameObject.Find("MusicVolume").GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Settings_UI.SetActive(false);
        Screen.SetResolution(1920, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
