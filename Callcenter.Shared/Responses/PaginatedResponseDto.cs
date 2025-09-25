namespace Callcenter.Shared.Responses;

public record PaginatedResponseDto<T>(
    int Page,
    long TotalItems,
    IEnumerable<T> Items
);