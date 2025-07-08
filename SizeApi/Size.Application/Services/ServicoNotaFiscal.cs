using Size.Application.DTOs;
using Size.Application.Interfaces;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Domain.Interfaces.Repositories;

namespace Size.Application.Services;

/// <summary>
/// Serviço responsável pelas operações relacionadas às notas fiscais
/// </summary>
public class ServicoNotaFiscal : IServicoNotaFiscal
{
    private readonly INotaFiscalRepo _repositorioNotaFiscal;
    private readonly IEmpresaRepo _repositorioEmpresa;

    /// <summary>
    /// Construtor do serviço de notas fiscais
    /// </summary>
    /// <param name="repositorioNotaFiscal">Repositório de notas fiscais</param>
    /// <param name="repositorioEmpresa">Repositório de empresas</param>
    public ServicoNotaFiscal(INotaFiscalRepo repositorioNotaFiscal, IEmpresaRepo repositorioEmpresa)
    {
        _repositorioNotaFiscal = repositorioNotaFiscal;
        _repositorioEmpresa = repositorioEmpresa;
    }

    /// <summary>
    /// Cria uma nova nota fiscal
    /// </summary>
    /// <param name="criarNotaFiscalDto">Dados da nota fiscal a ser criada</param>
    /// <returns>Nota fiscal criada</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando a empresa não existe ou já existe nota com o número</exception>
    public async Task<NotaFiscalDto> CriarNotaFiscalAsync(CriarNotaFiscalDto criarNotaFiscalDto)
    {
        // Validações de negócio (complementares às validações de modelo)
        if (string.IsNullOrWhiteSpace(criarNotaFiscalDto.Numero))
            throw new ArgumentException("O número da nota fiscal é obrigatório e não pode estar vazio");
            
        if (criarNotaFiscalDto.Numero.Length > 50)
            throw new ArgumentException("O número da nota fiscal não pode ter mais de 50 caracteres");
            
        if (criarNotaFiscalDto.Valor <= 0)
            throw new ArgumentException("O valor da nota fiscal deve ser maior que zero");
            
        if (criarNotaFiscalDto.DataVencimento.Date <= DateTime.Now.Date)
            throw new ArgumentException($"A data de vencimento ({criarNotaFiscalDto.DataVencimento:dd/MM/yyyy}) deve ser maior que a data atual ({DateTime.Now:dd/MM/yyyy})");

        // Verifica se a empresa existe
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(criarNotaFiscalDto.EmpresaId);
        if (empresa == null)
            throw new InvalidOperationException("Empresa não encontrada");

        // Verifica se já existe nota fiscal com o mesmo número para a empresa
        if (await _repositorioNotaFiscal.ExistePorNumeroEEmpresaAsync(criarNotaFiscalDto.Numero, criarNotaFiscalDto.EmpresaId))
            throw new InvalidOperationException("Já existe uma nota fiscal com este número para esta empresa");

        // Cria a entidade
        var notaFiscal = new NotaFiscal
        {
            Numero = criarNotaFiscalDto.Numero,
            Valor = criarNotaFiscalDto.Valor,
            DataVencimento = criarNotaFiscalDto.DataVencimento,
            EmpresaId = criarNotaFiscalDto.EmpresaId,
            Empresa = empresa
        };

        // Salva no repositório
        var notaFiscalCriada = await _repositorioNotaFiscal.AdicionarAsync(notaFiscal);

        // Retorna o DTO
        return ConverterParaDto(notaFiscalCriada);
    }

    /// <summary>
    /// Obtém uma nota fiscal por ID
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Nota fiscal encontrada ou null</returns>
    public async Task<NotaFiscalDto?> ObterNotaFiscalPorIdAsync(int id)
    {
        var notaFiscal = await _repositorioNotaFiscal.ObterPorIdAsync(id);
        return notaFiscal != null ? ConverterParaDto(notaFiscal) : null;
    }

    /// <summary>
    /// Obtém todas as notas fiscais de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Lista de notas fiscais da empresa</returns>
    public async Task<IEnumerable<NotaFiscalDto>> ObterNotasFiscaisPorEmpresaAsync(int empresaId)
    {
        var notasFiscais = await _repositorioNotaFiscal.ObterPorEmpresaAsync(empresaId);
        return notasFiscais.Select(ConverterParaDto);
    }

    /// <summary>
    /// Remove uma nota fiscal
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <returns>Task</returns>
    /// <exception cref="InvalidOperationException">Lançada quando a nota fiscal não é encontrada</exception>
    public async Task RemoverNotaFiscalAsync(int id)
    {
        var notaFiscal = await _repositorioNotaFiscal.ObterPorIdAsync(id);
        if (notaFiscal == null)
            throw new InvalidOperationException("Nota fiscal não encontrada");

        await _repositorioNotaFiscal.RemoverAsync(notaFiscal);
    }

    /// <summary>
    /// Atualiza uma nota fiscal existente
    /// </summary>
    /// <param name="id">Identificador da nota fiscal</param>
    /// <param name="atualizarNotaFiscalDto">Dados atualizados da nota fiscal</param>
    /// <returns>Nota fiscal atualizada</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando a nota fiscal não é encontrada ou há conflito de número</exception>
    public async Task<NotaFiscalDto> AtualizarNotaFiscalAsync(int id, AtualizarNotaFiscalDto atualizarNotaFiscalDto)
    {
        // Validações de negócio (complementares às validações de modelo)
        if (string.IsNullOrWhiteSpace(atualizarNotaFiscalDto.Numero))
            throw new ArgumentException("O número da nota fiscal é obrigatório e não pode estar vazio");
            
        if (atualizarNotaFiscalDto.Numero.Length > 50)
            throw new ArgumentException("O número da nota fiscal não pode ter mais de 50 caracteres");
            
        if (atualizarNotaFiscalDto.Valor <= 0)
            throw new ArgumentException("O valor da nota fiscal deve ser maior que zero");
            
        if (atualizarNotaFiscalDto.DataVencimento.Date <= DateTime.Now.Date)
            throw new ArgumentException($"A data de vencimento ({atualizarNotaFiscalDto.DataVencimento:dd/MM/yyyy}) deve ser maior que a data atual ({DateTime.Now:dd/MM/yyyy})");

        // Verifica se a nota fiscal existe
        var notaFiscal = await _repositorioNotaFiscal.ObterPorIdAsync(id);
        if (notaFiscal == null)
            throw new InvalidOperationException("Nota fiscal não encontrada");

        // Verifica se já existe outra nota fiscal com o mesmo número para a empresa (exceto a atual)
        var notaExistente = await _repositorioNotaFiscal.ObterPorNumeroEEmpresaAsync(atualizarNotaFiscalDto.Numero, notaFiscal.EmpresaId);
        if (notaExistente != null && notaExistente.Id != id)
            throw new InvalidOperationException("Já existe uma nota fiscal com este número para esta empresa");

        // Atualiza os campos
        notaFiscal.Numero = atualizarNotaFiscalDto.Numero;
        notaFiscal.Valor = atualizarNotaFiscalDto.Valor;
        notaFiscal.DataVencimento = atualizarNotaFiscalDto.DataVencimento;

        // Salva as alterações
        var notaFiscalAtualizada = await _repositorioNotaFiscal.AtualizarAsync(notaFiscal);

        // Retorna o DTO
        return ConverterParaDto(notaFiscalAtualizada);
    }

    /// <summary>
    /// Converte uma entidade NotaFiscal para NotaFiscalDto
    /// </summary>
    /// <param name="notaFiscal">Entidade nota fiscal</param>
    /// <returns>DTO da nota fiscal</returns>
    private static NotaFiscalDto ConverterParaDto(NotaFiscal notaFiscal)
    {
        return new NotaFiscalDto
        {
            Id = notaFiscal.Id,
            Numero = notaFiscal.Numero,
            Valor = notaFiscal.Valor,
            DataVencimento = notaFiscal.DataVencimento,
            EmpresaId = notaFiscal.EmpresaId,
            NomeEmpresa = notaFiscal.Empresa?.Nome ?? string.Empty,
            DataCriacao = notaFiscal.DataCriacao,
            PrazoEmDias = notaFiscal.CalcularPrazoEmDias(),
            Desagio = notaFiscal.CalcularDesagio(),
            ValorLiquido = notaFiscal.CalcularValorLiquido()
        };
    }
}