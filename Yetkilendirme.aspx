<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Yetkilendirme.aspx.cs" Inherits="WebApplication1.View.Yetkilendirme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- SqlDataSourceOtel güncelleniyor -->
    <asp:SqlDataSource runat="server" ID="SqlDataSourceOtel" ConnectionString="<%$ ConnectionStrings:DivanDevConnectionString %>" SelectCommand="SELECT id, HotelName FROM [Hotels]"></asp:SqlDataSource>

    <!-- SqlDataSourceYetki güncelleniyor -->
    <asp:SqlDataSource runat="server" ID="SqlDataSourceYetki" ConnectionString="<%$ ConnectionStrings:DivanDevConnectionString %>" SelectCommand="SELECT ID, Authority FROM [Authorizations]"></asp:SqlDataSource>

    <div class="container-fluid">
        <h2 class="text-danger text-center">Kullanıcı Yetkilendirme</h2>
        <div class="mb-3 mt-5">
            <asp:Label ID="MainContent_lblAd" CssClass="form-label" runat="server" Text="Yetkilendirilecek Kişinin Kullanıcı Adı: "></asp:Label>
            <asp:TextBox ID="MainContent_txtAd" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="mb-3">
            <asp:Label ID="MainContent_llblYetki" CssClass="form-label" runat="server" Text="Verilecek Olan Yetki: "></asp:Label>
            <asp:DropDownList ID="MainContent_ddlYetki" CssClass="form-select" runat="server" OnSelectedIndexChanged="ddlYetki_SelectedIndexChanged" DataTextField="Authority" DataValueField="id" DataSourceID="SqlDataSourceYetki"></asp:DropDownList>
        </div>       
        <div class="mb-3">
            <asp:Label ID="MainContent_lblOtel" CssClass="form-label" runat="server" Text="Otel: "></asp:Label>
            <asp:DropDownList ID="MainContent_ddlOtel" CssClass="form-select" runat="server" OnSelectedIndexChanged="ddlOtel_SelectedIndexChanged" DataTextField="HotelName" DataValueField="id" DataSourceID="SqlDataSourceOtel"></asp:DropDownList>
        </div>       
        <div class="btn btn-success" style="background-color:#0d6efd">
            <asp:Button Text="Onayla" runat="server" OnClick="btnOnayla_Click" BorderStyle="None" BackColor="#0d6efd" ForeColor="White"/>
        </div>    
    </div>
</asp:Content>