using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System;
using System.Net.Http;
using System.ComponentModel;
using TMPro;
using Unity.Collections;

public class ARCameraCapture : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private Texture2D cameraTexture;
    private byte[] imageData;
    [SerializeField] TextMeshProUGUI notify;
    private void Awake()
    {
        arCameraManager = GetComponent<ARCameraManager>();
    }


    public async Awaitable<byte[]> Capture()
    {
        await Awaitable.EndOfFrameAsync();

        if (arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            cameraTexture = ConvertImage(image);
            // clear the XRCpuImage to avoid resource leak
            image.Dispose();
            imageData = ConvertTextureToByteArray(cameraTexture);
            notify.text = "Lấy ảnh từ AR Camera thành công!";
            return imageData;

        }
        else
        {
            notify.text = "Không thể lấy ảnh từ AR Camera!";
            return null;
        }
    }

    private byte[] ConvertTextureToByteArray(Texture2D texture)
    {
        return texture.EncodeToPNG();
    }
    private Texture2D ConvertImage(XRCpuImage image)
    {
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width, image.height),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None // Không xoay hoặc lật ảnh
        };

        var buffer = new NativeArray<byte>(image.GetConvertedDataSize(conversionParams), Allocator.Temp);

        try
        {
            // Chuyển đổi ảnh sang buffer
            image.Convert(conversionParams, buffer);

            // Tạo Texture2D từ dữ liệu buffer
            Texture2D texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(buffer);
            texture.Apply();

            return texture; // Trả về Texture2D
        }
        finally
        {
            // Giải phóng bộ nhớ của buffer
            if (buffer.IsCreated)
            {
                buffer.Dispose();
            }
        }
    }

}
