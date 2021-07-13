namespace CompraAplicativos.Core.Cartoes.ValueObjects
{
    public class Cartao
    {
        public Cartao(
            string numero,
            string ccv,
            string validade)
        {
            Numero = numero;
            Ccv = ccv;
            Validade = validade;
        }

        public string Numero { get; }
        public string Ccv { get; }
        public string Validade { get; }
    }
}
