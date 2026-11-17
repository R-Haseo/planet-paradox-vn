using System;
using System.Collections.Generic;

public class ScenarioFlagStore
{
    private readonly Dictionary<string, bool> flags = new();

    public void SetFlag(string key, bool value)
    {
        flags[key] = value;
    }

    public bool GetFlag(string key)
    {
        return flags.TryGetValue(key, out var value) && value;
    }

    public void Clear()
    {
        flags.Clear();
    }

    public Dictionary<string, bool> CreateSnapshot()
    {
        return new Dictionary<string, bool>(flags);
    }

    public void Restore(Dictionary<string, bool> snapshot)
    {
        if (snapshot == null)
        {
            throw new ArgumentNullException(nameof(snapshot));
        }

        flags.Clear();

        foreach (var entry in snapshot)
        {
            flags[entry.Key] = entry.Value;
        }
    }
}
