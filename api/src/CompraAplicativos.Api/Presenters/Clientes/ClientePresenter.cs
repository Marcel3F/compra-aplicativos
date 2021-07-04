using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;

namespace CompraAplicativos.Api.Presenters.Clientes
{
    public sealed class ClientePresenter
    {
        private readonly object _output;

        public ClientePresenter(object output)
        {
            _output = output;
        }

        public object Presenter()
        {
            return _output;
        }
    }
}
