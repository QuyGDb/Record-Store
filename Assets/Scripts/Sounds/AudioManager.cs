using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    private float normalizedValue = 20f;

    private void Start()
    {
        StartCoroutine(SetVolumeCoroutine());
        sfxSlider.onValueChanged.RemoveAllListeners();

        sfxSlider.onValueChanged.AddListener((value) =>
        {
            // MusicManager.Instance.ChangeSoundsVolume(value * normalizedValue);
            PlayerPrefs.SetFloat("soundsVolume", value * normalizedValue);
            PlayerPrefs.Save();
        });
    }
    private IEnumerator SetVolumeCoroutine()
    {
        yield return null;
        //sfxSlider.value = MusicManager.Instance.soundsVolume / normalizedValue;
    }
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(sfxSlider), sfxSlider);
    }
#endif
    #endregion
}
