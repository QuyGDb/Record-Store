using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

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

    [Header("Material")]
    public Material defaultMaterial;
    public Material[] liveAidMAT;




    #region Instrument Showcase
    public InstrumentShowcaseListSO instrumentShowCase;
    #endregion

    #region Physical CD Album
    public AlbumSO Ngot;
    public AlbumSO SDDBP;
    #endregion




    #region History Music
    [Header("History Music")]
    public MusicGenreSO rap;
    public MusicGenreSO pop;
    public MusicGenreSO rock;

    #endregion

    #region Pool Reference
    [HideInInspector] public AnchorsManager anchorsManager;
    [HideInInspector] public CloudAnchorsManager cloudAnchorsManager;
    [HideInInspector] public TextMeshProUGUI anchorSceneText;
    [HideInInspector] public TextMeshProUGUI notifyResolveText;
    [HideInInspector] public CreateMapSceneNavigationBar sceneNavigationBar;
    [HideInInspector] public WallManager currentwallManager;
    [HideInInspector] public GameObject contentCloudAnchor;
    [HideInInspector] public TransformObjectsManager transformObjectsManager;
    [HideInInspector] public AROcclusionManager occlusionManager;
    [HideInInspector] public List<string> resolveCloudAnchorIdList;
    [HideInInspector] public TextMeshProUGUI objectSceneText;

    [HideInInspector] public InstrumentShowcaseManager instrumentShowcaseManager;
    [HideInInspector] public PictureFrameManager pictureFrameManager;
    #endregion
    // Vinyl ShowCase
    [Header("Vinyl ShowCase")]
    public GameObject VinylShowCasePrefab;

    // Portal

    [Header("Portal")]
    public GameObject PortalPrefab;
    #region Validation
    // Wall
    [Header("Wall")]
    public GameObject wallPrefab;

    public InputActionReference touchRef;
    public InputActionReference pinchGapDeltaRef;
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(defaultMaterial), defaultMaterial.name);

    }
#endif
    #endregion

}