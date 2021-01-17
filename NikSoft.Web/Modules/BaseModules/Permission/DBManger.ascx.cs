using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Data;

namespace NikSoft.Web.Modules.BaseModules.Permission
{
    public partial class DBManger : WidgetUIContainer
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // test1();

            }
        }
        public void test2()
        {
            var dbi = new DBUtilities();
            string Sql = "select * from Portals where id !=1 ";
            DataTable dt1 = dbi.ExecuteCommand(Sql);

            Sql = "select * from PageTemplates where portalid=1";
            DataTable dt2 = dbi.ExecuteCommand(Sql);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {

                for (int j = 0; j < dt2.Rows.Count; j++)
                {

                    Sql = @"INSERT INTO [PageTemplates]
                           ([Title]
                           ,[Description]
                           ,[PortalID]
                           ,[TemplateType]
                           ,[TemplateName]
                           ,[Direction]
                           ,[Culture]
                           ,[UserMustLogin]
                           ,[IsSelected])
                     VALUES
                           ('" + dt2.Rows[j]["Title"].ToString() + "','" +
                          dt2.Rows[j]["Description"].ToString() + "'," +
                          dt1.Rows[i]["id"].ToString() + "," +
                          dt2.Rows[j]["TemplateType"].ToString() + ",'" +
                          dt2.Rows[j]["TemplateName"].ToString() + "','" +
                          dt2.Rows[j]["Direction"].ToString() + "','" +
                          dt2.Rows[j]["Culture"].ToString() + "',0,1)";

                    dbi.ExecNonQuery(Sql);
                }
            }
        }

        public void test1()
        {
            var dbi = new DBUtilities();
            string Sql = @"select * from Widgets  where PageTemplateID=685";
            DataTable dt = dbi.ExecuteCommand(Sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int ShowTitle = dt.Rows[i]["ShowTitle"].ToString() == "True" ? 1 : 0;
                int Expanded = dt.Rows[i]["Expanded"].ToString() == "True" ? 1 : 0;

                Sql = "select * from PageTemplates where id!=685 and TemplateType=0 ";
                DataTable dt2 = dbi.ExecuteCommand(Sql);
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Sql = @"INSERT INTO [Widgets]
                   ([PageTemplateID]
                   ,[WidgetDefinitionID]
                   ,[WidgetSkinPath]
                   ,[PanelNo]
                   ,[OrderNo]
                   ,[Expanded]
                   ,[Title]
                   ,[TitleLink]
                   ,[State]
                   ,[ShowTitle])
             VALUES
                   (" + dt2.Rows[j]["id"].ToString() + "," +
                     dt.Rows[i]["WidgetDefinitionID"].ToString() + "," +
                     "'" + dt.Rows[i]["WidgetSkinPath"].ToString() + "'," +
                     dt.Rows[i]["PanelNo"].ToString() + "," +
                     dt.Rows[i]["OrderNo"].ToString() + "," +
                     Expanded + "," +
                     "'" + dt.Rows[i]["Title"].ToString() + "'," +
                     "'" + dt.Rows[i]["TitleLink"].ToString() + "'," +
                     "'" + dt.Rows[i]["State"].ToString() + "'," +
                     ShowTitle + ")";

                    dbi.ExecNonQuery(Sql);
                }


            }


        }



        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                var dbi = new DBUtilities();
                dl.DataSource = dbi.ReturnQuery(txt.Text);
                dl.DataBind();
                messageBox.Visible = false;
            }
            catch (Exception ex)
            {
                messageBox.Visible = true;
                LtrErrors.Text = ex.Message;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (("nikdb" == txtUname.Text) && ("Nik@10104" == txtPass.Text))
            {
                plcSql.Visible = true;
                plcLoing.Visible = false;
            }
            else
            {
                Response.Redirect("~/panel/page/default");
            }
        }
    }
}