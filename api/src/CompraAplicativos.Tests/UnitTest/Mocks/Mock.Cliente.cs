using Bogus;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Core.Clientes.ValueObjects;

namespace CompraAplicativos.Tests.UnitTest.Mocks
{
    public partial class Mock
    {
        public Cliente GerarCliente()
        {
            Cliente clienteMock = new Faker<Cliente>()
                .CustomInstantiator(faker => new Cliente(
                    faker.Lorem.Letter(10),
                    faker.Person.FullName,
                    faker.Random.Int(11).ToString(),
                    faker.Person.DateOfBirth,
                    "F",
                    new Endereco(
                        faker.Address.StreetAddress(),
                        faker.Random.Int(3).ToString(),
                        faker.Address.SecondaryAddress(),
                        faker.Address.ZipCode(),
                        faker.Address.City(),
                        faker.Address.StateAbbr()
                    )))
                .Generate();

            return clienteMock;
        }
    }
}
