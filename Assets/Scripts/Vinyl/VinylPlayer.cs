using DG.Tweening;
using System;
using UnityEngine;

public class VinylPlayer : MonoBehaviour
{
    bool isOpen = false;
    [SerializeField] private Transform lid;
    private Vector3 lidOpenRotation = new Vector3(0, 0, 68);
    private Vector3 lidClosedRotation = new Vector3(0, 0, -44);
    private void Awake()
    {
        StaticEventHandler.OnVinylDiscChanged += OnVinylDiscChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnVinylDiscChanged -= OnVinylDiscChanged;
    }

    private void OnVinylDiscChanged(VinylDisc disc)
    {
        if (!isOpen)
        {
            OpenLidVinylPlayer();
        }

    }

    private async void OpenLidVinylPlayer()
    {
        isOpen = true;
        lid.DOLocalRotate(lidOpenRotation, 2f);
        await Awaitable.WaitForSecondsAsync(2);
    }
}
