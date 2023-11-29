namespace Models
{
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// "CommandModel" represents the model for handling commands with associated values.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public class CommandModel
    {
        public string? Command { get; set; }
        public string? Value { get; set; }
        
        public CommandModel() { }
        public CommandModel(string command, string value)
        {
            Command = command;
            Value = value;
        }
    }
}
