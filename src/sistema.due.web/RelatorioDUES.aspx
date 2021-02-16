<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RelatorioDUES.aspx.cs" Inherits="Sistema.DUE.Web.RelatorioDUES" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Relatório de DUE's</h3>
                </div>
                <div class="panel-body">

                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">DUE:</label>
                                <asp:TextBox ID="txtDUE" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Emissão DUE (De):</label>
                                <asp:TextBox ID="txtEmissaoDUEDe" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Emissão DUE (Até):</label>
                                <asp:TextBox ID="txtEmissaoDueAte" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Exportador:</label>
                                <asp:TextBox ID="txtCnpjExportador" runat="server" CssClass="form-control form-control-sm cnpj"></asp:TextBox>
                            </div>
                        </div>     
                        <div class="col-md-1 col-md-offset-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnLimparFiltro" runat="server" CssClass="btn btn-default btn-block" Text="Limpar" OnClick="btnLimparFiltro_Click" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-block" Text="Filtrar" OnClick="btnFiltrar_Click" />
                            </div>
                        </div>
                    </div>
                    
                    
                    <br />
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView
                                    ID="gvEstoque"
                                    runat="server"
                                    Width="100%"
                                    CssClass="table table-striped"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="13px"
                                    ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvEstoque_PageIndexChanging" >
                                    <Columns>                                        
                                        <asp:BoundField DataField="Due" HeaderText="DUE" />
                                        <asp:BoundField DataField="ExportadorCnpj" HeaderText="CNPJ Exportador" />
                                        <asp:BoundField DataField="UltimoEvento" HeaderText="Último Evento" />
                                        <asp:BoundField DataField="DataDue" HeaderText="Data DUE" />
                                        <asp:BoundField DataField="DataEmbarque" HeaderText="Data Embarque" />
                                        <asp:BoundField DataField="DataAverbacao" HeaderText="Data Averbação" />
                                        <asp:BoundField DataField="Canal" HeaderText="Canal" />
                                        <asp:BoundField DataField="Item" HeaderText="Item" />
                                        <asp:BoundField DataField="VMLE" HeaderText="VMLE" />
                                        <asp:BoundField DataField="CpfDue" HeaderText="CPF DUE" />
                                        <asp:BoundField DataField="NCM" HeaderText="NCM" />
                                        <asp:BoundField DataField="PesoLiquidoTotal" HeaderText="Peso Liq. Total" />                                        
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <asp:Button ID="btnGerarCsv" Text="Gerar .CSV" CssClass="btn btn-info btn-block" runat="server" OnClick="btnGerarCsv_Click" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/select2.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>
        
        $('#MainContent_txtEmissaoDUEDe').mask('00/00/0000');
        $('#MainContent_txtEmissaoDueAte').mask('00/00/0000');
        $('#MainContent_txtCnpjExportador').mask('00.000.000/0000-00');

    </script>

</asp:Content>
