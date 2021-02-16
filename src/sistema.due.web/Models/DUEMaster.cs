using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Sistema.DUE.Web.Models
{
    public class DUEMaster
    {
        public DUEMaster()
        {

        }

        public DUEMaster(
            string documentoDeclarante,
            string moedaNegociacao,
            string ruc,
            int situacaoEspecial,
            int formaExportacao,
            int viaEspecialTransporte,
            string informacoesComplementares,
            int usuarioCadastro,
            decimal valorUnitVMLE_Default,
            decimal valorUnitVMCV_Default,
            string paisDestino_Default,
            int enquadramento1_Default,
            int enquadramento2_Default,
            int enquadramento3_Default,
            int enquadramento4_Default,
            string lpco_Default,
            string condicaoVendaDefault,
            int prioridade_Default,
            string descricaoComplementar_Default,
            decimal comissaoAgente_Default,
            string ac_Tipo_Default,
            string ac_Numero_Default,
            string ac_CNPJ_Default,
            string ac_Item_Default,
            string ac_NCM_Default,
            decimal ac_Qtde_Default,
            decimal ac_VMLE_Sem_Cob_Default,
            decimal ac_VMLE_Com_Cob_Default,
            string padraoQualidade_Default,
            string embarcadoEm_Default,
            string tipo_Default,
            string metodoProcessamento_Default,
            string caracteristicaEspecial_Default,
            string outraCaracteristicaEspecial_Default,
            int embalagemFinal_Default,
            string ncm_default)
        {
            DocumentoDeclarante = documentoDeclarante;
            MoedaNegociacao = moedaNegociacao;
            RUC = ruc;
            SituacaoEspecial = situacaoEspecial;
            FormaExportacao = formaExportacao;
            ViaEspecialTransporte = viaEspecialTransporte;
            InformacoesComplementares = informacoesComplementares;
            UsuarioCadastro = usuarioCadastro;
            ValorUnitVMLE_Default = valorUnitVMLE_Default;
            ValorUnitVMCV_Default = valorUnitVMCV_Default;
            PaisDestino_Default = paisDestino_Default;
            Enquadramento1_Default = enquadramento1_Default;
            Enquadramento2_Default = enquadramento2_Default;
            Enquadramento3_Default = enquadramento3_Default;
            Enquadramento4_Default = enquadramento4_Default;
            LPCO_Default = lpco_Default;
            CondicaoVenda_Default = condicaoVendaDefault;
            Prioridade_Default = prioridade_Default;
            DescricaoComplementar_Default = descricaoComplementar_Default;
            ComissaoAgente_Default = comissaoAgente_Default;
            AC_Tipo_Default = ac_Tipo_Default;
            AC_Numero_Default = ac_Numero_Default;
            AC_CNPJ_Default = ac_CNPJ_Default;
            AC_Item_Default = ac_Item_Default;
            AC_NCM_Default = ac_NCM_Default;
            AC_Qtde_Default = ac_Qtde_Default;
            AC_VMLE_Sem_Cob_Default = ac_VMLE_Sem_Cob_Default;
            AC_VMLE_Com_Cob_Default = ac_VMLE_Com_Cob_Default;

            Attr_Padrao_Qualidade_Default = padraoQualidade_Default;
            Attr_Embarque_Em_Default = embarcadoEm_Default;
            Attr_Tipo_Default = tipo_Default;
            Attr_Metodo_Processamento_Default = metodoProcessamento_Default;
            Attr_Caracteristica_Especial_Default = caracteristicaEspecial_Default;
            Attr_Outra_Caracteristica_Especial_Default = outraCaracteristicaEspecial_Default;
            Attr_Embalagem_Final_Default = embalagemFinal_Default;

            Ncm_Default = ncm_default;

            UnidadeDespacho = new UnidadeDespacho();
            UnidadeEmbarque = new UnidadeEmbarque();

            Itens = new List<DUEItem>();
        }

        public DUEMaster(
          string documentoDeclarante,
          string moedaNegociacao,
          string ruc,
          int situacaoEspecial,
          int formaExportacao,
          int viaEspecialTransporte,
          string informacoesComplementares,
          int usuarioCadastro)
        {
            DocumentoDeclarante = documentoDeclarante;
            MoedaNegociacao = moedaNegociacao;
            RUC = ruc;
            SituacaoEspecial = situacaoEspecial;
            FormaExportacao = formaExportacao;
            ViaEspecialTransporte = viaEspecialTransporte;
            InformacoesComplementares = informacoesComplementares;
            UsuarioCadastro = usuarioCadastro;

            UnidadeDespacho = new UnidadeDespacho();
            UnidadeEmbarque = new UnidadeEmbarque();

            Itens = new List<DUEItem>();
        }

        public int Id { get; set; }

        public string DUE { get; set; }

        public string DocumentoDeclarante { get; set; }

        public string MoedaNegociacao { get; set; }

        public string RUC { get; set; }

        public int SituacaoEspecial { get; set; }

        public int FormaExportacao { get; set; }

        public int ViaEspecialTransporte { get; set; }

        public UnidadeDespacho UnidadeDespacho { get; set; }

        public UnidadeEmbarque UnidadeEmbarque { get; set; }

        public string InformacoesComplementares { get; set; }

        public DateTime DataCadastro { get; set; }

        public int UsuarioCadastro { get; set; }

        public List<DUEItem> Itens { get; set; }

        public int EnviadoSiscomex { get; set; }

        public int Completa { get; set; }

        public int CriadoPorNF { get; set; }

        public string ChaveAcesso { get; set; }

        public int ImportadoSiscomex { get; set; }

        public decimal ValorUnitVMLE_Default { get; set; }

        public decimal ValorUnitVMCV_Default { get; set; }

        public string PaisDestino_Default { get; set; }

        public string LPCO_Default { get; set; }

        public int Enquadramento1_Default { get; set; }

        public int Enquadramento2_Default { get; set; }

        public int Enquadramento3_Default { get; set; }

        public int Enquadramento4_Default { get; set; }

        public string CondicaoVenda_Default { get; set; }

        public int Prioridade_Default { get; set; }

        public string DescricaoComplementar_Default { get; set; }

        public decimal ComissaoAgente_Default { get; set; }

        public string AC_Tipo_Default { get; set; }

        public string AC_Exp_Benefic_Default { get; set; }

        public string AC_Numero_Default { get; set; }

        public string AC_CNPJ_Default { get; set; }

        public string AC_Item_Default { get; set; }

        public string AC_NCM_Default { get; set; }

        public decimal AC_Qtde_Default { get; set; }

        public decimal AC_VMLE_Sem_Cob_Default { get; set; }

        public decimal AC_VMLE_Com_Cob_Default { get; set; }

        public string Attr_Padrao_Qualidade_Default { get; set; }

        public string Attr_Embarque_Em_Default { get; set; }

        public string Attr_Tipo_Default { get; set; }

        public string Attr_Metodo_Processamento_Default { get; set; }

        public string Attr_Caracteristica_Especial_Default { get; set; }

        public string Attr_Outra_Caracteristica_Especial_Default { get; set; }

        public int Attr_Embalagem_Final_Default { get; set; }

        public string Ncm_Default { get; set; }

        public string DescricaoSituacao { get; set; }

        public string DataSituacaoDUE { get; set; }

        public string StatusSiscomex { get; set; }

        public string SituacaoDUE { get; set; }

        public int UltimoXMLFoiGerado { get; set; }

        public XDocument UltimoXMLGeradoIn { set { UltimoXMLGerado = value.ToString(); } }
        public string UltimoXMLGerado { get; set; }
        public XDocument UltimoXMLGeradoOut { get { return XDocument.Parse(UltimoXMLGerado); } }


        public void AdicionarItens(IEnumerable<DUEItem> itens)
        {
            if (itens != null)
                Itens = itens.ToList();
        }

        public void AdicionarUnidadeDespacho(UnidadeDespacho unidadeDespacho)
        {
            if (unidadeDespacho != null)
                UnidadeDespacho = unidadeDespacho;
        }

        public void AdicionarUnidadeEmbarque(UnidadeEmbarque unidadeEmbarque)
        {
            if (unidadeEmbarque != null)
                UnidadeEmbarque = unidadeEmbarque;
        }
    }
}