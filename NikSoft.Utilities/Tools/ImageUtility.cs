namespace NikSoft.Utilities
{
    public class ImageUtility
    {
        private System.Drawing.Image p_Image;

        public ImageUtility()
        {
        }

        public ImageUtility(string ImageFile)
        {
            ImageFile = ImageFile.StartsWith("~") == true ? ImageFile : "~/" + ImageFile;
            System.Web.HttpContext context;
            context = System.Web.HttpContext.Current;
            string iName = ImageFile.Substring(ImageFile.LastIndexOf("/"));
            string iPath = context.Server.MapPath(ImageFile.Substring(0, ImageFile.LastIndexOf("/")));
            try
            {
                p_Image = System.Drawing.Image.FromFile(iPath + iName);
            }
            catch
            {
            }
        }

        public ImageUtility(System.Drawing.Image ImageFile)
        {
            p_Image = ImageFile;
        }

        public int Height()
        {
            if (p_Image != null)
            {
                return p_Image.Height;
            }
            return 0;
        }

        public int width()
        {
            if (p_Image != null)
            {
                return p_Image.Width;
            }
            return 0;
        }
    }
}