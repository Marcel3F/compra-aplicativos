using System.Threading.Tasks;

namespace CompraAplicativos.Core.Clientes
{
    public interface IClienteRepository
    {
        Task<Cliente> Cadastrar(Cliente cliente);
        Task<bool> VerificarClienteExistePorCpf(string cpf);
        Task<Cliente> ObterClientePorCpf(string cpf);
        Task<Cliente> ObterClientePorId(string clienteId);
    }
}
