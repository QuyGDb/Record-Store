using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WallManager : MonoBehaviour
{
    [HideInInspector] public WallSO wallSO;
    private Transform wallTransform;
    private Rigidbody wallRigidbody;
    private XRGrabInteractable wallInteractable;
    private void Awake()
    {
        wallRigidbody = GetComponent<Rigidbody>();
        wallInteractable = GetComponent<XRGrabInteractable>();
    }
    private void Start()
    {
        wallInteractable.selectEntered.AddListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        wallTransform = ES3.Load(wallSO.name, wallTransform);
        if (wallTransform != null)
        {
            LoadTransfrom();
        }
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
        wallInteractable.selectEntered.RemoveListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.WallManager)
        {
            wallRigidbody.constraints = RigidbodyConstraints.None;
        }
        if (state == ApplicationState.ObjectManager)
        {

            wallRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        if (wallTransform != null)
            ES3.Save(wallSO.name, transform);

    }
    private async void LoadTransfrom()
    {
        await Awaitable.NextFrameAsync();
        transform.localPosition = wallTransform.localPosition;
        transform.localRotation = wallTransform.localRotation;
        transform.localScale = wallTransform.localScale;

    }
    public void StretchPlaneFromEdge(PlaneEdge edge, float delta)
    {
        Vector3 scale = transform.localScale;
        Vector3 positionOffset = Vector3.zero;
        const float minScale = 0.01f;

        switch (edge)
        {
            case PlaneEdge.Left:
                if (scale.x + delta >= minScale)
                {
                    scale.x += delta;
                    positionOffset -= transform.right * (delta / 2f);
                }
                break;

            case PlaneEdge.Right:
                if (scale.x + delta >= minScale)
                {
                    scale.x += delta;
                    positionOffset += transform.right * (delta / 2f);
                }
                break;

            case PlaneEdge.Top:
                if (scale.y + delta >= minScale)
                {
                    scale.y += delta;
                    positionOffset += transform.up * (delta / 2f);
                }
                break;

            case PlaneEdge.Bottom:
                if (scale.y + delta >= minScale)
                {
                    scale.y += delta;
                    positionOffset -= transform.up * (delta / 2f);
                }
                break;
        }

        transform.localScale = scale;
        transform.localPosition += positionOffset;
    }


    private void OnApplicationQuit()
    {
        if (wallTransform != null)
            ES3.Save(wallSO.name, transform);
    }

}
