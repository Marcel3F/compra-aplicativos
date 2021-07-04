using CompraAplicativos.Core.Clientes.ValueObjects;
using System;

namespace CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente
{
    public class CadastrarClienteOutput
    {
        public CadastrarClienteOutput(Core.Clientes.Cliente cliente)
        {
            Nome = cliente.Nome;
            Cpf = cliente.Cpf;
            DataNascimento = cliente.DataNascimento;
            Sexo = cliente.Sexo;
            Endereco = cliente.Endereco;
        }

        public string Nome { get; }
        public string Cpf { get; }
        public DateTime DataNascimento { get; }
        public string Sexo { get; }
        public Endereco Endereco { get; }
    }
}