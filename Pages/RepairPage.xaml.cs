using MySqlConnector;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Linq;
using Zoloto.Classes;

namespace Zoloto;

public partial class RepairPage : ContentPage
{
    bool con = true;
    string link;

    public RepairPage()
	{
		InitializeComponent();
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType == NetworkAccess.Internet)
        {
            con = true;
        }
        if (con == false)
        {
            Internet.IsVisible = true;
            name.IsVisible = false;
            editor.IsVisible = false;
        }
        else
        {
            Internet.IsVisible = false;
            name.IsVisible = true;
            editor.IsVisible = true;
        }
    }

    async void GetPhotoAsync(object sender, EventArgs e)
    {
        try
        {

            var photo = await MediaPicker.PickPhotoAsync();
            ima.Source = ImageSource.FromFile(photo.FullPath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(photo.FullPath);
            string fileString = Convert.ToBase64String(fileBytes);
            HttpClient client = new HttpClient();
            string secretkey = "Yqlk7Hlfpm3d63hazOR3enw4BGlroTmnyYE";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretkey);
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(photo.FullPath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(fileContent, "image", "image.jpg");

                HttpResponseMessage response = await client.PostAsync("https://api.imageban.ru/v1", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                using (JsonDocument document = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = document.RootElement;
                    JsonElement data = root.GetProperty("data");
                    link = data.GetProperty("link").GetString();

                    ima.Source = link;

                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
        }
    }

    private void Send_Clicked(object sender, EventArgs e)
    {
        int id = 0;
        Connection con = new Connection();
        if (name.Text != null && name.Text.Length >= 4 && phone.Text != null && phone.Text.Length < 15 && editor.Text != null)
        {
            string newconnectionString = con.Con;
            MySqlConnection conn = new MySqlConnection(newconnectionString);
            conn.Open();

            MySqlCommand cmd_all = new MySqlCommand("SELECT id FROM `Users` WHERE Phone = @phone && Name = @name", conn);
            cmd_all.Parameters.AddWithValue("@phone", phone.Text);
            cmd_all.Parameters.AddWithValue("@name", name.Text);
            MySqlDataReader MyDataReader_id;
            MyDataReader_id = cmd_all.ExecuteReader();
            if (MyDataReader_id.HasRows)
            {
                while (MyDataReader_id.Read())
                {
                    id = MyDataReader_id.GetInt32(0);
                    MyDataReader_id.Close();
                }
            }
            else
            {
                MyDataReader_id.Close();
                MySqlCommand cmd_insert = new MySqlCommand("INSERT INTO `Users` (Phone, Name) VALUES (@phone, @name)", conn);
                cmd_insert.Parameters.AddWithValue("@phone", phone.Text);
                cmd_insert.Parameters.AddWithValue("@name", name.Text);
                cmd_insert.ExecuteNonQuery();

                id = (int)cmd_insert.LastInsertedId;
            }



            MySqlCommand cmd_add = new MySqlCommand($"INSERT INTO `RepairOrders`(`example_image_url`, `problem`, `status`, `UserId`) VALUES ('{link}','{editor.Text}','{"new"}','{id}')", conn);
            cmd_add.ExecuteNonQuery();
            conn.Close();
            DisplayAlert("Успешно", "С вами свяжется наш оператор в ближайшее время", "Ок");
        }
        else
        {
            DisplayAlert("Ошибка", "Неверный ввод", "Ок");
        }
    }
}