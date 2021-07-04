namespace CompraAplicativos.Core.Compras.ValueObjects
{
    public sealed class Item
    {
        public Item(
            string aplicativo,
            decimal valor)
        {
            Aplicativo = aplicativo;
            Valor = valor;
        }

        public string Aplicativo { get; }
        public decimal Valor { get; }
    }
}
