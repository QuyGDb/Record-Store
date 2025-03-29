using TMPro;
using UnityEngine;

public class ShowTranform : MonoBehaviour
{
    private TextMeshProUGUI TextMeshProUGUI;

    private void Awake()
    {
        TextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        TextMeshProUGUI.text = @$"Position: {transform.position} + rotate: {Quaternion.ToEulerAngles(transform.rotation)}";

    }
}
