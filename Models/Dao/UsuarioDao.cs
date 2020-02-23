using ProjectCRUD.Models.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectCRUD.Models.Dao
{
    public class UsuarioDao
    {
        private SqlConnection con;
        private void Connection()
        {
            String ConString = ConfigurationManager.ConnectionStrings["CONEXION"].ToString();
            con = new SqlConnection(ConString);
        }

        public bool Login(Usuario usuario)
        {
            String Query = "SELECT Usuario, Password FROM Usuarios WHERE Usuario =@Usuario AND Password = @Password";
            Connection();
            Usuario usuario1 = new Usuario();
            SqlCommand cmd =new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@Usuario", usuario.usuarioEntrar);
            cmd.Parameters.AddWithValue("@Password", usuario.password);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            if(dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool InsertUsuario(Usuario usuario)
        {
            SqlTransaction tx;

            Connection();
            con.Open();
            tx = con.BeginTransaction("SampleTransaction");

            try
            {
                string Query = "INSERT INTO Usuarios(Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, " +
                               " Telefono, Direccion, Identificacion, Barrio) VALUES(@Nombre, @PrimerApellido, @SegundoApellido, @FechaNacimiento," +
                               " @Telefono, @Direccion, @Identificacion, @Barrio)";
                SqlCommand cmd = new SqlCommand(Query, con, tx);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", usuario.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", usuario.SegundoApellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                cmd.Parameters.AddWithValue("@Direccion", usuario.Direccion);
                cmd.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
                cmd.Parameters.AddWithValue("@Barrio", usuario.Barrio);
                cmd.ExecuteNonQuery();
                tx.Commit();
                con.Close();
            }
            catch(Exception ex)
            {
                tx.Rollback();
                con.Dispose();
            }
            return true;
        }

        public List<Usuario> MostrarUsuarios()
        {
            List<Usuario> ListUsuario = new List<Usuario>();
            Connection();
            String Query = "SELECT id, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono" +
                           ", Direccion, Usuario, Password, Identificacion, (cast(datediff(dd,FechaNacimiento,GETDATE()) / 365.25 as int)) as Edad, Barrio" +
                           " FROM Usuarios WHERE Identificacion IS NOT NULL AND Nombre IS NOT NULL ORDER BY id";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                ListUsuario.Add(
                    new Usuario
                    {
                        id = (String.IsNullOrEmpty(dr["id"].ToString())) ? 0: Convert.ToInt32(dr["id"]),
                        Identificacion = (String.IsNullOrEmpty(dr["Identificacion"].ToString())) ? "" : Convert.ToString(dr["Identificacion"]),
                        Nombre = (String.IsNullOrEmpty(dr["Nombre"].ToString())) ? "" : Convert.ToString(dr["Nombre"]),
                        PrimerApellido = (String.IsNullOrEmpty(dr["PrimerApellido"].ToString())) ? "" : Convert.ToString(dr["PrimerApellido"]),
                        SegundoApellido = (String.IsNullOrEmpty(dr["SegundoApellido"].ToString())) ? "" : Convert.ToString(dr["SegundoApellido"]),
                        FechaNacimiento = (String.IsNullOrEmpty(dr["FechaNacimiento"].ToString())) ? DateTime.Now : Convert.ToDateTime(dr["FechaNacimiento"]),
                        Telefono = (String.IsNullOrWhiteSpace(dr["Telefono"].ToString())) ? "" : Convert.ToString(dr["Telefono"]),
                        Direccion = (String.IsNullOrEmpty(dr["Direccion"].ToString())) ? "" : Convert.ToString(dr["Direccion"]),
                        usuarioEntrar = (String.IsNullOrEmpty(dr["Usuario"].ToString())) ? "" : Convert.ToString(dr["Usuario"]),
                        password = (String.IsNullOrWhiteSpace(dr["Password"].ToString())) ? "" : Convert.ToString(dr["Password"]),
                        Barrio = (String.IsNullOrWhiteSpace(dr["Barrio"].ToString())) ? "" : Convert.ToString(dr["Barrio"]),
                        Edad = (String.IsNullOrWhiteSpace(dr["Edad"].ToString())) ? 0 : Convert.ToInt32(dr["Edad"])
                    }
                );
            }

            return ListUsuario;
        }

        public bool DeleteUsuario(int Id)
        {
            SqlTransaction tx;
            Connection();
            con.Open();
            tx = con.BeginTransaction("SampleTransaction");

            try
            {
                string Query = "DELETE FROM Usuarios WHERE id = @Id";
                SqlCommand cmd = new SqlCommand(Query, con, tx);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                tx.Commit();
                con.Close();
                return true;

            }catch(Exception ex)
            {
                tx.Rollback();
                con.Dispose();
                return false;
            }
        }

        public bool UpdateUsuario(Usuario usuario)
        {
            SqlTransaction tx;
            Connection();
            con.Open();
            tx = con.BeginTransaction("SampleTransaction");

            try
            {
                String Query = "UPDATE Usuarios SET Nombre = @Nombre, PrimerApellido = @PrimerApellido, SegundoApellido = @SegundoApellido, " +
                               "FechaNacimiento = @FechaNacimiento, Telefono = @Telefono, Direccion = @Direccion, " +
                               " Identificacion = @Identificacion WHERE id = @Id";
                SqlCommand cmd = new SqlCommand(Query, con, tx);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", usuario.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", usuario.SegundoApellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                cmd.Parameters.AddWithValue("@Direccion", usuario.Direccion);
                cmd.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
                cmd.Parameters.AddWithValue("@Id", usuario.id);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                tx.Commit();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                con.Dispose();
                return false;
            }
        }

        public Usuario MostrarUsuario(int Id)
        {
            String Query = "SELECT id, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, Telefono" +
                           ", Direccion, Usuario, Password, Identificacion, (cast(datediff(dd,FechaNacimiento,GETDATE()) / 365.25 as int)) as Edad, Barrio FROM Usuarios WHERE id = @Id";
            Connection();
            Usuario usuario1 = new Usuario();
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                usuario1.id = (String.IsNullOrEmpty(dr["id"].ToString())) ? 0 : Convert.ToInt32(dr["id"]);
                usuario1.Identificacion = (String.IsNullOrEmpty(dr["Identificacion"].ToString())) ? "" : Convert.ToString(dr["Identificacion"]);
                usuario1.Nombre = (String.IsNullOrEmpty(dr["Nombre"].ToString())) ? "" : Convert.ToString(dr["Nombre"]);
                usuario1.PrimerApellido = (String.IsNullOrEmpty(dr["PrimerApellido"].ToString())) ? "" : Convert.ToString(dr["PrimerApellido"]);
                usuario1.SegundoApellido = (String.IsNullOrEmpty(dr["SegundoApellido"].ToString())) ? "" : Convert.ToString(dr["SegundoApellido"]);
                usuario1.FechaNacimiento = (String.IsNullOrEmpty(dr["FechaNacimiento"].ToString())) ? DateTime.Now : Convert.ToDateTime(dr["FechaNacimiento"]);
                usuario1.Telefono = (String.IsNullOrWhiteSpace(dr["Telefono"].ToString())) ? "" : Convert.ToString(dr["Telefono"]);
                usuario1.Direccion = (String.IsNullOrEmpty(dr["Direccion"].ToString())) ? "" : Convert.ToString(dr["Direccion"]);
                usuario1.usuarioEntrar = (String.IsNullOrEmpty(dr["Usuario"].ToString())) ? "" : Convert.ToString(dr["Usuario"]);
                usuario1.password = (String.IsNullOrWhiteSpace(dr["Password"].ToString())) ? "" : Convert.ToString(dr["Password"]);
                usuario1.Edad = (String.IsNullOrWhiteSpace(dr["Edad"].ToString())) ? 0 : Convert.ToInt32(dr["Edad"]);
                usuario1.Barrio = (String.IsNullOrWhiteSpace(dr["Barrio"].ToString())) ? "" : Convert.ToString(dr["Barrio"]);
            }

            return usuario1;
        }

        public bool InsertUser(Usuario usuario)
        {
            SqlTransaction tx;

            Connection();
            con.Open();
            tx = con.BeginTransaction("SampleTransaction");

            try
            {
                string Query = "INSERT INTO Usuarios(Usuario, Password) VALUES(@Usuario, @Password)";
                SqlCommand cmd = new SqlCommand(Query, con, tx);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Usuario", usuario.usuarioEntrar);
                cmd.Parameters.AddWithValue("@Password", usuario.password);
                cmd.ExecuteNonQuery();
                tx.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                con.Dispose();
            }
            return true;
        }

    }
}