using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.RetificacaoSemServico;
using Sistema.DUE.Web.Services;
using Sistema.DUE.Web.ServicoSiscomex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using System.Xml;

namespace Sistema.DUE.Web.OperacoesXML
{
    public class GerarXML
    {
        private readonly DocumentoUnicoExportacaoDAO documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly NotaFiscalDAO notaFiscalDAO = new NotaFiscalDAO();
        private readonly CertificadoDAO _certificadoDAO = new CertificadoDAO();
        private readonly AtributosNCMDAO atributosDAO = new AtributosNCMDAO();
        private readonly ParametrosDAO _parametrosDAO = new ParametrosDAO();

        public int DUEId { get; set; }
        public DUEMaster DUE { get; set; }

        public void Validar(ModelStateDictionary ModelState)
        {
            try
            {
                this.DUE = documentoUnicoExportacaoDAO.ObterDUEPorId(this.DUEId);

                if (this.DUE == null)
                {
                    ModelState.AddModelError(string.Empty, "Portal Microled: DUE não encontrada");
                    return;
                }

                if (this.DUE.UnidadeDespacho == null)
                    ModelState.AddModelError(string.Empty, "Portal Microled: Unidade de Despacho não informada");

                if (this.DUE.UnidadeEmbarque == null)
                    ModelState.AddModelError(string.Empty, "Portal Microled: Unidade de Embarque não informada");

                var itens = documentoUnicoExportacaoDAO.ObterItensDUE(this.DUE.Id);

                if (itens == null || itens.Count() == 0)
                    ModelState.AddModelError(string.Empty, "Portal Microled: A DUE não possui nenhum item vinculado");

                this.DUE.AdicionarItens(itens);

                if (this.DUE.Itens.Where(c => c.CondicaoVenda == string.Empty).Any())
                    ModelState.AddModelError(string.Empty, "Portal Microled: Existem itens sem Condição de Venda");

                foreach (var item in this.DUE.Itens)
                {
                    var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                    if (subItens.Where(c => c.PaisDestino == string.Empty).Any())
                    {
                        ModelState.AddModelError(string.Empty, "Portal Microled: Existem itens sem País de Destino");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogsService.Logar("GerarXML", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        public string Gerar()
        {
            var xml = string.Empty;

            if (this.DUE.Completa > 0 || this.DUE.CriadoPorNF > 0 || this.DUE.Itens.Any(c => !string.IsNullOrEmpty(c.NF)))
            {
                xml = GerarXMLDUESiscomexComNotaFiscal(this.DUE.Id);
            }
            else
            {
                if (this.DUE.SituacaoEspecial == 0)
                {
                    return $"VincularDUENF.aspx?embarquenormal=1&due={this.DUE.Id}";
                }

                xml = GerarXMLDUESiscomex(this.DUE);
            }

            //Gravar XML

            documentoUnicoExportacaoDAO.AtualizarInformacoesEnvioUltimoXMLGerado(this.DUE.Id, xml);

            return string.Empty;
        }

        public RetornoSiscomex Retificar(ModelStateDictionary ModelState)
        {
            this.DUE = documentoUnicoExportacaoDAO.ObterDUEPorId(this.DUEId);
            var retorno = new ServicoSiscomex.RetornoSiscomex();

            using (var ws = new ServicoSiscomex.WsDUE())
            {
                ws.Timeout = 900000;
                XmlDocument documentoXML = new XmlDocument();
                var xml = documentoUnicoExportacaoDAO.ObterUltimoXMLGerado(this.DUEId);
                documentoXML.LoadXml(xml);

                var cpfCertificado = _certificadoDAO.ObterCpfCertificado(this.DUE.DocumentoDeclarante);
                var documentoFormatado = string.Empty;

                if (string.IsNullOrEmpty(cpfCertificado))
                {
                    if (!string.IsNullOrEmpty(this.DUE.DocumentoDeclarante))
                    {
                        if (this.DUE.DocumentoDeclarante.Length == 14)
                        {
                            documentoFormatado = Convert.ToUInt64(this.DUE.DocumentoDeclarante).ToString(@"00\.000\.000\/0000-00");
                        }
                        else
                        {
                            documentoFormatado = Convert.ToUInt64(this.DUE.DocumentoDeclarante).ToString(@"000\.000\.000\-00");
                        }
                    }

                    var cnpjExportador = this.DUE.Itens
                        .Select(c => c.Exportador?.Documento)
                        .Distinct()
                        .FirstOrDefault();

                    cpfCertificado = _certificadoDAO.ObterCpfCertificado(cnpjExportador);

                    if (string.IsNullOrEmpty(cpfCertificado))
                    {
                        if (!string.IsNullOrEmpty(cnpjExportador))
                        {
                            documentoFormatado = Convert.ToUInt64(cnpjExportador).ToString(@"00\.000\.000\/0000-00");
                        }

                        ModelState.AddModelError(string.Empty, $"Certificado Digital para o CNPJ {documentoFormatado} não encontrado");
                        return retorno;
                    }
                }

                if (this.DUE.EnviadoSiscomex == 0)
                {
                    retorno = ws.EnviarDUESemNF(xml, cpfCertificado);
                }
                else
                {
                    if (this.DUE.Completa > 0 || this.DUE.CriadoPorNF > 0 || this.DUE.Itens.Any(c => !string.IsNullOrEmpty(c.NF)))
                    {
                        var parametros = _parametrosDAO.ObterParametros();

                        if (parametros.EnvioRetificacaoSemServico > 0)
                        {
                            retorno = EnvioRetificacao.EnviarSemServico(documentoXML.InnerXml, this.DUE.DUE.Replace("-", ""), cpfCertificado).GetAwaiter().GetResult();
                        }
                        else
                        {
                            retorno = ws.EnviarRetificacao(xml, this.DUE.DUE.Replace("-", ""), cpfCertificado);
                        }
                    }
                }

                var xmlEnviado = xml;

                xmlEnviado = xmlEnviado.Replace("\r\n", string.Empty);
                xmlEnviado = System.Text.RegularExpressions.Regex.Replace(xmlEnviado, @"\t|\n|\r", "");
                xmlEnviado = xmlEnviado.Replace("      ", " ");
                xmlEnviado = xmlEnviado.Replace("    ", " ");
                xmlEnviado = xmlEnviado.Replace("   ", " ");
                xmlEnviado = xmlEnviado.Replace("  ", " ");
                xmlEnviado = xmlEnviado.Replace(Environment.NewLine, "");

                if (retorno.Sucesso)
                {
                    if (this.DUE.EnviadoSiscomex == 0)
                    {
                        documentoUnicoExportacaoDAO.AtualizarInformacoesEnvio(this.DUE.Id, retorno.DUE, retorno.RUC, retorno.ChaveDeAcesso, xmlEnviado, retorno.XmlRetorno, 1);
                    }
                    else
                    {
                        documentoUnicoExportacaoDAO.MarcarComoRetificado(this.DUE.Id);
                    }
                }
                else
                {
                    if (retorno.Criticas != null)
                    {
                        foreach (var erro in retorno.Criticas)
                            ModelState.AddModelError(string.Empty, "Siscomex: " + erro.message);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Siscomex: " + retorno.Message);
                    }

                    documentoUnicoExportacaoDAO.AtualizarInformacoesErro(this.DUE.Id, xmlEnviado, retorno.XmlRetorno);
                }
            }
            return retorno;
        }

        private string GerarXMLDUESiscomexComNotaFiscal(int dueId)
        {
            var parametros = _parametrosDAO.ObterParametros();

            var due = documentoUnicoExportacaoDAO.ObterDUEPorId(dueId);

            var itens = documentoUnicoExportacaoDAO.ObterItensDUE(dueId);

            due.AdicionarItens(itens);

            var recintoDespachoId = string.IsNullOrEmpty(due.UnidadeDespacho.RecintoAduaneiroId)
                ? due.UnidadeDespacho.DocumentoResponsavel
                : due.UnidadeDespacho.RecintoAduaneiroId;

            var xml = $@"
                    <Declaration
                        xsi:schemaLocation='urn:wco:datamodel:WCO:GoodsDeclaration:1 GoodsDeclaration_1p0_DUE.xsd'
                        xmlns:ds='urn:wco:datamodel:WCO:GoodsDeclaration_DS:1'
                        xmlns='urn:wco:datamodel:WCO:GoodsDeclaration:1'
                        xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                            <DeclarationNFe>
                            <ID>{due.DUE}</ID>
                            <DeclarationOffice>
                                <ID listID='token'>{due.UnidadeDespacho.Id.ToString()}</ID>
                                <Warehouse>
                                    <ID>{recintoDespachoId}</ID>
                                    <TypeCode>{due.UnidadeDespacho.TipoRecinto}</TypeCode>";

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Latitude))
            {
                xml = xml + $@"<LatitudeMeasure>{due.UnidadeDespacho.Latitude}</LatitudeMeasure>";
            }

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Longitude))
            {
                xml = xml + $@"<LongitudeMeasure>{due.UnidadeDespacho.Longitude}</LongitudeMeasure>";
            }

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Endereco))
            {
                xml = xml + $@"<Address><Line>{due.UnidadeDespacho.Endereco}</Line></Address>";
            }

            xml = xml + $@"
                                </Warehouse>
                            </DeclarationOffice>
                            <AdditionalInformation>
                                <StatementCode>{due.FormaExportacao.ToString()}</StatementCode>
                                <StatementTypeCode>CUS</StatementTypeCode>
                            </AdditionalInformation>";

            if (!string.IsNullOrEmpty(due.DUE))
            {
                xml = xml + $@"<AdditionalInformation>
                                <StatementTypeCode>DEF</StatementTypeCode>
                                <StatementDescription>Vinculo de notas fiscais</StatementDescription>
                            </AdditionalInformation>";
            }

            if (due.SituacaoEspecial != 0)
            {
                xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{due.SituacaoEspecial.ToString()}</StatementCode>
                                <StatementTypeCode>AHZ</StatementTypeCode>
                            </AdditionalInformation>";
            }

            if (due.ViaEspecialTransporte != 0)
            {
                xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{due.ViaEspecialTransporte.ToString()}</StatementCode>
                                <StatementTypeCode>TRA</StatementTypeCode>
                            </AdditionalInformation>";
            }

            xml = xml + $@"<AdditionalInformation>
                                <StatementDescription languageID=''>{due.InformacoesComplementares.ToString().RemoverCaracteresEspeciais().QuebraDeLinhaXML()}</StatementDescription>
                                <LimitDateTime>10</LimitDateTime>
                                <StatementTypeCode>AAI</StatementTypeCode>
                            </AdditionalInformation>
                            <CurrencyExchange>
                                <CurrencyTypeCode>{due.MoedaNegociacao}</CurrencyTypeCode>
                            </CurrencyExchange>
                            <Declarant>
                                <ID schemeID='token'>{due.DocumentoDeclarante}</ID>
                            </Declarant>
                            <ExitOffice>
                                <ID>{due.UnidadeEmbarque.Id}</ID>
                                    <Warehouse>
                                        <ID>{due.UnidadeEmbarque.RecintoAduaneiroId}</ID>
                                        <TypeCode>{due.UnidadeEmbarque.TipoRecinto}</TypeCode>
                                    </Warehouse>
                            </ExitOffice>";

            var sequenciaItem = 1;

            foreach (var item in due.Itens)
            {
                xml = xml + $@"<GoodsShipment>";

                if (!string.IsNullOrEmpty(item.Exportador?.Descricao))
                {
                    xml = xml + $@"<Exporter>
                            <Name languageID=''>{item.Exportador?.Descricao}</Name>
                            <ID schemeID='token'>{item.Exportador?.Documento}</ID>
                            <Address>
                                <CountryCode>{item.Exportador?.Pais}</CountryCode>
                                <CountrySubDivisionCode>{string.Concat(item.Exportador?.Pais, "-", item.Exportador?.UF.ToUpper())}</CountrySubDivisionCode>
                                <Line languageID=''>{item.Exportador?.Endereco}</Line>
                            </Address>
                        </Exporter>";
                }

                var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                foreach (var subItem in subItens)
                {
                    xml = xml + $@"
	                            <GovernmentAgencyGoodsItem>
	                                <CustomsValueAmount languageID=''>{subItem.ValorMercadoriaLocalEmbarque.ToString().PPonto()}</CustomsValueAmount>
	                                <SequenceNumeric>{sequenciaItem}</SequenceNumeric>
	                                    <Destination>
		                                    <CountryCode>{subItem.PaisDestino}</CountryCode>
		                                    <GoodsMeasure>
		                                    <TariffQuantity>{subItem.QuantidadeEstatistica.ToString().PPonto()}</TariffQuantity>
		                                    </GoodsMeasure>
	                                    </Destination>";

                    if (subItem.PrioridadeCarga != 0)
                    {
                        xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{subItem.PrioridadeCarga.ToString()}</StatementCode>
                                <StatementDescription languageID=''></StatementDescription>
                                <LimitDateTime>{subItem.Limite.ToString()}</LimitDateTime>
                                <StatementTypeCode>PRI</StatementTypeCode>
                            </AdditionalInformation>";
                    }

                    xml = xml + $@"
	                            <Commodity>
		                            <Description>{subItem.DescricaoComplementar}</Description>
		                            <ValueAmount schemeID='token'>{subItem.ValorMercadoriaCondicaoVenda.ToString().PPonto()}</ValueAmount>
                                    <CommercialDescription languageID=''>{subItem.DescricaoMercadoria}</CommercialDescription>
                                    <Classification>
                                        <ID schemeID='token'>{subItem.NCM.ToString()}</ID>
                                        <IdentificationTypeCode>HS</IdentificationTypeCode>
                                    </Classification>";

                    if (parametros.ValidarAtributosCafe > 0)
                    {
                        if (!string.IsNullOrEmpty(subItem.Attr_Padrao_Qualidade))
                        {
                            if (subItem.Attr_Padrao_Qualidade.ToInt() > 0)
                            {
                                var atributoPadraoQualidade = atributosDAO.ObterAtributo(
                                    "PadraoQualidade",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Padrao_Qualidade.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoPadraoQualidade?.Atributo}</TypeCode>
                                        <Description>{atributoPadraoQualidade?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Embarque_Em))
                        {
                            if (subItem.Attr_Embarque_Em.ToInt() > 0)
                            {
                                var atributoEmbarcadoEm = atributosDAO.ObterAtributo(
                                    "EmbarcadoEm",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Embarque_Em.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoEmbarcadoEm?.Atributo}</TypeCode>
                                        <Description>{atributoEmbarcadoEm?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Tipo))
                        {
                            if (subItem.Attr_Tipo.ToInt() > 0)
                            {
                                var atributoTipo = atributosDAO.ObterAtributo(
                                    "Tipo",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Tipo.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoTipo?.Atributo}</TypeCode>
                                        <Description>{atributoTipo?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Metodo_Processamento))
                        {
                            if (subItem.Attr_Metodo_Processamento.ToInt() > 0)
                            {
                                var atributoMetodoProcessamento = atributosDAO.ObterAtributo(
                                    "MetodoProcessamento",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Metodo_Processamento.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoMetodoProcessamento?.Atributo}</TypeCode>
                                        <Description>{atributoMetodoProcessamento?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Caracteristica_Especial))
                        {
                            var atributoCaracteristicaEspecial = atributosDAO.ObterAtributo(
                                "CaracteristicaEspecial",
                                subItem.NCM.ToString(),
                                subItem.Attr_Caracteristica_Especial);

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoCaracteristicaEspecial?.Atributo}</TypeCode>
                                    <Description>{atributoCaracteristicaEspecial?.Codigo}</Description>
                                </ProductCharacteristics>";
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Outra_Caracteristica_Especial))
                        {
                            var atributoOutraCaracteristicaEspecial = atributosDAO.ObterAtributo("OutraCaracteristicaEspecial", subItem.NCM.ToString());

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoOutraCaracteristicaEspecial?.Atributo}</TypeCode>
                                    <Description>{subItem?.Attr_Outra_Caracteristica_Especial}</Description>
                                </ProductCharacteristics>";
                        }
                    }

                    xml = xml + $@"<Product>
                                        <ID schemeID='token'>{subItem.CodigoProduto.ToString()}</ID>
                                        <IdentifierTypeCode>VN</IdentifierTypeCode>
                                    </Product>";

                    xml = xml + $@"<InvoiceLine>
		                                <SequenceNumeric>{subItem.Item}</SequenceNumeric>";

                    IEnumerable<Models.NotaFiscal> notasReferenciadas = new List<Models.NotaFiscal>();

                    if (due.Completa > 0 || due.CriadoPorNF > 0)
                    {
                        // Pega somente as notas que foram cadastradas NA DUE
                        notasReferenciadas = notaFiscalDAO.ObterNotasFiscaisRemessaDUE(item.NF, item.DueId);
                    }
                    else
                    {
                        // Pega somente as notas que foram importadas PELO ARQUIVO

                        notasReferenciadas = notaFiscalDAO.ObterNotasFiscaisRemessa(item.NF);
                    }

                    foreach (var nfRef in notasReferenciadas)
                    {
                        // <SequenceNumeric>1</SequenceNumeric>
                        // Alterado em 09/04 para
                        // <SequenceNumeric>{nfRef.Item}</SequenceNumeric>

                        var nfRefItem = nfRef.Item == 0 ? 1 : nfRef.Item;

                        xml = xml + $@"
		                                <ReferencedInvoiceLine>
			                                <SequenceNumeric>{nfRefItem}</SequenceNumeric>
			                                <InvoiceIdentificationID schemeID='token'>{nfRef.ChaveNF}</InvoiceIdentificationID>
			                                <GoodsMeasure>
			                                    <TariffQuantity unitCode=''>{nfRef.QuantidadeNF.ToString().PPonto()}</TariffQuantity>
			                                </GoodsMeasure>
		                                </ReferencedInvoiceLine>";
                    }

                    xml = xml + $@"</InvoiceLine>";

                    xml = xml + $@"</Commodity>
	                            <GoodsMeasure>
		                            <NetNetWeightMeasure>{subItem.PesoLiquidoTotal.ToString().PPonto()}</NetNetWeightMeasure>
	                            </GoodsMeasure>
	                            <GovernmentProcedure>
		                            <CurrentCode>{subItem.Enquadramento1Id.ToString()}</CurrentCode>
	                            </GovernmentProcedure>";

                    if (subItem.Enquadramento2Id > 0)
                    {
                        xml = xml + $@"<GovernmentProcedure>
		                                 <CurrentCode>{subItem.Enquadramento2Id.ToString()}</CurrentCode>
	                                   </GovernmentProcedure>";
                    }

                    if (subItem.ComissaoAgente > 0)
                    {
                        xml = xml + $@"
                                    <ValuationAdjustment>
		                                <AdditionCode>149</AdditionCode>
		                                <PercentageNumeric>{subItem.ComissaoAgente.ToString().PPonto()}</PercentageNumeric>
	                                </ValuationAdjustment>";
                    }

                    var dadosAtosConcessorios = documentoUnicoExportacaoDAO.ObterAtosConcessorios(subItem.Id);

                    if (dadosAtosConcessorios != null)
                    {
                        foreach (var atoConcessorio in dadosAtosConcessorios)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>AC</CategoryCode>
                                      <ID>{atoConcessorio.Numero}</ID>
                                      <ItemID>{atoConcessorio.NumeroItem}</ItemID>
                                      <QuantityQuantity>{atoConcessorio.QuantidadeUtilizada.ToString().PPonto()}</QuantityQuantity>
                                      <ValueWithExchangeCoverAmount>{atoConcessorio.VMLEComCoberturaCambial.ToString().PPonto()}</ValueWithExchangeCoverAmount>
                                      <ValueWithoutExchangeCoverAmount>{atoConcessorio.VMLESemCoberturaCambial.ToString().PPonto()}</ValueWithoutExchangeCoverAmount>
                                      <DrawbackHsClassification>{atoConcessorio.NCMItem}</DrawbackHsClassification>
                                      <DrawbackRecipientId>{atoConcessorio.CNPJBeneficiario}</DrawbackRecipientId>
                                   </AdditionalDocument>";
                        }
                    }

                    var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(subItem.Id);

                    if (lpcos != null)
                    {
                        foreach (var lpco in lpcos)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>LPCO</CategoryCode>
                                      <ID>{lpco.Numero}</ID>
                                   </AdditionalDocument>";
                        }
                    }

                    xml = xml + "</GovernmentAgencyGoodsItem>";
                }

                if (!string.IsNullOrEmpty(item.Importador?.Descricao))
                {
                    xml = xml + $@"
                            <Importer>
                                <Name languageID=''>{item.Importador?.Descricao}</Name>
                                <Address>
                                    <CountryCode>{item.Importador?.Pais}</CountryCode>
                                    <Line languageID=''>{item.Importador?.Endereco}</Line>
                                </Address>
                            </Importer>";
                }

                IEnumerable<Models.NotaFiscal> notasRef = new List<Models.NotaFiscal>();

                if (due.Completa > 0 || due.CriadoPorNF > 0)
                {
                    // Pega somente as notas que foram cadastradas NA DUE
                    notasRef = notaFiscalDAO.ObterNotasFiscaisRemessaDUE(item.NF, item.DueId);
                }
                else
                {
                    // Pega somente as notas que foram importadas PELO ARQUIVO
                    notasRef = notaFiscalDAO.ObterNotasFiscaisRemessa(item.NF);
                }

                xml = xml + $@"<Invoice>
	                            <ID schemeID='token'>{item.NF}</ID>
	                            <TypeCode>388</TypeCode>";

                var notasRefSemRepeticao = notasRef
                        .Select(c => new
                        {
                            c.ChaveNF,
                            c.CnpjNF
                        }).ToList()
                        .Distinct().ToList();

                foreach (var nfRef in notasRefSemRepeticao)
                {
                    var cpfCnpj = string.Empty;
                    var cpfFormatado = string.Empty;

                    if (nfRef.CnpjNF.Length >= 11)
                    {
                        cpfFormatado = nfRef.CnpjNF.Substring(nfRef.CnpjNF.Length - 11);
                    }

                    if (ValidacaoCPF.Validar(cpfFormatado))
                        cpfCnpj = cpfFormatado;
                    else
                        cpfCnpj = nfRef.CnpjNF;

                    xml = xml + $@"
                                <ReferencedInvoice>
		                            <ID schemeID='token'>{nfRef.ChaveNF}</ID>
		                            <TypeCode>REM</TypeCode>
		                            <Submitter>
		                                <ID schemeID='token'>{cpfCnpj}</ID>
		                            </Submitter>
	                            </ReferencedInvoice>";
                }

                xml = xml + $@"</Invoice>";

                xml = xml + $@"<TradeTerms>
	                                <ConditionCode>{item.CondicaoVenda}</ConditionCode>
	                           </TradeTerms>
                        </GoodsShipment>";

                sequenciaItem++;
            }

            xml = xml + $@"
                            <UCR>
	                            <TraderAssignedReferenceID>{due.RUC}</TraderAssignedReferenceID>
                            </UCR>
                        </DeclarationNFe>
                    </Declaration>";

            return xml;
        }

        private string GerarXMLDUESiscomex(DUEMaster due)
        {
            var parametros = _parametrosDAO.ObterParametros();

            var recintoDespachoId = string.IsNullOrEmpty(due.UnidadeDespacho.RecintoAduaneiroId)
                ? due.UnidadeDespacho.DocumentoResponsavel
                : due.UnidadeDespacho.RecintoAduaneiroId;

            var xml = $@"<Declaration
                        xsi:schemaLocation='urn:wco:datamodel:WCO:GoodsDeclaration:1 GoodsDeclaration_1p0_DUE.xsd'
                        xmlns:ds='urn:wco:datamodel:WCO:GoodsDeclaration_DS:1'
                        xmlns='urn:wco:datamodel:WCO:GoodsDeclaration:1'
                        xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                          <DeclarationNoNF>
                            <ID schemeID='token'/>
                            <DeclarationOffice>
                              <ID listID='token'>{due.UnidadeDespacho.Id}</ID>
                              <Warehouse>
                                <ID schemeID='token'>{recintoDespachoId}</ID>
                                <TypeCode>{due.UnidadeDespacho.TipoRecinto.ToString()}</TypeCode>";

            if (due.UnidadeDespacho.TipoRecinto != 281)
            {
                xml = xml + $@"
                    <LatitudeMeasure unitCode=''>{due.UnidadeDespacho.Latitude}</LatitudeMeasure>
                    <LongitudeMeasure unitCode=''>{due.UnidadeDespacho.Longitude}</LongitudeMeasure>
                    <Address>
                        <Line languageID=''>{due.UnidadeDespacho.Endereco}</Line>
                    </Address>";
            }

            xml = xml + $@"
                             </Warehouse>
                    </DeclarationOffice>
                    <AdditionalInformation>
                        <StatementCode>{due.FormaExportacao.ToString()}</StatementCode>
                        <StatementTypeCode>CUS</StatementTypeCode>
                    </AdditionalInformation>";

            if (due.ViaEspecialTransporte != 0)
            {
                xml = xml + $@"
                    <AdditionalInformation>
                        <StatementCode>{due.ViaEspecialTransporte.ToString()}</StatementCode>
                        <StatementTypeCode>TRA</StatementTypeCode>
                    </AdditionalInformation>";
            }

            if (due.SituacaoEspecial != 0)
            {
                xml = xml + $@"
                    <AdditionalInformation>
                        <StatementCode>{due.SituacaoEspecial.ToString()}</StatementCode>
                        <StatementTypeCode>AHZ</StatementTypeCode>
                    </AdditionalInformation>";
            }

            xml = xml + $@"<AdditionalInformation>
                        <StatementDescription languageID=''>{due.InformacoesComplementares}</StatementDescription>
                        <StatementTypeCode>AAI</StatementTypeCode>
                    </AdditionalInformation>
                    <CurrencyExchange>
                        <CurrencyTypeCode>{due.MoedaNegociacao}</CurrencyTypeCode>
                    </CurrencyExchange>
                    <Declarant>
                        <ID schemeID='token'>{due.DocumentoDeclarante}</ID>
                    </Declarant>
                    <ExitOffice>
                        <ID schemeID='token'>{due.UnidadeEmbarque.Id.ToString()}</ID>
                        <Warehouse>
                        <ID schemeID='token'>{due.UnidadeEmbarque.RecintoAduaneiroId.ToString()}</ID>
                        <TypeCode>{due.UnidadeEmbarque.TipoRecinto.ToString()}</TypeCode>
                        </Warehouse>
                    </ExitOffice>";

            var sequenciaItem = 1;

            foreach (var item in due.Itens)
            {
                xml = xml + $@"<GoodsShipment>";

                if (!string.IsNullOrEmpty(item.Exportador.Descricao))
                {
                    xml = xml + $@"
                        <Exporter>
                            <Name languageID=''>{item.Exportador.Descricao}</Name>
                            <ID schemeID='token'>{item.Exportador.Documento}</ID>
                            <Address>
                                <CountryCode>{item.Exportador.Pais}</CountryCode>
                                <CountrySubDivisionCode>{string.Concat(item.Exportador.Pais, "-", item.Exportador.UF.ToUpper())}</CountrySubDivisionCode>
                                <Line languageID=''>{item.Exportador.Endereco}</Line>
                            </Address>
                        </Exporter>";
                }

                var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                foreach (var subItem in subItens)
                {
                    xml = xml + $@"
                        <GovernmentAgencyGoodsItem>
                            <CustomsValueAmount languageID=''>{subItem.ValorMercadoriaLocalEmbarque.ToString().PPonto()}</CustomsValueAmount>
                            <SequenceNumeric>{sequenciaItem}</SequenceNumeric>
                            <Destination>
                                <CountryCode>{subItem.PaisDestino}</CountryCode>
                                <GoodsMeasure>
                                <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>
                            </Destination>";

                    if (subItem.PrioridadeCarga != 0)
                    {
                        xml = xml + $@"<AdditionalInformation>
                                            <StatementCode>{subItem.PrioridadeCarga.ToString()}</StatementCode>
                                            <StatementDescription languageID=''></StatementDescription>
                                            <LimitDateTime>{subItem.Limite.ToString()}</LimitDateTime>
                                            <StatementTypeCode>PRI</StatementTypeCode>
                                       </AdditionalInformation>";
                    }

                    xml = xml + $@"
                            <Commodity>
                                <Description languageID=''>{subItem.DescricaoComplementar}</Description>
                                <ValueAmount schemeID='token'>{subItem.ValorMercadoriaCondicaoVenda.ToString().PPonto()}</ValueAmount>
                                <CommercialDescription languageID=''>{subItem.DescricaoMercadoria}</CommercialDescription>
                                <Classification>
                                    <ID schemeID='token'>{subItem.NCM.ToString()}</ID>
                                    <IdentificationTypeCode>HS</IdentificationTypeCode>
                                </Classification>
                                <GoodsMeasure>
                                    <TypeCode>AAF</TypeCode>
                                    <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>
                                <GoodsMeasure>
                                    <UnitDescription languageID=''>{subItem.DescricaoUnidade}</UnitDescription>
                                    <TypeCode>ABW</TypeCode>
                                    <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>";

                    if (parametros.ValidarAtributosCafe > 0)
                    {
                        if (!string.IsNullOrEmpty(subItem.Attr_Padrao_Qualidade))
                        {
                            if (subItem.Attr_Padrao_Qualidade.ToInt() > 0)
                            {
                                var atributoPadraoQualidade = atributosDAO.ObterAtributo(
                                    "PadraoQualidade",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Padrao_Qualidade.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoPadraoQualidade.Atributo}</TypeCode>
                                        <Description>{atributoPadraoQualidade.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Embarque_Em))
                        {
                            if (subItem.Attr_Embarque_Em.ToInt() > 0)
                            {
                                var atributoEmbarcadoEm = atributosDAO.ObterAtributo(
                                    "EmbarcadoEm",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Embarque_Em.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoEmbarcadoEm.Atributo}</TypeCode>
                                        <Description>{atributoEmbarcadoEm.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Tipo))
                        {
                            if (subItem.Attr_Tipo.ToInt() > 0)
                            {
                                var atributoTipo = atributosDAO.ObterAtributo(
                                    "Tipo",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Tipo.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoTipo.Atributo}</TypeCode>
                                        <Description>{atributoTipo.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Metodo_Processamento))
                        {
                            if (subItem.Attr_Metodo_Processamento.ToInt() > 0)
                            {
                                var atributoMetodoProcessamento = atributosDAO.ObterAtributo(
                                    "MetodoProcessamento",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Metodo_Processamento.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoMetodoProcessamento.Atributo}</TypeCode>
                                        <Description>{atributoMetodoProcessamento.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Caracteristica_Especial))
                        {
                            var atributoCaracteristicaEspecial = atributosDAO.ObterAtributo(
                                "CaracteristicaEspecial",
                                subItem.NCM.ToString(),
                                subItem.Attr_Caracteristica_Especial);

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoCaracteristicaEspecial.Atributo}</TypeCode>
                                    <Description>{atributoCaracteristicaEspecial.Codigo}</Description>
                                </ProductCharacteristics>";
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Outra_Caracteristica_Especial))
                        {
                            var atributoOutraCaracteristicaEspecial = atributosDAO.ObterAtributo("OutraCaracteristicaEspecial", subItem.NCM.ToString());

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoOutraCaracteristicaEspecial.Atributo}</TypeCode>
                                    <Description>{subItem.Attr_Outra_Caracteristica_Especial}</Description>
                                </ProductCharacteristics>";
                        }
                    }

                    xml = xml + $@" <Product>
                                    <ID schemeID='token'>{subItem.CodigoProduto.ToString()}</ID>
                                    <IdentifierTypeCode>VN</IdentifierTypeCode>
                                </Product>
                            </Commodity>
                            <GoodsMeasure>
                                <NetNetWeightMeasure unitCode=''>{subItem.PesoLiquidoTotal.ToString().PPonto()}</NetNetWeightMeasure>
                            </GoodsMeasure>";

                    if (subItem.Enquadramento1Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento1Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento2Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento2Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento3Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento3Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento4Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento4Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (!string.IsNullOrEmpty(due.DUE))
                    {
                        xml = xml + $@"
                            <PreviousDocument>
                                <ID schemeID='token'>ID</ID>
                                <TypeCode>830</TypeCode>
                            </PreviousDocument>";
                    }

                    if (subItem.ComissaoAgente != 0)
                    {
                        xml = xml + $@"
                            <ValuationAdjustment>
                                <AdditionCode>149</AdditionCode>
                                <PercentageNumeric>{subItem.ComissaoAgente.ToString().PPonto()}</PercentageNumeric>
                            </ValuationAdjustment>";
                    }

                    var dadosAtosConcessorios = documentoUnicoExportacaoDAO.ObterAtosConcessorios(subItem.Id);

                    if (dadosAtosConcessorios != null)
                    {
                        foreach (var atoConcessorio in dadosAtosConcessorios)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>AC</CategoryCode>
                                      <ID>{atoConcessorio.Numero}</ID>
                                      <ItemID>{atoConcessorio.NumeroItem}</ItemID>
                                      <QuantityQuantity>{atoConcessorio.QuantidadeUtilizada.ToString().PPonto()}</QuantityQuantity>
                                      <ValueWithExchangeCoverAmount>{atoConcessorio.VMLEComCoberturaCambial.ToString().PPonto()}</ValueWithExchangeCoverAmount>
                                      <ValueWithoutExchangeCoverAmount>{atoConcessorio.VMLESemCoberturaCambial.ToString().PPonto()}</ValueWithoutExchangeCoverAmount>
                                      <DrawbackHsClassification>{atoConcessorio.NCMItem}</DrawbackHsClassification>
                                      <DrawbackRecipientId>{atoConcessorio.CNPJBeneficiario}</DrawbackRecipientId>
                                   </AdditionalDocument>";
                        }
                    }

                    var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(subItem.Id);

                    if (lpcos != null)
                    {
                        foreach (var lpco in lpcos)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>LPCO</CategoryCode>
                                      <ID>{lpco.Numero}</ID>
                                   </AdditionalDocument>";
                        }
                    }

                    xml = xml + "</GovernmentAgencyGoodsItem>";
                }

                if (!string.IsNullOrEmpty(item.Importador.Descricao))
                {
                    xml = xml + $@"
                        <Importer>
                            <Name languageID=''>{item.Importador.Descricao}</Name>
                            <Address>
                                <CountryCode>{item.Importador.Pais}</CountryCode>
                                <Line languageID=''>{item.Importador.Endereco.Replace("'", " ")}</Line>
                            </Address>
                        </Importer>
                        <Invoice>
                            <TypeCode>388</TypeCode>";
                }

                if (item.MotivoDispensaNF != 0)
                {
                    xml = xml + $@"
                        <AdditionalInformation>
                            <StatementCode>{item.MotivoDispensaNF.ToString()}</StatementCode>
                            <StatementTypeCode>ACG</StatementTypeCode>
                        </AdditionalInformation>";
                }

                xml = xml + $@"
                    </Invoice>
                        <TradeTerms>
                            <ConditionCode>{item.CondicaoVenda}</ConditionCode>
                        </TradeTerms>
                </GoodsShipment>";

                sequenciaItem++;
            }

            xml = xml + $@"<UCR>
                            <TraderAssignedReferenceID schemeID='token'>{due.RUC}</TraderAssignedReferenceID>
                           </UCR>
                        </DeclarationNoNF>
                    </Declaration>";

            return xml;
        }
    }
}