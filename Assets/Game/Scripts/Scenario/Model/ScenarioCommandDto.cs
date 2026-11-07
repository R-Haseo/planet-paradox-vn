using System;

[Serializable]
public class ScenarioCommandDto
{
    public string Type;
    public string Id;
    public string Speaker;
    public string Text;
    public string AssetId;
    public float? Volume;
    public string Action;
    public string Position;
    public float Width;
    public float Height;
    public float OffsetX;
    public float OffsetY;
    public float Duration;
    public bool FlipX { get; set; }
    public ScenarioChoiceOptionDto[] Options;
    public string Label;
    public string Target;
    public ScenarioConditionDto Condition;
    public string TrueTarget;
    public string FalseTarget;
    public string Flag;
    public bool Value;
}
