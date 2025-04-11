using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentUI : MonoBehaviour
{
    private Button[] buttons;
    private RawImage videoRawImage;
    [SerializeField] private TextMeshProUGUI instrumentName;
    [SerializeField] private TextMeshProUGUI videoTitle;
    [SerializeField] private TextMeshProUGUI information;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        videoRawImage = GetComponentInChildren<RawImage>();
    }
}
