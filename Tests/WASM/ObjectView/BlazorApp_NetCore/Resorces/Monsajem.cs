

using WebAssembly.Browser.DOM;
using System;
using System.Linq;
namespace Monsajem_Incs.Resources
{



	public class BasePage_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head>
    <link href=""./Files/bootstrap.min.css"" rel=""stylesheet"">
    <link href=""./Files/animate.min.css"" rel=""stylesheet"">
    <link rel=""stylesheet"" href=""./Files/materialize.min.css"">
    <link rel=""stylesheet"" href=""./Files/w3.css"">
    <link rel=""stylesheet"" href=""Mprdaa_data/normalize.css"">
    <meta charset=""utf-8"" lang=""en"">
    <meta name=""viewport"" content=""width=device-width , initial-scale=1"">
</head><body>
    <script mnsrc=""Monsajem_Incs.Resources.BasePageNeeds.jquery_3_2_1_slim_min_js""></script>
    <script mnsrc=""Monsajem_Incs.Resources.BasePageNeeds.bootstrap_notify_min_js""></script>
    <script mnsrc=""Monsajem_Incs.Resources.BasePageNeeds.materialize_min_js""></script>

    <img src=""./Files/BackGround.jpg"" id=""background"" style=""top:0;left:0; position:fixed;width:100%;height:calc(100vh); z-index:-1;"">


    <div style=""width:100%;height:100%;background-color:rgba(0,0,0,0);"" id=""Hole"">
        <!-- Start your project here-->
        <style>
            input {
                width: 100%;
                height: 8%;
            }

            .mn-btn {
                margin-top: 4%;
                margin-left: auto;
                margin-right: auto;
                background-color: rgba(255,255,255, 0.7);
                transition-duration: 0.2s;
                width: 90%;
                height: calc(10vh);
                box-shadow: 0 4px 8px 0 rgba(182, 255, 0, 0.52),0 6px 20px 0 rgba(182, 255, 0, 0.70);
            }

                .mn-btn:active {
                    width: 75%;
                    height: calc(7vh);
                    background-color: rgba(229, 255, 0, 0.83);
                    box-shadow: 0 4px 8px 0 rgba(252, 255, 0, 0.70),0 6px 20px 0 rgba(229, 255, 0, 0.83);
                }

            .mn-obj {
                background-color: rgba(255,255,255,0.7);
                margin-left: 2.5%;
                width: 95%;
            }

                .mn-obj:active {
                    box-shadow: 0 4px 8px 0 rgba(252, 255, 0, 0.70),0 6px 20px 0 rgba(229, 255, 0, 0.83);
                }

            .mn-btn-basket {
                transition-duration: 0.2s;
            }

                .mn-btn-basket:active {
                    background-color: rgb(134, 187, 0);
                }

            .ViewObject {
                background-color: rgba(255,255,255, 0.7);
                transition-duration: 0.5s;
            }

                .ViewObject:hover {
                    background-color: rgba(19, 211, 246, 0.61);
                }

            .overlay {
                border-radius: 100%;
                height: 0%;
                width: 0%;
                position: fixed;
                z-index: 3;
                top: 0;
                right: 0;
                background-color: rgba(0,0,0, 0.9);
                overflow-y: scroll;
                transition: 0.3s;
            }

            .overlay-content {
                position: relative;
                top: 15%;
                width: 100%;
                text-align: center;
                margin-top: 30px;
            }

            .overlay-closebtn {
                color: aliceblue;
                position: absolute;
                top: 4%;
                right: 10%;
                font-size: 40px;
            }
            ::-webkit-input-placeholder { /* Edge */
                color: dimgray;
            }

            :-ms-input-placeholder { /* Internet Explorer 10-11 */
                color: dimgray;
            }

            ::placeholder {
                color: dimgray;
            }
        </style>

        <style>
            body {
                text-align: center;
            }
        </style>

        <div id=""P_Page"" style=""background-color:rgba(0,0,0,0);z-index:1;position:relative;display:block;width:100%;margin-top:40px"">

        </div>
            <div id=""P_Action"" style=""z-index:10;opacity:0;display:none;position:fixed;top:0;left:0;width:100%;height:100%"">
                <div class=""w3-display-container"" style=""background-color:rgba(255,255,255, 0.72) ;opacity:inherit;margin:0;z-index:1;position:fixed;width:100%;height:100%;"">
                    <img src=""./Files/loading.gif"" style=""opacity:0.5;top:0;left:0; position:absolute;width:100%;height:calc(100vh);"">
                </div>
                <div id=""P_Action_Content"" style=""position:relative;margin-left:auto;margin-right:auto;top:50%;background-color:rgba(0,0,0,0); z-index:2"">
                </div>
            </div>
        </div>



</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLImageElement background;
		public readonly HTMLDivElement Hole;
		public readonly HTMLDivElement P_Page;
		public readonly HTMLDivElement P_Action;
		public readonly HTMLDivElement P_Action_Content;
		public BasePage_html():this(false){}
		public BasePage_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				background= Document.GetElementById<HTMLImageElement>("background");
				Hole= Document.GetElementById<HTMLDivElement>("Hole");
				P_Page= Document.GetElementById<HTMLDivElement>("P_Page");
				P_Action= Document.GetElementById<HTMLDivElement>("P_Action");
				P_Action_Content= Document.GetElementById<HTMLDivElement>("P_Action_Content");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			background= doc.GetElementById<HTMLImageElement>("background");
			Hole= doc.GetElementById<HTMLDivElement>("Hole");
			P_Page= doc.GetElementById<HTMLDivElement>("P_Page");
			P_Action= doc.GetElementById<HTMLDivElement>("P_Action");
			P_Action_Content= doc.GetElementById<HTMLDivElement>("P_Action_Content");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			background.Id="";
			Hole.Id="";
			P_Page.Id="";
			P_Action.Id="";
			P_Action_Content.Id="";
		}
	
}
	
namespace BasePageNeeds
{
	public class bootstrap_notify_min_js
	{
		public static readonly string TextContent = ((Func<string>)(() =>
		{
			byte[] Result = null;
			{
				var ZipMemmory = new System.IO.MemoryStream(System.Convert.FromBase64String("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"));
				var DezippedMemory = new System.IO.MemoryStream();
				using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
				{
				Dezip.CopyTo(DezippedMemory);
				}
				Result = DezippedMemory.ToArray();
			}

			return System.Text.Encoding.UTF8.GetString(Result);
		}))();

	
	}
	public class jquery_3_2_1_slim_min_js
	{
		public static readonly string TextContent = ((Func<string>)(() =>
		{
			byte[] Result = null;
			{
				var ZipMemmory = new System.IO.MemoryStream(System.Convert.FromBase64String("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"));
				var DezippedMemory = new System.IO.MemoryStream();
				using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
				{
				Dezip.CopyTo(DezippedMemory);
				}
				Result = DezippedMemory.ToArray();
			}

			return System.Text.Encoding.UTF8.GetString(Result);
		}))();

	
	}
	public class materialize_min_js
	{
		public static readonly string TextContent = ((Func<string>)(() =>
		{
			byte[] Result = null;
			{
				var ZipMemmory = new System.IO.MemoryStream(System.Convert.FromBase64String("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"));
				var DezippedMemory = new System.IO.MemoryStream();
				using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
				{
				Dezip.CopyTo(DezippedMemory);
				}
				Result = DezippedMemory.ToArray();
			}

			return System.Text.Encoding.UTF8.GetString(Result);
		}))();

	
	}
	
}
	
namespace Database
{
	public class GroupEdit_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div id=""Main"">
    <br>
    <input id=""Name"" type=""text"" placeholder=""نام گروه را وارد کنید"">
    <br>
    <button id=""Done"">تایید</button>
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLInputElement Name;
		public readonly HTMLButtonElement Done;
		public GroupEdit_html():this(false){}
		public GroupEdit_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Name= Document.GetElementById<HTMLInputElement>("Name");
				Done= Document.GetElementById<HTMLButtonElement>("Done");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Name= doc.GetElementById<HTMLInputElement>("Name");
			Done= doc.GetElementById<HTMLButtonElement>("Done");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Name.Id="";
			Done.Id="";
		}
	
	}
	public class GroupView_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div dir=""rtl"" id=""Main"">
    <label>نام گروه:</label> <label id=""Name""></label>
    <label>تعداد دانش آموزان:</label> <label id=""StuCount""></label>
    <br>
    <input id=""Edit"" type=""button"" value=""edit"">
    <input id=""Delete"" type=""button"" value=""Delete"">
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLLabelElement Name;
		public readonly HTMLLabelElement StuCount;
		public readonly HTMLInputElement Edit;
		public readonly HTMLInputElement Delete;
		public GroupView_html():this(false){}
		public GroupView_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Name= Document.GetElementById<HTMLLabelElement>("Name");
				StuCount= Document.GetElementById<HTMLLabelElement>("StuCount");
				Edit= Document.GetElementById<HTMLInputElement>("Edit");
				Delete= Document.GetElementById<HTMLInputElement>("Delete");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Name= doc.GetElementById<HTMLLabelElement>("Name");
			StuCount= doc.GetElementById<HTMLLabelElement>("StuCount");
			Edit= doc.GetElementById<HTMLInputElement>("Edit");
			Delete= doc.GetElementById<HTMLInputElement>("Delete");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Name.Id="";
			StuCount.Id="";
			Edit.Id="";
			Delete.Id="";
		}
	
	}
	
}
	
namespace Partials
{
	public class Button_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>

    <div id=""btn_view"" class=""w3-round-large mn-btn"">
        <label id=""txt"" style=""margin-top:auto;margin-bottom:auto;color:black;""></label>
        <img id=""icon"" style=""max-width:30%;max-height:95%;"">
    </div>




</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement btn_view;
		public readonly HTMLLabelElement txt;
		public readonly HTMLImageElement icon;
		public Button_html():this(false){}
		public Button_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				btn_view= Document.GetElementById<HTMLDivElement>("btn_view");
				txt= Document.GetElementById<HTMLLabelElement>("txt");
				icon= Document.GetElementById<HTMLImageElement>("icon");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			btn_view= doc.GetElementById<HTMLDivElement>("btn_view");
			txt= doc.GetElementById<HTMLLabelElement>("txt");
			icon= doc.GetElementById<HTMLImageElement>("icon");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			btn_view.Id="";
			txt.Id="";
			icon.Id="";
		}
	
	}
	public class ConnetionLost_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div id=""main"">
    <br>
    <br>
    <b style=""color:darkred""> اتصال اینترنت بر قرار نیست!</b>
    <br>
    <br>
    <input type=""button"" value=""تلاش مجدد"" id=""btn_retry"">
    <br>
    <input type=""button"" value=""ورود در حالت بدون اتصال"" id=""btn_TryOffline"">
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement main;
		public readonly HTMLInputElement btn_retry;
		public readonly HTMLInputElement btn_TryOffline;
		public ConnetionLost_html():this(false){}
		public ConnetionLost_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				main= Document.GetElementById<HTMLDivElement>("main");
				btn_retry= Document.GetElementById<HTMLInputElement>("btn_retry");
				btn_TryOffline= Document.GetElementById<HTMLInputElement>("btn_TryOffline");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			main= doc.GetElementById<HTMLDivElement>("main");
			btn_retry= doc.GetElementById<HTMLInputElement>("btn_retry");
			btn_TryOffline= doc.GetElementById<HTMLInputElement>("btn_TryOffline");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			main.Id="";
			btn_retry.Id="";
			btn_TryOffline.Id="";
		}
	
	}
	public class Content_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>


<div id=""myModal"" class=""mdl"" style=""width:100%;height:100%"">
    <style>
        /* The Modal (background) */
        .mdl {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 3; /* Sit on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .mdl-content {
            background-color: #fefefe;
            padding: 0;
            border: 1px solid #888;
            width: 100%;
            min-height:100%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                width: 10%;
                opacity: 0
            }

            to {
                width: 100%;
                opacity: 1
            }
        }

        @keyframes animatetop {
            from {
                width: 10%;
                opacity: 0
            }

            to {
                width: 100%;
                opacity: 1
            }
        }

        /* The Close Button */
        .mdlclose {
            color: darkred;
            float: right;
            font-size: 35px;
            font-weight: bold;
            z-index: 4;
            margin-right:10%;
        }

            .mdlclose:hover,
            .mdlclose:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .mdl-header {
            padding: 2px;
            background-color: rgba(0, 148, 255,0.7);
            color: white;
            min-height:45px;
        }

        .mdl-body {
            background-color:rgba(0,0,0,0.0);
        }
    </style>

  <!-- Modal content -->
  <div class=""mdl-content"">
    <div class=""mdl-header"">
      <span id=""Btn_Close"" class=""mdlclose"">×</span>
      <div id=""head""> </div>
    </div>
    <div id=""body"" class=""mdl-body"">
    </div>
  </div>
</div>

<script>
// Get the modal
var modal = document.getElementById('myModal');

//// Get the <span> element that closes the modal
var span = document.getElementsByClassName(""close"")[0];

// When the user clicks on <span> (x), close the modal
//span.onclick = function() {
//  modal.style.display = ""none"";
//}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        span.onclick(); 
    }
}
</script>



</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement myModal;
		public readonly HTMLElement Btn_Close;
		public readonly HTMLDivElement head;
		public readonly HTMLDivElement body;
		public Content_html():this(false){}
		public Content_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				myModal= Document.GetElementById<HTMLDivElement>("myModal");
				Btn_Close= Document.GetElementById<HTMLElement>("Btn_Close");
				head= Document.GetElementById<HTMLDivElement>("head");
				body= Document.GetElementById<HTMLDivElement>("body");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			myModal= doc.GetElementById<HTMLDivElement>("myModal");
			Btn_Close= doc.GetElementById<HTMLElement>("Btn_Close");
			head= doc.GetElementById<HTMLDivElement>("head");
			body= doc.GetElementById<HTMLDivElement>("body");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			myModal.Id="";
			Btn_Close.Id="";
			head.Id="";
			body.Id="";
		}
	
	}
	public class GetMessage_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>
    <div id=""Main"" class=""card w3-round-large"" style=""background-color:rgba(255,255,255,0.7);margin-left:2.5%;width:95%;"">
        <div id=""Body""></div>
        <img class=""card-img-top w3-round-large"" style=""margin-top:3%;width:70%"" id=""Image"">
        <br>
        <label id=""txt_Prompt""></label>
        <br>
        <div class=""card-body"">
            <div class=""input-field"">
                <textarea class=""form-control"" id=""txt_message""></textarea>
                <label id=""txt_placeHolder"" for=""txt_message"">پیام شما</label>
            </div>
            <input type=""button"" id=""btn_send"" value=""ارسال"">
        </div>
    </div>

</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLDivElement Body;
		public readonly HTMLImageElement Image;
		public readonly HTMLLabelElement txt_Prompt;
		public readonly HTMLElement txt_message;
		public readonly HTMLLabelElement txt_placeHolder;
		public readonly HTMLInputElement btn_send;
		public GetMessage_html():this(false){}
		public GetMessage_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Body= Document.GetElementById<HTMLDivElement>("Body");
				Image= Document.GetElementById<HTMLImageElement>("Image");
				txt_Prompt= Document.GetElementById<HTMLLabelElement>("txt_Prompt");
				txt_message= Document.GetElementById<HTMLElement>("txt_message");
				txt_placeHolder= Document.GetElementById<HTMLLabelElement>("txt_placeHolder");
				btn_send= Document.GetElementById<HTMLInputElement>("btn_send");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Body= doc.GetElementById<HTMLDivElement>("Body");
			Image= doc.GetElementById<HTMLImageElement>("Image");
			txt_Prompt= doc.GetElementById<HTMLLabelElement>("txt_Prompt");
			txt_message= doc.GetElementById<HTMLElement>("txt_message");
			txt_placeHolder= doc.GetElementById<HTMLLabelElement>("txt_placeHolder");
			btn_send= doc.GetElementById<HTMLInputElement>("btn_send");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Body.Id="";
			Image.Id="";
			txt_Prompt.Id="";
			txt_message.Id="";
			txt_placeHolder.Id="";
			btn_send.Id="";
		}
	
	}
	public class GetMessage_SingleLine_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>
    <div id=""Main"" class=""card w3-round-large"" style=""background-color:rgba(255,255,255,0.7);margin-left:2.5%;width:95%;"">
        <div id=""Body""></div>
        <img class=""card-img-top w3-round-large"" style=""margin-top:3%;width:95%"" id=""Image"">
        <br>
        <label id=""txt_Prompt""></label>
        <br>
        <div class=""card-body"">
            <div class=""input-field"">
                <input type=""text"" class=""form-control"" id=""txt_message"">
                <label id=""txt_placeHolder"" for=""txt_message"">پیام شما</label>
            </div>
            <input type=""button"" id=""btn_send"" value=""ارسال"">
        </div>
    </div>

</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLDivElement Body;
		public readonly HTMLImageElement Image;
		public readonly HTMLLabelElement txt_Prompt;
		public readonly HTMLInputElement txt_message;
		public readonly HTMLLabelElement txt_placeHolder;
		public readonly HTMLInputElement btn_send;
		public GetMessage_SingleLine_html():this(false){}
		public GetMessage_SingleLine_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Body= Document.GetElementById<HTMLDivElement>("Body");
				Image= Document.GetElementById<HTMLImageElement>("Image");
				txt_Prompt= Document.GetElementById<HTMLLabelElement>("txt_Prompt");
				txt_message= Document.GetElementById<HTMLInputElement>("txt_message");
				txt_placeHolder= Document.GetElementById<HTMLLabelElement>("txt_placeHolder");
				btn_send= Document.GetElementById<HTMLInputElement>("btn_send");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Body= doc.GetElementById<HTMLDivElement>("Body");
			Image= doc.GetElementById<HTMLImageElement>("Image");
			txt_Prompt= doc.GetElementById<HTMLLabelElement>("txt_Prompt");
			txt_message= doc.GetElementById<HTMLInputElement>("txt_message");
			txt_placeHolder= doc.GetElementById<HTMLLabelElement>("txt_placeHolder");
			btn_send= doc.GetElementById<HTMLInputElement>("btn_send");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Body.Id="";
			Image.Id="";
			txt_Prompt.Id="";
			txt_message.Id="";
			txt_placeHolder.Id="";
			btn_send.Id="";
		}
	
	}
	public class Hint_Person_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>
    <div id=""Main"" class=""card w3-round-large"" style=""background-color:rgba(255,255,255,0.5);margin-left:2.5%;width:95%;height:100%"">
        <img src=""./Files/NeedUpdate.png"" class=""card-img-top w3-round-large"" style=""max-height:calc(50vh);max-width:100%;margin-top:3%"" id=""Image"">
        <div id=""Body"" class=""card-body"">
        </div>
    </div>

</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLImageElement Image;
		public readonly HTMLDivElement Body;
		public Hint_Person_html():this(false){}
		public Hint_Person_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Image= Document.GetElementById<HTMLImageElement>("Image");
				Body= Document.GetElementById<HTMLDivElement>("Body");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Image= doc.GetElementById<HTMLImageElement>("Image");
			Body= doc.GetElementById<HTMLDivElement>("Body");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Image.Id="";
			Body.Id="";
		}
	
	}
	public class MessageViewer_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>
    <p style=""font-size:medium"" id=""lblMessage"">

</p></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLElement lblMessage;
		public MessageViewer_html():this(false){}
		public MessageViewer_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				lblMessage= Document.GetElementById<HTMLElement>("lblMessage");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			lblMessage= doc.GetElementById<HTMLElement>("lblMessage");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			lblMessage.Id="";
		}
	
	}
	public class Modal_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>


<div id=""myModal"" class=""mdl w3-display-container"" style=""width:100%;height:100%"">
    <style>
        /* The Modal (background) */
        .mdl {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 3; /* Sit on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .mdl-content {
            background-color: rgba(255,255,255,0.8);
            padding: 0;
            border: 1px solid #888;
            width: 80vw;
            max-height: 100vh;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                width: 10vw;
                max-height: 10vh;
                opacity: 0
            }

            to {
                width: 80vw;
                max-height: 100vh;
                opacity: 1
            }
        }

        @keyframes animatetop {
            from {
                width: 10vw;
                max-height: 10vh;
                opacity: 0
            }

            to {
                width: 80vw;
                max-height: 100vh;
                opacity: 1
            }
        }

        /* The Close Button */
        .mdlclose {
            color: darkred;
            float: right;
            font-size: 28px;
            font-weight: bold;
            z-index: 4;
        }

            .mdlclose:hover,
            .mdlclose:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .mdl-header {
            padding: 2px 7px;
            background-color:rgba(21, 158, 189,0.8);
            color: white;
            min-height:35px;
        }

        .mdl-body {
            padding: 10px 16px;
        }
    </style>

  <!-- Modal content -->
  <div class=""mdl-content w3-display-middle w3-round-large"">
    <div class=""mdl-header w3-round-large"">
      <span id=""Btn_Close"" class=""mdlclose"">×</span>
      <div id=""head""> </div>
    </div>
    <div id=""body"" class=""mdl-body w3-round-large"">
    </div>
  </div>
</div>

<script>
// Get the modal
var modal = document.getElementById('myModal');

//// Get the <span> element that closes the modal
var span = document.getElementsByClassName(""close"")[0];

// When the user clicks on <span> (x), close the modal
//span.onclick = function() {
//  modal.style.display = ""none"";
//}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        span.onclick(); 
    }
}
</script>



</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement myModal;
		public readonly HTMLElement Btn_Close;
		public readonly HTMLDivElement head;
		public readonly HTMLDivElement body;
		public Modal_html():this(false){}
		public Modal_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				myModal= Document.GetElementById<HTMLDivElement>("myModal");
				Btn_Close= Document.GetElementById<HTMLElement>("Btn_Close");
				head= Document.GetElementById<HTMLDivElement>("head");
				body= Document.GetElementById<HTMLDivElement>("body");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			myModal= doc.GetElementById<HTMLDivElement>("myModal");
			Btn_Close= doc.GetElementById<HTMLElement>("Btn_Close");
			head= doc.GetElementById<HTMLDivElement>("head");
			body= doc.GetElementById<HTMLDivElement>("body");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			myModal.Id="";
			Btn_Close.Id="";
			head.Id="";
			body.Id="";
		}
	
	}
	public class PlayerFrame_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>


<div id=""myModal"" class=""mdl"" style=""width:100%;height:100%"">
    <style>
        /* The Modal (background) */
        .mdl {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 3; /* Sit on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .mdl-content {
            margin-left:auto;
            margin-right:auto;
            background-color: #fefefe;
            padding: 0;
            border: 1px solid #888;
            width: 100%;
            min-height:100%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                width: 10%;
                height:10%;
                border-radius: 70%;
            }

            to {
                width: 100%;
                height:100%;
                border-radius: 0%;
            }
        }

        @keyframes animatetop {
            from {
                width: 10%;
                height:10%;
                border-radius: 100%;
            }

            to {
                width: 100%;
                height:100%;
                border-radius: 0%;
            }
        }

        /* The Close Button */
        .mdlclose {
            color: darkred;
            float: right;
            font-size: 35px;
            font-weight: bold;
            z-index: 4;
            position:fixed;
            top:0;
            right:0;
            width:10%;
            margin-right:10%;
        }

            .mdlclose:hover,
            .mdlclose:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .mdl-body {
            background-color:rgba(0,0,0,0.0);
        }
    </style>

  <!-- Modal content -->
  <div class=""mdl-content"">
      <img id=""Btn_Close"" src=""../Files/Close.png"" class=""mdlclose"">
    <div id=""body"" class=""mdl-body"">
    </div>
  </div>
</div>

<script>
// Get the modal
var modal = document.getElementById('myModal');

//// Get the <span> element that closes the modal
var span = document.getElementsByClassName(""close"")[0];

// When the user clicks on <span> (x), close the modal
//span.onclick = function() {
//  modal.style.display = ""none"";
//}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        span.onclick(); 
    }
}
</script>



</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement myModal;
		public readonly HTMLImageElement Btn_Close;
		public readonly HTMLDivElement body;
		public PlayerFrame_html():this(false){}
		public PlayerFrame_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				myModal= Document.GetElementById<HTMLDivElement>("myModal");
				Btn_Close= Document.GetElementById<HTMLImageElement>("Btn_Close");
				body= Document.GetElementById<HTMLDivElement>("body");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			myModal= doc.GetElementById<HTMLDivElement>("myModal");
			Btn_Close= doc.GetElementById<HTMLImageElement>("Btn_Close");
			body= doc.GetElementById<HTMLDivElement>("body");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			myModal.Id="";
			Btn_Close.Id="";
			body.Id="";
		}
	
	}
	public class Prompt_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>
    <div id=""Main"" class=""card w3-round-large"" style=""background-color:rgba(255,255,255,0.7);margin-left:2.5%;width:95%;"">
        <img class=""card-img-top w3-round-large"" style=""margin-top:3%;width:70%"" id=""Image"">
        <div id=""Body"" class=""card-body"">
            
        </div>
    </div>

</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Main;
		public readonly HTMLImageElement Image;
		public readonly HTMLDivElement Body;
		public Prompt_html():this(false){}
		public Prompt_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Main= Document.GetElementById<HTMLDivElement>("Main");
				Image= Document.GetElementById<HTMLImageElement>("Image");
				Body= Document.GetElementById<HTMLDivElement>("Body");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Main= doc.GetElementById<HTMLDivElement>("Main");
			Image= doc.GetElementById<HTMLImageElement>("Image");
			Body= doc.GetElementById<HTMLDivElement>("Body");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Main.Id="";
			Image.Id="";
			Body.Id="";
		}
	
	}
	public class SendFile_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body>

    <div id=""main"">
        <input type=""file"" id=""File"">
        <br>
        <input type=""button"" id=""btn_send"" value=""ارسال فایل"">
    </div>


</body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement main;
		public readonly HTMLInputElement File;
		public readonly HTMLInputElement btn_send;
		public SendFile_html():this(false){}
		public SendFile_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				main= Document.GetElementById<HTMLDivElement>("main");
				File= Document.GetElementById<HTMLInputElement>("File");
				btn_send= Document.GetElementById<HTMLInputElement>("btn_send");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			main= doc.GetElementById<HTMLDivElement>("main");
			File= doc.GetElementById<HTMLInputElement>("File");
			btn_send= doc.GetElementById<HTMLInputElement>("btn_send");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			main.Id="";
			File.Id="";
			btn_send.Id="";
		}
	
	}
	public class SimpleButton_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><input type=""button"" id=""btn""></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLInputElement btn;
		public SimpleButton_html():this(false){}
		public SimpleButton_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				btn= Document.GetElementById<HTMLInputElement>("btn");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			btn= doc.GetElementById<HTMLInputElement>("btn");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			btn.Id="";
		}
	
	}
	public class Success_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div id=""main"" style=""direction:rtl;-webkit-flex-wrap:nowrap;display: flex;flex-wrap:nowrap;"">
    <label id=""txt_Success"" style=""color:darkgreen""></label>
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement main;
		public readonly HTMLLabelElement txt_Success;
		public Success_html():this(false){}
		public Success_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				main= Document.GetElementById<HTMLDivElement>("main");
				txt_Success= Document.GetElementById<HTMLLabelElement>("txt_Success");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			main= doc.GetElementById<HTMLDivElement>("main");
			txt_Success= doc.GetElementById<HTMLLabelElement>("txt_Success");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			main.Id="";
			txt_Success.Id="";
		}
	
	}
	public class TextArea_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div id=""main"" class=""input-field"">
    <textarea class=""form-control"" id=""txt""></textarea>
    <label id=""txt_placeHolder"" for=""txt_label"">پیام شما</label>
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement main;
		public readonly HTMLElement txt;
		public readonly HTMLLabelElement txt_placeHolder;
		public TextArea_html():this(false){}
		public TextArea_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				main= Document.GetElementById<HTMLDivElement>("main");
				txt= Document.GetElementById<HTMLElement>("txt");
				txt_placeHolder= Document.GetElementById<HTMLLabelElement>("txt_placeHolder");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			main= doc.GetElementById<HTMLDivElement>("main");
			txt= doc.GetElementById<HTMLElement>("txt");
			txt_placeHolder= doc.GetElementById<HTMLLabelElement>("txt_placeHolder");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			main.Id="";
			txt.Id="";
			txt_placeHolder.Id="";
		}
	
	}
	public class TextBox_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div class=""input-field"">
    <input type=""text"" class=""form-control"" id=""txt_message"">
    <label id=""txt_placeHolder"" for=""txt_message"">پیام شما</label>
</div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLInputElement txt_message;
		public readonly HTMLLabelElement txt_placeHolder;
		public TextBox_html():this(false){}
		public TextBox_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				txt_message= Document.GetElementById<HTMLInputElement>("txt_message");
				txt_placeHolder= Document.GetElementById<HTMLLabelElement>("txt_placeHolder");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			txt_message= doc.GetElementById<HTMLInputElement>("txt_message");
			txt_placeHolder= doc.GetElementById<HTMLLabelElement>("txt_placeHolder");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			txt_message.Id="";
			txt_placeHolder.Id="";
		}
	
	}
	public class TitleContentView_html
	{
		public static readonly string HtmlText = ((Func<string>)(() =>
		{
			byte[] ByteResult = null;
			var Result =
@"<html><head></head><body><div id=""Title""></div>
<br>
<div id=""Content""></div></body></html>";
			var Doc = Document.Parse(Result);
			var Elements = Doc.GetElementsByTagName("*").ToArray();
			foreach(var Element in Elements)
			{
				var MNsrc = Element.GetAttribute("MNsrc");
				if (MNsrc == "")
				MNsrc = null;
				if (MNsrc != null)
				{
					Element.RemoveAttribute("MNsrc");
					var TagName = Element.TagName.ToLower();
					switch(TagName)
					{
						case "script":
							Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
							break;
						case "img":
							Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
							break;
						case "link":
							if (Element.GetAttribute("type").ToLower() == "text/css")
							{
								var Style = Document.document.CreateElement<HTMLStyleElement>();
								Element.SetAttribute("src",(string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
								Element.ParentElement.ReplaceChild(Style, Element);
							}
							break;
					}
				}
			}
			Result = "<html>\"" +"<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" +"<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
			return Result;
		}))();

		public readonly HTMLDivElement Title;
		public readonly HTMLDivElement Content;
		public TitleContentView_html():this(false){}
		public TitleContentView_html(bool IsGlobal)
		{
			if(IsGlobal==true)
			{
				var Document = new Document();
				Title= Document.GetElementById<HTMLDivElement>("Title");
				Content= Document.GetElementById<HTMLDivElement>("Content");
				return;
			}
			var doc =  Document.Parse(HtmlText);
			var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
			foreach(var Tag in HeadTags)
			Document.document.Head.AppendChild(Tag);
			Title= doc.GetElementById<HTMLDivElement>("Title");
			Content= doc.GetElementById<HTMLDivElement>("Content");
			var div = Document.document.CreateElement("Div");
			div.AppendChild(doc.Body);
			var Scripts = div.GetElementsByTagName("Script").ToArray();
			foreach(var Script in Scripts)
			{
				var NewScript = Document.document.CreateElement("Script");
				var Src = Script.GetAttribute("src");
				if(Src!=null)
					NewScript.SetAttribute("src",Src);
				NewScript.InnerHtml = Script.InnerHtml;
				Script.ParentElement.ReplaceChild(NewScript, Script);
			}
			div.SetStyleAttribute("display","none");
			Document.document.Body.AppendChild(div);
			Document.document.Body.RemoveChild(div);
			Title.Id="";
			Content.Id="";
		}
	
	}
	
}
}

