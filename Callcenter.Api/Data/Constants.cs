namespace Callcenter.Api.Data;

public static class Constants
{
    public static readonly DateTime NullDateTime = new DateTime(1900, 1, 1);
    public static readonly DateOnly NullDateOnly = DateOnly.FromDateTime(NullDateTime);
}