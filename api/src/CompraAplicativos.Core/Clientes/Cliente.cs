using CompraAplicativos.Core.Clientes.ValueObjects;
using System;

namespace CompraAplicativos.Core.Clientes
{
    public class Cliente
    {
        public Cliente(
            string nome,
            string cpf,
            DateTime dataNascimento,
            string sexo,
            Endereco endereco)
        {
            this.Nome = nome;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Endereco = endereco;
        }

        public string Nome { get; }
        public string Cpf { get; }
        public DateTime DataNascimento { get; }
        public string Sexo { get; }
        public Endereco Endereco { get; }
    }
}
