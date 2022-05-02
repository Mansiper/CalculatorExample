using System.Collections.Generic;
using System.Threading.Tasks;
using Calculator.Dto;
using Calculator.Models;

namespace Calculator.Services
{
    public interface ICalcService
    {
        Task<(double? result, string error)> CalcAsync(CalcDto model, string ip);
        Task<CalcDto> GetCalculation(int id);
        Task<List<Calc>> GetCalculations(string ip);
    }
}