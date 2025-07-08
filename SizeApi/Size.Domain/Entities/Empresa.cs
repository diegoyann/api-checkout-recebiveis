using Size.Domain.Enums;

namespace Size.Domain.Entities;

/// <summary>
/// Entidade que representa uma empresa no sistema
/// </summary>
public class Empresa
{
    /// <summary>
    /// Identificador único da empresa
    /// </summary>
    public int Id { get; set; }
    
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
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Lista de notas fiscais da empresa
    /// </summary>
    public virtual ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>();
    
    /// <summary>
    /// Lista de carrinhos de antecipação da empresa
    /// </summary>
    public virtual ICollection<CarrinhoAntecipacao> CarrinhosAntecipacao { get; set; } = new List<CarrinhoAntecipacao>();
    
    /// <summary>
    /// Calcula o limite de antecipação baseado no faturamento mensal e ramo da empresa
    /// </summary>
    /// <returns>Valor do limite de antecipação</returns>
    public decimal CalcularLimiteAntecipacao()
    {
        return FaturamentoMensal switch
        {
            //Faturamento entre R$ 10 e R$ 50 mil
            >= 10000 and <= 50000 => FaturamentoMensal * 0.50m,
            
            //Faturamento entre R$ 50 mil e R$ 100 mil no ramo de serviços
            >= 50001 and <= 100000 when RamoEmpresa == RamoEmpresa.Servicos => FaturamentoMensal * 0.55m,
            
            //Faturamento entre R$ 50 mil e R$ 100 mil no ramo de produtos
            >= 50001 and <= 100000 when RamoEmpresa == RamoEmpresa.Produtos => FaturamentoMensal * 0.60m,
            
            //Faturamento acima de R$ 100 mil no ramo de serviços
            > 100000 when RamoEmpresa == RamoEmpresa.Servicos => FaturamentoMensal * 0.60m,
            
            //Faturamento acima de R$ 100 mil no ramo de produtos
            > 100000 when RamoEmpresa == RamoEmpresa.Produtos => FaturamentoMensal * 0.65m,
            _                                                 => 0
        };
    }
}