using UnityEngine;

public class PictureFrame : MonoBehaviour
{
    public string pictureName;
    private Transform localTranssfrom;
    private void Awake()
    {
        localTranssfrom = ES3.Load(pictureName, transform);
        if (localTranssfrom != null)
        {
            transform.localPosition = localTranssfrom.localPosition;
            transform.localRotation = localTranssfrom.localRotation;
            transform.localScale = localTranssfrom.localScale;
        }
    }
}
