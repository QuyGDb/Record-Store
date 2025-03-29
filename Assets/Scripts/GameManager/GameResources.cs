using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    public Material defaultMaterial;
    public Material selectAnchorMAT;

    #region SOUNDS

    public AudioMixerGroup musicMasterMixerGroup;
    public AudioMixerSnapshot musicLowSnapshot;
    public AudioMixerSnapshot musicOnFullSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;


    #endregion
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(defaultMaterial), defaultMaterial.name);
        HelperUtilities.ValidateCheckEmptyString(this, nameof(selectAnchorMAT), selectAnchorMAT.name);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicMasterMixerGroup), musicMasterMixerGroup);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicLowSnapshot), musicLowSnapshot);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicOnFullSnapshot), musicOnFullSnapshot);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicOffSnapshot), musicOffSnapshot);

    }
#endif
    #endregion
}