
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ScenarioRewindController : MonoBehaviour
{
    [SerializeField] private ScenarioStateRestorer stateRestorer;

    public bool TryRewind(
        IReadOnlyList<TextAsset> scenarioJsons,
        int currentScenarioIndex,
        int currentCommandIndex,
        out RewindResult result)
    {
        result = null;

        if (!TryFindPreviousDisplayCommand(
                scenarioJsons,
                currentScenarioIndex,
                currentCommandIndex,
                out var targetScenarioIndex,
                out var targetCommandIndex))
        {
            Debug.Log("No previous display command.");
            return false;
        }

        var targetCommands = GetScenarioCommands(scenarioJsons, targetScenarioIndex);

        stateRestorer.Restore(
            scenarioJsons,
            currentScenarioIndex,
            currentCommandIndex,
            targetScenarioIndex,
            targetCommandIndex);

        result = new RewindResult(
            targetScenarioIndex,
            targetCommandIndex,
            targetCommands);

        Debug.Log(
            $"Rewind: scenario={targetScenarioIndex}, command={targetCommandIndex}");

        return true;
    }

    private bool TryFindPreviousDisplayCommand(
        IReadOnlyList<TextAsset> scenarioJsons,
        int scenarioIndex,
        int commandIndex,
        out int targetScenarioIndex,
        out int targetCommandIndex)
    {
        for (var i = scenarioIndex; i >= 0; i--)
        {
            var scenarioCommands = GetScenarioCommands(scenarioJsons, i);

            var startIndex = i == scenarioIndex ? commandIndex - 1 : scenarioCommands.Count - 1;

            for (var j = startIndex; j >= 0; j--)
            {
                if (!IsDisplayCommand(scenarioCommands[j]))
                {
                    continue;
                }

                targetScenarioIndex = i;
                targetCommandIndex = j;
                return true;
            }
        }

        targetScenarioIndex = -1;
        targetCommandIndex = -1;
        return false;
    }

    private static bool IsDisplayCommand(ScenarioCommandDto command)
    {
        return command.Type == "dialogue" || command.Type == "description";
    }

    private static List<ScenarioCommandDto> GetScenarioCommands(IReadOnlyList<TextAsset> scenarioJsons, int scenarioIndex)
    {
        var scenarioJson = scenarioJsons[scenarioIndex];

        var scenarioData = JsonConvert.DeserializeObject<ScenarioDataDto>(scenarioJson.text);

        return scenarioData.Commands;
    }
}

public class RewindResult
{
    public int ScenarioIndex { get; }
    public int CommandIndex { get; }
    public List<ScenarioCommandDto> Commands { get; }

    public RewindResult(
        int scenarioIndex,
        int commandIndex,
        List<ScenarioCommandDto> commands)
    {
        ScenarioIndex = scenarioIndex;
        CommandIndex = commandIndex;
        Commands = commands;
    }
}
