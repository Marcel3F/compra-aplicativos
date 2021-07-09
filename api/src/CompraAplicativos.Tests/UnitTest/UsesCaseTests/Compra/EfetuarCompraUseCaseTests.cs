using Bogus;
using CompraAplicativos.Application.MessageBroker;
using CompraAplicativos.Application.UseCases.Compra.EfetuarCompra;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
using CompraAplicativos.Tests.UnitTest.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CompraAplicativos.Tests.UnitTest.UsesCaseTests.Compra
{
    public sealed class EfetuarCompraUseCaseTests
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAplicativoRepository _aplicativoRepository;
        private readonly IProcessaCompraSender _processaCompraSender;
        private readonly ILogger<EfetuarCompraUseCase> _logger;

        private readonly Mock _mock;

        public EfetuarCompraUseCaseTests()
        {
            _compraRepository = Substitute.For<ICompraRepository>();
            _clienteRepository = Substitute.For<IClienteRepository>();
            _aplicativoRepository = Substitute.For<IAplicativoRepository>();
            _processaCompraSender = Substitute.For<IProcessaCompraSender>();

            _mock = new Mock();

            _logger = Substitute.For<ILogger<EfetuarCompraUseCase>>();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeCompraEfetuadaComSucesso()
        {
            //Arrange
            EfetuarCompraUseCase useCase = new EfetuarCompraUseCase(
                _compraRepository,
                _clienteRepository,
                _aplicativoRepository,
                _processaCompraSender,
                _logger);

            EfetuarCompraInput input = new Faker<EfetuarCompraInput>("pt_BR")
                .CustomInstantiator(faker => new EfetuarCompraInput()
                {
                    AplicativoId = faker.Lorem.Letter(10),
                    ClienteId = faker.Lorem.Letter(10),
                    ModoPagamento = 1,
                    Valor = faker.Random.Decimal()
                })
                .Generate();


            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarAplicativo());
            _compraRepository.Registrar(Arg.Any<Core.Compras.Compra>()).ReturnsForAnyArgs(_mock.GerarCompra((ModoPagamento)input.ModoPagamento));

            Action registraCompra = () => _compraRepository.Received(1).Registrar(Arg.Any<Core.Compras.Compra>());

            //Act
            EfetuarCompraOutput output = await useCase.Executar(input);

            //Assert
            registraCompra.Should().NotThrow<ReceivedCallsException>();
            output.Should().NotBeNull();
        }
    }
}
