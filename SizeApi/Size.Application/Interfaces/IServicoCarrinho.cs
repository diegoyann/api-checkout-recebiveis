using Size.Application.DTOs;

namespace Size.Application.Interfaces;

/// <summary>
/// Interface do serviço de carrinho de antecipação
/// </summary>
public interface IServicoCarrinho
{
    /// <summary>
    /// Adiciona uma nota fiscal ao carrinho
    /// </summary>
    /// <param name="adicionarItemDto">Dados do item a ser adicionado</param>
    /// <returns>Carrinho atualizado</returns>
    Task<CarrinhoDto> AdicionarItemAsync(AdicionarItemCarrinhoDto adicionarItemDto);
    
    /// <summary>
    /// Remove uma nota fiscal do carrinho
    /// </summary>
    /// <param name="removerItemDto">Dados do item a ser removido</param>
    /// <returns>Carrinho atualizado</returns>
    Task<CarrinhoDto> RemoverItemAsync(RemoverItemCarrinhoDto removerItemDto);
    
    /// <summary>
    /// Obtém o carrinho ativo de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo da empresa ou null</returns>
    Task<CarrinhoDto?> ObterCarrinhoAtivoAsync(int empresaId);
    
    /// <summary>
    /// Obtém o carrinho ativo de uma empresa no formato de checkout
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho no formato de checkout ou null</returns>
    Task<CheckoutDto?> ObterCarrinhoFormatoCheckoutAsync(int empresaId);
    
    /// <summary>
    /// Realiza o checkout do carrinho calculando os valores de antecipação
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Resultado do checkout com valores calculados</returns>
    Task<CheckoutDto> RealizarCheckoutAsync(int empresaId);
    
    /// <summary>
    /// Limpa o carrinho de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Task</returns>
    Task LimparCarrinhoAsync(int empresaId);
    
    /// <summary>
    /// Atualiza o carrinho substituindo todos os itens pelos especificados
    /// </summary>
    /// <param name="atualizarCarrinhoDto">Dados para atualização do carrinho</param>
    /// <returns>Carrinho atualizado</returns>
    Task<CarrinhoDto> AtualizarCarrinhoAsync(AtualizarCarrinhoDto atualizarCarrinhoDto);
}