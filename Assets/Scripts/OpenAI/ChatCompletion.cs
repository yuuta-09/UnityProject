using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json.Linq;

public class ChatCompletion
{
  /*************************
    enum
  **************************/
  public enum ModelTypes
  {
    GPT_4o,
    GPT_4o_mini
  }

  public enum RoleTypes
  {
    System,
    User
  }

  /*************************
    instance fields(private)
  **************************/
  private string _apiUrl = "https://api.openai.iniad.org/api/v1/chat/completions";

  /*************************
    instance fields
  **************************/
  public ModelTypes ModelType = ModelTypes.GPT_4o_mini;
  public JArray Messages { get; private set; } = new JArray();

  /**************************
    instance methods(public)
  **************************/
  public void ResetMessages()
  {
    Messages.Clear();
  }

  public void AddMessage(RoleTypes role, string content)
  {
    JObject message = new JObject { { "role", RoleTypeToString(role) }, { "content", content } };
    Messages.Add(message);
  }

  public async Task<string> SendRequest(string apiKey)
  {
    JObject jsonData = new JObject();

    // request bodyの作成
    jsonData["model"] = ModelTypeToString(ModelType);
    jsonData["messages"] = Messages;

    using (UnityWebRequest request = CreateJsonRequest(_apiUrl, apiKey, jsonData))
    {
      var asyncOperation = request.SendWebRequest();

      while (!asyncOperation.isDone)
      {
        await Task.Yield();
      }

      if (request.result != UnityWebRequest.Result.Success)
      {
        Debug.LogError("Error: " + request.error);
        Debug.LogError("Response: " + request.downloadHandler.text);
        return null;
      }

      JObject jsonResponse = JObject.Parse(request.downloadHandler.text);
      Debug.Log(jsonResponse);
      string generatedText = jsonResponse["choices"][0]["message"]["content"].ToString();

      return generatedText;
    }
  }

  /**************************
    instance methods(private)
  **************************/
  private UnityWebRequest CreateJsonRequest(string url, string apiKey, JObject jsonData)
   {
    UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData.ToString());
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();

    request.SetRequestHeader("Content-Type", "application/json");
    request.SetRequestHeader("Authorization", "Bearer" + " " + apiKey);

    return request;
  }

  private string ModelTypeToString(ModelTypes model)
  {
    string result = null;

    switch (model)
    {
      case ModelTypes.GPT_4o:
        result = "gpt-4o";
        break;
      case ModelTypes.GPT_4o_mini:
        result = "gpt-4o-mini";
        break;
    }

    return result;
  }

  private string RoleTypeToString(RoleTypes role)
  {
    string result = null;

    switch (role)
    {
      case RoleTypes.System:
        result = "system";
        break;
      case RoleTypes.User:
        result = "user";
        break;
    }

    return result;

  }
}
