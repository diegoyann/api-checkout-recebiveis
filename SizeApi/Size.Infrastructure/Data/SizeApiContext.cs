using Microsoft.EntityFrameworkCore;
using Size.Domain.Entities;
using Size.Domain.Enums;

namespace Size.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados para o Size
/// </summary>
public class SizeContext : DbContext
{
    /// <summary>
    /// Construtor do contexto
    /// </summary>
    /// <param name="options">Opções de configuração do contexto</param>
    public SizeContext(DbContextOptions<SizeContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet de empresas
    /// </summary>
    public DbSet<Empresa> Empresas { get; set; }

    /// <summary>
    /// DbSet de notas fiscais
    /// </summary>
    public DbSet<NotaFiscal> NotasFiscais { get; set; }

    /// <summary>
    /// DbSet de carrinhos de antecipação
    /// </summary>
    public DbSet<CarrinhoAntecipacao> CarrinhosAntecipacao { get; set; }

    /// <summary>
    /// DbSet de itens do carrinho de antecipação
    /// </summary>
    public DbSet<ItemCarrinhoAntecipacao> ItensCarrinhoAntecipacao { get; set; }

    /// <summary>
    /// Configuração do modelo de dados
    /// </summary>
    /// <param name="modelBuilder">Builder do modelo</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Empresa
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Cnpj)
                .IsRequired()
                .HasMaxLength(14);
            
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.FaturamentoMensal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.RamoEmpresa)
                .IsRequired()
                .HasConversion<int>();
            
            entity.Property(e => e.DataCriacao)
                .IsRequired();

            // Índice único para o CNPJ
            entity.HasIndex(e => e.Cnpj)
                .IsUnique();
        });

        // Configuração da entidade NotaFiscal
        modelBuilder.Entity<NotaFiscal>(entity =>
        {
            entity.HasKey(nf => nf.Id);
            
            entity.Property(nf => nf.Numero)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(nf => nf.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            entity.Property(nf => nf.DataVencimento)
                .IsRequired();
            
            entity.Property(nf => nf.DataCriacao)
                .IsRequired();

            // Relacionamento com Empresa
            entity.HasOne(nf => nf.Empresa)
                .WithMany(e => e.NotasFiscais)
                .HasForeignKey(nf => nf.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único para Número + EmpresaId
            entity.HasIndex(nf => new { nf.Numero, nf.EmpresaId })
                .IsUnique();
        });

        // Configuração da entidade CarrinhoAntecipacao
        modelBuilder.Entity<CarrinhoAntecipacao>(entity =>
        {
            entity.HasKey(ca => ca.Id);
            
            entity.Property(ca => ca.DataCriacao)
                .IsRequired();
            
            entity.Property(ca => ca.Ativo)
                .IsRequired();

            // Relacionamento com Empresa
            entity.HasOne(ca => ca.Empresa)
                .WithMany(e => e.CarrinhosAntecipacao)
                .HasForeignKey(ca => ca.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade ItemCarrinhoAntecipacao
        modelBuilder.Entity<ItemCarrinhoAntecipacao>(entity =>
        {
            entity.HasKey(ica => ica.Id);
            
            entity.Property(ica => ica.DataAdicao)
                .IsRequired();

            // Relacionamento com CarrinhoAntecipacao
            entity.HasOne(ica => ica.CarrinhoAntecipacao)
                .WithMany(ca => ca.Itens)
                .HasForeignKey(ica => ica.CarrinhoAntecipacaoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com NotaFiscal
            entity.HasOne(ica => ica.NotaFiscal)
                .WithMany(nf => nf.ItensCarrinho)
                .HasForeignKey(ica => ica.NotaFiscalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice único para CarrinhoAntecipacaoId + NotaFiscalId
            entity.HasIndex(ica => new { ica.CarrinhoAntecipacaoId, ica.NotaFiscalId })
                .IsUnique();
        });


    }


}