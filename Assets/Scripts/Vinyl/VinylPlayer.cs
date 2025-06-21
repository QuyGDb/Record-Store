using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VinylPlayer : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference;
    private AudioSource audioSource;
    private InputAction touchAction;
    private VinylDisc previousVinylDics;
    private VinylDisc currentVinylDisc;
    private Button pauseButton;
    [SerializeField] private Transform lid;
    [SerializeField] private Transform tonearm;
    public Transform platter;
    private Vector3 lidOpenRotation = new Vector3(0, 0, 68);
    private Vector3 lidClosedRotation = new Vector3(0, 0, -44);
    private Vector3 tonearmOpenRotation = new Vector3(0, 35, 0);
    private Vector3 tonearmClosedRotation = new Vector3(0, 0, 0);
    private Vector3 tonearmEndRotation = new Vector3(0, 62.8f, 0);
    private Tween spinTween;
    int currentTrack = 0;
    bool isOpen = false;
    bool isPicked = false;
    public bool isSpinning = false;
    private string BohemianRhapsody = "Bohemian Rhapsody";
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        touchAction = inputActionReference.action;
        touchAction.Enable();
        touchAction.started += ctx => OnTouch(ctx);
        pauseButton = GetComponentInChildren<Button>();
    }

    private void OnTouch(InputAction.CallbackContext ctx)
    {
        if (isPicked) return;
        PickupVinyl();
    }
    private void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseOrResumeVinyl);
        }

        spinTween = platter.DOLocalRotate(
           new Vector3(0f, 360f, 0f),
           2f,
           RotateMode.FastBeyond360
       )
       .SetEase(Ease.Linear)
       .SetLoops(-1, LoopType.Incremental)
       .Pause();
    }

    private void OnDestroy()
    {
        touchAction.started -= ctx => OnTouch(ctx);
        touchAction.Disable();
    }

    private void Update()
    {
        if (isSpinning && !spinTween.IsPlaying())
        {
            spinTween.Play();
        }
        else if (!isSpinning && spinTween.IsPlaying())
        {
            spinTween.Pause();
        }
    }
    private async void OpenLidVinylPlayer()
    {
        isOpen = true;
        lid.localRotation = Quaternion.Euler(lidClosedRotation);
        lid.DOLocalRotate(lidOpenRotation, 2f);
        await Awaitable.WaitForSecondsAsync(2);
    }
    private async void PickupVinyl()
    {
        if (ApplicationManager.Instance.currentAppState == ApplicationState.TestMap ||
            ApplicationManager.Instance.currentAppState == ApplicationState.Client)
        {
            Vector2 tapPosition = touchAction.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(tapPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<VinylDisc>(out VinylDisc disc))
                {
                    if (!isOpen)
                    {
                        OpenLidVinylPlayer();
                        isPicked = true;
                        await disc.SelectPickAnimation(platter);
                        currentVinylDisc = disc;
                        previousVinylDics = currentVinylDisc;
                    }
                    if (currentVinylDisc != disc)
                    {
                        PauseOrResumeVinyl();
                        tonearm.DOLocalRotate(tonearmClosedRotation, 2f);
                        await previousVinylDics.ReturnDiscToOriginalPosition();
                        currentVinylDisc = disc;
                        previousVinylDics = currentVinylDisc;
                        await currentVinylDisc.SelectPickAnimation(platter);
                        isPicked = true;
                    }
                }
            }
        }
    }
    public void PauseOrResumeVinyl()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isSpinning = false;
        }
        else
        {
            audioSource.Play();
            isSpinning = true;
        }
    }
    public void StartPlayingVinyl(VinylDiscSO vinylDiscSO)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(tonearm.DOLocalRotate(tonearmOpenRotation, 2f))
            .AppendCallback(() =>
            {
                PlayCurrentTrack(vinylDiscSO);
                isPicked = false;
                isSpinning = true;
            })
        .Append(tonearm.DOLocalRotate(tonearmEndRotation, 3600f));

    }
    private void PlayCurrentTrack(VinylDiscSO vinylDiscSO)
    {
        audioSource.clip = vinylDiscSO.songs[currentTrack];
        if (vinylDiscSO.discName == BohemianRhapsody)
        {
            StaticEventHandler.InvokeMovePortal();
        }
        audioSource.Play();
        StartCoroutine(CheckTrackEndCoroutine(vinylDiscSO));
    }
    private IEnumerator CheckTrackEndCoroutine(VinylDiscSO vinylDiscSO)
    {
        AudioClip currentClip = audioSource.clip;
        yield return new WaitUntil(() => audioSource.isPlaying);
        yield return new WaitForSeconds(currentClip.length);
        yield return null;
        if (audioSource.clip == currentClip && !audioSource.isPlaying)
        {
            if (currentTrack < vinylDiscSO.songs.Count - 1)
            {
                currentTrack++;
                PlayCurrentTrack(vinylDiscSO);
            }
        }
    }

}
