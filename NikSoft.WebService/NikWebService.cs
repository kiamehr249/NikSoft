using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace NikSoft.WebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class NikWebService : System.Web.Services.WebService
    {
        private readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public NikWebService()
        {

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void VahidLicense()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var BitLicense = new
            {
                State = true
            };

            HttpContext.Current.Response.Write(Serializer.Serialize(BitLicense));
            HttpContext.Current.Response.End();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void BlogArchive()
        {
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");


            List<BlogModel> blogs = new List<BlogModel>();

            var blog1 = new BlogModel();
            blog1.Title = "۳ تفاوت ثبت شرکت سهامی عام و سهامی خاص";
            blog1.Image = "/files/FekrBartar/Blogs/b1.jpg";
            blog1.Link = "https://fekrbartar.com/3-%d8%aa%d9%81%d8%a7%d9%88%d8%aa-%d8%ab%d8%a8%d8%aa-%d8%b4%d8%b1%da%a9%d8%aa-%d8%b3%d9%87%d8%a7%d9%85%db%8c-%d8%b9%d8%a7%d9%85-%d9%88-%d8%b3%d9%87%d8%a7%d9%85%db%8c-%d8%ae%d8%a7%d8%b5/";
            blog1.Description = "در این مقاله مهم ترین تفاوتها بین ثبت شرکت سهامی خاص و ثبت شرکت سهامی عام را به طور خلاصه و مفید بیان نموده ایم تا بتوان با استفاده از مقایسه بین این دو نوع شرکت بهترین شرکت متناسب با فعالیت مورد نظ…";

            var blog2 = new BlogModel();
            blog2.Title = "۲۲ گام در انتخاب نام شرکت ";
            blog2.Image = "/files/FekrBartar/Blogs/b2.jpg";
            blog2.Link = "https://fekrbartar.com/22-%da%af%d8%a7%d9%85-%d8%af%d8%b1-%d8%a7%d9%86%d8%aa%d8%ae%d8%a7%d8%a8-%d9%86%d8%a7%d9%85-%d8%b4%d8%b1%da%a9%d8%aa/";
            blog2.Description = "نام شرکت یکی از مهم ترین موارد در هنگام ثبت شرکت می باشد ،به گونه ای که انتخاب یک نام مناسب و متناسب با حوزه فعالیت شرکت می تواند در جهت بهبود کسب و کار شرکت و هم چنین در شناساندن شرکت کمک بسیاری را…";

            var blog3 = new BlogModel();
            blog3.Title = "۲ نکته مهم در مورد ثبت شرکت سهامی خاص ";
            blog3.Image = "/files/FekrBartar/Blogs/b3.jpg";
            blog3.Link = "https://fekrbartar.com/2-%d9%86%da%a9%d8%aa%d9%87-%d9%85%d9%87%d9%85-%d8%af%d8%b1-%d9%85%d9%88%d8%b1%d8%af-%d8%ab%d8%a8%d8%aa-%d8%b4%d8%b1%da%a9%d8%aa-%d8%b3%d9%87%d8%a7%d9%85%db%8c-%d8%ae%d8%a7%d8%b5/";
            blog3.Description = "برای ثبت شرکت اگر چه مهم است تمامی مراحل و شرایط ثبت به طور کامل انجام گیرد اما توجه به مسائلی که بعد از ثبت ممکن است اتفاق بیفتد بسیار حائز اهمیت است، به همین دلیل در این مطلب سعی می شود به دو نکته…";

            var blog4 = new BlogModel();
            blog4.Title = "۱۰ نکته مهم در ثبت شرکت تعاونی ";
            blog4.Image = "/files/FekrBartar/Blogs/b4.jpg";
            blog4.Link = "https://fekrbartar.com/10-%d9%86%da%a9%d8%aa%d9%87-%d9%85%d9%87%d9%85-%d8%af%d8%b1-%d8%ab%d8%a8%d8%aa-%d8%b4%d8%b1%da%a9%d8%aa-%d8%aa%d8%b9%d8%a7%d9%88%d9%86%db%8c/";
            blog4.Description = "در این مقاله سعی شده نکاتی مهم در مورد ثبت شرکتهای تعاونی ارائه گردد که توجه به آنها بسیار مهم و ضروری است ،این نکات در ۱۰ بند به طور خلاصه و مفید تشریح گردیده که کاربران با مطالعه آن می توانند مسیر…";

            var blog5 = new BlogModel();
            blog5.Title = "مراحل ثبت برند ";
            blog5.Image = "/files/FekrBartar/Blogs/b5.jpg";
            blog5.Link = "https://fekrbartar.com/%d9%85%d8%b1%d8%a7%d8%ad%d9%84-%d8%ab%d8%a8%d8%aa-%d8%a8%d8%b1%d9%86%d8%af/";
            blog5.Description = "برای عرضه هرگونه کالا و خدمات می بایست از یک نام و یا علامت خاص استفاده کرد ، عبارت بهتر این نام و نشان کالا و خدمات شماست که به وجهه شما در بازار کسب و کار هویت می بخشد، به همین منظور برای قانونی ب…";

            var blog6 = new BlogModel();
            blog6.Title = "گواهی ارزش افزوده چیست؟ ";
            blog6.Image = "/files/FekrBartar/Blogs/b6.jpg";
            blog6.Link = "https://fekrbartar.com/%da%af%d9%88%d8%a7%d9%87%db%8c-%d8%a7%d8%b1%d8%b2%d8%b4-%d8%a7%d9%81%d8%b2%d9%88%d8%af%d9%87-%da%86%db%8c%d8%b3%d8%aa/";
            blog6.Description = "یکی از مسائل مهم در اجرای نظام مالیات بر ارزش افزوده گرفتن مجوز فعالیت در این نهاد مالیاتی است، یکی از این مجوزها گواهینامه ثبت نام در نظام مالیات بر ارزش افزوده می باشد .شاید عده زیادی از فروشندگان…";

            blogs.Add(blog1);
            blogs.Add(blog2);
            blogs.Add(blog3);
            blogs.Add(blog4);
            blogs.Add(blog5);
            blogs.Add(blog6);

            var data = new
            {
                Part = 1,
                DataSet = blogs
            };

            HttpContext.Current.Response.Write(Serializer.Serialize(data));
        }
    }
}
