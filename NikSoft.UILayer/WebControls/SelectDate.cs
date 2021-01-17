using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:selectdate runat=server></{0}:selectdate>")]
    public class SelectDate : Control
    {
        protected DropDownList daylist;
        protected DropDownList monthlist;
        protected DropDownList yearlist;

        override protected void OnInit(EventArgs e)
        {

            base.OnInit(e);

            daylist = new DropDownList();
            monthlist = new DropDownList();
            yearlist = new DropDownList();

            //daylist.Items.Insert(0, new ListItem("انتخاب كنيد", "0"));
            //monthlist.Items.Insert(0, new ListItem("انتخاب كنيد", "0"));
            //yearlist.Items.Insert(0, new ListItem("انتخاب كنيد", "0"));
            for (int i = 1; i < 32; i++)
            {
                daylist.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            for (int i = _startYear; i <= _endYear; i++)
            {
                yearlist.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            String[] PersianMonthNames = new String[] {"", "فروردین", "اردیبهشت", "خرداد", "تیر",
                        "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"};
            for (int i = 1; i < 13; i++)
            {
                monthlist.Items.Add(new ListItem(PersianMonthNames[i], i.ToString()));
            }

            if (_RenderAsTable)
            {
                Table t = new Table();
                t.CssClass = "adminlist";

                TableRow tr = new TableRow();
                TableCell tc = new TableCell();
                tc.Controls.Add(new LiteralControl("روز"));
                tc.Controls.Add(daylist);
                tc.Controls.Add(new LiteralControl("ماه"));
                tc.Controls.Add(monthlist);
                tc.Controls.Add(new LiteralControl("سال"));
                tc.Controls.Add(yearlist);

                tr.Controls.Add(tc);
                t.Controls.Add(tr);
                this.Controls.AddAt(0, t);
            }
            else
            {
                this.Controls.AddAt(0, new LiteralControl("روز"));
                this.Controls.AddAt(1, daylist);
                this.Controls.AddAt(2, new LiteralControl("ماه"));
                this.Controls.AddAt(3, monthlist);
                this.Controls.AddAt(4, new LiteralControl("سال"));
                this.Controls.AddAt(5, yearlist);
            }
            if (_allowNull)
            {
                ListItem li = new ListItem("انتخاب نماييد", "0");
                daylist.Items.Insert(0, li);
                monthlist.Items.Insert(0, li);
                yearlist.Items.Insert(0, li);
            }
        }

        private int _startYear = 1385;

        public int StartYear
        {
            get { return _startYear; }
            set { _startYear = value; }
        }


        private int _endYear = 1395;

        public int EndYear
        {
            get { return _endYear; }
            set { _endYear = value; }
        }




        private string _selectedDate;

        public string selectedDate
        {
            get
            {
                if (daylist.SelectedValue == "0" || yearlist.SelectedValue == "0" || monthlist.SelectedValue == "0")
                    return _selectedDate = "";
                string phyear = yearlist.SelectedItem.Value;
                string phmonth = monthlist.SelectedItem.Value;
                if (phmonth.Length == 1) phmonth = "0" + phmonth;
                string phday = daylist.SelectedItem.Value;
                if (phday.Length == 1) phday = "0" + phday;
                return _selectedDate = phyear + "/" + phmonth + "/" + phday;
            }
            set
            {
                _selectedDate = value;
                setDate(_selectedDate);
            }
        }


        public string selectedDate8Digit
        {
            get
            {
                if (daylist.SelectedValue == "0" || yearlist.SelectedValue == "0" || monthlist.SelectedValue == "0")
                    return _selectedDate = "";
                string phyear = yearlist.SelectedItem.Value;
                string phmonth = monthlist.SelectedItem.Value;
                if (phmonth.Length == 1) phmonth = "0" + phmonth;
                string phday = daylist.SelectedItem.Value;
                if (phday.Length == 1) phday = "0" + phday;
                return _selectedDate = phyear + phmonth + phday;
            }
        }


        private bool _allowNull = false;
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }



        private bool _RenderAsTable = true;
        public bool RenderAsTable
        {
            get { return _RenderAsTable; }
            set { _RenderAsTable = value; }
        }



        private void setDate(string value)
        {
            int py = 0, pm = 0, pd = 0;
            try
            {
                if (value != string.Empty && value.Length == 10)
                {
                    py = Convert.ToInt32(value.Substring(0, 4));
                    pm = Convert.ToInt32(value.Substring(5, 2));
                    pd = Convert.ToInt32(value.Substring(8, 2));
                    daylist.SelectedIndex = Convert.ToInt32(pd) - 1;
                    yearlist.SelectedIndex = Convert.ToInt32(py) - StartYear;
                    monthlist.SelectedIndex = Convert.ToInt32(pm) - 1;
                }
            }
            catch { }
        }

        public void disSelect()
        {
            daylist.SelectedIndex = -1;
            yearlist.SelectedIndex = -1;
            monthlist.SelectedIndex = -1;
            daylist.Items[0].Selected = false;
            yearlist.Items[0].Selected = false;
            monthlist.Items[0].Selected = false;
        }

        public void AddNull()
        {
            ListItem li = new ListItem("انتخاب نماييد", "0");
            daylist.Items.Insert(0, li);
            monthlist.Items.Insert(0, li);
            yearlist.Items.Insert(0, li);
        }
    }
}