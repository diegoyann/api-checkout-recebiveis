using Microsoft.EntityFrameworkCore;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Infrastructure.Data;

namespace Size.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de carrinho de antecipação
/// </summary>
public class CarrinhoAntecipacaoRepo : GeralRepo<CarrinhoAntecipacao>, ICarrinhoAntecipacaoRepo
{
    /// <summary>
    /// Construtor do repositório de carrinho de antecipação
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public CarrinhoAntecipacaoRepo(SizeContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém o carrinho ativo de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo da empresa ou null</returns>
    public async Task<CarrinhoAntecipacao?> ObterCarrinhoAtivoPorEmpresaAsync(int empresaId)
    {
        return await _dbSet
            .Include(c => c.Empresa)
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && c.Ativo);
    }

    /// <summary>
    /// Obtém um carrinho com seus itens carregados
    /// </summary>
    /// <param name="id">Identificador do carrinho</param>
    /// <returns>Carrinho com itens carregados ou null</returns>
    public async Task<CarrinhoAntecipacao?> ObterComItensAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Empresa)
            .Include(c => c.Itens)
                .ThenInclude(i => i.NotaFiscal)
                    .ThenInclude(nf => nf.Empresa)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Obtém o carrinho ativo de uma empresa com seus itens carregados
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo com itens carregados ou null</returns>
    public async Task<CarrinhoAntecipacao?> ObterCarrinhoAtivoComItensAsync(int empresaId)
    {
        return await _dbSet
            .Include(c => c.Empresa)
            .Include(c => c.Itens)
                .ThenInclude(i => i.NotaFiscal)
                    .ThenInclude(nf => nf.Empresa)
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && c.Ativo);
    }
}