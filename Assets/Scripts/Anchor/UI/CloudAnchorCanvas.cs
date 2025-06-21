using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CloudAnchorCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cloudAnchorText;

    private void Start()
    {
        GameResources.Instance.notifyResolveText = cloudAnchorText;
    }

}
