namespace Size.Domain.Entities;

public class NotaFiscal
{
    /// <summary>
    /// Identificador único da nota fiscal
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Número da nota fiscal (obrigatório)
    /// </summary>
    public string Numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da nota fiscal (obrigatório)
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Data de vencimento da nota fiscal (obrigatório)
    /// </summary>
    public DateTime DataVencimento { get; set; }
    
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Identificador da empresa proprietária da nota fiscal
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Empresa proprietária da nota fiscal
    /// </summary>
    public virtual Empresa Empresa { get; set; } = null!;
    
    /// <summary>
    /// Lista de itens do carrinho que contêm esta nota fiscal
    /// </summary>
    public virtual ICollection<ItemCarrinhoAntecipacao> ItensCarrinho { get; set; } = new List<ItemCarrinhoAntecipacao>();
    
    /// <summary>
    /// Calcula o prazo em dias entre a data atual e a data de vencimento
    /// Para antecipação, sempre retorna valor positivo (dias até o vencimento)
    /// </summary>
    /// <returns>Prazo em dias até o vencimento</returns>
    public int CalcularPrazoEmDias()
    {
        // Calculo de quantos dias faltam até o vencimento
        var diasAteVencimento = (DataVencimento.Date - DateTime.Now.Date).Days;
        return Math.Max(0, diasAteVencimento);
    }
    
    /// <summary>
    /// Calcula o deságio baseado na taxa mensal e prazo
    /// Fórmula: Valor Líquido = ValorNF / (1 + Taxa)^(Prazo / 30)
    /// Deságio = ValorNF - Valor Líquido
    /// </summary>
    /// <param name="taxaMensal">Taxa mensal para cálculo (padrão 4%)</param>
    /// <returns>Valor do deságio</returns>
    public decimal CalcularDesagio(decimal taxaMensal = 0.04m)
    {
        var prazoEmDias = CalcularPrazoEmDias();
        var prazoEmMeses = prazoEmDias / 30.0m;
        
        if (prazoEmDias <= 0)
            return 0;
            
        var fatorDesconto = (decimal)Math.Pow((double)(1 + taxaMensal), (double)prazoEmMeses);
        var valorLiquido = Valor / fatorDesconto;
        
        return Valor - valorLiquido;
    }
    
    /// <summary>
    /// Calcula o valor líquido após aplicar o deságio
    /// </summary>
    /// <param name="taxaMensal">Taxa mensal para cálculo (padrão 4%)</param>
    /// <returns>Valor líquido</returns>
    public decimal CalcularValorLiquido(decimal taxaMensal = 0.04m)
    {
        return Valor - CalcularDesagio(taxaMensal);
    }
}