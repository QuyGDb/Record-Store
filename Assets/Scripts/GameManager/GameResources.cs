using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

    #region SOUNDS

    public AudioMixerGroup musicMasterMixerGroup;
    public AudioMixerSnapshot musicLowSnapshot;
    public AudioMixerSnapshot musicOnFullSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    #endregion



    #region Instrument Showcase
    public InstrumentShowcaseListSO instrumentShowCaseVN;
    #endregion

    #region Physical CD Album
    public AlbumSO Ngot;
    public AlbumSO SDDBP;
    #endregion

    #region Wall
    public WallSO wallSO_ShowcaseVN;
    #endregion

    #region AnchorsManager
    [HideInInspector] public AnchorsManager anchorsManager;
    [HideInInspector] public TextMeshProUGUI anchorSceneText;
    [HideInInspector] public TextMeshProUGUI cloudAnchorSceneText;
    [HideInInspector] public CloudAnchorsManager cloudAnchorsManager;
    [HideInInspector] public SceneNavigationBar sceneNavigationBar;
    [HideInInspector] public WallManager wallManager;
    [HideInInspector] public Image cloudAnchorListScrollViewImage;
    #endregion


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //HelperUtilities.ValidateCheckEmptyString(this, nameof(defaultMaterial), defaultMaterial.name);
        //HelperUtilities.ValidateCheckEmptyString(this, nameof(selectAnchorMAT), selectAnchorMAT.name);
        //HelperUtilities.ValidateCheckNullValue(this, nameof(musicMasterMixerGroup), musicMasterMixerGroup);
        //HelperUtilities.ValidateCheckNullValue(this, nameof(musicLowSnapshot), musicLowSnapshot);
        //HelperUtilities.ValidateCheckNullValue(this, nameof(musicOnFullSnapshot), musicOnFullSnapshot);
        //HelperUtilities.ValidateCheckNullValue(this, nameof(musicOffSnapshot), musicOffSnapshot);

    }
#endif
    #endregion
}