using Moonad;
using System.Collections.Immutable;
using VolcanoBlue.Core.Error;
using VolcanoBlue.EventSourcing.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - AGGREGATE ROOT] User domain entity with Event Sourcing.
    /// Architectural Role: Encapsulates business logic and validation rules for users.
    /// Implements Event Sourcing to maintain complete history of state changes through events.
    /// Ensures business invariants (name and email required) and exposes only valid operations.
    /// </summary>
    public sealed partial class User : EventSourcedEntity
    {
        public Guid Id { get; private set; } = Guid.Empty;
        public Name Name { get; private set; }
        public Email Email { get; private set; }

        private User(Guid id, Name name, Email email) =>
            RaiseEvent(new UserCreated(Version, id, name, email));

        private User(IEnumerable<IEvent> stream) =>
            FeedEvents(stream);

        public static Result<User, IError> Create(Guid id, string name, string email)
        {
            var nameCreated = Name.Create(name);
            if (nameCreated.IsError)
                return Result<User, IError>.Error(nameCreated.ErrorValue);

            var emailCreated = Email.Create(email);
            if (emailCreated.IsError)
                return Result<User, IError>.Error(emailCreated.ErrorValue);

            return new User(id, nameCreated.ResultValue, emailCreated.ResultValue);
        }

        public static Result<User, IError> Restore(ImmutableArray<IEvent> stream)
        {
            if(stream.Length > 0)
                return new User(stream);

            return UserNotFoundError.Instance;
        }
        
        public Result<Unit, IError> ChangeEmail(string newEmail)
        {
            var emailCreated = Email.Create(newEmail);
            if (emailCreated.IsError)
                return Result<Unit, IError>.Error(emailCreated.ErrorValue);

            RaiseEvent(new UserEmailChanged(Version, Id, emailCreated));

            return Unit.Value;
        }

        private void Apply(UserCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Email = @event.Email;
        }

        private void Apply(UserEmailChanged @event)
        {
            Email = @event.NewEmail;
        }

        protected override void ProcessEvent(IEvent @event)
        {
            switch (@event)
            {
                case UserCreated userCreated:
                    Apply(userCreated);
                    break;
                case UserEmailChanged userEmailChanged:
                    Apply(userEmailChanged);
                    break;
            }
        }
    }
}
