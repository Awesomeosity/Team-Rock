using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace TeamRock.Managers
{
    public class SoundManager
    {
        private HashSet<SoundEffect> _soundEffects;
        private List<SoundData> _soundData;
        private static int _currentSoundCounter;

        #region Initialize

        public void Initialize()
        {
            _soundEffects = new HashSet<SoundEffect>();
            _soundData = new List<SoundData>();

            _currentSoundCounter = 0;
        }

        #endregion

        #region External Functions

        #region Play Sound

        public int PlaySound(SoundEffect soundEffect)
        {
            if (!_soundEffects.Contains(soundEffect))
            {
                _soundEffects.Add(soundEffect);
            }

            SoundData soundData = GetEmptySoundEffect(soundEffect);
            soundData.SoundEffectInstance.Play();

            return soundData.SoundIndex;
        }

        public void PlaySound(int soundIndex)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Play();
        }

        public void ResumeSound(int soundIndex)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Resume();
        }

        #endregion

        #region Pause And Stop

        public void PauseSound(int soundIndex)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Pause();
        }

        public void StopSound(int soundIndex)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Stop();
        }

        public int StopSound(SoundEffect soundEffect)
        {
            if (!_soundEffects.Contains(soundEffect))
            {
                _soundEffects.Add(soundEffect);
            }

            SoundData soundData = GetEmptySoundEffect(soundEffect);
            soundData.SoundEffectInstance.Play();

            return soundData.SoundIndex;
        }

        #endregion

        #region Looping Tracks

        public void SetSoundLooping(int soundIndex, bool looping = true)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.IsLooped = looping;
        }

        #endregion

        #region Volume and Pitch

        public void SetSoundVolume(int soundIndex, float volume = 1)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Volume = volume;
        }

        public void SetSoundPitch(int soundIndex, float pitch = 0)
        {
            SoundData soundData = GetEffectByIndex(soundIndex);
            if (soundData == null)
            {
                Console.WriteLine("Invalid Sound Requested");
                return;
            }

            soundData.SoundEffectInstance.Pitch = pitch;
        }

        #endregion

        #endregion

        #region Utility Functions

        private SoundData GetEmptySoundEffect(SoundEffect soundEffect)
        {
            SoundData soundData = _soundData.FirstOrDefault(_ =>
                _.OriginalSoundEffect == soundEffect && _.SoundEffectInstance.State == SoundState.Stopped);

            if (soundData == null)
            {
                Console.WriteLine("No Existing Sound Found. Creating new instance");
                _currentSoundCounter += 1;
                soundData = new SoundData()
                {
                    SoundEffectInstance = soundEffect.CreateInstance(),
                    OriginalSoundEffect = soundEffect,
                    SoundIndex = _currentSoundCounter
                };

                _soundData.Add(soundData);
            }

            return soundData;
        }

        private SoundData GetEffectByIndex(int soundIndex) =>
            _soundData.FirstOrDefault(_ => _.SoundIndex == soundIndex);

        #endregion

        #region Singleton

        private static SoundManager _instance;
        public static SoundManager Instance => _instance ?? (_instance = new SoundManager());

        private SoundManager()
        {
        }

        #endregion

        #region SoundData

        private class SoundData
        {
            public int SoundIndex { get; set; }
            public SoundEffect OriginalSoundEffect { get; set; }
            public SoundEffectInstance SoundEffectInstance { get; set; }
        }

        #endregion
    }
}