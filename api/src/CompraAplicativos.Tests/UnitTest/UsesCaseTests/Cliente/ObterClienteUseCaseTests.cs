using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Cliente.ObterCliente;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Tests.UnitTest.Mocks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CompraAplicativos.Tests.UnitTest.UsesCaseTests.Cliente
{
    public sealed class ObterClienteUseCaseTests
    {
        private const string Cpf = "30030030030";
        private readonly IClienteRepository _clienteRepository;

        private readonly Mock _mock;

        public ObterClienteUseCaseTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();

            _mock = new Mock();
        }

        [Fact]
        public async Task ObterAplicativosUseCase_ValidarSeClienteObtidoComSucesso()
        {
            //Arrange
            ObterClienteUseCase useCase = new ObterClienteUseCase(
                _clienteRepository);

            _clienteRepository.ObterClientePorCpf(Arg.Any<string>()).ReturnsForAnyArgs(_mock.GerarCliente());

            //Act
            ObterClienteOutput output = await useCase.Executar(Cpf);

            //Assert
            output.Should().NotBeNull();
        }

        [Fact]
        public async Task ObterAplicativosUseCase_ValidarSeClienteNaoCadastrado()
        {
            //Arrange
            ObterClienteUseCase useCase = new ObterClienteUseCase(
                _clienteRepository);

            _clienteRepository.ObterClientePorCpf(Arg.Any<string>()).ReturnsNull();

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(Cpf);

            //Assert
            await executarUseCase.Should().ThrowAsync<NotFoundException>();
        }
    }
}
