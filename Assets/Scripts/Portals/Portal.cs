using DG.Tweening;
using System;
using Unity.Jobs;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] Transform doorTranform;
    [SerializeField] Transform mask;
    private Vector3 overturnMark = new Vector3(0, 180, 0);
    private Vector3 openDoor = new Vector3(0, 148f, 0);
    bool isInsidePortal;
    bool isOpen;
    private void Awake()
    {
        isOpen = false;
        isInsidePortal = false;
    }
    public void SetDoorState()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void CloseDoor()
    {
        isOpen = false;
        doorTranform.DOLocalRotate(Vector3.zero, 2f);
        mask.localRotation = Quaternion.Euler(overturnMark);
    }

    private void OpenDoor()
    {
        isOpen = true;
        doorTranform.DOLocalRotate(openDoor, 2f).SetEase(Ease.OutBack);
        mask.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            var liveAid = transform.parent.GetComponentInChildren<LiveAid>();
            if (isInsidePortal)
            {
                isInsidePortal = false;
                liveAid.SwapLayer(liveAid.insideMaskLayer);

            }
            else
            {
                isInsidePortal = true;
                liveAid.SwapLayer(liveAid.defaultLayer);
            }

        }
    }
}
