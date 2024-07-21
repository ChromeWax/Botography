using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour
{
    private List<ResolutionCustomSetting> _applicableResolutions = new();
    private List<RefreshRate> _refreshRates = new();

    private int _screenWidth, _screenHeight;

    private RefreshRate _refreshRate;

    [SerializeField] private float _masterVolume;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _soundEffectsVolume;
    [SerializeField] private float _musicVolumeFromSlider;
    [SerializeField] private float _soundEffectsVolumeFromSlider;

    public TMP_Dropdown dropMenu;
    public TMP_Dropdown refDropMenu;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;
    public Toggle fullScreenButton;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;

        dropMenu.ClearOptions();
        refDropMenu.ClearOptions();

        foreach (var res in resolutions)
        {
            float correctWidth = ((float)res.height / 9) * 16;
            if ((int)correctWidth == res.width)
            {
                ResolutionCustomSetting newRes = new ResolutionCustomSetting(res.height, res.width);

                if (!_applicableResolutions.Contains(newRes))
                {
                    _applicableResolutions.Add(newRes);
                    var option = new TMP_Dropdown.OptionData(res.width + "x" + res.height);
                    dropMenu.options.Add(option);
                    if (res.width == Screen.width && res.height == Screen.height)
                        dropMenu.value = _applicableResolutions.Count - 1;
                }
                if (!_refreshRates.Contains(res.refreshRateRatio))
                {
                    _refreshRates.Add(res.refreshRateRatio);
                    var refOption = new TMP_Dropdown.OptionData(res.refreshRateRatio + " Hz");
                    refDropMenu.options.Add(refOption);
                    if (res.refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
                        refDropMenu.value = _refreshRates.Count - 1;
                }
            }
        }

        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            fullScreenButton.isOn = true;
        else
            fullScreenButton.isOn = false;

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolumeWithSlider(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolumeWithSlider(); });
        soundEffectsVolumeSlider.onValueChanged.AddListener(delegate { SetSoundEffectsVolumeWithSlider(); });
        SoundManager.Instance.BGMusicVolume(_musicVolume);
        SoundManager.Instance.AmbienceVolume(_soundEffectsVolume);
        SoundManager.Instance.UIVolume(_soundEffectsVolume);
        SoundManager.Instance.FootstepVolume(_soundEffectsVolume * 0.5f);
        SoundManager.Instance.EmitterVolume(_soundEffectsVolume);
    }

    public void ChangeResolution(TMP_Dropdown dropdown)
    {
        //Resolution[] resolutions = Screen.resolutions;
        Debug.Log(_applicableResolutions[dropdown.value].width + ", " + _applicableResolutions[dropdown.value].height);
        _screenWidth = _applicableResolutions[dropdown.value].width;
        _screenHeight = _applicableResolutions[dropdown.value].height;
        SetScreen();
    }

    public void ChangeRefreshRate(TMP_Dropdown dropdown)
    {
        _refreshRate = _refreshRates[dropdown.value];
        SetScreen();
    }

    public void SetScreen()
    {
        Screen.SetResolution(_screenWidth, _screenHeight, Screen.fullScreenMode, _refreshRate);
    }

    public void ChangeFullscreenStatus()
    {
        FullScreenMode _fullscreenMode = Screen.fullScreenMode;
        if(_fullscreenMode == FullScreenMode.FullScreenWindow)
        {
            _fullscreenMode = FullScreenMode.Windowed;
            fullScreenButton.isOn = false;
        }
        else if (_fullscreenMode == FullScreenMode.Windowed)
        {
            _fullscreenMode = FullScreenMode.FullScreenWindow;
            fullScreenButton.isOn = true;
        }
        Screen.fullScreenMode = _fullscreenMode;
    }

    public float GetMasterVolume()
    {
        return _masterVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public float GetSoundEffectsVolume()
    {
        return _soundEffectsVolume;
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = volume;
        _musicVolume *= _masterVolume;
        _soundEffectsVolume *= _masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        _musicVolume *= _masterVolume;
    }

    public void SetSoundEffectsVolume(float volume)
    {
        _soundEffectsVolume = volume;
        _soundEffectsVolume *= _masterVolume;
    }

    public void SetMasterVolumeWithSlider()
    {
        _masterVolume = masterVolumeSlider.value;
        _musicVolume = _masterVolume * _musicVolumeFromSlider;
        _soundEffectsVolume = _masterVolume * _soundEffectsVolumeFromSlider;
        SoundManager.Instance.BGMusicVolume(_musicVolume);
        SoundManager.Instance.AmbienceVolume(_soundEffectsVolume);
        SoundManager.Instance.UIVolume(_soundEffectsVolume);
        SoundManager.Instance.FootstepVolume(_soundEffectsVolume * 0.5f);
        SoundManager.Instance.EmitterVolume(_soundEffectsVolume);
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        //Debug.Log("Master volume is " + _masterVolume);
        //Debug.Log("Music volume is " + _musicVolume);
        //Debug.Log("Sound effects volume is " + _soundEffectsVolume);
    }

    public void SetMusicVolumeWithSlider()
    {
        _musicVolumeFromSlider = musicVolumeSlider.value;
        _musicVolume = _masterVolume * _musicVolumeFromSlider;
        SoundManager.Instance.BGMusicVolume(_musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolumeFromSlider);
        //Debug.Log("Music volume is " + _musicVolume);
    }

    public void SetSoundEffectsVolumeWithSlider()
    {
        _soundEffectsVolumeFromSlider = soundEffectsVolumeSlider.value;
        _soundEffectsVolume = _masterVolume * _soundEffectsVolumeFromSlider;
        SoundManager.Instance.AmbienceVolume(_soundEffectsVolume);
        SoundManager.Instance.UIVolume(_soundEffectsVolume);
        SoundManager.Instance.FootstepVolume(_soundEffectsVolume * 0.5f);
        SoundManager.Instance.EmitterVolume(_soundEffectsVolume);
        PlayerPrefs.SetFloat("SFXVolume", _masterVolume);
        //Debug.Log("Sound effects volume is " + _soundEffectsVolume);
    }
}
