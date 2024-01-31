<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KayipEsyaVeriGirisi.aspx.cs" Inherits="WebApplication1.View.KayipEsyaVeriGirisi" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- SqlDataSourceOtel eklenmiş hali -->
    <asp:SqlDataSource runat="server" ID="SqlDataSourceOtel" ConnectionString="<%$ ConnectionStrings:DivanDevConnectionString %>" SelectCommand="SELECT id, HotelName FROM [Hotels]"></asp:SqlDataSource>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid">
                <h2 class="text-danger text-center">Kayıp Eşya Veri Girişi</h2>
                <div class="mb-3">
                    <asp:Label AssociatedControlID="MainContent_txtEsya" ID="MainContent_lblEsya" CssClass="form-label" runat="server" Text="Bulunan Eşya:"></asp:Label>
                    <asp:TextBox ID="MainContent_txtEsya" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Eşya Giriniz" ForeColor="Red" ControlToValidate="MainContent_txtEsya" Font-Size="Smaller"></asp:RequiredFieldValidator>
                </div>
                <div class="mb-3">
                    <asp:Label AssociatedControlID="MainContent_ddlBulunmayeri" ID="MainContent_lblBulunmayeri" CssClass="form-label" runat="server" Text="Bulunduğu Yer:"></asp:Label>
                    <asp:DropDownList ID="MainContent_ddlBulunmayeri" runat="server" CssClass="form-select" OnSelectedIndexChanged="MainContent_ddlBulunmayeri_SelectedIndexChanged" AutoPostBack="true" DataTextField="HotelName" DataValueField="id" DataSourceID="SqlDataSourceOtel">
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <asp:Label AssociatedControlID="MainContent_ddlBulunmayerioda" ID="MainContent_lblBulunmayerioda" CssClass="form-label" runat="server" Text="Bulunduğu Oda:"></asp:Label>
                    <asp:DropDownList ID="MainContent_ddlBulunmayerioda" runat="server" CssClass="form-select" DataValueField="id" DataTextField="RoomName">
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <asp:Label AssociatedControlID="PhotoUpload" ID="MainContent_lblFoto" CssClass="form-label" runat="server" Text="Fotoğraf:"></asp:Label>
                    <asp:FileUpload ID="PhotoUpload" CssClass="form-control" runat="server" AllowMultiple="True" />
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="Fotoğraf Ekleyiniz" ForeColor="Red" ControlToValidate="PhotoUpload" Font-Size="Smaller"></asp:RequiredFieldValidator>--%>
                </div>

                <div class="mb-3">
                    <asp:Label AssociatedControlID="MainContent_txtNot" ID="MainContent_lblNot" CssClass="form-label" runat="server" Text="Notlar:"></asp:Label>
                    <asp:TextBox ID="MainContent_txtNot" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Not Giriniz" ForeColor="Red" ControlToValidate="MainContent_txtNot" Font-Size="Smaller"></asp:RequiredFieldValidator>
                </div>
                <div class="mb-3">
                    <asp:Label AssociatedControlID="MainContent_ddlDurum" ID="MainContent_lblDurum" CssClass="form-label" runat="server" Text="Durumu:"></asp:Label>
                    <asp:DropDownList ID="MainContent_ddlDurum" runat="server">
                        <asp:ListItem>depoda</asp:ListItem>
                        <asp:ListItem>teslim edildi</asp:ListItem>
                        <asp:ListItem>hurda</asp:ListItem>
                        <asp:ListItem>atık</asp:ListItem>
                        <asp:ListItem>müşteri talebi oluşturuldu</asp:ListItem>
                        <asp:ListItem>bulan kişiye teslim edildi</asp:ListItem>
                        <asp:ListItem>zaman aşımına uğradı</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="MainContent_txtBulunmaTarihi">Bulunma Tarihi:</label>
                    <asp:TextBox type="date" ID="MainContent_txtBulunmaTarihi" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Tarih Giriniz" ControlToValidate="MainContent_txtBulunmaTarihi" Font-Size="Smaller" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <asp:Button Text="Ekle" runat="server" OnClick="btnEkle_Click" CssClass="btn btn-primary" />
                <div>
                    <asp:Label ID="MainContent_lblsuccess" CssClass="form-label" runat="server" Text=""></asp:Label>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
