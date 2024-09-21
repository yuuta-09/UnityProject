using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class EnvManager
{
  /*
    このクラスはシングルトンデザインに基づいて作成されています。
    instanceを呼び出すときにはgetInstanceメソッドを使用してください。
  */

  /*************************
      static fields(public)
    **************************/
  public static string OpenAIKeyName { get; private set; } = "openai-chat-key";

  /*************************
    static fields(private)
  **************************/
  static EnvManager _instance = new EnvManager();


  /*************************
    public fields(public)
  **************************/
  public Dictionary<string, string> EnvInfo = new Dictionary<string, string>(); // 環境変数を格納する

  /*************************
    instance fields(private)
  **************************/
  string _envFilePath = "Assets/Env/.env"; // 環境変数を格納したファイル

  /*************************
    constructor
  **************************/
  EnvManager()
  {
    SetApiInfo();
  }

  /**************************
    static methods(publie)
  **************************/
  public static EnvManager GetInstance()
  {
    return _instance;
  }

  /**************************
    instance methods(private)
  **************************/
  JObject LoadJsonFromFile(string path)
  {
    if (!File.Exists(path))
    {
      throw new FileNotFoundException(path + "が見つかりません");
    }

    string jsonText = File.ReadAllText(path);
    return JObject.Parse(jsonText);
  }

  void SetApiInfo()
  {
    EnvInfo.Clear();

    JObject env = LoadJsonFromFile(_envFilePath);
    if (env == null)
    {
      Debug.LogError("環境変数ファイルの読み込みに失敗しました。");
      return;
    }

    EnvInfo.Add(EnvManager.OpenAIKeyName, env["openAI"]["chatKey"].ToString());
  }

}
