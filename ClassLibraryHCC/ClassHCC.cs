using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3
{
    public static class Histo_cta_banco
    {



        public static void Acoplamiento()
        {
            int [] = {}

            string query_anexo_v = "Select * from anexo_v_pnr ";
            DataTable dtAnexoV = EjecutarSentencia(query_anexo_v, "timbradoestatal");

            string query_anexo_vi = "select * from anexo_vi_pnr where clave_plaza = '070707A01803010071781'";
            DataTable dtAnexoVi = EjecutarSentencia(query_anexo_vi, "timbradoestatal");

            string query_centro_trabajo = "select unidad_dist_cheque from centro_trabajo where (ent_fed=7 and ct_clasif = 'A' and ct_id = 'ZP' and ct_secuencial = 57  and ct_digito_ver = 'C')";
            DataTable dtCentroTrabajo = EjecutarSentencia(query_centro_trabajo, "timbradoestatal");

            string query_emp_plaza = "select niv_puesto, nivel_sueldo, * from emp_plaza where  cod_pago=7 and unidad=62 and subunidad=27 and cat_puesto=trim('  E0465') and horas=2 and cons_plaza=266 ";
            DataTable dtEmpPlaza = EjecutarSentencia(query_emp_plaza, "timbradoestatal");
            
            foreach (DataRow rowAnexoV in dtAnexoV.Rows)
            {

            }



        }

        private static List<string> DesacoplamientoCadenas(string cadena)
        {
            List<string> list = new List<string>();
            int posicion = 0;
            int[] longitudes = { };
            Dictionary<int, int[]> DiccionarioLongitudes = new Dictionary<int, int[]>()
            {
                {10, new int[]{ 2, 1, 2, 4, 1 }},
                {20, new int[]{ 2, 2, 2, 5, 3, 6 }},
                {21, new int[]{ 2, 2, 2, 6, 3, 6 }},
                {22, new int[]{ 2, 2, 2, 7, 3, 6 }},
            };

            int noSubcadenas = DiccionarioLongitudes[cadena.Length].Length;
            longitudes = DiccionarioLongitudes[cadena.Length];

            for (int i = 0; i <= noSubcadenas - 1; i++)
            {
                string subCadena = cadena.Substring(posicion, longitudes[i]);
                posicion += longitudes[i];
                list.Add(subCadena);
            }
            return list;
        }

        private static DataTable EjecutarSentencia(string sentencia, string baseD)
        {
            DataTable tabla = new DataTable();
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                Conexion conn = new Conexion();
                conn.Coneccion(baseD);
                conn.GetConnection().Open();
                //conn.getCommand().CommandTimeout = 6000;
                conn.Ejecutar(sentencia);
                SqlDataReader dr = conn.GetCommand().ExecuteReader();
                if (dr.HasRows)
                {
                    try
                    {
                        tabla.Load(dr);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetType().Name + ":" + ex.Message);
                    }
                    dr.Close();
                    conn.GetConnection().Close();
                    conn.GetCommand().Dispose();
                    return tabla;
                }
                return tabla;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {

                    errorMessages.Append("Index #" + i + "\n" +
                        "Message ES: " + ex.Errors[i].Message + "\n" +
                        "Error Number ES: " + ex.Errors[i].Number + "\n" +
                        "LineNumber ES: " + ex.Errors[i].LineNumber + "\n" +
                        "Source ES: " + ex.Errors[i].Source + "\n" +
                        "Procedure ES: " + ex.Errors[i].Procedure + "\n" +
                        "SENTENCIA EJECUTADA : " + sentencia + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return null;
            }
        }

    }

    class Conexion
    {
        private SqlConnection connection = new SqlConnection();
        private SqlCommand command = new SqlCommand();
        private readonly Dictionary<string, string> DiccionaryConnection = new Dictionary<string, string>()
            {
                {"consultalectura","data source=winsql;initial catalog=consultalectura;user id=udiaz;password=servicio2022!"},
                {"siapsep","data source=winsql;initial catalog=siapsep;user id=consultatimbrado;password=6A7F1A56-7252-4587-BBCD-236142240B50"},
                {"hsiapsep","data source=winsql;initial catalog=hsiapsep;user id=consultatimbrado;password=6A7F1A56-7252-4587-BBCD-236142240B50"},
                {"fup","data source=winsql;initial catalog=fup;user id=consultatimbrado;password=6A7F1A56-7252-4587-BBCD-236142240B50"},
                {"timbradoestatal","data source=winsql;initial catalog=TimbradoEstatal;user id=timbradoestatal;password=D312F891-ECB8-4A08-8A4B-DFB7D1ABCADE"}
            };

        public SqlConnection Coneccion(string baseD)
        {

            StringBuilder errorMessages = new StringBuilder();
            try
            {
                connection = new SqlConnection(DiccionaryConnection[baseD]);
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "Error Number: " + ex.Errors[i].Number + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
            }

            return connection;
        }
        public void Ejecutar(string sentencia)
        {
            command = new SqlCommand(sentencia, connection);
        }

        public SqlConnection GetConnection() { return connection; }

        public SqlCommand GetCommand() { return command; }

        public void ConnOpen()
        {
            StringBuilder errorMessages = new StringBuilder();
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "Error Number: " + ex.Errors[i].Number + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
            }

        }
        public void ConnClose()
        {
            connection.Close();
        }
    }
}
