using UnityEngine;

public class LiveAid : MonoBehaviour
{
    private Transform[] transforms;

    public int defaultLayer;
    public int insideMaskLayer;

    private void Awake()
    {
        transforms = gameObject.GetComponentsInChildren<Transform>();
        defaultLayer = LayerMask.NameToLayer("Default");
        insideMaskLayer = LayerMask.NameToLayer("InsideMask");
    }

    public void SwapLayer(int targetLayer)
    {
        foreach (Transform t in transforms)
        {
            t.gameObject.layer = targetLayer;
        }
    }


}

