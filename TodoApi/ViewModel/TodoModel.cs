using Flunt.Notifications;
using Flunt.Validations;
using TodoApi.Model;

namespace TodoApi.ViewModel;

public class TodoModel : Notifiable<Notification>
{
    public string Name { get; set; }
    public bool IsFinished { get; } = false;
    public Todo MapTo()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNull(Name, "Not be null")
            .IsGreaterThan(Name, 5, "Should be more 5 caracters"));

        return new (new int(), Name, IsFinished);
    }
}
