namespace GSG.Repository;

public interface IIdentityContext
{
    string UserName { get; }
    string Name { get;  }
    string LastName { get;  }
    string Email { get; }
}