using System;
using System.Web;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer.WebControls
{
    public class TinyMCE : TextBox
    {
        private readonly string scriptKey = "tinymcejs";

        public TinyMCE()
        {
            this.TextMode = TextBoxMode.MultiLine;
        }

        public string Content
        {
            get
            {
                var temp = HttpUtility.HtmlDecode(base.Text.Trim());
                if (temp.IndexOf("<body>") < 0)
                {
                    return temp;
                }
                var text = temp.Substring(temp.IndexOf("<body>") + "<body>".Length);
                text = text.Substring(0, text.IndexOf("</body>"));
                return text.Trim();
            }
            set
            {
                base.Text = value;
            }
        }

        [Obsolete("Use Content Instead", true)]
        public new string Text
        {
            get;
            set;
        }

        private bool plugIn = true;
        public bool PlugIn
        {
            get { return plugIn; }
            set { plugIn = value; }
        }

        private bool loadFromCDN = false;
        public bool LoadFromCDN
        {
            get { return loadFromCDN; }
            set { loadFromCDN = value; }
        }

        private bool showEditor = true;
        public bool ShowEditor
        {
            get { return showEditor; }
            set { showEditor = value; }
        }

        private int height = 500;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private TinyButtonSize buttonSize = TinyButtonSize.Small;

        public TinyButtonSize ButtonSize
        {
            get { return buttonSize; }
            set { buttonSize = value; }
        }

        private TinyToolbarMode toolbarMode = TinyToolbarMode.Default;

        public TinyToolbarMode ToolbarMode
        {
            get { return toolbarMode; }
            set { toolbarMode = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Page.IsPostBack)
            {
                base.Text = this.Content;
            }
            base.OnUnload(e);
            AddJSFiles();
            if (this.Visible && this.showEditor)
            {
                RegisterInit();
            }
            base.OnPreRender(e);
        }

        private void RegisterInit()
        {
            string scriptBlock = string.Empty;
            scriptBlock = string.Format(@"$(function () {{
												tinymce.init({{
													selector:'#{0}',
													menubar: false,
													resize: false,
													style_formats_merge: true,
													style_formats: [
													//{{title: 'Bold text', inline: 'b'}},
													//{{title: 'Red text', inline: 'span', styles: {{color: '#ff0000'}}}},
													//{{title: 'Red header', block: 'h1', styles: {{color: '#ff0000'}}}},
													//{{title: 'Example 1', inline: 'span', classes: 'example1'}},
													//{{title: 'Example 2', inline: 'span', classes: 'example2'}},
													{{title: 'orange highlight', selector: 'p, div, span', classes: 'orange-highlight'}},
													{{title: 'side pink', selector: 'p, div, span', classes: 'side-pink'}},

													{{
														title: 'Image Left',
														selector: 'img',
														styles: {{
														'float': 'left',
														'margin': '0 10px 0 10px'
														}}
													}},
													{{
														title: 'Image Right',
														selector: 'img',
														styles: {{
														'float': 'right',
														'margin': '0 0 10px 10px'
														}}
													}}
													], 
													formats: {{
														bold: {{inline: 'b'}},  
														italic: {{inline: 'i'}}
													}},
													height : " + height + @",
													directionality : 'rtl',
													//valid_elements : 'b,u,i,font[color|size]',
													//valid_elements :'',
													valid_elements : '*[*]',
													//extended_valid_elements : 'iframe[src|frameborder|style|scrolling|class|width|height|name|align],i[class|style]',
													//invalid_elements : 'strong,em',
                                                    convert_urls: false,
													font_formats: 'B Yekan=BYekan;'+
															'Sans Serif=sans-serif;'+
															'Traditional Arabic=TraditionalArabic',
													{1}
													{2}
													{3}
													file_browser_callback: RoxyFileBrowser
												}});
											}});
									    function RoxyFileBrowser(field_name, url, type, win) {{
										  var roxyFileman = '/contents/FileManeager/fileman/index.html';
										  if (roxyFileman.indexOf('?') < 0) {{
											roxyFileman += '?type=' + type;   
										  }}
										  else {{
											roxyFileman += '&type=' + type;
										  }}
										  roxyFileman += '&input=' + field_name + '&value=' + document.getElementById(field_name).value;
										  tinyMCE.activeEditor.windowManager.open({{
											 file: roxyFileman,
											 title: 'Roxy File Manager',
											 width: 850, 
											 height: 650,
											 resizable: 'yes',
											 plugins: 'media',
											 inline: 'yes',
											 close_previous: 'no'  
										  }}, {{     window: win,     input: field_name    }});
										  return false; 
										}}", this.ClientID, ItemSize(), LoadPlugIn(), LoadToolbar());
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), this.ClientID + "tiny", scriptBlock, true);
        }

        private string LoadPlugIn()
        {
            if (plugIn)
            {
                return @"plugins: [
                'advlist autolink autosave link image lists charmap print preview hr anchor pagebreak spellchecker',
                'searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking',
                'table directionality emoticons template textcolor paste  textcolor'
				],";
            }
            else
            {
                return string.Empty;
            }
        }

        private string LoadToolbar()
        {
            switch (toolbarMode)
            {
                case TinyToolbarMode.Basic:
                    return @"toolbar1: 'newdocument | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | fontselect fontsizeselect | ltr rtl',";

                case TinyToolbarMode.Default:
                    return @"toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
						toolbar2: 'print preview media | forecolor backcolor emoticons | ltr rtl ',";

                case TinyToolbarMode.Full:
                    {
                        return @"toolbar1: 'newdocument  | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect',
							toolbar2: 'cut copy paste pastetext | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor image media code | inserttime preview | forecolor backcolor',
							toolbar3: 'table | hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | spellchecker | visualchars visualblocks nonbreaking template pagebreak restoredraft',";
                    }

                case TinyToolbarMode.Full2:
                    {
                        return @"toolbar1: 'newdocument  | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect',
							toolbar2: 'cut copy paste pastetext | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor code | inserttime preview | forecolor backcolor',
							toolbar3: 'table | hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | spellchecker | visualchars visualblocks nonbreaking template pagebreak restoredraft',";
                    }

                default:
                    {
                        return @"toolbar1: 'newdocument | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect | ltr rtl',";
                    }
            }
        }

        private string ItemSize()
        {
            if (ButtonSize == TinyButtonSize.Small)
            {
                return "toolbar_items_size: 'small',";
            }
            else
            {
                return string.Empty;
            }
        }

        private void AddJSFiles()
        {
            if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), scriptKey))
            {
                if (loadFromCDN)
                {
                    Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), scriptKey, ResolveClientUrl("//tinymce.cachefly.net/4.0/tinymce.min.js"));
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), scriptKey, ResolveClientUrl("~/contents/tinymce/tinymce.min.js"));
                }
            }
        }
    }
}
public enum TinyButtonSize
{
    Small,
    Normal
}

public enum TinyToolbarMode
{
    Basic,
    Default,
    Full,
    Full2
}