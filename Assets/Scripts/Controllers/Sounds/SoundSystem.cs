using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AirCoder.Core;
using AirCoder.Helper;
using NaughtyAttributes;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Sounds
{
    public enum MusicThemes
    {
        MainMenu, Mission
    }
    
    public class SoundSystem : GameSystem //2D Sound System
    {
        private readonly AudioSource _musicSource;
        private readonly SoundSystemConfig _config;
        
        public SoundSystem(GameController inController, Application inApp, SystemConfig inConfig = null) : base(inController, inApp, inConfig)
        {
            _config = inConfig as SoundSystemConfig;
            if(_config == null) throw new Exception($"System config must be not null.");
            
            _musicSource = SystemFacade.Application.gameObject.AddComponent<AudioSource>();
            foreach (var s in _config.sfxMap)
            {
                s.source = SystemFacade.Application.gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.hasOwnVolume? s.volume : _config.sfxVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        
        private Sound GetSfx(SoundList inSound)
        {
            return _config.sfxMap.FirstOrDefault(s => s.label == inSound);
        }

        private AudioClip GetMusic(MusicThemes inTheme)
        {
            return _config.musicMap.FirstOrDefault(m => m.theme == inTheme).audio;
        }
        
        public void PlaySfx(SoundList inSfxLabel)
        {
            if(inSfxLabel == SoundList.None) return;
            var sound = GetSfx(inSfxLabel);
            if (sound == null)
            {
                throw new WarningException($"SFX audio not found : {inSfxLabel}");
            }
            sound.source.volume = (sound.hasOwnVolume ? sound.volume : _config.sfxVolume) * _config.masterVolume;
            sound.source.Play();
        }

        public void PlayMusic(MusicThemes inTheme)
        {
            var music = GetMusic(inTheme);
            if (music == null)
            {
                throw new WarningException($"Music Theme not found : {inTheme}");
            }

            _musicSource.clip = music;
            _musicSource.volume = _config.musicVolume * _config.masterVolume;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            if(!_musicSource.isPlaying) return;
            _musicSource.Stop();
        }

        
    }
}