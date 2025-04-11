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
            case AnchorType.IntrumentShowCase:
                break;

        }
    }
    void ActiveInstructionScene()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.Instruction);
        LoadSceneWithName(sceneNames[0]);
    }
    void ActiveAnchorScene()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.Anchor);
        LoadSceneWithName(sceneNames[1]);
    }
    public void ActiveCloudAnchorScene()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.CloudAnchor);
        if (currentSceneName == sceneNames[2])
            GameResources.Instance.contentCloudAnchor.SetActive(!GameResources.Instance.contentCloudAnchor.activeSelf);
        LoadSceneWithName(sceneNames[2]);
    }

    void ActiveWallManagerCanvas()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.WallManager);
        LoadSceneWithName(sceneNames[3]);
    }
    void ActiveObjectManagerCanvas()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.ObjectManager);
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
