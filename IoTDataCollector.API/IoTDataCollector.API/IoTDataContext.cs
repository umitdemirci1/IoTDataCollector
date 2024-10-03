using IoTDataCollector.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTDataCollector.API
{
    public class IoTDataContext : DbContext
    {
        public IoTDataContext(DbContextOptions<IoTDataContext> options) : base(options)
        {
        }

        public DbSet<IoTData> IoTData { get; set; }
    }
}
