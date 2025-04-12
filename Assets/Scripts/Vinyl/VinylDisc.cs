using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class VinylDisc : MonoBehaviour
{
    public VinylDiscSO vinylDiscSO;
    [SerializeField] private Transform disc;

    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private Vector3 verticalRotation;
    private Vector3 horizontalDiscRotation;
    private Vector3 offsetWithPlatter = new Vector3(0, 0.009f, 0);
    private void Awake()
    {
        initialRotation = transform.localRotation.eulerAngles;
        initialPosition = transform.localPosition;
        verticalRotation = new Vector3(0, 180, 0);
        horizontalDiscRotation = new Vector3(90, 0, 0);
    }
    public async Task SelectPickAnimation(Transform targetPosition)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(transform.localPosition.y + 0.35f, 2f))
           .Append(transform.DOLocalRotate(verticalRotation, 2f))
           .Append(disc.DOLocalMoveX(disc.localPosition.x + 0.35f, 2f))
           .Append(disc.DOLocalRotate(horizontalDiscRotation, 2f))

           .Append(disc.DOMove(targetPosition.position + offsetWithPlatter, 2.5f))
              .AppendCallback(() =>
              {
                  disc.SetParent(targetPosition);

              })
             .Append(transform.DOLocalRotate(initialRotation, 2f))
        .Append(transform.DOLocalMove(initialPosition, 2f))
        .AppendCallback(() => targetPosition.parent.parent.GetComponent<VinylPlayer>().StartPlayingVinyl(vinylDiscSO));
        await seq.AsyncWaitForCompletion();
    }
    public async Task ReturnDiscToOriginalPosition()
    {
        disc.SetParent(transform);

        Sequence seq = DOTween.Sequence();
        seq.Append(disc.DOLocalMoveZ(disc.localPosition.z + 0.35f, 2f))
            .Append(disc.DOMove(transform.position, 2.5f));
        await seq.AsyncWaitForCompletion();
    }

}
