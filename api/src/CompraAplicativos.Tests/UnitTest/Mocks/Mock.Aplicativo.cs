using Bogus;
using CompraAplicativos.Core.Aplicativos;
using System;

namespace CompraAplicativos.Tests.UnitTest.Mocks
{
    public partial class Mock
    {
        public Aplicativo GerarAplicativo()
        {
            Aplicativo aplicativoMock = new Faker<Aplicativo>()
                .CustomInstantiator(faker => new Aplicativo(
                    faker.Lorem.Letter(10),
                    faker.Commerce.Product(),
                    Convert.ToDecimal(faker.Commerce.Price())))
                .Generate();

            return aplicativoMock;
        }
    }
}
