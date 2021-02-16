using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.DTO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sistema.DUE.Web.DAO
{
    public class DocumentoUnicoExportacaoDAO
    {
        public int RegistrarDUE(DUEMaster due)
        {

            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "DocumentoDeclarante", value: due.DocumentoDeclarante, direction: ParameterDirection.Input);
                parametros.Add(name: "MoedaNegociacao", value: due.MoedaNegociacao, direction: ParameterDirection.Input);
                parametros.Add(name: "DUE", value: due.DUE, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveAcesso", value: due.ChaveAcesso, direction: ParameterDirection.Input);
                parametros.Add(name: "RUC", value: due.RUC, direction: ParameterDirection.Input);
                parametros.Add(name: "SituacaoEspecial", value: due.SituacaoEspecial, direction: ParameterDirection.Input);
                parametros.Add(name: "FormaExportacao", value: due.FormaExportacao, direction: ParameterDirection.Input);
                parametros.Add(name: "ViaEspecialTransporte", value: due.ViaEspecialTransporte, direction: ParameterDirection.Input);
                parametros.Add(name: "UnidadeDespachoId", value: due.UnidadeDespacho.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoRecintoAduaneiroDespacho", value: due.UnidadeDespacho.TipoRecinto, direction: ParameterDirection.Input);
                parametros.Add(name: "RecintoAduaneiroDespachoId", value: due.UnidadeDespacho.RecintoAduaneiroId, direction: ParameterDirection.Input);
                parametros.Add(name: "DocumentoResponsavelRecintoDespacho", value: due.UnidadeDespacho.DocumentoResponsavel, direction: ParameterDirection.Input);
                parametros.Add(name: "LatitudeRecintoDespacho", value: due.UnidadeDespacho.Latitude, direction: ParameterDirection.Input);
                parametros.Add(name: "LongitudeRecintoDespacho", value: due.UnidadeDespacho.Longitude, direction: ParameterDirection.Input);
                parametros.Add(name: "EnderecoRecintoDespacho", value: due.UnidadeDespacho.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "UnidadeEmbarqueId", value: due.UnidadeEmbarque.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoRecintoAduaneiroEmbarque", value: due.UnidadeEmbarque.TipoRecinto, direction: ParameterDirection.Input);
                parametros.Add(name: "RecintoAduaneiroEmbarqueId", value: due.UnidadeEmbarque.RecintoAduaneiroId, direction: ParameterDirection.Input);
                parametros.Add(name: "EnderecoReferenciaRecintoAduaneiroEmbarque", value: due.UnidadeEmbarque.EnderecoReferencia, direction: ParameterDirection.Input);
                parametros.Add(name: "InformacoesComplementares", value: due.InformacoesComplementares, direction: ParameterDirection.Input);
                parametros.Add(name: "DataCadastro", value: DateTime.Now, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioCadastro", value: due.UsuarioCadastro, direction: ParameterDirection.Input);
                parametros.Add(name: "EnviadoSiscomex", value: due.EnviadoSiscomex, direction: ParameterDirection.Input);
                parametros.Add(name: "ImportadoSiscomex", value: due.ImportadoSiscomex, direction: ParameterDirection.Input);
                parametros.Add(name: "Completa", value: due.Completa, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMLE_Default", value: due.ValorUnitVMLE_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMCV_Default", value: due.ValorUnitVMCV_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "PaisDestino_Default", value: due.PaisDestino_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento1_Default", value: due.Enquadramento1_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento2_Default", value: due.Enquadramento2_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento3_Default", value: due.Enquadramento3_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento4_Default", value: due.Enquadramento4_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "CondicaoVenda_Default", value: due.CondicaoVenda_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "LPCO_Default", value: due.LPCO_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Prioridade_Default", value: due.Prioridade_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoComplementar_Default", value: due.DescricaoComplementar_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "ComissaoAgente_Default", value: due.ComissaoAgente_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Tipo_Default", value: due.AC_Tipo_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Numero_Default", value: due.AC_Numero_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_CNPJ_Default", value: due.AC_CNPJ_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Item_Default", value: due.AC_Item_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_NCM_Default", value: due.AC_NCM_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Qtde_Default", value: due.AC_Qtde_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_VMLE_Sem_Cob_Default", value: due.AC_VMLE_Sem_Cob_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_VMLE_Com_Cob_Default", value: due.AC_VMLE_Com_Cob_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Padrao_Qualidade_Default", value: due.Attr_Padrao_Qualidade_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embarque_Em_Default", value: due.Attr_Embarque_Em_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Tipo_Default", value: due.Attr_Tipo_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Metodo_Processamento_Default", value: due.Attr_Metodo_Processamento_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Caracteristica_Especial_Default", value: due.Attr_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Outra_Caracteristica_Especial_Default", value: due.Attr_Outra_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embalagem_Final_Default", value: due.Attr_Embalagem_Final_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Ncm_Default", value: due.Ncm_Default, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(@"
                        INSERT INTO 
                            [dbo].[Tb_DUE] (
                                DocumentoDeclarante,
                                MoedaNegociacao,
                                DUE,
                                ChaveAcesso,
                                RUC,
                                SituacaoEspecial,
                                FormaExportacao,
                                ViaEspecialTransporte,
                                UnidadeDespachoId,
                                TipoRecintoAduaneiroDespacho,
                                RecintoAduaneiroDespachoId,
                                DocumentoResponsavelRecintoDespacho,
                                LatitudeRecintoDespacho,
                                LongitudeRecintoDespacho,
                                EnderecoRecintoDespacho,
                                UnidadeEmbarqueId,
                                TipoRecintoAduaneiroEmbarque,
                                RecintoAduaneiroEmbarqueId,
                                EnderecoReferenciaRecintoAduaneiroEmbarque,
                                InformacoesComplementares,
                                DataCadastro,
                                UsuarioCadastro,
                                EnviadoSiscomex,
                                ImportadoSiscomex,
                                Completa,
                                ValorUnitVMLE_Default,
                                ValorUnitVMCV_Default,
                                PaisDestino_Default,
                                Enquadramento1_Default,
                                Enquadramento2_Default,
                                Enquadramento3_Default,
                                Enquadramento4_Default,
                                CondicaoVenda_Default,
                                LPCO_Default,
                                Prioridade_Default,
                                DescricaoComplementar_Default,
                                ComissaoAgente_Default,
                                AC_Tipo_Default,
                                AC_Numero_Default,
                                AC_CNPJ_Default,
                                AC_Item_Default,
                                AC_NCM_Default,
                                AC_Qtde_Default,
                                AC_VMLE_Sem_Cob_Default,
                                AC_VMLE_Com_Cob_Default,
                                Attr_Padrao_Qualidade_Default,
                                Attr_Tipo_Default,
                                Attr_Metodo_Processamento_Default,
                                Attr_Outra_Caracteristica_Especial_Default,
                                Attr_Caracteristica_Especial_Default,
                                Attr_Embarque_Em_Default,
                                Attr_Embalagem_Final_Default,
                                NCM_Default
                            ) VALUES (
                                @DocumentoDeclarante,
                                @MoedaNegociacao,
                                @DUE,
                                @ChaveAcesso,
                                @RUC,
                                @SituacaoEspecial,
                                @FormaExportacao,
                                @ViaEspecialTransporte,
                                @UnidadeDespachoId,
                                @TipoRecintoAduaneiroDespacho,
                                @RecintoAduaneiroDespachoId,
                                @DocumentoResponsavelRecintoDespacho,
                                @LatitudeRecintoDespacho,
                                @LongitudeRecintoDespacho,
                                @EnderecoRecintoDespacho,
                                @UnidadeEmbarqueId,
                                @TipoRecintoAduaneiroEmbarque,
                                @RecintoAduaneiroEmbarqueId,
                                @EnderecoReferenciaRecintoAduaneiroEmbarque,
                                @InformacoesComplementares,
                                @DataCadastro,
                                @UsuarioCadastro,
                                @EnviadoSiscomex,
                                @ImportadoSiscomex,
                                @Completa,
                                @ValorUnitVMLE_Default,
                                @ValorUnitVMCV_Default,
                                @PaisDestino_Default,
                                @Enquadramento1_Default,
                                @Enquadramento2_Default,
                                @Enquadramento3_Default,
                                @Enquadramento4_Default,
                                @CondicaoVenda_Default,
                                @LPCO_Default,
                                @Prioridade_Default,
                                @DescricaoComplementar_Default,
                                @ComissaoAgente_Default,
                                @AC_Tipo_Default,
                                @AC_Numero_Default,
                                @AC_CNPJ_Default,
                                @AC_Item_Default,
                                @AC_NCM_Default,
                                @AC_Qtde_Default,
                                @AC_VMLE_Sem_Cob_Default,
                                @AC_VMLE_Com_Cob_Default,
                                @Attr_Padrao_Qualidade_Default,
                                @Attr_Tipo_Default,
                                @Attr_Metodo_Processamento_Default,
                                @Attr_Outra_Caracteristica_Especial_Default,
                                @Attr_Caracteristica_Especial_Default,
                                @Attr_Embarque_Em_Default,
                                @Attr_Embalagem_Final_Default,
                                @Ncm_Default); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
            }


        }

        public void AtualizarDUE(DUEMaster due)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: due.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "DocumentoDeclarante", value: due.DocumentoDeclarante, direction: ParameterDirection.Input);
                parametros.Add(name: "MoedaNegociacao", value: due.MoedaNegociacao, direction: ParameterDirection.Input);
                parametros.Add(name: "RUC", value: due.RUC, direction: ParameterDirection.Input);
                parametros.Add(name: "SituacaoEspecial", value: due.SituacaoEspecial, direction: ParameterDirection.Input);
                parametros.Add(name: "FormaExportacao", value: due.FormaExportacao, direction: ParameterDirection.Input);
                parametros.Add(name: "ViaEspecialTransporte", value: due.ViaEspecialTransporte, direction: ParameterDirection.Input);
                parametros.Add(name: "UnidadeDespachoId", value: due.UnidadeDespacho.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoRecintoAduaneiroDespacho", value: due.UnidadeDespacho.TipoRecinto, direction: ParameterDirection.Input);
                parametros.Add(name: "RecintoAduaneiroDespachoId", value: due.UnidadeDespacho.RecintoAduaneiroId, direction: ParameterDirection.Input);
                parametros.Add(name: "DocumentoResponsavelRecintoDespacho", value: due.UnidadeDespacho.DocumentoResponsavel, direction: ParameterDirection.Input);
                parametros.Add(name: "LatitudeRecintoDespacho", value: due.UnidadeDespacho.Latitude, direction: ParameterDirection.Input);
                parametros.Add(name: "LongitudeRecintoDespacho", value: due.UnidadeDespacho.Longitude, direction: ParameterDirection.Input);
                parametros.Add(name: "EnderecoRecintoDespacho", value: due.UnidadeDespacho.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "UnidadeEmbarqueId", value: due.UnidadeEmbarque.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoRecintoAduaneiroEmbarque", value: due.UnidadeEmbarque.TipoRecinto, direction: ParameterDirection.Input);
                parametros.Add(name: "RecintoAduaneiroEmbarqueId", value: due.UnidadeEmbarque.RecintoAduaneiroId, direction: ParameterDirection.Input);
                parametros.Add(name: "EnderecoReferenciaRecintoAduaneiroEmbarque", value: due.UnidadeEmbarque.EnderecoReferencia, direction: ParameterDirection.Input);
                parametros.Add(name: "InformacoesComplementares", value: due.InformacoesComplementares, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMLE_Default", value: due.ValorUnitVMLE_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMCV_Default", value: due.ValorUnitVMCV_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "PaisDestino_Default", value: due.PaisDestino_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento1_Default", value: due.Enquadramento1_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento2_Default", value: due.Enquadramento2_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento3_Default", value: due.Enquadramento3_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento4_Default", value: due.Enquadramento4_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "CondicaoVenda_Default", value: due.CondicaoVenda_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "LPCO_Default", value: due.LPCO_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Prioridade_Default", value: due.Prioridade_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoComplementar_Default", value: due.DescricaoComplementar_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "ComissaoAgente_Default", value: due.ComissaoAgente_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Numero_Default", value: due.AC_Numero_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_CNPJ_Default", value: due.AC_CNPJ_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Item_Default", value: due.AC_Item_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_NCM_Default", value: due.AC_NCM_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_Qtde_Default", value: due.AC_Qtde_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_VMLE_Sem_Cob_Default", value: due.AC_VMLE_Sem_Cob_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "AC_VMLE_Com_Cob_Default", value: due.AC_VMLE_Com_Cob_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Padrao_Qualidade_Default", value: due.Attr_Padrao_Qualidade_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embarque_Em_Default", value: due.Attr_Embarque_Em_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Tipo_Default", value: due.Attr_Tipo_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Metodo_Processamento_Default", value: due.Attr_Metodo_Processamento_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Caracteristica_Especial_Default", value: due.Attr_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Outra_Caracteristica_Especial_Default", value: due.Attr_Outra_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embalagem_Final_Default", value: due.Attr_Embalagem_Final_Default, direction: ParameterDirection.Input);
                parametros.Add(name: "Ncm_Default", value: due.Ncm_Default, direction: ParameterDirection.Input);

                con.Execute(@"
                        UPDATE [dbo].[Tb_DUE] SET
                                DocumentoDeclarante = @DocumentoDeclarante,
                                MoedaNegociacao = @MoedaNegociacao,
                                RUC = @RUC,
                                SituacaoEspecial = @SituacaoEspecial,
                                FormaExportacao = @FormaExportacao,
                                ViaEspecialTransporte = @ViaEspecialTransporte,
                                UnidadeDespachoId = @UnidadeDespachoId,
                                TipoRecintoAduaneiroDespacho = @TipoRecintoAduaneiroDespacho,
                                RecintoAduaneiroDespachoId = @RecintoAduaneiroDespachoId,
                                DocumentoResponsavelRecintoDespacho = @DocumentoResponsavelRecintoDespacho,
                                LatitudeRecintoDespacho = @LatitudeRecintoDespacho,
                                LongitudeRecintoDespacho = @LongitudeRecintoDespacho,
                                EnderecoRecintoDespacho = @EnderecoRecintoDespacho,
                                UnidadeEmbarqueId = @UnidadeEmbarqueId,
                                TipoRecintoAduaneiroEmbarque = @TipoRecintoAduaneiroEmbarque,
                                RecintoAduaneiroEmbarqueId = @RecintoAduaneiroEmbarqueId,
                                EnderecoReferenciaRecintoAduaneiroEmbarque = @EnderecoReferenciaRecintoAduaneiroEmbarque,
                                InformacoesComplementares = @InformacoesComplementares,
                                ValorUnitVMLE_Default = @ValorUnitVMLE_Default,
                                ValorUnitVMCV_Default = @ValorUnitVMCV_Default,
                                PaisDestino_Default = @PaisDestino_Default,
                                Enquadramento1_Default = @Enquadramento1_Default,
                                Enquadramento2_Default = @Enquadramento2_Default,
                                Enquadramento3_Default = @Enquadramento3_Default,
                                Enquadramento4_Default = @Enquadramento4_Default,
                                CondicaoVenda_Default = @CondicaoVenda_Default,
                                LPCO_Default = @LPCO_Default,
                                Prioridade_Default = @Prioridade_Default,
                                DescricaoComplementar_Default = @DescricaoComplementar_Default,
                                ComissaoAgente_Default = @ComissaoAgente_Default,
                                AC_Numero_Default = @AC_Numero_Default,
                                AC_CNPJ_Default = @AC_CNPJ_Default,
                                AC_Item_Default = @AC_Item_Default,
                                AC_NCM_Default = @AC_NCM_Default,
                                AC_Qtde_Default = @AC_Qtde_Default,
                                AC_VMLE_Sem_Cob_Default = @AC_VMLE_Sem_Cob_Default,
                                AC_VMLE_Com_Cob_Default = @AC_VMLE_Com_Cob_Default,
                                Attr_Padrao_Qualidade_Default = @Attr_Padrao_Qualidade_Default,
                                Attr_Tipo_Default = @Attr_Tipo_Default,
                                Attr_Metodo_Processamento_Default = @Attr_Metodo_Processamento_Default,
                                Attr_Outra_Caracteristica_Especial_Default = @Attr_Outra_Caracteristica_Especial_Default,
                                Attr_Caracteristica_Especial_Default = @Attr_Caracteristica_Especial_Default,
                                Attr_Embarque_Em_Default = @Attr_Embarque_Em_Default,
                                Attr_Embalagem_Final_Default = @Attr_Embalagem_Final_Default,
                                Ncm_Default = @Ncm_Default
                        WHERE Id = @Id", parametros);
            }
        }

        public void AtualizarInformacoesEnvio(int dueId, string due, string ruc, string chaveAcesso, string xml, string xmlRetorno, int enviado)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "DUE", value: due, direction: ParameterDirection.Input);
                parametros.Add(name: "RUC", value: ruc, direction: ParameterDirection.Input);
                parametros.Add(name: "EnviadoSiscomex", value: enviado, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveAcesso", value: chaveAcesso, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlEnviadoSiscomex", value: xml, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlRetorno", value: xmlRetorno, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET DUE = @DUE, RUC = @RUC, ChaveAcesso = @ChaveAcesso, EnviadoSiscomex = @EnviadoSiscomex, XmlEnviadoSiscomex = @XmlEnviadoSiscomex, XmlRetorno = @XmlRetorno, HorarioEnvioSiscomex = GetDate() WHERE Id = @Id", parametros);
            }
        }

        public void AtualizarInformacoesEnvioUltimoXMLGerado(int dueId, string xml)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlEnviadoSiscomex", value: xml, dbType: DbType.String, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET  UltimoXMLGerado = @XmlEnviadoSiscomex WHERE Id = @Id", parametros);
            }

        }

        public void AtualizarXMLRetorno(int dueId, string xmlRetorno)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlRetorno", value: xmlRetorno, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET  XmlRetorno = @XmlRetorno WHERE Id = @Id", parametros);
            }

        }

        public string ObterUltimoXMLGerado(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT UltimoXMLGerado FROM Tb_DUE WHERE ID= @Id", parametros).SingleOrDefault();

            }

        }

        public string ObterUltimoXMLRetorno(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT XmlRetorno FROM Tb_DUE WHERE ID= @Id", parametros).SingleOrDefault();

            }

        }

        public void AtualizarInformacoesErro(int dueId, string xml, string xmlRetorno)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlEnviadoSiscomex", value: xml, direction: ParameterDirection.Input);
                parametros.Add(name: "XmlRetorno", value: xmlRetorno, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET XmlRetorno = @XmlRetorno, UltimoXMLGerado = @XmlEnviadoSiscomex WHERE Id = @Id", parametros);
            }
        }

        public void ExcluirDUE(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE DueId = @Id", parametros);
                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DUEItemId IN (SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE DueId = @Id)", parametros);
                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item] WHERE DueId = @Id", parametros);
                con.Execute(@"DELETE FROM [dbo].[Tb_DUE] WHERE Id = @Id", parametros);
            }
        }

        public int RegistrarItemDUE(DUEItem dueItem)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "DueId", value: dueItem.DueId, direction: ParameterDirection.Input);
                parametros.Add(name: "Exportador", value: dueItem.Exportador?.Descricao, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorDocumento", value: dueItem.Exportador?.Documento, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorEndereco", value: dueItem.Exportador?.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorUF", value: dueItem.Exportador?.UF, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorPais", value: dueItem.Exportador?.Pais, direction: ParameterDirection.Input);
                parametros.Add(name: "Importador", value: dueItem.Importador?.Descricao, direction: ParameterDirection.Input);
                parametros.Add(name: "ImportadorEndereco", value: dueItem.Importador?.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "ImportadorPais", value: dueItem.Importador?.Pais, direction: ParameterDirection.Input);
                parametros.Add(name: "MotivoDispensaNF", value: dueItem.MotivoDispensaNF, direction: ParameterDirection.Input);
                parametros.Add(name: "CondicaoVenda", value: dueItem.CondicaoVenda, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMLE", value: dueItem.ValorUnitVMLE, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMCV", value: dueItem.ValorUnitVMCV, direction: ParameterDirection.Input);
                parametros.Add(name: "NF", value: dueItem.NF, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(@"
                        INSERT INTO 
                            [dbo].[Tb_DUE_Item] (
                                DueId,
                                Exportador,
                                ExportadorDocumento,
                                ExportadorEndereco,
                                ExportadorUF,
                                ExportadorPais,
                                Importador,
                                ImportadorEndereco,
                                ImportadorPais,
                                MotivoDispensaNF,
                                CondicaoVenda,
                                ValorUnitVMLE,
                                ValorUnitVMCV,
                                NF
                            ) VALUES (
                                @DueId,
                                @Exportador,
                                @ExportadorDocumento,
                                @ExportadorEndereco,
                                @ExportadorUF,
                                @ExportadorPais,
                                @Importador,
                                @ImportadorEndereco,
                                @ImportadorPais,
                                @MotivoDispensaNF,
                                @CondicaoVenda,
                                @ValorUnitVMLE,
                                @ValorUnitVMCV,
                                @NF); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
            }
        }

        public void ReplicarItemDUE(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                using (var transaction = con.BeginTransaction())
                {
                    var idItem = con.Query<int>(@"
                        INSERT INTO 
                            [dbo].[Tb_DUE_Item] 
                            (
                                DueId,
                                Exportador,
                                ExportadorDocumento,
                                ExportadorEndereco,
                                ExportadorUF,
                                ExportadorPais,
                                Importador,
                                ImportadorEndereco,
                                ImportadorPais,
                                MotivoDispensaNF,
                                CondicaoVenda,
                                ValorUnitVMLE,
                                ValorUnitVMCV
                            ) 
                            SELECT
                                DueId,
                                Exportador,
                                ExportadorDocumento,
                                ExportadorEndereco,
                                ExportadorUF,
                                ExportadorPais,
                                Importador,
                                ImportadorEndereco,
                                ImportadorPais,
                                MotivoDispensaNF,
                                CondicaoVenda,
                                ValorUnitVMLE,
                                ValorUnitVMCV
                            FROM
                                [dbo].[Tb_DUE_Item]
                            WHERE
                                Id = @Id; SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                    parametros.Add(name: "IdItem", value: idItem, direction: ParameterDirection.Input);

                    var idDetalheItem = con.Query<int?>(@"
                        INSERT INTO 
                            [dbo].[Tb_DUE_Item_Detalhes]
	                        (
		                        [DUEItemId],
		                        [Item],
		                        [ValorMercadoriaLocalEmbarque],
		                        [PaisDestino],
		                        [QuantidadeEstatistica],
		                        [PrioridadeCarga],
		                        [Limite],
		                        [DescricaoComplementar],
		                        [NCM],
		                        [ValorMercadoriaCondicaoVenda],
		                        [DescricaoMercadoria],
		                        [QuantidadeUnidades],
		                        [DescricaoUnidade],
		                        [CodigoProduto],
		                        [PesoLiquidoTotal],
		                        [Enquadramento1Id],
		                        [Enquadramento2Id],
                                [Enquadramento3Id],
		                        [Enquadramento4Id],
		                        [ComissaoAgente]
	                        )
                            (SELECT
                                @IdItem,
                                [Item],
                                [ValorMercadoriaLocalEmbarque],
                                [PaisDestino],
                                [QuantidadeEstatistica],
                                [PrioridadeCarga],
                                [Limite],
                                [DescricaoComplementar],
                                [NCM],
                                [ValorMercadoriaCondicaoVenda],
                                [DescricaoMercadoria],
                                [QuantidadeUnidades],
                                [DescricaoUnidade],
                                [CodigoProduto],
                                [PesoLiquidoTotal],
                                [Enquadramento1Id],
                                [Enquadramento2Id],
                                [Enquadramento3Id],
                                [Enquadramento4Id],
                                [ComissaoAgente]
                            FROM 
	                            [dbo].[Tb_DUE_Item_Detalhes]
                            WHERE
	                            DUEItemId = @Id); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).FirstOrDefault();

                    if (idDetalheItem != null)
                    {
                        parametros.Add(name: "IdDetalheItem", value: idDetalheItem, direction: ParameterDirection.Input);

                        con.Execute(@"
                            INSERT INTO
	                            [dbo].[Tb_DUE_Item_Detalhes_AC]
		                            (
			                            [IdDetalheItem],
			                            [Numero],
			                            [CNPJBeneficiario],
			                            [NumeroItem],
			                            [NCMItem],
			                            [QuantidadeUtilizada],
			                            [VMLEComCoberturaCambial],
			                            [VMLESemCoberturaCambial],
                                        [TipoAC],
                                        [ExportadorBeneficiario]
		                            )
                            SELECT 
                                @IdDetalheItem,
                                A.[Numero],
                                A.[CNPJBeneficiario],
                                A.[NumeroItem],
                                A.[NCMItem],
                                A.[QuantidadeUtilizada],
                                A.[VMLEComCoberturaCambial],
                                A.[VMLESemCoberturaCambial],
                                A.[TipoAC],
                                A.[ExportadorBeneficiario]
                            FROM 
	                            [dbo].[Tb_DUE_Item_Detalhes_AC] A
                            INNER JOIN
	                            [dbo].[Tb_DUE_Item_Detalhes] B ON A.IdDetalheItem = B.Id
                            WHERE
	                            B.DUEItemId = @Id", parametros, transaction);

                        con.Execute(@"
                            INSERT INTO
	                            [dbo].[Tb_DUE_Item_Detalhes_LPCO]
		                            (
                                        [IdDetalheItem],
			                            [Numero]
		                            )
                            SELECT 
                                @IdDetalheItem,
                                A.[Numero]
                            FROM 
	                            [dbo].[Tb_DUE_Item_Detalhes_LPCO] A
                            INNER JOIN
	                            [dbo].[Tb_DUE_Item_Detalhes] B ON A.IdDetalheItem = B.Id
                            WHERE
	                            B.DUEItemId = @Id", parametros, transaction);
                    }

                    transaction.Commit();
                }
            }
        }

        public void RegistrarAtoConcessorio(DUEItemDetalhesAC dueItemAC)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "IdDetalheItem", value: dueItemAC.IdDetalheItem, direction: ParameterDirection.Input);
                parametros.Add(name: "Numero", value: dueItemAC.Numero, direction: ParameterDirection.Input);
                parametros.Add(name: "CNPJBeneficiario", value: dueItemAC.CNPJBeneficiario, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroItem", value: dueItemAC.NumeroItem, direction: ParameterDirection.Input);
                parametros.Add(name: "NCMItem", value: dueItemAC.NCMItem, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeUtilizada", value: dueItemAC.QuantidadeUtilizada, direction: ParameterDirection.Input);
                parametros.Add(name: "VMLEComCoberturaCambial", value: dueItemAC.VMLEComCoberturaCambial, direction: ParameterDirection.Input);
                parametros.Add(name: "VMLESemCoberturaCambial", value: dueItemAC.VMLESemCoberturaCambial, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoAC", value: dueItemAC.TipoAC, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorBeneficiario", value: dueItemAC.ExportadorBeneficiario, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute(@"
                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_AC]
                           (
                               [IdDetalheItem],
                               [Numero],
                               [CNPJBeneficiario],
                               [NumeroItem],
                               [NCMItem],
                               [QuantidadeUtilizada],
                               [VMLEComCoberturaCambial],
                               [VMLESemCoberturaCambial],
                               [TipoAC],
                               [ExportadorBeneficiario]
                            ) VALUES (
                               @IdDetalheItem,
                               @Numero,
                               @CNPJBeneficiario,
                               @NumeroItem,
                               @NCMItem,
                               @QuantidadeUtilizada,
                               @VMLEComCoberturaCambial,
                               @VMLESemCoberturaCambial,
                               @TipoAC,
                               @ExportadorBeneficiario
                            )", parametros);
            }
        }

        public void RegistrarAtoConcessorioNF(DUEItemDetalhesACNF dueItemACNF)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "IdAcd", value: dueItemACNF.IdAcd, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNumNFAC", value: dueItemACNF.ChaveNota, direction: ParameterDirection.Input);
                parametros.Add(name: "DataEmissaoNFAC", value: dueItemACNF.DataEmissao, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeNFAC", value: dueItemACNF.Quantidade, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorNFAC", value: dueItemACNF.Valor, direction: ParameterDirection.Input);

                con.Execute(@"
                        INSERT INTO [dbo].[TB_DUE_Item_Detalhes_AC_NF]
                           (
                               [ACId],
                               [ChaveNota],
                               [DataEmissao],
                               [Quantidade],                               
                               [Valor]                               
                            ) VALUES (
                               @IdAcd,
                               @ChaveNumNFAC,
                               @DataEmissaoNFAC,
                               @QuantidadeNFAC,
                               @ValorNFAC
                            )", parametros);
            }
        }

        public void AtualizarAtoConcessorio(DUEItemDetalhesAC dueItemAC)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "IdDetalheItem", value: dueItemAC.IdDetalheItem, direction: ParameterDirection.Input);
                parametros.Add(name: "Numero", value: dueItemAC.Numero, direction: ParameterDirection.Input);
                parametros.Add(name: "CNPJBeneficiario", value: dueItemAC.CNPJBeneficiario, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroItem", value: dueItemAC.NumeroItem, direction: ParameterDirection.Input);
                parametros.Add(name: "NCMItem", value: dueItemAC.NCMItem, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeUtilizada", value: dueItemAC.QuantidadeUtilizada, direction: ParameterDirection.Input);
                parametros.Add(name: "VMLEComCoberturaCambial", value: dueItemAC.VMLEComCoberturaCambial, direction: ParameterDirection.Input);
                parametros.Add(name: "VMLESemCoberturaCambial", value: dueItemAC.VMLESemCoberturaCambial, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoAC", value: dueItemAC.TipoAC, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorBeneficiario", value: dueItemAC.ExportadorBeneficiario, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: dueItemAC.Id, direction: ParameterDirection.Input);

                con.Execute(@"
                        UPDATE [dbo].[Tb_DUE_Item_Detalhes_AC]
                           SET
                               [IdDetalheItem] = @IdDetalheItem,
                               [Numero] = @Numero,
                               [CNPJBeneficiario] = @CNPJBeneficiario,
                               [NumeroItem] = @NumeroItem,
                               [NCMItem] = @NCMItem,
                               [QuantidadeUtilizada] = @QuantidadeUtilizada,
                               [VMLEComCoberturaCambial] = @VMLEComCoberturaCambial,
                               [VMLESemCoberturaCambial] = @VMLESemCoberturaCambial,
                               [TipoAC] = @TipoAC,
                               [ExportadorBeneficiario] = @ExportadorBeneficiario
                            WHERE [Id] = @Id", parametros);
            }
        }

        public void ExcluirAtoConcessorio(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_Item_Detalhes_AC_NF] WHERE ACId = @Id", parametros);
                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE Id = @Id", parametros);
            }
        }

        public void ExcluirAtoConcessorioNF(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_Item_Detalhes_AC_NF] WHERE Id = @Id", parametros);
            }
        }

        public void ExcluirAtoConcessorioDoItem(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DetalheItemId", value: detalheItemId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE IdDetalheItem = @DetalheItemId", parametros);
            }
        }

        public IEnumerable<DUEItemDetalhesAC> ObterAtosConcessorios(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);

                return con.Query<DUEItemDetalhesAC>(@"SELECT Id,IdDetalheItem,Numero,CNPJBeneficiario,NumeroItem,NCMItem,QuantidadeUtilizada,VMLEComCoberturaCambial,VMLESemCoberturaCambial,ISNULL(TipoAC, 'AC') TipoAC, ISNULL(ExportadorBeneficiario, 0) ExportadorBeneficiario FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE IdDetalheItem = @IdDetalheItem", parametros);
            }
        }

        public IEnumerable<DUEItemDetalhesACNF> ObterAtosConcessoriosNF(int atoConcessorioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "AtoConcessorioId", value: atoConcessorioId, direction: ParameterDirection.Input);

                return con.Query<DUEItemDetalhesACNF>(@"SELECT Id, ACId, ChaveNota, DataEmissao, Valor, Quantidade FROM TB_DUE_Item_Detalhes_AC_NF WHERE AcId = @AtoConcessorioId", parametros);
            }
        }

        public DUEItemDetalhesAC ObterAtosConcessorioPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<DUEItemDetalhesAC>(@"SELECT Id,IdDetalheItem,Numero,CNPJBeneficiario,NumeroItem,NCMItem,QuantidadeUtilizada,VMLEComCoberturaCambial,VMLESemCoberturaCambial,ISNULL(TipoAC, 'AC') TipoAC, ISNULL(ExportadorBeneficiario, 0) ExportadorBeneficiario FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE Id = @Id", parametros).FirstOrDefault();
            }
        }

        public int InserirLogSiscomex(string xml, int due)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Xml", value: xml, direction: ParameterDirection.Input);
                parametros.Add(name: "Due", value: due, direction: ParameterDirection.Input);

                return con.Execute(@"INSERT INTO [dbo].[TB_LOG_SISCOMEX] (XML, FUNCAO, AUTONUM_DUE) VALUES (@Xml, 10, @Due)", parametros);
            }
        }

        public int AtualizarItemDUE(DUEItem dueItem)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: dueItem.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Exportador", value: dueItem.Exportador?.Descricao, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorDocumento", value: dueItem.Exportador?.Documento, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorEndereco", value: dueItem.Exportador?.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorUF", value: dueItem.Exportador?.UF, direction: ParameterDirection.Input);
                parametros.Add(name: "ExportadorPais", value: dueItem.Exportador?.Pais, direction: ParameterDirection.Input);
                parametros.Add(name: "Importador", value: dueItem.Importador?.Descricao, direction: ParameterDirection.Input);
                parametros.Add(name: "ImportadorEndereco", value: dueItem.Importador?.Endereco, direction: ParameterDirection.Input);
                parametros.Add(name: "ImportadorPais", value: dueItem.Importador?.Pais, direction: ParameterDirection.Input);
                parametros.Add(name: "MotivoDispensaNF", value: dueItem.MotivoDispensaNF, direction: ParameterDirection.Input);
                parametros.Add(name: "CondicaoVenda", value: dueItem.CondicaoVenda, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMLE", value: dueItem.ValorUnitVMLE, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorUnitVMCV", value: dueItem.ValorUnitVMCV, direction: ParameterDirection.Input);

                return con.Execute(@"
                        UPDATE
                            [dbo].[Tb_DUE_Item] SET
                                Exportador = @Exportador,
                                ExportadorDocumento = @ExportadorDocumento,
                                ExportadorEndereco = @ExportadorEndereco,
                                ExportadorUF = @ExportadorUF,
                                ExportadorPais = @ExportadorPais,
                                Importador = @Importador,
                                ImportadorEndereco = @ImportadorEndereco,
                                ImportadorPais = @ImportadorPais,
                                MotivoDispensaNF = @MotivoDispensaNF,
                                CondicaoVenda = @CondicaoVenda,
                                ValorUnitVMLE = @ValorUnitVMLE,
                                ValorUnitVMCV = @ValorUnitVMCV
                            WHERE
                                Id = @Id", parametros);
            }
        }

        public DUEMaster ObterDUEPorId(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                return con.Query<DUEMaster, UnidadeDespacho, UnidadeEmbarque, DUEMaster>(@"
                    SELECT
                        Id,
                        DUE,
                        DocumentoDeclarante,
                        MoedaNegociacao,
                        RUC,
                        SituacaoDUE,
                        SituacaoEspecial,
                        FormaExportacao,
                        ViaEspecialTransporte,
                        InformacoesComplementares,
                        DataCadastro,
                        UsuarioCadastro,
                        EnviadoSiscomex,
                        CriadoPorNF,
                        ChaveAcesso,
                        Completa,
                        ISNULL(ImportadoSiscomex, 0) ImportadoSiscomex,
                        ValorUnitVMLE_Default,
                        ValorUnitVMCV_Default,
                        PaisDestino_Default,
                        Enquadramento1_Default,
                        Enquadramento2_Default,
                        Enquadramento3_Default,
                        Enquadramento4_Default,
                        CondicaoVenda_Default,
                        LPCO_Default,
                        Prioridade_Default,
                        DescricaoComplementar_Default,
                        ComissaoAgente_Default,
                        ISNULL(AC_Tipo_Default, 'AC') AC_Tipo_Default, 
                        ISNULL(AC_Exp_Benefic_Default, 0) AC_Exp_Benefic_Default,
                        AC_Numero_Default,
                        AC_CNPJ_Default,
                        AC_Item_Default,
                        AC_NCM_Default,
                        AC_Qtde_Default,
                        AC_VMLE_Sem_Cob_Default,
                        AC_VMLE_Com_Cob_Default,
                        Attr_Padrao_Qualidade_Default,
                        Attr_Tipo_Default,
                        Attr_Metodo_Processamento_Default,
                        Attr_Outra_Caracteristica_Especial_Default,
                        Attr_Caracteristica_Especial_Default,
                        Attr_Embarque_Em_Default,
                        Attr_Embalagem_Final_Default,
                        ISNULL(NCM_Default,'') NCM_Default,
                        UnidadeDespachoId As Id,
                        TipoRecintoAduaneiroDespacho As TipoRecinto,
                        RecintoAduaneiroDespachoId As RecintoAduaneiroId,
                        DocumentoResponsavelRecintoDespacho As DocumentoResponsavel,
                        LatitudeRecintoDespacho As Latitude,
                        LongitudeRecintoDespacho As Longitude,
                        EnderecoRecintoDespacho As Endereco,
                        UnidadeEmbarqueId As Id,
                        TipoRecintoAduaneiroEmbarque As TipoRecinto,
                        RecintoAduaneiroEmbarqueId As RecintoAduaneiroId,
                        EnderecoReferenciaRecintoAduaneiroEmbarque As EnderecoReferencia

                    FROM
                        [dbo].[Tb_DUE]
                    WHERE
                        Id = @Id", (d, ud, ue) =>
                {
                    if (ud != null)
                        d.UnidadeDespacho = ud;

                    if (ue != null)
                        d.UnidadeEmbarque = ue;

                    return d;
                }, parametros, splitOn: "Id").FirstOrDefault();
            }
        }

        public DUEMaster ObterDUEPorNumero(string numeroDUE)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DUE", value: numeroDUE, direction: ParameterDirection.Input);

                return con.Query<DUEMaster, UnidadeDespacho, UnidadeEmbarque, DUEMaster>(@"
                    SELECT
                        Id,
                        DUE,
                        DocumentoDeclarante,
                        MoedaNegociacao,
                        RUC,
                        SituacaoEspecial,
                        FormaExportacao,
                        ViaEspecialTransporte,
                        InformacoesComplementares,
                        DataCadastro,
                        UsuarioCadastro,
                        UnidadeDespachoId As Id,
                        TipoRecintoAduaneiroDespacho As TipoRecinto,
                        RecintoAduaneiroDespachoId As RecintoAduaneiroId,
                        DocumentoResponsavelRecintoDespacho As DocumentoResponsavel,
                        LatitudeRecintoDespacho As Latitude,
                        LongitudeRecintoDespacho As Longitude,
                        EnderecoRecintoDespacho As Endereco,
                        UnidadeEmbarqueId As Id,
                        TipoRecintoAduaneiroEmbarque As TipoRecinto,
                        RecintoAduaneiroEmbarqueId As RecintoAduaneiroId,
                        EnderecoReferenciaRecintoAduaneiroEmbarque As EnderecoReferencia                        
                    FROM
                        [dbo].[Tb_DUE]
                    WHERE
                        DUE = @DUE", (d, ud, ue) =>
                {
                    if (ud != null)
                        d.UnidadeDespacho = ud;

                    if (ue != null)
                        d.UnidadeEmbarque = ue;

                    return d;
                }, parametros, splitOn: "Id").FirstOrDefault();
            }
        }

        public IEnumerable<DUEMasterDTO> ObterDUEs(string filtro, int usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Filtro", value: filtro, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);

                var sql = $@"
                   SELECT 
	                    A.[Id],
                        STUFF(A.[DUE], 14, 0, '-') As DUE,
                        A.[DocumentoDeclarante],
                        A.[MoedaNegociacao],
                        REPLACE(A.[RUC], '0000000000000000000', '00...00') RUC,
	                    Case 
		                    WHEN A.[SituacaoEspecial] = 0 THEN 'Nenhuma' 
		                    WHEN A.[SituacaoEspecial] = 2001 THEN 'DU-E a posteriori'
		                    WHEN A.[SituacaoEspecial] = 2002 THEN 'Embarque Antecipado'
		                    WHEN A.[SituacaoEspecial] = 2003 THEN 'Exportação sem Saída da Mercadoria do País'
	                    END As SituacaoEspecial,
	                    Case	
		                    WHEN A.[FormaExportacao] = 1001 THEN 'Por conta própria'
		                    WHEN A.[FormaExportacao] = 1002 THEN 'Por conta e ordem de terceiros'
		                    WHEN A.[FormaExportacao] = 1003 THEN 'Por operador de remessa postal ou expressa'
	                    END As FormaExportacao,
                        Case
		                    WHEN A.[ViaEspecialTransporte] = 4001 THEN 'Por meios próprios / por reboque'
		                    WHEN A.[ViaEspecialTransporte] = 4002 THEN 'Por Dutos'
		                    WHEN A.[ViaEspecialTransporte] = 4003 THEN 'Linhas de Transmissão'
		                    WHEN A.[ViaEspecialTransporte] = 4004 THEN 'Em mãos'
	                    END as ViaEspecialTransporte,
	                    A.[UnidadeDespachoId],
	                    B.[Descricao] As UnidadeDespacho,
                        Case
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 281 THEN 'Recinto Alfandegado'
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 19 THEN 'Fora do Recinto Alfandegado (Domiciliar)'
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 22 THEN 'Fora do Recinto Alfandegado (Não domiciliar)'
	                    END As TipoRecintoAduaneiroDespacho,
	                    A.[RecintoAduaneiroDespachoId],
	                    D.[Descricao] as RecintoAduaneiroDespacho,
	                    A.[DocumentoResponsavelRecintoDespacho], 
	                    A.[UnidadeEmbarqueId],
	                    C.[Descricao] As UnidadeEmbarque,
	                    Case
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 281 THEN 'Recinto Alfandegado'
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 19 THEN 'Fora do Recinto Alfandegado (Domiciliar)'
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 22 THEN 'Fora do Recinto Alfandegado (Não domiciliar)'
	                    END As [TipoRecintoAduaneiroEmbarque],
	                    A.[RecintoAduaneiroEmbarqueId],
	                    E.[Descricao] As RecintoAduaneiroEmbarque,
	                    CONVERT(VARCHAR,A.[DataCadastro],103) + ' ' + CONVERT(VARCHAR(5),A.[DataCadastro],108) DataCadastro,
                        F.Login,
                        ISNULL(A.[EnviadoSiscomex],0) EnviadoSiscomex,
                        ISNULL(A.[RetificadoSiscomex],0) RetificadoSiscomex,
                        ISNULL(A.[ImportadoSiscomex],0) ImportadoSiscomex,
                        A.ChaveAcesso,
                        ISNULL(A.Completa, 0) Completa,
                        ISNULL(A.CriadoPorNF, 0) CriadoPorNF,
                        ISNULL(dbo.CorrigeSituacaoDUE(dbo.CapitalizeText(REPLACE(SituacaoDUE,'_',' '))),'N/D') As SituacaoDUE
                    FROM 
	                    [dbo].[Tb_DUE] A
                    LEFT JOIN
	                    [dbo].[Tb_Unidades_RFB] B ON A.UnidadeDespachoId = B.Codigo
                    LEFT JOIN
	                    [dbo].[Tb_Unidades_RFB] C ON A.UnidadeEmbarqueId = C.Codigo
                    LEFT JOIN
	                    [dbo].[Tb_Recintos] D ON A.RecintoAduaneiroDespachoId = D.Id
                    LEFT JOIN
	                    [dbo].[Tb_Recintos] E ON A.[RecintoAduaneiroEmbarqueId] = E.Id
                    LEFT JOIN
	                    [dbo].[Tb_DUE_Usuarios] F ON A.[UsuarioCadastro] = F.Id
                    WHERE 
                        A.Id > 0 {filtro}";

                sql = sql + @" ORDER BY 
	                    A.[Id] DESC";

                return con.Query<DUEMasterDTO>(sql, parametros);
            }
        }

        public IEnumerable<DUEMasterDTO> ObterDUESSemVinculoNotaFiscal(int usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);

                return con.Query<DUEMasterDTO>(@"

                  SELECT 
	                    A.[Id],
                        A.[DUE],
                        A.[DocumentoDeclarante],
                        A.[MoedaNegociacao],
                        A.[RUC],
	                    Case 
		                    WHEN A.[SituacaoEspecial] = 0 THEN 'Nenhuma' 
		                    WHEN A.[SituacaoEspecial] = 2001 THEN 'DU-E a posteriori'
		                    WHEN A.[SituacaoEspecial] = 2002 THEN 'Embarque Antecipado'
		                    WHEN A.[SituacaoEspecial] = 2003 THEN 'Exportação sem Saída da Mercadoria do País'
	                    END As SituacaoEspecial,
	                    Case	
		                    WHEN A.[FormaExportacao] = 1001 THEN 'Por conta própria'
		                    WHEN A.[FormaExportacao] = 1002 THEN 'Por conta e ordem de terceiros'
		                    WHEN A.[FormaExportacao] = 1003 THEN 'Por operador de remessa postal ou expressa'
	                    END As FormaExportacao,
                        Case
		                    WHEN A.[ViaEspecialTransporte] = 4001 THEN 'Por meios próprios / por reboque'
		                    WHEN A.[ViaEspecialTransporte] = 4002 THEN 'Por Dutos'
		                    WHEN A.[ViaEspecialTransporte] = 4003 THEN 'Linhas de Transmissão'
		                    WHEN A.[ViaEspecialTransporte] = 4004 THEN 'Em mãos'
	                    END as ViaEspecialTransporte,
	                    A.[UnidadeDespachoId],
	                    B.[Descricao] As UnidadeDespacho,
                        Case
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 281 THEN 'Recinto Alfandegado'
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 19 THEN 'Fora do Recinto Alfandegado (Domiciliar)'
		                    WHEN A.[TipoRecintoAduaneiroDespacho] = 22 THEN 'Fora do Recinto Alfandegado (Não domiciliar)'
	                    END As TipoRecintoAduaneiroDespacho,
	                    A.[RecintoAduaneiroDespachoId],
	                    D.[Descricao] as RecintoAduaneiroDespacho,
	                    A.[DocumentoResponsavelRecintoDespacho], 
	                    A.[UnidadeEmbarqueId],
	                    C.[Descricao] As UnidadeEmbarque,
	                    Case
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 281 THEN 'Recinto Alfandegado'
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 19 THEN 'Fora do Recinto Alfandegado (Domiciliar)'
		                    WHEN A.[TipoRecintoAduaneiroEmbarque] = 22 THEN 'Fora do Recinto Alfandegado (Não domiciliar)'
	                    END As [TipoRecintoAduaneiroEmbarque],
	                    A.[RecintoAduaneiroEmbarqueId],
	                    E.[Descricao] As RecintoAduaneiroEmbarque,
	                    A.[DataCadastro],
                        A.[EnviadoSiscomex]
                    FROM 
	                    [dbo].[Tb_DUE] A
                    LEFT JOIN
	                    [dbo].[Tb_Unidades_RFB] B ON A.UnidadeDespachoId = B.Codigo
                    LEFT JOIN
	                    [dbo].[Tb_Unidades_RFB] C ON A.UnidadeEmbarqueId = C.Codigo
                    LEFT JOIN
	                    [dbo].[Tb_Recintos] D ON A.RecintoAduaneiroDespachoId = D.Id
                    LEFT JOIN
	                    [dbo].[Tb_Recintos] E ON A.[RecintoAduaneiroEmbarqueId] = E.Id
                    LEFT JOIN
	                    [dbo].[Tb_DUE_Item] F ON F.[DueId] = A.[Id]
                    LEFT JOIN
	                    [dbo].[Tb_DUE_Item_Detalhes] G ON F.Id = G.DUEItemId
                    WHERE
	                    F.NF IS NULL AND A.DUE IS NOT NULL 
                    AND
                        (A.UsuarioCadastro = @Usuario OR UsuarioCadastro in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario)) 
                    ORDER BY 
	                    A.[Id] DESC", parametros);
            }
        }

        public IEnumerable<DUEItemDTO> ObterResumoItensDUE(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItemDTO>(@"
                    SELECT
                        Id,
                        DueId,
                        ROW_NUMBER() OVER (order by Id) as NroItem,
                        MotivoDispensaNF,
                        CASE 
		                    WHEN MotivoDispensaNF = 3001 THEN 'BAGAGEM DESACOMPANHADA' 
		                    WHEN MotivoDispensaNF = 3002 THEN 'BENS DE VIAJANTE NÃO INCLUÍDOS NO CONCEITO DE BAGAGEM' 
		                    WHEN MotivoDispensaNF = 3003 THEN 'RETORNO DE MERCADORIA AO EXTERIOR ANTES DO REGISTRO DA DI' 
		                    WHEN MotivoDispensaNF = 3004 THEN 'EMBARQUE ENTECIPADO' 
	                    END MotivoDispensaNFDescricao,
                        CondicaoVenda,                        
                        Exportador,
                        ExportadorDocumento,
                        ExportadorEndereco,
                        ExportadorUF,
                        ExportadorPais,
                        Importador,
                        ImportadorEndereco,
                        ImportadorPais 
                    FROM
                        [dbo].[Tb_DUE_Item]
                    WHERE
                        DueId = @dueId
                    ORDER BY Id", new { dueId });
            }
        }

        public IEnumerable<DUEItem> ObterItensDUE(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItem, Exportador, Importador, DUEItem>(@"
                    SELECT
                        Id,
                        DueId,
                        MotivoDispensaNF,         
                        CondicaoVenda,
                        NF,
                        Exportador As Descricao,
                        ExportadorDocumento As Documento,
                        ExportadorEndereco As Endereco,
                        ExportadorUF As UF,
                        ExportadorPais As Pais,
                        Importador As Descricao,
                        ImportadorEndereco As Endereco,
                        ImportadorPais As Pais 
                    FROM
                        [dbo].[Tb_DUE_Item]
                    WHERE
                        DueId = @dueId", (item, exportador, importador) =>
                {
                    if (exportador != null)
                        item.Exportador = exportador;

                    if (importador != null)
                        item.Importador = importador;

                    return item;
                }, new { dueId }, splitOn: "Descricao");
            }
        }

        public DUEItemDTO ObterItemDUEPorID(int dueItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItemDTO>(@"
                    SELECT
                        Id,
                        DueId,
                        MotivoDispensaNF,
                        CondicaoVenda,                        
                        Exportador,
                        ExportadorDocumento,
                        ExportadorEndereco,
                        ExportadorUF,
                        ExportadorPais,
                        Importador,
                        ImportadorEndereco,
                        ImportadorPais,
                        NF,
                        ValorUnitVMLE,
                        ValorUnitVMCV
                    FROM
                        [dbo].[Tb_DUE_Item]
                    WHERE
                        Id = @dueItemId", new { dueItemId }).FirstOrDefault();
            }
        }

        public void ExcluirItemDUE(int itemDueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: itemDueId, direction: ParameterDirection.Input);

                using (var transaction = con.BeginTransaction())
                {
                    con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_LPCO] WHERE IdDetalheItem IN (SELECT Id FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DueItemId IN (SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE Id = @Id))", parametros, transaction);
                    con.Execute(@"DELETE FROM [dbo].[TB_DUE_Item_Detalhes_AC_NF] WHERE ACId IN (SELECT Id FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE IdDetalheItem IN (SELECT Id FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DueItemId IN (SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE Id = @Id)))", parametros, transaction);
                    con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_AC] WHERE IdDetalheItem IN (SELECT Id FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DueItemId IN (SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE Id = @Id))", parametros, transaction);
                    con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DUEItemId IN (SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE Id = @Id)", parametros, transaction);
                    con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item] WHERE Id = @Id", parametros, transaction);

                    transaction.Commit();
                }
            }
        }

        public int RegistrarDUEItemDetalhe(DUEItemDetalhes detalhe)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "DUEItemId", value: detalhe.DUEItemId, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: detalhe.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorMercadoriaLocalEmbarque", value: detalhe.ValorMercadoriaLocalEmbarque, direction: ParameterDirection.Input);
                parametros.Add(name: "PaisDestino", value: detalhe.PaisDestino, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeEstatistica", value: detalhe.QuantidadeEstatistica, direction: ParameterDirection.Input);
                parametros.Add(name: "PrioridadeCarga", value: detalhe.PrioridadeCarga, direction: ParameterDirection.Input);
                parametros.Add(name: "Limite", value: detalhe.Limite, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoComplementar", value: detalhe.DescricaoComplementar, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: detalhe.NCM, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorMercadoriaCondicaoVenda", value: detalhe.ValorMercadoriaCondicaoVenda, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoMercadoria", value: detalhe.DescricaoMercadoria, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeUnidades", value: detalhe.QuantidadeUnidades, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoUnidade", value: detalhe.DescricaoUnidade, direction: ParameterDirection.Input);
                parametros.Add(name: "CodigoProduto", value: detalhe.CodigoProduto, direction: ParameterDirection.Input);
                parametros.Add(name: "PesoLiquidoTotal", value: detalhe.PesoLiquidoTotal, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento1Id", value: detalhe.Enquadramento1Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento2Id", value: detalhe.Enquadramento2Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento3Id", value: detalhe.Enquadramento3Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento4Id", value: detalhe.Enquadramento4Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Padrao_Qualidade", value: detalhe.Attr_Padrao_Qualidade, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embarque_Em", value: detalhe.Attr_Embarque_Em, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Tipo", value: detalhe.Attr_Tipo, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Metodo_Processamento", value: detalhe.Attr_Metodo_Processamento, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Caracteristica_Especial", value: detalhe.Attr_Caracteristica_Especial, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Outra_Caracteristica_Especial", value: detalhe.Attr_Outra_Caracteristica_Especial, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embalagem_Final", value: detalhe.Attr_Embalagem_Final, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(@"
                        INSERT INTO	
	                        [dbo].[Tb_DUE_Item_Detalhes] (
		                        [DUEItemId],
	                            [Item],
		                        [ValorMercadoriaLocalEmbarque],
		                        [PaisDestino],
		                        [QuantidadeEstatistica],
		                        [PrioridadeCarga],
		                        [Limite],
		                        [DescricaoComplementar],
		                        [NCM],
		                        [ValorMercadoriaCondicaoVenda],
		                        [DescricaoMercadoria],
		                        [QuantidadeUnidades],
		                        [DescricaoUnidade],
		                        [CodigoProduto],
		                        [PesoLiquidoTotal],
		                        [Enquadramento1Id],
                                [Enquadramento2Id],
                                [Enquadramento3Id],
                                [Enquadramento4Id],
                                [Attr_Padrao_Qualidade],
                                [Attr_Embarque_Em],
                                [Attr_Tipo],
                                [Attr_Metodo_Processamento],
                                [Attr_Caracteristica_Especial],
                                [Attr_Outra_Caracteristica_Especial],
                                [Attr_Embalagem_Final]
	                        ) VALUES (
		                        @DUEItemId,
	                            @Item,
		                        @ValorMercadoriaLocalEmbarque,
		                        @PaisDestino,
		                        @QuantidadeEstatistica,
		                        @PrioridadeCarga,
		                        @Limite,
		                        @DescricaoComplementar,
		                        @NCM,
		                        @ValorMercadoriaCondicaoVenda,
		                        @DescricaoMercadoria,
		                        @QuantidadeUnidades,
		                        @DescricaoUnidade,
		                        @CodigoProduto,
		                        @PesoLiquidoTotal,
		                        @Enquadramento1Id,
                                @Enquadramento2Id,
                                @Enquadramento3Id,
                                @Enquadramento4Id,
                                @Attr_Padrao_Qualidade,
                                @Attr_Embarque_Em,
                                @Attr_Tipo,
                                @Attr_Metodo_Processamento,
                                @Attr_Caracteristica_Especial,
                                @Attr_Outra_Caracteristica_Especial,
                                @Attr_Embalagem_Final); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
            }
        }

        public string ObterSequenciaItem(int dueItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<string>(@"SELECT ISNULL(MAX(Item),0) + 1 As Item FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE DUEItemId = @dueItemId", new { dueItemId }).Single();
            }
        }

        public void AtualizarDUEItemDetalhe(DUEItemDetalhes detalhe)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: detalhe.Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: detalhe.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorMercadoriaLocalEmbarque", value: detalhe.ValorMercadoriaLocalEmbarque, direction: ParameterDirection.Input);
                parametros.Add(name: "PaisDestino", value: detalhe.PaisDestino, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeEstatistica", value: detalhe.QuantidadeEstatistica, direction: ParameterDirection.Input);
                parametros.Add(name: "PrioridadeCarga", value: detalhe.PrioridadeCarga, direction: ParameterDirection.Input);
                parametros.Add(name: "Limite", value: detalhe.Limite, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoComplementar", value: detalhe.DescricaoComplementar, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: detalhe.NCM, direction: ParameterDirection.Input);
                parametros.Add(name: "ValorMercadoriaCondicaoVenda", value: detalhe.ValorMercadoriaCondicaoVenda, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoMercadoria", value: detalhe.DescricaoMercadoria, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeUnidades", value: detalhe.QuantidadeUnidades, direction: ParameterDirection.Input);
                parametros.Add(name: "DescricaoUnidade", value: detalhe.DescricaoUnidade, direction: ParameterDirection.Input);
                parametros.Add(name: "CodigoProduto", value: detalhe.CodigoProduto, direction: ParameterDirection.Input);
                parametros.Add(name: "PesoLiquidoTotal", value: detalhe.PesoLiquidoTotal, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento1Id", value: detalhe.Enquadramento1Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento2Id", value: detalhe.Enquadramento2Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento3Id", value: detalhe.Enquadramento3Id, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento4Id", value: detalhe.Enquadramento4Id, direction: ParameterDirection.Input);
                parametros.Add(name: "ComissaoAgente", value: detalhe.ComissaoAgente, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Padrao_Qualidade", value: detalhe.Attr_Padrao_Qualidade, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embarque_Em", value: detalhe.Attr_Embarque_Em, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Tipo", value: detalhe.Attr_Tipo, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Metodo_Processamento", value: detalhe.Attr_Metodo_Processamento, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Caracteristica_Especial", value: detalhe.Attr_Caracteristica_Especial, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Outra_Caracteristica_Especial", value: detalhe.Attr_Outra_Caracteristica_Especial, direction: ParameterDirection.Input);
                parametros.Add(name: "Attr_Embalagem_Final", value: detalhe.Attr_Embalagem_Final, direction: ParameterDirection.Input);

                con.Execute(@"
                        UPDATE
	                        [dbo].[Tb_DUE_Item_Detalhes] SET
	                            [Item] = @Item,
		                        [ValorMercadoriaLocalEmbarque] = @ValorMercadoriaLocalEmbarque,
		                        [PaisDestino] = @PaisDestino,
		                        [QuantidadeEstatistica] = @QuantidadeEstatistica,
		                        [PrioridadeCarga] = @PrioridadeCarga,
		                        [Limite] = @Limite,
		                        [DescricaoComplementar] = @DescricaoComplementar,
		                        [NCM] = @NCM,
		                        [ValorMercadoriaCondicaoVenda] = @ValorMercadoriaCondicaoVenda,
		                        [DescricaoMercadoria] = @DescricaoMercadoria,
		                        [QuantidadeUnidades] = @QuantidadeUnidades,
		                        [DescricaoUnidade] = @DescricaoUnidade,
		                        [CodigoProduto] = @CodigoProduto,
		                        [PesoLiquidoTotal] = @PesoLiquidoTotal,
		                        [Enquadramento1Id] = @Enquadramento1Id,
                                [Enquadramento2Id] = @Enquadramento2Id,
                                [Enquadramento3Id] = @Enquadramento3Id,
                                [Enquadramento4Id] = @Enquadramento4Id,
                                [ComissaoAgente] = @ComissaoAgente,
                                [Attr_Padrao_Qualidade] = @Attr_Padrao_Qualidade,
                                [Attr_Embarque_Em] = @Attr_Embarque_Em,
                                [Attr_Tipo] = @Attr_Tipo,
                                [Attr_Metodo_Processamento] = @Attr_Metodo_Processamento,
                                [Attr_Caracteristica_Especial] = @Attr_Caracteristica_Especial,
                                [Attr_Outra_Caracteristica_Especial] = @Attr_Outra_Caracteristica_Especial,
                                [Attr_Embalagem_Final] = @Attr_Embalagem_Final
	                        WHERE
                                [Id] = @Id", parametros);
            }
        }

        public IEnumerable<DUEItemDetalhes> ObterDetalhesItem(int dueItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItemDetalhes>(@"
                    SELECT
	                    Id,
                        Item,
                        ValorMercadoriaLocalEmbarque,
                        PaisDestino,
                        QuantidadeEstatistica,
                        PrioridadeCarga,
                        Limite,
                        DescricaoComplementar,
                        NCM,
                        ValorMercadoriaCondicaoVenda,
                        DescricaoMercadoria,
                        QuantidadeUnidades,
                        DescricaoUnidade,
                        CodigoProduto,
                        PesoLiquidoTotal,
                        Enquadramento1Id,
                        Enquadramento2Id,
                        ComissaoAgente,
                        Attr_Padrao_Qualidade,
                        Attr_Embarque_Em,
                        Attr_Tipo,
                        Attr_Metodo_Processamento,
                        Attr_Caracteristica_Especial,
                        Attr_Outra_Caracteristica_Especial,
                        Attr_Embalagem_Final
                    FROM
	                    Tb_DUE_Item_Detalhes
                    WHERE
	                    DUEItemId = @dueItemId
                    ORDER BY
	                    Item", new { dueItemId });
            }
        }

        public IEnumerable<DUEItemDetalhes> ObterItensNotaPorDUE(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItemDetalhes>(@"
                    SELECT
	                    B.Id,
	                    A.DUEItemId,
                        A.Item,
                        A.ValorMercadoriaLocalEmbarque,
                        A.PaisDestino,
                        A.QuantidadeEstatistica,
                        A.PrioridadeCarga,
                        A.Limite,
                        A.DescricaoComplementar,
                        A.NCM,
                        A.ValorMercadoriaCondicaoVenda,
                        A.DescricaoMercadoria,
                        A.QuantidadeUnidades,
                        A.DescricaoUnidade,
                        A.CodigoProduto,
                        A.PesoLiquidoTotal,
                        A.Enquadramento1Id,
                        A.Enquadramento2Id,
                        A.ComissaoAgente,
                        B.NF
                    FROM
	                    Tb_DUE_Item_Detalhes A
                    INNER JOIN
	                    TB_DUE_Item B ON A.DueItemId = B.Id
                    WHERE
	                    B.DueId = @dueId
                    ORDER BY
	                    A.Item", new { dueId });
            }
        }

        public IEnumerable<DUEItensVinculoDTO> ObterItensVinculoNota(int dueId, int usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);

                return con.Query<DUEItensVinculoDTO>(@"
                    SELECT
	                    A.Id,
                        B.DescricaoMercadoria,
                        SUM(B.QuantidadeUnidades) QuantidadeUnidades,
                        SUM(B.PesoLiquidoTotal) PesoLiquidoTotal,
                        SUM(B.ValorMercadoriaLocalEmbarque) ValorMercadoriaLocalEmbarque,
                        SUM(B.ValorMercadoriaCondicaoVenda) ValorMercadoriaCondicaoVenda,
                        (SELECT SUM(QuantidadeNF) As Total FROM TB_DUE_NF WHERE ChaveNf = A.NF AND TipoNF = 'EXP' AND ISNULL(DueId, 0) = 0 AND (Usuario = @Usuario OR Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario))) As TotalEXP,
                        (SELECT SUM(QuantidadeNF) As Total FROM TB_DUE_NF WHERE ChaveNfReferencia = A.NF AND TipoNF = 'FDL' AND ISNULL(DueId, 0) = 0 AND (Usuario = @Usuario OR Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario))) As TotalFDL,
                        (SELECT SUM(QuantidadeNF) As Total FROM TB_DUE_NF WHERE ChaveNfReferencia = A.NF AND (TipoNF = 'REM' OR TipoNF = 'NFF') AND ISNULL(DueId, 0) = 0 AND (Usuario = @Usuario OR Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario))) As TotalREM_NFF,
                        A.NF,
	                    row_number() over ( order by a.Id ) as 'Item'
                    FROM
	                    Tb_DUE_Item A
                    LEFT JOIN
	                    Tb_DUE_Item_Detalhes B ON B.DueItemId = A.Id                    
                    WHERE
	                    A.DueId = @DueId
                    GROUP BY
	                    A.Id,
	                    A.NF,
	                    B.DescricaoMercadoria
                    ORDER BY Item", parametros);
            }
        }

        public void VincularNotaItem(int idItem, string nf)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: idItem, direction: ParameterDirection.Input);
                parametros.Add(name: "NF", value: nf, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE_Item] SET [NF] = @NF WHERE [Id] = @Id", parametros);
            }
        }

        public void MarcarComoAutomatica(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET Completa = 1, CriadoPorNF = 1 WHERE [Id] = @Id", parametros);
            }
        }

        public void ApagarVinculoNF(int idItem)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Id", value: idItem, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE_Item] SET [NF] = NULL WHERE [Id] = @Id", parametros);
            }
        }

        public DUEItemDetalhes ObterDetalhesItemPorId(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<DUEItemDetalhes>(@"SELECT * FROM Tb_DUE_Item_Detalhes WHERE Id = @detalheItemId", new { detalheItemId }).FirstOrDefault();
            }
        }

        public void ExcluirDetalheItem(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: detalheItemId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes] WHERE Id = @Id", parametros);
            }
        }

        public void MarcarComoRetificado(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: dueId, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[Tb_DUE] SET RetificadoSiscomex = 1 WHERE Id = @Id", parametros);
            }
        }

        public void RegistrarLPCO(DUEItemDetalhesLPCO dueItemLPCO)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "IdDetalheItem", value: dueItemLPCO.IdDetalheItem, direction: ParameterDirection.Input);
                parametros.Add(name: "Numero", value: dueItemLPCO.Numero, direction: ParameterDirection.Input);

                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Execute(@"
                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_LPCO]
                           (
                               [IdDetalheItem],
                               [Numero]
                            ) VALUES (
                               @IdDetalheItem,
                               @Numero
                            )", parametros);
            }
        }

        public void AtualizarLPCO(DUEItemDetalhesLPCO dueItemLPCO)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "IdDetalheItem", value: dueItemLPCO.IdDetalheItem, direction: ParameterDirection.Input);
                parametros.Add(name: "Numero", value: dueItemLPCO.Numero, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: dueItemLPCO.Id, direction: ParameterDirection.Input);

                con.Execute(@"
                        UPDATE [dbo].[Tb_DUE_Item_Detalhes_LPCO]
                           SET
                               [Numero] = @Numero
                            WHERE [Id] = @Id", parametros);
            }
        }

        public void ExcluirLPCO(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_LPCO] WHERE Id = @Id", parametros);
            }
        }

        public void ExcluirTodosLPCODoItem(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[Tb_DUE_Item_Detalhes_LPCO] WHERE IdDetalheItem = @IdDetalheItem", parametros);
            }
        }

        public IEnumerable<DUEItemDetalhesLPCO> ObterLPCO(int detalheItemId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);

                return con.Query<DUEItemDetalhesLPCO>(@"SELECT * FROM [dbo].[Tb_DUE_Item_Detalhes_LPCO] WHERE IdDetalheItem = @IdDetalheItem", parametros);
            }
        }

        public DUEItemDetalhesLPCO ObterLPCOPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<DUEItemDetalhesLPCO>(@"SELECT * FROM [dbo].[Tb_DUE_Item_Detalhes_LPCO] WHERE Id = @Id", parametros).FirstOrDefault();
            }
        }

        public DueItemNota ObterItemPorNota(string chaveNF, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "NF", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<DueItemNota>(@"SELECT Id FROM [dbo].[Tb_DUE_Item] WHERE NF = @NF AND DueId = @DueId", parametros).FirstOrDefault();
            }
        }

        public void ExcluirNotaFiscalManual(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE Id = @Id", parametros);
            }
        }

        public IEnumerable<DueItemNota> ObterNotasFiscaisDUE(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<DueItemNota>(@"SELECT DISTINCT A.Id, A.ChaveNF FROM [dbo].[Tb_DUE_NF] A INNER JOIN [dbo].[Tb_DUE_ITEM] B ON A.DueId = B.DueId WHERE A.DueId = @DueId AND A.TipoNF = 'EXP'", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasPorDueId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT Id, TipoNF, ChaveNF, Item, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo, Usuario, DueId, '' As Recinto FROM [dbo].[TB_DUE_NF] WHERE DueId = @Id", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasExportacaoPorDueId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"
                    SELECT 
	                    A.Id, 
	                    A.TipoNF, 
                        A.Item,
	                    A.ChaveNF, 
	                    A.NumeroNF, 
	                    A.CNPJNF, 
	                    A.QuantidadeNF, 
	                    A.UnidadeNF, 
	                    A.NCM, 
	                    A.ChaveNFReferencia, 
	                    A.Arquivo, 
	                    A.Usuario, 
	                    A.DueId 
                    FROM 
	                    [dbo].[TB_DUE_NF] A
                    INNER JOIN
	                    [dbo].[TB_DUE_ITEM] B ON A.ChaveNF = B.NF
                    WHERE
	                    A.DueId = @Id AND A.TipoNF = 'EXP'", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasRemessaPorDueId(string chave)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Chave", value: chave, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"
                    SELECT 
	                    A.Id, 
	                    A.TipoNF, 
                        A.Item,
	                    A.ChaveNF, 
	                    A.NumeroNF, 
	                    A.CNPJNF, 
	                    A.QuantidadeNF, 
	                    A.UnidadeNF, 
	                    A.NCM, 
	                    A.ChaveNFReferencia, 
	                    A.Arquivo, 
	                    A.Usuario, 
	                    A.DueId 
                    FROM 
	                    [dbo].[TB_DUE_NF] A
                    WHERE
	                    A.ChaveNFReferencia = @Chave AND A.TipoNF <> 'EXP'", parametros);
            }
        }

        public IEnumerable<DueItemNota> ObterNotasFiscaisRemessaDUE(string chaveNF, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveNFReferencia", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<DueItemNota>(@"SELECT Id, ChaveNF, TipoNF, Item FROM [dbo].[Tb_DUE_NF] WHERE DueId = @DueId AND ChaveNFReferencia = @ChaveNFReferencia", parametros);
            }
        }

        public bool JaExisteNotaCadastrada(string chaveNF, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<bool>(@"SELECT Id FROM [dbo].[Tb_DUE_NF] WHERE ChaveNF = @ChaveNF AND DueId = @DueId", parametros).Any();
            }
        }

        public bool JaExisteNotaRemessaCadastrada(string chaveNF, int item, int dueId, string chaveNfRef)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: item, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNFReferencia", value: chaveNfRef, direction: ParameterDirection.Input);

                return con.Query<bool>(@"SELECT Id FROM [dbo].[Tb_DUE_NF] WHERE ChaveNF = @ChaveNF AND Item = @Item AND DueId = @DueId AND ChaveNFReferencia = @ChaveNFReferencia", parametros).Any();
            }
        }

        public decimal ObterQuantidadeDUEPorNF(string chaveNf)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveNF", value: chaveNf, direction: ParameterDirection.Input);

                return con.Query<decimal>(@"
                    SELECT 
	                    ISNULL(MAX(A.QuantidadeEstatistica),0) QuantidadeEstatistica 
                    FROM 
	                    TB_DUE_ITEM_DETALHES A 
                    INNER JOIN 
	                    TB_DUE_ITEM B ON A.DUEITEMID = B.ID 
                    INNER JOIN 
	                    TB_DUE C ON B.DueId = C.Id 
                    INNER JOIN
	                    TB_DUE_NF D ON B.NF = D.ChaveNFReferencia
                    WHERE 
	                    C.DUE IS NULL AND D.ChaveNF = @ChaveNF AND ISNULL(D.DueId, 0) = 0", parametros).FirstOrDefault();
            }
        }

        public int CriarItensDUE(
            List<NotaFiscal> notasFiscais,
            int usuario,
            string valorUnitVMLE_default,
            string valorUnitVMCV_default,
            string paisDestino_default,
            string enquadramento1_default,
            string enquadramento2_default,
            string enquadramento3_default,
            string enquadramento4_default,
            string condicaoVenda_default,
            string lpco_default,
            string prioridade_default,
            string descricaoComplementar_default,
            string comissaoAgente_Default,
            string AC_Tipo_Default,
            string AC_Exp_Benef_Default,
            string AC_Numero_Default,
            string AC_CNPJ_Default,
            string AC_Item_Default,
            string AC_NCM_Default,
            string AC_Qtde_Default,
            string AC_VMLE_Sem_Cob_Default,
            string AC_VMLE_Com_Cob_Default,
            string padraoQualidade_Default,
            string embarcadoEm_Default,
            string tipo_Default,
            string metodoProcessamento_Default,
            string caracteristicaEspecial_Default,
            string outraCaracteristicaEspecial_Default,
            int embalagemFinal_Default,
            string ncmDefault)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "UsuarioId", value: usuario, direction: ParameterDirection.Input);

                    parametros.Add(name: "ValorUnitVMLE_Default", value: valorUnitVMLE_default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "ValorUnitVMCV_Default", value: valorUnitVMCV_default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "PaisDestino_Default", value: paisDestino_default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento1_Default", value: enquadramento1_default.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento2_Default", value: enquadramento2_default.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento3_Default", value: enquadramento3_default.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento4_Default", value: enquadramento4_default.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "LPCO_Default", value: lpco_default, direction: ParameterDirection.Input);
                    parametros.Add(name: "CondicaoVenda_Default", value: condicaoVenda_default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Prioridade_Default", value: prioridade_default, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoComplementar_Default", value: descricaoComplementar_default, direction: ParameterDirection.Input);
                    parametros.Add(name: "ComissaoAgente_Default", value: comissaoAgente_Default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_Numero_Default", value: AC_Numero_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_CNPJ_Default", value: AC_CNPJ_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_Item_Default", value: AC_Item_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_NCM_Default", value: AC_NCM_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_Qtde_Default", value: AC_Qtde_Default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_VMLE_Sem_Cob_Default", value: AC_VMLE_Sem_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "AC_VMLE_Com_Cob_Default", value: AC_VMLE_Com_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Padrao_Qualidade_Default", value: padraoQualidade_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Embarque_Em_Default", value: embarcadoEm_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Tipo_Default", value: tipo_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Metodo_Processamento_Default", value: metodoProcessamento_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Caracteristica_Especial_Default", value: caracteristicaEspecial_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Outra_Caracteristica_Especial_Default", value: outraCaracteristicaEspecial_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Embalagem_Final_Default", value: embalagemFinal_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Ncm_Default", value: ncmDefault, direction: ParameterDirection.Input);

                    var dueId = con.Query<int>(@"
                        INSERT INTO [dbo].[Tb_DUE] (DataCadastro, UsuarioCadastro, MoedaNegociacao, CriadoPorNF, Completa, ValorUnitVMLE_Default, ValorUnitVMCV_Default, PaisDestino_Default, Enquadramento1_Default, Enquadramento2_Default, Enquadramento3_Default, Enquadramento4_Default, LPCO_Default, CondicaoVenda_Default, Prioridade_Default, DescricaoComplementar_Default, ComissaoAgente_Default, AC_Numero_Default, AC_CNPJ_Default, AC_Item_Default, AC_NCM_Default, AC_Qtde_Default, AC_VMLE_Sem_Cob_Default, AC_VMLE_Com_Cob_Default, Attr_Padrao_Qualidade_Default, Attr_Tipo_Default, Attr_Metodo_Processamento_Default, Attr_Outra_Caracteristica_Especial_Default, Attr_Caracteristica_Especial_Default, Attr_Embarque_Em_Default, Attr_Embalagem_Final_Default, NCM_Default) 
                            VALUES (GETDATE(), @UsuarioId, 'USD', 1, 1, @ValorUnitVMLE_Default, @ValorUnitVMCV_Default, @PaisDestino_Default, @Enquadramento1_Default, @Enquadramento2_Default, @Enquadramento3_Default, @Enquadramento4_Default, @LPCO_Default, @CondicaoVenda_Default, @Prioridade_Default, @DescricaoComplementar_Default, @ComissaoAgente_Default, @AC_Numero_Default, @AC_CNPJ_Default, @AC_Item_Default, @AC_NCM_Default, @AC_Qtde_Default, @AC_VMLE_Sem_Cob_Default, @AC_VMLE_Com_Cob_Default, @Attr_Padrao_Qualidade_Default, @Attr_Tipo_Default, @Attr_Metodo_Processamento_Default, @Attr_Outra_Caracteristica_Especial_Default, @Attr_Caracteristica_Especial_Default, @Attr_Embarque_Em_Default, @Attr_Embalagem_Final_Default, @Ncm_Default); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                    foreach (var nf in notasFiscais)
                    {
                        parametros = new DynamicParameters();

                        parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                        parametros.Add(name: "NF", value: nf.ChaveNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorUnitVMLE", value: valorUnitVMLE_default.ToDecimal(), direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorUnitVMCV", value: valorUnitVMCV_default.ToDecimal(), direction: ParameterDirection.Input);
                        parametros.Add(name: "CondicaoVenda", value: condicaoVenda_default, direction: ParameterDirection.Input);

                        var dueItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item] (DueId, NF, ValorUnitVMLE, ValorUnitVMCV, CondicaoVenda) VALUES (@DueId, @NF, @ValorUnitVMLE, @ValorUnitVMCV, @CondicaoVenda); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                        parametros = new DynamicParameters();

                        parametros.Add(name: "DueItemId", value: dueItemId, direction: ParameterDirection.Input);
                        parametros.Add(name: "NCM", value: nf.NCM, direction: ParameterDirection.Input);
                        parametros.Add(name: "DescricaoUnidade", value: nf.UnidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "QuantidadeEstatistica", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "QuantidadeUnidades", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "PaisDestino", value: paisDestino_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "PrioridadeCarga", value: prioridade_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "DescricaoComplementar", value: descricaoComplementar_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "ComissaoAgente", value: comissaoAgente_Default.ToDecimal(), direction: ParameterDirection.Input);

                        decimal pesoLiquidoTotal = 0;

                        if (nf.UnidadeNF.Equals("TON"))
                        {
                            pesoLiquidoTotal = nf.QuantidadeNF * 1000;
                        }
                        else
                        {
                            pesoLiquidoTotal = nf.QuantidadeNF;
                        }

                        parametros.Add(name: "PesoLiquidoTotal", value: pesoLiquidoTotal, direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento1Id", value: enquadramento1_default.ToInt(), direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento2Id", value: enquadramento2_default.ToInt(), direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento3Id", value: enquadramento3_default.ToInt(), direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento4Id", value: enquadramento4_default.ToInt(), direction: ParameterDirection.Input);

                        var resultadoVMLE = ((pesoLiquidoTotal * (valorUnitVMLE_default.ToDecimal() / 1000)));
                        var resultadoVMCV = ((pesoLiquidoTotal * (valorUnitVMCV_default.ToDecimal() / 1000)));

                        parametros.Add(name: "ValorMercadoriaLocalEmbarque", value: resultadoVMLE, direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorMercadoriaCondicaoVenda", value: resultadoVMCV, direction: ParameterDirection.Input);

                        parametros.Add(name: "Attr_Padrao_Qualidade", value: padraoQualidade_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Tipo", value: tipo_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Metodo_Processamento", value: metodoProcessamento_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Outra_Caracteristica_Especial", value: outraCaracteristicaEspecial_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Caracteristica_Especial", value: caracteristicaEspecial_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Embarque_Em", value: embarcadoEm_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Embalagem_Final", value: embalagemFinal_Default, direction: ParameterDirection.Input);

                        var detalheItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item_Detalhes] (DueItemId, Item, NCM, DescricaoUnidade, Enquadramento1Id, Enquadramento2Id, Enquadramento3Id, Enquadramento4Id, CodigoProduto, QuantidadeEstatistica, QuantidadeUnidades, PesoLiquidoTotal, ValorMercadoriaLocalEmbarque, ValorMercadoriaCondicaoVenda, PaisDestino, PrioridadeCarga, DescricaoComplementar, ComissaoAgente, Attr_Padrao_Qualidade, Attr_Tipo, Attr_Metodo_Processamento, Attr_Outra_Caracteristica_Especial, Attr_Caracteristica_Especial, Attr_Embarque_Em, Attr_Embalagem_Final) VALUES (@DueItemId, 1, @NCM, @DescricaoUnidade, @Enquadramento1Id, @Enquadramento2Id, @Enquadramento3Id, @Enquadramento4Id, 0, @QuantidadeEstatistica, @QuantidadeUnidades, @PesoLiquidoTotal, @ValorMercadoriaLocalEmbarque, @ValorMercadoriaCondicaoVenda, @PaisDestino, @PrioridadeCarga, @DescricaoComplementar, @ComissaoAgente, @Attr_Padrao_Qualidade, @Attr_Tipo, @Attr_Metodo_Processamento, @Attr_Outra_Caracteristica_Especial, @Attr_Caracteristica_Especial, @Attr_Embarque_Em, @Attr_Embalagem_Final); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                        if (!string.IsNullOrEmpty(lpco_default))
                        {
                            if (enquadramento1_default.ToInt() == 80380 || enquadramento2_default.ToInt() == 80380 || enquadramento3_default.ToInt() == 80380 || enquadramento4_default.ToInt() == 80380)
                            {
                                var lpcos = lpco_default.Split(',');

                                foreach (var lpco in lpcos)
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Numero", value: lpco, direction: ParameterDirection.Input);

                                    parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                    con.Execute(@"
                                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_LPCO] ([IdDetalheItem],[Numero]) VALUES (@IdDetalheItem,@Numero)", parametros, transaction);
                                }
                            }
                        }

                        if (AC_Numero_Default != string.Empty)
                        {
                            if (AC_Numero_Default.ToInt() > 0)
                            {
                                if (enquadramento1_default.ToInt() == 81101 || enquadramento2_default.ToInt() == 81101 || enquadramento3_default.ToInt() == 81101 || enquadramento4_default.ToInt() == 81101)
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Numero", value: AC_Numero_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "CNPJBeneficiario", value: AC_CNPJ_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "NumeroItem", value: AC_Item_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "NCMItem", value: AC_NCM_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "VMLESemCoberturaCambial", value: AC_VMLE_Sem_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                                    parametros.Add(name: "TipoAC", value: AC_Tipo_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "ExportadorBeneficiario", value: AC_Exp_Benef_Default.ToInt(), direction: ParameterDirection.Input);

                                    if (enquadramento1_default.ToInt() == 81101 || enquadramento2_default.ToInt() == 81101 || enquadramento3_default.ToInt() == 81101 || enquadramento4_default.ToInt() == 81101)
                                    {
                                        parametros.Add(name: "QuantidadeUtilizada", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                                        parametros.Add(name: "VMLEComCoberturaCambial", value: resultadoVMLE, direction: ParameterDirection.Input);
                                    }
                                    else
                                    {
                                        parametros.Add(name: "QuantidadeUtilizada", value: AC_Qtde_Default.ToDecimal(), direction: ParameterDirection.Input);
                                        parametros.Add(name: "VMLEComCoberturaCambial", value: AC_VMLE_Com_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                                    }

                                    con.Execute(@"
                                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_AC]
                                           (
                                               [IdDetalheItem],
                                               [Numero],
                                               [CNPJBeneficiario],
                                               [NumeroItem],
                                               [NCMItem],
                                               [QuantidadeUtilizada],
                                               [VMLEComCoberturaCambial],
                                               [VMLESemCoberturaCambial],
                                               [TipoAC],
                                               [ExportadorBeneficiario]
                                            ) VALUES (
                                               @IdDetalheItem,
                                               @Numero,
                                               @CNPJBeneficiario,
                                               @NumeroItem,
                                               @NCMItem,
                                               @QuantidadeUtilizada,
                                               @VMLEComCoberturaCambial,
                                               @VMLESemCoberturaCambial,
                                               @TipoAC,
                                               @ExportadorBeneficiario
                                            )", parametros, transaction);
                                }
                            }
                        }
                    }

                    transaction.Commit();

                    return dueId;
                }
            }
        }

        public string ObterMensagemPadrao(string ncm, string unidade)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);
                parametros.Add(name: "Unidade", value: unidade, direction: ParameterDirection.Input);

                return con.Query<string>("SELECT Mensagem FROM Tb_Mensagens_Recinto_NCM WHERE NCM = @NCM AND UnidadeRFB = @Unidade", parametros).FirstOrDefault();
            }
        }
        public int CriarItensDUEFromXML(
           int dueId,
           int usuario,
           string cnpjExportador,
           string razaoSocialExportador,
           string enderecoExportador,
           string estadoExportador,
           string nomeDestinatario,
           string enderecoDestinatario,
           string paisDestinatario,
           string chaveEXP,
           int nfItem,
           string ncm,
           string descricaoProduto,
           string descricaoUnidade,
           decimal quantidadeEstatistica,
           decimal quantidadeUnidades,
           decimal valorUnitVMLE,
           decimal valorUnitVMCV,
           string enquadramento,
           DUEMaster due)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    //var parametros = new DynamicParameters();

                    ////parametros = new DynamicParameters();

                    //parametros.Add(name: "NF", value: chaveEXP, direction: ParameterDirection.Input);
                    //parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                    var parametros = new DynamicParameters();

                    parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                    parametros.Add(name: "NF", value: chaveEXP, direction: ParameterDirection.Input);
                    parametros.Add(name: "Exportador", value: razaoSocialExportador, direction: ParameterDirection.Input);
                    parametros.Add(name: "ExportadorDocumento", value: cnpjExportador, direction: ParameterDirection.Input);
                    parametros.Add(name: "ExportadorEndereco", value: enderecoExportador, direction: ParameterDirection.Input);
                    parametros.Add(name: "ExportadorUF", value: estadoExportador, direction: ParameterDirection.Input);
                    parametros.Add(name: "ValorUnitVMLE", value: valorUnitVMLE, direction: ParameterDirection.Input);
                    parametros.Add(name: "ValorUnitVMCV", value: valorUnitVMCV, direction: ParameterDirection.Input);
                    parametros.Add(name: "CondicaoVenda", value: due.CondicaoVenda_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "ImportadorPais", value: paisDestinatario, direction: ParameterDirection.Input);
                    parametros.Add(name: "ImportadorEndereco", value: enderecoDestinatario, direction: ParameterDirection.Input);
                    parametros.Add(name: "Importador", value: nomeDestinatario, direction: ParameterDirection.Input);

                    var dueItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item] (DueId, NF, Exportador, ExportadorDocumento, ValorUnitVMLE, ValorUnitVMCV, CondicaoVenda, ExportadorPais, ExportadorEndereco, ExportadorUF, Importador, ImportadorPais, ImportadorEndereco) VALUES (@DueId, @NF, @Exportador, @ExportadorDocumento, @ValorUnitVMLE, @ValorUnitVMCV, @CondicaoVenda, 'BR', @ExportadorEndereco, @ExportadorUF, @Importador, @ImportadorPais, @ImportadorEndereco); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                    parametros = new DynamicParameters();

                    parametros.Add(name: "DueItemId", value: dueItemId, direction: ParameterDirection.Input);
                    parametros.Add(name: "NumeroItem", value: nfItem, direction: ParameterDirection.Input);
                    parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoUnidade", value: descricaoUnidade, direction: ParameterDirection.Input);
                    parametros.Add(name: "QuantidadeEstatistica", value: quantidadeEstatistica, direction: ParameterDirection.Input);
                    parametros.Add(name: "QuantidadeUnidades", value: quantidadeUnidades, direction: ParameterDirection.Input);
                    parametros.Add(name: "PaisDestino", value: paisDestinatario, direction: ParameterDirection.Input);
                    parametros.Add(name: "PrioridadeCarga", value: due.Prioridade_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoComplementar", value: due.DescricaoComplementar_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "ComissaoAgente", value: due.ComissaoAgente_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoMercadoria", value: descricaoProduto, direction: ParameterDirection.Input);

                    decimal pesoLiquidoTotal = 0;

                    if (descricaoUnidade.Equals("TON"))
                    {
                        pesoLiquidoTotal = quantidadeEstatistica * 1000;
                    }
                    else
                    {
                        pesoLiquidoTotal = quantidadeEstatistica;
                    }

                    parametros.Add(name: "PesoLiquidoTotal", value: pesoLiquidoTotal, direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento1Id", value: enquadramento, direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento2Id", value: due.Enquadramento2_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento3Id", value: due.Enquadramento3_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Enquadramento4Id", value: due.Enquadramento4_Default, direction: ParameterDirection.Input);

                    var resultadoVMLE = ((pesoLiquidoTotal * (valorUnitVMLE / 1000)));
                    var resultadoVMCV = ((pesoLiquidoTotal * (valorUnitVMCV / 1000)));

                    parametros.Add(name: "ValorMercadoriaLocalEmbarque", value: resultadoVMLE, direction: ParameterDirection.Input);
                    parametros.Add(name: "ValorMercadoriaCondicaoVenda", value: resultadoVMCV, direction: ParameterDirection.Input);

                    parametros.Add(name: "Attr_Padrao_Qualidade", value: due.Attr_Padrao_Qualidade_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Tipo", value: due.Attr_Tipo_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Metodo_Processamento", value: due.Attr_Metodo_Processamento_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Outra_Caracteristica_Especial", value: due.Attr_Outra_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Caracteristica_Especial", value: due.Attr_Caracteristica_Especial_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Embarque_Em", value: due.Attr_Embarque_Em_Default, direction: ParameterDirection.Input);
                    parametros.Add(name: "Attr_Embalagem_Final", value: due.Attr_Embalagem_Final_Default, direction: ParameterDirection.Input);

                    var detalheItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item_Detalhes] (DueItemId, Item, NCM, DescricaoMercadoria, DescricaoUnidade, Enquadramento1Id, Enquadramento2Id, Enquadramento3Id, Enquadramento4Id, CodigoProduto, QuantidadeEstatistica, QuantidadeUnidades, PesoLiquidoTotal, ValorMercadoriaLocalEmbarque, ValorMercadoriaCondicaoVenda, PaisDestino, PrioridadeCarga, DescricaoComplementar, ComissaoAgente, Attr_Padrao_Qualidade, Attr_Tipo, Attr_Metodo_Processamento, Attr_Outra_Caracteristica_Especial, Attr_Caracteristica_Especial, Attr_Embarque_Em, Attr_Embalagem_Final) VALUES (@DueItemId, @NumeroItem, @NCM, @DescricaoMercadoria, @DescricaoUnidade, @Enquadramento1Id, @Enquadramento2Id, @Enquadramento3Id, @Enquadramento4Id, 0, @QuantidadeEstatistica, @QuantidadeUnidades, @PesoLiquidoTotal, @ValorMercadoriaLocalEmbarque, @ValorMercadoriaCondicaoVenda, @PaisDestino, @PrioridadeCarga, @DescricaoComplementar, @ComissaoAgente, @Attr_Padrao_Qualidade, @Attr_Tipo, @Attr_Metodo_Processamento, @Attr_Outra_Caracteristica_Especial, @Attr_Caracteristica_Especial, @Attr_Embarque_Em, @Attr_Embalagem_Final); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                    if (!string.IsNullOrEmpty(due.LPCO_Default))
                    {
                        if (enquadramento.ToInt() == 80380 || due.Enquadramento2_Default == 80380 || due.Enquadramento3_Default == 80380 || due.Enquadramento4_Default == 80380)
                        {
                            var lpcos = due.LPCO_Default.Split(',');

                            foreach (var lpco in lpcos)
                            {
                                parametros = new DynamicParameters();

                                parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                parametros.Add(name: "Numero", value: lpco, direction: ParameterDirection.Input);

                                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                con.Execute(@"
                                    INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_LPCO] ([IdDetalheItem],[Numero]) VALUES (@IdDetalheItem,@Numero)", parametros, transaction);
                            }
                        }
                    }

                    if (due.AC_Numero_Default != string.Empty)
                    {
                        if (due.AC_Numero_Default.ToInt() > 0)
                        {
                            if (enquadramento.ToInt() == 81101 || due.Enquadramento2_Default == 81101 || due.Enquadramento3_Default == 81101 || due.Enquadramento4_Default == 81101)
                            {
                                parametros = new DynamicParameters();

                                parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                parametros.Add(name: "Numero", value: due.AC_Numero_Default, direction: ParameterDirection.Input);
                                parametros.Add(name: "CNPJBeneficiario", value: due.AC_CNPJ_Default, direction: ParameterDirection.Input);
                                parametros.Add(name: "NumeroItem", value: due.AC_Item_Default, direction: ParameterDirection.Input);
                                parametros.Add(name: "NCMItem", value: due.AC_NCM_Default, direction: ParameterDirection.Input);
                                parametros.Add(name: "VMLESemCoberturaCambial", value: due.AC_VMLE_Sem_Cob_Default, direction: ParameterDirection.Input);

                                if (due.Enquadramento1_Default == 81101 || due.Enquadramento2_Default == 81101 || due.Enquadramento3_Default == 81101 || due.Enquadramento4_Default == 81101)
                                {
                                    parametros.Add(name: "QuantidadeUtilizada", value: quantidadeUnidades, direction: ParameterDirection.Input);
                                    parametros.Add(name: "VMLEComCoberturaCambial", value: resultadoVMLE, direction: ParameterDirection.Input);
                                }
                                else
                                {
                                    parametros.Add(name: "QuantidadeUtilizada", value: due.AC_Qtde_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "VMLEComCoberturaCambial", value: due.AC_VMLE_Com_Cob_Default, direction: ParameterDirection.Input);
                                }

                                con.Execute(@"
                                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_AC]
                                           (
                                               [IdDetalheItem],
                                               [Numero],
                                               [CNPJBeneficiario],
                                               [NumeroItem],
                                               [NCMItem],
                                               [QuantidadeUtilizada],
                                               [VMLEComCoberturaCambial],
                                               [VMLESemCoberturaCambial]
                                            ) VALUES (
                                               @IdDetalheItem,
                                               @Numero,
                                               @CNPJBeneficiario,
                                               @NumeroItem,
                                               @NCMItem,
                                               @QuantidadeUtilizada,
                                               @VMLEComCoberturaCambial,
                                               @VMLESemCoberturaCambial
                                            )", parametros, transaction);
                            }
                        }
                    }

                    transaction.Commit();

                    return dueId;
                }
            }
        }

        public int CriarItensDeDUEJaExistente(
            List<NotaFiscal> notasFiscais,
            int dueId,
            int usuario,
            string valorUnitVMLE_default,
            string valorUnitVMCV_default,
            string enquadramento,
            string paisDestino_default,
            string enquadramento1_default,
            string enquadramento2_default,
            string enquadramento3_default,
            string enquadramento4_default,
            string condicaoVenda_default,
            string lpco_default,
            string prioridade_default,
            string descricaoComplementar_default,
            string comissaoAgente_Default,
            string AC_Tipo_Default,
            string AC_Exp_Benefic_Default,
            string AC_Numero_Default,
            string AC_CNPJ_Default,
            string AC_Item_Default,
            string AC_NCM_Default,
            string AC_Qtde_Default,
            string AC_VMLE_Sem_Cob_Default,
            string AC_VMLE_Com_Cob_Default,
            string padraoQualidade_Default,
            string embarcadoEm_Default,
            string tipo_Default,
            string metodoProcessamento_Default,
            string caracteristicaEspecial_Default,
            string outraCaracteristicaEspecial_Default,
            int embalagemFinal_Default,
            string ncm_default)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();

                    foreach (var nf in notasFiscais)
                    {
                        //var mensagemPadrao = "";

                        //if (!string.IsNullOrEmpty(nf.NCM))
                        //{
                        //    mensagemPadrao = ObterMensagemPadrao(nf.NCM, unidadeEmbarque);
                        //}

                        parametros = new DynamicParameters();
                        parametros.Add(name: "NF", value: nf.ChaveNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                        var jaExisteItemComEstaNF = con.Query<bool>("SELECT Id FROM Tb_DUE_Item WHERE NF = @NF AND DueId = @DueId", parametros, transaction).Any();

                        if (jaExisteItemComEstaNF)
                        {
                            continue;
                        }

                        parametros = new DynamicParameters();

                        parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                        parametros.Add(name: "NF", value: nf.ChaveNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorUnitVMLE", value: valorUnitVMLE_default.ToDecimal(), direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorUnitVMCV", value: valorUnitVMCV_default.ToDecimal(), direction: ParameterDirection.Input);
                        parametros.Add(name: "CondicaoVenda", value: condicaoVenda_default, direction: ParameterDirection.Input);



                        var dueItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item] (DueId, NF, ValorUnitVMLE, ValorUnitVMCV, CondicaoVenda) VALUES (@DueId, @NF, @ValorUnitVMLE, @ValorUnitVMCV, @CondicaoVenda); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                        parametros = new DynamicParameters();

                        parametros.Add(name: "DueItemId", value: dueItemId, direction: ParameterDirection.Input);
                        parametros.Add(name: "NCM", value: nf.NCM, direction: ParameterDirection.Input);
                        parametros.Add(name: "DescricaoUnidade", value: nf.UnidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "QuantidadeEstatistica", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "QuantidadeUnidades", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                        parametros.Add(name: "PaisDestino", value: paisDestino_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "PrioridadeCarga", value: prioridade_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "DescricaoComplementar", value: descricaoComplementar_default, direction: ParameterDirection.Input);
                        parametros.Add(name: "ComissaoAgente", value: comissaoAgente_Default.ToDecimal(), direction: ParameterDirection.Input);

                        decimal pesoLiquidoTotal = 0;

                        if (nf.UnidadeNF.Equals("TON"))
                        {
                            pesoLiquidoTotal = nf.QuantidadeNF * 1000;
                        }
                        else
                        {
                            pesoLiquidoTotal = nf.QuantidadeNF;
                        }

                        parametros.Add(name: "PesoLiquidoTotal", value: pesoLiquidoTotal, direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento1Id", value: enquadramento, direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento2Id", value: enquadramento2_default.ToInt(), direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento3Id", value: enquadramento3_default.ToInt(), direction: ParameterDirection.Input);
                        parametros.Add(name: "Enquadramento4Id", value: enquadramento4_default.ToInt(), direction: ParameterDirection.Input);

                        var resultadoVMLE = ((pesoLiquidoTotal * (valorUnitVMLE_default.ToDecimal() / 1000)));
                        var resultadoVMCV = ((pesoLiquidoTotal * (valorUnitVMCV_default.ToDecimal() / 1000)));

                        parametros.Add(name: "ValorMercadoriaLocalEmbarque", value: resultadoVMLE, direction: ParameterDirection.Input);
                        parametros.Add(name: "ValorMercadoriaCondicaoVenda", value: resultadoVMCV, direction: ParameterDirection.Input);

                        parametros.Add(name: "Attr_Padrao_Qualidade", value: padraoQualidade_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Tipo", value: tipo_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Metodo_Processamento", value: metodoProcessamento_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Outra_Caracteristica_Especial", value: outraCaracteristicaEspecial_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Caracteristica_Especial", value: caracteristicaEspecial_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Embarque_Em", value: embarcadoEm_Default, direction: ParameterDirection.Input);
                        parametros.Add(name: "Attr_Embalagem_Final", value: embalagemFinal_Default, direction: ParameterDirection.Input);

                        var detalheItemId = con.Query<int>(@"
                            INSERT INTO 
                                [dbo].[Tb_DUE_Item_Detalhes] (DueItemId, Item, NCM, DescricaoUnidade, Enquadramento1Id, Enquadramento2Id, Enquadramento3Id, Enquadramento4Id, CodigoProduto, QuantidadeEstatistica, QuantidadeUnidades, PesoLiquidoTotal, ValorMercadoriaLocalEmbarque, ValorMercadoriaCondicaoVenda, PaisDestino, PrioridadeCarga, DescricaoComplementar, ComissaoAgente, Attr_Padrao_Qualidade, Attr_Tipo, Attr_Metodo_Processamento, Attr_Outra_Caracteristica_Especial, Attr_Caracteristica_Especial, Attr_Embarque_Em, Attr_Embalagem_Final) VALUES (@DueItemId, 1, @NCM, @DescricaoUnidade, @Enquadramento1Id, @Enquadramento2Id, @Enquadramento3Id, @Enquadramento4Id, 0, @QuantidadeEstatistica, @QuantidadeUnidades, @PesoLiquidoTotal, @ValorMercadoriaLocalEmbarque, @ValorMercadoriaCondicaoVenda, @PaisDestino, @PrioridadeCarga, @DescricaoComplementar, @ComissaoAgente, @Attr_Padrao_Qualidade, @Attr_Tipo, @Attr_Metodo_Processamento, @Attr_Outra_Caracteristica_Especial, @Attr_Caracteristica_Especial, @Attr_Embarque_Em, @Attr_Embalagem_Final); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros, transaction).Single();

                        if (!string.IsNullOrEmpty(lpco_default))
                        {
                            if (enquadramento.ToInt() == 80380 || enquadramento2_default.ToInt() == 80380 || enquadramento3_default.ToInt() == 80380 || enquadramento4_default.ToInt() == 80380)
                            {
                                var lpcos = lpco_default.Split(',');

                                foreach (var lpco in lpcos)
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Numero", value: lpco, direction: ParameterDirection.Input);

                                    parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                                    con.Execute(@"
                                    INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_LPCO] ([IdDetalheItem],[Numero]) VALUES (@IdDetalheItem,@Numero)", parametros, transaction);
                                }
                            }
                        }

                        if (AC_Numero_Default != string.Empty)
                        {
                            if (AC_Numero_Default.ToInt() > 0)
                            {
                                if (enquadramento.ToInt() == 81101 || enquadramento2_default.ToInt() == 81101 || enquadramento3_default.ToInt() == 81101 || enquadramento4_default.ToInt() == 81101)
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "IdDetalheItem", value: detalheItemId, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Numero", value: AC_Numero_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "CNPJBeneficiario", value: AC_CNPJ_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "NumeroItem", value: AC_Item_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "NCMItem", value: AC_NCM_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "VMLESemCoberturaCambial", value: AC_VMLE_Sem_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                                    parametros.Add(name: "TipoAC", value: AC_Tipo_Default, direction: ParameterDirection.Input);
                                    parametros.Add(name: "ExportadorBeneficiario", value: AC_Exp_Benefic_Default, direction: ParameterDirection.Input);

                                    if (enquadramento1_default.ToInt() == 81101 || enquadramento2_default.ToInt() == 81101 || enquadramento3_default.ToInt() == 81101 || enquadramento4_default.ToInt() == 81101)
                                    {
                                        parametros.Add(name: "QuantidadeUtilizada", value: nf.QuantidadeNF, direction: ParameterDirection.Input);
                                        parametros.Add(name: "VMLEComCoberturaCambial", value: resultadoVMLE, direction: ParameterDirection.Input);
                                    }
                                    else
                                    {
                                        parametros.Add(name: "QuantidadeUtilizada", value: AC_Qtde_Default.ToDecimal(), direction: ParameterDirection.Input);
                                        parametros.Add(name: "VMLEComCoberturaCambial", value: AC_VMLE_Com_Cob_Default.ToDecimal(), direction: ParameterDirection.Input);
                                    }

                                    con.Execute(@"
                                        INSERT INTO [dbo].[Tb_DUE_Item_Detalhes_AC]
                                           (
                                               [IdDetalheItem],
                                               [Numero],
                                               [CNPJBeneficiario],
                                               [NumeroItem],
                                               [NCMItem],
                                               [QuantidadeUtilizada],
                                               [VMLEComCoberturaCambial],
                                               [VMLESemCoberturaCambial],
                                               [TipoAC],
                                               [ExportadorBeneficiario]
                                            ) VALUES (
                                               @IdDetalheItem,
                                               @Numero,
                                               @CNPJBeneficiario,
                                               @NumeroItem,
                                               @NCMItem,
                                               @QuantidadeUtilizada,
                                               @VMLEComCoberturaCambial,
                                               @VMLESemCoberturaCambial,
                                               @TipoAC,
                                               @ExportadorBeneficiario
                                            )", parametros, transaction);
                                }
                            }
                        }
                    }

                    transaction.Commit();

                    return dueId;
                }
            }
        }
    }
}