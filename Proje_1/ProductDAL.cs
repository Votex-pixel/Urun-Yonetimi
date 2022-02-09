using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje_1
{
   public class ProductDAL
    {
        SqlConnection connection = new SqlConnection(@"server=(LocalDB)\MSSQLLocalDB; initial catalog=Stored Procedure; integrated security=true");

        void ConnectionKontrol()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public List<Product> GetAll()
        {
            ConnectionKontrol();
            List<Product> products = new List<Product>();
            SqlCommand command = new SqlCommand("select * from Urunler", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UrunAdi = reader["UrunAdi"].ToString(),
                    StokMiktari = Convert.ToInt32(reader["StokMiktari"]),
                    UrunFiyati = Convert.ToDecimal(reader["UrunFiyati"])
                };
                products.Add(product);
            }
            reader.Close();
            command.Dispose();
            connection.Close();
            return products;
        }

        public DataTable GetAllDataTable(string sorgu = "select * from Urunler")
        {
            ConnectionKontrol();
            SqlCommand komut = new SqlCommand(sorgu, connection);
            SqlDataReader reader = komut.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            reader.Close();
            connection.Close();
            return dataTable;
        }

        public int Add(Product product)
        {
            ConnectionKontrol();
            SqlCommand command = new SqlCommand("Insert into Urunler values (@UrunAdi, @UrunFiyati, @StokMiktari)", connection);
            command.Parameters.AddWithValue("@UrunAdi", product.UrunAdi);
            command.Parameters.AddWithValue("@UrunFiyati", product.UrunFiyati);
            command.Parameters.AddWithValue("@StokMiktari", product.StokMiktari);
            var sonuc = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return sonuc;
        }

        public int Update(Product product)
        {
            ConnectionKontrol();
            SqlCommand command = new SqlCommand("Update Urunler set UrunAdi=@Adi, UrunFiyati=@UrunFiyati, StokMiktari=@StokMiktari where Id=@id", connection);
            command.Parameters.AddWithValue("@Adi", product.UrunAdi);
            command.Parameters.AddWithValue("@UrunFiyati", product.UrunFiyati);
            command.Parameters.AddWithValue("@StokMiktari", product.StokMiktari);
            command.Parameters.AddWithValue("@id", product.Id);
            var sonuc = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return sonuc;
        }

        public int Delete(int id)
        {
            ConnectionKontrol();
            SqlCommand command = new SqlCommand("Delete from Urunler where Id=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            var sonuc = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return sonuc;
        }
    }

}
