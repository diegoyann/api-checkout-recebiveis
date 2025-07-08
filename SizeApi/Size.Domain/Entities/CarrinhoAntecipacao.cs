namespace Size.Domain.Entities;

public class CarrinhoAntecipacao
{
    /// <summary>
    /// Identificador único do carrinho
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identificador da empresa proprietária do carrinho
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Empresa proprietária do carrinho
    /// </summary>
    public virtual Empresa Empresa { get; set; } = null!;
    
    /// <summary>
    /// Data de criação do carrinho
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Indica se o carrinho está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;
    
    /// <summary>
    /// Lista de itens do carrinho
    /// </summary>
    public virtual ICollection<ItemCarrinhoAntecipacao> Itens { get; set; } = new List<ItemCarrinhoAntecipacao>();
    
    /// <summary>
    /// Calcula o valor total bruto do carrinho
    /// </summary>
    /// <returns>Valor total bruto</returns>
    public decimal CalcularValorTotalBruto()
    {
        return Itens.Sum(item => item.NotaFiscal.Valor);
    }
    
    /// <summary>
    /// Calcula o valor total líquido do carrinho
    /// </summary>
    /// <param name="taxaMensal">Taxa mensal para cálculo (padrão 4.65%)</param>
    /// <returns>Valor total líquido</returns>
    public decimal CalcularValorTotalLiquido(decimal taxaMensal = 0.04m)
    {
        return Itens.Sum(item => item.NotaFiscal.CalcularValorLiquido(taxaMensal));
    }
    
    /// <summary>
    /// Verifica se o carrinho respeita o limite de crédito da empresa
    /// </summary>
    /// <returns>True se está dentro do limite, False caso contrário</returns>
    public bool ValidarLimiteCredito()
    {
        var valorTotalBruto = CalcularValorTotalBruto();
        var limiteEmpresa   = Empresa.CalcularLimiteAntecipacao();
        
        return valorTotalBruto <= limiteEmpresa;
    }
}