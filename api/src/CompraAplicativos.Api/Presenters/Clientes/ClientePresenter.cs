using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;

namespace CompraAplicativos.Api.Presenters.Clientes
{
    public sealed class ClientePresenter
    {
        private readonly CadastrarClienteOutput _output;

        public ClientePresenter(CadastrarClienteOutput output)
        {
            _output = output;
        }

        public object Presenter()
        {
            return _output;
        }
    }
}
