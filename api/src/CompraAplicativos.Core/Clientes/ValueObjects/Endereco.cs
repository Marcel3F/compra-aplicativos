namespace CompraAplicativos.Core.Clientes.ValueObjects
{
    public class Endereco
    {
        public Endereco(
            string logradouro,
            string numero,
            string complemento,
            string cep,
            string cidade,
            string uf)
        {
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Cep = cep;
            this.Cidade = cidade;
            this.Uf = uf;
        }

        public string Logradouro { get; }
        public string Numero { get; }
        public string Complemento { get; }
        public string Cep { get; }
        public string Cidade { get; }
        public string Uf { get; }
    }
}
