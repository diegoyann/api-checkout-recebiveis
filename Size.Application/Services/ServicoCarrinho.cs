using Size.Application.DTOs;
using Size.Application.Interfaces;
using Size.Domain.Entities;
using Size.Domain.Interfaces;
using Size.Domain.Interfaces.Repositories;

namespace Size.Application.Services;

/// <summary>
/// Serviço responsável pelas operações relacionadas ao carrinho de antecipação
/// </summary>
public class ServicoCarrinho : IServicoCarrinho
{
    private readonly ICarrinhoAntecipacaoRepo _repositorioCarrinho;
    private readonly IEmpresaRepo _repositorioEmpresa;
    private readonly INotaFiscalRepo _repositorioNotaFiscal;

    /// <summary>
    /// Construtor do serviço de carrinho
    /// </summary>
    /// <param name="repositorioCarrinho">Repositório de carrinho de antecipação</param>
    /// <param name="repositorioEmpresa">Repositório de empresas</param>
    /// <param name="repositorioNotaFiscal">Repositório de notas fiscais</param>
    public ServicoCarrinho(
        ICarrinhoAntecipacaoRepo repositorioCarrinho,
        IEmpresaRepo             repositorioEmpresa,
        INotaFiscalRepo          repositorioNotaFiscal)
    {
        _repositorioCarrinho = repositorioCarrinho;
        _repositorioEmpresa = repositorioEmpresa;
        _repositorioNotaFiscal = repositorioNotaFiscal;
    }

    /// <summary>
    /// Adiciona uma nota fiscal ao carrinho
    /// </summary>
    /// <param name="adicionarItemDto">Dados do item a ser adicionado</param>
    /// <returns>Carrinho atualizado</returns>
    /// <exception cref="InvalidOperationException">Lançada quando há problemas de validação</exception>
    public async Task<CarrinhoDto> AdicionarItemAsync(AdicionarItemCarrinhoDto adicionarItemDto)
    {
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(adicionarItemDto.EmpresaId);
        if (empresa == null)
            throw new InvalidOperationException("Empresa não encontrada");

        // Verifica se a nota fiscal existe e pertence à empresa
        var notaFiscal = await _repositorioNotaFiscal.ObterPorIdAsync(adicionarItemDto.NotaFiscalId);
        if (notaFiscal == null)
            throw new InvalidOperationException("Nota fiscal não encontrada");
            
        if (notaFiscal.EmpresaId != adicionarItemDto.EmpresaId)
            throw new InvalidOperationException("A nota fiscal não pertence à empresa informada");

        // Obtém ou cria o carrinho ativo
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(adicionarItemDto.EmpresaId);
        if (carrinho == null)
        {
            carrinho = new CarrinhoAntecipacao
            {
                EmpresaId = adicionarItemDto.EmpresaId,
                Empresa = empresa,
                Ativo = true
            };
            carrinho = await _repositorioCarrinho.AdicionarAsync(carrinho);
        }

        // Verifica se a nota fiscal já está no carrinho
        if (carrinho.Itens.Any(i => i.NotaFiscalId == adicionarItemDto.NotaFiscalId))
            throw new InvalidOperationException("Esta nota fiscal já está no carrinho");

        // Simula a adição para verificar o limite
        var valorTotalComNovaItem = carrinho.CalcularValorTotalBruto() + notaFiscal.Valor;
        var limiteEmpresa = empresa.CalcularLimiteAntecipacao();
        
        if (valorTotalComNovaItem > limiteEmpresa)
            throw new InvalidOperationException($"Adicionar esta nota fiscal excederia o limite de crédito da empresa (R$ {limiteEmpresa:N2})");

        // Adiciona o item ao carrinho
        var novoItem = new ItemCarrinhoAntecipacao
        {
            CarrinhoAntecipacaoId = carrinho.Id,
            NotaFiscalId = adicionarItemDto.NotaFiscalId,
            NotaFiscal = notaFiscal
        };

        carrinho.Itens.Add(novoItem);
        await _repositorioCarrinho.AtualizarAsync(carrinho);

        // Recarrega o carrinho com todos os dados
        carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(adicionarItemDto.EmpresaId);
        
        return ConverterParaDto(carrinho!);
    }

    /// <summary>
    /// Remove uma nota fiscal do carrinho
    /// </summary>
    /// <param name="removerItemDto">Dados do item a ser removido</param>
    /// <returns>Carrinho atualizado</returns>
    /// <exception cref="InvalidOperationException">Lançada quando há problemas de validação</exception>
    public async Task<CarrinhoDto> RemoverItemAsync(RemoverItemCarrinhoDto removerItemDto)
    {
        // Obtém o carrinho ativo
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(removerItemDto.EmpresaId);
        if (carrinho == null)
            throw new InvalidOperationException("Carrinho não encontrado");

        // Verifica se o item está no carrinho
        var item = carrinho.Itens.FirstOrDefault(i => i.NotaFiscalId == removerItemDto.NotaFiscalId);
        if (item == null)
            throw new InvalidOperationException("Item não encontrado no carrinho");

        // Remove o item
        carrinho.Itens.Remove(item);
        await _repositorioCarrinho.AtualizarAsync(carrinho);

        // Recarrega o carrinho
        carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(removerItemDto.EmpresaId);
        
        return ConverterParaDto(carrinho!);
    }

    /// <summary>
    /// Obtém o carrinho ativo de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho ativo da empresa ou null</returns>
    public async Task<CarrinhoDto?> ObterCarrinhoAtivoAsync(int empresaId)
    {
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(empresaId);
        return carrinho != null ? ConverterParaDto(carrinho) : null;
    }

    /// <summary>
    /// Obtém o carrinho ativo de uma empresa no formato de checkout
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Carrinho no formato de checkout ou null</returns>
    public async Task<CheckoutDto?> ObterCarrinhoFormatoCheckoutAsync(int empresaId)
    {
        // Obtém a empresa
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(empresaId);
        if (empresa == null)
            return null;

        // Obtém o carrinho ativo
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(empresaId);
        if (carrinho == null)
        {
            // Retorna um carrinho vazio no formato de checkout
            return new CheckoutDto
            {
                empresa = empresa.Nome,
                cnpj = empresa.Cnpj,
                limite = empresa.CalcularLimiteAntecipacao(),
                notas_fiscais = new List<NotaFiscalCheckoutDto>(),
                total_bruto = 0,
                total_liquido = 0
            };
        }

        // Converte para o formato de checkout
        var notasFiscaisCheckout = carrinho.Itens.Select(item => new NotaFiscalCheckoutDto
        {
            numero = item.NotaFiscal.Numero,
            valor_bruto = item.NotaFiscal.Valor,
            valor_liquido = item.NotaFiscal.CalcularValorLiquido()
        }).ToList();

        return new CheckoutDto
        {
            empresa = empresa.Nome,
            cnpj = empresa.Cnpj,
            limite = empresa.CalcularLimiteAntecipacao(),
            notas_fiscais = notasFiscaisCheckout,
            total_bruto = carrinho.CalcularValorTotalBruto(),
            total_liquido = carrinho.CalcularValorTotalLiquido()
        };
    }

    /// <summary>
    /// Realiza o checkout do carrinho calculando os valores de antecipação
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Resultado do checkout com valores calculados</returns>
    /// <exception cref="InvalidOperationException">Lançada quando há problemas de validação</exception>
    public async Task<CheckoutDto> RealizarCheckoutAsync(int empresaId)
    {
        // Obtém a empresa
        var empresa = await _repositorioEmpresa.ObterPorIdAsync(empresaId);
        if (empresa == null)
            throw new InvalidOperationException("Empresa não encontrada");

        // Obtém o carrinho ativo
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(empresaId);
        if (carrinho == null || !carrinho.Itens.Any())
            throw new InvalidOperationException("Carrinho vazio ou não encontrado");

        // Verifica o limite de crédito
        if (!carrinho.ValidarLimiteCredito())
            throw new InvalidOperationException("O valor total do carrinho excede o limite de crédito da empresa");

        // Calcula os valores
        var notasFiscaisCheckout = carrinho.Itens.Select(item => new NotaFiscalCheckoutDto
        {
            numero = item.NotaFiscal.Numero,
            valor_bruto = item.NotaFiscal.Valor,
            valor_liquido = item.NotaFiscal.CalcularValorLiquido()
        }).ToList();

        var checkoutDto = new CheckoutDto
        {
            empresa = empresa.Nome,
            cnpj = empresa.Cnpj,
            limite = empresa.CalcularLimiteAntecipacao(),
            notas_fiscais = notasFiscaisCheckout,
            total_bruto = carrinho.CalcularValorTotalBruto(),
            total_liquido = carrinho.CalcularValorTotalLiquido()
        };

        return checkoutDto;
    }

    /// <summary>
    /// Limpa o carrinho de uma empresa
    /// </summary>
    /// <param name="empresaId">Identificador da empresa</param>
    /// <returns>Task</returns>
    public async Task LimparCarrinhoAsync(int empresaId)
    {
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(empresaId);
        if (carrinho != null)
        {
            carrinho.Itens.Clear();
            await _repositorioCarrinho.AtualizarAsync(carrinho);
        }
    }

    /// <summary>
    /// Atualiza o carrinho substituindo todos os itens pelos especificados
    /// </summary>
    /// <param name="atualizarCarrinhoDto">Dados para atualização do carrinho</param>
    /// <returns>Carrinho atualizado</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos</exception>
    /// <exception cref="InvalidOperationException">Lançada quando há problemas de validação</exception>
    public async Task<CarrinhoDto> AtualizarCarrinhoAsync(AtualizarCarrinhoDto atualizarCarrinhoDto)
    {
        // Validações
        if (atualizarCarrinhoDto.NotasFiscaisIds == null)
            throw new ArgumentException("Lista de notas fiscais não pode ser nula");

        var empresa = await _repositorioEmpresa.ObterPorIdAsync(atualizarCarrinhoDto.EmpresaId);
        if (empresa == null)
            throw new InvalidOperationException("Empresa não encontrada");

        // Obtém ou cria o carrinho ativo
        var carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(atualizarCarrinhoDto.EmpresaId);
        if (carrinho == null)
        {
            carrinho = new CarrinhoAntecipacao
            {
                EmpresaId = atualizarCarrinhoDto.EmpresaId,
                Empresa = empresa,
                Ativo = true
            };
            carrinho = await _repositorioCarrinho.AdicionarAsync(carrinho);
        }

        // Limpa os itens atuais
        carrinho.Itens.Clear();

        // Se não há notas fiscais para adicionar, apenas salva o carrinho vazio
        if (!atualizarCarrinhoDto.NotasFiscaisIds.Any())
        {
            await _repositorioCarrinho.AtualizarAsync(carrinho);
            return ConverterParaDto(carrinho);
        }

        // Verifica se todas as notas fiscais existem e pertencem à empresa
        var notasFiscais = new List<NotaFiscal>();
        foreach (var notaFiscalId in atualizarCarrinhoDto.NotasFiscaisIds)
        {
            var notaFiscal = await _repositorioNotaFiscal.ObterPorIdAsync(notaFiscalId);
            if (notaFiscal == null)
                throw new InvalidOperationException($"Nota fiscal com ID {notaFiscalId} não encontrada");
                
            if (notaFiscal.EmpresaId != atualizarCarrinhoDto.EmpresaId)
                throw new InvalidOperationException($"A nota fiscal {notaFiscal.Numero} não pertence à empresa informada");
                
            notasFiscais.Add(notaFiscal);
        }

        // Calcula o valor total das novas notas fiscais
        var valorTotalNovas = notasFiscais.Sum(nf => nf.Valor);
        var limiteEmpresa = empresa.CalcularLimiteAntecipacao();
        
        if (valorTotalNovas > limiteEmpresa)
            throw new InvalidOperationException($"O valor total das notas fiscais (R$ {valorTotalNovas:N2}) excede o limite de crédito da empresa (R$ {limiteEmpresa:N2})");

        // Adiciona os novos itens
        foreach (var notaFiscal in notasFiscais)
        {
            var novoItem = new ItemCarrinhoAntecipacao
            {
                CarrinhoAntecipacaoId = carrinho.Id,
                NotaFiscalId = notaFiscal.Id,
                NotaFiscal = notaFiscal
            };
            carrinho.Itens.Add(novoItem);
        }

        // Salva as alterações
        await _repositorioCarrinho.AtualizarAsync(carrinho);

        // Recarrega o carrinho com todos os dados
        carrinho = await _repositorioCarrinho.ObterCarrinhoAtivoComItensAsync(atualizarCarrinhoDto.EmpresaId);
        
        return ConverterParaDto(carrinho!);
    }

    /// <summary>
    /// Converte uma entidade CarrinhoAntecipacao para CarrinhoDto
    /// </summary>
    /// <param name="carrinho">Entidade carrinho</param>
    /// <returns>DTO do carrinho</returns>
    private static CarrinhoDto ConverterParaDto(CarrinhoAntecipacao carrinho)
    {
        return new CarrinhoDto
        {
            Id = carrinho.Id,
            EmpresaId = carrinho.EmpresaId,
            NomeEmpresa = carrinho.Empresa?.Nome ?? string.Empty,
            NotasFiscais = carrinho.Itens.Select(item => new NotaFiscalCarrinhoDto
            {
                Id = item.NotaFiscal.Id,
                Numero = item.NotaFiscal.Numero,
                Valor = item.NotaFiscal.Valor,
                DataVencimento = item.NotaFiscal.DataVencimento
            }).ToList(),
            ValorTotalBruto = carrinho.CalcularValorTotalBruto(),
            ValorTotalLiquido = carrinho.CalcularValorTotalLiquido(),
            DataCriacao = carrinho.DataCriacao
        };
    }
}