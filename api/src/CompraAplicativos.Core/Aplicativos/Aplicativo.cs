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

        public string Nome { get; }
        public decimal Valor { get; }
    }
}
