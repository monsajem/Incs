using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using Monsajem_LanguageCompiler;
using static Monsajem_LanguageCompiler.Block;
namespace Monsajem_ResourcesMaker
{
    public class ResourcesMaker
    {
        private static System.IO.Compression.CompressionLevel 
            CompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
        private static char[] ByteToChars = new char[] { '0', '0', '0', '0', '0', '1', '0', '0', '2', '0', '0', '3', '0', '0', '4', '0', '0', '5', '0', '0', '6', '0', '0', '7', '0', '0', '8', '0', '0', '9', '0', '1', '0', '0', '1', '1', '0', '1', '2', '0', '1', '3', '0', '1', '4', '0', '1', '5', '0', '1', '6', '0', '1', '7', '0', '1', '8', '0', '1', '9', '0', '2', '0', '0', '2', '1', '0', '2', '2', '0', '2', '3', '0', '2', '4', '0', '2', '5', '0', '2', '6', '0', '2', '7', '0', '2', '8', '0', '2', '9', '0', '3', '0', '0', '3', '1', '0', '3', '2', '0', '3', '3', '0', '3', '4', '0', '3', '5', '0', '3', '6', '0', '3', '7', '0', '3', '8', '0', '3', '9', '0', '4', '0', '0', '4', '1', '0', '4', '2', '0', '4', '3', '0', '4', '4', '0', '4', '5', '0', '4', '6', '0', '4', '7', '0', '4', '8', '0', '4', '9', '0', '5', '0', '0', '5', '1', '0', '5', '2', '0', '5', '3', '0', '5', '4', '0', '5', '5', '0', '5', '6', '0', '5', '7', '0', '5', '8', '0', '5', '9', '0', '6', '0', '0', '6', '1', '0', '6', '2', '0', '6', '3', '0', '6', '4', '0', '6', '5', '0', '6', '6', '0', '6', '7', '0', '6', '8', '0', '6', '9', '0', '7', '0', '0', '7', '1', '0', '7', '2', '0', '7', '3', '0', '7', '4', '0', '7', '5', '0', '7', '6', '0', '7', '7', '0', '7', '8', '0', '7', '9', '0', '8', '0', '0', '8', '1', '0', '8', '2', '0', '8', '3', '0', '8', '4', '0', '8', '5', '0', '8', '6', '0', '8', '7', '0', '8', '8', '0', '8', '9', '0', '9', '0', '0', '9', '1', '0', '9', '2', '0', '9', '3', '0', '9', '4', '0', '9', '5', '0', '9', '6', '0', '9', '7', '0', '9', '8', '0', '9', '9', '1', '0', '0', '1', '0', '1', '1', '0', '2', '1', '0', '3', '1', '0', '4', '1', '0', '5', '1', '0', '6', '1', '0', '7', '1', '0', '8', '1', '0', '9', '1', '1', '0', '1', '1', '1', '1', '1', '2', '1', '1', '3', '1', '1', '4', '1', '1', '5', '1', '1', '6', '1', '1', '7', '1', '1', '8', '1', '1', '9', '1', '2', '0', '1', '2', '1', '1', '2', '2', '1', '2', '3', '1', '2', '4', '1', '2', '5', '1', '2', '6', '1', '2', '7', '1', '2', '8', '1', '2', '9', '1', '3', '0', '1', '3', '1', '1', '3', '2', '1', '3', '3', '1', '3', '4', '1', '3', '5', '1', '3', '6', '1', '3', '7', '1', '3', '8', '1', '3', '9', '1', '4', '0', '1', '4', '1', '1', '4', '2', '1', '4', '3', '1', '4', '4', '1', '4', '5', '1', '4', '6', '1', '4', '7', '1', '4', '8', '1', '4', '9', '1', '5', '0', '1', '5', '1', '1', '5', '2', '1', '5', '3', '1', '5', '4', '1', '5', '5', '1', '5', '6', '1', '5', '7', '1', '5', '8', '1', '5', '9', '1', '6', '0', '1', '6', '1', '1', '6', '2', '1', '6', '3', '1', '6', '4', '1', '6', '5', '1', '6', '6', '1', '6', '7', '1', '6', '8', '1', '6', '9', '1', '7', '0', '1', '7', '1', '1', '7', '2', '1', '7', '3', '1', '7', '4', '1', '7', '5', '1', '7', '6', '1', '7', '7', '1', '7', '8', '1', '7', '9', '1', '8', '0', '1', '8', '1', '1', '8', '2', '1', '8', '3', '1', '8', '4', '1', '8', '5', '1', '8', '6', '1', '8', '7', '1', '8', '8', '1', '8', '9', '1', '9', '0', '1', '9', '1', '1', '9', '2', '1', '9', '3', '1', '9', '4', '1', '9', '5', '1', '9', '6', '1', '9', '7', '1', '9', '8', '1', '9', '9', '2', '0', '0', '2', '0', '1', '2', '0', '2', '2', '0', '3', '2', '0', '4', '2', '0', '5', '2', '0', '6', '2', '0', '7', '2', '0', '8', '2', '0', '9', '2', '1', '0', '2', '1', '1', '2', '1', '2', '2', '1', '3', '2', '1', '4', '2', '1', '5', '2', '1', '6', '2', '1', '7', '2', '1', '8', '2', '1', '9', '2', '2', '0', '2', '2', '1', '2', '2', '2', '2', '2', '3', '2', '2', '4', '2', '2', '5', '2', '2', '6', '2', '2', '7', '2', '2', '8', '2', '2', '9', '2', '3', '0', '2', '3', '1', '2', '3', '2', '2', '3', '3', '2', '3', '4', '2', '3', '5', '2', '3', '6', '2', '3', '7', '2', '3', '8', '2', '3', '9', '2', '4', '0', '2', '4', '1', '2', '4', '2', '2', '4', '3', '2', '4', '4', '2', '4', '5', '2', '4', '6', '2', '4', '7', '2', '4', '8', '2', '4', '9', '2', '5', '0', '2', '5', '1', '2', '5', '2', '2', '5', '3', '2', '5', '4', '2', '5', '5', };


        int Tabs;
        string AddTabs(int Tabs)
        {
            Tabs += this.Tabs;
            var tab = "";
            for (int i = 0; i < Tabs; i++)
                tab += "\t";
            return tab;
        }
        string AddTabs()
        {
            return AddTabs(0);
        }
        public static string BaseDir;
        public string MakeCs(System.IO.DirectoryInfo dirinfo)
        {
            BaseDir = dirinfo.FullName;
            Block cs = new Block();
            int Tabs = 0;
            cs.NewBlock(() => MakeForDir(dirinfo));
            return cs.Compile();
        }

        void MakeForDir(System.IO.DirectoryInfo dirinfo)
        {
            foreach (var file in dirinfo.GetFiles())
            {
                var FileName = file.Name.Replace(".", "_");
                FileName = FileName.Replace("-", "_");
                CurrentBlock += "public class " + FileName;
                CurrentBlock += "{";
                if (file.Name.ToLower().EndsWith("css"))
                {
                    MakeTextContentField(file.FullName);
                }
                else if(file.Name.ToLower().EndsWith("js"))
                {
                    CurrentBlock.NewBlock(() =>
                    {
                        CurrentBlock += "public static readonly string TextContent = ((Func<string>)(() =>";
                        CurrentBlock += "{";
                        CurrentBlock.NewBlock(() =>
                        {

#if DEBUG
                            CurrentBlock += "return " + MakeStringAsCSharp(System.IO.File.ReadAllText(file.FullName)) + " ;";
#else
                            CurrentBlock += "byte[] Result = null;";
                            MakeZippedBytesAsbyte(Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(file.FullName)), "Result");
                            CurrentBlock += "return System.Text.Encoding.UTF8.GetString(Result);";
#endif
                        });
                        CurrentBlock += "}))();\n";
                    });
                }
                else if (file.Name.ToLower().EndsWith("html") | file.Name.ToLower().EndsWith("htm"))
                {
                    //Use the default configuration for AngleSharp
                    var config = Configuration.Default;

                    //Create a new context for evaluating webpages with the given config
                    var context = BrowsingContext.New(config);

                    //Source to be parsed
                    var source = System.IO.File.ReadAllText(file.FullName);

                    //Create a virtual request to specify the document to load (here from our fixed string)
                    var web = context.OpenAsync(req => req.Content(source)).GetAwaiter().GetResult();

                    var ElementIds = new string[0];
                    var ElementTags = new string[0];
                    foreach (var Element in web.All)
                    {
                        string Attribute = null;
                        if (Element.GetAttribute("src") != null)
                            Attribute = "src";
                        else if (Element.GetAttribute("href") != null & Element.TagName.ToLower() == "link")
                            Attribute = "href";
                        if (Attribute != null)
                        {
                            string Address = file.DirectoryName + "\\" + Element.GetAttribute(Attribute).Replace("/", "\\");
                            if (System.IO.File.Exists(Address))
                            {
                                Address = new System.IO.FileInfo(Address).FullName;
                                if (Address.StartsWith(BaseDir))
                                {
                                    Element.RemoveAttribute(Attribute);
                                    Element.SetAttribute("MNsrc",
                                        "Monsajem_Incs.Resources" +
                                        Address.Substring(BaseDir.Length).
                                        Replace(".", "_").
                                        Replace("-", "_").
                                        Replace("\\", "."));
                                }
                            }
                        }
                    }
                    foreach (var Element in web.All)
                    {
                        if (Element.Id != null)
                        {
                            Array.Resize(ref ElementIds, ElementIds.Length + 1);
                            ElementIds[ElementIds.Length - 1] = Element.Id;
                            Array.Resize(ref ElementTags, ElementTags.Length + 1);
                            ElementTags[ElementTags.Length - 1] = Element.TagName.ToUpper();
                        }
                    }

                    if (ElementIds.Length > 0)
                    {
                        string DocText = "<html>" +
                        "<head>" + web.GetElementsByTagName("head")[0].Minify()+ "</head>" +
                        "<body>" + web.GetElementsByTagName("body")[0].Minify() + "</body></html>";

                        for (int i = 0; i < ElementIds.Length; i++)
                        {
                            if (ElementTags[i] == "INPUT")
                                ElementTags[i] = "HTMLInputElement";
                            else if (ElementTags[i] == "A")
                                ElementTags[i] = "HTMLLinkElement";
                            else if (ElementTags[i] == "LABEL")
                                ElementTags[i] = "HTMLLabelElement";
                            else if (ElementTags[i] == "DIV")
                                ElementTags[i] = "HTMLDivElement";
                            else if (ElementTags[i] == "DL")
                                ElementTags[i] = "HTMLDListElement";
                            else if (ElementTags[i] == "BUTTON")
                                ElementTags[i] = "HTMLButtonElement";
                            else if (ElementTags[i] == "AREA")
                                ElementTags[i] = "HTMLAreaElement";
                            else if (ElementTags[i] == "IMG")
                                ElementTags[i] = "HTMLImageElement";
                            else if (ElementTags[i] == "TABLE")
                                ElementTags[i] = "HTMLTableElement";
                            else if (ElementTags[i] == "TR")
                                ElementTags[i] = "HTMLTableRowElement";
                            else if (ElementTags[i] == "TD")
                                ElementTags[i] = "HTMLTableDataCellElement";
                            else if (ElementTags[i] == "SELECT")
                                ElementTags[i] = "HTMLSelectElement";
                            else if (ElementTags[i] == "OPTION")
                                ElementTags[i] = "HTMLOptionElement";
                            else if (ElementTags[i] == "IFRAME")
                                ElementTags[i] = "HTMLIFrameElement";
                            else
                                ElementTags[i] = "HTMLElement";
                        }


                        CurrentBlock.NewBlock(() =>
                        {
                            CurrentBlock += "public static readonly string HtmlText = ((Func<string>)(() =>";
                            CurrentBlock += "{";
                            CurrentBlock.NewBlock(() =>
                            {

#if DEBUG
                                CurrentBlock += $"var Result =\n"+MakeStringAsCSharp(DocText)+";";
#else
                                CurrentBlock += "byte[] ByteResult = null;";
                                CurrentBlock += $"var Result =\"\";";
                                MakeZippedBytesAsbyte(Encoding.UTF8.GetBytes(DocText), "ByteResult");
                                CurrentBlock += "Result = System.Text.Encoding.UTF8.GetString(ByteResult);";
#endif

                                //CurrentBlock += "#if DEBUG";
                                //CurrentBlock += "Console.WriteLine(\"Text Load Time Of "+ FileName +" \"+Monsajem_Incs.TimeingTester.Timing.run(()=>{";
                                //CurrentBlock += "#endif";

                                CurrentBlock += "var Doc = Document.Parse(Result);";
                                CurrentBlock += "var Elements = Doc.GetElementsByTagName(\"*\").ToArray();";
                                CurrentBlock += "foreach(var Element in Elements)";
                                CurrentBlock += "{";
                                CurrentBlock.NewBlock(() =>
                                {
                                    CurrentBlock += "var MNsrc = Element.GetAttribute(\"MNsrc\");";
                                    CurrentBlock += "if (MNsrc == \"\")";
                                    CurrentBlock += "MNsrc = null;";
                                    CurrentBlock += "if (MNsrc != null)";
                                    CurrentBlock += "{";
                                    CurrentBlock.NewBlock(() =>
                                    {
                                        CurrentBlock += "Element.RemoveAttribute(\"MNsrc\");";
                                        CurrentBlock += "var TagName = Element.TagName.ToLower();";
                                        CurrentBlock += "switch(TagName)";
                                        CurrentBlock += "{";
                                        CurrentBlock.NewBlock(() =>
                                        {
                                            CurrentBlock += "case \"script\":";
                                            CurrentBlock.NewBlock(() =>
                                            {
                                                CurrentBlock += "Element.InnerHtml = (string)Type.GetType(MNsrc).GetField(\"TextContent\").GetValue(null);";
                                                CurrentBlock += "break;";
                                            });
                                            CurrentBlock += "case \"img\":";
                                            CurrentBlock.NewBlock(() =>
                                            {
                                                CurrentBlock += "Element.SetAttribute(\"src\",(string)Type.GetType(MNsrc).GetField(\"Url\").GetValue(null));";
                                                CurrentBlock += "break;";
                                            });
                                            CurrentBlock += "case \"link\":";
                                            CurrentBlock.NewBlock(() =>
                                            {
                                                CurrentBlock += "var LinkType = Element.GetAttribute(\"type\");";
                                                CurrentBlock += "if (LinkType!=null)";
                                                CurrentBlock.NewBlock(() => CurrentBlock += @"LinkType=LinkType.ToLower();");
                                                CurrentBlock += "var LinkRel = Element.GetAttribute(\"rel\");";
                                                CurrentBlock += "if (LinkRel!=null)";
                                                CurrentBlock.NewBlock(() => CurrentBlock += @"LinkRel=LinkRel.ToLower();");
                                                CurrentBlock += "if (LinkType==\"text/css\" || LinkRel==\"stylesheet\")";
                                                CurrentBlock += "{";
                                                CurrentBlock.NewBlock(() =>
                                                {
                                                    CurrentBlock += "var Style = Document.document.CreateElement<HTMLStyleElement>();";
                                                    CurrentBlock += "Style.InnerHtml = (string)Type.GetType(MNsrc).GetField(\"TextContent\").GetValue(null);";
                                                    CurrentBlock += "Element.ParentElement.ReplaceChild(Style, Element);";
                                                });
                                                CurrentBlock += "}";
                                                CurrentBlock += "break;";
                                            });
                                        });
                                        CurrentBlock += "}";
                                    });
                                    CurrentBlock += "}";
                                });
                                CurrentBlock += "}";

                                CurrentBlock += "Result = \"<html>\\\"\" +" +
                                                "\"<head>\" + Doc.GetElementsByTagName(\"head\")[0].InnerHtml + \"</head>\" +"+
                                                "\"<body>\" + Doc.GetElementsByTagName(\"body\")[0].InnerHtml + \"</body></html>\";";
                               
                                //CurrentBlock += "#if DEBUG";
                                //CurrentBlock += "}).ToString());";
                                //CurrentBlock += "#endif";

                                CurrentBlock += "return Result;";

                            });
                            CurrentBlock += "}))();\n";

                            for (int i = 0; i < ElementIds.Length; i++)
                            {
                                CurrentBlock += "public readonly " + ElementTags[i] + " " + ElementIds[i] + ";";
                            }
                            CurrentBlock += "public " + FileName + "():this(false){}";
                            CurrentBlock += "public " + FileName + "(bool IsGlobal)";
                            CurrentBlock += "{";
                            CurrentBlock.NewBlock(() =>
                            {
                                CurrentBlock += "if(IsGlobal==true)";
                                CurrentBlock += "{";
                                CurrentBlock.NewBlock(() =>
                                {
                                    CurrentBlock += "var Document = new Document();";
                                    for (int i2 = 0; i2 < ElementIds.Length; i2++)
                                    {
                                        CurrentBlock += ElementIds[i2] + "= Document.GetElementById<" + ElementTags[i2] + ">(\"" + ElementIds[i2] + "\");";
                                    }
                                    CurrentBlock += "return;";
                                });
                                CurrentBlock += "}";

                                CurrentBlock += "var doc =  Document.Parse(HtmlText);";

                                if(web.Head.GetElementsByTagName("*").Length>0)
                                {
                                    CurrentBlock += "var HeadTags = doc.Head.GetElementsByTagName(\"*\").ToArray();";
                                    CurrentBlock += "foreach(var Tag in HeadTags)";
                                    CurrentBlock += "Document.document.Head.AppendChild(Tag);";
                                }

                                for (int i2 = 0; i2 < ElementIds.Length; i2++)
                                {
                                    CurrentBlock += ElementIds[i2] + "= doc.GetElementById<" + ElementTags[i2] + ">(\"" + ElementIds[i2] + "\");";
                                }

                                if(web.Body.GetElementsByTagName("Script").Length>0)
                                {
                                    CurrentBlock += "var div = Document.document.CreateElement(\"Div\");";
                                    CurrentBlock += "div.AppendChild(doc.Body);";
                                    CurrentBlock += "var Scripts = div.GetElementsByTagName(\"Script\").ToArray();";
                                    CurrentBlock += "foreach(var Script in Scripts)";
                                    CurrentBlock += "{";
                                    CurrentBlock.NewBlock(() =>
                                    {
                                        CurrentBlock += "var NewScript = Document.document.CreateElement(\"Script\");";
                                        CurrentBlock += "var Src = Script.GetAttribute(\"src\");";
                                        CurrentBlock += "if(Src!=null)";
                                        CurrentBlock.NewBlock(() =>
                                            CurrentBlock += "NewScript.SetAttribute(\"src\",Src);");
                                        CurrentBlock += "NewScript.InnerHtml = Script.InnerHtml;";
                                        CurrentBlock += "Script.ParentElement.ReplaceChild(NewScript, Script);";
                                    });
                                    CurrentBlock += "}";

                                    CurrentBlock += "div.SetStyleAttribute(\"display\",\"none\");";
                                    CurrentBlock += "Document.document.Body.AppendChild(div);";

                                    CurrentBlock += "Document.document.Body.RemoveChild(div);";
                                }

                                for (int i2 = 0; i2 < ElementIds.Length; i2++)
                                    CurrentBlock += ElementIds[i2] + ".Id=\"\";";

                            });
                            CurrentBlock += "}";

                        });
                    }

                }
                else
                {
                    MakeDataField(file.FullName);
                }

                CurrentBlock += "\n" + AddTabs() + "}";
            }

            foreach (var Dir in dirinfo.GetDirectories())
            {
                CurrentBlock += "\n" + AddTabs() + "namespace " + Dir.Name.Replace(".", "_") +
                        "\n" + AddTabs() + "{";
                Tabs += 1;
                MakeForDir(Dir);
                Tabs -= 1;
                CurrentBlock += "\n" + AddTabs() + "}";
            }
        }

        private static void MakeTextContentField(string FileAddress)
        {
            var Text = System.IO.File.ReadAllText(FileAddress);
            CurrentBlock.NewBlock(() =>
            {
                CurrentBlock += $"public static readonly string TextContent =\n@\"{Text.Replace("\"", "\"\"")}\";"; ;
            });
        }
        private static void MakeDataField(string FileAddress)
        {
            var Bytes = System.IO.File.ReadAllBytes(FileAddress);
            CurrentBlock.NewBlock(() =>
            {
                var FileType = "image/gif";
                FileType = "";
                CurrentBlock += $"public static readonly string Url =\"data:{FileType};base64,{Convert.ToBase64String(Bytes)}\";";
            });
        }

        private static void MakeBytesAsB64(byte[] Bytes, string ToVariable)
        {
            var Text = System.Convert.ToBase64String(Bytes);
            CurrentBlock += ToVariable + " = System.Convert.FromBase64String(\"" + Text + "\");";
        }

        private static void MakeZippedBytesAsB64(byte[] Bytes, string ToVariable)
        {
            var ZipMemmory = new System.IO.MemoryStream();
            using (var Zipper = new System.IO.Compression.DeflateStream(ZipMemmory, CompressionLevel))
            {
                Zipper.Write(Bytes, 0, Bytes.Length);
            }
            Bytes = ZipMemmory.ToArray();
            var Text = System.Convert.ToBase64String(Bytes);

            CurrentBlock += "{";
            CurrentBlock.NewBlock(() =>
            {
                CurrentBlock += "var ZipMemmory = new System.IO.MemoryStream(System.Convert.FromBase64String(\"" + Text + "\"));";
                CurrentBlock += "var DezippedMemory = new System.IO.MemoryStream();";
                CurrentBlock += "using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))";
                CurrentBlock += "{";
                CurrentBlock.NewBlock("Dezip.CopyTo(DezippedMemory);");
                CurrentBlock += "}";
                CurrentBlock += ToVariable + " = DezippedMemory.ToArray();";
            });
            CurrentBlock += "}\n";
        }


        private static string BytesToCSharpArray(byte[] Bytes)
        {
            var strBytes = new char[Bytes.Length * 4];
            for (int i = 0; i < Bytes.Length; i++)
            {
                var strByte = Bytes[i] * 3;
                var bytepos = i * 4;
                strBytes[bytepos] = ByteToChars[strByte];
                strBytes[bytepos + 1] = ByteToChars[strByte + 1];
                strBytes[bytepos + 2] = ByteToChars[strByte + 2];
                strBytes[bytepos + 3] = ',';
            }
            return "new byte[] {" + new string(strBytes) + "}";
        }
        private static void MakeZippedBytesAsbyte(byte[] Bytes, string ToVariable)
        {
            var ZipMemmory = new System.IO.MemoryStream();
            using (var Zipper = new System.IO.Compression.DeflateStream(ZipMemmory,CompressionLevel))
            {
                Zipper.Write(Bytes, 0, Bytes.Length);
            }

            var NewBytes = ZipMemmory.ToArray();

            if(NewBytes.Length<Bytes.Length)
            {
                Bytes = NewBytes;

                CurrentBlock += "{";
                CurrentBlock.NewBlock(() =>
                {
                    CurrentBlock += "var ZipMemmory = new System.IO.MemoryStream(" + BytesToCSharpArray(Bytes) + ");";
                    CurrentBlock += "var DezippedMemory = new System.IO.MemoryStream();";
                    CurrentBlock += "using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))";
                    CurrentBlock += "{";
                    CurrentBlock.NewBlock("Dezip.CopyTo(DezippedMemory);");
                    CurrentBlock += "}";
                    CurrentBlock += ToVariable + " = DezippedMemory.ToArray();";
                });
                CurrentBlock += "}\n";
            }
            else
            {
                CurrentBlock += ToVariable + " = " + BytesToCSharpArray(Bytes) + ";";
            }
        }

        private static void MakeBytesAsbyte(byte[] Bytes, string ToVariable)
        {
            CurrentBlock += ToVariable + " = " + BytesToCSharpArray(Bytes)+";";
        }

        private static string MakeStringAsCSharp(string Str)
        {
             return $"@\"{Str.Replace("\"", "\"\"")}\"";
        }

    }
}
