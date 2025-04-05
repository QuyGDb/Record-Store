using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    Button[] buttons = new Button[5];

    Canvas[] canvas = new Canvas[5];
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true);

    }
    void ActiveInstructionCanvas()
    {
        DeactivateAllCanvas();
        canvas[0].enabled = true;
    }
    void ActiveAnchorAnchorCanvas()
    {
        DeactivateAllCanvas();
        canvas[1].enabled = true;
    }
    private void DeactivateAllCanvas()
    {
        foreach (var item in canvas)
        {
            item.enabled = false;
        }
    }
}
