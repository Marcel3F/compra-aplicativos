using CompraAplicativos.Api.Presenters.Aplicativos;
using CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompraAplicativos.Api.Controllers.Aplicativos.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AplicativosController : ControllerBase
    {
        private readonly ILogger _logger;

        public AplicativosController(
            ILogger<AplicativosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IObterAplicativosUseCase useCase)
        {
            _logger.LogInformation("Obter lita de aplicativos");

            IEnumerable<ObterAplicativosOutput> output = await useCase.Executar();

            return Ok(new AplicativosPresenter(output).Presenter());
        }
    }
}
