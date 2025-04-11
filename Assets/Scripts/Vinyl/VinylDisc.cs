using DG.Tweening;
using UnityEngine;

public class VinylDisc : MonoBehaviour
{
    public VinylDiscSO vinylDiscSO;
    [SerializeField] private Transform disc;

    public void SelectPickAnimation(Transform targetPosition)
    {
        transform.DOLocalMoveY(targetPosition.localPosition.y, 0.5f)
            .OnComplete(() =>
            {
                transform.DOLocalRotate(new Vector3(0, 0, 0), 2f)
                    .OnComplete(() =>
                    {
                        disc.DOLocalMoveX(targetPosition.localPosition.x + 0.35f, 0.5f)
                            .OnComplete(() =>
                            {
                                disc.DOLocalRotate(new Vector3(90, 0, 0), 2f)
                                    .OnComplete(() =>
                                    {
                                        disc.SetParent(targetPosition);
                                        disc.DOMove(targetPosition.position, 2.5f)
                                            .OnComplete(() =>
                                            {

                                            });
                                    });
                            });

                    });
            });
    }
}
