using UnityEngine;
using DG.Tweening;

public class AlbumSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;   
    public Transform centerPoint;        
    public float spawnInterval = 5f;    
    public float tweenDuration = 5f;    
    public float horizontalOffset = 100f; 
public Vector3 initialScale = new Vector3(100f, 100f, 100f); // Initial scale for the objects   
    private GameObject[] pool;
    private int currentIndex = -1;

    void Start()
    {
        pool = new GameObject[objectPrefabs.Length];
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject obj = Instantiate(objectPrefabs[i], centerPoint.position, Quaternion.identity, transform);
            obj.transform.localScale = Vector3.zero;
            obj.transform.rotation = Quaternion.Euler(0, 180, 0);
            obj.SetActive(false);
            pool[i] = obj;
        }

        InvokeRepeating(nameof(ShowNextObject), 0f, spawnInterval);
    }

    void ShowNextObject()
    {
        if (currentIndex >= 0)
        {
            GameObject current = pool[currentIndex];
            current.transform.DOMoveX(centerPoint.position.x - horizontalOffset, tweenDuration).SetEase(Ease.InOutQuad);
            current.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InOutQuad);
        }

        currentIndex = (currentIndex + 1) % pool.Length;

        GameObject next = pool[currentIndex];
        next.SetActive(true);

        Vector3 startPos = centerPoint.position + Vector3.right * horizontalOffset;
        next.transform.position = startPos;
        next.transform.localScale = Vector3.zero;

        next.transform.DOMove(centerPoint.position, tweenDuration).SetEase(Ease.OutBack);
        next.transform.DOScale(initialScale, tweenDuration).SetEase(Ease.OutBack);
    }
}
