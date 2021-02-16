namespace Sistema.DUE.Web.Models
{
    public class Estoque
    {      
        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string Tipo { get; set; }
        public string NumeroNF { get; set; }
        public string DataEmissaoNF { get; set; }
        public string CodigoNCM { get; set; }
        public string CodigoURF { get; set; }
        public string CodigoRA { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IdResponsavel { get; set; }
        public string NomeResponsavel { get; set; }
        public string CodigoPaisDestinatario { get; set; }
        public string NomePaisDestinatario { get; set; }
        public string AnoDeposito { get; set; }
        public string SequenciaDeposito { get; set; }
        public string ExisteConteiner { get; set; }
        public string DataConsulta { get; set; }
        public string IdentificacaoCondutor { get; set; }
        public string NomeCondutor { get; set; }
        public string DataHoraEntradaEstoque { get; set; }
        public string DescricaoAvarias { get; set; }
        public string LocalRaCodigo { get; set; }
        public string LocalRaDescricao { get; set; }
        public string LocalUrfCodigo { get; set; }
        public string LocalUrfDescricao { get; set; }
        public string NcmCodigo { get; set; }
        public string NcmDescricao { get; set; }
        public string NotaFiscalDestinatarioNome { get; set; }
        public string NotaFiscalDestinatarioPais { get; set; }
        public string NotaFiscalEmissao { get; set; }
        public string NotaFiscalEmitenteIdentificacao { get; set; }
        public string NotaFiscalEmitenteNome { get; set; }
        public string NotaFiscalEmitentePais { get; set; }
        public string NotaFiscalModelo { get; set; }
        public string NotaFiscalNumero { get; set; }
        public string NotaFiscalSerie { get; set; }
        public string NotaFiscalUf { get; set; }
        public string NumeroDue { get; set; }
        public string NumeroItem { get; set; }
        public string PaisDestino { get; set; }
        public string PesoAferido { get; set; }
        public string QuantidadeExportada { get; set; }
        public string ResponsavelIdentificacao { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelPais { get; set; }
        public string Saldo { get; set; }
        public string TransportadorIdentificacao { get; set; }
        public string TransportadorNome { get; set; }
        public string TransportadorPais { get; set; }
        public string UnidadeEstatistica { get; set; }
        public string Valor { get; set; }
        public string CnpjDestinatario { get; set; }
        public string DescricaoURF { get; set; }
        public string DescricaoRA { get; set; }
        public string DataAverbacao { get; set; }
        public string Sobra { get; set; }
    }
}