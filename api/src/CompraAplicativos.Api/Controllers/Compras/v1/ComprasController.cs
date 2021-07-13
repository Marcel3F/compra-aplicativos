using CompraAplicativos.Api.Common;
using CompraAplicativos.Api.Presenters.Clientes;
using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Compra.EfetuarCompra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CompraAplicativos.Api.Controllers.Compras.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/compras")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ILogger _logger;

        public ComprasController(
            ILogger<ComprasController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Efetuar compra",
            Description = "Registra compra de um aplicativo",
            OperationId = "EfetuarCompra"
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EfetuarCompraOutput))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(
            [FromServices] IEfetuarCompraUseCase useCase,
            [FromBody] EfetuarCompraInput input)
        {
            try
            {
                _logger.LogInformation("Efetuar compra: Aplicativo {AplicativoId} Cliente {ClienteId} - Início", input.AplicativoId, input.ClienteId);

                EfetuarCompraOutput output = await useCase.Executar(input);

                _logger.LogInformation("Efetuar compra: Aplicativo {AplicativoId} Cliente {ClienteId} - Fim", input.AplicativoId, input.ClienteId);

                return new ObjectResult(new CompraPresenter(output).Presenter()) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (NotFoundException notfoundException)
            {
                _logger.LogError(notfoundException, "Efetuar compra: Aplicativo {AplicativoId} Cliente {ClienteId} - Erro", input.AplicativoId, input.ClienteId);

                return new NotFoundObjectResult(new ValidationProblemDetails(notfoundException.ToDictionary()));
            }
            catch (BusinessException businessException)
            {
                _logger.LogError(businessException, "Efetuar compra: Aplicativo {AplicativoId} Cliente {ClienteId} - Erro", input.AplicativoId, input.ClienteId);

                return new BadRequestObjectResult(error: new ValidationProblemDetails(businessException.ToDictionary()));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Efetuar compra: Aplicativo {AplicativoId} Cliente {ClienteId} - Erro inesperado", input.AplicativoId, input.ClienteId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
