using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _text;

  private ChatCompletion chatCompletion = new ChatCompletion();

  async void Start()
  {
    EnvManager envManager = EnvManager.GetInstance();
    string apiKey = envManager.EnvInfo[EnvManager.OpenAIKeyName];

    chatCompletion.AddMessage(ChatCompletion.RoleTypes.System, "You are nice assistant for programmer!");
    chatCompletion.AddMessage(ChatCompletion.RoleTypes.User, "Please tell me how to do 'Hello World!' using C#");
    string result = await chatCompletion.SendRequest(apiKey);

    _text.text = result;
  }
}
