using DG.Tweening;
using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{

    [SerializeField] Transform doorTranform;
    [SerializeField] Transform mask;
    private Vector3 overturnMark = new Vector3(0, 180, 0);
    private Vector3 openDoor = new Vector3(0, 148f, 0);
    bool isOpen;
    bool wasInFront;
    bool inOtherWorld;
    bool hasCollided;

    private void Awake()
    {
        isOpen = false;
    }
    private void Start()
    {
        SetMaterials(false);
    }
    private void OnDestroy()
    {
        SetMaterials(true);
    }
    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;

        foreach (var mat in GameResources.Instance.liveAidMAT)
        {
            mat.SetInt("_StencilTest", (int)stencilTest);
        }
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != Camera.main.transform)
            return;

        wasInFront = GetIsInFront();
        hasCollided = true;
    }
    void whileCameraColliding()
    {
        if (!hasCollided)
            return;
        bool isInFront = GetIsInFront();
        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);
        }
        wasInFront = isInFront;
    }
    private void Update()
    {
        whileCameraColliding();
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform != Camera.main.transform)
            return;
        hasCollided = false;
    }
    bool GetIsInFront()
    {
        Vector3 worldPos = Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane;
        Vector3 pos = transform.InverseTransformPoint(worldPos);
        return pos.z >= 0 ? true : false;
    }

}
