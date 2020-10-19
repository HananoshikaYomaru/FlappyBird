using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager
{
    public enum Sound
    {
        BirdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }

    private static List<GameObject> gameObjects;

    public static void PlaySound(Sound sound)
    {
        GameObject temp = new GameObject("Sound");
        temp.AddComponent<AudioSource>();
        AudioSource audioSource = temp.GetComponent<AudioSource>();
        AudioClip audioClip = GetAudioClip(sound);
        audioSource.PlayOneShot(audioClip);

        if (gameObjects == null)
            gameObjects = new List<GameObject>();
        gameObjects.Add(temp);
    }

    public static void Clean()
    {
        gameObjects?.RemoveAll(item => item == null);
        for (int i = 0; i < gameObjects?.Count; i++)
        {

            if (!gameObjects[i].GetComponent<AudioSource>().isPlaying)
            {
                //remove
                Object.Destroy(gameObjects[i]);

            }
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.Log("Sound " + sound + " not found ");
        return null;
    }

    public static void AddButtonSound(this Button button)
    {
        button.gameObject.AddComponent<ButtonSound>();
    }

}
