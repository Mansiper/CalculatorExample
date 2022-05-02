using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Calculator.Dto;
using Calculator.Services;

namespace Calculator.Controllers
{
    public class CalcController : Controller
    {
        private readonly ILogger<CalcController> _logger;
        private readonly ICalcService _service;

        private string IP => HttpContext.Connection.RemoteIpAddress.ToString();

        public CalcController(ILogger<CalcController> logger, ICalcService service)
        {
            _logger = logger;
            _service = service;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int? id)
        {
            CalcDto model = null;

            if (id.HasValue)
            {
                _logger.LogDebug($"Opened {id}");
                model = await _service.GetCalculation(id.Value);
                if (model == null)
                    return RedirectToAction(nameof(Calc));
            }

            ViewBag.PrevResults = await _service.GetCalculations(IP);

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Calc(CalcDto model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), model);

            var (result, error) = await _service.CalcAsync(model, IP);

            if (error != null)
                ModelState.AddModelError(nameof(CalcDto.Expression), error);

            model.Result = result;

            return View(nameof(Index), model);
        }
    }
}