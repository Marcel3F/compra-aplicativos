using CompraAplicativos.Core.Clientes.ValueObjects;
using System;

namespace CompraAplicativos.Application.UseCases.Cliente.ObterCliente
{
    public sealed class ObterClienteOutput
    {
        public ObterClienteOutput(Core.Clientes.Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Cpf = cliente.Cpf;
            DataNascimento = cliente.DataNascimento;
            Sexo = cliente.Sexo;
            Endereco = cliente.Endereco;
        }

        public string Id { get; }
        public string Nome { get; }
        public string Cpf { get; }
        public DateTime DataNascimento { get; }
        public string Sexo { get; }
        public Endereco Endereco { get; }
    }
}