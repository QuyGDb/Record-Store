using Google.XR.ARCoreExtensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    Button[] buttons = new Button[5];
    Canvas[] canvas = new Canvas[6];
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>(true);
        canvas = transform.parent.GetComponentsInChildren<Canvas>(true);
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;
    }

    private void Start()
    {
        buttons[0].onClick.AddListener(ActiveInstructionCanvas);
        buttons[1].onClick.AddListener(ActiveAnchorrCanvas);
        buttons[2].onClick.AddListener(ActiveAnchorCloudCanvas);
        buttons[3].onClick.AddListener(ActiveWallManagerCanvas);
        buttons[4].onClick.AddListener(ActiveObjectManagerCanvas);
        ActiveInstructionCanvas();
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(ActiveInstructionCanvas);
        buttons[1].onClick.RemoveListener(ActiveAnchorrCanvas);
        buttons[2].onClick.RemoveListener(ActiveAnchorCloudCanvas);
        buttons[3].onClick.RemoveListener(ActiveWallManagerCanvas);
        buttons[4].onClick.RemoveListener(ActiveObjectManagerCanvas);
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor anchor, AnchorType type)
    {
        switch (type)
        {
            case AnchorType.IntrumentShowCaseVN:
                ActiveWallManagerCanvas();
                break;


        }
    }

    void ActiveInstructionCanvas()
    {
        DeactivateAllCanvas();
        canvas[1].enabled = true;
    }
    void ActiveAnchorrCanvas()
    {
        DeactivateAllCanvas();
        canvas[2].enabled = true;
    }
    void ActiveAnchorCloudCanvas()
    {
        DeactivateAllCanvas();
        canvas[3].enabled = true;
    }

    void ActiveWallManagerCanvas()
    {
        DeactivateAllCanvas();
        canvas[4].enabled = true;
    }

    void ActiveObjectManagerCanvas()
    {
        DeactivateAllCanvas();
        canvas[5].enabled = true;
    }
    private void DeactivateAllCanvas()
    {
        for (int i = 1; i < canvas.Length; i++)
        {
            canvas[i].enabled = false;
        }
    }
}
