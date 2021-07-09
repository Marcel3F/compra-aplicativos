using Bogus;
using CompraAplicativos.Core.Compras;
using CompraAplicativos.Core.Compras.Enums;
using System;

namespace CompraAplicativos.Tests.UnitTest.Mocks
{
    public partial class Mock
    {
        public Compra GerarCompra(ModoPagamento modoPagamento)
        {
            Compra compraMock = new Faker<Compra>()
                .CustomInstantiator(faker => new Compra(
                    faker.Lorem.Letter(10),
                    GerarCliente(),
                    GerarAplicativo(),
                    Convert.ToDecimal(faker.Commerce.Price()),
                    modoPagamento))
                .Generate();

            return compraMock;
        }
    }
}
