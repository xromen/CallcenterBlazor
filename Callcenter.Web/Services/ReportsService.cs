using Callcenter.Web.Models;
using Callcenter.Web.Services.Interfaces;

namespace Callcenter.Web.Services;

public class ReportsService(IApiClient apiClient)
{
    public async Task<ApiResult<byte[]>> GetPgFormGeneral(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetPgFormGeneral(from, to, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    
    public async Task<ApiResult<byte[]>> GetPgFormGroupByMo(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetPgFormGroupByMo(from, to, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    
    public async Task<ApiResult<byte[]>> GetJustifiedComplaintsForm(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetJustifiedComplaintsForm(from, to, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    
    public async Task<ApiResult<byte[]>> GetAllComplaintsForm(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetAllComplaintsForm(from, to, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
    
    public async Task<ApiResult<byte[]>> GetOmsPerformanceCriteriaForm(int month, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetOmsPerformanceCriteriaForm(month, cancellationToken);
        
        return await ApiResult<byte[]>.FromResponseAsync(response, () => response.Content!.ReadAsByteArrayAsync(cancellationToken));
    }
}