namespace VolcanoBlue.Core.Command
{
    /// <summary>
    /// [APPLICATION - MARKER INTERFACE] Base marker interface for all commands in the system.
    /// Architectural Role: Identifies command objects in CQRS pattern. Commands represent write operations
    /// that change system state. Marker interface enables generic constraint on ICommandHandler.
    /// </summary>
    public interface ICommand
    {
    }
}
