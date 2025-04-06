using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigationBar : MonoBehaviour
{
    Button[] buttons = new Button[5];
    private string currentSceneName;
    private string[] sceneNames = new string[5] { "InstructionScene", "AnchorScene", "CloudAnchorScene", "WallManagerScene", "ObjectManagerScene" };
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true);
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;
        currentSceneName = sceneNames[0];
    }

    private void Start()
    {
        buttons[0].onClick.AddListener(ActiveInstructionScene);
        buttons[1].onClick.AddListener(ActiveAnchorScene);
        buttons[2].onClick.AddListener(ActiveCloudAnchorScene);
        buttons[3].onClick.AddListener(ActiveWallManagerCanvas);
        buttons[4].onClick.AddListener(ActiveObjectManagerCanvas);
        ActiveInstructionScene();
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(ActiveInstructionScene);
        buttons[1].onClick.RemoveListener(ActiveAnchorScene);
        buttons[2].onClick.RemoveListener(ActiveCloudAnchorScene);
        buttons[3].onClick.RemoveListener(ActiveWallManagerCanvas);
        buttons[4].onClick.RemoveListener(ActiveObjectManagerCanvas);
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor anchor, AnchorType type)
    {
        switch (type)
        {
            case AnchorType.IntrumentShowCaseVN:
                break;

        }
    }
    void ActiveInstructionScene()
    {
        LoadSceneWithName(sceneNames[0]);

    }
    void ActiveAnchorScene()
    {
        //if (currentSceneName != sceneNames[1] || currentSceneName != sceneNames[0])
        //    if (SceneManager.GetSceneByName(ARObjectScene).isLoaded)
        //        SceneManager.UnloadSceneAsync(ARObjectScene);
        LoadSceneWithName(sceneNames[1]);
    }
    void ActiveCloudAnchorScene()
    {
        //if (currentSceneName == sceneNames[1] || currentSceneName == sceneNames[0])
        //    if (SceneManager.GetSceneByName(ARObjectScene).isLoaded)
        //        SceneManager.UnloadSceneAsync(ARObjectScene);
        LoadSceneWithName(sceneNames[2]);
    }

    void ActiveWallManagerCanvas()
    {
        LoadSceneWithName(sceneNames[3]);
    }
    void ActiveObjectManagerCanvas()
    {
        LoadSceneWithName(sceneNames[4]);
    }

    void LoadSceneWithName(string name)
    {
        if (currentSceneName == name)
            return;

        SceneManager.UnloadSceneAsync(currentSceneName);

        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        currentSceneName = name;
    }



}
