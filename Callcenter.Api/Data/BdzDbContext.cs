using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Data
{
    public class BdzDbContext(DbContextOptions options) : DbContext(options)
    {
    }
}
