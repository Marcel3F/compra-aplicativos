namespace CompraAplicativos.Core.Aplicativos
{
    public class Aplicativo
    {
        public Aplicativo(
            string nome,
            decimal valor)
        {
            Nome = nome;
            Valor = valor;
        }

        public Aplicativo(
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
