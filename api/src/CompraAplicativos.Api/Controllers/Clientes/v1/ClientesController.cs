using CompraAplicativos.Api.Presenters.Clientes;
using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CompraAplicativos.Api.Controllers.Aplicativos.v1
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(
            [FromServices] ICadastrarClienteUseCase useCase,
            [FromBody] CadastrarClienteInput input)
        {
            try
            {
                _logger.LogInformation("Ínicio do cadastro de cliente");

                CadastrarClienteOutput output = await useCase.Executar(input);

                return Ok(new ClientePresenter(output).Presenter());
            }
            catch (BusinessException businessException)
            {
                _logger.LogError(businessException, "Erro ao tentar cadastrar o cliente");

                return UnprocessableEntity();
            }
        }
    }
}
