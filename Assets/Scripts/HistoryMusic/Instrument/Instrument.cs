using UnityEngine;

public class Instrument : MonoBehaviour
{
    [HideInInspector] public Transform currentTranforms;
    public InstrumentDetails instrumentDetails;

    private void Awake()
    {
        currentTranforms = ES3.Load(gameObject.name, transform);
        gameObject.transform.position = currentTranforms.position;
        gameObject.transform.rotation = currentTranforms.rotation;
        gameObject.transform.localScale = currentTranforms.localScale;
    }


}
