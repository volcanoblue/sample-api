using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public sealed class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; private set; }

        private User(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public static Result<User, IError> Create(Guid id, string name, string email)
        {
            if(name is null or { Length: 0 })
                return UserErrors.EmptyName;

            if (email is null or { Length: 0 })
                return UserErrors.EmptyEmail;

            return new User(id, name, email);
        }

        public Result<Unit, IError> ChangeEmail(string newEmail)
        {
            if (newEmail is null or { Length: 0 })
                return UserErrors.EmptyEmail;
            
            Email = newEmail;
                        
            return Unit.Value;
        }
    }
}
