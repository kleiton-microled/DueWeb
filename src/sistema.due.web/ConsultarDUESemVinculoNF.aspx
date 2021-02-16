<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarDUESemVinculoNF.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarDUESemVinculoNF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Consulta de DUEs sem vínculo de Nota Fiscal</h3>
                </div>
                <div class="panel-body">
                 
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView
                                    ID="gvDUE"
                                    runat="server"
                                    Width="100%"
                                    CssClass="table table-striped"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="11px"
                                    ShowHeaderWhenEmpty="True">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />
                                        <asp:BoundField DataField="DUE" HeaderText="DUE" />
                                        <asp:BoundField DataField="RUC" HeaderText="RUC" />
                                        <asp:BoundField DataField="DocumentoDeclarante" HeaderText="DOC. DECLARANTE" />
                                        <asp:BoundField DataField="MoedaNegociacao" HeaderText="MOEDA NEGOCIAÇÃO" />
                                        <asp:BoundField DataField="FormaExportacao" HeaderText="FORMA EXPORTAÇÃO" />
                                        <asp:BoundField DataField="RecintoAduaneiroDespachoId" HeaderText="RECINTO DESPACHO" />
                                        <asp:BoundField DataField="RecintoAduaneiroEmbarqueId" HeaderText="RECINTO EMBARQUE" />
                                        <asp:BoundField DataField="DataCadastro" HeaderText="CADASTRO" />
                                        <asp:TemplateField HeaderText="SISCOMEX">
                                            <ItemTemplate>
                                                <%# (Eval("EnviadoSiscomex").ToString() == "1" ? "<span class='label label-success'>Enviado</span>" : "<span class='label label-warning'>Pendente</span>") %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="VincularDUENF.aspx?due={0}" Text="Vincular" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>


    <div class="modal fade" id="modalNotasRef" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Notas Referenciadas</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <iframe id="frameNotasRef" width="550" height="200" frameborder="0"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script>
        function carregarNotasReferenciadas(chaveNf) {
            $('#frameNotasRef').attr('src', 'ConsultarNotasReferenciadas.aspx?chaveNF=' + chaveNf);
        }
    </script>

</asp:Content>
