﻿using CompraAplicativos.Api.Presenters.Clientes;
using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;
using CompraAplicativos.Application.UseCases.Cliente.ObterCliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
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
                _logger.LogInformation("Ínicio do cadastro de cliente");

                CadastrarClienteOutput output = await useCase.Executar(input);

                return new ObjectResult(new ClientePresenter(output).Presenter()) { StatusCode = (int)HttpStatusCode.Created };
            }
            catch (BusinessException businessException)
            {
                _logger.LogError(businessException, "Erro ao tentar cadastrar o cliente");

                Dictionary<string, IList<string>> erros = new Dictionary<string, IList<string>>
                {
                    [nameof(BusinessException)] = new List<string>()
                };
                erros[nameof(BusinessException)].Add(businessException.Message);
                return new BadRequestObjectResult(error: new ValidationProblemDetails(erros.ToDictionary(item => item.Key, item => item.Value.ToArray())));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro inesperado ao tentar obter o cliente");
                return new ObjectResult(exception) { StatusCode = (int)HttpStatusCode.InternalServerError };
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
                _logger.LogInformation("Ínicio do obter cliente por cpf");

                ObterClienteOutput output = await useCase.Executar(cpf);

                return Ok(new ClientePresenter(output).Presenter());
            }
            catch (NotFoundException notfoundException)
            {
                _logger.LogError(notfoundException, "Erro ao tentar obter o cliente");

                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro inesperado ao tentar obter o cliente");
                return new ObjectResult(exception) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
