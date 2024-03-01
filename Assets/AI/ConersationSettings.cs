using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // UIコンポーネントを使用するため
using TMPro; // TextMeshProを使用する場合

public class DialogueManager : MonoBehaviour
{
    public InputField userInputField; // ユーザー入力用のInputField
    public TextMeshProUGUI messageText; // レスポンス表示用のTextMeshProUGUI

    // ユーザーが入力を送信するためのメソッド
    public void SubmitInput()
    {
        string userMessage = userInputField.text;
        StartCoroutine(SendRequestToAPI(userMessage));
    }

    IEnumerator SendRequestToAPI(string message)
    {
        string apiUrl = "APIのURL"; // APIのエンドポイントURLを設定
        WWWForm form = new WWWForm();
        form.AddField("user_id", "1");
        form.AddField("character_id", "1");
        form.AddField("message", message);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + www.error);
            }
            else
            {
                // JSONからレスポンスメッセージをパースして表示
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(www.downloadHandler.text);
                messageText.text = responseData.message;
            }
        }
    }

    [Serializable]
    private class ResponseData
    {
        public string session_id;
        public string message;
        public Status status;
    }

    [Serializable]
    private class Status
    {
        public string code;
        public float execution_time;
    }
}
