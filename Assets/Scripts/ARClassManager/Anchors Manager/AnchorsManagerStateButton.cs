using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnchorsManagerStateButton : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI text;
    private AnchorsManagerState anchorsManagerState;
    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(ChangeAnchorsManagerState);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(ChangeAnchorsManagerState);
    }

    private void ChangeAnchorsManagerState()
    {
        if (anchorsManagerState == AnchorsManagerState.Creating)
        {
            anchorsManagerState = AnchorsManagerState.Deleting;
            text.text = "Deleting";
            StaticEventHandler.InvokeAnchorsManagerStateChanged(AnchorsManagerState.Deleting);

        }
        else
        {
            anchorsManagerState = AnchorsManagerState.Creating;
            text.text = "Creating";
            StaticEventHandler.InvokeAnchorsManagerStateChanged(AnchorsManagerState.Creating);

        }
    }
}
