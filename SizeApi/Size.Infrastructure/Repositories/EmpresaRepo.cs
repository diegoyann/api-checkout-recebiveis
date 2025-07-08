using Microsoft.EntityFrameworkCore;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Domain.Interfaces.Repositories;
using Size.Infrastructure.Data;

namespace Size.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de empresas
/// </summary>
public class EmpresaRepo : GeralRepo<Empresa>, IEmpresaRepo
{
    /// <summary>
    /// Construtor do repositório de empresas
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public EmpresaRepo(SizeContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    public async Task<Empresa?> ObterPorCnpjAsync(string cnpj)
    {
        return await _dbSet
                  .FirstOrDefaultAsync(e => e.Cnpj == cnpj);
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    /// <param name="cnpj">CNPJ a ser verificado</param>
    /// <returns>True se existe, False caso contrário</returns>
    public async Task<bool> ExistePorCnpjAsync(string cnpj)
    {
        return await _dbSet
                  .AnyAsync(e => e.Cnpj == cnpj);
    }
}