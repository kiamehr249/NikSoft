using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NikSoft.ContentManager.WebService
{
    public class ListJResultQuery
    {
        public List<JsonResult> GetResults(SqlDataReader reader, bool IsDefault, string defaultTitle = "", int defaultValue = 0)
        {
            var results = new List<JsonResult>();

            if (IsDefault)
            {
                var DefaultItem = new JsonResult();
                DefaultItem.ID = defaultValue;
                DefaultItem.Title = defaultTitle;
                results.Add(DefaultItem);
            }

            while (reader.Read())
            {
                var item = new JsonResult();
                try
                {
                    item.ID = Convert.ToInt32(reader["ID"]);
                    item.Title = reader["Title"].ToString();
                }
                catch (Exception e)
                {
                    
                }
                finally
                {
                    results.Add(item);
                }
            }

            return results;
        }
    }
}
