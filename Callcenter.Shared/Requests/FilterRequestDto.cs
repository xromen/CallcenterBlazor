namespace Callcenter.Shared.Requests;

public class FilterRequestDto
{
    public string? Operator { get; set; }
    public string? Field { get; set; }
    public object? Value { get; set; }
}