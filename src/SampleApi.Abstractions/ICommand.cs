namespace VolcanoBlue.SampleApi.Abstractions
{
    /// <summary>
    /// Marker interface for commands.
    /// 
    /// Architecture Role:
    /// - Represents an intention to change system state
    /// - Used by INPUT PORTS (ICommandHandler)
    /// - Part of the application core's public contract
    /// 
    /// Commands are:
    /// - Imperative (CreateUser, ChangeEmail, PlaceOrder)
    /// - Represent user intentions
    /// - Can succeed or fail with business errors
    /// </summary>
    public interface ICommand
    {
    }
}
