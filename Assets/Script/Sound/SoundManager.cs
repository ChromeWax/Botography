using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private string currentRegion="";

    [SerializeField] GameObject ambienceSource, secondaryAmbienceSource, footstepSource, UISource, emitterSource, secondaryEmitterSource;
    private AudioSource _ambiencePlayer, _secondaryAmbiencePlayer, _footstepPlayer, _uiPlayer, _emitterPlayer, _secondaryEmitterPlayer;
    private AudioSource _musicAudioSource;
    public static SoundManager Instance { get; private set; }
    [Header("BG Music")]
    [SerializeField] AudioClip hotMusic, coldMusic, swampMusic, midnightMusic, forestMusic, fairyCircle;
    [Header("Panacean Music")]
    [SerializeField] AudioClip hotPanacean, coldPanacean, swampPanacean, midnightPanacean;

    [Header("Ambient Sounds")]
    [SerializeField] List<SFX> ambientSFX;

    [Header("Footstep Sounds")]
    [SerializeField] List<SFX> forestFootsteps;
    [SerializeField] List<SFX> swampFootsteps;
    [SerializeField] List<SFX> coldFootsteps;
    [SerializeField] List<SFX> hotFootsteps;
    [SerializeField] List<SFX> midnightFootsteps;

    [Header("Special Footsteps Sounds")]
    [SerializeField] List<SFX> puddleFootsteps;
    [SerializeField] List<SFX> waterFootsteps;

    private List<SFX> footstepSFX;

    [Header("UI Sounds")]
    [SerializeField] List<SFX> uiSFX;

    [SerializeField] GameObject _almanacUI;
    [SerializeField] GameObject _mapUI;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _labUI;
    [SerializeField] GameObject _inventoryUI;
    

    private int _footstepIndex=0;
    private bool _specialFootstepRegion = false;
    private string _specialFootstep="";


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        _musicAudioSource=GetComponent<AudioSource>();
        _ambiencePlayer=ambienceSource.GetComponent<AudioSource>();
        _secondaryAmbiencePlayer=secondaryAmbienceSource.GetComponent<AudioSource>();
        _footstepPlayer=footstepSource.GetComponent<AudioSource>();
        _uiPlayer=UISource.GetComponent<AudioSource>();
        _emitterPlayer=emitterSource.GetComponent<AudioSource>();
        _secondaryEmitterPlayer=secondaryEmitterSource.GetComponent<AudioSource>();
    }

    public void SwitchMusic(string region){
        if (!currentRegion.Equals(region)){
            if(_secondaryAmbiencePlayer.isPlaying)
                _secondaryAmbiencePlayer.Stop();
            switch (region){
                case "Hot" : {
                    _musicAudioSource.clip = hotMusic;
                    currentRegion=region;
                    _secondaryAmbiencePlayer.clip = FindSFX("wind whistle", ambientSFX);
                    _secondaryAmbiencePlayer.Play();
                    break;
                }
                case "Cold" : {
                    _musicAudioSource.clip = coldMusic;
                    currentRegion=region;
                    _secondaryAmbiencePlayer.clip = FindSFX("wind whistle", ambientSFX);
                    _secondaryAmbiencePlayer.Play();
                    break;
                }
                case "Midnight" : {
                    _musicAudioSource.clip = midnightMusic;
                    currentRegion=region;
                    _secondaryAmbiencePlayer.clip = FindSFX("wind trees", ambientSFX);
                    _secondaryAmbiencePlayer.Play();
                    break;
                }
                case "Swamp" : {
                    _musicAudioSource.clip = swampMusic;
                    currentRegion=region;
                    _secondaryAmbiencePlayer.Play();
                    break;
                }
                case "Forest" : {
                    _musicAudioSource.clip = forestMusic;
                    currentRegion=region;
                    _secondaryAmbiencePlayer.clip = FindSFX("leaves", ambientSFX);
                    _secondaryAmbiencePlayer.Play();
                    break;
                }
            }
            SelectFootsteps();
            _musicAudioSource.Play();
        }
    }

    public void TriggerSpecialFootsteps(string footstep){
        _specialFootstepRegion=true;
        _specialFootstep=footstep;
        SelectFootsteps();
        
    }

    public void PlayFairyMusic(){
        _musicAudioSource.clip = fairyCircle;
        _secondaryAmbiencePlayer.Stop();
        _musicAudioSource.Play();
    }

    public void StopFairyMusic(){
        _musicAudioSource.clip = forestMusic;
        _secondaryAmbiencePlayer.clip = FindSFX("leaves", ambientSFX);
        _secondaryAmbiencePlayer.Play();
        _musicAudioSource.Play();
    }

    public void PlayPanaceanMusic(){
        switch (currentRegion){
                case "Hot" : {
                    _musicAudioSource.clip = hotPanacean;
                    break;
                }
                case "Cold" : {
                    _musicAudioSource.clip = coldPanacean;
                    break;
                }
                case "Midnight" : {
                    _musicAudioSource.clip = midnightPanacean;
                    break;
                }
                case "Swamp" : {
                    _musicAudioSource.clip = swampPanacean;
                    break;
                }
            }
            _musicAudioSource.Play();
    }

    public void StopPanaceanMusic(){
        switch (currentRegion){
                case "Hot" : {
                    _musicAudioSource.clip = hotMusic;
                    break;
                }
                case "Cold" : {
                    _musicAudioSource.clip = coldMusic;
                    break;
                }
                case "Midnight" : {
                    _musicAudioSource.clip = midnightMusic;
                    break;
                }
                case "Swamp" : {
                    _musicAudioSource.clip = swampMusic;
                    break;
                }
            }
            _musicAudioSource.Play();
    }

    public void StopSpecialFootstep(){
        _specialFootstepRegion=false;
        SelectFootsteps();
    }

    public void SelectFootsteps(){
        if(!_specialFootstepRegion){
            switch (currentRegion){
                case "Hot" : {
                    footstepSFX=hotFootsteps;
                    break;
                }
                case "Cold" : {
                    footstepSFX=coldFootsteps;
                    break;
                }
                case "Midnight" : {
                    footstepSFX=midnightFootsteps;
                    break;
                }
                case "Swamp" : {
                    footstepSFX=swampFootsteps;
                    break;
                }
                case "Forest" : {
                    footstepSFX=forestFootsteps;
                    break;
                }
            }
        }
        else{
            switch (_specialFootstep){
                case "Puddle" : {
                    footstepSFX=puddleFootsteps;
                    break;
                }
                case "Water" : {
                    footstepSFX=waterFootsteps;
                    break;
                }
            }
        }
    }

    public void PlayFootsteps(){
        if(!_footstepPlayer.isPlaying && !_almanacUI.activeInHierarchy && !_mapUI.activeInHierarchy && !_inventoryUI.activeInHierarchy && !_pauseMenu.activeInHierarchy && !_labUI.activeInHierarchy)
        {
            _footstepPlayer.clip = footstepSFX[_footstepIndex++].sfxAudio;
            if(_footstepIndex==6){
                _footstepIndex=0;
            }
            _footstepPlayer.Play();
        }
    }

    public void PlayAmbient(string sfxName){
        AudioClip ambientClip = FindSFX(sfxName, ambientSFX);
        if(ambientClip != null){
            _ambiencePlayer.clip = ambientClip;
            _ambiencePlayer.Play();
        }
    }

    public void PlayAmbient(AmbientSFX ambientSFX){
        _ambiencePlayer.clip = ambientSFX.sfxAudio;
        _ambiencePlayer.Play();
    }

    public void PlayEmitter(string sfxName){
        AudioClip emitterClip = FindSFX(sfxName, ambientSFX);
        if(emitterClip != null){
            if(!_emitterPlayer.isPlaying){
                _emitterPlayer.clip = emitterClip;
                _emitterPlayer.Play();
            }
            else{
                _secondaryEmitterPlayer.clip = emitterClip;
                
            }
        }
    }

    public void PlayEmitter(AmbientSFX ambientSFX){
        if(!_emitterPlayer.isPlaying){
            _emitterPlayer.clip = ambientSFX.sfxAudio;
            _emitterPlayer.Play();
        }
        else{
            _secondaryEmitterPlayer.clip = ambientSFX.sfxAudio;
            _secondaryEmitterPlayer.Play();
        }

    }

    public void MuteFootsteps(){
        _footstepPlayer.mute = true;
    }

    public void UnmuteFootsteps(){
        _footstepPlayer.mute = false;
    }

    public void StopAmbient(){
        if(_ambiencePlayer.isPlaying)
            _ambiencePlayer.Pause();
    }

    public void StopEmitter(){
        if(_emitterPlayer.isPlaying)
            _emitterPlayer.Stop();
        if(_secondaryEmitterPlayer.isPlaying)
            _secondaryEmitterPlayer.Stop();
    }

    public void PlaySFX(string sfxName){
        AudioClip sfxClip = FindSFX(sfxName, uiSFX);
        if(sfxClip != null){
            _uiPlayer.clip = sfxClip;
            _uiPlayer.Play();
        }
    }

    public void StopSFX(){
        _uiPlayer.Stop();
    }
    public void ButtonClicked(){
        AudioClip sfxClip = FindSFX("button clicked", uiSFX);
        if(sfxClip != null){
            _uiPlayer.clip = sfxClip;
            _uiPlayer.Play();
        }
    }

    public void MenuOpened(){
        AudioClip sfxClip = FindSFX("menu open", uiSFX);
        if(sfxClip != null){
            _uiPlayer.clip = sfxClip;
            _uiPlayer.Play();
        }
    }

    public void MenuClosed(){
        AudioClip sfxClip = FindSFX("menu close", uiSFX);
        if(sfxClip != null){
            _uiPlayer.clip = sfxClip;
            _uiPlayer.Play();
        }
    }

    public void BGMusicVolume (float volume){
        _musicAudioSource.volume = volume;
    }

    public void UIVolume (float volume){
        _uiPlayer.volume = volume;
    }

    public void AmbienceVolume (float volume){
        _ambiencePlayer.volume = volume;
        _secondaryAmbiencePlayer.volume = volume;
    }

    public void FootstepVolume (float volume){
        _footstepPlayer.volume = volume;
    }

    public void EmitterVolume (float volume){
        _emitterPlayer.volume = volume;
        _secondaryEmitterPlayer.volume = volume;
    }

    private AudioClip FindSFX(string sfxName, List<SFX> sfxList){
        foreach(SFX sfx in sfxList){
            if(sfx.sfxName == sfxName){
				return sfx.sfxAudio;
            }
        }
		return null;
        
    }

    public string getCurrentRegion(){
        return currentRegion;
    }

    public void setCurrentRegion(string region){
        currentRegion = region;
    }
}
