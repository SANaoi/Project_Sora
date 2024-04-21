using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using aoi;

public class LocalConfig
{
    public static Dictionary<string, UserData> usersData = new Dictionary<string,UserData>();
    public static void  SaveUserData(UserData userData)
    {
        if (!File.Exists(Application.persistentDataPath + "/users"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/users");
        }
        usersData[userData.name] = userData;
        string jsonData = JsonConvert.SerializeObject(userData);
        File.WriteAllText(Application.persistentDataPath + string.Format("/users/{0}.json", userData.name), jsonData);
    }

    public static UserData LoadUserData(string userName)
    {
        if (usersData.ContainsKey(userName))
        {
            return usersData[userName];
        }

        string dataPath = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);
            // 将json字符串转换为用户内存数据
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            return userData;
        }
        else
        {
            return null;
        }
    }
}   


public class UserData
{
    public string name;
    public List<string> teamCharacterNames;
    public List<int> health;
    public List<int> magazineAmmo;
    public string currentScene = "";
    public List<int> currentTasks = new();
    public List<int> completedTasks = new();
}
