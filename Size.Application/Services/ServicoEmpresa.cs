using Size.Application.DTOs;
using Size.Application.Interfaces;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Domain.Interfaces.Repositories;
using SizeApi.Application.Interfaces;

namespace Size.Application.Services;

/// <summary>
/// Serviço responsável pelas operações relacionadas às empresas
/// </summary>
public class ServicoEmpresa : IServicoEmpresa
{
    private readonly IEmpresaRepo _repositorioEmpresa;

    /// <summary>
    /// Construtor do serviço de empresas
    /// </summary>
    /// <param name="repositorioEmpresa">Repositório de empresas</param>
    public ServicoEmpresa(IEmpresaRepo repositorioEmpresa)
    {
        _repositorioEmpresa = repositorioEmpresa;
    }

    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    /// <param name="criarEmpresaDto">Dados da empresa a ser criada</param>
    /// <returns>Empresa criada</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando já existe empresa com o CNPJ</exception>
    public async Task<EmpresaDto> CriarEmpresaAsync(CriarEmpresaDto criarEmpresaDto)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(criarEmpresaDto.Cnpj))
            throw new ArgumentException("CNPJ é obrigatório");
            
        if (string.IsNullOrWhiteSpace(criarEmpresaDto.Nome))
            throw new ArgumentException("Nome é obrigatório");
            
        if (criarEmpresaDto.FaturamentoMensal <= 0)
            throw new ArgumentException("Faturamento mensal deve ser maior que zero");

        // Verifica se já existe empresa com o CNPJ
        if (await _repositorioEmpresa.ExistePorCnpjAsync(criarEmpresaDto.Cnpj))
            throw new InvalidOperationException("Já existe uma empresa cadastrada com este CNPJ");

        // Cria a entidade
        var empresa = new Empresa
        {
            Cnpj              = criarEmpresaDto.Cnpj,
            Nome              = criarEmpresaDto.Nome,
            FaturamentoMensal = criarEmpresaDto.FaturamentoMensal,
            RamoEmpresa       = criarEmpresaDto.RamoEmpresa
        };

        // Salva no repositório
        var empresaCriada = await _repositorioEmpresa.AdicionarAsync(empresa);

        // Retorna o DTO
        return ConverterParaDto(empresaCriada);
    }

    /// <summary>
    /// Obtém uma empresa por ID
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    public async Task<EmpresaDto?> ObterEmpresaPorIdAsync(int id)
    {
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(id);
        return empresa != null ? ConverterParaDto(empresa) : null;
    }

    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Empresa encontrada ou null</returns>
    public async Task<EmpresaDto?> ObterEmpresaPorCnpjAsync(string cnpj)
    {
        var empresa = await _repositorioEmpresa.ObterPorCnpjAsync(cnpj);
        return empresa != null ? ConverterParaDto(empresa) : null;
    }

    /// <summary>
    /// Obtém todas as empresas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    public async Task<IEnumerable<EmpresaDto>> ObterTodasEmpresasAsync()
    {
        var empresas = await _repositorioEmpresa.ObterTodosAsync();
        return empresas.Select(ConverterParaDto);
    }

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    /// <param name="id">Identificador da empresa</param>
    /// <param name="atualizarEmpresaDto">Dados atualizados da empresa</param>
    /// <returns>Empresa atualizada</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando a empresa não é encontrada</exception>
    public async Task<EmpresaDto> AtualizarEmpresaAsync(int id, AtualizarEmpresaDto atualizarEmpresaDto)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(atualizarEmpresaDto.Nome))
            throw new ArgumentException("Nome é obrigatório");
            
        if (atualizarEmpresaDto.FaturamentoMensal <= 0)
            throw new ArgumentException("Faturamento mensal deve ser maior que zero");

        // Verifica se a empresa existe
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(id);
        if (empresa == null)
            throw new InvalidOperationException("Empresa não encontrada");

        // Atualiza os campos permitidos (CNPJ não pode ser alterado)
        empresa.Nome = atualizarEmpresaDto.Nome;
        empresa.FaturamentoMensal = atualizarEmpresaDto.FaturamentoMensal;
        empresa.RamoEmpresa = atualizarEmpresaDto.RamoEmpresa;

        // Salva as alterações
        var empresaAtualizada = await _repositorioEmpresa.AtualizarAsync(empresa);

        // Retorna o DTO
        return ConverterParaDto(empresaAtualizada);
    }

    /// <summary>
    /// Converte uma entidade Empresa para EmpresaDto
    /// </summary>
    /// <param name="empresa">Entidade empresa</param>
    /// <returns>DTO da empresa</returns>
    private static EmpresaDto ConverterParaDto(Empresa empresa)
    {
        return new EmpresaDto
        {
            Id = empresa.Id,
            Cnpj = empresa.Cnpj,
            Nome = empresa.Nome,
            FaturamentoMensal = empresa.FaturamentoMensal,
            RamoEmpresa = empresa.RamoEmpresa,
            LimiteAntecipacao = empresa.CalcularLimiteAntecipacao(),
            DataCriacao = empresa.DataCriacao
        };
    }
}