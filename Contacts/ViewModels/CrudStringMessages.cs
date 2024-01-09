namespace Contacts.ViewModels;
public enum Crud
{
    Created,
    Read,
    Updated,
    Deleted,
}

public static class CrudStringMessage
{
    public static string FormatMessage(string name, Crud operation)
    {
        if (operation == Crud.Read) { return string.Empty; }
        return $"The contact {name} was {operation.ToString("g").ToLowerInvariant()} successfully.";
    }
}
