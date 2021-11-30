using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    
    [Header("Music Tracks")] 
    [Range(0f,1f)] [SerializeField] private float _maxVolume = .5f;
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private AudioSource[] _tracks;
    
    private List<AudioSource> _availableTracks = new List<AudioSource>();
    private AudioSource _currentTrack;

    [Header("UI Sound Effects")] 
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _invalidClickSound;
    [SerializeField] private AudioSource _upgradeClickSound;

    private bool _tracksMuted = true;
    private bool _muteTracks = false;

    private bool _resetTracks = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        SetupTracks();
        PlayTrack();
    }
    
    private void Update()
    {
        if (_muteTracks && !_tracksMuted)
        {
            // Stores the number of tracks that have already been muted
            int mutedTrackCount = 0;
            
            for (int i = 0; i < _tracks.Length; i++)
            {
                if (_tracks[i].volume > 0f)
                {
                    _tracks[i].volume -= (_maxVolume / _fadeTime) * Time.deltaTime;
                }
                else
                {
                    _tracks[i].volume = 0f;
                    mutedTrackCount++;
                }   
            }

            if (mutedTrackCount == _tracks.Length)
            {
                _tracksMuted = true;

                if (_resetTracks)
                {
                    SetupTracks();
                }
            }
        }
        else if (!_muteTracks && _tracksMuted)
        {
            if (_resetTracks)
            {
                _resetTracks = false;
                SetupTracks();
                PlayTrack();
            }
            
            // Stores the number of tracks that have already been unmuted
            int unmutedTrackCount = 0;
            
            for (int i = 0; i < _tracks.Length; i++)
            {
                if (_tracks[i].volume < _maxVolume)
                {
                    _tracks[i].volume += (_maxVolume / _fadeTime) * Time.deltaTime;
                }
                else
                {
                    _tracks[i].volume = _maxVolume;
                    unmutedTrackCount++;
                }   
            }

            if (unmutedTrackCount == _tracks.Length)
            {
                _tracksMuted = false;
            }
        }
    }
    
    private void SetupTracks()
    {
        _availableTracks.Clear();
        
        for (int i = 0; i < _tracks.Length; i++)
        {
            _availableTracks.Add(_tracks[i]);
        }
    }
    
    private void PlayTrack()
    {
        for (int i = 0; i < _tracks.Length; i++)
        {
            _tracks[i].Stop();
        }
        
        int randomSongIndex;

        if (_availableTracks.Count > 0)
        {
            randomSongIndex = Random.Range(0, _availableTracks.Count);


            _availableTracks[randomSongIndex].Play();

            _currentTrack = _availableTracks[randomSongIndex];

            _availableTracks.Remove(_availableTracks[randomSongIndex]);
            
            InvokeRepeating("WaitForNextTrack",0f,1f);
        }
        else
        {
            SetupTracks();
            PlayTrack();
        }
    }
    
    private void WaitForNextTrack()
    {
        if (!_currentTrack.isPlaying)
        {
            CancelInvoke("WaitForNextTrack");
            PlayTrack();
        }
    }
    
    public void ToggleMusic(bool mute, bool reset)
    {
        if (mute)
        {
            _muteTracks = true;
        }
        else
        {
            _muteTracks = false;
        }

        if (reset)
            _resetTracks = true;
    }
    
    /* Sound Effect Functions */
    public void PlayClickSound()
    {
        _clickSound.Play();
    }
    
    public void PlayInvalidClickSound()
    {
        _invalidClickSound.Play();
    }
    
    public void PlayUpgradeClickSound()
    {
        _upgradeClickSound.Play();
    }
    
}
