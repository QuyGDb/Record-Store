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
    public InstrumentShowcaseListSO instrumentShowCaseVN;
    #endregion

    #region Physical CD Album
    public AlbumSO Ngot;
    public AlbumSO SDDBP;
    #endregion

    #region Wall
    [Header("Wall")]
    public WallSO wallSO_Showcase;
    public WallSO wall_HistoryMusic;


    #endregion


    #region History Music
    [Header("History Music")]
    public MusicGenreSO rap;
    public MusicGenreSO pop;
    public MusicGenreSO rock;

    #endregion
    #region AnchorsManager
    [HideInInspector] public AnchorsManager anchorsManager;
    [HideInInspector] public TextMeshProUGUI anchorSceneText;
    [HideInInspector] public TextMeshProUGUI cloudAnchorSceneText;
    [HideInInspector] public CloudAnchorsManager cloudAnchorsManager;
    [HideInInspector] public SceneNavigationBar sceneNavigationBar;
    [HideInInspector] public WallManager wallManager;
    [HideInInspector] public GameObject contentCloudAnchor;
    [HideInInspector] public ObjectSpawnerAtAnchors objectSpawnerAtAnchors;
    [HideInInspector] public TransformObjectsManager transformObjectsManager;
    [HideInInspector] public AROcclusionManager occlusionManager;
    [HideInInspector] public List<string> resolveCloudAnchorIdList;
    #endregion

    [HideInInspector] public TextMeshProUGUI objectSceneText;

    // Vinyl ShowCase
    [Header("Vinyl ShowCase")]
    public GameObject VinylShowCasePrefab;

    // Portal

    [Header("Portal")]
    public GameObject PortalPrefab;
    #region Validation

    public InputActionReference touchRef;
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(defaultMaterial), defaultMaterial.name);

    }
#endif
    #endregion

}