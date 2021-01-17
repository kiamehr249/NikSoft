using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Setting
{
    public partial class SystemSettingView : NikUserControl
    {

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }

        protected override void BoundData()
        {
            NikSettingType setting = GetSetting();
            var query = iNikSettingServ.ExpressionMaker();
            query.Add(x => x.UseEditor == true && x.SettingModule == setting);
            query.Add(t => t.PortalID == PortalUser.PortalID);
            base.FillManageFrom(iNikSettingServ, query);
            if (!IsPostBack)
            {
                pan.Visible = false;
            }
            if (setting == NikSettingType.SystemSetting)
            {
                HypCancel.NavigateUrl = "/" + Level + "/systemsettingview";
            }
            else
            {
                HypCancel.NavigateUrl = "/" + Level + "/NikMessageSetting";
            }
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int tempID = int.Parse(e.CommandArgument.ToString());
            var t = iNikSettingServ.Find(x => x.ID == tempID);
            divint.Visible = false;
            if (t != null)
            {
                hid.Value = tempID.ToString();
                pan.Visible = true;
                switch (t.FieldType)
                {
                    case NikSettingFeildType.Text:
                        {
                            if (t.ShowEditorInEdit)
                            {
                                TinyMCE1.Content = t.SettingValue;
                                txt_Value.Visible = false;
                                TinyMCE1.Visible = true;
                            }
                            else
                            {
                                txt_Value.Text = t.SettingValue;
                                txt_Value.Visible = true;
                                TinyMCE1.Visible = false;
                            }

                            panchk.Visible = false;
                            pantxt.Visible = true;
                            pnlRange.Visible = false;
                            pnlPhoto.Visible = false;
                            break;
                        }
                    case NikSettingFeildType.Number:
                        {
                            txt_Value.Text = t.SettingValue;
                            txt_Value.Visible = true;
                            TinyMCE1.Visible = false;
                            lbl_int_Title.InnerText = "مقدار " + t.SettingLabel;
                            panchk.Visible = false;
                            pantxt.Visible = true;
                            pnlRange.Visible = false;
                            lblMax.InnerText = t.MaxAllowed.ToString();
                            lblmin.InnerText = t.MinAllowed.ToString();
                            divint.Visible = true;
                            pnlPhoto.Visible = false;
                            break;
                        }
                    case NikSettingFeildType.Boolean:
                        {
                            chkValue.Checked = Convert.ToBoolean(t.SettingValue);
                            pantxt.Visible = false;
                            panchk.Visible = true;
                            pnlRange.Visible = false;
                            pnlPhoto.Visible = false;
                            TinyMCE1.Visible = false;
                            break;
                        }
                    case NikSettingFeildType.Range:
                        {
                            txtStart.Text = t.MinAllowed.ToString();
                            txtEnd.Text = t.MaxAllowed.ToString();
                            lbl_title.InnerText = "مقدار " + t.SettingLabel;
                            pantxt.Visible = false;
                            panchk.Visible = false;
                            pnlRange.Visible = true;
                            TinyMCE1.Visible = false;
                            pnlPhoto.Visible = false;
                            break;
                        }
                    case NikSettingFeildType.Photo:
                        {
                            pantxt.Visible = false;
                            panchk.Visible = false;
                            pnlRange.Visible = false;
                            pnlPhoto.Visible = true;
                            TinyMCE1.Visible = false;
                            imgPhoto.ImageUrl = "~/" + t.SettingValue;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                l1.InnerText = t.SettingLabel;
                l2.InnerText = t.SettingLabel;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            int itemID = int.Parse(hid.Value);
            var i = iNikSettingServ.Find(x => x.ID == itemID);

            switch (i.FieldType)
            {
                case NikSettingFeildType.Text:
                    {
                        if (i.ShowEditorInEdit)
                        {
                            i.SettingValue = TinyMCE1.Content;
                        }
                        else
                        {
                            i.SettingValue = txt_Value.Text;
                        }

                        break;
                    }
                case NikSettingFeildType.Number:
                    {
                        int minVal = i.MinAllowed;
                        int maxVal = i.MaxAllowed;
                        int resultVal = 0;
                        if (int.TryParse(txt_Value.Text, out resultVal)
                            && resultVal >= minVal
                            && resultVal <= maxVal)
                        {
                            i.SettingValue = resultVal.ToString();
                        }
                        else
                        {
                            Notification.SetErrorMessage("لطف‌‌ن مقدار عددی و در داخل محدوده وارد کنید");
                            return;
                        }
                        break;
                    }
                case NikSettingFeildType.Boolean:
                    {
                        i.SettingValue = (chkValue.Checked) ? "True" : "False";
                        break;
                    }
                case NikSettingFeildType.Range:
                    {
                        i.MinAllowed = Convert.ToInt32(txtStart.Text.Trim());
                        i.MaxAllowed = Convert.ToInt32(txtEnd.Text.Trim());
                        i.SettingValue = string.Empty;
                        break;
                    }
                case NikSettingFeildType.Photo:
                    {
                        if (fuPhoto.PostedFile.ContentLength > 0)
                        {
                            string filename = "";
                            var isok = Utilities.Utilities.UploadFile(fuPhoto.PostedFile, "SettingDir", ref filename, PortalUser.PortalFolderPath);
                            if (!isok)
                            {
                                Notification.SetErrorMessage("آپلود فایل انجام نشد");
                                return;
                            }
                            i.SettingValue = filename;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            uow.SaveChanges();
            CachingProvider.Remove("NikSetting" + i.SettingModule);
            CachingProvider.Remove("NikSetting" + i.SettingModule + i.PortalID);
            BoundData();
            pan.Visible = false;
            divint.Visible = false;
        }

        protected NikSettingType GetSetting()
        {
            string url = ModuleName;
            switch (url.ToLower())
            {
                case "nikmessagesetting":
                    {
                        return NikSettingType.MessagesSetting;
                    }
                default:
                    {
                        return NikSettingType.SystemSetting;
                    }
            }
        }



        protected string GetRealServerSeenValue(string setName, NikSettingType rst)
        {
            return iNikSettingServ.GetSettingValue(setName, rst, PortalUser.PortalID);
        }



        //


        protected string GetValue(NikSoft.NikModel.NikSetting setting)
        {
            switch (setting.FieldType)
            {
                case NikSettingFeildType.Text:
                case NikSettingFeildType.Number:
                    {
                        return setting.SettingValue;
                    }
                case NikSettingFeildType.Range:
                    {
                        return setting.MinAllowed + " - " + setting.MaxAllowed;
                    }
                case NikSettingFeildType.Boolean:
                    {
                        if (Convert.ToBoolean(setting.SettingValue))
                        {
                            return "فعال";
                        }
                        else
                        {
                            return "غیر فعال";
                        }
                    }
                case NikSettingFeildType.Photo:
                    {
                        return "<img src='../../" + setting.SettingValue + "' style='max-width:100px;max-height:100px' />";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        protected void btnRebuildCache_Click(object sender, EventArgs e)
        {
            iNikSettingServ.ClearCahce(PortalUser.PortalID);
            BoundData();
        }
    }
}