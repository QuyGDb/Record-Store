using TMPro;
using UnityEngine;

public class NotifyCloudAnchorText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameResources.Instance.notifyResolveText = textMeshPro;
    }
}
