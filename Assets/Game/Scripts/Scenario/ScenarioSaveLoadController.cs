
using System;
using System.IO;
using UnityEngine;

public class ScenarioSaveLoadController : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void Save(int scenarioIndex, int commandIndex)
    {
        var data = new ScenarioSaveData
        {
            ScenarioIndex = scenarioIndex,
            CommandIndex = commandIndex
        };

        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));

        Debug.Log($"Saved: scenario={scenarioIndex}, command={commandIndex}");
    }

    public bool TryLoad(out ScenarioSaveData data)
    {
        data = null;

        if (!File.Exists(SavePath))
        {
            Debug.Log("Save data not found.");
            return false;
        }

        try
        {
            data = JsonUtility.FromJson<ScenarioSaveData>(File.ReadAllText(SavePath));
            return data != null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save data: {e.Message}");
            return false;
        }
    }
}

[Serializable]
public class ScenarioSaveData
{
    public int ScenarioIndex;
    public int CommandIndex;
}
