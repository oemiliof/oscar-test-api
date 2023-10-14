using Microsoft.AspNetCore.Mvc;
using prueba_oscar_api.BLL.EconomicIndicators;
using prueba_oscar_api.DTO.Bll.Results;

namespace prueba_oscar_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndicatorController : ControllerBase
    {
        private readonly EconomicIndicatorBll _economicIndicatorBll;

        public IndicatorController(EconomicIndicatorBll economicIndicatorBll)
        {
            _economicIndicatorBll = economicIndicatorBll;
        }

        [HttpGet("get-indicator")]
        public async Task<IActionResult> GetIndicator()
        {
            try
            {
                var result = await _economicIndicatorBll.GetIndicator();

                if (result is SuccessResultDto successResult && successResult.statusCode == 0)
                {
                    return Ok(successResult);
                }
                else if (result is ErrorResultDto errorResult)
                {
                    return StatusCode(500, errorResult);
                }
                else
                {
                    // Handle other cases or unknown results here
                    return StatusCode(500, "Unknown error occurred.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a 500 status code
                return StatusCode(500, "An exception occurred.");
            }
        }
    }
}