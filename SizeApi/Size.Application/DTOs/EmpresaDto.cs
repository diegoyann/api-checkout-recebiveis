using Size.Domain.Enums;

namespace Size.Application.DTOs;

/// <summary>
/// DTO para criação de empresa
/// </summary>
public class CriarEmpresaDto
{
    /// <summary>
    /// CNPJ da empresa (obrigatório)
    /// </summary>
    public string Cnpj { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome da empresa (obrigatório)
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Faturamento mensal da empresa (obrigatório)
    /// </summary>
    public decimal FaturamentoMensal { get; set; }
    
    /// <summary>
    /// Ramo de atividade da empresa (obrigatório)
    /// </summary>
    public RamoEmpresa RamoEmpresa { get; set; }
}

/// <summary>
/// DTO para atualização de empresa
/// </summary>
public class AtualizarEmpresaDto
{
    /// <summary>
    /// Nome da empresa (obrigatório)
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Faturamento mensal da empresa (obrigatório)
    /// </summary>
    public decimal FaturamentoMensal { get; set; }
    
    /// <summary>
    /// Ramo de atividade da empresa (obrigatório)
    /// </summary>
    public RamoEmpresa RamoEmpresa { get; set; }
}

/// <summary>
/// DTO para retorno de empresa
/// </summary>
public class EmpresaDto
{
    /// <summary>
    /// Identificador único da empresa
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// CNPJ da empresa
    /// </summary>
    public string Cnpj { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Faturamento mensal da empresa
    /// </summary>
    public decimal FaturamentoMensal { get; set; }
    
    /// <summary>
    /// Ramo de atividade da empresa
    /// </summary>
    public RamoEmpresa RamoEmpresa { get; set; }
    
    /// <summary>
    /// Limite de antecipação calculado
    /// </summary>
    public decimal LimiteAntecipacao { get; set; }
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; }
}