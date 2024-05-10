using UnityEngine;
using UnityEngine.Audio;
using Saves;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixerGroup mixer;

        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private AudioSource gameMusic;

        [SerializeField] private AudioSource buttonClick;
        [SerializeField] private AudioSource winGame;
        [SerializeField] private AudioSource loseGame;
        [SerializeField] private AudioSource scrollSlot;
        [SerializeField] private AudioSource loseSlot;
        [SerializeField] private AudioSource winSlot;
        [SerializeField] private AudioSource putCard;
        [SerializeField] private AudioSource errorCard;
        [SerializeField] private AudioSource claimReward;

        private string _soundKey = "_Sound_Key";
        private string _musicKey = "_Music_Key";

        protected override void Awake()
        {
            base.Awake();

            if (!PlayerPrefs.HasKey(_soundKey))
            {
                GameSaves.Instance.WriteData(_soundKey, true);
            }

            if (!PlayerPrefs.HasKey(_musicKey))
            {
                GameSaves.Instance.WriteData(_musicKey, true);
            }
        }

        private void Start()
        {
            SetMusic(GetActivityMusic());
            SetSound(GetActivitySound());
        }

        public void PutCardSound() => putCard.Play();

        public void ErrorPutCardSound() => errorCard.Play();

        public void ButtonClickSound() => buttonClick.Play();

        public void WinSlotSound() => winSlot.Play();

        public void LoseSlotSound() => loseSlot.Play();

        public void WinGameSound() => winGame.Play();

        public void LoseGameSound() => loseGame.Play();

        public void ScrollSlotSound() => scrollSlot.Play();

        public void ClaimRewardSound() => claimReward.Play();

        public void GameMusicPlay(bool playMusic)
        {
            if (playMusic)
                gameMusic.Play();
            else
                gameMusic.Pause();
        }

        public void BackgroundMusicPlay(bool playMusic)
        {
            if (playMusic)
            {
                if (!backgroundMusic.isPlaying)
                {
                    backgroundMusic.Play();
                }
            }
            else
            {
                backgroundMusic.Pause();
            }
        }

        public void SetSound(bool enabled)
        {
            if (enabled)
            {
                mixer.audioMixer.SetFloat("SoundVolume", 0);
            }
            else
            {
                mixer.audioMixer.SetFloat("SoundVolume", -80);
            }

            GameSaves.Instance.WriteData(_soundKey, enabled);
        }

        public void SetMusic(bool enabled)
        {
            if (enabled)
            {
                mixer.audioMixer.SetFloat("MusicVolume", -12);
            }
            else
            {
                mixer.audioMixer.SetFloat("MusicVolume", -80);
            }

            GameSaves.Instance.WriteData(_musicKey, enabled);
        }

        public bool GetActivitySound() => GameSaves.Instance.ReadData<bool>(_soundKey);
        public bool GetActivityMusic() => GameSaves.Instance.ReadData<bool>(_musicKey);
    }
}