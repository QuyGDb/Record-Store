using UnityEngine;
using UnityEditor;
using System.IO;

public class BatchPrefabScreenshot : EditorWindow
{
    private Camera screenshotCamera;
    private string prefabFolder = "Assets/Prefabs";
    private string saveFolder = "Assets/Screenshots";
    private int imageWidth = 512;
    private int imageHeight = 512;

    [MenuItem("Tools/Batch Prefab Screenshot")]
    public static void ShowWindow()
    {
        GetWindow<BatchPrefabScreenshot>("Batch Prefab Screenshot");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Prefab Screenshot", EditorStyles.boldLabel);

        screenshotCamera = (Camera)EditorGUILayout.ObjectField("Screenshot Camera", screenshotCamera, typeof(Camera), true);
        prefabFolder = EditorGUILayout.TextField("Prefab Folder", prefabFolder);
        saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);
        imageWidth = EditorGUILayout.IntField("Image Width", imageWidth);
        imageHeight = EditorGUILayout.IntField("Image Height", imageHeight);

        if (GUILayout.Button("Capture Screenshots"))
        {
            CaptureAllPrefabs();
        }
    }

    private void CaptureAllPrefabs()
    {
        if (!screenshotCamera)
        {
            Debug.LogError("Ch?a gán Camera ch?p ?nh!");
            return;
        }

        string[] prefabPaths = Directory.GetFiles(prefabFolder, "*.prefab", SearchOption.AllDirectories);
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab)
            {
                GameObject instance = Instantiate(prefab);
                instance.transform.position = Vector3.zero;

                string fileName = Path.Combine(saveFolder, prefab.name + ".png");
                CaptureScreenshot(fileName);

                DestroyImmediate(instance);
            }
        }

        AssetDatabase.Refresh();
    }

    private void CaptureScreenshot(string filePath)
    {
        RenderTexture rt = new RenderTexture(imageWidth, imageHeight, 24);
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();

        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenshot.Apply();

        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        DestroyImmediate(screenshot);
    }
}
