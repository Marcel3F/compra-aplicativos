using Bogus;
using CompraAplicativos.Application.Exceptions;
using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Tests.UnitTest.Mocks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace CompraAplicativos.Tests.UnitTest.UsesCaseTests.Cliente
{
    public sealed class CadastrarClienteUseCaseTests
    {
        private readonly IClienteRepository _clienteRepository;

        private readonly Mock _mock;

        public CadastrarClienteUseCaseTests()
        {
            _clienteRepository = Substitute.For<IClienteRepository>();

            _mock = new Mock();
        }

        [Fact]
        public async Task CadastrarClienteUseCase_ValidarSeClienteCadastradoComSucesso()
        {
            //Arrange
            CadastrarClienteUseCase useCase = new CadastrarClienteUseCase(
                _clienteRepository);

            CadastrarClienteInput input = new Faker<CadastrarClienteInput>("pt_BR")
                .CustomInstantiator(faker => new CadastrarClienteInput()
                {
                    Nome = faker.Person.FullName,
                    Cpf = faker.Random.Replace("###########"),
                    DataNascimento = faker.Person.DateOfBirth,
                    Sexo = "F",
                    Logradouro = faker.Address.StreetAddress(),
                    Numero = faker.Random.Int(3).ToString(),
                    Complemento = faker.Address.SecondaryAddress(),
                    Cep = faker.Address.ZipCode(),
                    Cidade = faker.Address.City(),
                    Uf = faker.Address.StateAbbr()
                })
                .Generate();

            _clienteRepository.VerificarClienteExistePorCpf(Arg.Any<string>()).ReturnsForAnyArgs(false);
            _clienteRepository.Cadastrar(Arg.Any<Core.Clientes.Cliente>()).ReturnsForAnyArgs(_mock.GerarCliente());

            Action cadastrarCliente = () => _clienteRepository.Received(1).Cadastrar(Arg.Any<Core.Clientes.Cliente>());

            //Act
            bool inputValido = Validator.TryValidateObject(input, new ValidationContext(input, null, null), null, true);
            CadastrarClienteOutput output = await useCase.Executar(input);

            //Assert
            inputValido.Should().BeTrue();
            cadastrarCliente.Should().NotThrow<ReceivedCallsException>();
            output.Should().NotBeNull();
        }

        [Fact]
        public async Task CadastrarClienteUseCase_ValidarSeClienteJaPossuiCadastro()
        {
            //Arrange
            CadastrarClienteUseCase useCase = new CadastrarClienteUseCase(
                _clienteRepository);

            CadastrarClienteInput input = new Faker<CadastrarClienteInput>("pt_BR")
                .CustomInstantiator(faker => new CadastrarClienteInput()
                {
                    Nome = faker.Person.FullName,
                    Cpf = faker.Random.Replace("###########"),
                    DataNascimento = faker.Person.DateOfBirth,
                    Sexo = "F",
                    Logradouro = faker.Address.StreetAddress(),
                    Numero = faker.Random.Int(3).ToString(),
                    Complemento = faker.Address.SecondaryAddress(),
                    Cep = faker.Address.ZipCode(),
                    Cidade = faker.Address.City(),
                    Uf = faker.Address.StateAbbr()
                })
                .Generate();

            _clienteRepository.VerificarClienteExistePorCpf(Arg.Any<string>()).ReturnsForAnyArgs(true);

            //Act
            Func<Task> executarUseCase = () => useCase.Executar(input);

            //Assert
            await executarUseCase.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task CadastrarClienteUseCase_ValidarSeCpfInvalido()
        {
            //Arrange
            CadastrarClienteUseCase useCase = new CadastrarClienteUseCase(
                _clienteRepository);

            CadastrarClienteInput input = new Faker<CadastrarClienteInput>("pt_BR")
                .CustomInstantiator(faker => new CadastrarClienteInput()
                {
                    Nome = faker.Person.FullName,
                    Cpf = faker.Random.Replace("##########"),
                    DataNascimento = faker.Person.DateOfBirth,
                    Sexo = "F",
                    Logradouro = faker.Address.StreetAddress(),
                    Numero = faker.Random.Int(3).ToString(),
                    Complemento = faker.Address.SecondaryAddress(),
                    Cep = faker.Address.ZipCode(),
                    Cidade = faker.Address.City(),
                    Uf = faker.Address.StateAbbr()
                })
                .Generate();

            _clienteRepository.VerificarClienteExistePorCpf(Arg.Any<string>()).ReturnsForAnyArgs(false);
            _clienteRepository.Cadastrar(Arg.Any<Core.Clientes.Cliente>()).ReturnsForAnyArgs(_mock.GerarCliente());

            Action cadastrarCliente = () => _clienteRepository.Received(1).Cadastrar(Arg.Any<Core.Clientes.Cliente>());

            //Act
            bool inputValido = Validator.TryValidateObject(input, new ValidationContext(input, null, null), null, true);
            CadastrarClienteOutput output = await useCase.Executar(input);

            //Assert
            inputValido.Should().BeFalse();
            cadastrarCliente.Should().NotThrow<ReceivedCallsException>();
            output.Should().NotBeNull();
        }
    }
}
