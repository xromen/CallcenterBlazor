namespace Callcenter.Shared.Responses;

public class GetDeclarationsResponseDto
{
    public DeclarationsStatsDto Statistics { get; set; }
    
    public IEnumerable<GetRkListDto> Declarations { get; set; }
    
    public long TotalDeclarationsItems { get; set; }
}

public class DeclarationsStatsDto
{
    public long SendAnswerCount { get; set; }
    
    public long NeedReworkCount { get; set; }
    
    public long SmoRedirectCount { get; set; }
}