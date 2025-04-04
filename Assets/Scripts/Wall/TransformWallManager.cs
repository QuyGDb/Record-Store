using UnityEngine;

public class TransformWallManager : MonoBehaviour
{
    public enum PlaneEdge { Left, Right, Top, Bottom }

    public void StretchPlaneFromEdge(Transform plane, PlaneEdge edge, float delta)
    {
        Vector3 scale = plane.localScale;
        Vector3 positionOffset = Vector3.zero;

        float unit = 10f; // vì Unity Plane m?c ??nh là 10x10

        switch (edge)
        {
            case PlaneEdge.Left:
                scale.x += delta / unit;
                positionOffset += plane.right * (delta / 2f);
                break;

            case PlaneEdge.Right:
                scale.x += delta / unit;
                positionOffset -= plane.right * (delta / 2f);
                break;

            case PlaneEdge.Top:
                scale.z += delta / unit;
                positionOffset -= plane.forward * (delta / 2f);
                break;

            case PlaneEdge.Bottom:
                scale.z += delta / unit;
                positionOffset += plane.forward * (delta / 2f);
                break;
        }

        plane.localScale = scale;
        plane.position += positionOffset;
    }

}
