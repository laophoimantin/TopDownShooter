using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [Header (" ----------------- Audio Source ----------------- ")]
    [SerializeField] private AudioSource _audioSource;

    
    [Header(" ----------------- Audio Clip ----------------- ")]
    public AudioClip backgroundMusicClip;
    public AudioClip monsterSoundClip;

    public AudioClip hurtSoundClip;
    public AudioClip healSoundClip;


    [Header(" ----------------- Random Clip ----------------- ")]
    [SerializeField] private List<AudioClip> sfxClips; 
    private float min = 0f;
    private float max = 20f;


    private void Start()
    {
        if (backgroundMusicClip != null)
        {
            _audioSource.clip = backgroundMusicClip;
            _audioSource.Play();
        }
        StartCoroutine(PlayRandomSFX());
    }

    
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator PlayRandomSFX()
    {
        while (true)
        {
            float randomIndex = Random.Range(min, max);
            yield return new WaitForSeconds(randomIndex);

            if (sfxClips.Count > 0)
            {
                int randomSound = Random.Range(0, sfxClips.Count);
                PlaySFX(sfxClips[randomSound]);
            }
        }
    }
}
