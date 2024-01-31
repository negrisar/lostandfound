using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Web;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using System.Globalization;


namespace WebApplication1.View
{
    public partial class ReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                if (!this.IsPostBack)
                {
                    this.SearchCustomers();
                }

            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        
        private void SearchCustomers(string sortExpression = "", bool isAscending = true)
        {
            string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    
                    cmd.Parameters.AddWithValue("@P1", Session["Username"]);
                    if (Session["UserName"] != null && Session["AuthorityId"] != null)
                    {
                        // Eğer AuthorityId bir tamsayıysa bu türde dönüşüm yapın
                        if (int.TryParse(Session["AuthorityId"].ToString(), out int authorityId))
                        {
                            DateTime startDate;
                            DateTime endDate;
                            if (DateTime.TryParse(txtStartDate.Text, out startDate) && DateTime.TryParse(txtEndDate.Text, out endDate))
                            {
                                // Parametreleri ekleyerek tarih aralığına göre filtreleme yap
                                cmd.Parameters.AddWithValue("@StartDate", startDate);
                                cmd.Parameters.AddWithValue("@EndDate", endDate);
                                if (authorityId == 3)
                                {
                                    string sql = @"SELECT lf.id, lf.FoundDate, lf.Item, lf.Notes, lf.State, lf.UpdateDate, h.HotelName, R.RoomName
                                    FROM [Lost&Found] lf
                                    FULL JOIN Hotels h ON h.id = lf.HotelId
                                    full join Rooms R on R.id = lf.RoomId
                                    where FoundDate between  @StartDate AND @EndDate";

                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " AND Item LIKE @Item + '%'";
                                        cmd.Parameters.AddWithValue("@Item", txtSearch.Text.Trim());
                                    }

                                    // If there is a sort expression, add an ORDER BY clause
                                    if (!string.IsNullOrEmpty(sortExpression))
                                    {
                                        string orderByClause = isAscending ? "ASC" : "DESC";
                                        sql += $" ORDER BY {sortExpression} {orderByClause}";
                                    }

                                    cmd.CommandText = sql;
                                    cmd.Connection = con;

                                    // SqlDataAdapter kullanarak veriyi çek
                                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                    {
                                        DataTable dt = new DataTable();
                                        adapter.Fill(dt);

                                        // Save the DataTable to ViewState
                                        ViewState["LostAndFoundData"] = dt;

                                        // Filtrelenmiş veriyi GridView'e ata
                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }
                                }
                                else
                                {
                                    string sql = @"SELECT lf.id, lf.FoundDate, lf.Item, lf.Notes, lf.State, lf.UpdateDate, h.HotelName, R.RoomName
                                    FROM [Lost&Found] lf
                                    FULL JOIN Hotels h ON h.id = lf.HotelId
                                    full join Rooms R on R.id = lf.RoomId
                                    INNER JOIN Users u ON u.HotelId = lf.HotelId 
                                    where h.id in (select u.HotelId where u.Username = @P1)
                                    AND FoundDate between  @StartDate AND @EndDate";

                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " AND Item LIKE @Item + '%'";
                                        cmd.Parameters.AddWithValue("@Item", txtSearch.Text.Trim());
                                    }

                                    // If there is a sort expression, add an ORDER BY clause
                                    if (!string.IsNullOrEmpty(sortExpression))
                                    {
                                        string orderByClause = isAscending ? "ASC" : "DESC";
                                        sql += $" ORDER BY {sortExpression} {orderByClause}";
                                    }


                                    cmd.CommandText = sql;
                                    cmd.Connection = con;

                                    // SqlDataAdapter kullanarak veriyi çek
                                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                    {
                                        DataTable dt = new DataTable();
                                        adapter.Fill(dt);

                                        // Save the DataTable to ViewState
                                        ViewState["LostAndFoundData"] = dt;

                                        // Filtrelenmiş veriyi GridView'e ata
                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }
                                }
                            }
                            else
                            {
                                if (authorityId == 3)
                                {
                                    string sql = @"
 
                            
                                SELECT lf.id, lf.FoundDate, lf.Item, lf.Notes, lf.State, lf.UpdateDate, h.HotelName, R.RoomName
                                FROM [Lost&Found] lf
                                FULL JOIN Hotels h ON h.id = lf.HotelId
                                full join Rooms R on R.id = lf.RoomId";


                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " WHERE Item LIKE @Item + '%'";
                                        cmd.Parameters.AddWithValue("@Item", txtSearch.Text.Trim());
                                    }

                                    // If there is a sort expression, add an ORDER BY clause
                                    if (!string.IsNullOrEmpty(sortExpression))
                                    {
                                        string orderByClause = isAscending ? "ASC" : "DESC";
                                        sql += $" ORDER BY {sortExpression} {orderByClause}";
                                    }

                                    cmd.CommandText = sql;
                                    cmd.Connection = con;

                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                    {
                                        DataTable dt = new DataTable();
                                        sda.Fill(dt);

                                        // Save the DataTable to ViewState
                                        ViewState["LostAndFoundData"] = dt;

                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }
                                }
                                else
                                {
                                    string sql = @"
 
                                SELECT lf.id, lf.FoundDate, lf.Item, lf.Notes, lf.State, lf.UpdateDate, h.HotelName, R.RoomName
                                FROM [Lost&Found] lf
                                FULL JOIN Hotels h ON h.id = lf.HotelId
                                full join Rooms R on R.id = lf.RoomId
                                INNER JOIN Users u ON u.HotelId = lf.HotelId 
                                where h.id in (select u.HotelId where u.Username = @P1)
                                ";

                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " AND Item LIKE @Item + '%'";
                                        cmd.Parameters.AddWithValue("@Item", txtSearch.Text.Trim());
                                    }

                                    // If there is a sort expression, add an ORDER BY clause
                                    if (!string.IsNullOrEmpty(sortExpression))
                                    {
                                        string orderByClause = isAscending ? "ASC" : "DESC";
                                        sql += $" ORDER BY {sortExpression} {orderByClause}";
                                    }

                                    cmd.CommandText = sql;
                                    cmd.Connection = con;

                                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                    {
                                        DataTable dt = new DataTable();
                                        sda.Fill(dt);

                                        // Save the DataTable to ViewState
                                        ViewState["LostAndFoundData"] = dt;

                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }
                                }
                            }
                        }

                    }


                }
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = ViewState["SortDirection"] as string;

            // If the clicked column is the same as the current sort column, reverse the sort direction
            if (sortExpression == ViewState["SortExpression"] as string)
            {
                sortDirection = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                // If a new column is clicked, set the sort direction to ASC
                sortDirection = "ASC";
            }

            // Perform your data retrieval and sorting
            SearchCustomers(sortExpression, sortDirection == "ASC");

            // Update the ViewState with the new sort information
            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortDirection;

            // Rebind the GridView using the DataTable in ViewState
            DataTable dt = ViewState["LostAndFoundData"] as DataTable;
            DataView dv = new DataView(dt);

            // Apply the sort order
            dv.Sort = $"{sortExpression} {sortDirection}";

            GridView1.DataSource = dv;
            GridView1.PageIndex = 0; // Reset page index when sorting
            GridView1.DataBind();
        }


        protected void Search(object sender, EventArgs e)
        {
            this.SearchCustomers();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            string sortExpression = ViewState["SortExpression"] as string;
            string sortDirection = ViewState["SortDirection"] as string;

            GridView1.PageIndex = e.NewPageIndex;

            // Convert sort direction string to boolean
            bool isAscending = string.Equals(sortDirection, "ASC", StringComparison.OrdinalIgnoreCase);

            // Reapply the sorting and bind data
            this.SearchCustomers(sortExpression, isAscending);
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.SearchCustomers();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
            GridViewRow row = GridView1.Rows[e.RowIndex];
            
            try
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string foundDate = (row.FindControl("txtFoundDate") as TextBox).Text;
                DateTime parsedDate = DateTime.ParseExact(foundDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string formattedDate = parsedDate.ToString("yyyy/MM/dd");
                string item = (row.FindControl("txtItem") as TextBox).Text;
                string notes = (row.FindControl("txtNotes") as TextBox).Text;
                string state = (row.FindControl("ddlState") as DropDownList).SelectedValue;
                string query = "insert into [UpdateLog](FoundDate,HotelID,Notes,UpdateDate,Item,RoomId,[State],LFID)\r\nselect FoundDate,HotelID,Notes,UpdateDate,Item,RoomId,[State],id from [Lost&Found] lf\r\nwhere id = @id \r\nupdate [UpdateLog]\r\nset Updater = @P1 where LFID = @id UPDATE [Lost&Found] SET FoundDate=@FoundDate, Item=@Item, Notes=@Notes, State=@State, UpdateDate=GetDate() WHERE id=@id ";
                string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@P1", Session["Username"]);
                        cmd.Parameters.AddWithValue("@FoundDate", formattedDate);
                        cmd.Parameters.AddWithValue("@Item", item);
                        cmd.Parameters.AddWithValue("@Notes", notes);
                        cmd.Parameters.AddWithValue("@State", state);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                GridView1.EditIndex = -1;
                this.SearchCustomers();
            }
            catch (FormatException ex)
            {
                throw ex;
            }
                
            
        }
        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            this.SearchCustomers();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int authorityId;
            if ((int.TryParse(Session["AuthorityId"].ToString(), out authorityId) && authorityId > 1))
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string query = "insert into [Lost&FoundDeleted](FoundDate,HotelID,Notes,DeleteDate,Item,RoomId,[State],LFID) " +
                    "select FoundDate,HotelID,Notes,UpdateDate,Item,RoomId,[State],id from [Lost&Found] lf\r\nwhere id = @id " +
                    "update [Lost&FoundDeleted]\r\nset Updater = @P1 where LFID = @id " +
                    "delete from [Lost&Found] where id = @id ";

                //string query = "DELETE FROM [Lost&Found] WHERE id=@id";
                string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@P1", Session["Username"]);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                this.SearchCustomers();
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.Cells[7].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Bu satırı silmek istediğinize emin misiniz?');";
            }
        }

    }
}
