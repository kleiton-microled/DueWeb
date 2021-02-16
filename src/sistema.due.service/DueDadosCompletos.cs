using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cargill.DUE.Service
{
    public class EstabelecimentoDoLocalDeDespacho
    {
        public string numero { get; set; }
        public string nome { get; set; }
        public bool cnpj { get; set; }
        public bool cpf { get; set; }
    }

    public class EventosDoHistorico
    {
        public string evento { get; set; }
        public string responsavel { get; set; }
        public string informacoesAdicionais { get; set; }
        public string motivo { get; set; }
        public string detalhes { get; set; }
        public DateTime dataEHoraDoEvento { get; set; }
    }

    public class ExigenciasFiscai
    {
        public string orgao { get; set; }
        public int numeroOrdem { get; set; }
        public string textoDaExigencia { get; set; }
        public string textoDaJustificativa { get; set; }
        public DateTime dataDaExigencia { get; set; }
        public DateTime dataDeLiberacao { get; set; }
        public string auditorDoRegistro { get; set; }
        public string auditorDaLiberacao { get; set; }
        public string situacao { get; set; }
    }

    public class Ncm
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidadeMedidaEstatistica { get; set; }
    }

    public class IdentificacaoDoEmitente
    {
        public string numero { get; set; }
        public bool cnpj { get; set; }
        public bool cpf { get; set; }
    }

    public class NotaFiscal
    {
        public string chaveDeAcesso { get; set; }
        public string modelo { get; set; }
        public int serie { get; set; }
        public int numeroDoDocumento { get; set; }
        public string ufDoEmissor { get; set; }
        public IdentificacaoDoEmitente identificacaoDoEmitente { get; set; }
        public string finalidade { get; set; }
        public int quantidadeDeItens { get; set; }
        public bool notaFicalEletronica { get; set; }
    }

    public class Ncm2
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidadeMedidaEstatistica { get; set; }
    }

    public class ItemDaNotaFiscalDeExportacao
    {
        public int numeroDoItem { get; set; }
        public NotaFiscal notaFiscal { get; set; }
        public int cfop { get; set; }
        public string codigoDoProduto { get; set; }
        public string descricao { get; set; }
        public int quantidadeEstatistica { get; set; }
        public string unidadeComercial { get; set; }
        public int valorTotalBruto { get; set; }
        public int valorTotalCalculado { get; set; }
        public Ncm2 ncm { get; set; }
        public bool apresentadaParaDespacho { get; set; }
        public int quantidadeConsumida { get; set; }
    }

    public class IdentificacaoDoEmitente2
    {
        public string numero { get; set; }
        public string nome { get; set; }
        public bool cnpj { get; set; }
        public bool cpf { get; set; }
    }

    public class NotaFiscal2
    {
        public string chaveDeAcesso { get; set; }
        public string modelo { get; set; }
        public int serie { get; set; }
        public int numeroDoDocumento { get; set; }
        public string ufDoEmissor { get; set; }
        public IdentificacaoDoEmitente2 identificacaoDoEmitente { get; set; }
        public string finalidade { get; set; }
        public int quantidadeDeItens { get; set; }
        public bool notaFicalEletronica { get; set; }
    }

    public class Ncm3
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidadeMedidaEstatistica { get; set; }
    }

    public class ItensDeNotaComplementar
    {
        public int numeroDoItem { get; set; }
        public NotaFiscal2 notaFiscal { get; set; }
        public int cfop { get; set; }
        public string codigoDoProduto { get; set; }
        public string descricao { get; set; }
        public int quantidadeEstatistica { get; set; }
        public string unidadeComercial { get; set; }
        public int valorTotalBruto { get; set; }
        public int valorTotalCalculado { get; set; }
        public Ncm3 ncm { get; set; }
        public bool apresentadaParaDespacho { get; set; }
        public int quantidadeConsumida { get; set; }
    }

    public class IdentificacaoDoEmitente3
    {
        public string numero { get; set; }
        public string nome { get; set; }
        public bool cnpj { get; set; }
        public bool cpf { get; set; }
    }

    public class NotaFiscal3
    {
        public string chaveDeAcesso { get; set; }
        public string modelo { get; set; }
        public int serie { get; set; }
        public int numeroDoDocumento { get; set; }
        public string ufDoEmissor { get; set; }
        public IdentificacaoDoEmitente3 identificacaoDoEmitente { get; set; }
        public string finalidade { get; set; }
        public int quantidadeDeItens { get; set; }
        public bool notaFicalEletronica { get; set; }
    }

    public class Ncm4
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidadeMedidaEstatistica { get; set; }
    }

    public class ItensDaNotaDeRemessa
    {
        public int numeroDoItem { get; set; }
        public NotaFiscal3 notaFiscal { get; set; }
        public int cfop { get; set; }
        public string codigoDoProduto { get; set; }
        public string descricao { get; set; }
        public int quantidadeEstatistica { get; set; }
        public string unidadeComercial { get; set; }
        public int valorTotalBruto { get; set; }
        public int valorTotalCalculado { get; set; }
        public Ncm4 ncm { get; set; }
        public bool apresentadaParaDespacho { get; set; }
        public int quantidadeConsumida { get; set; }
    }

    public class Nacionalidade
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string nomeResumido { get; set; }
    }

    public class Exportador
    {
        public Nacionalidade nacionalidade { get; set; }
        public string numeroDoDocumento { get; set; }
        public bool estrangeiro { get; set; }
        public string tipoDoDocumento { get; set; }
    }

    public class DocumentosImportacao
    {
        public DateTime dataRegistro { get; set; }
        public int numeroProcesso { get; set; }
        public string numeroDI { get; set; }
        public string numeroDSI { get; set; }
        public string numeroDSIFormulario { get; set; }
        public string numeroEDBV { get; set; }
        public string complemento { get; set; }
        public int quantidadeUtilizada { get; set; }
    }

    public class CodigoCondicaoVenda
    {
        public string codigo { get; set; }
    }

    public class ListaPaisDestino
    {
        public int codigo { get; set; }
    }

    public class Iten
    {
        public Ncm ncm { get; set; }
        public int quantidadeNaUnidadeEstatistica { get; set; }
        public int numero { get; set; }
        public int pesoLiquidoTotal { get; set; }
        public int valorDaMercadoriaNaCondicaoDeVenda { get; set; }
        public int valorDaMercadoriaNoLocalDeEmbarque { get; set; }
        public int valorDaMercadoriaNoLocalDeEmbarqueEmReais { get; set; }
        public DateTime dataDeConversao { get; set; }
        public ItemDaNotaFiscalDeExportacao itemDaNotaFiscalDeExportacao { get; set; }
        public List<ItensDeNotaComplementar> itensDeNotaComplementar { get; set; }
        public List<ItensDaNotaDeRemessa> itensDaNotaDeRemessa { get; set; }
        public string motivoDoTratamentoPrioritario { get; set; }
        public string descricaoDaMercadoria { get; set; }
        public Exportador exportador { get; set; }
        public string unidadeComercializada { get; set; }
        public int percentualDeComissaoDoAgente { get; set; }
        public List<DocumentosImportacao> documentosImportacao { get; set; }
        public string atributoDestaqueNcmBD { get; set; }
        public CodigoCondicaoVenda codigoCondicaoVenda { get; set; }
        public string nomeImportador { get; set; }
        public string enderecoImportador { get; set; }
        public List<ListaPaisDestino> listaPaisDestino { get; set; }
    }

    public class Moeda
    {
        public int codigo { get; set; }
    }

    public class PaisImportador
    {
        public int codigo { get; set; }
    }

    public class RecintoAduaneiroDeDespacho
    {
        public string codigo { get; set; }
    }

    public class SituacoesDaCarga
    {
        public string cpfOuCnpjDoResponsavel { get; set; }
        public string urfDeDespacho { get; set; }
        public string recintoAduaneiro { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public int codigo { get; set; }
        public string descricao { get; set; }
        public bool cargaOperada { get; set; }
    }

    public class Solicitaco
    {
        public string tipoSolicitacao { get; set; }
        public DateTime dataDaSolicitacao { get; set; }
        public string usuarioResponsavel { get; set; }
        public int codigoDoStatusDaSolicitacao { get; set; }
        public string statusDaSolicitacao { get; set; }
        public DateTime dataDeApreciacao { get; set; }
        public string usuarioQueAnalisou { get; set; }
        public string motivo { get; set; }
        public string numeroDoComprot { get; set; }
        public string motivoDoIndeferimento { get; set; }
    }

    public class UnidadeLocalDeDespacho
    {
        public string codigo { get; set; }
    }

    public class UnidadeLocalDeEmbarque
    {
        public string codigo { get; set; }
    }

    public class MotivoDeDispensaDaNotaFiscal
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
    }

    public class DueDadosCompletos
    {
        public bool bloqueio { get; set; }
        public string chaveDeAcesso { get; set; }
        public DateTime dataDeRegistro { get; set; }
        public bool embarqueEmRecintoAlfandegado { get; set; }
        public bool despachoEmRecintoAlfandegado { get; set; }
        public string enderecoDoEstabelecimentoDoLocalDeDespacho { get; set; }
        public string enderecoDoEstabelecimentoDoLocalDeEmbarque { get; set; }
        public EstabelecimentoDoLocalDeDespacho estabelecimentoDoLocalDeDespacho { get; set; }
        public List<EventosDoHistorico> eventosDoHistorico { get; set; }
        public List<ExigenciasFiscai> exigenciasFiscais { get; set; }
        public bool impedidoDeEmbarque { get; set; }
        public List<Iten> itens { get; set; }
        public Moeda moeda { get; set; }
        public string numero { get; set; }
        public PaisImportador paisImportador { get; set; }
        public RecintoAduaneiroDeDespacho recintoAduaneiroDeDespacho { get; set; }
        public string referenciaDoEnderecoDoLocalDeDespacho { get; set; }
        public string referenciaDoEnderecoDoLocalDeEmbarque { get; set; }
        public string ruc { get; set; }
        public string situacao { get; set; }
        public List<SituacoesDaCarga> situacoesDaCarga { get; set; }
        public List<Solicitaco> solicitacoes { get; set; }
        public bool tratamentoPrioritario { get; set; }
        public UnidadeLocalDeDespacho unidadeLocalDeDespacho { get; set; }
        public UnidadeLocalDeEmbarque unidadeLocalDeEmbarque { get; set; }
        public string responsavelPeloACD { get; set; }
        public MotivoDeDispensaDaNotaFiscal motivoDeDispensaDaNotaFiscal { get; set; }
        public string justificativaDeDispensaDaNotaFiscal { get; set; }
        public bool despachoEmRecintoDomiciliar { get; set; }
        public DateTime dataDoCCE { get; set; }
        public DateTime dataDeCriacao { get; set; }
        public bool dat { get; set; }
        public bool oea { get; set; }
    }
}