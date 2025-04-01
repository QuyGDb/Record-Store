using TMPro;
using UnityEngine;

public class ShowTranform : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        textMeshProUGUI.text = @$"Position: {transform.position} + rotate: {Quaternion.ToEulerAngles(transform.rotation)}";
    }
}
