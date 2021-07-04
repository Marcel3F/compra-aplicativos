namespace CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos
{
    public class ObterAplicativosOutput
    {
        public ObterAplicativosOutput(
            string nome,
            decimal valor)
        {
            Nome = nome;
            Valor = valor;
        }

        public string Nome { get; }
        public decimal Valor { get; }
    }
}
