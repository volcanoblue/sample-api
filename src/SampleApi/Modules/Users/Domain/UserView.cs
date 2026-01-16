namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - READ MODEL] Optimized projection for reading user data.
    /// Architectural Role: Implements CQRS pattern separating read model from write model.
    /// Read model optimized for queries, separated from domain entity that handles commands.
    /// </summary>
    public sealed record UserView(Guid Id, string Name, string Email)
    {
        public static UserView From(User user) =>
            new(user.Id, user.Name, user.Email);
    }
}
