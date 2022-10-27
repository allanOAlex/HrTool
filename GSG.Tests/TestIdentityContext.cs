using GSG.Repository;

namespace GSG.Tests;

public class TestIdentityContext : IIdentityContext
{
    public string UserName { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Email { get; }

    public TestIdentityContext(string userName, string name, string lastName, string email)
    {
        UserName = userName;
        Name = name;
        LastName = lastName;
        Email = email;
    }
}