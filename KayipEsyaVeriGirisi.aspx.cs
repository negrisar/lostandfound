using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication1.View
{
    public partial class KayipEsyaVeriGirisi : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                if (!IsPostBack)
                {
                    PopulateHotels();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        private void PopulateHotels()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, HotelName FROM Hotels";
                using (SqlCommand komut = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = komut.ExecuteReader())
                    {
                        MainContent_ddlBulunmayeri.Items.Clear();

                        while (reader.Read())
                        {
                            ListItem item = new ListItem(reader["HotelName"].ToString(), reader["id"].ToString());
                            MainContent_ddlBulunmayeri.Items.Add(item);
                        }
                    }
                }
                connection.Close();
            }
        }



        protected void MainContent_ddlBulunmayeri_SelectedIndexChanged(object sender, EventArgs e)
        {
            int hotelId;
            if (int.TryParse(MainContent_ddlBulunmayeri.SelectedValue, out hotelId))
            {
                PopulateRooms(hotelId);
                MainContent_ddlBulunmayerioda.DataBind();
                UpdatePanel1.Update();

                // Kullanıcı yetkili ise ve seçtiği otel kendi oteli değilse uyarı göster
                if (Session["UserName"] != null && Session["AuthorityId"] != null)
                {
                    int kullaniciOtelId = KullaniciOtelIdAl(Session["UserName"].ToString());
                    if (int.Parse(MainContent_ddlBulunmayeri.SelectedValue) != kullaniciOtelId)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Başka bir oteli seçtiniz. Lütfen kendi otelinizi seçiniz.');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Otel seçimi geçersiz.');", true);
            }
        }

        private void PopulateRooms(int hotelId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id, RoomName FROM Rooms WHERE HotelId = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", hotelId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            MainContent_ddlBulunmayerioda.Items.Clear();

                            while (reader.Read())
                            {
                                ListItem item = new ListItem(reader["RoomName"].ToString(), reader["id"].ToString());
                                MainContent_ddlBulunmayerioda.Items.Add(item);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Hata: " + ex.Message + "');</script>");

            }
        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserName"] != null && Session["AuthorityId"] != null)
                {
                    if (int.TryParse(Session["AuthorityId"].ToString(), out int authorityId))
                    {
                        int kullaniciOtelId = KullaniciOtelIdAl(Session["UserName"].ToString());

                        if (authorityId == 3 || kullaniciOtelId == int.Parse(MainContent_ddlBulunmayeri.SelectedValue))
                        {
                            string esyaAdi = MainContent_txtEsya.Text;
                            int hotelId, roomId;

                            if (int.TryParse(MainContent_ddlBulunmayeri.SelectedValue, out hotelId) &&
                                int.TryParse(MainContent_ddlBulunmayerioda.SelectedValue, out roomId))
                            {
                                string notlar = MainContent_txtNot.Text;
                                string durum = MainContent_ddlDurum.SelectedValue;
                                string bulunmaTarihi = MainContent_txtBulunmaTarihi.Text;

                                if (!string.IsNullOrEmpty(esyaAdi) && !string.IsNullOrEmpty(notlar) && !string.IsNullOrEmpty(durum) && !string.IsNullOrEmpty(bulunmaTarihi))
                                {
                                    string connectionString = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;


                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();


                                        string insertQuery = "INSERT INTO [Lost&Found] (FoundDate, HotelId, RoomId, Notes, State, UpdateDate, Item, Updater)" +
                                                   " VALUES (@FoundDate, @HotelId, @RoomId, @Notes, @State, @UpdateDate, @Item, @Updater); SELECT SCOPE_IDENTITY() ";

                                        int lfId;

                                        using (SqlCommand kmt = new SqlCommand(insertQuery, connection))
                                        {
                                            kmt.Parameters.AddWithValue("@FoundDate", bulunmaTarihi);
                                            kmt.Parameters.AddWithValue("@HotelId", hotelId);
                                            kmt.Parameters.AddWithValue("@RoomId", roomId);
                                            kmt.Parameters.AddWithValue("@Notes", notlar);
                                            kmt.Parameters.AddWithValue("@State", durum);
                                            kmt.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                                            kmt.Parameters.AddWithValue("@Item", esyaAdi);
                                            kmt.Parameters.AddWithValue("@Updater", Session["UserName"]);

                                            lfId = Convert.ToInt32(kmt.ExecuteScalar());
                                        }
                                        string fileName = Path.GetFileName(PhotoUpload.FileName);

                                        string folderPath = Server.MapPath("~/Images/");
                                        //Check whether Directory (Folder) exists.

                                        if (!Directory.Exists(folderPath))
                                        {
                                            //If Directory (Folder) does not exists Create it.
                                            Directory.CreateDirectory(folderPath);
                                        }

                                        //Save the File to the Directory (Folder).

                                            
                                            string windowsFileName = "Tutanak " + lfId + Path.GetExtension(fileName); // Dosya uzantısını ekleyin
                                            string filePath = Path.Combine(folderPath, fileName);
                                            PhotoUpload.SaveAs(filePath);


                                            // Insert data into Photos table
                                            string insertPhotosQuery = "INSERT INTO Photos (LFId, PName, Path) VALUES (@LFId, @PName, @Path)";
                                            string localFilePath = Path.Combine(folderPath, windowsFileName);


                                        using (SqlCommand cmd = new SqlCommand(insertPhotosQuery, connection))
                                            {
                                                cmd.Parameters.AddWithValue("@LFId", lfId);
                                                cmd.Parameters.AddWithValue("@PName", windowsFileName);
                                                cmd.Parameters.AddWithValue("@Path", localFilePath);
                                                cmd.ExecuteNonQuery();
                                            }



                                        //string fileName = Path.GetFileName(PhotoUpload.FileName);
                                        //string filePath = Path.Combine(folderPath, fileName);
                                        //PhotoUpload.SaveAs(filePath);





                                        connection.Close();
                                    }

                                    MainContent_lblsuccess.Text = "Eşya eklendi";
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Lütfen tüm alanları doldurunuz.');", true);

                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Otel veya oda seçimi geçersiz.');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlert", "alert('Başka otel için kayıt eklemeye izniniz yok.');", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Bir hata oluştu: " + ex.Message + "');</script>");
            }
        }

        private int KullaniciOtelIdAl(string kullaniciAdi)
        {
            int kullaniciOtelId = 0;
            string constr = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                string query = "SELECT HotelId FROM Users WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", kullaniciAdi);
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        kullaniciOtelId = Convert.ToInt32(result);
                    }
                }
            }

            return kullaniciOtelId;
        }
    }
}