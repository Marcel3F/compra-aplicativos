﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente
{
    public sealed class CadastrarClienteInput
    {
        [Required(ErrorMessage = "O nome é obrigatório", AllowEmptyStrings = false)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório", AllowEmptyStrings = false)]
        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório", AllowEmptyStrings = false)]
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}