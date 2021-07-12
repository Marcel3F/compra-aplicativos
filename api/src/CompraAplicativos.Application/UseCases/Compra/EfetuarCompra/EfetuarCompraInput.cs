using CompraAplicativos.Application.Common;
using System.ComponentModel.DataAnnotations;

namespace CompraAplicativos.Application.UseCases.Compra.EfetuarCompra
{
    public sealed class EfetuarCompraInput
    {
        [Required(ErrorMessage = MensagensValidacao.CampoObrigatorio, AllowEmptyStrings = false)]
        public string ClienteId { get; set; }

        [Required(ErrorMessage = MensagensValidacao.CampoObrigatorio, AllowEmptyStrings = false)]
        public string AplicativoId { get; set; }
        public decimal Valor { get; set; }
        public int ModoPagamento { get; set; }

        [Required(ErrorMessage = MensagensValidacao.CampoObrigatorio, AllowEmptyStrings = false)]
        [CreditCard(ErrorMessage = MensagensValidacao.CampoInvalido)]
        public string Cartao { get; set; }

        public bool GuardarCartao { get; set; }
    }
}