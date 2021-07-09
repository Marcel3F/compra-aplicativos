using CompraAplicativos.Core.Clientes.ValueObjects;
using System;

namespace CompraAplicativos.Core.Clientes
{
    public class Cliente
    {

        public Cliente(
            string id,
            string nome,
            string cpf,
            DateTime dataNascimento,
            string sexo,
            Endereco endereco)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Sexo = sexo;
            DataNascimento = dataNascimento;

            AtribuirEndereco(endereco);
        }

        public Cliente(
            string nome,
            string cpf,
            DateTime dataNascimento,
            string sexo,
            Endereco endereco)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Sexo = sexo;

            AtribuirEndereco(endereco);
        }

        public Cliente(
            string id,
            string nome,
            string cpf)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
        }

        private void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public string Id { get; }
        public string Nome { get; }
        public string Cpf { get; }
        public DateTime DataNascimento { get; }
        public string Sexo { get; }
        public Endereco Endereco { get; private set; }
    }
}
