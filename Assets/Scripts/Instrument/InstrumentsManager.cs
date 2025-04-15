using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InstrumentsManager : MonoBehaviour
{
    private InputAction touchAction;
    [SerializeField] private GameObject instrumentUIPrefab;
    private InstrumentUI instrumentUI;
    private InstrumentSO instrumentSO;
    private Vector3 offset = new Vector3(0, 0.35f, -0.35f);
    private void Awake()
    {
        touchAction = GameResources.Instance.touchRef.action;
        touchAction.Enable();
        touchAction.started += OnTouchStarted;
    }
    private void OnDestroy()
    {
        touchAction.started -= OnTouchStarted;
        touchAction.Disable();
    }
    private void Start()
    {
        GameObject obj = Instantiate(instrumentUIPrefab, transform);
        obj.SetActive(false);
        instrumentUI = obj.GetComponent<InstrumentUI>();
    }
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (GameManager.Instance.applicationState != ApplicationState.LoadMapMode) return;
        Vector2 touchPosition = context.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        int instrumentLayerMask = 1 << LayerMask.NameToLayer("Instrument");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, instrumentLayerMask))
        {
            var instrument = hit.collider.gameObject.GetComponent<Instrument>();
            instrumentSO = instrument.instrumentSO;
            instrumentUI.SetData(instrumentSO, hit.transform.position + offset);
        }
    }




}
