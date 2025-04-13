using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstrumentUI : MonoBehaviour
{
    private Button[] buttons;
    [SerializeField] private TextMeshProUGUI instrumentName;
    [SerializeField] private TextMeshProUGUI videoTitle;
    [SerializeField] private TextMeshProUGUI information;
    [SerializeField] private Transform videoUI;

    private Canvas instrumentUICanvas;
    private InstrumentVideoHandler videoHandler;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        instrumentUICanvas = GetComponent<Canvas>();
        videoHandler = GetComponentInChildren<InstrumentVideoHandler>();
    }

    public void SetData(InstrumentSO instrumentSO, Vector3 postion)
    {
        instrumentName.text = instrumentSO.instrumentName;
        videoTitle.text = instrumentSO.videoClipTitle;
        information.text = instrumentSO.description;
        gameObject.SetActive(true);
        videoHandler.PlayVideo(instrumentSO.videoClip);
        transform.position = postion;
    }
    private void Start()
    {
        instrumentUICanvas.worldCamera = Camera.main;
        buttons[0].onClick.AddListener(ToggleVideoPanel);
        buttons[1].onClick.AddListener(ToggleInformationText);
        buttons[2].onClick.AddListener(ToggleInstrumentUI);
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(ToggleVideoPanel);
        buttons[1].onClick.RemoveListener(ToggleInformationText);
        buttons[2].onClick.RemoveListener(ToggleInstrumentUI);
    }
    void ToggleVideoPanel()
    {
        information.gameObject.SetActive(false);
        videoUI.gameObject.SetActive(true);
    }
    void ToggleInformationText()
    {
        information.gameObject.SetActive(true);
        videoUI.gameObject.SetActive(false);
    }

    void ToggleInstrumentUI()
    {
        gameObject.SetActive(false);
        ToggleVideoPanel();
    }
}
