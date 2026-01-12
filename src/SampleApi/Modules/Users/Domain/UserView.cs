namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - READ MODEL] Optimized projection for reading user data.
    /// Architectural Role: Implements CQRS pattern separating read model from write model.
    /// DTO optimized for queries, separated from domain entity that handles commands.
    /// </summary>
    public sealed record UserView(Guid Id, string Name, string Email);

    public static class UserViewMapper
    {
        public static UserView FromEntity(User user) => 
            new (user.Id, user.Name, user.Email);
    }
}
