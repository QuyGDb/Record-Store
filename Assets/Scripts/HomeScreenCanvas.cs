using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenCanvas : MonoBehaviour
{

    // Client
    [SerializeField] private Canvas mapCanvas;
    [SerializeField] private Image vinylIcon;
    public TextMeshProUGUI uiText;

    [SerializeField] private GameObject albumSpawer;
    public Material textMat;
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public float speed = 0.5f;

    private float basePower = 0f;
    private float amplitude = 1f;


    private void Update()
    {
        if (vinylIcon != null)
        {
            vinylIcon.transform.Rotate(0f, 0f, 50f * Time.deltaTime);
        }

        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        uiText.color = Color.Lerp(colorA, colorB, t);

        float glowPower = basePower + Mathf.PingPong(Time.time * speed, amplitude);
        textMat.SetFloat("_GlowPower", glowPower);
    }

    public void ClientButton()
    {

        mapCanvas.enabled = true;
        gameObject.SetActive(false);
        albumSpawer.gameObject.SetActive(false);
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.Client);
    }
    public void AdminButton()
    {
        gameObject.SetActive(false);
        albumSpawer.gameObject.SetActive(false);
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.Admin);
    }
}
