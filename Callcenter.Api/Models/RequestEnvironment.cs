using System.Net;
using Callcenter.Api.Data.Entities;

namespace Callcenter.Api.Models;

public class RequestEnvironment
{
    public User? AuthUser { get; set; }
    
    public IPAddress? ClientIp { get; set; }
}