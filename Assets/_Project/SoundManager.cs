using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header (" ----------------- Audio Source ----------------- ")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;
    public AudioSource gunshotSource;
    public AudioSource monsterSoundSource;
    public AudioSource otherSoundSource;

    [Header(" ----------------- Audio Clip ----------------- ")]
    public AudioClip backgroundMusicClip;
    public AudioClip monsterSoundClip;
    public AudioClip monsterShootClip;

    public AudioClip handgunSoundClip;
    public AudioClip shotgunSoundClip;

    public AudioClip levelUpSoundClip;
    public AudioClip hurtSoundClip;
    public AudioClip healSoundClip;


    [Header(" ----------------- Random Clip ----------------- ")]
    [SerializeField] private List<AudioClip> sfxClips; // List of sound effects
    private float min = 0f;
    private float max = 20f;


    private void Start()
    {
        if (backgroundMusicClip != null)
        {
            backgroundMusicSource.clip = backgroundMusicClip;
            backgroundMusicSource.Play();
        }
        StartCoroutine(PlayRandomSFX());
    }

    public void PlaySFX(AudioClip clip, AudioSource source)
    {
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    public IEnumerator PlayRandomSFX()
    {
        while (true)
        {
            float randomIndex = Random.Range(min, max);
            yield return new WaitForSeconds(randomIndex);

            if (sfxClips.Count > 0)
            {
                int randomSound = Random.Range(0, sfxClips.Count);
                PlaySFX(sfxClips[randomSound], sfxSource);
            }
        }
    }


}
