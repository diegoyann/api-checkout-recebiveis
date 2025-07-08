using Size.Application.DTOs;

namespace Size.Application.Interfaces;

/// <summary>
/// Interface do serviço de notas fiscais
/// </summary>
public interface IServicoNotaFiscal
{
    /// <summary>
    /// Cria uma nova nota fiscal
    /// </summary>
    /// <param name="criarNotaFiscalDto">Dados da nota fiscal a ser criada</param>
    /// <returns>Nota fiscal criada</returns>
    Task<NotaFiscalDto> CriarNotaFiscalAsync(CriarNotaFiscalDto criarNotaFiscalDto);
    
    /// <summary>
    /// Obtém uma nota fiscal por ID
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Nota fiscal encontrada ou null</returns>
    Task<NotaFiscalDto?> ObterNotaFiscalPorIdAsync(int id);
    
    /// <summary>
    /// Obtém todas as notas fiscais de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Lista de notas fiscais da empresa</returns>
    Task<IEnumerable<NotaFiscalDto>> ObterNotasFiscaisPorEmpresaAsync(int empresaId);
    
    /// <summary>
    /// Remove uma nota fiscal
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Task</returns>
    Task RemoverNotaFiscalAsync(int id);
    
    /// <summary>
    /// Atualiza uma nota fiscal existente
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <param name="atualizarNotaFiscalDto">Dados atualizados da nota fiscal</param>
    /// <returns>Nota fiscal atualizada</returns>
    Task<NotaFiscalDto> AtualizarNotaFiscalAsync(int id, AtualizarNotaFiscalDto atualizarNotaFiscalDto);
}