using Size.Domain.Entities;

namespace Size.Domain.Interfaces;

/// <summary>
/// Interface do repositório de notas fiscais
/// </summary>
public interface INotaFiscalRepo : IGeralRepo<NotaFiscal>
{
    /// <summary>
    /// Obtém todas as notas fiscais de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Lista de notas fiscais da empresa</returns>
    Task<IEnumerable<NotaFiscal>> ObterPorEmpresaAsync(int empresaId);
    
    /// <summary>
    /// Obtém uma nota fiscal por número e empresa
    /// </summary>
    /// <param name="numero">Número da nota fiscal</param>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Nota fiscal encontrada ou null</returns>
    Task<NotaFiscal?> ObterPorNumeroEEmpresaAsync(string numero, int empresaId);
    
    /// <summary>
    /// Verifica se já existe uma nota fiscal com o número informado para a empresa
    /// </summary>
    /// <param name="numero">Número da nota fiscal</param>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>True se existe, False caso contrário</returns>
    Task<bool> ExistePorNumeroEEmpresaAsync(string numero, int empresaId);
}