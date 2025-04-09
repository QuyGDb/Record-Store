using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AROcclusionHandler : MonoBehaviour
{
    private AROcclusionManager occlusionManager;


    private void Awake()
    {
        occlusionManager = GetComponent<AROcclusionManager>();
        GameResources.Instance.occlusionManager = occlusionManager;
    }
}
