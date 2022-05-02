using System.Collections.Generic;
using System.Threading.Tasks;
using Calculator.Models;

namespace Calculator.Db.Repositories
{
    public interface ICalcRepository
    {
        Task<List<Calc>> GetCalculationsAsync(string ip);
        Task<Calc> GetCalculationAsync(int id);
        Task AddCalculationAsync(Calc calculation);
    }
}