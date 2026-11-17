using System;
using System.Collections.Generic;

[Serializable]
public class ScenarioChoiceOptionDto
{
    public string OptionId;
    public string Text;
    public string Target;
    public Dictionary<string, bool> SetFlags;
}
