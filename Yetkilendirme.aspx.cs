using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.View
{
    public partial class Yetkilendirme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                int authorityId;
                if (int.TryParse(Session["AuthorityId"]?.ToString(), out authorityId) && authorityId == 3)
                {
                }
                else
                {
                    // Kullanıcının yetkisi 3 değilse, isteği başka bir sayfaya yönlendir
                    Response.Redirect("~/View/ReportPage.aspx");
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void ddlYetki_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlOtel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnOnayla_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcının girdiği bilgileri al
                string kullaniciAdi = MainContent_txtAd.Text;
                string yetki = MainContent_ddlYetki.SelectedValue;
                string otel = MainContent_ddlOtel.SelectedValue;

                // Gerekli alanların boş olup olmadığını kontrol et
                if (!string.IsNullOrEmpty(kullaniciAdi) && !string.IsNullOrEmpty(yetki) && !string.IsNullOrEmpty(otel))
                {
                    // Veritabanına bağlantıyı kur
                    string connectionString = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Veritabanına ekleme sorgusunu hazırla
                        string insertQuery = "UPDATE [Users] SET AuthorityId = @AuthorityId WHERE UserName = @UserName ";

                        // Ekleme sorgusunu çalıştır
                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserName", kullaniciAdi);
                            cmd.Parameters.AddWithValue("@AuthorityId", yetki);
                            cmd.Parameters.AddWithValue("@HotelId", otel);

                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }

                    // Başarılı bir şekilde eklendiğini kullanıcıya bildir
                    Response.Write("<script>alert('Yetkilendirme başarıyla eklendi.');</script>");
                }
                else
                {
                    // Gerekli alanları doldurun uyarısı
                    Response.Write("<script>alert('Lütfen tüm alanları doldurun.');</script>");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bir hata mesajı göster
                Response.Write("<script>alert('Bir hata oluştu: " + ex.Message + "');</script>");
            }
        }
    }
}