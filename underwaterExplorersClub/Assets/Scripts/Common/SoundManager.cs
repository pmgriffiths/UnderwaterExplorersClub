using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace PodTheDog.Common
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
        public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.

        public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

        public static SoundManager instance = null;

        public AudioClip cameraSound;

        public AudioClip bubblesSound;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            Debug.Log("efxSource is enabled: " + efxEnabled);
            Debug.Log("Music is enabled: " + musicEnabled);
        }
        
        public bool efxEnabled = true;

        public bool musicEnabled = false;

        public void EnableEfx(bool enabled)
        {
            Debug.Log("Enable EFX: " + enabled);
            efxEnabled = enabled;
        }

        public void EnableMusic(bool enabled)
        {
            musicEnabled = enabled;
        }

        private void PlayEfxClip(AudioClip clip)
        {
            if (efxEnabled)
            {
                //Set the clip of our efxSource audio source to the clip passed in as a parameter.
                efxSource.clip = clip;
    
                //Play the clip.
                efxSource.Play();
            }
        }


        //Used to play single sound clips.
        public void PlayCamera()
        {
            PlayEfxClip(cameraSound);
        }

        //Used to play single sound clips.
        public void PlayBubbles()
        {
            PlayEfxClip(bubblesSound);
        }

    }
}
