<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Silinenler.aspx.cs" Inherits="WebApplication1.View.Silinenler" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="margin-top:50px">
        <div class="search-container" style=" margin-left:40px; display:flex; flex-direction:row; justify-content:space-between; margin-right:60px;">
            <div class="searc" style="display:flex;">
                <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server" style="border-radius:0"></asp:TextBox>
                <asp:Button Text="Ara" CssClass="btn btn-primary" runat="server" style="border-radius:0" OnClick="Search" />
            </div>
            <div class="filter" style="display:flex">
                <asp:TextBox type="date" ID="txtStartDate" CssClass="form-control" style="border-radius:0" runat="server"></asp:TextBox>
                <asp:TextBox type="date" ID="txtEndDate" CssClass="form-control" style="border-radius:0" runat="server"></asp:TextBox>
                <asp:Button ID="btnFilter" CssClass="btn btn-secondary" runat="server" style="border-radius:0" Text="Filtrele" OnClick="Search" />
            </div>
        </div>
        <hr />
        <div style="margin-left:50px; margin-bottom:50px;">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                DataKeyNames="id" AllowPaging="True" AllowSorting="True"
                OnPageIndexChanging="OnPaging" OnRowDeleting="OnRowDeleting"
                OnRowDataBound="OnRowDataBound" OnSorting="GridView1_Sorting" EnableViewState="true" ViewStateMode="Enabled"
                EmptyDataText="Hiçbir kayıt eklenmedi." CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Bulunma Tarihi" ItemStyle-Width="150" SortExpression="FoundDate">
                        <ItemTemplate>
                            <asp:Label ID="lblFoundDate" runat="server" Text='<%# Eval("FoundDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFoundDate" runat="server" Text='<%# Eval("FoundDate", "{0:dd-MM-yyyy}") %>' Width="140"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Otel Adı" ItemStyle-Width="150" SortExpression="HotelName">
                        <ItemTemplate>
                            <asp:Label ID="lblHotelName" runat="server" Text='<%# Eval("HotelName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtHotelName" runat="server" Text='<%# Eval("HotelName") %>' Width="140" ReadOnly="True"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bulunma Yeri" ItemStyle-Width="150" SortExpression="RoomName">
                        <ItemTemplate>
                            <asp:Label ID="lblRoomName" runat="server" Text='<%# Eval("RoomName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRoomName" runat="server" Text='<%# Eval("RoomName") %>' Width="140" ReadOnly="True"></asp:TextBox>
                        </EditItemTemplate>
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
                            <asp:TextBox ID="txtState" runat="server" Text='<%# Eval("State") %>' Width="140"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Silinme Tarihi" ItemStyle-Width="150" SortExpression="DeleteDate">
                        <ItemTemplate>
                            <asp:Label ID="lblDeleteDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("DeleteDate").ToString())) ? "No Date Available" : Eval("DeleteDate", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDeleteDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("DeleteDate").ToString())) ? "No Date Available" : Eval("DeleteDate", "{0:d}") %>' Width="140" ReadOnly="True"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Silen Personel" ItemStyle-Width="150" SortExpression="Updater">
                        <ItemTemplate>
                            <asp:Label ID="lblUpdater" runat="server" Text='<%# Eval("Updater") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUpdater" runat="server" Text='<%# Eval("Updater") %>' Width="140" ReadOnly="True"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="İşlemler" ItemStyle-Width="150">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRestore" runat="server" CommandName="Restore" CommandArgument='<%# Eval("id") %>' Text="Geri Yükle" OnClientClick="return confirmRestore();"></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("id") %>' Text="Sil" OnClientClick="return confirmDelete();"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:TemplateField>
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

            <script type="text/javascript">
                function confirmRestore() {
                    return confirm('Bu satırı geri yüklemek istediğinize emin misiniz?');
                }

                function confirmDelete() {
                    return confirm('Bu satırı silmek istediğinize emin misiniz?');
                }
            </script>
        </div>
    </div>
</asp:Content>
