using System;
using UnityEngine;
using UnityEngine.UI;

public class UploadMapButton : MonoBehaviour
{
    private Button updateButton;

    private void Awake()
    {
        updateButton = GetComponent<Button>();
        updateButton.onClick.AddListener(OnUpdateButtonClick);
    }
    private void OnDestroy()
    {
        updateButton.onClick.RemoveListener(OnUpdateButtonClick);
    }
    private void OnUpdateButtonClick()
    {
        SupabaseStorage.Instance.UploadSaveFile().ContinueWith(task =>
         {
             if (task.IsFaulted)
             {
                 Debug.LogError("Upload failed: " + task.Exception);
             }
             else
             {
                 Debug.Log("Upload completed successfully.");
             }
         });
    }
}
