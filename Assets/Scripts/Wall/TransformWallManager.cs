using UnityEngine;

public class TransformWallManager : MonoBehaviour
{
    [HideInInspector] public WallSO wallSO;
    private Transform wallTransform;
    private void Start()
    {

        wallTransform = ES3.Load(wallSO.name, wallTransform);
        if (wallTransform != null)
        {
            LoadTransfrom();
        }
    }

    private async void LoadTransfrom()
    {
        await Awaitable.NextFrameAsync();
        wallTransform.position = wallTransform.localPosition;
        wallTransform.rotation = wallTransform.localRotation;
        wallTransform.localScale = wallTransform.localScale;

    }
    public void StretchPlaneFromEdge(PlaneEdge edge, float delta)
    {
        Vector3 scale = transform.localScale;
        Vector3 positionOffset = Vector3.zero;

        switch (edge)
        {
            case PlaneEdge.Left:
                scale.x += delta;
                positionOffset += transform.right * (delta / 2f);
                break;

            case PlaneEdge.Right:
                scale.x += delta;
                positionOffset -= transform.right * (delta / 2f);
                break;

            case PlaneEdge.Top:
                scale.z += delta;
                positionOffset -= transform.forward * (delta / 2f);
                break;

            case PlaneEdge.Bottom:
                scale.z += delta;
                positionOffset += transform.forward * (delta / 2f);
                break;
        }

        transform.localScale = scale;
        transform.position += positionOffset;
        ES3.Save(wallSO.name, transform);
    }

}
