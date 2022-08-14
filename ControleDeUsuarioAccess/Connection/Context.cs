using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeUsuarioAccess.Connection
{
    public class Context<T> : ConnectionDataBase
    {
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public List<T> Listar()
        {
            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                try
                {
                    Type table = typeof(T);

                    OleDbCommand command = new OleDbCommand($"Select * from {table.Name}", conn);
                    conn.Open();

                    OleDbDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return ConvertDataTable<T>(dt);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void Inserir(T row, out string message)
        {
            message = "";

            using (OleDbConnection conn = new OleDbConnection(constr))
            {
                StringBuilder sql = new StringBuilder();

                try
                {
                    Type table = typeof(T);

                    sql.Append($"insert into {table.Name} (");
                    
                    PropertyInfo[] properties = table.GetProperties();
                    
                    for (int i = 0; i < properties.Count(); i++)
                    {
                        if (i < properties.Count() - 1)
                        {
                            sql.Append($"{properties[i].Name},");
                        }
                        else
                        {
                            sql.Append($"{properties[i].Name})");
                        }
                    }
                    
                    sql.Append(" values(");

                    for (int i = 0; i < properties.Count(); i++)
                    {
                        object value = properties[i].GetValue(row) ;

                        if (i < properties.Count() - 1)
                        {
                            sql.Append($"'{value.ToString()}',");
                        }
                        else
                        {
                            sql.Append($"'{value.ToString()}')");
                        }
                    }
                    
                    OleDbCommand command = conn.CreateCommand();
                    conn.Open();
                    command.CommandText = sql.ToString();
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                    message = "Item inserido com sucesso";

                }
                catch (Exception ex)
                {
                    message = $"Erro ao inserir: {ex.Message}";
                }
                finally
                {
                    conn.Close();
                }
                
            }
        }
    }
}
