using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Calculator.Code;
using Calculator.Db.Repositories;
using Calculator.Dto;
using Calculator.Exceptions;
using Calculator.Models;

namespace Calculator.Services
{
    public class CalcService: ICalcService
    {
        private readonly ICalcRepository _repository;

        public CalcService(ICalcRepository repository)
        {
            _repository = repository;
        }

        public async Task<(double? result, string error)> CalcAsync(CalcDto model, string ip)
        {
            double? result;
            long time;

            try
            {
                (result, time) = Solver.Solve(model.Expression);
            }
            catch (CalculationException e)
            {
                return (null, "Calculation error: " + e.Message);
            }
            catch (Exception e)
            {
                return (null, "Error: " + e.Message);
            }

            var calculation = new Calc
            {
                CalcDate = DateTime.UtcNow,
                Expression = model.Expression,
                Result = result.Value,
                CalculationTime = time,
                ClientIp = ip,
            };
            await _repository.AddCalculationAsync(calculation);

            return (result, null);
        }

        public async Task<CalcDto> GetCalculation(int id)
        {
            var result = await _repository.GetCalculationAsync(id);
            if (result == null)
                return null;

            return new CalcDto
            {
                Expression = result.Expression,
                Result = result.Result,
            };
        }

        public async Task<List<Calc>> GetCalculations(string ip) =>
            await _repository.GetCalculationsAsync(ip);
    }
}