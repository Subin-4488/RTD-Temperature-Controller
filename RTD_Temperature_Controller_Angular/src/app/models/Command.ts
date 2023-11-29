
//////////////////////////////////////////////////////////////////////////
/// <summary>
/// "Command" represents the model for handling commands with associated values.
/// </summary>
/// <remarks>
/// E.G: for the command "GET TMPA", a new Command object is Command object is generated as
/// Command = "GET", Value = "GET TMPA"
/// </remarks>
//////////////////////////////////////////////////////////////////////////

export class Command{
    Command:string
    Value:string
    constructor(command:string,value:string){
        this.Command=command
        this.Value = value
    }
}