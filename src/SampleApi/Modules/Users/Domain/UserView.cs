namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public sealed record UserView(Guid Id, string Name, string Email);

    public static class UserViewMapper
    {
        public static UserView FromEntity(User user) => 
            new (user.Id, user.Name, user.Email);
    }
}
