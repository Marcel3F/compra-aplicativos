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
                _logger.LogInformation("Ínicio do cadastro de cliente");

                EfetuarCompraOutput output = await useCase.Executar(input);

                return new ObjectResult(new CompraPresenter(output).Presenter()) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (NotFoundException notfoundException)
            {
                _logger.LogError(notfoundException, "Erro ao tentar efetuar compra");

                return new NotFoundObjectResult(new ValidationProblemDetails(notfoundException.ToDictionary()));
            }
            catch (BusinessException businessException)
            {
                _logger.LogError(businessException, "Erro ao tentar efetuar compra");

                return new BadRequestObjectResult(error: new ValidationProblemDetails(businessException.ToDictionary()));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro inesperado ao tentar efetuar compra");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
