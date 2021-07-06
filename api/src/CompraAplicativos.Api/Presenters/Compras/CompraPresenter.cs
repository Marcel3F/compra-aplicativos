namespace CompraAplicativos.Api.Presenters.Clientes
{
    public sealed class CompraPresenter
    {
        private readonly object _output;

        public CompraPresenter(object output)
        {
            _output = output;
        }

        public object Presenter()
        {
            return _output;
        }
    }
}
