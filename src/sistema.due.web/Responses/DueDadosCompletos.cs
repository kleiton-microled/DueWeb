using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.DUE.Web.Responses
{    
    public class Nacionalidade
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string nomeResumido { get; set; }
    }

    public class Declarante
    {
        public string numeroDoDocumento { get; set; }
        public string tipoDoDocumento { get; set; }
        public string nome { get; set; }
        public Nacionalidade nacionalidade { get; set; }
        public bool estrangeiro { get; set; }
    }

    public class EventosDoHistorico
    {
        public string evento { get; set; }
        public string responsavel { get; set; }
        public DateTime dataEHoraDoEvento { get; set; }
        public string informacoesAdicionais { get; set; }
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
        public decimal quantidadeDeItens { get; set; }
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
        public double quantidadeEstatistica { get; set; }
        public string unidadeComercial { get; set; }
        public double valorTotalCalculado { get; set; }
        public Ncm2 ncm { get; set; }
        public bool apresentadaParaDespacho { get; set; }
    }

    public class IdentificacaoDoEmitente2
    {
        public string numero { get; set; }
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
        public decimal quantidadeDeItens { get; set; }
        public bool notaFicalEletronica { get; set; }
    }

    public class Ncm3
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidadeMedidaEstatistica { get; set; }
    }

    public class ItensDaNotaDeRemessa
    {
        public int numeroDoItem { get; set; }
        public NotaFiscal2 notaFiscal { get; set; }
        public int cfop { get; set; }
        public string codigoDoProduto { get; set; }
        public string descricao { get; set; }
        public double quantidadeEstatistica { get; set; }
        public string unidadeComercial { get; set; }
        public double valorTotalBruto { get; set; }
        public Ncm3 ncm { get; set; }
        public bool apresentadaParaDespacho { get; set; }
        public double quantidadeConsumida { get; set; }
    }

    public class Nacionalidade2
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string nomeResumido { get; set; }
    }

    public class Exportador
    {
        public string numeroDoDocumento { get; set; }
        public string tipoDoDocumento { get; set; }
        public Nacionalidade2 nacionalidade { get; set; }
        public bool estrangeiro { get; set; }
    }

    public class CodigoCondicaoVenda
    {
        public string codigo { get; set; }
    }

    public class ListaDeEnquadramento
    {
        public int codigo { get; set; }
        public DateTime dataRegistro { get; set; }
    }

    public class ListaPaisDestino
    {
        public int codigo { get; set; }
    }

    public class Item
    {
        public Ncm ncm { get; set; }
        public decimal quantidadeNaUnidadeEstatistica { get; set; }
        public int numero { get; set; }
        public decimal pesoLiquidoTotal { get; set; }
        public decimal valorDaMercadoriaNaCondicaoDeVenda { get; set; }
        public decimal valorDaMercadoriaNoLocalDeEmbarque { get; set; }
        public decimal valorDaMercadoriaNoLocalDeEmbarqueEmReais { get; set; }
        public DateTime dataDeConversao { get; set; }
        public ItemDaNotaFiscalDeExportacao itemDaNotaFiscalDeExportacao { get; set; }
        public List<ItensDaNotaDeRemessa> itensDaNotaDeRemessa { get; set; }
        public string descricaoDaMercadoria { get; set; }
        public Exportador exportador { get; set; }
        public string unidadeComercializada { get; set; }
        public CodigoCondicaoVenda codigoCondicaoVenda { get; set; }
        public string nomeImportador { get; set; }
        public string enderecoImportador { get; set; }
        public List<ListaDeEnquadramento> listaDeEnquadramentos { get; set; }
        public List<ListaPaisDestino> listaPaisDestino { get; set; }
        public decimal valorTotalCalculadoItem { get; set; }
        public decimal quantidadeNaUnidadeComercializada { get; set; }
        public Tratamentosadministrativo[] tratamentosAdministrativos { get; set; }
    }

    public class MoedaDue
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

    public class RecintoAduaneiroDeEmbarque
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
        public string motivo { get; set; }
    }

    public class UnidadeLocalDeDespacho
    {
        public string codigo { get; set; }
    }

    public class UnidadeLocalDeEmbarque
    {
        public string codigo { get; set; }
    }

    public class MotivoDispensaNotaFiscal
    {
        public int codigo { get; set; }
    }

    public class Tratamentosadministrativo
    {
        public string mensagem { get; set; }
        public string[] orgaos { get; set; }
        public bool impeditivoDeEmbarque { get; set; }
        public string situacao { get; set; }
        public string codigoLPCO { get; set; }
    }

    public class DueDadosCompletos
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public bool bloqueio { get; set; }
        public string chaveDeAcesso { get; set; }
        public string formaDeExportacao { get; set; }
        public string enderecoDoEstabelecimentoDoLocalDeDespacho { get; set; }
        public string latitudeDoLocalDeDespacho { get; set; }
        public string longitudeDoLocalDeDespacho { get; set; }
        public DateTime dataDeRegistro { get; set; }
        public Declarante declarante { get; set; }
        public MotivoDispensaNotaFiscal motivoDeDispensaDaNotaFiscal { get; set; }
        public PaisImportador paisImportador { get; set; }
        public bool embarqueEmRecintoAlfandegado { get; set; }
        public bool despachoEmRecintoAlfandegado { get; set; }
        public List<EventosDoHistorico> eventosDoHistorico { get; set; }
        public bool impedidoDeEmbarque { get; set; }
        public string informacoesComplementares { get; set; }
        public List<Item> itens { get; set; }
        public MoedaDue moeda { get; set; }
        public string numero { get; set; }
        public RecintoAduaneiroDeDespacho recintoAduaneiroDeDespacho { get; set; }
        public RecintoAduaneiroDeEmbarque recintoAduaneiroDeEmbarque { get; set; }
        public string ruc { get; set; }
        public string situacao { get; set; }
        public string situacaoEspecial { get; set; }
        public List<SituacoesDaCarga> situacoesDaCarga { get; set; }
        public List<Solicitaco> solicitacoes { get; set; }
        public bool tratamentoPrioritario { get; set; }
        public UnidadeLocalDeDespacho unidadeLocalDeDespacho { get; set; }
        public UnidadeLocalDeEmbarque unidadeLocalDeEmbarque { get; set; }
        public string responsavelPeloACD { get; set; }
        public bool despachoEmRecintoDomiciliar { get; set; }
        public DateTime dataDeCriacao { get; set; }
        public double valorTotalMercadoria { get; set; }
        public bool dat { get; set; }
        public bool oea { get; set; }
        public string situacaoDue { get; set; }
    }
}