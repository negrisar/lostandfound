<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReportPage.aspx.cs" Inherits="WebApplication1.View.ReportPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <div class="container-fluid" style="margin-top: 50px">
        <div class="search-container" style="margin-left: 40px; display: flex; flex-direction: row; justify-content: space-between; margin-right: 60px;">
            <div class="searc" style="display: flex;">
                <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" Style="border-radius: 0"></asp:TextBox>
                <asp:Button Text="Ara" CssClass="btn btn-primary" runat="server" Style="border-radius: 0" OnClick="Search" />
            </div>
            <div class="filer" style="display: flex">
                <asp:TextBox type="date" ID="txtStartDate"  CssClass="form-control" Style="border-radius: 0" runat="server" ></asp:TextBox>
                
                <asp:TextBox type="date" ID="txtEndDate"  CssClass="form-control" Style="border-radius: 0" runat="server" ></asp:TextBox>

                <asp:Button ID="btnFilter" CssClass="btn btn-secondary" runat="server" Style="border-radius: 0" Text="Filtrele" OnClick="Search" />
            </div>
        </div>
    </div>
    <hr />
    <div style="margin-left: 50px; margin-bottom: 50px;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSorting="GridView1_Sorting"
            DataKeyNames="id" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit" AllowPaging="True" OnPageIndexChanging="OnPaging" OnRowUpdating="OnRowUpdating" EnableViewState="true"
            OnRowDeleting="OnRowDeleting" OnRowDataBound="OnRowDataBound" EmptyDataText="Hiçbir kayıt eklenmedi." CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="true" ViewStateMode="Enabled">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Bulunma Tarihi" ItemStyle-Width="150" SortExpression="FoundDate">
                    <ItemTemplate>
                        <asp:Label ID="lblFoundDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("FoundDate").ToString())) ? "No Date Available" : Eval("FoundDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFoundDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("FoundDate").ToString())) ? "No Date Available" : Eval("FoundDate", "{0:dd-MM-yyyy}") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Otel Adı" ItemStyle-Width="150" SortExpression="HotelName">
                    <ItemTemplate>
                        <asp:Label ID="lblHotelName" runat="server" Text='<%# Eval("HotelName") %>'></asp:Label>
                    </ItemTemplate>
                    <%--<EditItemTemplate>
                        <asp:TextBox ID="txtHotelName" runat="server" Text='<%# Eval("HotelName") %>' Width="140" ReadOnly="True"></asp:TextBox>
                    </EditItemTemplate>--%>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bulunma Yeri" ItemStyle-Width="150" SortExpression="RoomName">
                    <ItemTemplate>
                        <asp:Label ID="lblRoomName" runat="server" Text='<%# Eval("RoomName") %>'></asp:Label>
                    </ItemTemplate>
                    <%--<EditItemTemplate>
                        <asp:TextBox ID="txtRoomName" runat="server" Text='<%# Eval("RoomName") %>' Width="140" ReadOnly="True"></asp:TextBox>
                    </EditItemTemplate>--%>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eşya" ItemStyle-Width="150" SortExpression="Item">
                    <ItemTemplate>
                        <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtItem" runat="server" Text='<%# Eval("Item") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notlar" ItemStyle-Width="150" SortExpression="Notes">
                    <ItemTemplate>
                        <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("Notes") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNotes" runat="server" Text='<%# Eval("Notes") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Durumu" ItemStyle-Width="150" SortExpression="State">
                    <ItemTemplate>
                        <asp:Label ID="lblState" runat="server" Text='<%# Eval("State") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlState" SelectedValue='<%# Eval("State") %>'  runat="server">
                            <asp:ListItem>depoda</asp:ListItem>
                            <asp:ListItem>teslim edildi</asp:ListItem>
                            <asp:ListItem>hurda</asp:ListItem>
                            <asp:ListItem>atık</asp:ListItem>
                            <asp:ListItem>müşteri talebi oluşturuldu</asp:ListItem>
                            <asp:ListItem>bulan kişiye teslim edildi</asp:ListItem>
                            <asp:ListItem>zaman aşımına uğradı</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Son Güncellenme Tarihi" ItemStyle-Width="150" SortExpression="UpdateDate">
                    <ItemTemplate>
                        <asp:Label ID="lblUpdateDate" runat="server" Text='<%# Eval("UpdateDate") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
                    <%--<EditItemTemplate>
                        <asp:TextBox ID="txtUpdateDate" runat="server" Text='<%# Eval("UpdateDate") %>' Width="140" ReadOnly="True"></asp:TextBox>
                    </EditItemTemplate>--%>

                <%--<asp:TemplateField HeaderText="Foto Yükleme" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblUpload" runat="server" Text='<%# Eval("State") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtState" runat="server" Text='<%# Eval("State") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>

                    <ItemStyle Width="150px"></ItemStyle>
                </asp:TemplateField>
--%>

                <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
                    ItemStyle-Width="150" EditText="Değiştir" DeleteText="Sil" UpdateText="Güncelle" CancelText="İptal" ItemStyle-CssClass="custom-command-field a" ControlStyle-ForeColor="Black">
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:CommandField>
            </Columns>



            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>
</asp:Content>

