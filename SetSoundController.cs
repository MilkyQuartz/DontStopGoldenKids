using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetSoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Start()
    {
        float savedBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        bgmSlider.value = savedBGMVolume;
        SetBGMVolume(savedBGMVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
        sfxSlider.value = savedSFXVolume;
        SetEffectVolume(savedSFXVolume);

        // 이벤트 리스너 추가
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetEffectVolume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BackMusic", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(BGMVolumeKey, volume);
    }

    public void SetEffectVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }
}
