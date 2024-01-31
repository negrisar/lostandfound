using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Web.UI.WebControls;

namespace WebApplication1.View
{
    public partial class Silinenler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                if (!this.IsPostBack)
                {
                    this.SearchCustomers();
                }
                GridView1.RowCommand += GridView1_RowCommand;
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Restore")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                KaydiGeriYukle(id);
                SearchCustomers();
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
                                        string sql = @"
                                        SELECT lf.id, lf.FoundDate, lf.HotelID, lf.Notes, lf.DeleteDate, lf.Item, lf.RoomId, lf.State, lf.Updater, h.HotelName, r.RoomName
                                        FROM [Lost&FoundDeleted] lf
                                        LEFT JOIN Hotels h ON h.id = lf.HotelID
                                        LEFT JOIN Rooms R ON R.id = lf.RoomId
                                        where DeleteDate between @StartDate AND @EndDate";

                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " And lf.Item LIKE @Item + '%'";
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
                                        ViewState["LostAndFoundDeletedData"] = dt;

                                        // Filtrelenmiş veriyi GridView'e ata
                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }
                                }
                                else
                                {
                                    string sql = @"
                                    SELECT lf.id, lf.FoundDate, lf.HotelID, lf.Notes, lf.DeleteDate, lf.Item, lf.RoomId, lf.State, lf.Updater, h.HotelName, r.RoomName
                                    FROM [Lost&FoundDeleted] lf
                                    LEFT JOIN Hotels h ON h.id = lf.HotelID
                                    LEFT JOIN Rooms R ON R.id = lf.RoomId
                                    where DeleteDate between @StartDate AND @EndDate 
                                    INNER JOIN Users u ON u.HotelId = lf.HotelID
                                    WHERE u.Username = @P1 And DeleteDate between @StartDate AND @EndDate ";

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
                                        ViewState["LostAndFoundDeletedData"] = dt;

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
                                        SELECT lf.id, lf.FoundDate, lf.HotelID, lf.Notes, lf.DeleteDate, lf.Item, lf.RoomId, lf.State, lf.Updater, h.HotelName, r.RoomName
                                        FROM [Lost&FoundDeleted] lf
                                        LEFT JOIN Hotels h ON h.id = lf.HotelID
                                        LEFT JOIN Rooms R ON R.id = lf.RoomId";
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

                                        foreach (DataRow row in dt.Rows)
                                        {
                                            if (row["HotelID"] != DBNull.Value)
                                            {
                                                int hotelId = Convert.ToInt32(row["HotelID"]);
                                                row["HotelName"] = GetHotelNameById(hotelId);
                                            }

                                            if (row["RoomId"] != DBNull.Value)
                                            {
                                                int roomId = Convert.ToInt32(row["RoomId"]);
                                                row["RoomName"] = GetRoomNameById(roomId);
                                            }
                                        }

                                        ViewState["LostAndFoundDeletedData"] = dt;

                                        GridView1.DataSource = dt;
                                        GridView1.DataBind();
                                    }

                                }
                                else
                                {
                                    string sql = @"
                                        SELECT lf.id, lf.FoundDate, lf.HotelID, lf.Notes, lf.DeleteDate, lf.Item, lf.RoomId, lf.State, lf.Updater, h.HotelName, r.RoomName
                                        FROM [Lost&FoundDeleted] lf
                                        LEFT JOIN Hotels h ON h.id = lf.HotelID
                                        LEFT JOIN Rooms R ON R.id = lf.RoomId
                                        INNER JOIN Users u ON u.HotelId = lf.HotelID ";

                                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                                    {
                                        sql += " Where Item LIKE @Item + '%'";
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
                                        ViewState["LostAndFoundDeletedData"] = dt;

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


        private string GetHotelNameById(object hotelIdObj)
        {
            string hotelName = string.Empty;

            if (hotelIdObj != DBNull.Value)
            {
                int hotelId = Convert.ToInt32(hotelIdObj);
                string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    string query = "SELECT HotelName FROM Hotels WHERE id = @HotelID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@HotelID", hotelId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            hotelName = result.ToString();
                        }
                    }
                    con.Close();
                }
            }

            return hotelName;
        }

        private string GetRoomNameById(object roomIdObj)
        {
            string RoomName = string.Empty;

            if (roomIdObj != DBNull.Value)
            {
                int roomId = Convert.ToInt32(roomIdObj);
                string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    string query = "SELECT RoomName FROM Rooms WHERE id = @RoomId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@RoomId", roomId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            RoomName = result.ToString();
                        }
                    }
                    con.Close();
                }
            }

            return RoomName;
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

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int authorityId;
            if (int.TryParse(Session["AuthorityId"].ToString(), out authorityId) && authorityId > 1)
            {
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string query = "DELETE FROM [Lost&FoundDeleted] WHERE id = @id";
                string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
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
                LinkButton linkButton = e.Row.Cells[7].Controls[2] as LinkButton;
                if (linkButton != null)
                {
                    (e.Row.Cells[7].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Bu satırı silmek istediğinize emin misiniz?');";
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
            DataTable dt = ViewState["LostAndFoundDeletedData"] as DataTable;
            DataView dv = new DataView(dt);

            // Apply the sort order
            dv.Sort = $"{sortExpression} {sortDirection}";

            GridView1.DataSource = dv;
            GridView1.PageIndex = 0; // Reset page index when sorting
            GridView1.DataBind();
        }




        private void KaydiGeriYukle(int id)
        {
            string selectQuery = "SELECT * FROM [Lost&FoundDeleted] WHERE id = @id";
            string deleteQuery = "DELETE FROM [Lost&FoundDeleted] WHERE id = @id";
            string insertQuery = "INSERT INTO [Lost&Found] (FoundDate, HotelID, Notes, UpdateDate, Item, RoomId, State, Updater) VALUES (@FoundDate, @HotelID, @Notes, @UpdateDate, @Item, @RoomId, @State, @Updater)";

            string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                {
                    selectCmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime foundDate = Convert.ToDateTime(reader["FoundDate"]);
                            int hotelID = Convert.ToInt32(reader["HotelID"]);
                            string notes = reader["Notes"].ToString();
                            DateTime updateDate = Convert.ToDateTime(reader["DeleteDate"]);
                            string item = reader["Item"].ToString();
                            int roomId = Convert.ToInt32(reader["RoomId"]);
                            string state = reader["State"].ToString();
                            string updater = reader["Updater"].ToString();

                            // SqlDataReader'ı kapat
                            reader.Close();

                            using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                            {
                                deleteCmd.Parameters.AddWithValue("@id", id);
                                deleteCmd.ExecuteNonQuery();
                            }

                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                            {
                                insertCmd.Parameters.AddWithValue("@FoundDate", foundDate);
                                insertCmd.Parameters.AddWithValue("@HotelID", hotelID);
                                insertCmd.Parameters.AddWithValue("@Notes", notes);
                                insertCmd.Parameters.AddWithValue("@UpdateDate", updateDate);
                                insertCmd.Parameters.AddWithValue("@Item", item);
                                insertCmd.Parameters.AddWithValue("@RoomId", roomId);
                                insertCmd.Parameters.AddWithValue("@State", state);
                                insertCmd.Parameters.AddWithValue("@Updater", updater);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                con.Close();
            }
        }
    }
}