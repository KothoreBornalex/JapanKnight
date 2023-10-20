using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool _startMusic;
    [Header("Volume")]
    [Range(0f, 1)] public float masterVolume = 1;
    [Range(0f, 1)] public float musicVolume = 0.6f;
    [Range(0f, 1)] public float ambienceVolume = 1;
    [Range(0f, 1)] public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;


    public static AudioManager instance;
    public List<EventInstance> eventInstances = new List<EventInstance>();

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        if (_startMusic)
        {
            //InitializeAmbience(FMODEvents.instance.ambienceMainMenu);
            InitializeMusic(FMODEvents.instance.musicMainMenu);
        }
        
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }


    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetMusic(MusicsEnum music)
    {
        musicEventInstance.setParameterByName("Musics", (float)music);
    }

    private EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }


    public void PlayOneShot_LocatedSound(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public void PlayOneShot_GlobalSound(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }
}
