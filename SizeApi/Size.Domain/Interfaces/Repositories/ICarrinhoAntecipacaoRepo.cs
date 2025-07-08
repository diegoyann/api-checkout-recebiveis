using Size.Domain.Entities;
using Size.Domain.Interfaces.Repositories;

namespace Size.Domain.Interfaces;

/// <summary>
/// Interface do repositório de carrinho de antecipação
/// </summary>
public interface ICarrinhoAntecipacaoRepo : IGeralRepo<CarrinhoAntecipacao>
{
    /// <summary>
    /// Obtém o carrinho ativo de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo da empresa ou null</returns>
    Task<CarrinhoAntecipacao?> ObterCarrinhoAtivoPorEmpresaAsync(int empresaId);
    
    /// <summary>
    /// Obtém um carrinho com seus itens carregados
    /// </summary>
    /// <param name="id">Identificador do carrinho</param>
    /// <returns>Carrinho com itens carregados ou null</returns>
    Task<CarrinhoAntecipacao?> ObterComItensAsync(int id);
    
    /// <summary>
    /// Obtém o carrinho ativo de uma empresa com seus itens carregados
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo com itens carregados ou null</returns>
    Task<CarrinhoAntecipacao?> ObterCarrinhoAtivoComItensAsync(int empresaId);
}