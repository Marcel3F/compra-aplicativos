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
        public string NumeroCartao { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = MensagensValidacao.CampoInvalido)]
        public string CCVCartao { get; set; }

        [StringLength(4, MinimumLength = 4, ErrorMessage = MensagensValidacao.CampoInvalido)]
        public string ValidadeCartao { get; set; }

        public bool GuardarCartao { get; set; }
    }
}