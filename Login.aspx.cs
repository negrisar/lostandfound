using System;
using System.DirectoryServices.AccountManagement;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Diagnostics;
using System.Configuration;


namespace WebApplication1.View
{
    public static class ADExtensionMethods
    {
        public static string GetPropertyValue(this SearchResult sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
                ret = sr.Properties[propertyName][0].ToString();

            return ret;

        }

    }
    public partial class Login : System.Web.UI.Page
    {

        private bool AuthenticateUser(string domainName, string userName, string password)
        {
            bool ret = false;

            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://" + domainName, userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                SearchResult results = null;

                results = dsearch.FindOne();

                ret = results != null; // Check if the search result is not null
            }
            catch
            {
                ret = false;
            }

            return ret;
        }




        private string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://divan.local");

            return "LDAP://divan.local" + de.Properties["defaultNamingContext"][0].ToString();

        }
        private void GetAllUsers()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new
            DirectoryEntry(GetCurrentDomainPath());

            ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                // Using the index zero (0) is required!
                Debug.WriteLine(sr.Properties["name"][0].ToString());
            }
        }
        private void GetAdditionalUserInfo()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());

            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                    Debug.WriteLine(sr.Properties["name"][0].ToString());

                // If not filled in, then you will get an error
                if (sr.Properties["mail"].Count > 0)
                    Debug.WriteLine(sr.Properties["mail"][0].ToString());

                if (sr.Properties["givenname"].Count > 0)
                    Debug.WriteLine(sr.Properties["givenname"][0].ToString());

                if (sr.Properties["sn"].Count > 0)
                    Debug.WriteLine(sr.Properties["sn"][0].ToString());

                if (sr.Properties["userPrincipalName"].Count > 0)
                    Debug.WriteLine(sr.Properties["userPrincipalName"][0].ToString());

                if (sr.Properties["distinguishedName"].Count > 0)
                    Debug.WriteLine(sr.Properties["distinguishedName"][0].ToString());
            }
        }
        private DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            return ds;
        }
        private void GetAUser(string userName)
        {
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            SearchResult sr;

            // Build User Searcher
            ds = BuildUserSearcher(de);
            // Set the filter to look for a specific user
            ds.Filter = "(&(objectCategory=User)(objectClass=person)(name=" + userName + "))";

            sr = ds.FindOne();

            if (sr != null)
            {
                Debug.WriteLine(sr.GetPropertyValue("name"));
                Debug.WriteLine(sr.GetPropertyValue("mail"));
                Debug.WriteLine(sr.GetPropertyValue("givenname"));
                Debug.WriteLine(sr.GetPropertyValue("sn"));
                Debug.WriteLine(sr.GetPropertyValue("userPrincipalName"));
                Debug.WriteLine(sr.GetPropertyValue("distinguishedName"));
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string EnteredUsername = TextBox1.Text.Trim();
            string EnteredPassword = TextBox2.Text.Trim();
            string UserCaptcha = (txtCaptcha.Text.Trim());

            Captcha1.ValidateCaptcha(UserCaptcha);
            if (Captcha1.UserValidated)
            {
                // Call the Active Directory authentication method
                bool adAuthenticated = AuthenticateUser("divan.local", EnteredUsername, EnteredPassword);

                Debug.WriteLine("Authentication Result: " + adAuthenticated);

                if (adAuthenticated)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["DivanDevConnectionString"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand komut = new SqlCommand(("Select * From Users where Username=@P1"), connection);
                        komut.Parameters.AddWithValue("@P1", EnteredUsername);
                        SqlDataReader dr = komut.ExecuteReader();

                        if (dr.Read())
                        {
                            Session["UserName"] = dr["Username"].ToString();
                            Session["AuthorityId"] = dr["AuthorityId"].ToString();
                        }
                        connection.Close();

                        if (Session["UserName"] != null && Session["AuthorityId"] != null)
                        {
                            // Eğer AuthorityId bir tamsayıysa bu türde dönüşüm yapın
                            if (int.TryParse(Session["AuthorityId"].ToString(), out int authorityId))
                            {
                                if (authorityId != 4)
                                {
                                    // Sayfa yönlendirme
                                    Response.Redirect("~/View/ReportPage.aspx");
                                }
                                else
                                {
                                    Response.Write("Giriş Yetkiniz Kaldırılmıştır. Lütfen Yöneticinizle Görüşünüz.");
                                }
                            }

                        }
                        else
                        {
                            Response.Write("<script>alert('Kullanıcı adı veya şifre yanlış!');</script>");
                        }
                    }
                }
                else
                {
                    Response.Write("<script>alert('Kullanıcı adı veya şifre yanlış!');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Girdiğiniz güvenlik kodu yanlış!');</script>");
            }
        }




        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtCaptcha_TextChanged(object sender, EventArgs e)
        {

        }



    }
}








//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using WebApplication1.Database;

//namespace WebApplication1.View
//{
//    public partial class Login : System.Web.UI.Page
//    {
//        SqlConnection baglanti = new SqlConnection(@"data source = 10.2.254.79; initial catalog = DivanDev; 
//            user id = devuser; password = 2Kek@1956; trustservercertificate = True;
//            MultipleActiveResultSets = True");
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }

//        protected void Button1_Click(object sender, EventArgs e)
//        {
//            string EnteredUsername = (TextBox1.Text.Trim());
//            string EnteredPassword = (TextBox2.Text.Trim());
//            string UserCaptcha = (txtCaptcha.Text.Trim());

//            // burada herhangi bir verinin bos olup olmadigini kontrol ediyoruz.
//            if (!string.IsNullOrEmpty(EnteredUsername) && !string.IsNullOrEmpty(EnteredPassword) && !string.IsNullOrEmpty(UserCaptcha))
//            {
//                //captcha kontrol ediyoruz!!
//                Captcha1.ValidateCaptcha(UserCaptcha);
//                if (Captcha1.UserValidated)
//                {

//                    try
//                    {
//                        bool login = false;

//                        login = Database.DatabaseOperation.Authenticate(PasswordOperation.Protect(EnteredUsername), PasswordOperation.Protect(EnteredPassword)); // kullanıcı var ise active directory'den şifresini doğrula

//                        //baglanti.Open();
//                        //SqlCommand komut = new SqlCommand(("Select * From Users where Username=@P1 AND Password=@P2"), baglanti);
//                        //komut.Parameters.AddWithValue("@P1", EnteredUsername);
//                        //komut.Parameters.AddWithValue("@P2", EnteredPassword);
//                        //SqlDataReader dr = komut.ExecuteReader();

//                        //if (dr.Read())
//                        //{
//                        //    Session["UserName"] = dr["Username"].ToString();
//                        //    Session["AuthorityId"] = dr["AuthorityId"].ToString();
//                        //}

//                        if (login == true)
//                        {
//                            baglanti.Open();
//                            SqlCommand komut = new SqlCommand(("Select * From Users where Username=@P1"), baglanti);
//                            komut.Parameters.AddWithValue("@P1", EnteredUsername);
//                            SqlDataReader dr = komut.ExecuteReader();
//                            Session["UserName"] = dr["Username"].ToString();
//                            Session["AuthorityId"] = dr["AuthorityId"].ToString();
//                        }
//                    }
//                    catch { }
//                    finally
//                    {
//                        baglanti.Close();
//                    }


//                    if (Session["UserName"] != null && Session["AuthorityId"] != null)
//                    {
//                        // Eğer AuthorityId bir tamsayıysa bu türde dönüşüm yapın
//                        if (int.TryParse(Session["AuthorityId"].ToString(), out int authorityId))
//                        {
//                            if (authorityId != 4)
//                            {
//                                // Sayfa yönlendirme
//                                Response.Redirect("~/View/ReportPage.aspx");
//                            }
//                            else
//                            {
//                                Response.Write("Giriş Yetkiniz Kaldırılmıştır. Lütfen Yöneticinizle Görüşünüz.");
//                            }
//                        }

//                    }
//                    else
//                    {
//                        Response.Write("<script>alert('Kullanıcı adı veya şifre yanlış!');</script>");
//                    }
//                }
//                else
//                {
//                    Response.Write("<script>alert('Girdiğiniz güvenlik kodu yanlış!');</script>");
//                }
//            }
//            else
//            {
//                Response.Write("<script>alert('Kullanıcı Adı, Şifre ve Captcha Girilmesi Zorunludur!');</script>");
//            }
//        }

//        protected void TextBox1_TextChanged(object sender, EventArgs e)
//        {

//        }
//        protected void TextBox2_TextChanged(object sender, EventArgs e)
//        {

//        }
//        protected void txtCaptcha_TextChanged(object sender, EventArgs e)
//        {

//        }
//    }
//}