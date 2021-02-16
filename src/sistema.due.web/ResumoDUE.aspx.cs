using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using System;
using System.Linq;
using System.Text;

namespace Sistema.DUE.Web
{
    public partial class ResumoDUE : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly UnidadesReceitaDAO _unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly RecintosDAO _recintosDAO = new RecintosDAO();
        private readonly AtributosNCMDAO atributosDAO = new AtributosNCMDAO();

        private decimal totalQuantidade = 0;
        private decimal totalPrecoCV = 0;
        private decimal totalPrecoLE = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["due"] != null)
            {
                var due = _documentoUnicoExportacaoDAO.ObterDUEPorId(Request.QueryString["due"].ToString().ToInt());

                if (due != null)
                {
                    // Informações Iniciais

                    this.lblInfoIniciaisCNPJDeclarante.Text = due.DocumentoDeclarante;
                    this.lblInfoIniciaisMoeda.Text = due.MoedaNegociacao;
                    this.lblInfoIniciaisRUC.Text = due.RUC;

                    switch (due.SituacaoEspecial)
                    {
                        case 0:
                            this.lblInfoIniciaisSitEspecial.Text = "EMBARQUE NORMAL";
                            break;
                        case 2001:
                            this.lblInfoIniciaisSitEspecial.Text = "DU-E A POSTERIORI";
                            break;
                        case 2002:
                            this.lblInfoIniciaisSitEspecial.Text = "EMBARQUE ANTECIPADO";
                            break;
                        case 2003:
                            this.lblInfoIniciaisSitEspecial.Text = "EXPORTAÇÃO SEM SAÌDA DA MERCADORIA DO PAÍS";
                            break;
                    }

                    switch (due.FormaExportacao)
                    {
                        case 1001:
                            this.lblInfoIniciaisFormaExportacao.Text = "POR CONTA PRÓPRIA";
                            break;
                        case 1002:
                            this.lblInfoIniciaisFormaExportacao.Text = "POR CONTA E ORDEM DE TERCEIROS";
                            break;
                        case 1003:
                            this.lblInfoIniciaisFormaExportacao.Text = "POR OPERADOR DE REMESSA POSTAL OU EXPRESSA";
                            break;
                    }

                    switch (due.ViaEspecialTransporte)
                    {
                        case 4001:
                            this.lblInfoIniciaisViaEspecialTransp.Text = "MEIOS PRÓPRIOS / POR REBOQUE";
                            break;
                        case 4002:
                            this.lblInfoIniciaisViaEspecialTransp.Text = "DUTOS";
                            break;
                        case 4003:
                            this.lblInfoIniciaisViaEspecialTransp.Text = "LINHAS DE TRANSMISSÃO";
                            break;
                        case 4004:
                            this.lblInfoIniciaisViaEspecialTransp.Text = "EM MÃOS";
                            break;
                    }

                    // Informações Local Despacho

                    if (due.UnidadeDespacho != null)
                    {
                        var unidadeRFBDespacho = _unidadesReceitaDAO.ObterUnidadesRFBPorId(due.UnidadeDespacho.Id.ToInt());

                        if (unidadeRFBDespacho != null)
                        {
                            this.lblInfLocalDespUnidadeRFB.Text = unidadeRFBDespacho.Descricao;
                        }

                        var recintoDespacho = _recintosDAO.ObterRecintoPorId(due.UnidadeDespacho.RecintoAduaneiroId.ToInt());

                        if (recintoDespacho != null)
                        {
                            this.lblInfLocalDespRecintoAduaneiro.Text = recintoDespacho.Descricao;
                        }

                        this.lblInfLocalDespCNPJResponsavel.Text = due.UnidadeDespacho.DocumentoResponsavel;
                        this.lblInfLocalDespEndereco.Text = due.UnidadeDespacho.Endereco;
                        this.lblInfLocalDespLatitude.Text = due.UnidadeDespacho.Latitude;
                        this.lblInfLocalDespLongitude.Text = due.UnidadeDespacho.Longitude;

                        switch (due.UnidadeDespacho.TipoRecinto)
                        {
                            case 19:
                                this.lblInfLocalDespTipoRecinto.Text = "FORA DO RECINTO ALFANDEGADO (DOMICILIAR)";
                                break;
                            case 22:
                                this.lblInfLocalDespTipoRecinto.Text = "FORA DO RECINTO ALFANDEGADO (NÃO DOMICILIAR)";
                                break;
                            case 281:
                                this.lblInfLocalDespTipoRecinto.Text = "RECINTO ALFANDEGADO";
                                break;
                        }
                    }

                    // Informações Local Embarque

                    if (due.UnidadeEmbarque != null)
                    {
                        var unidadeRFBEmbarque = _unidadesReceitaDAO.ObterUnidadesRFBPorId(due.UnidadeEmbarque.Id.ToInt());
                        var recintoEmbarque = _recintosDAO.ObterRecintoPorId(due.UnidadeEmbarque.RecintoAduaneiroId.ToInt());

                        if (unidadeRFBEmbarque != null)
                        {
                            this.lblInfLocalEmbarqueUnidadeRFB.Text = unidadeRFBEmbarque.Descricao;
                        }

                        if (recintoEmbarque != null)
                        {
                            this.lblInfLocalEmbarqueRecintoAduaneiro.Text = recintoEmbarque.Descricao;
                        }

                        this.lblInfLocalEmbarqueRefEndereco.Text = due.UnidadeEmbarque.EnderecoReferencia;

                        switch (due.UnidadeEmbarque.TipoRecinto)
                        {
                            case 19:
                                this.lblInfLocalEmbarqueTipoRecinto.Text = "FORA DO RECINTO ALFANDEGADO (DOMICILIAR)";
                                break;
                            case 22:
                                this.lblInfLocalEmbarqueTipoRecinto.Text = "FORA DO RECINTO ALFANDEGADO (NÃO DOMICILIAR)";
                                break;
                            case 281:
                                this.lblInfLocalEmbarqueTipoRecinto.Text = "RECINTO ALFANDEGADO";
                                break;
                        }
                    }

                    // Observações

                    this.lblObservacoes.Text = due.InformacoesComplementares;

                    var itens = _documentoUnicoExportacaoDAO.ObterItensDUE(due.Id);

                    var tabelaItem = new StringBuilder();

                    int contadorItem = 1;

                    foreach (var item in itens)
                    {
                        tabelaItem.Append($@"
                            
                            <br /><br /><br /> 

                            <h2>Item {contadorItem}</h2>

                            <table style='width:100%;'>
                                <tr class='cinza'>
                                    <td><strong>Nome Exportador</strong></td>
                                    <td><strong>CNPJ Exportador</strong></td>
                                    <td><strong>Endereço</strong></td>
                                    <td><strong>UF</strong></td>
                                    <td><strong>País</strong></td>
                                </tr>                   
                                <tr>
                                    <td>{item.Exportador?.Descricao ?? "&nbsp;"}</td>
                                    <td>{item.Exportador?.Documento ?? "&nbsp;"}</td>
                                    <td>{item.Exportador?.Endereco ?? "&nbsp;"}</td>
                                    <td>{item.Exportador?.UF ?? "&nbsp;"}</td>
                                    <td>{item.Exportador?.Pais ?? "&nbsp;"}</td>
                                </tr>
                                <tr class='cinza'>
                                    <td><strong>Nome Importador</strong></td>
                                    <td><strong>Endereço Exportador</strong></td>
                                    <td><strong>País</strong></td>
                                    <td><strong>Condição Venda</strong></td>
                                    <td><strong>Motivo Dispensa NF</strong></td>
                                </tr>
                                <tr>
                                    <td>{item.Importador?.Descricao ?? "&nbsp;"}</td>
                                    <td>{item.Importador?.Endereco ?? "&nbsp;"}</td>
                                    <td>{item.Importador?.Pais ?? "&nbsp;"}</td>
                                    <td>{item.CondicaoVenda ?? "&nbsp;"}</td>
                                    <td>{item.MotivoDispensaNF}</td>
                                </tr>
                                <tr class='cinza'>
                                    <td colspan='5'><strong>Chave NF Exportação</strong></td>
                                </tr>
                                <tr>
                                    <td colspan='5'>{item.NF}</td>
                                </tr>
                            </table>");

                        var subItens = _documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                        foreach (var subItem in subItens)
                        {
                            totalQuantidade = totalQuantidade + subItem.QuantidadeEstatistica;
                            totalPrecoCV = totalPrecoCV + subItem.ValorMercadoriaCondicaoVenda;
                            totalPrecoLE = totalPrecoLE + subItem.ValorMercadoriaLocalEmbarque;

                            tabelaItem.Append($@"  

                                <h3>Detalhamento Item {contadorItem}</h3>

                                <table style='width:100%;'>
                                    <tr class='cinza'>
                                        <td><strong>Item</strong></td>
                                        <td><strong>VMLE</strong></td>
                                        <td><strong>País Destino</strong></td>
                                        <td><strong>Qtde</strong></td>
                                        <td><strong>Prioridade</strong></td>
                                        <td><strong>Hora Limite</strong></td>
                                        <td><strong>Descrição Comp.</strong></td>
                                        <td><strong>NCM</strong></td>
                                        <td><strong>VMCV</strong></td>
                                        <td><strong>Cód Produto</strong></td>
                                        <td><strong>Desc. Mercadoria</strong></td>
                                        <td><strong>Qtde. Unid</strong></td>
                                        <td><strong>Descr. Unid.</strong></td>
                                        <td><strong>Peso Liq. Total</strong></td>
                                        <td><strong>Enquadramento</strong></td>
                                        <td><strong>Comissão Agente</strong></td>
                                    </tr>
                                    <tr>
                                        <td>{subItem.Item}</td>
                                        <td>{subItem.ValorMercadoriaLocalEmbarque}</td>
                                        <td>{subItem.PaisDestino}</td>
                                        <td>{subItem.QuantidadeEstatistica}</td>
                                        <td>{subItem.PrioridadeCarga}</td>
                                        <td>{subItem.Limite}</td>
                                        <td>{subItem.DescricaoComplementar}</td>
                                        <td>{subItem.NCM}</td>
                                        <td>{subItem.ValorMercadoriaCondicaoVenda}</td>
                                        <td>{subItem.CodigoProduto}</td>
                                        <td>{subItem.DescricaoMercadoria}</td>
                                        <td>{subItem.QuantidadeUnidades}</td>
                                        <td>{subItem.DescricaoUnidade}</td>
                                        <td>{subItem.PesoLiquidoTotal}</td>
                                        <td>{subItem.Enquadramento1Id}</td>
                                        <td>{subItem.ComissaoAgente}</td>
                                    </tr>
                                </table>");

                            if (subItem.NCM == "09011110" || subItem.NCM == "09011190" || subItem.NCM == "09011200" ||
                                subItem.NCM == "09012100" || subItem.NCM == "09012200" || subItem.NCM == "21011110" ||
                                subItem.NCM == "21011190" || subItem.NCM == "21011200")
                            {
                                var atributoPadraoQualidade = atributosDAO.ObterAtributo("PadraoQualidade", subItem.NCM.ToString(), subItem.Attr_Padrao_Qualidade.ToString());
                                var atributoEmbarcadoEm = atributosDAO.ObterAtributo("EmbarcadoEm", subItem.NCM.ToString(), subItem.Attr_Embarque_Em.ToString());
                                var atributoTipo = atributosDAO.ObterAtributo("Tipo", subItem.NCM.ToString(), subItem.Attr_Tipo.ToString());
                                var atributoMetodoProcessamento = atributosDAO.ObterAtributo("MetodoProcessamento", subItem.NCM.ToString(), subItem.Attr_Metodo_Processamento.ToString());
                                var atributoCaracteristicaEspecial = atributosDAO.ObterAtributo("CaracteristicaEspecial", subItem.NCM.ToString(), subItem.Attr_Caracteristica_Especial);
                                var atributoOutraCaracteristicaEspecial = atributosDAO.ObterAtributo("OutraCaracteristicaEspecial", subItem.NCM.ToString());

                                tabelaItem.Append($@"  

                                <h3>Atributos Café</h3>

                                <table style='width:100%;'>
                                    <tr class='cinza'>
                                        <td><strong>Padrão Qualidade</strong></td>
                                        <td><strong>Embarcado Em</strong></td>
                                        <td><strong>Tipo</strong></td>
                                        <td colspan='4'><strong>Método Processamento</strong></td>
                                        <td colspan='4'><strong>Caractéristica Especial</strong></td>
                                        <td colspan='4'><strong>Outra Caractéristica Especial</strong></td>
                                        <td><strong>Embalagem Final</strong></td>
                                    </tr>
                                    <tr>                                       
                                        <td>{atributoPadraoQualidade?.Descricao}</td>
                                        <td>{atributoEmbarcadoEm?.Descricao}</td>
                                        <td>{atributoTipo?.Descricao}</td>
                                        <td colspan='4'>{atributoMetodoProcessamento?.Descricao}</td>
                                        <td colspan='4'>{atributoCaracteristicaEspecial?.Descricao}</td>
                                        <td colspan='4'>{atributoOutraCaracteristicaEspecial?.Descricao}</td>
                                        <td>{subItem?.Attr_Embalagem_Final}</td>
                                    </tr>
                                </table>");
                            }

                            var dadosAtosConcessorios = _documentoUnicoExportacaoDAO.ObterAtosConcessorios(subItem.Id);
                            var lpcos = _documentoUnicoExportacaoDAO.ObterLPCO(subItem.Id);

                            if (dadosAtosConcessorios.Count() > 0 || lpcos.Count() > 0)
                            {
                                tabelaItem.Append($@"<h3>Atos Concessórios / LPCO</h3>");
                            }

                            if (dadosAtosConcessorios != null)
                            {
                                tabelaItem.Append($@"  

                                    <br />

                                    <table style='width:60%;float:left;'>
                                        <tr class='cinza'>
                                            <th colspan='7'>Atos Concessórios</th>
                                        </tr>
                                        <tr class='cinza'>
                                            <th>Número AC</th>
                                            <th>CNPJ Benef. AC</th>
                                            <th>Número item AC</th>
                                            <th>NCM item AC</th>
                                            <th>Qtde Utilizada</th>
                                            <th>VMLE c/ Cob. Cambial</th>
                                            <th>VMLE s/ Cob. Cambial</th>
                                        </tr>");

                                foreach (var atoConcessorio in dadosAtosConcessorios)
                                {
                                    tabelaItem.Append($@"
                                            <tr>
                                                <td>{atoConcessorio.Numero}</td>
                                                <td>{atoConcessorio.CNPJBeneficiario}</td>
                                                <td>{atoConcessorio.NumeroItem}</td>
                                                <td>{atoConcessorio.NCMItem}</td>
                                                <td>{atoConcessorio.QuantidadeUtilizada}</td>
                                                <td>{atoConcessorio.VMLEComCoberturaCambial}</td>
                                                <td>{atoConcessorio.VMLESemCoberturaCambial}</td>
                                            </tr>");
                                }

                                tabelaItem.Append($@"</table>");
                            }

                            if (lpcos != null)
                            {
                                tabelaItem.Append($@"  

                                        <br />

                                        <table style='width:30%;float:right;'>
                                            <tr class='cinza'>
                                                <th>LPCO</th>
                                            </tr>");

                                foreach (var lpco in lpcos)
                                {
                                    tabelaItem.Append($@"
                                            <tr>
                                                <td>{lpco.Numero}</td>                                                
                                            </tr>");
                                }

                                tabelaItem.Append($@"</table>");
                            }
                        }

                        tabelaItem.Append($@"<br /> ");

                        contadorItem++;
                    }

                    this.lblItens.Text = tabelaItem.ToString();

                    this.txtQuantidadeTotal.Text = string.Format("{0:N3}", totalQuantidade);
                    this.txtPrecoTotalCV.Text = string.Format("{0:N3}", totalPrecoCV);
                    this.txtPrecoTotalLE.Text = string.Format("{0:N3}", totalPrecoLE);
                }
            }
        }
    }
}