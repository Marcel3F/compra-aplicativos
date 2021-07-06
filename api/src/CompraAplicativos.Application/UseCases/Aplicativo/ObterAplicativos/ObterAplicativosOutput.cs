namespace CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos
{
    public class ObterAplicativosOutput
    {
        public ObterAplicativosOutput(
            string id,
            string nome,
            decimal valor)
        {
            Id = id;
            Nome = nome;
            Valor = valor;
        }

        public string Id { get; }
        public string Nome { get; }
        public decimal Valor { get; }
    }
}
