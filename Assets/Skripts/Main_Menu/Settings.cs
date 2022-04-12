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
    Dropdown DropdownResolution;
    Dropdown DropdownVollbild;
    AudioSource audioSrc;
    Slider MusicVolume;

    public Sprite mute;
    public Sprite unmute;
    public Sprite volume25;
    public Sprite volume50;
    public Sprite volume75;


    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Setting_UI_Show()
    {
        Settings_UI.SetActive(true);
    }

    public void Change_Window()
    {
        string vollbild = DropdownVollbild.options[DropdownVollbild.value].text;

        string[] subsvollbild = vollbild.Split(' ');

        if (subsvollbild[0] == "Vollbild")
        {
            FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.fullScreenMode = fullScreenMode;
        }
        else if (subsvollbild[0] == "Fenstermodus")
        {
            FullScreenMode fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreenMode = fullScreenMode;
        }
    }

    public void Change_Resolution()
    {
        string s = DropdownResolution.options[DropdownResolution.value].text;

        string[] subs = s.Split(' ');

        SceneManager.LoadScene("MainMenu(" + subs[0] + "x" + subs[2] + ")");     
    }

    public void Change_Volume()
    {
        audioSrc.volume = MusicVolume.value;
        if (MusicVolume.value == 0.0f)
        {
            audioSrc.mute = true;
            MuteButton.GetComponent<Image>().sprite = mute;
        }
        else if (MusicVolume.value > 0.0f && MusicVolume.value <= 0.25f)
        {
            audioSrc.mute = false;
            MuteButton.GetComponent<Image>().sprite = volume25;
        }
        else if (MusicVolume.value > 0.25f && MusicVolume.value <= 0.50f)
        {
            audioSrc.mute = false;
            MuteButton.GetComponent<Image>().sprite = volume50;
        }
        else if (MusicVolume.value > 0.50f && MusicVolume.value <= 0.75f)
        {
            audioSrc.mute = false;
            MuteButton.GetComponent<Image>().sprite = volume75;
        }
        else if(MusicVolume.value == 1.0f)
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
        DropdownResolution = GameObject.Find("Auflösung").GetComponent<Dropdown>();
        DropdownVollbild = GameObject.Find("Anzeigemodus").GetComponent<Dropdown>();
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
