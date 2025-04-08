using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class InstrumentsManager : MonoBehaviour
{
    public InstrumentShowcaseListSO instrumentShowcaseListSO;
    public float maxDistance = 100f;
    public LayerMask hitLayers;
    private InstrumentShowcase previousInstrumentShowcase;
    private InstrumentShowcase currentInstrumentShowcase;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    public float scaleSpeed = 0.1f;

    private void OnEnable()
    {
        StaticEventHandler.OnInstrumentSOSelected += StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnInstrumentShowcaseChanged += StaticEventHandler_OnInstrumentShowcaseChanged;
    }

    private void StaticEventHandler_OnInstrumentShowcaseChanged(InstrumentShowcase showcase)
    {
        currentInstrumentShowcase = showcase;
        if (previousInstrumentShowcase != null && previousInstrumentShowcase != currentInstrumentShowcase)
        {
            previousInstrumentShowcase.Deselect();
        }
        previousInstrumentShowcase = currentInstrumentShowcase;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnInstrumentSOSelected -= StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnInstrumentShowcaseChanged -= StaticEventHandler_OnInstrumentShowcaseChanged;
    }

    private void Start()
    {
        StaticEventHandler.InvokeInstrumentsManagerChanged(this);
    }


    private void StaticEventHandler_OnInstrumentSelected(InstrumentShowcaseSO instrumentShowcase, bool isAdd)
    {
        // select list of instrument to add to the select anchor
        if (isAdd)
        {
            instrumentShowcaseListSO.instrumentShowcaseList.Add(instrumentShowcase);
        }
        else
        {
            instrumentShowcaseListSO.instrumentShowcaseList.Remove(instrumentShowcase);
        }
    }

    public void SaveObjectAtReleasePosition()
    {
        if (currentInstrumentShowcase == null) return;
        LocalTransfrom localTransfrom = new LocalTransfrom();
        localTransfrom.localPosition = currentInstrumentShowcase.transform.localPosition;
        localTransfrom.localRotation = currentInstrumentShowcase.transform.rotation;
        localTransfrom.localScale = currentInstrumentShowcase.transform.localScale;
        textMeshProUGUI.text = localTransfrom.localPosition + ", " + localTransfrom.localRotation + ", " + localTransfrom.localScale;
        ES3.Save(currentInstrumentShowcase.instrumentShowcaseSO.instrumentName, localTransfrom);
    }

    public void DeteleCurrentSelectedObject()
    {
        if (currentInstrumentShowcase != null)
        {
            Destroy(currentInstrumentShowcase);
        }
    }
}


