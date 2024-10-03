using IoTDataCollector.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTDataCollector.API.Services
{
    public class IoTDataService
    {
        private readonly IoTDataContext _context;

        public IoTDataService(IoTDataContext context)
        {
            _context = context;
        }

        public async Task SaveDataAsync(IoTData data)
        {
            _context.IoTData.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IoTData>> GetAllDataAsync()
        {
            return await _context.IoTData.ToListAsync();
        }
    }
}
