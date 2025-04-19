using UnityEngine;

public class MapNameFileInputField : MonoBehaviour
{
    private TMPro.TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();

    }

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnEndEdit);
    }
    private void OnEndEdit(string text)
    {
        Settings.es3Name = text;
        StaticEventHandler.InvokeNameMapText(text);
    }
}
