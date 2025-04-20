using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMapNagivationBar : MonoBehaviour
{
    public string[] sceneNames = new string[3] { "ListMapScene", "CloudAnchorSceneLoadMap", "ViewScene" };

    private Button[] buttons = new Button[3];

    private string currentSceneName;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true);
        currentSceneName = sceneNames[0];
    }
    private void Start()
    {
        buttons[0].onClick.AddListener(ActiveListMapScene);
        buttons[1].onClick.AddListener(ActiveCloudAnchorScene);
        buttons[2].onClick.AddListener(ActiveViewScene);
        ActiveListMapScene();
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(ActiveListMapScene);
        buttons[1].onClick.RemoveListener(ActiveCloudAnchorScene);
        buttons[2].onClick.RemoveListener(ActiveViewScene);
    }
    void ActiveListMapScene()
    {
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.ListMap);
        LoadSceneWithName(sceneNames[0]);
    }
    void ActiveCloudAnchorScene()
    {
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.CloudAnchorInLoadMap);
        LoadSceneWithName(sceneNames[1]);
    }
    void ActiveViewScene()
    {
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.View);
        LoadSceneWithName(sceneNames[2]);
    }
    void LoadSceneWithName(string sceneName)
    {
        if (currentSceneName != sceneName)
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            currentSceneName = sceneName;
        }
    }
}
