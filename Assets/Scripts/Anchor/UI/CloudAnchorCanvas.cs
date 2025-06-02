using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CloudAnchorCanvas : MonoBehaviour
{
    private Button[] buttons;
    [SerializeField] private TextMeshProUGUI cloudAnchorText;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();

    }
    private void Start()
    {
        GameResources.Instance.notifyResolveText = cloudAnchorText;
        AddEventForButton();
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
    }
    private void AddEventForButton()
    {
        buttons[0].onClick.AddListener(GameResources.Instance.cloudAnchorsManager.ResolveSelectedCloudAnchor);
        buttons[1].onClick.AddListener(GameResources.Instance.cloudAnchorsManager.RemoveCloudAnchorInAnchorDetails);

    }
}
