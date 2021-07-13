using CompraAplicativos.Api.Common;
using CompraAplicativos.Api.Presenters.Clientes;
using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;
using CompraAplicativos.Application.UseCases.Cliente.ObterCliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CompraAplicativos.Api.Controllers.Clientes.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ClientesController(
            ILogger<ClientesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar cliente",
            Description = "Cadastro de cliente",
            OperationId = "CadastrarCliente"
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CadastrarClienteOutput))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(
            [FromServices] ICadastrarClienteUseCase useCase,
            [FromBody] CadastrarClienteInput input)
        {
            try
            {
                _logger.LogInformation("Cadastro de cliente {Cpf}: Início", input.Cpf);

                CadastrarClienteOutput output = await useCase.Executar(input);

                _logger.LogInformation("Cadastro de cliente {Cpf}: Fim", input.Cpf);

                return new ObjectResult(new ClientePresenter(output).Presenter()) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (BusinessException businessException)
            {
                _logger.LogError(businessException, "Cadastro de cliente {Cpf}: Erro de validação", input.Cpf);

                return new BadRequestObjectResult(error: new ValidationProblemDetails(businessException.ToDictionary()));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cadastro de cliente {Cpf}: Erro inesperado", input.Cpf);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{cpf}")]
        [SwaggerOperation(
            Summary = "Obter cliente",
            Description = "Buscar dados do cliente",
            OperationId = "ObterCliente"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CadastrarClienteOutput))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(
            [FromServices] IObterClienteUseCase useCase,
            [FromRoute][Required][RegularExpression(@"^[0-9]{11}$", ErrorMessage = "CPF inválido")] string cpf)
        {
            try
            {
                _logger.LogInformation("Obter cliente {cpf}: Início", cpf);

                ObterClienteOutput output = await useCase.Executar(cpf);

                _logger.LogInformation("Obter cliente {cpf}: Fim", cpf);
                return Ok(new ClientePresenter(output).Presenter());
            }
            catch (NotFoundException notfoundException)
            {
                _logger.LogError(notfoundException, "Obter cliente {cpf}: Não encontrado", cpf);

                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Obter cliente {cpf}: Erro inesperado", cpf);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
