using Bogus;
using CompraAplicativos.Application.Exceptions;
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
using NSubstitute.ReturnsExtensions;
using System;
using System.ComponentModel.DataAnnotations;
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
                    Valor = faker.Random.Decimal(),
                    Cartao = faker.Finance.CreditCardNumber()
                })
                .Generate();

            
            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarAplicativo());
            _compraRepository.Registrar(Arg.Any<Core.Compras.Compra>()).ReturnsForAnyArgs(_mock.GerarCompra((ModoPagamento)input.ModoPagamento));

            Action registraCompra = () => _compraRepository.Received(1).Registrar(Arg.Any<Core.Compras.Compra>());

            //Act
            bool inputValido = Validator.TryValidateObject(input, new ValidationContext(input, null, null), null, true);
            EfetuarCompraOutput output = await useCase.Executar(input);

            //Assert
            inputValido.Should().BeTrue();
            registraCompra.Should().NotThrow<ReceivedCallsException>();
            output.Should().NotBeNull();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeModoPagamentoCompraIndisponivel()
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
                    ModoPagamento = 2,
                    Valor = faker.Random.Decimal(),
                    Cartao = faker.Finance.CreditCardNumber()
                })
                .Generate();

            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarAplicativo());

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            await executarUseCase.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeClienteNaoPossuiCadastro()
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
                    Valor = faker.Random.Decimal(),
                    Cartao = faker.Finance.CreditCardNumber()
                })
                .Generate();

            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsNull();
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarAplicativo());

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            await executarUseCase.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeAplicativoNaoPossuiCadastro()
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
                    Valor = faker.Random.Decimal(),
                    Cartao = faker.Finance.CreditCardNumber()
                })
                .Generate();

            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsNull();

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            await executarUseCase.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeCartaoInvalido()
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
                    Valor = faker.Random.Decimal(),
                    Cartao = "1111111111111111111111111111"
                })
                .Generate();

            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsNull();

            //Act
            bool inputValido = Validator.TryValidateObject(input, new ValidationContext(input, null, null), null, true);
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            inputValido.Should().BeFalse();
            await executarUseCase.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task EfetuarCompraUseCase_ValidarSeCompraFoiRegistradaComoFalha()
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
                    Valor = faker.Random.Decimal(),
                    Cartao = faker.Finance.CreditCardNumber()
                })
                .Generate();

            _clienteRepository.ObterClientePorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());
            _aplicativoRepository.ObterAplicativoPorId(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarAplicativo());
            _compraRepository.Registrar(Arg.Any<Core.Compras.Compra>()).ReturnsForAnyArgs(_mock.GerarCompra((ModoPagamento)input.ModoPagamento));

            _processaCompraSender.Enviar(Arg.Any<Core.Compras.Compra>()).Returns(x => { throw new Exception(); });

            Action alterarStatusParaFalha = () => _compraRepository.Received(1).AlterarStatusCompraParaFalha(Arg.Any<string>());

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            await executarUseCase.Should().ThrowAsync<Exception>();
            alterarStatusParaFalha.Should().NotThrow<ReceivedCallsException>();
        }
    }
}
