using Size.Domain.Entities;

namespace Size.Domain.Interfaces.Repositories;

public interface IEmpresaRepo : IGeralRepo<Empresa>
{
    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    Task<Empresa?> ObterPorCnpjAsync(string cnpj);
    
    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    /// <param name="cnpj">CNPJ a ser verificado</param>
    /// <returns>True se existe, False caso contrário</returns>
    Task<bool> ExistePorCnpjAsync(string cnpj);
}