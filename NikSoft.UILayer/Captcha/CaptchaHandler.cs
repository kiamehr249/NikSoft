using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Specialized;
//
//to use captcha handler
using NikSoft.UILayer.WebControl;

namespace Rayan.Handlers {
	public class CaptchaHandler : IHttpHandler {

		private const int MAX_IMAGE_WIDTH = 600;
		private const int MAX_IMAGE_HEIGHT = 600;


		#region IHttpHandler Members
		
		public bool IsReusable {
			get { return false; }
		}

		public void ProcessRequest(HttpContext context) {
			NameValueCollection queryString = context.Request.QueryString;
			string text = queryString[CaptchaControl.KEY];
			HttpResponse response = context.Response;
			Bitmap bmp = CaptchaControl.GenerateImage(text);
            //If bmp IsNot Nothing Then;
			if (null != bmp)
				bmp.Save(response.OutputStream, ImageFormat.Jpeg);
		}

		#endregion
	}
}