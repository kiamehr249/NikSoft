using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using System;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class FormTemplate : WidgetUIContainer
    {
        public IFormModelService iFormModelServ { get; set; }
        public FormModel thisForm;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            int formId;
            if (!int.TryParse(ModuleParameters, out formId))
            {
                RedirectTo("/" + Level + "/rd_Forms");
                return;
            }

            thisForm = iFormModelServ.Find(x => x.ID == formId);
            if (thisForm == null)
            {
                RedirectTo("/" + Level + "/rd_Forms");
                return;
            }

            txtCode.Text = thisForm.Template;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            thisForm.Template = txtCode.Text;
            iFormModelServ.SaveChanges();
        }


    }
}