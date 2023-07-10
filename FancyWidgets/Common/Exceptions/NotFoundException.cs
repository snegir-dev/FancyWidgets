namespace FancyWidgets.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string? message)
        : base(message)
    {
    }

    public static NotFoundException ThrowNotFoundException(string id) =>
        throw new NotFoundException($"Element with id - {id} not found.");

    public static NotFoundException ThrowNotFoundException(string fullNameClass, string propertyName) =>
        throw new NotFoundException($"Element with FullClassName - " +
                                    $"{fullNameClass} and PropertyName - {propertyName} not found.");
}