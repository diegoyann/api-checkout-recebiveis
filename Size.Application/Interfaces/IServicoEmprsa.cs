using Size.Application.DTOs;

namespace SizeApi.Application.Interfaces;

/// <summary>
/// Interface do serviço de empresas
/// </summary>
public interface IServicoEmpresa
{
    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    /// <param name="criarEmpresaDto">Dados da empresa a ser criada</param>
    /// <returns>Empresa criada</returns>
    Task<EmpresaDto> CriarEmpresaAsync(CriarEmpresaDto criarEmpresaDto);
    
    /// <summary>
    /// Obtém uma empresa por ID
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    Task<EmpresaDto?> ObterEmpresaPorIdAsync(int id);
    
    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    Task<EmpresaDto?> ObterEmpresaPorCnpjAsync(string cnpj);
    
    /// <summary>
    /// Obtém todas as empresas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    Task<IEnumerable<EmpresaDto>> ObterTodasEmpresasAsync();
    
    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <param name="atualizarEmpresaDto">Dados atualizados da empresa</param>
    /// <returns>Empresa atualizada</returns>
    Task<EmpresaDto> AtualizarEmpresaAsync(int id, AtualizarEmpresaDto atualizarEmpresaDto);
}