using CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos;
using System.Collections.Generic;

namespace CompraAplicativos.Api.Presenters.Aplicativos
{
    public sealed class AplicativosPresenter
    {
        private readonly IEnumerable<ObterAplicativosOutput> output;

        public AplicativosPresenter(IEnumerable<ObterAplicativosOutput> output)
        {
            this.output = output;
        }

        public object Presenter()
        {
            return output;
        }
    }
}
