namespace CompraAplicativos.Core.Compras.ValueObjects
{
    public class Cartao
    {
        public Cartao(string numero)
        {
            Numero = numero;
        }

        public string Numero { get; }
    }
}
