using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Utilities
{
    public class DBUtilities
    {
        protected static string StrConnection = WebConfigurationManager.ConnectionStrings["NikDBContext"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(StrConnection);
            return connection;
        }

        public DataTable ExecuteCommand(string command)
        {
            command = command.Replace('\x06A9', '\x0643');
            command = command.Replace('\x064a', '\x06cc');
            command = command.Replace('\x0649', '\x06cc');

            var table = new DataTable();
            var conn = GetConnection();
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var data = new SqlDataAdapter(command, conn);
            data.Fill(table);
            conn.Close();
            return table;
        }

        protected bool NonQuery(string sql)
        {
            sql = sql.Replace('\x06A9', '\x0643');
            sql = sql.Replace('\x064a', '\x06cc');
            sql = sql.Replace('\x0649', '\x06cc');
            var cmd = new SqlCommand(sql, GetConnection());
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(sql);
                System.Web.HttpContext.Current.Response.Write(sql);
                System.Web.HttpContext.Current.Response.Write(e.Message);
                return false;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return true;
        }

        public bool ExecNonQuery(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new Exception("NonQuery : Empty Query");
            return NonQuery(sql);
        }

        protected void NonQuery2(string sql, string sql2)
        {
            sql = sql.Replace('\x06A9', '\x0643');
            sql = sql.Replace('\x064a', '\x06cc');
            sql = sql.Replace('\x0649', '\x06cc');

            sql2 = sql2.Replace('\x06A9', '\x0643');
            sql2 = sql2.Replace('\x064a', '\x06cc');
            sql2 = sql2.Replace('\x0649', '\x06cc');
            object result = null;
            SqlTransaction trans = null;
            var cmd = new SqlCommand(sql, GetConnection());
            try
            {
                cmd.Connection.Open();
                trans = cmd.Connection.BeginTransaction();
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select @@IDENTITY AS NewID";
                result = cmd.ExecuteScalar();
                sql2 = sql2.Replace("ispgidentity", result.ToString());
                cmd.CommandText = sql2;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
                if (trans != null) trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null) trans.Commit();
                cmd.Connection.Close();
            }
        }

        protected void NonQuery2(string mainSQL, string[] otherSQLs)
        {
            mainSQL = mainSQL.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');

            for (int i = 0; i < otherSQLs.Length; i++)
            {
                otherSQLs[i] = otherSQLs[i].Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            }

            //object result = null;
            SqlTransaction trans = null;
            var conn = GetConnection();
            var cmd = new SqlCommand(mainSQL, conn);
            try
            {
                cmd.Connection.Open();
                trans = cmd.Connection.BeginTransaction();
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select @@IDENTITY AS NewID";
                var result = cmd.ExecuteScalar().ToString();

                for (int i = 0; i < otherSQLs.Length; i++)
                {
                    if (!string.IsNullOrEmpty(otherSQLs[i]))
                    {
                        otherSQLs[i] = otherSQLs[i].Replace("ispgidentity", result);
                        cmd.CommandText = otherSQLs[i];
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
                if (trans != null) trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null) trans.Commit();

                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                if (cmd.Connection != null)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
            }
        }

        protected void NonQuery3(string mainSQL, string secondSQL, string[] otherSQLs)
        {
            mainSQL = mainSQL.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            secondSQL = secondSQL.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            for (int i = 0; i < otherSQLs.Length; i++)
            {
                otherSQLs[i] = otherSQLs[i].Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            }
            var result = string.Empty;
            SqlTransaction trans = null;
            var conn = GetConnection();
            var cmd = new SqlCommand(mainSQL, conn);
            try
            {
                cmd.Connection.Open();
                trans = cmd.Connection.BeginTransaction();
                cmd.Transaction = trans;
                //First SQL
                cmd.ExecuteNonQuery();
                //GET Identity
                cmd.CommandText = "select @@IDENTITY AS NewID";
                result = cmd.ExecuteScalar().ToString();
                //SecondSQL
                secondSQL = secondSQL.Replace("ispgidentity", result);
                cmd.CommandText = secondSQL;
                cmd.ExecuteNonQuery();
                //Get Identity
                cmd.CommandText = "select @@IDENTITY AS NewID";
                result = cmd.ExecuteScalar().ToString();
                for (int i = 0; i < otherSQLs.Length; i++)
                {
                    if (!string.IsNullOrEmpty(otherSQLs[i]))
                    {
                        otherSQLs[i] = otherSQLs[i].Replace("ispgidentity", result);
                        cmd.CommandText = otherSQLs[i];
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
                if (trans != null) trans.Rollback();
                throw;
            }
            finally
            {
                trans.Commit();
                if (conn != null)
                {
                    conn.Dispose();
                    conn.Close();
                }
                if (cmd.Connection != null)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
            }
        }

        protected void NonQuery3(string mainSQL, string secondSQL, string[] otherSQLs, bool usesecondIDENTITY)
        {
            mainSQL = mainSQL.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            secondSQL = secondSQL.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            for (int i = 0; i < otherSQLs.Length; i++)
            {
                if (!string.IsNullOrEmpty(otherSQLs[i]))
                {
                    otherSQLs[i] = otherSQLs[i].Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
                }
            }
            var identityResult = "";
            SqlTransaction trans = null;
            var conn = GetConnection();
            var Cmd = new SqlCommand(mainSQL, conn);
            try
            {
                Cmd.Connection.Open();
                trans = Cmd.Connection.BeginTransaction();
                Cmd.Transaction = trans;
                //First SQL
                Cmd.ExecuteNonQuery();
                //GET Identity
                Cmd.CommandText = "select @@IDENTITY AS NewID";
                identityResult = Cmd.ExecuteScalar().ToString();
                //SecondSQL
                secondSQL = secondSQL.Replace("ispgidentity", identityResult);
                Cmd.CommandText = secondSQL;
                Cmd.ExecuteNonQuery();
                if (usesecondIDENTITY)
                {
                    //Get second Identity
                    Cmd.CommandText = "select @@IDENTITY AS NewID";
                    identityResult = Cmd.ExecuteScalar().ToString();
                }
                for (int i = 0; i < otherSQLs.Length; i++)
                {
                    if (!string.IsNullOrEmpty(otherSQLs[i]))
                    {
                        otherSQLs[i] = otherSQLs[i].Replace("ispgidentity", identityResult);
                        Cmd.CommandText = otherSQLs[i];
                        Cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
                if (trans != null) trans.Rollback();
                throw;
            }
            finally
            {
                if (trans != null) trans.Commit();
                if (conn != null)
                {
                    conn.Dispose();
                    conn.Close();
                }
                if (Cmd.Connection != null)
                {
                    Cmd.Connection.Close();
                    Cmd.Connection.Dispose();
                }
            }
        }

        protected void NonQueryinTransaction(string[] SQLs)
        {
            for (int i = 0; i < SQLs.Length; i++)
            {
                SQLs[i] = SQLs[i].Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            }
            if (SQLs.Length > 0)
            {
                SqlTransaction trans = null;
                SqlConnection conn = GetConnection();
                //SqlCommand Cmd = new SqlCommand(Mainsql, conn);
                var Cmd = new SqlCommand();
                Cmd.Connection = conn;
                try
                {
                    Cmd.Connection.Open();
                    trans = Cmd.Connection.BeginTransaction();
                    Cmd.Transaction = trans;
                    foreach (string t in SQLs)
                    {
                        Cmd.CommandText = t;
                        Cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    if (trans != null) trans.Rollback();
                    trans = null;
                    if (conn != null)
                    {
                        conn.Dispose();
                        conn.Close();
                    }
                    if (Cmd.Connection != null)
                    {
                        Cmd.Connection.Close();
                        Cmd.Connection.Dispose();
                    }
                    if (null != Cmd)
                        Cmd.Dispose();
                    throw new Exception("Nik Exception");
                }
                finally
                {
                    if (null != trans)
                        trans.Commit();
                    if (conn != null)
                    {
                        conn.Dispose();
                        conn.Close();
                    }
                    if (Cmd.Connection != null)
                    {
                        Cmd.Connection.Close();
                        Cmd.Connection.Dispose();
                    }
                    if (null != Cmd)
                        Cmd.Dispose();
                }
            }
        }

        public SqlDataReader ReturnQuery(string sql)
        {
            return Query(sql);
        }

        private SqlDataReader Query(string sql)
        {
            sql = sql.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            var cmd = new SqlCommand(sql, GetConnection());
            SqlDataReader dtr;
            try
            {
                cmd.Connection.Open();
                dtr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return dtr;
        }

        protected object ScalarQuery(string sql)
        {
            sql = sql.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            object result;
            var cmd = new SqlCommand(sql, GetConnection());
            try
            {
                cmd.Connection.Open();
                result = cmd.ExecuteScalar();
            }
            catch
            {
                result = "";
            }
            finally
            {
                cmd.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Returns the max of MaxColumn from table TableName
        /// </summary>
        protected string MaxColumn(string tableName, string columnName)
        {
            if (string.Empty == tableName || string.Empty == columnName) return "";
            string query = "Select Max(" + columnName + ") From " + tableName;
            return ScalarQuery(query).ToString();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ReturnScalarQuery(string sql)
        {
            sql = sql.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            object result;
            try
            {
                var cmd = new SqlCommand(sql, GetConnection());
                cmd.Connection.Open();
                result = cmd.ExecuteScalar();
                cmd.Connection.Close();
            }
            catch
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        ///Executes a query(typically insert) & returns the inserted row IDENTITY
        /// </summary>
        /// <param name="sql">Query to be ruuned</param>
        /// <returns>The ID of inserted row</returns>
        protected int NoneQueryID(string sql)
        {
            sql = sql.Replace('\x06A9', '\x0643').Replace('\x064a', '\x06cc').Replace('\x0649', '\x06cc');
            object result = "0";
            var cmd = new SqlCommand(sql, GetConnection());
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "select @@IDENTITY AS NewID";
                result = cmd.ExecuteScalar();
            }
            catch
            {
            }
            finally
            {
                cmd.Connection.Close();
            }
            return Convert.ToInt32(result);
        }

        public int NonQueryID(string sql)
        {
            return NoneQueryID(sql);
        }

        public bool FillCombo(ListControl ddl, string tname, bool fillDefault)
        {
            if (tname == String.Empty) return false;
            const string idColumn = "ID";
            const string textColumn = "Name";
            var table = "";
            // :) Beautiful Query (:
            // Returnd the name of table;
            var query = " Select TableName FROM TableName WHERE Name='" + tname + "' ";
            table = ScalarQuery(query).ToString();
            query = "Select " + idColumn + ", " + textColumn + " From " + table;
            return FillCombo(query, ddl, textColumn, idColumn, fillDefault);
        }

        public bool FillCombo(ListControl ddl, string tname, string combotext, bool fillDefault)
        {
            if (tname == String.Empty) return false;
            const string idColumn = "ID";
            var query = "Select TableName From TableName Where Name='" + tname + "'";
            var table = "";
            table = ScalarQuery(query).ToString();
            query = "Select ID, " + combotext + " From " + table;
            return FillCombo(query, ddl, combotext, idColumn, fillDefault);
        }

        public bool FillCombo(ListControl ddl, string tname, string combotext, string ComboValue, bool fillDefault)
        {
            if (tname == String.Empty) return false;
            const string IDColumn = "ID";
            string query = "Select TableName From TableName Where Name='" + tname + "'";
            string table = "";
            table = ScalarQuery(query).ToString();
            query = "Select " + ComboValue + ", " + combotext + " From " + table;
            return FillCombo(query, ddl, combotext, IDColumn, fillDefault);
        }

        //public bool FillCombo(DropDownList ddl, string tname, string combotext, string ComboValue, string Condition) {
        //    if (tname == String.Empty) return false;
        //    string IDColumn = "ID";
        //    string query = "Select TableName From TableName Where Name='" + tname + "'";
        //    string table = "";
        //    table = ScalarQuery(query).ToString();
        //    query = "Select " + ComboValue + ", " + combotext + " From " + table + " Where " + Condition;
        //    return FillCombo(query, ddl, combotext, IDColumn);
        //}
        //public bool FillCombo(string sql, DropDownList ddl, string combotext, string combovalue) {
        //    try {
        //        ddl.DataSource = Query(sql);
        //        ddl.DataTextField = combotext;
        //        ddl.DataValueField = combovalue;
        //    } catch (Exception e) {
        //        return false;
        //    } finally {
        //        ddl.DataBind();
        //    }
        //    return true;
        //}

        // Change by A2MN :D
        // Added Default Selection 1386/05/20
        public bool FillCombo(string sql, ListControl ddl, string combotext, string combovalue, bool addDefault)
        {
            try
            {
                ddl.DataSource = Query(sql);
                ddl.DataTextField = combotext;
                ddl.DataValueField = combovalue;
            }
            catch
            {
                return false;
            }
            finally
            {
                ddl.DataBind();
            }
            if (addDefault)
            {
                var li = new ListItem("انتخاب نمایید", "0");
                ddl.Items.Insert(0, li);
                ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        public bool FillCombo(string sql, ListControl ddl, string combotext1, string combotext2, string combovalue, bool addDefault)
        {
            try
            {
                ddl.DataSource = Query(sql);
                ddl.DataTextField = combotext1 + "" + "" + combotext2;
                ddl.DataValueField = combovalue;
            }
            catch
            {
                return false;
            }
            finally
            {
                ddl.DataBind();
            }
            if (addDefault)
            {
                var li = new ListItem("انتخاب نمایید", "0");
                ddl.Items.Insert(0, li);
                ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ddl"></param>
        /// <param name="combotext"></param>
        /// <param name="combovalue"></param>
        /// <param name="addDefault"></param>
        /// <param name="selectdefault"></param>
        /// <returns></returns>
        public bool FillCombo(string sql, ListControl ddl, string combotext, string combovalue, bool addDefault, bool selectdefault)
        {
            try
            {
                using (SqlDataReader sdr = Query(sql))
                {
                    ddl.DataSource = sdr;
                    ddl.DataTextField = combotext;
                    ddl.DataValueField = combovalue;
                    ddl.DataBind();
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
            if (addDefault)
            {
                var li = new ListItem("انتخاب نمایید", "0");
                ddl.Items.Insert(0, li);
                if (selectdefault)
                    ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        public bool FillCombo(string sql, ListControl ddl, string combotext, string combovalue, bool addDefault, bool selectdefault, string defaultText, string defaultValue)
        {
            try
            {
                using (SqlDataReader sdr = Query(sql))
                {
                    ddl.DataSource = sdr;
                    ddl.DataTextField = combotext;
                    ddl.DataValueField = combovalue;
                    ddl.DataBind();
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
            if (addDefault)
            {
                ListItem li;
                if (!string.IsNullOrEmpty(defaultText) && !string.IsNullOrEmpty(defaultValue))
                {
                    li = new ListItem(defaultText, defaultValue);
                }
                else
                {
                    li = new ListItem("انتخاب نمایید", "0");
                }

                ddl.Items.Insert(0, li);
                if (selectdefault)
                    ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        public bool FillCombo(DataTable dt, ListControl combo, string valueMember, string DisplayMember, bool addDefault)
        {
            try
            {
                combo.DataSource = dt;
                combo.DataTextField = DisplayMember;
                combo.DataValueField = valueMember;
                combo.DataBind();
                if (addDefault) combo.Items.Insert(0, new ListItem("انتخاب كنيد", "0"));
            }
            catch { return false; }
            return true;
        }

        public bool FillCheckBox(string sql, ListControl chl, string checkBoxText, string checkBoxValue)
        {
            try
            {
                using (SqlDataReader sdr = Query(sql))
                {
                    chl.DataSource = sdr;
                    chl.DataTextField = checkBoxText;
                    chl.DataValueField = checkBoxValue;
                    chl.DataBind();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool FillCheckBox(DataTable dt, ListControl chl, string checkBoxText, string checkBoxValue)
        {
            try
            {
                chl.DataSource = dt;
                chl.DataTextField = checkBoxText;
                chl.DataValueField = checkBoxValue;
            }
            catch
            {
                return false;
            }
            finally
            {
                chl.DataBind();
            }
            return true;
        }

        public bool FillRadioButtonList(string sql, ListControl chl, string RadioText, string RadioValue)
        {
            try
            {
                using (SqlDataReader sdr = Query(sql))
                {
                    chl.DataSource = sdr;
                    chl.DataTextField = RadioText;
                    chl.DataValueField = RadioValue;
                    chl.DataBind();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected int ReturnCount(string tableName, string anyCondition)
        {
            int result;
            var sql = "Select count(1) From " + tableName;
            if (!string.IsNullOrEmpty(anyCondition))
            {
                sql += " Where " + anyCondition;
            }
            var cmd = new SqlCommand(sql, GetConnection());
            var rs = false;
            try
            {
                cmd.Connection.Open();
                rs = int.TryParse(cmd.ExecuteScalar().ToString(), out result);
            }
            catch
            {
                result = 0;
            }
            finally
            {
                cmd.Connection.Close();
            }
            if (!rs)
                return 0;
            return result;
        }

        public object FindControl(UserControl uform, string controlName)
        {
            return uform.FindControl(controlName);
        }

        //Added @1387/09/ in last panjsanbeh of month:)
        public bool FillListControl(string sql, ListControl ddl, string combotext, string combovalue, bool addDefault)
        {
            try
            {
                ddl.DataSource = ExecuteCommand(sql);
                ddl.DataTextField = combotext;
                ddl.DataValueField = combovalue;
            }
            catch
            {
                return false;
            }
            finally
            {
                ddl.DataBind();
            }
            if (addDefault)
            {
                var li = new ListItem("انتخاب نمایید", "0");
                ddl.Items.Insert(0, li);
                ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ddl"></param>
        /// <param name="combotext"></param>
        /// <param name="combovalue"></param>
        /// <param name="addDefault"></param>
        /// <param name="selectdefault"></param>
        /// <returns></returns>
        public bool FillListControl(string sql, ListControl ddl, string combotext, string combovalue, bool addDefault, bool selectdefault)
        {
            try
            {
                ddl.DataSource = ExecuteCommand(sql);
                ddl.DataTextField = combotext;
                ddl.DataValueField = combovalue;
            }
            catch
            {
                return false;
            }
            finally
            {
                ddl.DataBind();
            }
            if (addDefault)
            {
                var li = new ListItem("انتخاب نمایید", "0");
                ddl.Items.Insert(0, li);
                if (selectdefault)
                    ddl.Items.FindByValue("0").Selected = true;
            }
            return true;
        }

        public bool FillListControl(DataTable dt, ListControl combo, string valueMember, string displayMember, bool addDefault)
        {
            try
            {
                combo.DataSource = dt;
                combo.DataTextField = displayMember;
                combo.DataValueField = valueMember;
                combo.DataBind();
                if (addDefault) combo.Items.Insert(0, new ListItem("انتخاب كنيد", "0"));
            }
            catch { return false; }
            return true;
        }

        public string InputEncode(string st)
        {
            string temp = st;
            string temp2 = st.ToLower();
            temp2 = temp2.Replace("'", "''  ");
            temp2 = temp2.Replace(";", "  ");
            temp2 = temp2.Replace("--", "  ");
            //st = st.Replace("go", " ");
            temp2 = temp2.Replace("select", " ");
            temp2 = temp2.Replace("update", " ");
            temp2 = temp2.Replace("insert", " ");
            temp2 = temp2.Replace("delete", " ");
            temp2 = temp2.Replace("drop", " ");
            temp2 = temp2.Replace("alter", " ");
            //temp2 = temp2.Replace("xp", " ");
            temp2 = temp2.Replace("dbo", " ");
            //st = st.Replace("sa", " ");
            temp2 = temp2.Replace("schema", " ");
            if (temp2 == st.ToLower())
            {
                return temp;
            }
            return temp2;
        }
    }
}