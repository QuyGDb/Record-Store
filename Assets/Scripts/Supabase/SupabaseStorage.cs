using System.Net.Http.Headers;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;

public class SupabaseStorage : SingletonMonobehaviour<SupabaseStorage>
{
    private string supabaseUrl = "https://bcyivtadfugfhlihobmf.supabase.co";
    private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJjeWl2dGFkZnVnZmhsaWhvYm1mIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDUwNjY1MTksImV4cCI6MjA2MDY0MjUxOX0.QofTpju91THDtHxsmKBLsIu_OT9_sX23tXGzcybTDsQ";
    private string bucketName = "record.store";
    private HashSet<string> _processedFiles = new HashSet<string>();


    public async Task UploadSaveFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, Settings.es3Name);
        if (!File.Exists(filePath))
        {
            Debug.LogError("❌ File không tồn tại local để upload!");
            return;
        }

        byte[] fileBytes = File.ReadAllBytes(filePath);

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", supabaseApiKey);

            var content = new ByteArrayContent(fileBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            string encodedBucket = UnityWebRequest.EscapeURL(bucketName);
            string encodedFileName = UnityWebRequest.EscapeURL(Settings.es3Name);
            string url = $"{supabaseUrl}/storage/v1/object/{encodedBucket}/{encodedFileName}";

            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Debug.Log("✅ Upload");
            else
                Debug.LogError("❌ Upload error " + await response.Content.ReadAsStringAsync());
        }
    }

    public async Task ListAndDownloadAllFiles()
    {
        string escapedBucket = UnityWebRequest.EscapeURL(bucketName);
        string listUrl = $"{supabaseUrl}/storage/v1/object/list/{escapedBucket}";

        UnityWebRequest listReq = new UnityWebRequest(listUrl, "POST");
        string bodyJson = "{\"prefix\": \"\"}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);

        listReq.uploadHandler = new UploadHandlerRaw(bodyRaw);
        listReq.downloadHandler = new DownloadHandlerBuffer();
        listReq.SetRequestHeader("Content-Type", "application/json");
        listReq.SetRequestHeader("apikey", supabaseApiKey);
        listReq.SetRequestHeader("Authorization", $"Bearer {supabaseApiKey}");
        listReq.SetRequestHeader("accept", "application/json");

        var asyncListOp = listReq.SendWebRequest();
        while (!asyncListOp.isDone) await Task.Yield();

        if (listReq.result != UnityWebRequest.Result.Success)
            return;

        var filesJson = JSON.Parse(listReq.downloadHandler.text);
        foreach (JSONNode file in filesJson)
        {
            string fileName = file["name"];

            if (!_processedFiles.Add(fileName))
            {
                continue;
            }

            string localPath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(localPath))
            {
                continue;
            }

            await DownloadFile(fileName); 
        }
    }


    public async Task DownloadFile(string fileName)
    {
        if (fileName == ".emptyFolderPlaceholder")
        {
            return;
        }
        string fileUrl = $"{supabaseUrl}/storage/v1/object/public/{UnityWebRequest.EscapeURL(bucketName)}/{UnityWebRequest.EscapeURL(fileName)}";
        UnityWebRequest req = UnityWebRequest.Get(fileUrl);
        var asyncOp = req.SendWebRequest();
        while (!asyncOp.isDone) await Task.Yield();

        if (req.result != UnityWebRequest.Result.Success || req.downloadHandler.data == null)
            return;
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, req.downloadHandler.data);
    }


}
