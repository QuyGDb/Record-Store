using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public static class HelperUtilities
{
    public static Camera mainCamera;
    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }
    /// <summary>
    /// null value debug check
    /// </summary>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }
    /// <summary>
    /// list empty or contains null value check - returns true if there is an error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject.name.ToString());
            return true;
        }


        foreach (var item in enumerableObjectToCheck)
        {

            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }



    /// <summary>
    /// Get the angle in degrees from a direction vector
    /// </summary>
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;

    }

    public static byte[] TextureToBytes(Texture2D texture, bool usePNG = true)
    {
        if (texture == null) return null;

        return usePNG ? texture.EncodeToPNG() : texture.EncodeToJPG();
    }


    /// <summary>
    /// Get the direction vector from an angle in degrees
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }

    /// <summary>
    /// positive value debug check - if zero is allowed set isZeroAllowed to true. Returns true if there is an error
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }
    private static bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    /// <summary>
    /// Get the mouse world position.
    /// </summary>

    public static Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.transform.position.z));
        return worldPosition;
    }
    /// <summary>
    /// positive range debug check - set isZeroAllowed to true if the min and max range values can both be zero. Returns true if there is an error
    /// </summary>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum, float valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be less than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;

        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }

    // / <summary>
    // / Get the mouse position in UI space.
    // / </summary>
    public static Vector3 GetMousePositionInUI(RectTransform rectTransform, Camera uiCamera = null)
    {

        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

        // Clamp mouse position to screen size
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                mouseScreenPosition,
                uiCamera,
                out Vector2 localPoint
            );
        Vector3 mousePosition = rectTransform.TransformPoint(localPoint);

        return mousePosition;
    }

    /// <summary>
    /// Convert the linear volume scale to decibels
    /// </summary>
    public static float LinearToDecibels(float linear)
    {
        float linearScaleRange = 20f;

        // formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
    #region CaptureScreenshot
    public static async Awaitable<byte[]> CaptureScreenshot(ARAnchor anchor)
    {
        await Awaitable.EndOfFrameAsync();
        byte[] textureData;

        int width = Screen.width;
        int height = Screen.height;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        textureData = texture.EncodeToPNG();
        return textureData;

    }
    public static Sprite TextureToSprite(Texture2D texture)
    {
        if (texture == null) return null;

        return Sprite.Create(texture,
                             new Rect(0, 0, texture.width, texture.height),
                             new Vector2(0.5f, 0.5f),
                             100.0f); // Pixels Per Unit (PPU)
    }
    public static Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite == null) return null;

        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x,
                                                  (int)sprite.rect.y,
                                                  (int)sprite.rect.width,
                                                  (int)sprite.rect.height);
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
    #endregion

    public static Sprite SetSprite(byte [] spriteBytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(spriteBytes);
        Sprite newSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        return newSprite;
    }


    public async static Awaitable<byte[]> CaptureWithARObjects()
    {
        await Awaitable.EndOfFrameAsync();

        int width = Screen.width;
        int height = Screen.height;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        Camera.main.targetTexture = renderTexture;

        Camera.main.Render();

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        byte[] imageData = texture.EncodeToPNG();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(renderTexture);
        Object.Destroy(texture);

        return imageData;
    }

}
