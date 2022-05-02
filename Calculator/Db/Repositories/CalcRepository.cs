using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Db.Repositories
{
    public class CalcRepository : ICalcRepository
    {
        private readonly CalcContext _context;

        public CalcRepository(CalcContext context) =>
            _context = context;

        public async Task<List<Calc>> GetCalculationsAsync(string ip) =>
            await _context.Calcs
                .AsNoTracking()
                .Where(c => c.ClientIp == ip)
                .ToListAsync();

        public async Task<Calc> GetCalculationAsync(int id) =>
            await _context.Calcs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddCalculationAsync(Calc calculation)
        {
            _context.Calcs.Add(calculation);
            await _context.SaveChangesAsync();
        }
    }
}