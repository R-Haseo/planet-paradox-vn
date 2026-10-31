
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ScenarioStateRestorer : MonoBehaviour
{
    [SerializeField] private CharacterPlayer characterPlayer;
    [SerializeField] private BackgroundPlayer backgroundPlayer;
    [SerializeField] private BGMPlayer bgmPlayer;

    public void Restore(
        IReadOnlyList<TextAsset> scenarioJsons,
        int currentScenarioIndex,
        int currentCommandIndex,
        int targetScenarioIndex,
        int targetCommandIndex)
    {
        var currentCharacterStates = GetCharacterStates(
            scenarioJsons,
            currentScenarioIndex,
            currentCommandIndex);

        var targetState = BuildState(
            scenarioJsons,
            targetScenarioIndex,
            targetCommandIndex);

        RestoreBackground(targetState.BackgroundAssetId);
        RestoreCharacters(currentCharacterStates, targetState.CharacterStates);
        RestoreBgm(targetState.BgmAssetId, targetState.BgmPlaying);
    }

    private ScenarioState BuildState(
        IReadOnlyList<TextAsset> scenarioJsons,
        int targetScenarioIndex,
        int targetCommandIndex)
    {
        var state = new ScenarioState();

        for (var scenarioIndex = 0; scenarioIndex <= targetScenarioIndex; scenarioIndex++)
        {
            var commands = GetScenarioCommands(scenarioJsons, scenarioIndex);

            var endIndex = scenarioIndex == targetScenarioIndex ? targetCommandIndex : commands.Count - 1;

            for (var commandIndex = 0; commandIndex <= endIndex; commandIndex++)
            {
                var command = commands[commandIndex];

                switch (command.Type)
                {
                    case "background":
                        state.BackgroundAssetId = command.AssetId;
                        break;

                    case "character":
                        UpdateCharacterState(state.CharacterStates, command);
                        break;

                    case "bgm":
                        UpdateBgmState(command, state);
                        break;
                }
            }
        }

        return state;
    }

    private Dictionary<CharacterPosition, ScenarioCommandDto>
        GetCharacterStates(
            IReadOnlyList<TextAsset> scenarioJsons,
            int scenarioIndex,
            int commandIndex)
    {
        return BuildState(
            scenarioJsons,
            scenarioIndex,
            commandIndex).CharacterStates;
    }

    private static void UpdateCharacterState(
        Dictionary<CharacterPosition, ScenarioCommandDto> characterStates,
        ScenarioCommandDto command)
    {
        var position = ParseCharacterPosition(command.Position);

        switch (command.Action)
        {
            case "show":
                characterStates[position] = command;
                break;

            case "hide":
                characterStates.Remove(position);
                break;
        }
    }

    private static void UpdateBgmState(ScenarioCommandDto command, ScenarioState state)
    {
        switch (command.Action)
        {
            case "play":
                state.BgmAssetId = command.AssetId;
                state.BgmPlaying = true;
                break;

            case "stop":
                state.BgmAssetId = null;
                state.BgmPlaying = false;
                break;
        }
    }

    private void RestoreBackground(string assetId)
    {
        if (string.IsNullOrEmpty(assetId))
        {
            return;
        }

        backgroundPlayer.Show(assetId);
    }

    private void RestoreCharacters(
        Dictionary<CharacterPosition, ScenarioCommandDto> currentStates,
        Dictionary<CharacterPosition, ScenarioCommandDto> targetStates)
    {
        var positions = new[]
        {
            CharacterPosition.Left,
            CharacterPosition.Center,
            CharacterPosition.Right
        };

        foreach (var position in positions)
        {
            var hasCurrent = currentStates.TryGetValue(position, out var currentCommand);
            var hasTarget = targetStates.TryGetValue(position, out var targetCommand);

            if (!hasTarget)
            {
                if (hasCurrent)
                {
                    characterPlayer.Hide(position);
                }

                continue;
            }

            if (hasCurrent && IsSameCharacterState(currentCommand, targetCommand))
            {
                continue;
            }

            characterPlayer.Show(
                targetCommand.AssetId,
                position,
                targetCommand.Width,
                targetCommand.Height,
                targetCommand.OffsetX,
                targetCommand.OffsetY);
        }
    }

    private void RestoreBgm(string assetId, bool isPlaying)
    {
        if (isPlaying && !string.IsNullOrEmpty(assetId))
        {
            bgmPlayer.Play(assetId);
            return;
        }

        bgmPlayer.Stop();
    }

    private static bool IsSameCharacterState(ScenarioCommandDto current, ScenarioCommandDto target)
    {
        return current.AssetId == target.AssetId &&
               current.Width == target.Width &&
               current.Height == target.Height &&
               current.OffsetX == target.OffsetX &&
               current.OffsetY == target.OffsetY;
    }

    private static CharacterPosition ParseCharacterPosition(string position)
    {
        return position switch
        {
            "left" => CharacterPosition.Left,
            "right" => CharacterPosition.Right,
            _ => CharacterPosition.Center
        };
    }

    private static List<ScenarioCommandDto> GetScenarioCommands(IReadOnlyList<TextAsset> scenarioJsons, int scenarioIndex)
    {
        var scenarioJson = scenarioJsons[scenarioIndex];

        var scenarioData = JsonConvert.DeserializeObject<ScenarioDataDto>(scenarioJson.text);

        return scenarioData.Commands;
    }

    private class ScenarioState
    {
        public string BackgroundAssetId { get; set; }
        public string BgmAssetId { get; set; }
        public bool BgmPlaying { get; set; }

        public Dictionary<CharacterPosition, ScenarioCommandDto>
            CharacterStates { get; } = new();
    }
}
