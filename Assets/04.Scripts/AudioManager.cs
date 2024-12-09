using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [Header("음량")]
    public float volumeBGM = 1f;
    public float volumeEffect = 1f;

    [Header("배경음악")]
    public AudioSource asBGM;
    public AudioClip BGMMain;

    [Header("효과음")]
    public AudioSource asEffect;
    public AudioClip btn;
    public AudioClip cam;
    public AudioClip treasure;
    public AudioClip dance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        asBGM.loop = true;
        asBGM.playOnAwake = true;
        PlayBGM("main");
        asEffect.loop = false;
        asEffect.playOnAwake = false;
    }
    private void PlayBGM(string bgm)
    {
        switch (bgm)
        {
            case "main":
                asBGM.clip = BGMMain;
                break;
        }
        asBGM.Play();
    }
    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "btn":
                asEffect.clip = btn;
                break;
            case "cam":
                asEffect.clip = cam;
                break;
            case "treasure":
                asEffect.clip = treasure;
                break;
            case "dance":
                asEffect.clip = dance;
                break;

        }
        asEffect.PlayOneShot(asEffect.clip);
    }
}