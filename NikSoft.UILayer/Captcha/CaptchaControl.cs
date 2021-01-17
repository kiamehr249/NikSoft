using System;
using System.Web;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace NikSoft.UILayer.WebControl
{
    /// <summary>
    /// The CaptchaControl control provides a Captcha Challenge control
    /// </summary>
    [ToolboxData("<{0}:CaptchaControl Runat=\"server\" CaptchaHeight=\"100px\" CaptchaWidth=\"300px\" />")]
	public class CaptchaControl : System.Web.UI.WebControls.WebControl, IPostBackDataHandler {


		#region "By a2 from OLD Portal, 1394/07/15, going SIMA Conductor"

		private Button BtntoClick;

		public Button ButtonToClick {
			get { return BtntoClick; }
			set { BtntoClick = value; }
		}



		#endregion


		#region "Controls"

		private System.Web.UI.WebControls.Image _image;

		#endregion

		#region "Events"

		private ServerValidateEventHandler UserValidatedEvent;
		public event ServerValidateEventHandler UserValidated {
			add {
				UserValidatedEvent = (ServerValidateEventHandler)System.Delegate.Combine(UserValidatedEvent, value);
			}
			remove {
				UserValidatedEvent = (ServerValidateEventHandler)System.Delegate.Remove(UserValidatedEvent, value);
			}
		}

		#endregion

		#region "Private Constants"
		private const int EXPIRATION_DEFAULT = 120;
		private const int LENGTH_DEFAULT = 6;
		private const string RENDERURL_DEFAULT = "ImageChallenge.captcha.aspx";
		private const string CHARS_DEFAULT = "abcdefghijklmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
		#endregion

		#region "Friend Constants"
		internal const string KEY = "captcha";
		#endregion

		#region "Private Members"
		private bool _Authenticated;
		private Color _BackGroundColor;
		private string _BackGroundImage = "";
		private string _CaptchaChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
		private Unit _CaptchaHeight;
		private int _CaptchaLength = 6;
		private string _CaptchaText;
		private Unit _CaptchaWidth;
		private string _ErrorMessage;
		private Style _ErrorStyle = new Style();
		private int _Expiration = 120;
		private bool _IsValid = false;
		private string _RenderUrl = "ImageChallenge.captcha.aspx";
		private string _Text = "کد بالا را وارد کنید:";

		private static string[] _FontFamilies = new string[] { "Arial", "Comic Sans MS", "Courier New", "Georgia", "Lucida Console", "MS Sans Serif", "Stencil", "Tahoma", "Times New Roman", "Trebuchet MS", "Verdana" };
		private static Random _Rand = new Random();
		private static string _Separator = ":-:";

		private bool IsDesignMode {
			get {
				return HttpContext.Current == null;
			}
		}

		#endregion

		#region "Public Properties"

		/// <summary>
		/// Gets and sets the BackGroundColor
		/// </summary>
		[Category("Appearance"), Description("The Background Color to use for the Captcha Image.")]
		public Color BackGroundColor {
			get {
				return _BackGroundColor;
			}
			set {
				_BackGroundColor = value;
			}
		}

		/// <summary>
		/// Gets and sets the BackGround Image
		/// </summary>
		[Category("Appearance"), Description("A Background Image to use for the Captcha Image.")]
		public string BackGroundImage {
			get {
				return _BackGroundImage;
			}
			set {
				_BackGroundImage = value;
			}
		}

		/// <summary>
		/// Gets and sets the list of characters
		/// </summary>
		[Category("Behavior"), DefaultValue(CHARS_DEFAULT), Description("Characters used to render CAPTCHA text. A character will be picked randomly from the string.")]
		public string CaptchaChars {
			get {
				return _CaptchaChars;
			}
			set {
				_CaptchaChars = value;
			}
		}

		/// <summary>
		/// Gets and sets the height of the Captcha image
		/// </summary>
		[Category("Appearance"), Description("Height of Captcha Image.")]
		public Unit CaptchaHeight {
			get {
				return _CaptchaHeight;
			}
			set {
				_CaptchaHeight = value;
			}
		}

		/// <summary>
		/// Gets and sets the length of the Captcha string
		/// </summary>
		[Category("Behavior"), DefaultValue(LENGTH_DEFAULT), Description("Number of CaptchaChars used in the CAPTCHA text")]
		public int CaptchaLength {
			get {
				return _CaptchaLength;
			}
			set {
				_CaptchaLength = value;
			}
		}

		/// <summary>
		/// Gets and sets the width of the Captcha image
		/// </summary>
		[Category("Appearance"), Description("Width of Captcha Image.")]
		public Unit CaptchaWidth {
			get {
				return _CaptchaWidth;
			}
			set {
				_CaptchaWidth = value;
			}
		}

		/// <summary>
		/// Gets and sets whether the Viewstate is enabled
		/// </summary>
		[Browsable(false)]
		public override bool EnableViewState {
			get {
				return base.EnableViewState;
			}
			set {
				base.EnableViewState = value;
			}
		}

		/// <summary>
		/// Gets and sets the ErrorMessage to display if the control is invalid
		/// </summary>
		[Category("Behavior"), Description("The Error Message to display if invalid."), DefaultValue("")]
		public string ErrorMessage {
			get {
				return _ErrorMessage;
			}
			set {
				_ErrorMessage = value;
			}
		}

		/// <summary>
		/// Gets the Style to use for the ErrorMessage
		/// </summary>
		[Browsable(true), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter)), Description("Set the Style for the Error Message Control.")]
		public Style ErrorStyle {
			get {
				return _ErrorStyle;
			}
		}

		/// <summary>
		/// Gets and sets the Expiration time in seconds
		/// </summary>
		[Category("Behavior"), Description("The duration of time (seconds) a user has before the challenge expires."), DefaultValue(EXPIRATION_DEFAULT)]
		public int Expiration {
			get {
				return _Expiration;
			}
			set {
				_Expiration = value;
			}
		}

		/// <summary>
		/// Gets whether the control is valid
		/// </summary>
		[Category("Validation"), Description("Returns True if the user was CAPTCHA validated after a postback.")]
		public bool IsValid {
			get {
				return _IsValid;
			}
		}

		/// <summary>
		/// Gets and sets the Url to use to render the control
		/// </summary>
		[Category("Behavior"), Description("The URL used to render the image to the client."), DefaultValue(RENDERURL_DEFAULT)]
		public string RenderUrl {
			get {
				return _RenderUrl;
			}
			set {
				_RenderUrl = value;
			}
		}

		/// <summary>
		/// Gets and sets the Help Text to use
		/// </summary>
		[Category("Captcha"), DefaultValue("Enter the code shown above:"), Description("Instructional text displayed next to CAPTCHA image.")]
		public string Text {
			get {
				return _Text;
			}
			set {
				_Text = value;
			}
		}

		#endregion

		#region "Private Methods"

		/// <summary>
		/// Builds the url for the Handler
		/// </summary>
		private string GetUrl() {
			string url = ResolveUrl(RenderUrl);
			url += "?" + KEY + "=" + Encrypt(EncodeTicket(), DateTime.Now.AddSeconds(Expiration));

			//Append the Alias to the url so that it doesn't lose track of the alias it's currently on
			//by a2mn
			//PortalSettings _portalSettings = PortalController.GetCurrentPortalSettings;
			//url += "&alias=" + _portalSettings.PortalAlias.HTTPAlias();
			//url += "&alias=" + "Testa2mn";
			return url;
		}

		/// <summary>
		/// Encodes the querystring to pass to the Handler
		/// </summary>
		private string EncodeTicket() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(CaptchaWidth.Value.ToString());
			sb.Append(_Separator + CaptchaHeight.Value.ToString());
			sb.Append(_Separator + _CaptchaText);
			sb.Append(_Separator + BackGroundImage);

			return sb.ToString();

		}

		#endregion

		#region "Shared/Static Methods"

		/// <summary>
		/// Creates the Image
		/// </summary>
		/// <param name="width">The width of the image</param>
		/// <param name="height">The height of the image</param>
		private static Bitmap CreateImage(int width, int height) {

			Bitmap bmp = new Bitmap(width, height);
			Graphics g;
			Rectangle rect = new Rectangle(0, 0, width, height);
			RectangleF rectF = new RectangleF(0, 0, width, height);

			g = Graphics.FromImage(bmp);

			Brush b = new LinearGradientBrush(rect, Color.FromArgb(_Rand.Next(192), _Rand.Next(192), _Rand.Next(192)), Color.FromArgb(_Rand.Next(192), _Rand.Next(192), _Rand.Next(192)), System.Convert.ToSingle(_Rand.NextDouble()) * 360, false);
			g.FillRectangle(b, rectF);

			if (_Rand.Next(2) == 1) {
				DistortImage(bmp, _Rand.Next(1, 20));
			} else {
				DistortImage(bmp, -_Rand.Next(5, 15));
			}

			return bmp;

		}

		/// <summary>
		/// Creates the Text
		/// </summary>
		/// <param name="text">The text to display</param>
		/// <param name="width">The width of the image</param>
		/// <param name="height">The height of the image</param>
		private static GraphicsPath CreateText(string text, int width, int height, Graphics g) {

			GraphicsPath textPath = new GraphicsPath();
			FontFamily ff = GetFont();
			int emSize = System.Convert.ToInt32(width * 2 / text.Length);
			Font f = null;
			try {
				SizeF measured = new SizeF(0, 0);
				SizeF workingSize = new SizeF(width, height);
				while (emSize > 2) {
					f = new Font(ff, emSize);
					measured = g.MeasureString(text, f);
					if (!(measured.Width > workingSize.Width || measured.Height > workingSize.Height)) {
						break;
					}
					f.Dispose();
					emSize -= 2;
				}
				emSize += 8;
				f = new Font(ff, emSize);

				StringFormat fmt = new StringFormat();
				fmt.Alignment = StringAlignment.Center;
				fmt.LineAlignment = StringAlignment.Center;

				textPath.AddString(text, f.FontFamily, System.Convert.ToInt32(f.Style), f.Size, new RectangleF(0, 0, width, height), fmt);
				WarpText(textPath, new Rectangle(0, 0, width, height));
				//Matrix matrix = new Matrix(1.0f, 1.0f, 2.0f, 2.0f, 0.1f, 0.2f);
				//textPath.Transform(matrix);

			} catch (Exception) {
			} finally {
				f.Dispose();
			}

			return textPath;

		}

		/// <summary>
		/// Decrypts the CAPTCHA Text
		/// </summary>
		/// <param name="encryptedContent">The encrypted text</param>
		private static string Decrypt(string encryptedContent) {

			string decryptedText = string.Empty;
			try {
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encryptedContent);
				if (!ticket.Expired) {
					decryptedText = ticket.UserData;
				}
			} catch (ArgumentException) {

			}

			return decryptedText;

		}

		/// <summary>
		/// DistortImage distorts the captcha image
		/// </summary>
		/// <param name="b">The Image to distort</param>
		private static void DistortImage(Bitmap b, double distortion) {

			int width = b.Width;
			int height = b.Height;

			Bitmap copy = (Bitmap)b.Clone();

			for (int y = 0; y <= height - 1; y++) {
				for (int x = 0; x <= width - 1; x++) {
					int newX = System.Convert.ToInt32(x + (distortion * Math.Sin(Math.PI * y / 64.0)));
					int newY = System.Convert.ToInt32(y + (distortion * Math.Cos(Math.PI * x / 64.0)));
					if (newX < 0 || newX >= width) {
						newX = 0;
					}
					if (newY < 0 || newY >= height) {
						newY = 0;
					}
					b.SetPixel(x, y, copy.GetPixel(newX, newY));
				}
			}
		}

		/// <summary>
		/// Encrypts the CAPTCHA Text
		/// </summary>
		/// <param name="content">The text to encrypt</param>
		/// <param name="expiration">The time the ticket expires</param>
		private static string Encrypt(string content, DateTime expiration) {
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, HttpContext.Current.Request.UserHostAddress, DateTime.Now, expiration, false, content);
			return FormsAuthentication.Encrypt(ticket);

		}

		/// <summary>
		/// GenerateImage generates the Captch Image
		/// </summary>
		/// <param name="encryptedText">The Encrypted Text to display</param>
		internal static Bitmap GenerateImage(string encryptedText) {
			string encodedText = Decrypt(encryptedText);
			Bitmap bmp = null;
			string[] Settings = encodedText.Split(new string[] { _Separator }, StringSplitOptions.None);

			try {
				int width = int.Parse(Settings[0]);
				int height = int.Parse(Settings[1]);
				string text = Settings[2];
				string backgroundImage = Settings[3];

				Graphics g;
				Brush b = new SolidBrush(Color.LightGray);
				Brush b1 = new SolidBrush(Color.Black);

				if (backgroundImage == "") {
					bmp = CreateImage(width, height);
				} else {
					bmp = (Bitmap)Bitmap.FromFile(HttpContext.Current.Request.MapPath(backgroundImage));
				}
				g = Graphics.FromImage(bmp);

				//Create Text
				GraphicsPath textPath = CreateText(text, width, height, g);
				if (backgroundImage == "") {
					g.FillPath(b, textPath);
				} else {
					g.FillPath(b1, textPath);
				}

			} catch {
				//LogException(exc);
			}

			return bmp;

		}

		/// <summary>
		/// GetFont gets a random font to use for the Captcha Text
		/// </summary>
		private static FontFamily GetFont() {

			FontFamily _font = null;
			while (_font == null) {
				try {
					_font = new FontFamily(_FontFamilies[_Rand.Next(_FontFamilies.Length)]);
				} catch (Exception) {
					_font = null;
				}
			}
			return _font;

		}

		/// <summary>
		/// Generates a random point
		/// </summary>
		/// <param name="xmin">The minimum x value</param>
		/// <param name="xmax">The maximum x value</param>
		/// <param name="ymin">The minimum y value</param>
		/// <param name="ymax">The maximum y value</param>
		private static PointF RandomPoint(int xmin, int xmax, int ymin, int ymax) {
			return new PointF(_Rand.Next(xmin, xmax), _Rand.Next(ymin, ymax));
		}

		/// <summary>
		/// Warps the Text
		/// </summary>
		/// <param name="textPath">The Graphics Path for the text</param>
		/// <param name="rect">a rectangle which defines the image</param>
		private static void WarpText(GraphicsPath textPath, Rectangle rect) {

			int intWarpDivisor;
			RectangleF rectF = new RectangleF(0, 0, rect.Width, rect.Height);

			intWarpDivisor = _Rand.Next(4, 8);

			int intHrange = Convert.ToInt32(rect.Height / intWarpDivisor);
			int intWrange = Convert.ToInt32(rect.Width / intWarpDivisor);

			PointF p1 = RandomPoint(0, intWrange, 0, intHrange);
			PointF p2 = RandomPoint(rect.Width - (intWrange - Convert.ToInt32(p1.X)), rect.Width, 0, intHrange);
			PointF p3 = RandomPoint(0, intWrange, rect.Height - (intHrange - Convert.ToInt32(p1.Y)), rect.Height);
			PointF p4 = RandomPoint(rect.Width - (intWrange - Convert.ToInt32(p3.X)), rect.Width, rect.Height - (intHrange - Convert.ToInt32(p2.Y)), rect.Height);

			PointF[] points = new PointF[] { p1, p2, p3, p4 };
			Matrix m = new Matrix();
			m.Translate(0, 0);
			textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);

		}

		#endregion

		#region "Protected Methods"

		/// <summary>
		/// Creates the child controls
		/// </summary>
		protected override void CreateChildControls() {
			base.CreateChildControls();

			if (this.CaptchaWidth.IsEmpty || this.CaptchaWidth.Type != UnitType.Pixel || this.CaptchaHeight.IsEmpty || this.CaptchaHeight.Type != UnitType.Pixel) {
				throw (new InvalidOperationException("Must specify size of control in pixels."));
			}

			_image = new System.Web.UI.WebControls.Image();
			_image.BorderColor = this.BorderColor;
			_image.BorderStyle = this.BorderStyle;
			_image.BorderWidth = this.BorderWidth;
			_image.ToolTip = this.ToolTip;
			_image.EnableViewState = false;
			Controls.Add(_image);
		}

		/// <summary>
		/// Gets the next Captcha
		/// </summary>
		protected virtual string GetNextCaptcha() {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			Random _rand = new Random();
			int n;

			int intMaxLength = CaptchaChars.Length;

			for (n = 0; n <= CaptchaLength - 1; n++) {
				sb.Append(CaptchaChars.Substring(_rand.Next(intMaxLength), 1));
			}
			return sb.ToString();

		}

		/// <summary>
		/// Loads the previously saved Viewstate
		/// </summary>
		/// <param name="savedState">The saved state</param>
		protected override void LoadViewState(object savedState) {
			if (savedState != null) {
				// Load State from the array of objects that was saved at SaveViewState.
				object[] myState = (object[])savedState;

				//Load the ViewState of the Base Control
				//if (myState.Length > 0)
				if (myState[0] != null)
					base.LoadViewState(myState[0]);


				//Load the CAPTCHA Text from the ViewState
				//if (myState.Length > 0)
				if (myState[1] != null)
					_CaptchaText = myState[1].ToString();

			}
		}

		/// <summary>
		/// Runs just before the control is to be rendered
		/// </summary>
		protected override void OnPreRender(EventArgs e) {
			//Generate Random Challenge Text
			_CaptchaText = GetNextCaptcha();

			//Call Base Class method
			base.OnPreRender(e);
		}

		/// <summary>
		/// Render the  control
		/// </summary>
		/// <param name="writer">An Html Text Writer</param>
		protected override void Render(HtmlTextWriter writer) {

			ControlStyle.AddAttributesToRender(writer);

			//Render outer <div> Tag
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			//Render image <img> Tag
			writer.AddAttribute(HtmlTextWriterAttribute.Src, GetUrl());
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (ToolTip.Length > 0) {
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			//Render Help Text
			if (Text.Length > 0) {
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.Write(Text);
				writer.RenderEndTag();
			}

			//Render text box <input> Tag
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
			//writer.AddAttribute(HtmlTextWriterAttribute.Style, "width:" + Width.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, _CaptchaText.Length.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			//writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ID);
			if (AccessKey.Length > 0) {
				writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
			}
			if (!Enabled) {
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			//NOT GOD CODE
			if (BtntoClick != null) {
				writer.AddAttribute("onkeypress", "return clickButton(event,'" + BtntoClick.ClientID + "')");
			}
			//TextBox1.Attributes.Add("onkeypress", "return clickButton(event,'" + Button1.ClientID + "')");
			if (TabIndex > 0) {
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Value, "");
			writer.AddAttribute("lang", "en");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();

			//Render error message
			if (!IsValid && Page.IsPostBack) {
				ErrorStyle.AddAttributesToRender(writer);
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.Write(ErrorMessage);
				writer.RenderEndTag();
			}

			//Render </div>
			writer.RenderEndTag();

		}

		/// <summary>
		/// Save the controls Voewstate
		/// </summary>
		protected override object SaveViewState() {

			object baseState = base.SaveViewState();
			object[] allStates = new object[2];
			allStates[0] = baseState;
			allStates[1] = _CaptchaText;
			return allStates;

		}

		#endregion

		#region "Public Methods"

		/// <summary>
		/// Validates the posted back data
		/// </summary>
		/// <param name="userData">The user entered data</param>
		public bool Validate(string userData) {
			if (string.Compare(userData, this._CaptchaText, true) == 0) {
				_IsValid = true;
			} else {
				_IsValid = false;
			}
			if (UserValidatedEvent != null)
				UserValidatedEvent(this, new ServerValidateEventArgs(_CaptchaText, _IsValid));

			//By a2mn for test.
			return _IsValid;
		}

		#endregion

		#region "IPostBackDataHandler Methods"

		/// <summary>
		/// LoadPostData loads the Post Back Data and determines whether the value has change
		/// </summary>
		/// <param name="postDataKey">A key to the PostBack Data to load</param>
		/// <param name="postCollection">A name value collection of postback data</param>
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection) {

			string returnedValue = postCollection[postDataKey];
			Validate(returnedValue);

			//Generate Random Challenge Text
			_CaptchaText = GetNextCaptcha();

			return false;

		}

		/// <summary>
		/// RaisePostDataChangedEvent runs when the PostBackData has changed.
		/// </summary>
		public void RaisePostDataChangedEvent() {

		}

		#endregion

	}
}