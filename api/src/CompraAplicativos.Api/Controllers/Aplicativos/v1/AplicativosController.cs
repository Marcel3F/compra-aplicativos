using CompraAplicativos.Api.Presenters.Aplicativos;
using CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CompraAplicativos.Api.Controllers.Aplicativos.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/aplicativos")]
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
        [SwaggerOperation(
            Summary = "Obter aplicativos",
            Description = "Obtêm todos os aplicativos cadastrados",
            OperationId = "ObterAplicativos"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ObterAplicativosOutput>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromServices] IObterAplicativosUseCase useCase)
        {
            try
            {
                _logger.LogInformation("Obter lita de aplicativos");

                IEnumerable<ObterAplicativosOutput> output = await useCase.Executar();

                return Ok(new AplicativosPresenter(output).Presenter());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro inesperado ao tentar recuperar aplicativos");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
