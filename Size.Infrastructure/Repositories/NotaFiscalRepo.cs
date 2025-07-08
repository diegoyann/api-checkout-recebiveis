using Microsoft.EntityFrameworkCore;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Infrastructure.Data;

namespace Size.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de notas fiscais
/// </summary>
public class NotaFiscalRepo : GeralRepo<NotaFiscal>, INotaFiscalRepo
{
    /// <summary>
    /// Construtor do repositório de notas fiscais
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public NotaFiscalRepo(SizeContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todas as notas fiscais de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Lista de notas fiscais da empresa</returns>
    public async Task<IEnumerable<NotaFiscal>> ObterPorEmpresaAsync(int empresaId)
    {
        return await _dbSet
            .Include(nf => nf.Empresa)
            .Where(nf => nf.EmpresaId == empresaId)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém uma nota fiscal por número e empresa
    /// </summary>
    /// <param name="numero">Número da nota fiscal</param>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Nota fiscal encontrada ou null</returns>
    public async Task<NotaFiscal?> ObterPorNumeroEEmpresaAsync(string numero, int empresaId)
    {
        return await _dbSet
            .Include(nf => nf.Empresa)
            .FirstOrDefaultAsync(nf => nf.Numero == numero && nf.EmpresaId == empresaId);
    }

    /// <summary>
    /// Verifica se já existe uma nota fiscal com o número informado para a empresa
    /// </summary>
    /// <param name="numero">Número da nota fiscal</param>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>True se existe, False caso contrário</returns>
    public async Task<bool> ExistePorNumeroEEmpresaAsync(string numero, int empresaId)
    {
        return await _dbSet
            .AnyAsync(nf => nf.Numero == numero && nf.EmpresaId == empresaId);
    }

    /// <summary>
    /// Obtém uma nota fiscal por ID com a empresa carregada
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Nota fiscal encontrada ou null</returns>
    public override async Task<NotaFiscal?> ObterPorIdAsync(int id)
    {
        return await _dbSet
            .Include(nf => nf.Empresa)
            .FirstOrDefaultAsync(nf => nf.Id == id);
    }
}