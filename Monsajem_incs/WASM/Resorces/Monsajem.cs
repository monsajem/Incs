

using System;
using System.Linq;
using WebAssembly.Browser.DOM;
using WebAssembly.Browser.MonsajemDomHelpers;
namespace Monsajem_Incs.Resources
{




    namespace Base
    {
        public class Permitions_html
        {
            public static readonly string HtmlText = ((Func<string>)(() =>
            {
                byte[] ByteResult = null;
                var Result = "";
                {
                    var ZipMemmory = new System.IO.MemoryStream(new byte[] { 109, 144, 049, 014, 130, 064, 016, 069, 175, 178, 225, 002, 244, 102, 151, 196, 068, 011, 011, 173, 188, 000, 176, 019, 217, 184, 002, 193, 005, 165, 164, 081, 010, 239, 129, 049, 026, 148, 112, 152, 089, 189, 140, 043, 052, 152, 080, 077, 102, 242, 254, 159, 153, 079, 003, 181, 147, 014, 013, 192, 229, 014, 181, 251, 226, 069, 060, 119, 040, 023, 025, 217, 171, 092, 002, 243, 162, 132, 067, 050, 225, 238, 062, 000, 078, 004, 103, 075, 087, 132, 014, 233, 008, 211, 173, 133, 146, 096, 196, 166, 053, 067, 017, 198, 169, 234, 161, 045, 172, 224, 064, 084, 030, 003, 243, 003, 240, 183, 094, 116, 036, 153, 043, 083, 096, 088, 125, 046, 120, 199, 010, 235, 161, 098, 206, 133, 026, 197, 117, 105, 240, 087, 039, 106, 135, 130, 025, 072, 080, 048, 190, 225, 129, 079, 093, 012, 225, 169, 239, 067, 060, 238, 111, 097, 165, 011, 108, 116, 137, 181, 062, 017, 188, 234, 051, 121, 215, 248, 210, 165, 062, 091, 067, 139, 197, 038, 140, 146, 241, 125, 022, 094, 205, 133, 181, 121, 169, 197, 219, 207, 164, 194, 230, 207, 164, 139, 135, 218, 125, 182, 118, 151, 250, 023, });
                    var DezippedMemory = new System.IO.MemoryStream();
                    using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                    {
                        Dezip.CopyTo(DezippedMemory);
                    }
                    ByteResult = DezippedMemory.ToArray();
                }

                Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                var Doc = Document.Parse(Result);
                var Elements = Doc.GetElementsByTagName("*").ToArray();
                foreach (var Element in Elements)
                {
                    var MNsrc = Element.GetAttribute("MNsrc");
                    if (MNsrc == "")
                        MNsrc = null;
                    if (MNsrc != null)
                    {
                        Element.RemoveAttribute("MNsrc");
                        var TagName = Element.TagName.ToLower();
                        switch (TagName)
                        {
                            case "script":
                                Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                break;
                            case "img":
                                Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                break;
                            case "link":
                                var LinkType = Element.GetAttribute("type");
                                if (LinkType != null)
                                    LinkType = LinkType.ToLower();
                                var LinkRel = Element.GetAttribute("rel");
                                if (LinkRel != null)
                                    LinkRel = LinkRel.ToLower();
                                if (LinkType == "text/css" || LinkRel == "stylesheet")
                                {
                                    var Style = js.Document.CreateElement<HTMLStyleElement>();
                                    Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    _ = Element.ParentElement.ReplaceChild(Style, Element);
                                }
                                break;
                        }
                    }
                }
                Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                return Result;
            }))();

            public readonly HTMLDivElement Main;
            public readonly HTMLDivElement Title;
            public readonly HTMLInputElement MakeNew;
            public readonly HTMLInputElement Edit;
            public readonly HTMLInputElement Delete;
            public readonly HTMLInputElement Accept;
            public readonly HTMLInputElement Ignore;
            public Permitions_html() : this(false) { }
            public Permitions_html(bool IsGlobal)
            {
                if (IsGlobal == true)
                {
                    var Document = new Document();
                    Main = Document.GetElementById<HTMLDivElement>("Main");
                    Title = Document.GetElementById<HTMLDivElement>("Title");
                    MakeNew = Document.GetElementById<HTMLInputElement>("MakeNew");
                    Edit = Document.GetElementById<HTMLInputElement>("Edit");
                    Delete = Document.GetElementById<HTMLInputElement>("Delete");
                    Accept = Document.GetElementById<HTMLInputElement>("Accept");
                    Ignore = Document.GetElementById<HTMLInputElement>("Ignore");
                    return;
                }
                var doc = Document.Parse(HtmlText);
                Main = doc.GetElementById<HTMLDivElement>("Main");
                Title = doc.GetElementById<HTMLDivElement>("Title");
                MakeNew = doc.GetElementById<HTMLInputElement>("MakeNew");
                Edit = doc.GetElementById<HTMLInputElement>("Edit");
                Delete = doc.GetElementById<HTMLInputElement>("Delete");
                Accept = doc.GetElementById<HTMLInputElement>("Accept");
                Ignore = doc.GetElementById<HTMLInputElement>("Ignore");
                Main.Id = "";
                Title.Id = "";
                MakeNew.Id = "";
                Edit.Id = "";
                Delete.Id = "";
                Accept.Id = "";
                Ignore.Id = "";
            }

        }

        namespace Chat
        {
            public class ChatBox_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 173, 088, 209, 114, 155, 058, 016, 125, 239, 087, 104, 146, 233, 180, 119, 198, 138, 193, 137, 221, 154, 058, 126, 232, 125, 189, 247, 169, 095, 032, 144, 048, 154, 008, 196, 032, 097, 039, 237, 244, 223, 239, 130, 068, 130, 205, 130, 221, 235, 224, 012, 196, 104, 181, 218, 061, 058, 123, 036, 121, 147, 217, 092, 109, 055, 153, 096, 124, 187, 153, 187, 071, 172, 249, 139, 187, 019, 091, 177, 194, 040, 102, 197, 099, 161, 183, 027, 046, 247, 068, 242, 199, 191, 051, 102, 191, 235, 231, 045, 217, 024, 251, 162, 196, 246, 003, 233, 093, 119, 009, 180, 198, 250, 153, 252, 058, 122, 221, 092, 169, 046, 044, 077, 089, 046, 213, 075, 068, 062, 253, 195, 172, 254, 052, 035, 006, 006, 160, 070, 084, 050, 253, 134, 119, 048, 242, 167, 136, 072, 248, 080, 062, 015, 013, 014, 146, 219, 044, 034, 015, 065, 128, 181, 102, 066, 238, 050, 059, 218, 156, 179, 106, 039, 139, 136, 004, 100, 001, 237, 240, 008, 134, 054, 165, 054, 210, 074, 013, 086, 149, 000, 024, 228, 094, 012, 109, 032, 089, 106, 050, 198, 245, 161, 241, 021, 144, 101, 235, 172, 218, 197, 236, 115, 048, 035, 238, 239, 110, 241, 215, 176, 035, 151, 166, 084, 012, 176, 072, 149, 064, 226, 107, 222, 210, 084, 053, 110, 019, 173, 234, 188, 192, 134, 174, 184, 168, 104, 197, 184, 172, 013, 128, 212, 228, 017, 142, 038, 019, 179, 228, 105, 087, 233, 186, 224, 017, 057, 100, 210, 162, 185, 088, 171, 243, 008, 235, 221, 082, 193, 131, 113, 023, 026, 034, 152, 017, 084, 215, 246, 216, 242, 247, 135, 033, 027, 104, 046, 140, 097, 059, 097, 016, 078, 248, 012, 172, 046, 033, 124, 008, 220, 104, 037, 249, 041, 120, 193, 018, 065, 175, 100, 156, 203, 098, 231, 178, 030, 054, 235, 189, 168, 028, 120, 172, 182, 250, 026, 240, 043, 125, 032, 135, 138, 149, 067, 035, 166, 228, 174, 160, 009, 176, 084, 020, 214, 121, 162, 198, 178, 202, 226, 254, 032, 212, 097, 131, 073, 042, 173, 020, 141, 069, 198, 246, 082, 087, 017, 049, 185, 214, 054, 155, 070, 213, 003, 074, 027, 234, 101, 090, 001, 130, 008, 180, 190, 056, 194, 032, 248, 056, 069, 254, 128, 132, 075, 012, 192, 235, 217, 233, 224, 001, 158, 229, 198, 131, 035, 010, 126, 089, 098, 006, 044, 209, 164, 250, 146, 176, 152, 174, 234, 177, 196, 032, 220, 006, 231, 219, 251, 096, 245, 176, 078, 198, 194, 054, 066, 165, 227, 083, 122, 018, 055, 022, 059, 155, 013, 095, 069, 074, 022, 079, 216, 251, 189, 132, 210, 018, 124, 230, 011, 166, 132, 001, 011, 040, 139, 130, 229, 162, 117, 132, 188, 237, 124, 097, 077, 222, 029, 130, 223, 037, 000, 052, 151, 021, 207, 150, 114, 145, 232, 138, 185, 146, 047, 116, 129, 232, 197, 004, 049, 145, 193, 095, 075, 118, 229, 117, 234, 172, 162, 173, 090, 045, 107, 239, 255, 083, 156, 123, 154, 215, 170, 010, 148, 195, 140, 132, 095, 188, 178, 132, 136, 176, 184, 016, 034, 178, 056, 150, 163, 243, 029, 059, 092, 087, 073, 243, 153, 094, 206, 006, 220, 157, 098, 020, 160, 025, 177, 212, 162, 005, 225, 006, 246, 010, 116, 115, 131, 207, 230, 027, 082, 044, 134, 148, 106, 076, 250, 251, 169, 183, 107, 136, 203, 189, 149, 125, 160, 023, 248, 159, 234, 227, 021, 060, 152, 194, 012, 091, 003, 123, 030, 042, 183, 090, 227, 068, 115, 118, 110, 113, 162, 011, 180, 242, 155, 203, 251, 192, 185, 053, 070, 086, 095, 060, 216, 234, 116, 202, 158, 038, 157, 240, 001, 110, 247, 203, 063, 099, 207, 249, 142, 215, 202, 142, 207, 098, 146, 040, 030, 030, 124, 069, 116, 209, 059, 136, 199, 045, 218, 121, 158, 152, 001, 037, 082, 024, 097, 061, 214, 124, 005, 195, 186, 216, 078, 073, 118, 012, 237, 025, 146, 185, 240, 046, 019, 179, 086, 089, 101, 081, 214, 118, 124, 145, 061, 179, 074, 094, 180, 189, 057, 101, 195, 120, 028, 072, 000, 149, 112, 138, 130, 167, 244, 170, 184, 203, 081, 197, 125, 219, 037, 099, 173, 239, 191, 103, 239, 084, 114, 221, 094, 127, 176, 083, 234, 168, 131, 103, 138, 149, 234, 212, 246, 241, 148, 084, 023, 239, 061, 167, 182, 044, 200, 252, 208, 131, 136, 159, 164, 165, 172, 044, 005, 003, 166, 039, 163, 115, 213, 207, 224, 118, 157, 036, 235, 224, 204, 030, 158, 230, 250, 039, 133, 093, 000, 248, 165, 187, 102, 197, 132, 010, 250, 028, 126, 013, 184, 216, 205, 200, 109, 016, 240, 175, 105, 218, 254, 019, 047, 249, 010, 131, 160, 239, 203, 135, 249, 094, 238, 244, 187, 121, 202, 205, 123, 185, 186, 194, 141, 039, 237, 200, 169, 233, 236, 166, 244, 181, 012, 199, 246, 164, 211, 220, 110, 055, 099, 221, 017, 179, 097, 106, 136, 028, 049, 131, 187, 123, 036, 112, 119, 008, 168, 011, 035, 016, 113, 245, 165, 143, 180, 254, 126, 253, 182, 153, 187, 067, 062, 105, 079, 255, 137, 098, 198, 060, 222, 116, 103, 252, 006, 217, 146, 054, 223, 110, 188, 129, 255, 121, 224, 223, 238, 196, 231, 058, 028, 157, 002, 193, 114, 014, 166, 071, 030, 007, 082, 011, 173, 077, 214, 176, 042, 176, 198, 231, 015, 040, 046, 239, 115, 208, 099, 187, 153, 119, 166, 208, 203, 233, 164, 239, 242, 189, 134, 234, 046, 136, 125, 041, 197, 163, 169, 227, 092, 090, 178, 103, 170, 022, 109, 163, 119, 212, 175, 223, 183, 208, 250, 143, 205, 220, 253, 034, 210, 061, 218, 159, 076, 254, 003, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement ChatBox;
                public readonly HTMLDivElement ChatMessages;
                public readonly HTMLElement SendMessage;
                public readonly HTMLInputElement SendButton;
                public ChatBox_html() : this(false) { }
                public ChatBox_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        ChatBox = Document.GetElementById<HTMLDivElement>("ChatBox");
                        ChatMessages = Document.GetElementById<HTMLDivElement>("ChatMessages");
                        SendMessage = Document.GetElementById<HTMLElement>("SendMessage");
                        SendButton = Document.GetElementById<HTMLInputElement>("SendButton");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    ChatBox = doc.GetElementById<HTMLDivElement>("ChatBox");
                    ChatMessages = doc.GetElementById<HTMLDivElement>("ChatMessages");
                    SendMessage = doc.GetElementById<HTMLElement>("SendMessage");
                    SendButton = doc.GetElementById<HTMLInputElement>("SendButton");
                    ChatBox.Id = "";
                    ChatMessages.Id = "";
                    SendMessage.Id = "";
                    SendButton.Id = "";
                }

            }
            public class ReciveMessage_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 101, 143, 049, 010, 195, 048, 012, 069, 175, 034, 178, 007, 095, 064, 246, 156, 165, 083, 079, 160, 068, 034, 014, 056, 118, 177, 076, 073, 111, 223, 096, 199, 164, 208, 069, 095, 124, 061, 125, 036, 244, 101, 015, 014, 189, 016, 059, 052, 077, 230, 196, 159, 086, 161, 100, 138, 026, 168, 136, 141, 201, 033, 111, 111, 216, 216, 062, 068, 149, 086, 153, 082, 096, 201, 176, 004, 082, 181, 123, 243, 198, 057, 029, 163, 175, 003, 007, 157, 127, 074, 252, 007, 181, 154, 039, 100, 078, 234, 102, 175, 236, 011, 030, 126, 098, 161, 247, 047, 202, 037, 074, 030, 238, 221, 042, 104, 218, 221, 093, 234, 099, 095, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement MessageHolder;
                public readonly HTMLDivElement Sender;
                public readonly HTMLDivElement Message;
                public ReciveMessage_html() : this(false) { }
                public ReciveMessage_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        MessageHolder = Document.GetElementById<HTMLDivElement>("MessageHolder");
                        Sender = Document.GetElementById<HTMLDivElement>("Sender");
                        Message = Document.GetElementById<HTMLDivElement>("Message");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    MessageHolder = doc.GetElementById<HTMLDivElement>("MessageHolder");
                    Sender = doc.GetElementById<HTMLDivElement>("Sender");
                    Message = doc.GetElementById<HTMLDivElement>("Message");
                    MessageHolder.Id = "";
                    Sender.Id = "";
                    Message.Id = "";
                }

            }
            public class SendMessage_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 016, 082, 161, 164, 040, 049, 175, 056, 039, 177, 036, 213, 054, 047, 223, 206, 038, 037, 179, 076, 033, 051, 197, 214, 055, 181, 184, 056, 049, 061, 213, 035, 063, 039, 037, 181, 072, 033, 057, 039, 177, 184, 216, 054, 023, 034, 166, 155, 148, 095, 161, 155, 001, 150, 176, 083, 064, 083, 143, 169, 018, 168, 068, 031, 168, 006, 070, 217, 232, 067, 108, 134, 081, 096, 167, 001, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement MessageHolder;
                public readonly HTMLDivElement Message;
                public SendMessage_html() : this(false) { }
                public SendMessage_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        MessageHolder = Document.GetElementById<HTMLDivElement>("MessageHolder");
                        Message = Document.GetElementById<HTMLDivElement>("Message");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    MessageHolder = doc.GetElementById<HTMLDivElement>("MessageHolder");
                    Message = doc.GetElementById<HTMLDivElement>("Message");
                    MessageHolder.Id = "";
                    Message.Id = "";
                }

            }

        }

        namespace Html
        {
            public class a_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 137, 010, 153, 041, 182, 190, 137, 153, 121, 118, 010, 054, 250, 137, 064, 073, 136, 176, 062, 088, 003, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLLinkElement Main;
                public a_html() : this(false) { }
                public a_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLLinkElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLLinkElement>("Main");
                    Main.Id = "";
                }

            }
            public class button_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 178, 180, 164, 036, 063, 079, 033, 051, 197, 214, 055, 049, 051, 015, 040, 009, 017, 000, 049, 192, 242, 250, 096, 157, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLButtonElement Main;
                public button_html() : this(false) { }
                public button_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLButtonElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLButtonElement>("Main");
                    Main.Id = "";
                }

            }
            public class Div_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 041, 153, 101, 010, 153, 041, 182, 190, 137, 153, 121, 118, 010, 054, 250, 064, 046, 080, 001, 068, 074, 031, 172, 009, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement Main;
                public Div_html() : this(false) { }
                public Div_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLDivElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLDivElement>("Main");
                    Main.Id = "";
                }

            }
            public class h1_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 067, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 033, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h1_html() : this(false) { }
                public h1_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class h2_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 035, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 017, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h2_html() : this(false) { }
                public h2_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class h3_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 099, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 049, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h3_html() : this(false) { }
                public h3_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class h4_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 019, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 009, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h4_html() : this(false) { }
                public h4_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class h5_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 083, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 041, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h5_html() : this(false) { }
                public h5_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class h6_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 064, 033, 051, 133, 204, 020, 091, 223, 196, 204, 060, 144, 132, 025, 144, 128, 136, 235, 131, 117, 000, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public h6_html() : this(false) { }
                public h6_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }
            public class Img_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 185, 233, 010, 153, 041, 182, 190, 137, 153, 121, 064, 025, 136, 152, 062, 088, 053, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLImageElement Main;
                public Img_html() : this(false) { }
                public Img_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLImageElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLImageElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 153, 041, 182, 190, 137, 153, 121, 064, 057, 136, 168, 062, 088, 061, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_html() : this(false) { }
                public input_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_button_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 037, 149, 005, 169, 182, 073, 165, 037, 037, 249, 121, 010, 153, 041, 182, 190, 137, 153, 121, 064, 117, 016, 021, 250, 096, 189, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_button_html() : this(false) { }
                public input_button_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_CheckBox_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 037, 149, 005, 169, 182, 201, 025, 169, 201, 217, 073, 249, 021, 010, 153, 041, 182, 190, 137, 153, 121, 064, 149, 016, 053, 250, 096, 221, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_CheckBox_html() : this(false) { }
                public input_CheckBox_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_File_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 037, 149, 005, 169, 182, 105, 153, 057, 169, 010, 153, 041, 182, 190, 137, 153, 121, 064, 085, 016, 121, 125, 176, 078, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_File_html() : this(false) { }
                public input_File_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_tel_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 037, 149, 005, 169, 182, 037, 169, 057, 010, 153, 041, 182, 190, 137, 153, 121, 064, 069, 016, 105, 125, 176, 070, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_tel_html() : this(false) { }
                public input_tel_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class input_Text_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 209, 135, 080, 073, 249, 041, 149, 118, 054, 153, 121, 005, 165, 037, 010, 037, 149, 005, 169, 182, 037, 169, 021, 037, 010, 153, 041, 182, 190, 137, 153, 121, 064, 085, 016, 121, 125, 176, 078, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLInputElement Main;
                public input_Text_html() : this(false) { }
                public input_Text_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLInputElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    Main = doc.GetElementById<HTMLInputElement>("Main");
                    Main.Id = "";
                }

            }
            public class Script_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 179, 201, 040, 201, 205, 177, 179, 201, 072, 077, 076, 177, 179, 041, 078, 046, 202, 044, 040, 081, 200, 076, 177, 245, 077, 204, 204, 179, 227, 178, 209, 135, 136, 216, 217, 232, 067, 020, 036, 229, 167, 084, 002, 057, 080, 010, 172, 021, 000, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLElement Main;
                public Script_html() : this(false) { }
                public Script_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        Main = Document.GetElementById<HTMLElement>("Main");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    var HeadTags = doc.Head.GetElementsByTagName("*").ToArray();
                    foreach (var Tag in HeadTags)
                        _ = js.Document.Head.AppendChild(Tag);
                    Main = doc.GetElementById<HTMLElement>("Main");
                    Main.Id = "";
                }

            }

        }

        namespace Partials
        {
            public class Modal_html
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 237, 086, 093, 146, 155, 056, 016, 126, 247, 041, 186, 156, 074, 197, 078, 025, 027, 038, 051, 019, 199, 127, 149, 076, 042, 091, 187, 015, 251, 180, 251, 190, 037, 144, 048, 042, 011, 137, 002, 097, 076, 082, 057, 071, 014, 180, 023, 219, 022, 194, 030, 016, 246, 102, 014, 016, 166, 060, 128, 212, 063, 095, 247, 215, 180, 122, 147, 232, 084, 236, 054, 009, 035, 116, 183, 089, 216, 091, 168, 104, 189, 219, 080, 126, 004, 078, 183, 105, 253, 167, 162, 068, 064, 036, 072, 081, 108, 199, 041, 021, 080, 189, 243, 040, 047, 050, 065, 106, 047, 082, 082, 019, 046, 089, 062, 134, 066, 215, 130, 109, 043, 078, 117, 178, 010, 124, 255, 245, 058, 097, 124, 159, 232, 230, 121, 007, 155, 102, 123, 055, 130, 246, 090, 188, 133, 191, 019, 006, 214, 246, 036, 036, 209, 097, 159, 171, 082, 210, 041, 188, 093, 092, 132, 230, 198, 219, 183, 203, 171, 185, 090, 199, 043, 008, 133, 138, 014, 107, 099, 231, 119, 078, 041, 147, 016, 214, 064, 089, 076, 074, 161, 187, 038, 204, 149, 169, 130, 107, 174, 228, 010, 098, 126, 098, 180, 081, 250, 075, 147, 026, 184, 004, 052, 022, 049, 087, 225, 171, 199, 037, 101, 167, 021, 188, 179, 178, 092, 131, 146, 160, 085, 230, 010, 010, 022, 235, 021, 248, 235, 222, 034, 202, 013, 214, 108, 090, 160, 201, 139, 049, 249, 091, 041, 132, 093, 116, 077, 182, 089, 115, 068, 237, 170, 043, 171, 142, 044, 143, 133, 170, 086, 064, 074, 173, 026, 233, 047, 146, 132, 130, 065, 017, 229, 010, 213, 120, 012, 146, 049, 202, 168, 171, 249, 156, 112, 164, 080, 168, 124, 005, 249, 062, 156, 248, 051, 252, 155, 090, 175, 068, 008, 035, 004, 205, 246, 139, 212, 137, 213, 159, 249, 243, 123, 107, 227, 073, 024, 003, 213, 002, 084, 070, 034, 174, 235, 174, 149, 239, 163, 110, 041, 216, 050, 248, 140, 197, 196, 164, 118, 043, 160, 041, 050, 179, 254, 237, 037, 024, 238, 030, 030, 102, 231, 159, 063, 095, 078, 251, 060, 100, 132, 082, 046, 247, 003, 126, 066, 149, 083, 134, 038, 130, 236, 004, 133, 018, 156, 194, 171, 229, 114, 121, 149, 195, 165, 127, 172, 250, 027, 041, 057, 121, 029, 218, 142, 137, 107, 251, 228, 021, 009, 161, 134, 039, 031, 238, 209, 195, 018, 127, 126, 063, 101, 119, 211, 153, 015, 143, 184, 126, 231, 015, 055, 131, 015, 078, 024, 094, 197, 194, 003, 215, 030, 145, 060, 037, 166, 180, 061, 073, 082, 134, 101, 208, 188, 051, 172, 192, 159, 201, 211, 050, 039, 246, 155, 064, 186, 138, 190, 244, 075, 173, 222, 178, 118, 131, 228, 079, 148, 194, 167, 179, 074, 151, 228, 143, 103, 120, 007, 086, 199, 057, 186, 044, 058, 046, 029, 214, 227, 092, 165, 206, 082, 135, 155, 096, 192, 205, 144, 031, 151, 030, 115, 181, 005, 138, 240, 123, 091, 029, 252, 230, 210, 234, 182, 231, 097, 085, 012, 061, 255, 191, 235, 192, 113, 125, 005, 196, 199, 095, 009, 234, 184, 190, 002, 162, 061, 085, 062, 011, 085, 048, 120, 042, 181, 238, 023, 154, 233, 038, 081, 179, 215, 135, 217, 246, 015, 074, 242, 067, 142, 231, 067, 063, 159, 066, 017, 004, 151, 027, 140, 206, 014, 182, 037, 175, 224, 095, 241, 011, 121, 112, 129, 055, 123, 085, 027, 088, 168, 132, 099, 244, 114, 192, 220, 175, 175, 133, 209, 195, 186, 074, 076, 155, 159, 221, 216, 140, 085, 084, 022, 087, 178, 222, 134, 244, 202, 247, 253, 097, 074, 053, 059, 105, 143, 178, 072, 157, 191, 090, 169, 036, 027, 138, 069, 101, 094, 024, 035, 153, 226, 216, 128, 243, 245, 045, 234, 155, 038, 109, 070, 007, 150, 059, 072, 046, 237, 246, 014, 155, 218, 251, 236, 228, 052, 070, 183, 131, 219, 006, 030, 204, 032, 120, 088, 226, 191, 229, 135, 043, 029, 188, 013, 172, 074, 184, 118, 032, 167, 092, 158, 107, 233, 177, 203, 135, 139, 212, 140, 055, 183, 112, 006, 166, 251, 006, 143, 093, 164, 182, 208, 054, 011, 059, 192, 224, 147, 025, 140, 158, 167, 161, 203, 233, 212, 153, 138, 082, 156, 072, 240, 016, 198, 021, 027, 157, 032, 249, 158, 141, 119, 003, 213, 054, 103, 067, 185, 034, 035, 210, 012, 095, 079, 090, 254, 099, 139, 217, 106, 157, 105, 223, 253, 251, 003, 001, 161, 080, 107, 019, 069, 155, 209, 013, 097, 226, 235, 243, 173, 221, 107, 034, 238, 248, 109, 222, 135, 094, 251, 186, 237, 013, 039, 009, 158, 233, 221, 232, 072, 114, 072, 155, 099, 122, 011, 020, 139, 046, 197, 152, 231, 123, 166, 191, 008, 102, 030, 159, 234, 063, 232, 228, 077, 059, 044, 190, 065, 206, 140, 124, 019, 198, 109, 241, 241, 037, 188, 049, 042, 084, 248, 073, 168, 106, 174, 100, 036, 056, 142, 013, 091, 136, 075, 025, 153, 242, 156, 176, 035, 106, 076, 091, 202, 112, 164, 177, 011, 115, 109, 112, 107, 216, 110, 045, 174, 105, 135, 083, 227, 120, 222, 216, 153, 224, 040, 050, 178, 052, 126, 031, 097, 206, 108, 052, 155, 133, 157, 113, 023, 205, 244, 251, 031, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement myModal;
                public readonly HTMLElement Btn_Close;
                public readonly HTMLDivElement head;
                public readonly HTMLDivElement body;
                public Modal_html() : this(false) { }
                public Modal_html(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        myModal = Document.GetElementById<HTMLDivElement>("myModal");
                        Btn_Close = Document.GetElementById<HTMLElement>("Btn_Close");
                        head = Document.GetElementById<HTMLDivElement>("head");
                        body = Document.GetElementById<HTMLDivElement>("body");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    myModal = doc.GetElementById<HTMLDivElement>("myModal");
                    Btn_Close = doc.GetElementById<HTMLElement>("Btn_Close");
                    head = doc.GetElementById<HTMLDivElement>("head");
                    body = doc.GetElementById<HTMLDivElement>("body");
                    var div = js.Document.CreateElement("Div");
                    _ = div.AppendChild(doc.Body);
                    var Scripts = div.GetElementsByTagName("Script").ToArray();
                    foreach (var Script in Scripts)
                    {
                        var NewScript = js.Document.CreateElement("Script");
                        var Src = Script.GetAttribute("src");
                        if (Src != null)
                            NewScript.SetAttribute("src", Src);
                        NewScript.InnerHtml = Script.InnerHtml;
                        _ = Script.ParentElement.ReplaceChild(NewScript, Script);
                    }
                    div.SetStyleAttribute("display", "none");
                    _ = js.Document.Body.AppendChild(div);
                    _ = js.Document.Body.RemoveChild(div);
                    myModal.Id = "";
                    Btn_Close.Id = "";
                    head.Id = "";
                    body.Id = "";
                }

            }

            namespace Edit
            {
                public class Img_html
                {
                    public static readonly string HtmlText = ((Func<string>)(() =>
                    {
                        byte[] ByteResult = null;
                        var Result = "";
                        {
                            var ZipMemmory = new System.IO.MemoryStream(new byte[] { 133, 082, 065, 106, 195, 048, 016, 188, 231, 021, 075, 014, 137, 013, 197, 238, 189, 178, 032, 165, 045, 004, 218, 075, 160, 167, 210, 131, 098, 109, 108, 129, 045, 007, 105, 157, 018, 074, 254, 094, 173, 237, 186, 137, 219, 208, 189, 172, 053, 154, 025, 205, 202, 018, 037, 213, 149, 020, 037, 042, 045, 069, 218, 183, 109, 163, 143, 082, 104, 115, 000, 163, 179, 023, 101, 172, 004, 094, 073, 097, 234, 130, 161, 117, 173, 010, 012, 108, 198, 250, 029, 016, 198, 238, 091, 002, 058, 238, 049, 219, 153, 010, 071, 218, 083, 088, 132, 237, 129, 059, 052, 159, 059, 179, 039, 057, 131, 161, 014, 202, 001, 123, 103, 160, 155, 188, 173, 209, 082, 082, 032, 061, 086, 200, 159, 247, 199, 181, 142, 230, 157, 217, 060, 190, 187, 208, 140, 039, 252, 171, 100, 210, 185, 122, 004, 147, 198, 230, 165, 178, 005, 091, 068, 049, 100, 018, 062, 103, 035, 139, 203, 236, 032, 250, 097, 243, 108, 030, 022, 011, 152, 064, 111, 183, 239, 113, 080, 194, 164, 056, 164, 011, 119, 138, 046, 216, 091, 252, 000, 086, 108, 058, 032, 010, 105, 126, 241, 123, 110, 200, 084, 053, 074, 007, 201, 174, 181, 057, 153, 198, 066, 132, 127, 217, 119, 249, 234, 034, 241, 072, 043, 034, 103, 182, 045, 097, 180, 244, 046, 095, 222, 000, 038, 164, 092, 184, 139, 196, 161, 111, 043, 058, 155, 253, 187, 078, 215, 003, 112, 091, 249, 007, 069, 234, 117, 243, 060, 029, 159, 103, 189, 116, 059, 205, 206, 060, 185, 137, 116, 248, 197, 034, 237, 031, 083, 218, 061, 179, 047, });
                            var DezippedMemory = new System.IO.MemoryStream();
                            using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                            {
                                Dezip.CopyTo(DezippedMemory);
                            }
                            ByteResult = DezippedMemory.ToArray();
                        }

                        Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                        var Doc = Document.Parse(Result);
                        var Elements = Doc.GetElementsByTagName("*").ToArray();
                        foreach (var Element in Elements)
                        {
                            var MNsrc = Element.GetAttribute("MNsrc");
                            if (MNsrc == "")
                                MNsrc = null;
                            if (MNsrc != null)
                            {
                                Element.RemoveAttribute("MNsrc");
                                var TagName = Element.TagName.ToLower();
                                switch (TagName)
                                {
                                    case "script":
                                        Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        break;
                                    case "img":
                                        Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                        break;
                                    case "link":
                                        var LinkType = Element.GetAttribute("type");
                                        if (LinkType != null)
                                            LinkType = LinkType.ToLower();
                                        var LinkRel = Element.GetAttribute("rel");
                                        if (LinkRel != null)
                                            LinkRel = LinkRel.ToLower();
                                        if (LinkType == "text/css" || LinkRel == "stylesheet")
                                        {
                                            var Style = js.Document.CreateElement<HTMLStyleElement>();
                                            Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                            _ = Element.ParentElement.ReplaceChild(Style, Element);
                                        }
                                        break;
                                }
                            }
                        }
                        Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                        return Result;
                    }))();

                    public readonly HTMLDivElement Main;
                    public readonly HTMLImageElement Image;
                    public readonly HTMLInputElement ImageFile;
                    public Img_html() : this(false) { }
                    public Img_html(bool IsGlobal)
                    {
                        if (IsGlobal == true)
                        {
                            var Document = new Document();
                            Main = Document.GetElementById<HTMLDivElement>("Main");
                            Image = Document.GetElementById<HTMLImageElement>("Image");
                            ImageFile = Document.GetElementById<HTMLInputElement>("ImageFile");
                            return;
                        }
                        var doc = Document.Parse(HtmlText);
                        Main = doc.GetElementById<HTMLDivElement>("Main");
                        Image = doc.GetElementById<HTMLImageElement>("Image");
                        ImageFile = doc.GetElementById<HTMLInputElement>("ImageFile");
                        var div = js.Document.CreateElement("Div");
                        _ = div.AppendChild(doc.Body);
                        var Scripts = div.GetElementsByTagName("Script").ToArray();
                        foreach (var Script in Scripts)
                        {
                            var NewScript = js.Document.CreateElement("Script");
                            var Src = Script.GetAttribute("src");
                            if (Src != null)
                                NewScript.SetAttribute("src", Src);
                            NewScript.InnerHtml = Script.InnerHtml;
                            _ = Script.ParentElement.ReplaceChild(NewScript, Script);
                        }
                        div.SetStyleAttribute("display", "none");
                        _ = js.Document.Body.AppendChild(div);
                        _ = js.Document.Body.RemoveChild(div);
                        Main.Id = "";
                        Image.Id = "";
                        ImageFile.Id = "";
                    }

                }
                public class Input_Count_html
                {
                    public static readonly string HtmlText = ((Func<string>)(() =>
                    {
                        byte[] ByteResult = null;
                        var Result = "";
                        {
                            var ZipMemmory = new System.IO.MemoryStream(new byte[] { 205, 084, 189, 110, 194, 048, 016, 222, 121, 138, 171, 007, 148, 072, 085, 096, 143, 147, 129, 138, 161, 082, 097, 040, 015, 080, 025, 124, 128, 213, 196, 137, 156, 051, 106, 084, 241, 238, 117, 098, 254, 066, 010, 098, 196, 075, 236, 187, 239, 187, 255, 028, 223, 082, 158, 165, 124, 139, 066, 166, 124, 228, 063, 203, 066, 214, 041, 151, 106, 007, 074, 038, 051, 161, 116, 010, 092, 233, 210, 082, 243, 166, 031, 250, 122, 043, 172, 038, 160, 186, 196, 132, 048, 115, 218, 106, 101, 084, 073, 233, 000, 014, 103, 039, 012, 120, 070, 002, 178, 088, 217, 028, 053, 069, 027, 164, 105, 134, 205, 117, 082, 191, 203, 128, 157, 044, 177, 048, 238, 048, 063, 068, 213, 016, 091, 003, 209, 078, 100, 022, 251, 250, 005, 009, 211, 128, 198, 125, 213, 084, 203, 174, 194, 027, 042, 244, 055, 214, 165, 193, 170, 114, 218, 181, 213, 043, 082, 133, 134, 032, 132, 223, 019, 176, 057, 151, 198, 061, 177, 194, 012, 091, 112, 043, 142, 123, 104, 239, 239, 010, 235, 132, 103, 228, 062, 030, 244, 130, 057, 150, 231, 118, 036, 106, 013, 193, 220, 230, 075, 052, 193, 069, 041, 194, 136, 138, 005, 025, 165, 055, 142, 144, 036, 192, 230, 098, 206, 174, 169, 103, 071, 045, 199, 185, 105, 002, 141, 111, 096, 042, 164, 197, 049, 238, 079, 161, 055, 024, 156, 138, 240, 122, 204, 048, 236, 147, 013, 146, 053, 186, 043, 223, 015, 030, 073, 001, 056, 140, 159, 057, 228, 153, 160, 109, 100, 220, 104, 202, 255, 162, 239, 116, 224, 165, 051, 167, 048, 028, 118, 158, 078, 203, 218, 230, 060, 075, 170, 015, 052, 231, 122, 190, 198, 015, 076, 023, 099, 119, 107, 122, 239, 135, 222, 251, 043, 031, 029, 118, 136, 187, 185, 205, 227, 118, 145, 223, 066, 163, 118, 063, 253, 001, });
                            var DezippedMemory = new System.IO.MemoryStream();
                            using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                            {
                                Dezip.CopyTo(DezippedMemory);
                            }
                            ByteResult = DezippedMemory.ToArray();
                        }

                        Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                        var Doc = Document.Parse(Result);
                        var Elements = Doc.GetElementsByTagName("*").ToArray();
                        foreach (var Element in Elements)
                        {
                            var MNsrc = Element.GetAttribute("MNsrc");
                            if (MNsrc == "")
                                MNsrc = null;
                            if (MNsrc != null)
                            {
                                Element.RemoveAttribute("MNsrc");
                                var TagName = Element.TagName.ToLower();
                                switch (TagName)
                                {
                                    case "script":
                                        Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        break;
                                    case "img":
                                        Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                        break;
                                    case "link":
                                        var LinkType = Element.GetAttribute("type");
                                        if (LinkType != null)
                                            LinkType = LinkType.ToLower();
                                        var LinkRel = Element.GetAttribute("rel");
                                        if (LinkRel != null)
                                            LinkRel = LinkRel.ToLower();
                                        if (LinkType == "text/css" || LinkRel == "stylesheet")
                                        {
                                            var Style = js.Document.CreateElement<HTMLStyleElement>();
                                            Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                            _ = Element.ParentElement.ReplaceChild(Style, Element);
                                        }
                                        break;
                                }
                            }
                        }
                        Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                        return Result;
                    }))();

                    public readonly HTMLDivElement Main;
                    public readonly HTMLInputElement txt_Count;
                    public Input_Count_html() : this(false) { }
                    public Input_Count_html(bool IsGlobal)
                    {
                        if (IsGlobal == true)
                        {
                            var Document = new Document();
                            Main = Document.GetElementById<HTMLDivElement>("Main");
                            txt_Count = Document.GetElementById<HTMLInputElement>("txt_Count");
                            return;
                        }
                        var doc = Document.Parse(HtmlText);
                        Main = doc.GetElementById<HTMLDivElement>("Main");
                        txt_Count = doc.GetElementById<HTMLInputElement>("txt_Count");
                        var div = js.Document.CreateElement("Div");
                        _ = div.AppendChild(doc.Body);
                        var Scripts = div.GetElementsByTagName("Script").ToArray();
                        foreach (var Script in Scripts)
                        {
                            var NewScript = js.Document.CreateElement("Script");
                            var Src = Script.GetAttribute("src");
                            if (Src != null)
                                NewScript.SetAttribute("src", Src);
                            NewScript.InnerHtml = Script.InnerHtml;
                            _ = Script.ParentElement.ReplaceChild(NewScript, Script);
                        }
                        div.SetStyleAttribute("display", "none");
                        _ = js.Document.Body.AppendChild(div);
                        _ = js.Document.Body.RemoveChild(div);
                        Main.Id = "";
                        txt_Count.Id = "";
                    }

                }

            }

        }

        namespace Preneeds
        {
            public class Base_htm
            {
                public static readonly string HtmlText = ((Func<string>)(() =>
                {
                    byte[] ByteResult = null;
                    var Result = "";
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 173, 084, 075, 111, 219, 048, 012, 190, 239, 087, 024, 040, 118, 025, 016, 039, 105, 179, 029, 108, 199, 216, 003, 059, 020, 088, 129, 097, 216, 221, 144, 101, 058, 086, 034, 145, 158, 072, 183, 245, 134, 254, 247, 041, 246, 138, 206, 139, 119, 104, 017, 094, 196, 023, 062, 126, 034, 041, 101, 141, 056, 155, 103, 013, 168, 042, 207, 150, 227, 081, 082, 213, 231, 089, 101, 110, 035, 083, 109, 191, 122, 248, 066, 193, 027, 101, 214, 224, 033, 242, 096, 183, 044, 189, 005, 110, 000, 036, 114, 200, 094, 111, 111, 008, 089, 237, 193, 021, 215, 168, 057, 254, 006, 076, 157, 215, 192, 241, 071, 197, 016, 007, 000, 004, 168, 056, 190, 187, 042, 052, 243, 025, 128, 190, 195, 189, 092, 099, 219, 073, 081, 042, 107, 137, 240, 236, 184, 159, 008, 069, 025, 004, 127, 038, 228, 146, 072, 088, 188, 106, 011, 103, 030, 217, 178, 246, 166, 125, 030, 204, 254, 071, 007, 190, 047, 214, 197, 250, 178, 216, 012, 080, 123, 014, 083, 027, 145, 094, 006, 249, 196, 012, 073, 076, 221, 207, 161, 030, 239, 156, 191, 138, 254, 146, 178, 019, 033, 156, 184, 126, 077, 172, 163, 104, 178, 228, 147, 210, 042, 125, 072, 039, 193, 135, 137, 245, 254, 000, 125, 237, 149, 003, 142, 224, 094, 185, 214, 194, 012, 086, 237, 201, 205, 184, 143, 066, 173, 210, 070, 250, 036, 090, 165, 179, 241, 059, 083, 073, 019, 162, 039, 193, 135, 127, 072, 077, 204, 055, 051, 213, 020, 026, 167, 196, 016, 046, 048, 208, 077, 030, 233, 158, 150, 125, 074, 172, 058, 063, 040, 161, 126, 252, 150, 079, 051, 235, 176, 104, 011, 054, 063, 033, 185, 188, 013, 157, 255, 095, 159, 194, 052, 134, 033, 004, 037, 060, 203, 112, 252, 121, 156, 055, 097, 073, 063, 091, 112, 128, 018, 013, 025, 091, 009, 059, 188, 080, 214, 236, 048, 209, 193, 011, 062, 029, 199, 112, 081, 215, 117, 090, 134, 081, 236, 060, 117, 088, 037, 023, 235, 171, 205, 135, 205, 187, 116, 236, 206, 122, 181, 122, 157, 054, 096, 118, 141, 140, 122, 062, 022, 202, 150, 227, 095, 176, 028, 126, 137, 223, });
                        var DezippedMemory = new System.IO.MemoryStream();
                        using (var Dezip = new System.IO.Compression.DeflateStream(ZipMemmory, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Dezip.CopyTo(DezippedMemory);
                        }
                        ByteResult = DezippedMemory.ToArray();
                    }

                    Result = System.Text.Encoding.UTF8.GetString(ByteResult);
                    var Doc = Document.Parse(Result);
                    var Elements = Doc.GetElementsByTagName("*").ToArray();
                    foreach (var Element in Elements)
                    {
                        var MNsrc = Element.GetAttribute("MNsrc");
                        if (MNsrc == "")
                            MNsrc = null;
                        if (MNsrc != null)
                        {
                            Element.RemoveAttribute("MNsrc");
                            var TagName = Element.TagName.ToLower();
                            switch (TagName)
                            {
                                case "script":
                                    Element.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                    break;
                                case "img":
                                    Element.SetAttribute("src", (string)Type.GetType(MNsrc).GetField("Url").GetValue(null));
                                    break;
                                case "link":
                                    var LinkType = Element.GetAttribute("type");
                                    if (LinkType != null)
                                        LinkType = LinkType.ToLower();
                                    var LinkRel = Element.GetAttribute("rel");
                                    if (LinkRel != null)
                                        LinkRel = LinkRel.ToLower();
                                    if (LinkType == "text/css" || LinkRel == "stylesheet")
                                    {
                                        var Style = js.Document.CreateElement<HTMLStyleElement>();
                                        Style.InnerHtml = (string)Type.GetType(MNsrc).GetField("TextContent").GetValue(null);
                                        _ = Element.ParentElement.ReplaceChild(Style, Element);
                                    }
                                    break;
                            }
                        }
                    }
                    Result = "<html>\"" + "<head>" + Doc.GetElementsByTagName("head")[0].InnerHtml + "</head>" + "<body>" + Doc.GetElementsByTagName("body")[0].InnerHtml + "</body></html>";
                    return Result;
                }))();

                public readonly HTMLDivElement PreLoad;
                public readonly HTMLDivElement MainElement;
                public Base_htm() : this(false) { }
                public Base_htm(bool IsGlobal)
                {
                    if (IsGlobal == true)
                    {
                        var Document = new Document();
                        PreLoad = Document.GetElementById<HTMLDivElement>("PreLoad");
                        MainElement = Document.GetElementById<HTMLDivElement>("MainElement");
                        return;
                    }
                    var doc = Document.Parse(HtmlText);
                    PreLoad = doc.GetElementById<HTMLDivElement>("PreLoad");
                    MainElement = doc.GetElementById<HTMLDivElement>("MainElement");
                    var div = js.Document.CreateElement("Div");
                    _ = div.AppendChild(doc.Body);
                    var Scripts = div.GetElementsByTagName("Script").ToArray();
                    foreach (var Script in Scripts)
                    {
                        var NewScript = js.Document.CreateElement("Script");
                        var Src = Script.GetAttribute("src");
                        if (Src != null)
                            NewScript.SetAttribute("src", Src);
                        NewScript.InnerHtml = Script.InnerHtml;
                        _ = Script.ParentElement.ReplaceChild(NewScript, Script);
                    }
                    div.SetStyleAttribute("display", "none");
                    _ = js.Document.Body.AppendChild(div);
                    _ = js.Document.Body.RemoveChild(div);
                    PreLoad.Id = "";
                    MainElement.Id = "";
                }

            }
            public class bootstrap_notify_min_js
            {
                public static readonly string TextContent = ((Func<string>)(() =>
                {
                    byte[] Result = null;
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 197, 090, 123, 115, 219, 054, 018, 255, 042, 052, 238, 098, 147, 021, 068, 203, 077, 210, 155, 161, 076, 123, 156, 071, 175, 158, 073, 175, 109, 114, 253, 043, 245, 120, 032, 017, 146, 208, 080, 160, 074, 064, 177, 092, 153, 223, 253, 118, 001, 062, 064, 138, 082, 156, 199, 204, 101, 060, 018, 009, 044, 022, 139, 125, 252, 118, 023, 202, 209, 108, 045, 167, 090, 100, 210, 215, 193, 150, 084, 047, 036, 142, 245, 253, 138, 103, 051, 047, 225, 051, 033, 249, 241, 177, 253, 014, 217, 050, 185, 180, 143, 254, 123, 242, 231, 095, 107, 158, 223, 147, 027, 170, 131, 072, 251, 036, 155, 252, 201, 167, 186, 089, 202, 055, 171, 044, 215, 234, 050, 231, 127, 173, 069, 206, 253, 138, 062, 136, 254, 252, 013, 031, 130, 194, 119, 055, 175, 158, 061, 229, 171, 096, 251, 145, 229, 030, 143, 143, 206, 198, 057, 215, 235, 092, 122, 218, 063, 121, 159, 048, 205, 134, 050, 211, 098, 118, 031, 147, 105, 038, 053, 003, 057, 114, 114, 115, 018, 132, 156, 077, 023, 013, 055, 065, 165, 229, 192, 098, 237, 203, 128, 102, 049, 011, 065, 230, 164, 203, 067, 011, 157, 114, 179, 094, 243, 141, 246, 225, 043, 023, 075, 063, 160, 249, 030, 250, 037, 087, 138, 205, 237, 138, 133, 094, 166, 205, 138, 052, 206, 098, 056, 184, 079, 206, 019, 241, 241, 130, 012, 084, 168, 184, 214, 066, 206, 085, 136, 130, 114, 169, 067, 179, 217, 128, 156, 159, 026, 138, 046, 131, 036, 206, 063, 197, 160, 220, 125, 047, 139, 057, 072, 189, 096, 234, 101, 202, 148, 242, 009, 075, 121, 174, 135, 045, 070, 104, 151, 160, 082, 104, 010, 054, 061, 062, 158, 031, 031, 251, 160, 230, 081, 064, 143, 120, 017, 080, 094, 212, 086, 224, 062, 167, 146, 050, 171, 199, 044, 222, 150, 082, 068, 219, 082, 140, 104, 199, 222, 242, 082, 086, 050, 070, 146, 154, 227, 070, 210, 030, 251, 178, 252, 142, 008, 161, 002, 056, 193, 056, 126, 093, 218, 047, 028, 093, 231, 041, 012, 194, 231, 165, 249, 140, 200, 063, 008, 213, 044, 159, 115, 141, 060, 204, 195, 101, 245, 016, 145, 033, 041, 138, 049, 024, 055, 004, 179, 113, 176, 211, 209, 136, 110, 011, 154, 129, 184, 084, 047, 068, 115, 228, 046, 133, 168, 041, 110, 193, 139, 217, 058, 213, 042, 022, 020, 216, 161, 238, 221, 133, 141, 213, 204, 142, 160, 165, 067, 211, 157, 181, 032, 255, 173, 157, 040, 055, 099, 082, 044, 025, 106, 085, 197, 091, 005, 051, 112, 130, 059, 062, 249, 032, 244, 085, 053, 241, 014, 071, 189, 172, 033, 052, 239, 063, 191, 235, 016, 180, 231, 009, 133, 147, 237, 176, 122, 045, 019, 135, 017, 080, 184, 108, 112, 210, 157, 035, 005, 037, 114, 189, 156, 064, 024, 213, 118, 108, 031, 038, 155, 205, 084, 143, 002, 236, 112, 188, 221, 068, 125, 227, 244, 190, 119, 024, 060, 172, 195, 134, 165, 105, 118, 119, 155, 172, 087, 169, 152, 050, 205, 213, 195, 195, 209, 097, 130, 227, 227, 035, 101, 120, 004, 193, 241, 177, 033, 021, 082, 064, 232, 022, 232, 166, 034, 222, 242, 148, 047, 209, 077, 201, 036, 075, 238, 009, 093, 101, 074, 224, 081, 035, 185, 078, 083, 138, 007, 140, 136, 144, 179, 140, 208, 146, 177, 080, 075, 161, 084, 004, 238, 209, 221, 009, 199, 036, 191, 227, 074, 223, 102, 242, 086, 103, 171, 232, 232, 140, 170, 069, 118, 247, 107, 158, 205, 115, 240, 243, 009, 203, 113, 104, 149, 178, 169, 221, 115, 059, 203, 179, 101, 068, 128, 020, 217, 139, 057, 056, 118, 046, 230, 011, 013, 074, 182, 231, 143, 190, 031, 081, 181, 098, 083, 056, 089, 116, 054, 162, 127, 223, 002, 198, 240, 013, 060, 062, 061, 163, 009, 079, 217, 125, 244, 156, 063, 133, 200, 089, 242, 060, 058, 131, 167, 198, 145, 034, 114, 059, 073, 153, 252, 064, 232, 050, 091, 043, 126, 155, 125, 004, 018, 115, 038, 107, 077, 030, 109, 065, 002, 024, 035, 229, 123, 226, 205, 088, 194, 175, 229, 171, 236, 078, 130, 159, 108, 132, 238, 076, 253, 178, 214, 191, 175, 080, 050, 249, 014, 206, 100, 121, 217, 103, 089, 189, 188, 076, 051, 197, 091, 047, 137, 125, 195, 160, 189, 181, 202, 156, 034, 218, 064, 168, 242, 037, 232, 001, 196, 056, 065, 252, 242, 246, 032, 181, 103, 168, 113, 036, 029, 110, 212, 240, 236, 204, 195, 039, 181, 028, 062, 243, 012, 092, 217, 207, 225, 118, 084, 016, 047, 207, 082, 030, 091, 020, 035, 023, 231, 147, 181, 214, 000, 074, 184, 103, 076, 236, 011, 241, 088, 046, 216, 112, 033, 146, 132, 075, 192, 242, 124, 205, 155, 013, 080, 088, 210, 022, 163, 052, 053, 185, 056, 070, 005, 171, 241, 249, 169, 229, 115, 113, 014, 038, 145, 109, 090, 060, 032, 236, 122, 138, 051, 023, 094, 015, 129, 205, 028, 023, 219, 179, 226, 000, 081, 149, 046, 046, 182, 223, 087, 100, 070, 059, 165, 144, 171, 210, 143, 058, 114, 174, 026, 247, 034, 189, 244, 067, 152, 241, 220, 023, 087, 097, 238, 106, 171, 160, 143, 044, 093, 115, 153, 221, 197, 100, 228, 142, 044, 133, 236, 142, 176, 077, 076, 206, 070, 048, 166, 244, 061, 242, 186, 019, 137, 094, 068, 222, 232, 201, 024, 117, 129, 057, 167, 252, 100, 222, 034, 231, 179, 152, 108, 159, 194, 190, 037, 004, 146, 237, 179, 162, 115, 018, 112, 095, 092, 200, 202, 101, 039, 197, 248, 029, 164, 042, 057, 015, 103, 089, 014, 142, 024, 215, 249, 026, 146, 127, 150, 251, 024, 193, 058, 006, 110, 107, 140, 038, 245, 126, 116, 067, 085, 124, 054, 086, 231, 245, 080, 152, 114, 057, 215, 139, 177, 026, 012, 002, 000, 221, 048, 231, 038, 248, 252, 183, 124, 254, 122, 179, 242, 201, 031, 127, 108, 201, 192, 007, 199, 010, 006, 240, 092, 016, 074, 230, 075, 018, 208, 134, 165, 186, 169, 243, 159, 046, 104, 157, 028, 120, 008, 106, 211, 025, 122, 023, 221, 034, 158, 068, 142, 104, 086, 044, 132, 154, 177, 193, 155, 201, 090, 164, 201, 127, 204, 017, 253, 078, 186, 169, 211, 002, 250, 079, 009, 079, 048, 119, 061, 069, 070, 020, 082, 218, 209, 158, 044, 003, 154, 170, 200, 081, 245, 191, 191, 125, 083, 179, 198, 247, 087, 214, 117, 171, 177, 026, 113, 170, 129, 009, 214, 042, 229, 115, 169, 252, 237, 063, 001, 009, 045, 008, 227, 019, 093, 175, 018, 012, 207, 250, 092, 138, 114, 123, 052, 192, 204, 098, 076, 148, 049, 076, 147, 003, 212, 165, 000, 101, 197, 060, 018, 177, 026, 087, 198, 145, 158, 128, 191, 064, 221, 009, 013, 213, 022, 148, 088, 083, 006, 081, 134, 043, 072, 179, 019, 024, 101, 009, 232, 212, 169, 066, 116, 167, 010, 161, 013, 125, 095, 161, 229, 122, 241, 141, 119, 225, 133, 174, 187, 067, 241, 213, 218, 163, 021, 010, 125, 091, 181, 007, 098, 241, 094, 222, 056, 251, 179, 036, 233, 008, 139, 004, 193, 023, 201, 213, 240, 234, 008, 101, 088, 142, 039, 057, 103, 031, 198, 070, 107, 006, 097, 162, 178, 070, 061, 168, 011, 067, 009, 021, 231, 184, 196, 090, 044, 085, 154, 019, 213, 080, 028, 234, 236, 077, 118, 199, 243, 151, 192, 221, 015, 046, 089, 075, 071, 186, 223, 069, 029, 129, 141, 128, 145, 207, 066, 001, 194, 139, 229, 156, 004, 015, 015, 101, 017, 108, 095, 041, 011, 153, 214, 185, 079, 084, 062, 133, 034, 014, 201, 091, 007, 170, 209, 044, 178, 005, 163, 179, 165, 073, 109, 195, 238, 192, 119, 102, 207, 083, 000, 156, 096, 220, 104, 000, 143, 238, 019, 123, 246, 161, 161, 035, 052, 251, 092, 119, 001, 180, 065, 099, 024, 113, 091, 024, 088, 010, 030, 078, 209, 068, 006, 222, 236, 200, 128, 060, 033, 173, 211, 032, 116, 069, 135, 055, 069, 146, 155, 122, 027, 004, 196, 146, 187, 203, 199, 130, 227, 231, 177, 042, 215, 180, 152, 149, 213, 234, 039, 248, 156, 012, 228, 224, 164, 233, 078, 012, 003, 083, 024, 229, 142, 143, 101, 107, 168, 019, 126, 226, 088, 152, 248, 193, 096, 197, 114, 005, 101, 130, 118, 093, 164, 044, 081, 250, 039, 109, 037, 019, 222, 131, 209, 016, 127, 203, 250, 202, 207, 131, 130, 154, 172, 235, 066, 167, 014, 205, 016, 084, 103, 069, 065, 029, 220, 236, 194, 171, 234, 135, 197, 198, 045, 160, 049, 106, 165, 142, 078, 021, 089, 085, 031, 029, 052, 054, 120, 174, 108, 231, 001, 223, 101, 058, 166, 166, 064, 199, 113, 091, 162, 187, 206, 101, 013, 224, 232, 116, 088, 029, 144, 116, 120, 215, 024, 028, 098, 209, 055, 032, 008, 060, 123, 008, 076, 041, 216, 205, 020, 173, 242, 243, 225, 225, 176, 093, 171, 210, 005, 045, 107, 092, 023, 006, 086, 038, 052, 032, 082, 036, 039, 059, 101, 181, 009, 156, 243, 120, 004, 213, 114, 123, 162, 083, 191, 238, 148, 219, 157, 249, 170, 198, 126, 092, 228, 213, 192, 012, 006, 167, 101, 222, 115, 077, 237, 000, 088, 107, 211, 125, 024, 246, 056, 092, 108, 048, 108, 127, 046, 014, 062, 017, 056, 053, 175, 026, 253, 030, 189, 121, 131, 137, 223, 096, 123, 182, 090, 097, 061, 114, 114, 014, 050, 120, 192, 020, 067, 122, 063, 091, 136, 117, 168, 152, 161, 244, 178, 081, 229, 161, 190, 137, 119, 122, 113, 130, 234, 119, 234, 134, 086, 072, 126, 150, 163, 109, 235, 254, 137, 176, 137, 202, 082, 128, 014, 066, 077, 079, 019, 065, 165, 184, 218, 192, 161, 161, 047, 034, 207, 241, 233, 239, 107, 211, 204, 180, 197, 045, 091, 156, 193, 247, 069, 037, 019, 212, 054, 143, 151, 167, 002, 070, 035, 203, 132, 077, 063, 204, 243, 108, 045, 147, 235, 165, 185, 128, 128, 089, 031, 201, 035, 129, 239, 167, 115, 049, 027, 079, 192, 115, 126, 120, 070, 223, 142, 210, 127, 255, 242, 042, 093, 092, 253, 118, 245, 226, 234, 250, 202, 254, 251, 245, 244, 244, 244, 254, 167, 231, 047, 174, 094, 155, 215, 055, 118, 244, 197, 149, 121, 191, 126, 241, 246, 234, 234, 095, 001, 161, 011, 094, 029, 111, 244, 132, 208, 148, 207, 116, 052, 162, 125, 106, 192, 147, 143, 168, 173, 145, 075, 234, 131, 042, 056, 067, 021, 052, 205, 098, 047, 004, 082, 222, 009, 141, 010, 109, 041, 148, 105, 101, 208, 099, 003, 155, 066, 059, 053, 156, 164, 217, 020, 027, 066, 128, 049, 001, 146, 129, 057, 060, 182, 214, 153, 211, 244, 118, 016, 169, 028, 190, 236, 031, 046, 091, 230, 157, 232, 044, 059, 234, 075, 050, 019, 027, 158, 144, 150, 010, 114, 038, 107, 197, 164, 169, 023, 062, 087, 030, 007, 011, 012, 133, 028, 066, 154, 057, 172, 144, 130, 202, 024, 218, 103, 214, 222, 110, 092, 086, 151, 157, 251, 190, 026, 135, 119, 035, 226, 115, 145, 024, 051, 100, 004, 092, 125, 203, 031, 051, 020, 144, 149, 013, 228, 077, 176, 115, 153, 024, 108, 121, 252, 051, 211, 139, 016, 250, 035, 159, 211, 038, 039, 218, 107, 008, 227, 155, 172, 035, 069, 224, 230, 206, 146, 174, 149, 119, 157, 121, 086, 231, 219, 160, 232, 102, 137, 214, 253, 003, 088, 230, 104, 100, 238, 235, 250, 125, 036, 128, 138, 225, 144, 106, 160, 156, 031, 016, 019, 179, 007, 019, 149, 173, 234, 209, 241, 073, 100, 030, 237, 029, 070, 180, 159, 185, 089, 119, 211, 047, 213, 198, 108, 233, 150, 068, 083, 115, 081, 001, 252, 066, 220, 035, 030, 081, 017, 154, 029, 226, 081, 209, 128, 001, 042, 085, 236, 133, 246, 242, 034, 035, 052, 156, 176, 192, 055, 022, 187, 202, 115, 118, 239, 151, 215, 097, 067, 200, 141, 203, 236, 111, 252, 202, 204, 179, 194, 079, 200, 148, 205, 149, 051, 118, 064, 118, 059, 104, 053, 109, 163, 245, 030, 052, 084, 223, 149, 093, 003, 115, 243, 240, 018, 048, 071, 147, 155, 248, 012, 237, 227, 247, 070, 071, 013, 221, 245, 017, 030, 105, 202, 218, 013, 184, 235, 050, 237, 156, 092, 149, 099, 123, 106, 056, 187, 145, 083, 139, 113, 172, 105, 032, 149, 253, 088, 055, 123, 142, 089, 204, 021, 015, 228, 245, 157, 177, 112, 010, 033, 188, 035, 191, 221, 078, 114, 191, 115, 145, 025, 154, 027, 072, 234, 068, 009, 068, 243, 168, 008, 122, 105, 065, 049, 046, 101, 079, 163, 120, 200, 184, 135, 014, 034, 251, 078, 034, 155, 163, 096, 072, 193, 236, 127, 197, 146, 131, 206, 220, 160, 150, 080, 116, 125, 037, 107, 250, 003, 244, 046, 080, 216, 002, 156, 245, 195, 249, 088, 204, 026, 133, 126, 050, 225, 194, 098, 168, 144, 004, 098, 122, 075, 089, 085, 013, 237, 090, 196, 220, 002, 226, 037, 160, 123, 164, 010, 105, 108, 015, 101, 246, 089, 032, 013, 056, 190, 001, 055, 096, 081, 046, 108, 171, 226, 192, 186, 025, 075, 149, 089, 232, 236, 189, 159, 172, 227, 242, 166, 010, 189, 024, 213, 006, 239, 235, 237, 186, 228, 193, 216, 254, 242, 131, 229, 035, 058, 000, 116, 110, 254, 206, 101, 076, 029, 040, 251, 025, 007, 193, 208, 237, 004, 240, 074, 021, 173, 081, 074, 010, 241, 183, 239, 048, 096, 117, 178, 098, 235, 138, 168, 102, 209, 220, 187, 062, 060, 148, 004, 071, 253, 243, 213, 189, 138, 223, 061, 219, 080, 007, 167, 221, 177, 239, 160, 114, 024, 031, 208, 143, 006, 255, 253, 054, 141, 111, 167, 235, 053, 045, 111, 161, 207, 227, 029, 061, 001, 046, 077, 083, 206, 242, 090, 255, 028, 101, 040, 189, 016, 203, 184, 014, 061, 116, 119, 187, 173, 095, 171, 166, 105, 003, 091, 013, 240, 135, 146, 021, 064, 152, 192, 159, 255, 122, 123, 179, 050, 103, 215, 126, 253, 201, 060, 177, 017, 070, 143, 045, 140, 060, 128, 044, 230, 174, 187, 027, 254, 102, 240, 107, 065, 082, 060, 026, 036, 171, 160, 172, 058, 170, 079, 137, 155, 244, 202, 155, 060, 006, 010, 197, 195, 131, 223, 194, 100, 063, 160, 061, 172, 122, 249, 151, 011, 131, 026, 013, 027, 029, 059, 151, 141, 213, 239, 185, 198, 031, 068, 252, 255, 042, 237, 160, 226, 108, 236, 037, 249, 070, 095, 129, 110, 068, 121, 253, 116, 056, 083, 187, 043, 087, 057, 255, 104, 087, 006, 084, 238, 084, 139, 110, 093, 200, 247, 157, 133, 042, 080, 177, 131, 099, 078, 242, 231, 125, 153, 191, 175, 134, 132, 126, 002, 113, 185, 186, 241, 117, 010, 027, 085, 065, 016, 028, 197, 179, 158, 070, 113, 180, 186, 251, 022, 229, 154, 162, 094, 253, 170, 250, 053, 212, 181, 089, 069, 189, 251, 043, 042, 038, 191, 106, 169, 241, 131, 214, 058, 114, 199, 114, 105, 175, 147, 099, 005, 186, 083, 144, 231, 152, 156, 035, 184, 082, 002, 237, 155, 249, 255, 003, 137, 115, 217, 012, 136, 010, 094, 106, 200, 047, 181, 079, 092, 239, 184, 033, 253, 055, 177, 110, 226, 212, 080, 063, 002, 247, 042, 123, 006, 017, 081, 235, 233, 148, 219, 171, 006, 100, 110, 126, 241, 043, 159, 093, 217, 224, 181, 020, 172, 218, 058, 172, 127, 054, 031, 124, 189, 024, 200, 241, 091, 240, 057, 212, 011, 169, 242, 194, 239, 115, 153, 118, 236, 247, 122, 051, 229, 043, 253, 120, 043, 126, 177, 130, 187, 202, 192, 104, 117, 180, 254, 069, 218, 233, 101, 250, 005, 172, 032, 156, 198, 255, 003, });
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
            public class bootstrap_min_css
            {
                public static readonly string TextContent =
        @"/*!
 * Bootstrap v3.3.1 (http://getbootstrap.com)
 * Copyright 2011-2014 Twitter, Inc.
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 *//*! normalize.css v3.0.2 | MIT License | git.io/normalize */html{font-family:sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}body{margin:0}article,aside,details,figcaption,figure,footer,header,hgroup,main,menu,nav,section,summary{display:block}audio,canvas,progress,video{display:inline-block;vertical-align:baseline}audio:not([controls]){display:none;height:0}[hidden],template{display:none}a{background-color:transparent}a:active,a:hover{outline:0}abbr[title]{border-bottom:1px dotted}b,strong{font-weight:700}dfn{font-style:italic}h1{margin:.67em 0;font-size:2em}mark{color:#000;background:#ff0}small{font-size:80%}sub,sup{position:relative;font-size:75%;line-height:0;vertical-align:baseline}sup{top:-.5em}sub{bottom:-.25em}img{border:0}svg:not(:root){overflow:hidden}figure{margin:1em 40px}hr{height:0;-webkit-box-sizing:content-box;-moz-box-sizing:content-box;box-sizing:content-box}pre{overflow:auto}code,kbd,pre,samp{font-family:monospace,monospace;font-size:1em}button,input,optgroup,select,textarea{margin:0;font:inherit;color:inherit}button{overflow:visible}button,select{text-transform:none}button,html input[type=button],input[type=reset],input[type=submit]{-webkit-appearance:button;cursor:pointer}button[disabled],html input[disabled]{cursor:default}button::-moz-focus-inner,input::-moz-focus-inner{padding:0;border:0}input{line-height:normal}input[type=checkbox],input[type=radio]{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;padding:0}input[type=number]::-webkit-inner-spin-button,input[type=number]::-webkit-outer-spin-button{height:auto}input[type=search]{-webkit-box-sizing:content-box;-moz-box-sizing:content-box;box-sizing:content-box;-webkit-appearance:textfield}input[type=search]::-webkit-search-cancel-button,input[type=search]::-webkit-search-decoration{-webkit-appearance:none}fieldset{padding:.35em .625em .75em;margin:0 2px;border:1px solid silver}legend{padding:0;border:0}textarea{overflow:auto}optgroup{font-weight:700}table{border-spacing:0;border-collapse:collapse}td,th{padding:0}/*! Source: https://github.com/h5bp/html5-boilerplate/blob/master/src/css/main.css */@media print{*,:before,:after{color:#000!important;text-shadow:none!important;background:transparent!important;-webkit-box-shadow:none!important;box-shadow:none!important}a,a:visited{text-decoration:underline}a[href]:after{content:"" ("" attr(href) "")""}abbr[title]:after{content:"" ("" attr(title) "")""}a[href^=""#""]:after,a[href^=""javascript:""]:after{content:""""}pre,blockquote{border:1px solid #999;page-break-inside:avoid}thead{display:table-header-group}tr,img{page-break-inside:avoid}img{max-width:100%!important}p,h2,h3{orphans:3;widows:3}h2,h3{page-break-after:avoid}select{background:#fff!important}.navbar{display:none}.btn>.caret,.dropup>.btn>.caret{border-top-color:#000!important}.label{border:1px solid #000}.table{border-collapse:collapse!important}.table td,.table th{background-color:#fff!important}.table-bordered th,.table-bordered td{border:1px solid #ddd!important}}@font-face{font-family:'Glyphicons Halflings';src:url(../fonts/glyphicons-halflings-regular.eot);src:url(../fonts/glyphicons-halflings-regular.eot?#iefix) format('embedded-opentype'),url(../fonts/glyphicons-halflings-regular.woff) format('woff'),url(../fonts/glyphicons-halflings-regular.ttf) format('truetype'),url(../fonts/glyphicons-halflings-regular.svg#glyphicons_halflingsregular) format('svg')}.glyphicon{position:relative;top:1px;display:inline-block;font-family:'Glyphicons Halflings';font-style:normal;font-weight:400;line-height:1;-webkit-font-smoothing:antialiased;-moz-osx-font-smoothing:grayscale}.glyphicon-asterisk:before{content:""\2a""}.glyphicon-plus:before{content:""\2b""}.glyphicon-euro:before,.glyphicon-eur:before{content:""\20ac""}.glyphicon-minus:before{content:""\2212""}.glyphicon-cloud:before{content:""\2601""}.glyphicon-envelope:before{content:""\2709""}.glyphicon-pencil:before{content:""\270f""}.glyphicon-glass:before{content:""\e001""}.glyphicon-music:before{content:""\e002""}.glyphicon-search:before{content:""\e003""}.glyphicon-heart:before{content:""\e005""}.glyphicon-star:before{content:""\e006""}.glyphicon-star-empty:before{content:""\e007""}.glyphicon-user:before{content:""\e008""}.glyphicon-film:before{content:""\e009""}.glyphicon-th-large:before{content:""\e010""}.glyphicon-th:before{content:""\e011""}.glyphicon-th-list:before{content:""\e012""}.glyphicon-ok:before{content:""\e013""}.glyphicon-remove:before{content:""\e014""}.glyphicon-zoom-in:before{content:""\e015""}.glyphicon-zoom-out:before{content:""\e016""}.glyphicon-off:before{content:""\e017""}.glyphicon-signal:before{content:""\e018""}.glyphicon-cog:before{content:""\e019""}.glyphicon-trash:before{content:""\e020""}.glyphicon-home:before{content:""\e021""}.glyphicon-file:before{content:""\e022""}.glyphicon-time:before{content:""\e023""}.glyphicon-road:before{content:""\e024""}.glyphicon-download-alt:before{content:""\e025""}.glyphicon-download:before{content:""\e026""}.glyphicon-upload:before{content:""\e027""}.glyphicon-inbox:before{content:""\e028""}.glyphicon-play-circle:before{content:""\e029""}.glyphicon-repeat:before{content:""\e030""}.glyphicon-refresh:before{content:""\e031""}.glyphicon-list-alt:before{content:""\e032""}.glyphicon-lock:before{content:""\e033""}.glyphicon-flag:before{content:""\e034""}.glyphicon-headphones:before{content:""\e035""}.glyphicon-volume-off:before{content:""\e036""}.glyphicon-volume-down:before{content:""\e037""}.glyphicon-volume-up:before{content:""\e038""}.glyphicon-qrcode:before{content:""\e039""}.glyphicon-barcode:before{content:""\e040""}.glyphicon-tag:before{content:""\e041""}.glyphicon-tags:before{content:""\e042""}.glyphicon-book:before{content:""\e043""}.glyphicon-bookmark:before{content:""\e044""}.glyphicon-print:before{content:""\e045""}.glyphicon-camera:before{content:""\e046""}.glyphicon-font:before{content:""\e047""}.glyphicon-bold:before{content:""\e048""}.glyphicon-italic:before{content:""\e049""}.glyphicon-text-height:before{content:""\e050""}.glyphicon-text-width:before{content:""\e051""}.glyphicon-align-left:before{content:""\e052""}.glyphicon-align-center:before{content:""\e053""}.glyphicon-align-right:before{content:""\e054""}.glyphicon-align-justify:before{content:""\e055""}.glyphicon-list:before{content:""\e056""}.glyphicon-indent-left:before{content:""\e057""}.glyphicon-indent-right:before{content:""\e058""}.glyphicon-facetime-video:before{content:""\e059""}.glyphicon-picture:before{content:""\e060""}.glyphicon-map-marker:before{content:""\e062""}.glyphicon-adjust:before{content:""\e063""}.glyphicon-tint:before{content:""\e064""}.glyphicon-edit:before{content:""\e065""}.glyphicon-share:before{content:""\e066""}.glyphicon-check:before{content:""\e067""}.glyphicon-move:before{content:""\e068""}.glyphicon-step-backward:before{content:""\e069""}.glyphicon-fast-backward:before{content:""\e070""}.glyphicon-backward:before{content:""\e071""}.glyphicon-play:before{content:""\e072""}.glyphicon-pause:before{content:""\e073""}.glyphicon-stop:before{content:""\e074""}.glyphicon-forward:before{content:""\e075""}.glyphicon-fast-forward:before{content:""\e076""}.glyphicon-step-forward:before{content:""\e077""}.glyphicon-eject:before{content:""\e078""}.glyphicon-chevron-left:before{content:""\e079""}.glyphicon-chevron-right:before{content:""\e080""}.glyphicon-plus-sign:before{content:""\e081""}.glyphicon-minus-sign:before{content:""\e082""}.glyphicon-remove-sign:before{content:""\e083""}.glyphicon-ok-sign:before{content:""\e084""}.glyphicon-question-sign:before{content:""\e085""}.glyphicon-info-sign:before{content:""\e086""}.glyphicon-screenshot:before{content:""\e087""}.glyphicon-remove-circle:before{content:""\e088""}.glyphicon-ok-circle:before{content:""\e089""}.glyphicon-ban-circle:before{content:""\e090""}.glyphicon-arrow-left:before{content:""\e091""}.glyphicon-arrow-right:before{content:""\e092""}.glyphicon-arrow-up:before{content:""\e093""}.glyphicon-arrow-down:before{content:""\e094""}.glyphicon-share-alt:before{content:""\e095""}.glyphicon-resize-full:before{content:""\e096""}.glyphicon-resize-small:before{content:""\e097""}.glyphicon-exclamation-sign:before{content:""\e101""}.glyphicon-gift:before{content:""\e102""}.glyphicon-leaf:before{content:""\e103""}.glyphicon-fire:before{content:""\e104""}.glyphicon-eye-open:before{content:""\e105""}.glyphicon-eye-close:before{content:""\e106""}.glyphicon-warning-sign:before{content:""\e107""}.glyphicon-plane:before{content:""\e108""}.glyphicon-calendar:before{content:""\e109""}.glyphicon-random:before{content:""\e110""}.glyphicon-comment:before{content:""\e111""}.glyphicon-magnet:before{content:""\e112""}.glyphicon-chevron-up:before{content:""\e113""}.glyphicon-chevron-down:before{content:""\e114""}.glyphicon-retweet:before{content:""\e115""}.glyphicon-shopping-cart:before{content:""\e116""}.glyphicon-folder-close:before{content:""\e117""}.glyphicon-folder-open:before{content:""\e118""}.glyphicon-resize-vertical:before{content:""\e119""}.glyphicon-resize-horizontal:before{content:""\e120""}.glyphicon-hdd:before{content:""\e121""}.glyphicon-bullhorn:before{content:""\e122""}.glyphicon-bell:before{content:""\e123""}.glyphicon-certificate:before{content:""\e124""}.glyphicon-thumbs-up:before{content:""\e125""}.glyphicon-thumbs-down:before{content:""\e126""}.glyphicon-hand-right:before{content:""\e127""}.glyphicon-hand-left:before{content:""\e128""}.glyphicon-hand-up:before{content:""\e129""}.glyphicon-hand-down:before{content:""\e130""}.glyphicon-circle-arrow-right:before{content:""\e131""}.glyphicon-circle-arrow-left:before{content:""\e132""}.glyphicon-circle-arrow-up:before{content:""\e133""}.glyphicon-circle-arrow-down:before{content:""\e134""}.glyphicon-globe:before{content:""\e135""}.glyphicon-wrench:before{content:""\e136""}.glyphicon-tasks:before{content:""\e137""}.glyphicon-filter:before{content:""\e138""}.glyphicon-briefcase:before{content:""\e139""}.glyphicon-fullscreen:before{content:""\e140""}.glyphicon-dashboard:before{content:""\e141""}.glyphicon-paperclip:before{content:""\e142""}.glyphicon-heart-empty:before{content:""\e143""}.glyphicon-link:before{content:""\e144""}.glyphicon-phone:before{content:""\e145""}.glyphicon-pushpin:before{content:""\e146""}.glyphicon-usd:before{content:""\e148""}.glyphicon-gbp:before{content:""\e149""}.glyphicon-sort:before{content:""\e150""}.glyphicon-sort-by-alphabet:before{content:""\e151""}.glyphicon-sort-by-alphabet-alt:before{content:""\e152""}.glyphicon-sort-by-order:before{content:""\e153""}.glyphicon-sort-by-order-alt:before{content:""\e154""}.glyphicon-sort-by-attributes:before{content:""\e155""}.glyphicon-sort-by-attributes-alt:before{content:""\e156""}.glyphicon-unchecked:before{content:""\e157""}.glyphicon-expand:before{content:""\e158""}.glyphicon-collapse-down:before{content:""\e159""}.glyphicon-collapse-up:before{content:""\e160""}.glyphicon-log-in:before{content:""\e161""}.glyphicon-flash:before{content:""\e162""}.glyphicon-log-out:before{content:""\e163""}.glyphicon-new-window:before{content:""\e164""}.glyphicon-record:before{content:""\e165""}.glyphicon-save:before{content:""\e166""}.glyphicon-open:before{content:""\e167""}.glyphicon-saved:before{content:""\e168""}.glyphicon-import:before{content:""\e169""}.glyphicon-export:before{content:""\e170""}.glyphicon-send:before{content:""\e171""}.glyphicon-floppy-disk:before{content:""\e172""}.glyphicon-floppy-saved:before{content:""\e173""}.glyphicon-floppy-remove:before{content:""\e174""}.glyphicon-floppy-save:before{content:""\e175""}.glyphicon-floppy-open:before{content:""\e176""}.glyphicon-credit-card:before{content:""\e177""}.glyphicon-transfer:before{content:""\e178""}.glyphicon-cutlery:before{content:""\e179""}.glyphicon-header:before{content:""\e180""}.glyphicon-compressed:before{content:""\e181""}.glyphicon-earphone:before{content:""\e182""}.glyphicon-phone-alt:before{content:""\e183""}.glyphicon-tower:before{content:""\e184""}.glyphicon-stats:before{content:""\e185""}.glyphicon-sd-video:before{content:""\e186""}.glyphicon-hd-video:before{content:""\e187""}.glyphicon-subtitles:before{content:""\e188""}.glyphicon-sound-stereo:before{content:""\e189""}.glyphicon-sound-dolby:before{content:""\e190""}.glyphicon-sound-5-1:before{content:""\e191""}.glyphicon-sound-6-1:before{content:""\e192""}.glyphicon-sound-7-1:before{content:""\e193""}.glyphicon-copyright-mark:before{content:""\e194""}.glyphicon-registration-mark:before{content:""\e195""}.glyphicon-cloud-download:before{content:""\e197""}.glyphicon-cloud-upload:before{content:""\e198""}.glyphicon-tree-conifer:before{content:""\e199""}.glyphicon-tree-deciduous:before{content:""\e200""}*{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}:before,:after{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}html{font-size:10px;-webkit-tap-highlight-color:rgba(0,0,0,0)}body{font-family:""Helvetica Neue"",Helvetica,Arial,sans-serif;font-size:14px;line-height:1.42857143;color:#333;background-color:#fff}input,button,select,textarea{font-family:inherit;font-size:inherit;line-height:inherit}a{color:#337ab7;text-decoration:none}a:hover,a:focus{color:#23527c;text-decoration:underline}a:focus{outline:thin dotted;outline:5px auto -webkit-focus-ring-color;outline-offset:-2px}figure{margin:0}img{vertical-align:middle}.img-responsive,.thumbnail>img,.thumbnail a>img,.carousel-inner>.item>img,.carousel-inner>.item>a>img{display:block;max-width:100%;height:auto}.img-rounded{border-radius:6px}.img-thumbnail{display:inline-block;max-width:100%;height:auto;padding:4px;line-height:1.42857143;background-color:#fff;border:1px solid #ddd;border-radius:4px;-webkit-transition:all .2s ease-in-out;-o-transition:all .2s ease-in-out;transition:all .2s ease-in-out}.img-circle{border-radius:50%}hr{margin-top:20px;margin-bottom:20px;border:0;border-top:1px solid #eee}.sr-only{position:absolute;width:1px;height:1px;padding:0;margin:-1px;overflow:hidden;clip:rect(0,0,0,0);border:0}.sr-only-focusable:active,.sr-only-focusable:focus{position:static;width:auto;height:auto;margin:0;overflow:visible;clip:auto}h1,h2,h3,h4,h5,h6,.h1,.h2,.h3,.h4,.h5,.h6{font-family:inherit;font-weight:500;line-height:1.1;color:inherit}h1 small,h2 small,h3 small,h4 small,h5 small,h6 small,.h1 small,.h2 small,.h3 small,.h4 small,.h5 small,.h6 small,h1 .small,h2 .small,h3 .small,h4 .small,h5 .small,h6 .small,.h1 .small,.h2 .small,.h3 .small,.h4 .small,.h5 .small,.h6 .small{font-weight:400;line-height:1;color:#777}h1,.h1,h2,.h2,h3,.h3{margin-top:20px;margin-bottom:10px}h1 small,.h1 small,h2 small,.h2 small,h3 small,.h3 small,h1 .small,.h1 .small,h2 .small,.h2 .small,h3 .small,.h3 .small{font-size:65%}h4,.h4,h5,.h5,h6,.h6{margin-top:10px;margin-bottom:10px}h4 small,.h4 small,h5 small,.h5 small,h6 small,.h6 small,h4 .small,.h4 .small,h5 .small,.h5 .small,h6 .small,.h6 .small{font-size:75%}h1,.h1{font-size:36px}h2,.h2{font-size:30px}h3,.h3{font-size:24px}h4,.h4{font-size:18px}h5,.h5{font-size:14px}h6,.h6{font-size:12px}p{margin:0 0 10px}.lead{margin-bottom:20px;font-size:16px;font-weight:300;line-height:1.4}@media (min-width:768px){.lead{font-size:21px}}small,.small{font-size:85%}mark,.mark{padding:.2em;background-color:#fcf8e3}.text-left{text-align:left}.text-right{text-align:right}.text-center{text-align:center}.text-justify{text-align:justify}.text-nowrap{white-space:nowrap}.text-lowercase{text-transform:lowercase}.text-uppercase{text-transform:uppercase}.text-capitalize{text-transform:capitalize}.text-muted{color:#777}.text-primary{color:#337ab7}a.text-primary:hover{color:#286090}.text-success{color:#3c763d}a.text-success:hover{color:#2b542c}.text-info{color:#31708f}a.text-info:hover{color:#245269}.text-warning{color:#8a6d3b}a.text-warning:hover{color:#66512c}.text-danger{color:#a94442}a.text-danger:hover{color:#843534}.bg-primary{color:#fff;background-color:#337ab7}a.bg-primary:hover{background-color:#286090}.bg-success{background-color:#dff0d8}a.bg-success:hover{background-color:#c1e2b3}.bg-info{background-color:#d9edf7}a.bg-info:hover{background-color:#afd9ee}.bg-warning{background-color:#fcf8e3}a.bg-warning:hover{background-color:#f7ecb5}.bg-danger{background-color:#f2dede}a.bg-danger:hover{background-color:#e4b9b9}.page-header{padding-bottom:9px;margin:40px 0 20px;border-bottom:1px solid #eee}ul,ol{margin-top:0;margin-bottom:10px}ul ul,ol ul,ul ol,ol ol{margin-bottom:0}.list-unstyled{padding-left:0;list-style:none}.list-inline{padding-left:0;margin-left:-5px;list-style:none}.list-inline>li{display:inline-block;padding-right:5px;padding-left:5px}dl{margin-top:0;margin-bottom:20px}dt,dd{line-height:1.42857143}dt{font-weight:700}dd{margin-left:0}@media (min-width:768px){.dl-horizontal dt{float:left;width:160px;overflow:hidden;clear:left;text-align:right;text-overflow:ellipsis;white-space:nowrap}.dl-horizontal dd{margin-left:180px}}abbr[title],abbr[data-original-title]{cursor:help;border-bottom:1px dotted #777}.initialism{font-size:90%;text-transform:uppercase}blockquote{padding:10px 20px;margin:0 0 20px;font-size:17.5px;border-left:5px solid #eee}blockquote p:last-child,blockquote ul:last-child,blockquote ol:last-child{margin-bottom:0}blockquote footer,blockquote small,blockquote .small{display:block;font-size:80%;line-height:1.42857143;color:#777}blockquote footer:before,blockquote small:before,blockquote .small:before{content:'\2014 \00A0'}.blockquote-reverse,blockquote.pull-right{padding-right:15px;padding-left:0;text-align:right;border-right:5px solid #eee;border-left:0}.blockquote-reverse footer:before,blockquote.pull-right footer:before,.blockquote-reverse small:before,blockquote.pull-right small:before,.blockquote-reverse .small:before,blockquote.pull-right .small:before{content:''}.blockquote-reverse footer:after,blockquote.pull-right footer:after,.blockquote-reverse small:after,blockquote.pull-right small:after,.blockquote-reverse .small:after,blockquote.pull-right .small:after{content:'\00A0 \2014'}address{margin-bottom:20px;font-style:normal;line-height:1.42857143}code,kbd,pre,samp{font-family:Menlo,Monaco,Consolas,""Courier New"",monospace}code{padding:2px 4px;font-size:90%;color:#c7254e;background-color:#f9f2f4;border-radius:4px}kbd{padding:2px 4px;font-size:90%;color:#fff;background-color:#333;border-radius:3px;-webkit-box-shadow:inset 0 -1px 0 rgba(0,0,0,.25);box-shadow:inset 0 -1px 0 rgba(0,0,0,.25)}kbd kbd{padding:0;font-size:100%;font-weight:700;-webkit-box-shadow:none;box-shadow:none}pre{display:block;padding:9.5px;margin:0 0 10px;font-size:13px;line-height:1.42857143;color:#333;word-break:break-all;word-wrap:break-word;background-color:#f5f5f5;border:1px solid #ccc;border-radius:4px}pre code{padding:0;font-size:inherit;color:inherit;white-space:pre-wrap;background-color:transparent;border-radius:0}.pre-scrollable{max-height:340px;overflow-y:scroll}.container{padding-right:15px;padding-left:15px;margin-right:auto;margin-left:auto}@media (min-width:768px){.container{width:750px}}@media (min-width:992px){.container{width:970px}}@media (min-width:1200px){.container{width:1170px}}.container-fluid{padding-right:15px;padding-left:15px;margin-right:auto;margin-left:auto}.row{margin-right:-15px;margin-left:-15px}.col-xs-1,.col-sm-1,.col-md-1,.col-lg-1,.col-xs-2,.col-sm-2,.col-md-2,.col-lg-2,.col-xs-3,.col-sm-3,.col-md-3,.col-lg-3,.col-xs-4,.col-sm-4,.col-md-4,.col-lg-4,.col-xs-5,.col-sm-5,.col-md-5,.col-lg-5,.col-xs-6,.col-sm-6,.col-md-6,.col-lg-6,.col-xs-7,.col-sm-7,.col-md-7,.col-lg-7,.col-xs-8,.col-sm-8,.col-md-8,.col-lg-8,.col-xs-9,.col-sm-9,.col-md-9,.col-lg-9,.col-xs-10,.col-sm-10,.col-md-10,.col-lg-10,.col-xs-11,.col-sm-11,.col-md-11,.col-lg-11,.col-xs-12,.col-sm-12,.col-md-12,.col-lg-12{position:relative;min-height:1px;padding-right:15px;padding-left:15px}.col-xs-1,.col-xs-2,.col-xs-3,.col-xs-4,.col-xs-5,.col-xs-6,.col-xs-7,.col-xs-8,.col-xs-9,.col-xs-10,.col-xs-11,.col-xs-12{float:left}.col-xs-12{width:100%}.col-xs-11{width:91.66666667%}.col-xs-10{width:83.33333333%}.col-xs-9{width:75%}.col-xs-8{width:66.66666667%}.col-xs-7{width:58.33333333%}.col-xs-6{width:50%}.col-xs-5{width:41.66666667%}.col-xs-4{width:33.33333333%}.col-xs-3{width:25%}.col-xs-2{width:16.66666667%}.col-xs-1{width:8.33333333%}.col-xs-pull-12{right:100%}.col-xs-pull-11{right:91.66666667%}.col-xs-pull-10{right:83.33333333%}.col-xs-pull-9{right:75%}.col-xs-pull-8{right:66.66666667%}.col-xs-pull-7{right:58.33333333%}.col-xs-pull-6{right:50%}.col-xs-pull-5{right:41.66666667%}.col-xs-pull-4{right:33.33333333%}.col-xs-pull-3{right:25%}.col-xs-pull-2{right:16.66666667%}.col-xs-pull-1{right:8.33333333%}.col-xs-pull-0{right:auto}.col-xs-push-12{left:100%}.col-xs-push-11{left:91.66666667%}.col-xs-push-10{left:83.33333333%}.col-xs-push-9{left:75%}.col-xs-push-8{left:66.66666667%}.col-xs-push-7{left:58.33333333%}.col-xs-push-6{left:50%}.col-xs-push-5{left:41.66666667%}.col-xs-push-4{left:33.33333333%}.col-xs-push-3{left:25%}.col-xs-push-2{left:16.66666667%}.col-xs-push-1{left:8.33333333%}.col-xs-push-0{left:auto}.col-xs-offset-12{margin-left:100%}.col-xs-offset-11{margin-left:91.66666667%}.col-xs-offset-10{margin-left:83.33333333%}.col-xs-offset-9{margin-left:75%}.col-xs-offset-8{margin-left:66.66666667%}.col-xs-offset-7{margin-left:58.33333333%}.col-xs-offset-6{margin-left:50%}.col-xs-offset-5{margin-left:41.66666667%}.col-xs-offset-4{margin-left:33.33333333%}.col-xs-offset-3{margin-left:25%}.col-xs-offset-2{margin-left:16.66666667%}.col-xs-offset-1{margin-left:8.33333333%}.col-xs-offset-0{margin-left:0}@media (min-width:768px){.col-sm-1,.col-sm-2,.col-sm-3,.col-sm-4,.col-sm-5,.col-sm-6,.col-sm-7,.col-sm-8,.col-sm-9,.col-sm-10,.col-sm-11,.col-sm-12{float:left}.col-sm-12{width:100%}.col-sm-11{width:91.66666667%}.col-sm-10{width:83.33333333%}.col-sm-9{width:75%}.col-sm-8{width:66.66666667%}.col-sm-7{width:58.33333333%}.col-sm-6{width:50%}.col-sm-5{width:41.66666667%}.col-sm-4{width:33.33333333%}.col-sm-3{width:25%}.col-sm-2{width:16.66666667%}.col-sm-1{width:8.33333333%}.col-sm-pull-12{right:100%}.col-sm-pull-11{right:91.66666667%}.col-sm-pull-10{right:83.33333333%}.col-sm-pull-9{right:75%}.col-sm-pull-8{right:66.66666667%}.col-sm-pull-7{right:58.33333333%}.col-sm-pull-6{right:50%}.col-sm-pull-5{right:41.66666667%}.col-sm-pull-4{right:33.33333333%}.col-sm-pull-3{right:25%}.col-sm-pull-2{right:16.66666667%}.col-sm-pull-1{right:8.33333333%}.col-sm-pull-0{right:auto}.col-sm-push-12{left:100%}.col-sm-push-11{left:91.66666667%}.col-sm-push-10{left:83.33333333%}.col-sm-push-9{left:75%}.col-sm-push-8{left:66.66666667%}.col-sm-push-7{left:58.33333333%}.col-sm-push-6{left:50%}.col-sm-push-5{left:41.66666667%}.col-sm-push-4{left:33.33333333%}.col-sm-push-3{left:25%}.col-sm-push-2{left:16.66666667%}.col-sm-push-1{left:8.33333333%}.col-sm-push-0{left:auto}.col-sm-offset-12{margin-left:100%}.col-sm-offset-11{margin-left:91.66666667%}.col-sm-offset-10{margin-left:83.33333333%}.col-sm-offset-9{margin-left:75%}.col-sm-offset-8{margin-left:66.66666667%}.col-sm-offset-7{margin-left:58.33333333%}.col-sm-offset-6{margin-left:50%}.col-sm-offset-5{margin-left:41.66666667%}.col-sm-offset-4{margin-left:33.33333333%}.col-sm-offset-3{margin-left:25%}.col-sm-offset-2{margin-left:16.66666667%}.col-sm-offset-1{margin-left:8.33333333%}.col-sm-offset-0{margin-left:0}}@media (min-width:992px){.col-md-1,.col-md-2,.col-md-3,.col-md-4,.col-md-5,.col-md-6,.col-md-7,.col-md-8,.col-md-9,.col-md-10,.col-md-11,.col-md-12{float:left}.col-md-12{width:100%}.col-md-11{width:91.66666667%}.col-md-10{width:83.33333333%}.col-md-9{width:75%}.col-md-8{width:66.66666667%}.col-md-7{width:58.33333333%}.col-md-6{width:50%}.col-md-5{width:41.66666667%}.col-md-4{width:33.33333333%}.col-md-3{width:25%}.col-md-2{width:16.66666667%}.col-md-1{width:8.33333333%}.col-md-pull-12{right:100%}.col-md-pull-11{right:91.66666667%}.col-md-pull-10{right:83.33333333%}.col-md-pull-9{right:75%}.col-md-pull-8{right:66.66666667%}.col-md-pull-7{right:58.33333333%}.col-md-pull-6{right:50%}.col-md-pull-5{right:41.66666667%}.col-md-pull-4{right:33.33333333%}.col-md-pull-3{right:25%}.col-md-pull-2{right:16.66666667%}.col-md-pull-1{right:8.33333333%}.col-md-pull-0{right:auto}.col-md-push-12{left:100%}.col-md-push-11{left:91.66666667%}.col-md-push-10{left:83.33333333%}.col-md-push-9{left:75%}.col-md-push-8{left:66.66666667%}.col-md-push-7{left:58.33333333%}.col-md-push-6{left:50%}.col-md-push-5{left:41.66666667%}.col-md-push-4{left:33.33333333%}.col-md-push-3{left:25%}.col-md-push-2{left:16.66666667%}.col-md-push-1{left:8.33333333%}.col-md-push-0{left:auto}.col-md-offset-12{margin-left:100%}.col-md-offset-11{margin-left:91.66666667%}.col-md-offset-10{margin-left:83.33333333%}.col-md-offset-9{margin-left:75%}.col-md-offset-8{margin-left:66.66666667%}.col-md-offset-7{margin-left:58.33333333%}.col-md-offset-6{margin-left:50%}.col-md-offset-5{margin-left:41.66666667%}.col-md-offset-4{margin-left:33.33333333%}.col-md-offset-3{margin-left:25%}.col-md-offset-2{margin-left:16.66666667%}.col-md-offset-1{margin-left:8.33333333%}.col-md-offset-0{margin-left:0}}@media (min-width:1200px){.col-lg-1,.col-lg-2,.col-lg-3,.col-lg-4,.col-lg-5,.col-lg-6,.col-lg-7,.col-lg-8,.col-lg-9,.col-lg-10,.col-lg-11,.col-lg-12{float:left}.col-lg-12{width:100%}.col-lg-11{width:91.66666667%}.col-lg-10{width:83.33333333%}.col-lg-9{width:75%}.col-lg-8{width:66.66666667%}.col-lg-7{width:58.33333333%}.col-lg-6{width:50%}.col-lg-5{width:41.66666667%}.col-lg-4{width:33.33333333%}.col-lg-3{width:25%}.col-lg-2{width:16.66666667%}.col-lg-1{width:8.33333333%}.col-lg-pull-12{right:100%}.col-lg-pull-11{right:91.66666667%}.col-lg-pull-10{right:83.33333333%}.col-lg-pull-9{right:75%}.col-lg-pull-8{right:66.66666667%}.col-lg-pull-7{right:58.33333333%}.col-lg-pull-6{right:50%}.col-lg-pull-5{right:41.66666667%}.col-lg-pull-4{right:33.33333333%}.col-lg-pull-3{right:25%}.col-lg-pull-2{right:16.66666667%}.col-lg-pull-1{right:8.33333333%}.col-lg-pull-0{right:auto}.col-lg-push-12{left:100%}.col-lg-push-11{left:91.66666667%}.col-lg-push-10{left:83.33333333%}.col-lg-push-9{left:75%}.col-lg-push-8{left:66.66666667%}.col-lg-push-7{left:58.33333333%}.col-lg-push-6{left:50%}.col-lg-push-5{left:41.66666667%}.col-lg-push-4{left:33.33333333%}.col-lg-push-3{left:25%}.col-lg-push-2{left:16.66666667%}.col-lg-push-1{left:8.33333333%}.col-lg-push-0{left:auto}.col-lg-offset-12{margin-left:100%}.col-lg-offset-11{margin-left:91.66666667%}.col-lg-offset-10{margin-left:83.33333333%}.col-lg-offset-9{margin-left:75%}.col-lg-offset-8{margin-left:66.66666667%}.col-lg-offset-7{margin-left:58.33333333%}.col-lg-offset-6{margin-left:50%}.col-lg-offset-5{margin-left:41.66666667%}.col-lg-offset-4{margin-left:33.33333333%}.col-lg-offset-3{margin-left:25%}.col-lg-offset-2{margin-left:16.66666667%}.col-lg-offset-1{margin-left:8.33333333%}.col-lg-offset-0{margin-left:0}}table{background-color:transparent}caption{padding-top:8px;padding-bottom:8px;color:#777;text-align:left}th{text-align:left}.table{width:100%;max-width:100%;margin-bottom:20px}.table>thead>tr>th,.table>tbody>tr>th,.table>tfoot>tr>th,.table>thead>tr>td,.table>tbody>tr>td,.table>tfoot>tr>td{padding:8px;line-height:1.42857143;vertical-align:top;border-top:1px solid #ddd}.table>thead>tr>th{vertical-align:bottom;border-bottom:2px solid #ddd}.table>caption+thead>tr:first-child>th,.table>colgroup+thead>tr:first-child>th,.table>thead:first-child>tr:first-child>th,.table>caption+thead>tr:first-child>td,.table>colgroup+thead>tr:first-child>td,.table>thead:first-child>tr:first-child>td{border-top:0}.table>tbody+tbody{border-top:2px solid #ddd}.table .table{background-color:#fff}.table-condensed>thead>tr>th,.table-condensed>tbody>tr>th,.table-condensed>tfoot>tr>th,.table-condensed>thead>tr>td,.table-condensed>tbody>tr>td,.table-condensed>tfoot>tr>td{padding:5px}.table-bordered{border:1px solid #ddd}.table-bordered>thead>tr>th,.table-bordered>tbody>tr>th,.table-bordered>tfoot>tr>th,.table-bordered>thead>tr>td,.table-bordered>tbody>tr>td,.table-bordered>tfoot>tr>td{border:1px solid #ddd}.table-bordered>thead>tr>th,.table-bordered>thead>tr>td{border-bottom-width:2px}.table-striped>tbody>tr:nth-child(odd){background-color:#f9f9f9}.table-hover>tbody>tr:hover{background-color:#f5f5f5}table col[class*=col-]{position:static;display:table-column;float:none}table td[class*=col-],table th[class*=col-]{position:static;display:table-cell;float:none}.table>thead>tr>td.active,.table>tbody>tr>td.active,.table>tfoot>tr>td.active,.table>thead>tr>th.active,.table>tbody>tr>th.active,.table>tfoot>tr>th.active,.table>thead>tr.active>td,.table>tbody>tr.active>td,.table>tfoot>tr.active>td,.table>thead>tr.active>th,.table>tbody>tr.active>th,.table>tfoot>tr.active>th{background-color:#f5f5f5}.table-hover>tbody>tr>td.active:hover,.table-hover>tbody>tr>th.active:hover,.table-hover>tbody>tr.active:hover>td,.table-hover>tbody>tr:hover>.active,.table-hover>tbody>tr.active:hover>th{background-color:#e8e8e8}.table>thead>tr>td.success,.table>tbody>tr>td.success,.table>tfoot>tr>td.success,.table>thead>tr>th.success,.table>tbody>tr>th.success,.table>tfoot>tr>th.success,.table>thead>tr.success>td,.table>tbody>tr.success>td,.table>tfoot>tr.success>td,.table>thead>tr.success>th,.table>tbody>tr.success>th,.table>tfoot>tr.success>th{background-color:#dff0d8}.table-hover>tbody>tr>td.success:hover,.table-hover>tbody>tr>th.success:hover,.table-hover>tbody>tr.success:hover>td,.table-hover>tbody>tr:hover>.success,.table-hover>tbody>tr.success:hover>th{background-color:#d0e9c6}.table>thead>tr>td.info,.table>tbody>tr>td.info,.table>tfoot>tr>td.info,.table>thead>tr>th.info,.table>tbody>tr>th.info,.table>tfoot>tr>th.info,.table>thead>tr.info>td,.table>tbody>tr.info>td,.table>tfoot>tr.info>td,.table>thead>tr.info>th,.table>tbody>tr.info>th,.table>tfoot>tr.info>th{background-color:#d9edf7}.table-hover>tbody>tr>td.info:hover,.table-hover>tbody>tr>th.info:hover,.table-hover>tbody>tr.info:hover>td,.table-hover>tbody>tr:hover>.info,.table-hover>tbody>tr.info:hover>th{background-color:#c4e3f3}.table>thead>tr>td.warning,.table>tbody>tr>td.warning,.table>tfoot>tr>td.warning,.table>thead>tr>th.warning,.table>tbody>tr>th.warning,.table>tfoot>tr>th.warning,.table>thead>tr.warning>td,.table>tbody>tr.warning>td,.table>tfoot>tr.warning>td,.table>thead>tr.warning>th,.table>tbody>tr.warning>th,.table>tfoot>tr.warning>th{background-color:#fcf8e3}.table-hover>tbody>tr>td.warning:hover,.table-hover>tbody>tr>th.warning:hover,.table-hover>tbody>tr.warning:hover>td,.table-hover>tbody>tr:hover>.warning,.table-hover>tbody>tr.warning:hover>th{background-color:#faf2cc}.table>thead>tr>td.danger,.table>tbody>tr>td.danger,.table>tfoot>tr>td.danger,.table>thead>tr>th.danger,.table>tbody>tr>th.danger,.table>tfoot>tr>th.danger,.table>thead>tr.danger>td,.table>tbody>tr.danger>td,.table>tfoot>tr.danger>td,.table>thead>tr.danger>th,.table>tbody>tr.danger>th,.table>tfoot>tr.danger>th{background-color:#f2dede}.table-hover>tbody>tr>td.danger:hover,.table-hover>tbody>tr>th.danger:hover,.table-hover>tbody>tr.danger:hover>td,.table-hover>tbody>tr:hover>.danger,.table-hover>tbody>tr.danger:hover>th{background-color:#ebcccc}.table-responsive{min-height:.01%;overflow-x:auto}@media screen and (max-width:767px){.table-responsive{width:100%;margin-bottom:15px;overflow-y:hidden;-ms-overflow-style:-ms-autohiding-scrollbar;border:1px solid #ddd}.table-responsive>.table{margin-bottom:0}.table-responsive>.table>thead>tr>th,.table-responsive>.table>tbody>tr>th,.table-responsive>.table>tfoot>tr>th,.table-responsive>.table>thead>tr>td,.table-responsive>.table>tbody>tr>td,.table-responsive>.table>tfoot>tr>td{white-space:nowrap}.table-responsive>.table-bordered{border:0}.table-responsive>.table-bordered>thead>tr>th:first-child,.table-responsive>.table-bordered>tbody>tr>th:first-child,.table-responsive>.table-bordered>tfoot>tr>th:first-child,.table-responsive>.table-bordered>thead>tr>td:first-child,.table-responsive>.table-bordered>tbody>tr>td:first-child,.table-responsive>.table-bordered>tfoot>tr>td:first-child{border-left:0}.table-responsive>.table-bordered>thead>tr>th:last-child,.table-responsive>.table-bordered>tbody>tr>th:last-child,.table-responsive>.table-bordered>tfoot>tr>th:last-child,.table-responsive>.table-bordered>thead>tr>td:last-child,.table-responsive>.table-bordered>tbody>tr>td:last-child,.table-responsive>.table-bordered>tfoot>tr>td:last-child{border-right:0}.table-responsive>.table-bordered>tbody>tr:last-child>th,.table-responsive>.table-bordered>tfoot>tr:last-child>th,.table-responsive>.table-bordered>tbody>tr:last-child>td,.table-responsive>.table-bordered>tfoot>tr:last-child>td{border-bottom:0}}fieldset{min-width:0;padding:0;margin:0;border:0}legend{display:block;width:100%;padding:0;margin-bottom:20px;font-size:21px;line-height:inherit;color:#333;border:0;border-bottom:1px solid #e5e5e5}label{display:inline-block;max-width:100%;margin-bottom:5px;font-weight:700}input[type=search]{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}input[type=radio],input[type=checkbox]{margin:4px 0 0;margin-top:1px \9;line-height:normal}input[type=file]{display:block}input[type=range]{display:block;width:100%}select[multiple],select[size]{height:auto}input[type=file]:focus,input[type=radio]:focus,input[type=checkbox]:focus{outline:thin dotted;outline:5px auto -webkit-focus-ring-color;outline-offset:-2px}output{display:block;padding-top:7px;font-size:14px;line-height:1.42857143;color:#555}.form-control{display:block;width:100%;height:34px;padding:6px 12px;font-size:14px;line-height:1.42857143;color:#555;background-color:#fff;background-image:none;border:1px solid #ccc;border-radius:4px;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075);-webkit-transition:border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;-o-transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s;transition:border-color ease-in-out .15s,box-shadow ease-in-out .15s}.form-control:focus{border-color:#66afe9;outline:0;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6);box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgba(102,175,233,.6)}.form-control::-moz-placeholder{color:#999;opacity:1}.form-control:-ms-input-placeholder{color:#999}.form-control::-webkit-input-placeholder{color:#999}.form-control[disabled],.form-control[readonly],fieldset[disabled] .form-control{cursor:not-allowed;background-color:#eee;opacity:1}textarea.form-control{height:auto}input[type=search]{-webkit-appearance:none}@media screen and (-webkit-min-device-pixel-ratio:0){input[type=date],input[type=time],input[type=datetime-local],input[type=month]{line-height:34px}input[type=date].input-sm,input[type=time].input-sm,input[type=datetime-local].input-sm,input[type=month].input-sm{line-height:30px}input[type=date].input-lg,input[type=time].input-lg,input[type=datetime-local].input-lg,input[type=month].input-lg{line-height:46px}}.form-group{margin-bottom:15px}.radio,.checkbox{position:relative;display:block;margin-top:10px;margin-bottom:10px}.radio label,.checkbox label{min-height:20px;padding-left:20px;margin-bottom:0;font-weight:400;cursor:pointer}.radio input[type=radio],.radio-inline input[type=radio],.checkbox input[type=checkbox],.checkbox-inline input[type=checkbox]{position:absolute;margin-top:4px \9;margin-left:-20px}.radio+.radio,.checkbox+.checkbox{margin-top:-5px}.radio-inline,.checkbox-inline{display:inline-block;padding-left:20px;margin-bottom:0;font-weight:400;vertical-align:middle;cursor:pointer}.radio-inline+.radio-inline,.checkbox-inline+.checkbox-inline{margin-top:0;margin-left:10px}input[type=radio][disabled],input[type=checkbox][disabled],input[type=radio].disabled,input[type=checkbox].disabled,fieldset[disabled] input[type=radio],fieldset[disabled] input[type=checkbox]{cursor:not-allowed}.radio-inline.disabled,.checkbox-inline.disabled,fieldset[disabled] .radio-inline,fieldset[disabled] .checkbox-inline{cursor:not-allowed}.radio.disabled label,.checkbox.disabled label,fieldset[disabled] .radio label,fieldset[disabled] .checkbox label{cursor:not-allowed}.form-control-static{padding-top:7px;padding-bottom:7px;margin-bottom:0}.form-control-static.input-lg,.form-control-static.input-sm{padding-right:0;padding-left:0}.input-sm,.form-group-sm .form-control{height:30px;padding:5px 10px;font-size:12px;line-height:1.5;border-radius:3px}select.input-sm,select.form-group-sm .form-control{height:30px;line-height:30px}textarea.input-sm,textarea.form-group-sm .form-control,select[multiple].input-sm,select[multiple].form-group-sm .form-control{height:auto}.input-lg,.form-group-lg .form-control{height:46px;padding:10px 16px;font-size:18px;line-height:1.33;border-radius:6px}select.input-lg,select.form-group-lg .form-control{height:46px;line-height:46px}textarea.input-lg,textarea.form-group-lg .form-control,select[multiple].input-lg,select[multiple].form-group-lg .form-control{height:auto}.has-feedback{position:relative}.has-feedback .form-control{padding-right:42.5px}.form-control-feedback{position:absolute;top:0;right:0;z-index:2;display:block;width:34px;height:34px;line-height:34px;text-align:center;pointer-events:none}.input-lg+.form-control-feedback{width:46px;height:46px;line-height:46px}.input-sm+.form-control-feedback{width:30px;height:30px;line-height:30px}.has-success .help-block,.has-success .control-label,.has-success .radio,.has-success .checkbox,.has-success .radio-inline,.has-success .checkbox-inline,.has-success.radio label,.has-success.checkbox label,.has-success.radio-inline label,.has-success.checkbox-inline label{color:#3c763d}.has-success .form-control{border-color:#3c763d;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075)}.has-success .form-control:focus{border-color:#2b542c;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #67b168;box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #67b168}.has-success .input-group-addon{color:#3c763d;background-color:#dff0d8;border-color:#3c763d}.has-success .form-control-feedback{color:#3c763d}.has-warning .help-block,.has-warning .control-label,.has-warning .radio,.has-warning .checkbox,.has-warning .radio-inline,.has-warning .checkbox-inline,.has-warning.radio label,.has-warning.checkbox label,.has-warning.radio-inline label,.has-warning.checkbox-inline label{color:#8a6d3b}.has-warning .form-control{border-color:#8a6d3b;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075)}.has-warning .form-control:focus{border-color:#66512c;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #c0a16b;box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #c0a16b}.has-warning .input-group-addon{color:#8a6d3b;background-color:#fcf8e3;border-color:#8a6d3b}.has-warning .form-control-feedback{color:#8a6d3b}.has-error .help-block,.has-error .control-label,.has-error .radio,.has-error .checkbox,.has-error .radio-inline,.has-error .checkbox-inline,.has-error.radio label,.has-error.checkbox label,.has-error.radio-inline label,.has-error.checkbox-inline label{color:#a94442}.has-error .form-control{border-color:#a94442;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075);box-shadow:inset 0 1px 1px rgba(0,0,0,.075)}.has-error .form-control:focus{border-color:#843534;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #ce8483;box-shadow:inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #ce8483}.has-error .input-group-addon{color:#a94442;background-color:#f2dede;border-color:#a94442}.has-error .form-control-feedback{color:#a94442}.has-feedback label~.form-control-feedback{top:25px}.has-feedback label.sr-only~.form-control-feedback{top:0}.help-block{display:block;margin-top:5px;margin-bottom:10px;color:#737373}@media (min-width:768px){.form-inline .form-group{display:inline-block;margin-bottom:0;vertical-align:middle}.form-inline .form-control{display:inline-block;width:auto;vertical-align:middle}.form-inline .form-control-static{display:inline-block}.form-inline .input-group{display:inline-table;vertical-align:middle}.form-inline .input-group .input-group-addon,.form-inline .input-group .input-group-btn,.form-inline .input-group .form-control{width:auto}.form-inline .input-group>.form-control{width:100%}.form-inline .control-label{margin-bottom:0;vertical-align:middle}.form-inline .radio,.form-inline .checkbox{display:inline-block;margin-top:0;margin-bottom:0;vertical-align:middle}.form-inline .radio label,.form-inline .checkbox label{padding-left:0}.form-inline .radio input[type=radio],.form-inline .checkbox input[type=checkbox]{position:relative;margin-left:0}.form-inline .has-feedback .form-control-feedback{top:0}}.form-horizontal .radio,.form-horizontal .checkbox,.form-horizontal .radio-inline,.form-horizontal .checkbox-inline{padding-top:7px;margin-top:0;margin-bottom:0}.form-horizontal .radio,.form-horizontal .checkbox{min-height:27px}.form-horizontal .form-group{margin-right:-15px;margin-left:-15px}@media (min-width:768px){.form-horizontal .control-label{padding-top:7px;margin-bottom:0;text-align:right}}.form-horizontal .has-feedback .form-control-feedback{right:15px}@media (min-width:768px){.form-horizontal .form-group-lg .control-label{padding-top:14.3px}}@media (min-width:768px){.form-horizontal .form-group-sm .control-label{padding-top:6px}}.btn{display:inline-block;padding:6px 12px;margin-bottom:0;font-size:14px;font-weight:400;line-height:1.42857143;text-align:center;white-space:nowrap;vertical-align:middle;-ms-touch-action:manipulation;touch-action:manipulation;cursor:pointer;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;background-image:none;border:1px solid transparent;border-radius:4px}.btn:focus,.btn:active:focus,.btn.active:focus,.btn.focus,.btn:active.focus,.btn.active.focus{outline:thin dotted;outline:5px auto -webkit-focus-ring-color;outline-offset:-2px}.btn:hover,.btn:focus,.btn.focus{color:#333;text-decoration:none}.btn:active,.btn.active{background-image:none;outline:0;-webkit-box-shadow:inset 0 3px 5px rgba(0,0,0,.125);box-shadow:inset 0 3px 5px rgba(0,0,0,.125)}.btn.disabled,.btn[disabled],fieldset[disabled] .btn{pointer-events:none;cursor:not-allowed;filter:alpha(opacity=65);-webkit-box-shadow:none;box-shadow:none;opacity:.65}.btn-default{color:#333;background-color:#fff;border-color:#ccc}.btn-default:hover,.btn-default:focus,.btn-default.focus,.btn-default:active,.btn-default.active,.open>.dropdown-toggle.btn-default{color:#333;background-color:#e6e6e6;border-color:#adadad}.btn-default:active,.btn-default.active,.open>.dropdown-toggle.btn-default{background-image:none}.btn-default.disabled,.btn-default[disabled],fieldset[disabled] .btn-default,.btn-default.disabled:hover,.btn-default[disabled]:hover,fieldset[disabled] .btn-default:hover,.btn-default.disabled:focus,.btn-default[disabled]:focus,fieldset[disabled] .btn-default:focus,.btn-default.disabled.focus,.btn-default[disabled].focus,fieldset[disabled] .btn-default.focus,.btn-default.disabled:active,.btn-default[disabled]:active,fieldset[disabled] .btn-default:active,.btn-default.disabled.active,.btn-default[disabled].active,fieldset[disabled] .btn-default.active{background-color:#fff;border-color:#ccc}.btn-default .badge{color:#fff;background-color:#333}.btn-primary{color:#fff;background-color:#337ab7;border-color:#2e6da4}.btn-primary:hover,.btn-primary:focus,.btn-primary.focus,.btn-primary:active,.btn-primary.active,.open>.dropdown-toggle.btn-primary{color:#fff;background-color:#286090;border-color:#204d74}.btn-primary:active,.btn-primary.active,.open>.dropdown-toggle.btn-primary{background-image:none}.btn-primary.disabled,.btn-primary[disabled],fieldset[disabled] .btn-primary,.btn-primary.disabled:hover,.btn-primary[disabled]:hover,fieldset[disabled] .btn-primary:hover,.btn-primary.disabled:focus,.btn-primary[disabled]:focus,fieldset[disabled] .btn-primary:focus,.btn-primary.disabled.focus,.btn-primary[disabled].focus,fieldset[disabled] .btn-primary.focus,.btn-primary.disabled:active,.btn-primary[disabled]:active,fieldset[disabled] .btn-primary:active,.btn-primary.disabled.active,.btn-primary[disabled].active,fieldset[disabled] .btn-primary.active{background-color:#337ab7;border-color:#2e6da4}.btn-primary .badge{color:#337ab7;background-color:#fff}.btn-success{color:#fff;background-color:#5cb85c;border-color:#4cae4c}.btn-success:hover,.btn-success:focus,.btn-success.focus,.btn-success:active,.btn-success.active,.open>.dropdown-toggle.btn-success{color:#fff;background-color:#449d44;border-color:#398439}.btn-success:active,.btn-success.active,.open>.dropdown-toggle.btn-success{background-image:none}.btn-success.disabled,.btn-success[disabled],fieldset[disabled] .btn-success,.btn-success.disabled:hover,.btn-success[disabled]:hover,fieldset[disabled] .btn-success:hover,.btn-success.disabled:focus,.btn-success[disabled]:focus,fieldset[disabled] .btn-success:focus,.btn-success.disabled.focus,.btn-success[disabled].focus,fieldset[disabled] .btn-success.focus,.btn-success.disabled:active,.btn-success[disabled]:active,fieldset[disabled] .btn-success:active,.btn-success.disabled.active,.btn-success[disabled].active,fieldset[disabled] .btn-success.active{background-color:#5cb85c;border-color:#4cae4c}.btn-success .badge{color:#5cb85c;background-color:#fff}.btn-info{color:#fff;background-color:#5bc0de;border-color:#46b8da}.btn-info:hover,.btn-info:focus,.btn-info.focus,.btn-info:active,.btn-info.active,.open>.dropdown-toggle.btn-info{color:#fff;background-color:#31b0d5;border-color:#269abc}.btn-info:active,.btn-info.active,.open>.dropdown-toggle.btn-info{background-image:none}.btn-info.disabled,.btn-info[disabled],fieldset[disabled] .btn-info,.btn-info.disabled:hover,.btn-info[disabled]:hover,fieldset[disabled] .btn-info:hover,.btn-info.disabled:focus,.btn-info[disabled]:focus,fieldset[disabled] .btn-info:focus,.btn-info.disabled.focus,.btn-info[disabled].focus,fieldset[disabled] .btn-info.focus,.btn-info.disabled:active,.btn-info[disabled]:active,fieldset[disabled] .btn-info:active,.btn-info.disabled.active,.btn-info[disabled].active,fieldset[disabled] .btn-info.active{background-color:#5bc0de;border-color:#46b8da}.btn-info .badge{color:#5bc0de;background-color:#fff}.btn-warning{color:#fff;background-color:#f0ad4e;border-color:#eea236}.btn-warning:hover,.btn-warning:focus,.btn-warning.focus,.btn-warning:active,.btn-warning.active,.open>.dropdown-toggle.btn-warning{color:#fff;background-color:#ec971f;border-color:#d58512}.btn-warning:active,.btn-warning.active,.open>.dropdown-toggle.btn-warning{background-image:none}.btn-warning.disabled,.btn-warning[disabled],fieldset[disabled] .btn-warning,.btn-warning.disabled:hover,.btn-warning[disabled]:hover,fieldset[disabled] .btn-warning:hover,.btn-warning.disabled:focus,.btn-warning[disabled]:focus,fieldset[disabled] .btn-warning:focus,.btn-warning.disabled.focus,.btn-warning[disabled].focus,fieldset[disabled] .btn-warning.focus,.btn-warning.disabled:active,.btn-warning[disabled]:active,fieldset[disabled] .btn-warning:active,.btn-warning.disabled.active,.btn-warning[disabled].active,fieldset[disabled] .btn-warning.active{background-color:#f0ad4e;border-color:#eea236}.btn-warning .badge{color:#f0ad4e;background-color:#fff}.btn-danger{color:#fff;background-color:#d9534f;border-color:#d43f3a}.btn-danger:hover,.btn-danger:focus,.btn-danger.focus,.btn-danger:active,.btn-danger.active,.open>.dropdown-toggle.btn-danger{color:#fff;background-color:#c9302c;border-color:#ac2925}.btn-danger:active,.btn-danger.active,.open>.dropdown-toggle.btn-danger{background-image:none}.btn-danger.disabled,.btn-danger[disabled],fieldset[disabled] .btn-danger,.btn-danger.disabled:hover,.btn-danger[disabled]:hover,fieldset[disabled] .btn-danger:hover,.btn-danger.disabled:focus,.btn-danger[disabled]:focus,fieldset[disabled] .btn-danger:focus,.btn-danger.disabled.focus,.btn-danger[disabled].focus,fieldset[disabled] .btn-danger.focus,.btn-danger.disabled:active,.btn-danger[disabled]:active,fieldset[disabled] .btn-danger:active,.btn-danger.disabled.active,.btn-danger[disabled].active,fieldset[disabled] .btn-danger.active{background-color:#d9534f;border-color:#d43f3a}.btn-danger .badge{color:#d9534f;background-color:#fff}.btn-link{font-weight:400;color:#337ab7;border-radius:0}.btn-link,.btn-link:active,.btn-link.active,.btn-link[disabled],fieldset[disabled] .btn-link{background-color:transparent;-webkit-box-shadow:none;box-shadow:none}.btn-link,.btn-link:hover,.btn-link:focus,.btn-link:active{border-color:transparent}.btn-link:hover,.btn-link:focus{color:#23527c;text-decoration:underline;background-color:transparent}.btn-link[disabled]:hover,fieldset[disabled] .btn-link:hover,.btn-link[disabled]:focus,fieldset[disabled] .btn-link:focus{color:#777;text-decoration:none}.btn-lg,.btn-group-lg>.btn{padding:10px 16px;font-size:18px;line-height:1.33;border-radius:6px}.btn-sm,.btn-group-sm>.btn{padding:5px 10px;font-size:12px;line-height:1.5;border-radius:3px}.btn-xs,.btn-group-xs>.btn{padding:1px 5px;font-size:12px;line-height:1.5;border-radius:3px}.btn-block{display:block;width:100%}.btn-block+.btn-block{margin-top:5px}input[type=submit].btn-block,input[type=reset].btn-block,input[type=button].btn-block{width:100%}.fade{opacity:0;-webkit-transition:opacity .15s linear;-o-transition:opacity .15s linear;transition:opacity .15s linear}.fade.in{opacity:1}.collapse{display:none;visibility:hidden}.collapse.in{display:block;visibility:visible}tr.collapse.in{display:table-row}tbody.collapse.in{display:table-row-group}.collapsing{position:relative;height:0;overflow:hidden;-webkit-transition-timing-function:ease;-o-transition-timing-function:ease;transition-timing-function:ease;-webkit-transition-duration:.35s;-o-transition-duration:.35s;transition-duration:.35s;-webkit-transition-property:height,visibility;-o-transition-property:height,visibility;transition-property:height,visibility}.caret{display:inline-block;width:0;height:0;margin-left:2px;vertical-align:middle;border-top:4px solid;border-right:4px solid transparent;border-left:4px solid transparent}.dropdown{position:relative}.dropdown-toggle:focus{outline:0}.dropdown-menu{position:absolute;top:100%;left:0;z-index:1000;display:none;float:left;min-width:160px;padding:5px 0;margin:2px 0 0;font-size:14px;text-align:left;list-style:none;background-color:#fff;-webkit-background-clip:padding-box;background-clip:padding-box;border:1px solid #ccc;border:1px solid rgba(0,0,0,.15);border-radius:4px;-webkit-box-shadow:0 6px 12px rgba(0,0,0,.175);box-shadow:0 6px 12px rgba(0,0,0,.175)}.dropdown-menu.pull-right{right:0;left:auto}.dropdown-menu .divider{height:1px;margin:9px 0;overflow:hidden;background-color:#e5e5e5}.dropdown-menu>li>a{display:block;padding:3px 20px;clear:both;font-weight:400;line-height:1.42857143;color:#333;white-space:nowrap}.dropdown-menu>li>a:hover,.dropdown-menu>li>a:focus{color:#262626;text-decoration:none;background-color:#f5f5f5}.dropdown-menu>.active>a,.dropdown-menu>.active>a:hover,.dropdown-menu>.active>a:focus{color:#fff;text-decoration:none;background-color:#337ab7;outline:0}.dropdown-menu>.disabled>a,.dropdown-menu>.disabled>a:hover,.dropdown-menu>.disabled>a:focus{color:#777}.dropdown-menu>.disabled>a:hover,.dropdown-menu>.disabled>a:focus{text-decoration:none;cursor:not-allowed;background-color:transparent;background-image:none;filter:progid:DXImageTransform.Microsoft.gradient(enabled=false)}.open>.dropdown-menu{display:block}.open>a{outline:0}.dropdown-menu-right{right:0;left:auto}.dropdown-menu-left{right:auto;left:0}.dropdown-header{display:block;padding:3px 20px;font-size:12px;line-height:1.42857143;color:#777;white-space:nowrap}.dropdown-backdrop{position:fixed;top:0;right:0;bottom:0;left:0;z-index:990}.pull-right>.dropdown-menu{right:0;left:auto}.dropup .caret,.navbar-fixed-bottom .dropdown .caret{content:"""";border-top:0;border-bottom:4px solid}.dropup .dropdown-menu,.navbar-fixed-bottom .dropdown .dropdown-menu{top:auto;bottom:100%;margin-bottom:1px}@media (min-width:768px){.navbar-right .dropdown-menu{right:0;left:auto}.navbar-right .dropdown-menu-left{right:auto;left:0}}.btn-group,.btn-group-vertical{position:relative;display:inline-block;vertical-align:middle}.btn-group>.btn,.btn-group-vertical>.btn{position:relative;float:left}.btn-group>.btn:hover,.btn-group-vertical>.btn:hover,.btn-group>.btn:focus,.btn-group-vertical>.btn:focus,.btn-group>.btn:active,.btn-group-vertical>.btn:active,.btn-group>.btn.active,.btn-group-vertical>.btn.active{z-index:2}.btn-group .btn+.btn,.btn-group .btn+.btn-group,.btn-group .btn-group+.btn,.btn-group .btn-group+.btn-group{margin-left:-1px}.btn-toolbar{margin-left:-5px}.btn-toolbar .btn-group,.btn-toolbar .input-group{float:left}.btn-toolbar>.btn,.btn-toolbar>.btn-group,.btn-toolbar>.input-group{margin-left:5px}.btn-group>.btn:not(:first-child):not(:last-child):not(.dropdown-toggle){border-radius:0}.btn-group>.btn:first-child{margin-left:0}.btn-group>.btn:first-child:not(:last-child):not(.dropdown-toggle){border-top-right-radius:0;border-bottom-right-radius:0}.btn-group>.btn:last-child:not(:first-child),.btn-group>.dropdown-toggle:not(:first-child){border-top-left-radius:0;border-bottom-left-radius:0}.btn-group>.btn-group{float:left}.btn-group>.btn-group:not(:first-child):not(:last-child)>.btn{border-radius:0}.btn-group>.btn-group:first-child>.btn:last-child,.btn-group>.btn-group:first-child>.dropdown-toggle{border-top-right-radius:0;border-bottom-right-radius:0}.btn-group>.btn-group:last-child>.btn:first-child{border-top-left-radius:0;border-bottom-left-radius:0}.btn-group .dropdown-toggle:active,.btn-group.open .dropdown-toggle{outline:0}.btn-group>.btn+.dropdown-toggle{padding-right:8px;padding-left:8px}.btn-group>.btn-lg+.dropdown-toggle{padding-right:12px;padding-left:12px}.btn-group.open .dropdown-toggle{-webkit-box-shadow:inset 0 3px 5px rgba(0,0,0,.125);box-shadow:inset 0 3px 5px rgba(0,0,0,.125)}.btn-group.open .dropdown-toggle.btn-link{-webkit-box-shadow:none;box-shadow:none}.btn .caret{margin-left:0}.btn-lg .caret{border-width:5px 5px 0;border-bottom-width:0}.dropup .btn-lg .caret{border-width:0 5px 5px}.btn-group-vertical>.btn,.btn-group-vertical>.btn-group,.btn-group-vertical>.btn-group>.btn{display:block;float:none;width:100%;max-width:100%}.btn-group-vertical>.btn-group>.btn{float:none}.btn-group-vertical>.btn+.btn,.btn-group-vertical>.btn+.btn-group,.btn-group-vertical>.btn-group+.btn,.btn-group-vertical>.btn-group+.btn-group{margin-top:-1px;margin-left:0}.btn-group-vertical>.btn:not(:first-child):not(:last-child){border-radius:0}.btn-group-vertical>.btn:first-child:not(:last-child){border-top-right-radius:4px;border-bottom-right-radius:0;border-bottom-left-radius:0}.btn-group-vertical>.btn:last-child:not(:first-child){border-top-left-radius:0;border-top-right-radius:0;border-bottom-left-radius:4px}.btn-group-vertical>.btn-group:not(:first-child):not(:last-child)>.btn{border-radius:0}.btn-group-vertical>.btn-group:first-child:not(:last-child)>.btn:last-child,.btn-group-vertical>.btn-group:first-child:not(:last-child)>.dropdown-toggle{border-bottom-right-radius:0;border-bottom-left-radius:0}.btn-group-vertical>.btn-group:last-child:not(:first-child)>.btn:first-child{border-top-left-radius:0;border-top-right-radius:0}.btn-group-justified{display:table;width:100%;table-layout:fixed;border-collapse:separate}.btn-group-justified>.btn,.btn-group-justified>.btn-group{display:table-cell;float:none;width:1%}.btn-group-justified>.btn-group .btn{width:100%}.btn-group-justified>.btn-group .dropdown-menu{left:auto}[data-toggle=buttons]>.btn input[type=radio],[data-toggle=buttons]>.btn-group>.btn input[type=radio],[data-toggle=buttons]>.btn input[type=checkbox],[data-toggle=buttons]>.btn-group>.btn input[type=checkbox]{position:absolute;clip:rect(0,0,0,0);pointer-events:none}.input-group{position:relative;display:table;border-collapse:separate}.input-group[class*=col-]{float:none;padding-right:0;padding-left:0}.input-group .form-control{position:relative;z-index:2;float:left;width:100%;margin-bottom:0}.input-group-lg>.form-control,.input-group-lg>.input-group-addon,.input-group-lg>.input-group-btn>.btn{height:46px;padding:10px 16px;font-size:18px;line-height:1.33;border-radius:6px}select.input-group-lg>.form-control,select.input-group-lg>.input-group-addon,select.input-group-lg>.input-group-btn>.btn{height:46px;line-height:46px}textarea.input-group-lg>.form-control,textarea.input-group-lg>.input-group-addon,textarea.input-group-lg>.input-group-btn>.btn,select[multiple].input-group-lg>.form-control,select[multiple].input-group-lg>.input-group-addon,select[multiple].input-group-lg>.input-group-btn>.btn{height:auto}.input-group-sm>.form-control,.input-group-sm>.input-group-addon,.input-group-sm>.input-group-btn>.btn{height:30px;padding:5px 10px;font-size:12px;line-height:1.5;border-radius:3px}select.input-group-sm>.form-control,select.input-group-sm>.input-group-addon,select.input-group-sm>.input-group-btn>.btn{height:30px;line-height:30px}textarea.input-group-sm>.form-control,textarea.input-group-sm>.input-group-addon,textarea.input-group-sm>.input-group-btn>.btn,select[multiple].input-group-sm>.form-control,select[multiple].input-group-sm>.input-group-addon,select[multiple].input-group-sm>.input-group-btn>.btn{height:auto}.input-group-addon,.input-group-btn,.input-group .form-control{display:table-cell}.input-group-addon:not(:first-child):not(:last-child),.input-group-btn:not(:first-child):not(:last-child),.input-group .form-control:not(:first-child):not(:last-child){border-radius:0}.input-group-addon,.input-group-btn{width:1%;white-space:nowrap;vertical-align:middle}.input-group-addon{padding:6px 12px;font-size:14px;font-weight:400;line-height:1;color:#555;text-align:center;background-color:#eee;border:1px solid #ccc;border-radius:4px}.input-group-addon.input-sm{padding:5px 10px;font-size:12px;border-radius:3px}.input-group-addon.input-lg{padding:10px 16px;font-size:18px;border-radius:6px}.input-group-addon input[type=radio],.input-group-addon input[type=checkbox]{margin-top:0}.input-group .form-control:first-child,.input-group-addon:first-child,.input-group-btn:first-child>.btn,.input-group-btn:first-child>.btn-group>.btn,.input-group-btn:first-child>.dropdown-toggle,.input-group-btn:last-child>.btn:not(:last-child):not(.dropdown-toggle),.input-group-btn:last-child>.btn-group:not(:last-child)>.btn{border-top-right-radius:0;border-bottom-right-radius:0}.input-group-addon:first-child{border-right:0}.input-group .form-control:last-child,.input-group-addon:last-child,.input-group-btn:last-child>.btn,.input-group-btn:last-child>.btn-group>.btn,.input-group-btn:last-child>.dropdown-toggle,.input-group-btn:first-child>.btn:not(:first-child),.input-group-btn:first-child>.btn-group:not(:first-child)>.btn{border-top-left-radius:0;border-bottom-left-radius:0}.input-group-addon:last-child{border-left:0}.input-group-btn{position:relative;font-size:0;white-space:nowrap}.input-group-btn>.btn{position:relative}.input-group-btn>.btn+.btn{margin-left:-1px}.input-group-btn>.btn:hover,.input-group-btn>.btn:focus,.input-group-btn>.btn:active{z-index:2}.input-group-btn:first-child>.btn,.input-group-btn:first-child>.btn-group{margin-right:-1px}.input-group-btn:last-child>.btn,.input-group-btn:last-child>.btn-group{margin-left:-1px}.nav{padding-left:0;margin-bottom:0;list-style:none}.nav>li{position:relative;display:block}.nav>li>a{position:relative;display:block;padding:10px 15px}.nav>li>a:hover,.nav>li>a:focus{text-decoration:none;background-color:#eee}.nav>li.disabled>a{color:#777}.nav>li.disabled>a:hover,.nav>li.disabled>a:focus{color:#777;text-decoration:none;cursor:not-allowed;background-color:transparent}.nav .open>a,.nav .open>a:hover,.nav .open>a:focus{background-color:#eee;border-color:#337ab7}.nav .nav-divider{height:1px;margin:9px 0;overflow:hidden;background-color:#e5e5e5}.nav>li>a>img{max-width:none}.nav-tabs{border-bottom:1px solid #ddd}.nav-tabs>li{float:left;margin-bottom:-1px}.nav-tabs>li>a{margin-right:2px;line-height:1.42857143;border:1px solid transparent;border-radius:4px 4px 0 0}.nav-tabs>li>a:hover{border-color:#eee #eee #ddd}.nav-tabs>li.active>a,.nav-tabs>li.active>a:hover,.nav-tabs>li.active>a:focus{color:#555;cursor:default;background-color:#fff;border:1px solid #ddd;border-bottom-color:transparent}.nav-tabs.nav-justified{width:100%;border-bottom:0}.nav-tabs.nav-justified>li{float:none}.nav-tabs.nav-justified>li>a{margin-bottom:5px;text-align:center}.nav-tabs.nav-justified>.dropdown .dropdown-menu{top:auto;left:auto}@media (min-width:768px){.nav-tabs.nav-justified>li{display:table-cell;width:1%}.nav-tabs.nav-justified>li>a{margin-bottom:0}}.nav-tabs.nav-justified>li>a{margin-right:0;border-radius:4px}.nav-tabs.nav-justified>.active>a,.nav-tabs.nav-justified>.active>a:hover,.nav-tabs.nav-justified>.active>a:focus{border:1px solid #ddd}@media (min-width:768px){.nav-tabs.nav-justified>li>a{border-bottom:1px solid #ddd;border-radius:4px 4px 0 0}.nav-tabs.nav-justified>.active>a,.nav-tabs.nav-justified>.active>a:hover,.nav-tabs.nav-justified>.active>a:focus{border-bottom-color:#fff}}.nav-pills>li{float:left}.nav-pills>li>a{border-radius:4px}.nav-pills>li+li{margin-left:2px}.nav-pills>li.active>a,.nav-pills>li.active>a:hover,.nav-pills>li.active>a:focus{color:#fff;background-color:#337ab7}.nav-stacked>li{float:none}.nav-stacked>li+li{margin-top:2px;margin-left:0}.nav-justified{width:100%}.nav-justified>li{float:none}.nav-justified>li>a{margin-bottom:5px;text-align:center}.nav-justified>.dropdown .dropdown-menu{top:auto;left:auto}@media (min-width:768px){.nav-justified>li{display:table-cell;width:1%}.nav-justified>li>a{margin-bottom:0}}.nav-tabs-justified{border-bottom:0}.nav-tabs-justified>li>a{margin-right:0;border-radius:4px}.nav-tabs-justified>.active>a,.nav-tabs-justified>.active>a:hover,.nav-tabs-justified>.active>a:focus{border:1px solid #ddd}@media (min-width:768px){.nav-tabs-justified>li>a{border-bottom:1px solid #ddd;border-radius:4px 4px 0 0}.nav-tabs-justified>.active>a,.nav-tabs-justified>.active>a:hover,.nav-tabs-justified>.active>a:focus{border-bottom-color:#fff}}.tab-content>.tab-pane{display:none;visibility:hidden}.tab-content>.active{display:block;visibility:visible}.nav-tabs .dropdown-menu{margin-top:-1px;border-top-left-radius:0;border-top-right-radius:0}.navbar{position:relative;min-height:50px;margin-bottom:20px;border:1px solid transparent}@media (min-width:768px){.navbar{border-radius:4px}}@media (min-width:768px){.navbar-header{float:left}}.navbar-collapse{padding-right:15px;padding-left:15px;overflow-x:visible;-webkit-overflow-scrolling:touch;border-top:1px solid transparent;-webkit-box-shadow:inset 0 1px 0 rgba(255,255,255,.1);box-shadow:inset 0 1px 0 rgba(255,255,255,.1)}.navbar-collapse.in{overflow-y:auto}@media (min-width:768px){.navbar-collapse{width:auto;border-top:0;-webkit-box-shadow:none;box-shadow:none}.navbar-collapse.collapse{display:block!important;height:auto!important;padding-bottom:0;overflow:visible!important;visibility:visible!important}.navbar-collapse.in{overflow-y:visible}.navbar-fixed-top .navbar-collapse,.navbar-static-top .navbar-collapse,.navbar-fixed-bottom .navbar-collapse{padding-right:0;padding-left:0}}.navbar-fixed-top .navbar-collapse,.navbar-fixed-bottom .navbar-collapse{max-height:340px}@media (max-device-width:480px) and (orientation:landscape){.navbar-fixed-top .navbar-collapse,.navbar-fixed-bottom .navbar-collapse{max-height:200px}}.container>.navbar-header,.container-fluid>.navbar-header,.container>.navbar-collapse,.container-fluid>.navbar-collapse{margin-right:-15px;margin-left:-15px}@media (min-width:768px){.container>.navbar-header,.container-fluid>.navbar-header,.container>.navbar-collapse,.container-fluid>.navbar-collapse{margin-right:0;margin-left:0}}.navbar-static-top{z-index:1000;border-width:0 0 1px}@media (min-width:768px){.navbar-static-top{border-radius:0}}.navbar-fixed-top,.navbar-fixed-bottom{position:fixed;right:0;left:0;z-index:1030}@media (min-width:768px){.navbar-fixed-top,.navbar-fixed-bottom{border-radius:0}}.navbar-fixed-top{top:0;border-width:0 0 1px}.navbar-fixed-bottom{bottom:0;margin-bottom:0;border-width:1px 0 0}.navbar-brand{float:left;height:50px;padding:15px 15px;font-size:18px;line-height:20px}.navbar-brand:hover,.navbar-brand:focus{text-decoration:none}.navbar-brand>img{display:block}@media (min-width:768px){.navbar>.container .navbar-brand,.navbar>.container-fluid .navbar-brand{margin-left:-15px}}.navbar-toggle{position:relative;float:right;padding:9px 10px;margin-top:8px;margin-right:15px;margin-bottom:8px;background-color:transparent;background-image:none;border:1px solid transparent;border-radius:4px}.navbar-toggle:focus{outline:0}.navbar-toggle .icon-bar{display:block;width:22px;height:2px;border-radius:1px}.navbar-toggle .icon-bar+.icon-bar{margin-top:4px}@media (min-width:768px){.navbar-toggle{display:none}}.navbar-nav{margin:7.5px -15px}.navbar-nav>li>a{padding-top:10px;padding-bottom:10px;line-height:20px}@media (max-width:767px){.navbar-nav .open .dropdown-menu{position:static;float:none;width:auto;margin-top:0;background-color:transparent;border:0;-webkit-box-shadow:none;box-shadow:none}.navbar-nav .open .dropdown-menu>li>a,.navbar-nav .open .dropdown-menu .dropdown-header{padding:5px 15px 5px 25px}.navbar-nav .open .dropdown-menu>li>a{line-height:20px}.navbar-nav .open .dropdown-menu>li>a:hover,.navbar-nav .open .dropdown-menu>li>a:focus{background-image:none}}@media (min-width:768px){.navbar-nav{float:left;margin:0}.navbar-nav>li{float:left}.navbar-nav>li>a{padding-top:15px;padding-bottom:15px}}.navbar-form{padding:10px 15px;margin-top:8px;margin-right:-15px;margin-bottom:8px;margin-left:-15px;border-top:1px solid transparent;border-bottom:1px solid transparent;-webkit-box-shadow:inset 0 1px 0 rgba(255,255,255,.1),0 1px 0 rgba(255,255,255,.1);box-shadow:inset 0 1px 0 rgba(255,255,255,.1),0 1px 0 rgba(255,255,255,.1)}@media (min-width:768px){.navbar-form .form-group{display:inline-block;margin-bottom:0;vertical-align:middle}.navbar-form .form-control{display:inline-block;width:auto;vertical-align:middle}.navbar-form .form-control-static{display:inline-block}.navbar-form .input-group{display:inline-table;vertical-align:middle}.navbar-form .input-group .input-group-addon,.navbar-form .input-group .input-group-btn,.navbar-form .input-group .form-control{width:auto}.navbar-form .input-group>.form-control{width:100%}.navbar-form .control-label{margin-bottom:0;vertical-align:middle}.navbar-form .radio,.navbar-form .checkbox{display:inline-block;margin-top:0;margin-bottom:0;vertical-align:middle}.navbar-form .radio label,.navbar-form .checkbox label{padding-left:0}.navbar-form .radio input[type=radio],.navbar-form .checkbox input[type=checkbox]{position:relative;margin-left:0}.navbar-form .has-feedback .form-control-feedback{top:0}}@media (max-width:767px){.navbar-form .form-group{margin-bottom:5px}.navbar-form .form-group:last-child{margin-bottom:0}}@media (min-width:768px){.navbar-form{width:auto;padding-top:0;padding-bottom:0;margin-right:0;margin-left:0;border:0;-webkit-box-shadow:none;box-shadow:none}}.navbar-nav>li>.dropdown-menu{margin-top:0;border-top-left-radius:0;border-top-right-radius:0}.navbar-fixed-bottom .navbar-nav>li>.dropdown-menu{border-top-left-radius:4px;border-top-right-radius:4px;border-bottom-right-radius:0;border-bottom-left-radius:0}.navbar-btn{margin-top:8px;margin-bottom:8px}.navbar-btn.btn-sm{margin-top:10px;margin-bottom:10px}.navbar-btn.btn-xs{margin-top:14px;margin-bottom:14px}.navbar-text{margin-top:15px;margin-bottom:15px}@media (min-width:768px){.navbar-text{float:left;margin-right:15px;margin-left:15px}}@media (min-width:768px){.navbar-left{float:left!important}.navbar-right{float:right!important;margin-right:-15px}.navbar-right~.navbar-right{margin-right:0}}.navbar-default{background-color:#f8f8f8;border-color:#e7e7e7}.navbar-default .navbar-brand{color:#777}.navbar-default .navbar-brand:hover,.navbar-default .navbar-brand:focus{color:#5e5e5e;background-color:transparent}.navbar-default .navbar-text{color:#777}.navbar-default .navbar-nav>li>a{color:#777}.navbar-default .navbar-nav>li>a:hover,.navbar-default .navbar-nav>li>a:focus{color:#333;background-color:transparent}.navbar-default .navbar-nav>.active>a,.navbar-default .navbar-nav>.active>a:hover,.navbar-default .navbar-nav>.active>a:focus{color:#555;background-color:#e7e7e7}.navbar-default .navbar-nav>.disabled>a,.navbar-default .navbar-nav>.disabled>a:hover,.navbar-default .navbar-nav>.disabled>a:focus{color:#ccc;background-color:transparent}.navbar-default .navbar-toggle{border-color:#ddd}.navbar-default .navbar-toggle:hover,.navbar-default .navbar-toggle:focus{background-color:#ddd}.navbar-default .navbar-toggle .icon-bar{background-color:#888}.navbar-default .navbar-collapse,.navbar-default .navbar-form{border-color:#e7e7e7}.navbar-default .navbar-nav>.open>a,.navbar-default .navbar-nav>.open>a:hover,.navbar-default .navbar-nav>.open>a:focus{color:#555;background-color:#e7e7e7}@media (max-width:767px){.navbar-default .navbar-nav .open .dropdown-menu>li>a{color:#777}.navbar-default .navbar-nav .open .dropdown-menu>li>a:hover,.navbar-default .navbar-nav .open .dropdown-menu>li>a:focus{color:#333;background-color:transparent}.navbar-default .navbar-nav .open .dropdown-menu>.active>a,.navbar-default .navbar-nav .open .dropdown-menu>.active>a:hover,.navbar-default .navbar-nav .open .dropdown-menu>.active>a:focus{color:#555;background-color:#e7e7e7}.navbar-default .navbar-nav .open .dropdown-menu>.disabled>a,.navbar-default .navbar-nav .open .dropdown-menu>.disabled>a:hover,.navbar-default .navbar-nav .open .dropdown-menu>.disabled>a:focus{color:#ccc;background-color:transparent}}.navbar-default .navbar-link{color:#777}.navbar-default .navbar-link:hover{color:#333}.navbar-default .btn-link{color:#777}.navbar-default .btn-link:hover,.navbar-default .btn-link:focus{color:#333}.navbar-default .btn-link[disabled]:hover,fieldset[disabled] .navbar-default .btn-link:hover,.navbar-default .btn-link[disabled]:focus,fieldset[disabled] .navbar-default .btn-link:focus{color:#ccc}.navbar-inverse{background-color:#222;border-color:#080808}.navbar-inverse .navbar-brand{color:#9d9d9d}.navbar-inverse .navbar-brand:hover,.navbar-inverse .navbar-brand:focus{color:#fff;background-color:transparent}.navbar-inverse .navbar-text{color:#9d9d9d}.navbar-inverse .navbar-nav>li>a{color:#9d9d9d}.navbar-inverse .navbar-nav>li>a:hover,.navbar-inverse .navbar-nav>li>a:focus{color:#fff;background-color:transparent}.navbar-inverse .navbar-nav>.active>a,.navbar-inverse .navbar-nav>.active>a:hover,.navbar-inverse .navbar-nav>.active>a:focus{color:#fff;background-color:#080808}.navbar-inverse .navbar-nav>.disabled>a,.navbar-inverse .navbar-nav>.disabled>a:hover,.navbar-inverse .navbar-nav>.disabled>a:focus{color:#444;background-color:transparent}.navbar-inverse .navbar-toggle{border-color:#333}.navbar-inverse .navbar-toggle:hover,.navbar-inverse .navbar-toggle:focus{background-color:#333}.navbar-inverse .navbar-toggle .icon-bar{background-color:#fff}.navbar-inverse .navbar-collapse,.navbar-inverse .navbar-form{border-color:#101010}.navbar-inverse .navbar-nav>.open>a,.navbar-inverse .navbar-nav>.open>a:hover,.navbar-inverse .navbar-nav>.open>a:focus{color:#fff;background-color:#080808}@media (max-width:767px){.navbar-inverse .navbar-nav .open .dropdown-menu>.dropdown-header{border-color:#080808}.navbar-inverse .navbar-nav .open .dropdown-menu .divider{background-color:#080808}.navbar-inverse .navbar-nav .open .dropdown-menu>li>a{color:#9d9d9d}.navbar-inverse .navbar-nav .open .dropdown-menu>li>a:hover,.navbar-inverse .navbar-nav .open .dropdown-menu>li>a:focus{color:#fff;background-color:transparent}.navbar-inverse .navbar-nav .open .dropdown-menu>.active>a,.navbar-inverse .navbar-nav .open .dropdown-menu>.active>a:hover,.navbar-inverse .navbar-nav .open .dropdown-menu>.active>a:focus{color:#fff;background-color:#080808}.navbar-inverse .navbar-nav .open .dropdown-menu>.disabled>a,.navbar-inverse .navbar-nav .open .dropdown-menu>.disabled>a:hover,.navbar-inverse .navbar-nav .open .dropdown-menu>.disabled>a:focus{color:#444;background-color:transparent}}.navbar-inverse .navbar-link{color:#9d9d9d}.navbar-inverse .navbar-link:hover{color:#fff}.navbar-inverse .btn-link{color:#9d9d9d}.navbar-inverse .btn-link:hover,.navbar-inverse .btn-link:focus{color:#fff}.navbar-inverse .btn-link[disabled]:hover,fieldset[disabled] .navbar-inverse .btn-link:hover,.navbar-inverse .btn-link[disabled]:focus,fieldset[disabled] .navbar-inverse .btn-link:focus{color:#444}.breadcrumb{padding:8px 15px;margin-bottom:20px;list-style:none;background-color:#f5f5f5;border-radius:4px}.breadcrumb>li{display:inline-block}.breadcrumb>li+li:before{padding:0 5px;color:#ccc;content:""/\00a0""}.breadcrumb>.active{color:#777}.pagination{display:inline-block;padding-left:0;margin:20px 0;border-radius:4px}.pagination>li{display:inline}.pagination>li>a,.pagination>li>span{position:relative;float:left;padding:6px 12px;margin-left:-1px;line-height:1.42857143;color:#337ab7;text-decoration:none;background-color:#fff;border:1px solid #ddd}.pagination>li:first-child>a,.pagination>li:first-child>span{margin-left:0;border-top-left-radius:4px;border-bottom-left-radius:4px}.pagination>li:last-child>a,.pagination>li:last-child>span{border-top-right-radius:4px;border-bottom-right-radius:4px}.pagination>li>a:hover,.pagination>li>span:hover,.pagination>li>a:focus,.pagination>li>span:focus{color:#23527c;background-color:#eee;border-color:#ddd}.pagination>.active>a,.pagination>.active>span,.pagination>.active>a:hover,.pagination>.active>span:hover,.pagination>.active>a:focus,.pagination>.active>span:focus{z-index:2;color:#fff;cursor:default;background-color:#337ab7;border-color:#337ab7}.pagination>.disabled>span,.pagination>.disabled>span:hover,.pagination>.disabled>span:focus,.pagination>.disabled>a,.pagination>.disabled>a:hover,.pagination>.disabled>a:focus{color:#777;cursor:not-allowed;background-color:#fff;border-color:#ddd}.pagination-lg>li>a,.pagination-lg>li>span{padding:10px 16px;font-size:18px}.pagination-lg>li:first-child>a,.pagination-lg>li:first-child>span{border-top-left-radius:6px;border-bottom-left-radius:6px}.pagination-lg>li:last-child>a,.pagination-lg>li:last-child>span{border-top-right-radius:6px;border-bottom-right-radius:6px}.pagination-sm>li>a,.pagination-sm>li>span{padding:5px 10px;font-size:12px}.pagination-sm>li:first-child>a,.pagination-sm>li:first-child>span{border-top-left-radius:3px;border-bottom-left-radius:3px}.pagination-sm>li:last-child>a,.pagination-sm>li:last-child>span{border-top-right-radius:3px;border-bottom-right-radius:3px}.pager{padding-left:0;margin:20px 0;text-align:center;list-style:none}.pager li{display:inline}.pager li>a,.pager li>span{display:inline-block;padding:5px 14px;background-color:#fff;border:1px solid #ddd;border-radius:15px}.pager li>a:hover,.pager li>a:focus{text-decoration:none;background-color:#eee}.pager .next>a,.pager .next>span{float:right}.pager .previous>a,.pager .previous>span{float:left}.pager .disabled>a,.pager .disabled>a:hover,.pager .disabled>a:focus,.pager .disabled>span{color:#777;cursor:not-allowed;background-color:#fff}.label{display:inline;padding:.2em .6em .3em;font-size:75%;font-weight:700;line-height:1;color:#fff;text-align:center;white-space:nowrap;vertical-align:baseline;border-radius:.25em}a.label:hover,a.label:focus{color:#fff;text-decoration:none;cursor:pointer}.label:empty{display:none}.btn .label{position:relative;top:-1px}.label-default{background-color:#777}.label-default[href]:hover,.label-default[href]:focus{background-color:#5e5e5e}.label-primary{background-color:#337ab7}.label-primary[href]:hover,.label-primary[href]:focus{background-color:#286090}.label-success{background-color:#5cb85c}.label-success[href]:hover,.label-success[href]:focus{background-color:#449d44}.label-info{background-color:#5bc0de}.label-info[href]:hover,.label-info[href]:focus{background-color:#31b0d5}.label-warning{background-color:#f0ad4e}.label-warning[href]:hover,.label-warning[href]:focus{background-color:#ec971f}.label-danger{background-color:#d9534f}.label-danger[href]:hover,.label-danger[href]:focus{background-color:#c9302c}.badge{display:inline-block;min-width:10px;padding:3px 7px;font-size:12px;font-weight:700;line-height:1;color:#fff;text-align:center;white-space:nowrap;vertical-align:baseline;background-color:#777;border-radius:10px}.badge:empty{display:none}.btn .badge{position:relative;top:-1px}.btn-xs .badge{top:0;padding:1px 5px}a.badge:hover,a.badge:focus{color:#fff;text-decoration:none;cursor:pointer}.list-group-item.active>.badge,.nav-pills>.active>a>.badge{color:#337ab7;background-color:#fff}.list-group-item>.badge{float:right}.list-group-item>.badge+.badge{margin-right:5px}.nav-pills>li>a>.badge{margin-left:3px}.jumbotron{padding:30px 15px;margin-bottom:30px;color:inherit;background-color:#eee}.jumbotron h1,.jumbotron .h1{color:inherit}.jumbotron p{margin-bottom:15px;font-size:21px;font-weight:200}.jumbotron>hr{border-top-color:#d5d5d5}.container .jumbotron,.container-fluid .jumbotron{border-radius:6px}.jumbotron .container{max-width:100%}@media screen and (min-width:768px){.jumbotron{padding:48px 0}.container .jumbotron,.container-fluid .jumbotron{padding-right:60px;padding-left:60px}.jumbotron h1,.jumbotron .h1{font-size:63px}}.thumbnail{display:block;padding:4px;margin-bottom:20px;line-height:1.42857143;background-color:#fff;border:1px solid #ddd;border-radius:4px;-webkit-transition:border .2s ease-in-out;-o-transition:border .2s ease-in-out;transition:border .2s ease-in-out}.thumbnail>img,.thumbnail a>img{margin-right:auto;margin-left:auto}a.thumbnail:hover,a.thumbnail:focus,a.thumbnail.active{border-color:#337ab7}.thumbnail .caption{padding:9px;color:#333}.alert{padding:15px;margin-bottom:20px;border:1px solid transparent;border-radius:4px}.alert h4{margin-top:0;color:inherit}.alert .alert-link{font-weight:700}.alert>p,.alert>ul{margin-bottom:0}.alert>p+p{margin-top:5px}.alert-dismissable,.alert-dismissible{padding-right:35px}.alert-dismissable .close,.alert-dismissible .close{position:relative;top:-2px;right:-21px;color:inherit}.alert-success{color:#3c763d;background-color:#dff0d8;border-color:#d6e9c6}.alert-success hr{border-top-color:#c9e2b3}.alert-success .alert-link{color:#2b542c}.alert-info{color:#31708f;background-color:#d9edf7;border-color:#bce8f1}.alert-info hr{border-top-color:#a6e1ec}.alert-info .alert-link{color:#245269}.alert-warning{color:#8a6d3b;background-color:#fcf8e3;border-color:#faebcc}.alert-warning hr{border-top-color:#f7e1b5}.alert-warning .alert-link{color:#66512c}.alert-danger{color:#a94442;background-color:#f2dede;border-color:#ebccd1}.alert-danger hr{border-top-color:#e4b9c0}.alert-danger .alert-link{color:#843534}@-webkit-keyframes progress-bar-stripes{from{background-position:40px 0}to{background-position:0 0}}@-o-keyframes progress-bar-stripes{from{background-position:40px 0}to{background-position:0 0}}@keyframes progress-bar-stripes{from{background-position:40px 0}to{background-position:0 0}}.progress{height:20px;margin-bottom:20px;overflow:hidden;background-color:#f5f5f5;border-radius:4px;-webkit-box-shadow:inset 0 1px 2px rgba(0,0,0,.1);box-shadow:inset 0 1px 2px rgba(0,0,0,.1)}.progress-bar{float:left;width:0;height:100%;font-size:12px;line-height:20px;color:#fff;text-align:center;background-color:#337ab7;-webkit-box-shadow:inset 0 -1px 0 rgba(0,0,0,.15);box-shadow:inset 0 -1px 0 rgba(0,0,0,.15);-webkit-transition:width .6s ease;-o-transition:width .6s ease;transition:width .6s ease}.progress-striped .progress-bar,.progress-bar-striped{background-image:-webkit-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:-o-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);-webkit-background-size:40px 40px;background-size:40px 40px}.progress.active .progress-bar,.progress-bar.active{-webkit-animation:progress-bar-stripes 2s linear infinite;-o-animation:progress-bar-stripes 2s linear infinite;animation:progress-bar-stripes 2s linear infinite}.progress-bar-success{background-color:#5cb85c}.progress-striped .progress-bar-success{background-image:-webkit-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:-o-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent)}.progress-bar-info{background-color:#5bc0de}.progress-striped .progress-bar-info{background-image:-webkit-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:-o-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent)}.progress-bar-warning{background-color:#f0ad4e}.progress-striped .progress-bar-warning{background-image:-webkit-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:-o-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent)}.progress-bar-danger{background-color:#d9534f}.progress-striped .progress-bar-danger{background-image:-webkit-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:-o-linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);background-image:linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent)}.media{margin-top:15px}.media:first-child{margin-top:0}.media-right,.media>.pull-right{padding-left:10px}.media-left,.media>.pull-left{padding-right:10px}.media-left,.media-right,.media-body{display:table-cell;vertical-align:top}.media-middle{vertical-align:middle}.media-bottom{vertical-align:bottom}.media-heading{margin-top:0;margin-bottom:5px}.media-list{padding-left:0;list-style:none}.list-group{padding-left:0;margin-bottom:20px}.list-group-item{position:relative;display:block;padding:10px 15px;margin-bottom:-1px;background-color:#fff;border:1px solid #ddd}.list-group-item:first-child{border-top-left-radius:4px;border-top-right-radius:4px}.list-group-item:last-child{margin-bottom:0;border-bottom-right-radius:4px;border-bottom-left-radius:4px}a.list-group-item{color:#555}a.list-group-item .list-group-item-heading{color:#333}a.list-group-item:hover,a.list-group-item:focus{color:#555;text-decoration:none;background-color:#f5f5f5}.list-group-item.disabled,.list-group-item.disabled:hover,.list-group-item.disabled:focus{color:#777;cursor:not-allowed;background-color:#eee}.list-group-item.disabled .list-group-item-heading,.list-group-item.disabled:hover .list-group-item-heading,.list-group-item.disabled:focus .list-group-item-heading{color:inherit}.list-group-item.disabled .list-group-item-text,.list-group-item.disabled:hover .list-group-item-text,.list-group-item.disabled:focus .list-group-item-text{color:#777}.list-group-item.active,.list-group-item.active:hover,.list-group-item.active:focus{z-index:2;color:#fff;background-color:#337ab7;border-color:#337ab7}.list-group-item.active .list-group-item-heading,.list-group-item.active:hover .list-group-item-heading,.list-group-item.active:focus .list-group-item-heading,.list-group-item.active .list-group-item-heading>small,.list-group-item.active:hover .list-group-item-heading>small,.list-group-item.active:focus .list-group-item-heading>small,.list-group-item.active .list-group-item-heading>.small,.list-group-item.active:hover .list-group-item-heading>.small,.list-group-item.active:focus .list-group-item-heading>.small{color:inherit}.list-group-item.active .list-group-item-text,.list-group-item.active:hover .list-group-item-text,.list-group-item.active:focus .list-group-item-text{color:#c7ddef}.list-group-item-success{color:#3c763d;background-color:#dff0d8}a.list-group-item-success{color:#3c763d}a.list-group-item-success .list-group-item-heading{color:inherit}a.list-group-item-success:hover,a.list-group-item-success:focus{color:#3c763d;background-color:#d0e9c6}a.list-group-item-success.active,a.list-group-item-success.active:hover,a.list-group-item-success.active:focus{color:#fff;background-color:#3c763d;border-color:#3c763d}.list-group-item-info{color:#31708f;background-color:#d9edf7}a.list-group-item-info{color:#31708f}a.list-group-item-info .list-group-item-heading{color:inherit}a.list-group-item-info:hover,a.list-group-item-info:focus{color:#31708f;background-color:#c4e3f3}a.list-group-item-info.active,a.list-group-item-info.active:hover,a.list-group-item-info.active:focus{color:#fff;background-color:#31708f;border-color:#31708f}.list-group-item-warning{color:#8a6d3b;background-color:#fcf8e3}a.list-group-item-warning{color:#8a6d3b}a.list-group-item-warning .list-group-item-heading{color:inherit}a.list-group-item-warning:hover,a.list-group-item-warning:focus{color:#8a6d3b;background-color:#faf2cc}a.list-group-item-warning.active,a.list-group-item-warning.active:hover,a.list-group-item-warning.active:focus{color:#fff;background-color:#8a6d3b;border-color:#8a6d3b}.list-group-item-danger{color:#a94442;background-color:#f2dede}a.list-group-item-danger{color:#a94442}a.list-group-item-danger .list-group-item-heading{color:inherit}a.list-group-item-danger:hover,a.list-group-item-danger:focus{color:#a94442;background-color:#ebcccc}a.list-group-item-danger.active,a.list-group-item-danger.active:hover,a.list-group-item-danger.active:focus{color:#fff;background-color:#a94442;border-color:#a94442}.list-group-item-heading{margin-top:0;margin-bottom:5px}.list-group-item-text{margin-bottom:0;line-height:1.3}.panel{margin-bottom:20px;background-color:#fff;border:1px solid transparent;border-radius:4px;-webkit-box-shadow:0 1px 1px rgba(0,0,0,.05);box-shadow:0 1px 1px rgba(0,0,0,.05)}.panel-body{padding:15px}.panel-heading{padding:10px 15px;border-bottom:1px solid transparent;border-top-left-radius:3px;border-top-right-radius:3px}.panel-heading>.dropdown .dropdown-toggle{color:inherit}.panel-title{margin-top:0;margin-bottom:0;font-size:16px;color:inherit}.panel-title>a{color:inherit}.panel-footer{padding:10px 15px;background-color:#f5f5f5;border-top:1px solid #ddd;border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel>.list-group,.panel>.panel-collapse>.list-group{margin-bottom:0}.panel>.list-group .list-group-item,.panel>.panel-collapse>.list-group .list-group-item{border-width:1px 0;border-radius:0}.panel>.list-group:first-child .list-group-item:first-child,.panel>.panel-collapse>.list-group:first-child .list-group-item:first-child{border-top:0;border-top-left-radius:3px;border-top-right-radius:3px}.panel>.list-group:last-child .list-group-item:last-child,.panel>.panel-collapse>.list-group:last-child .list-group-item:last-child{border-bottom:0;border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel-heading+.list-group .list-group-item:first-child{border-top-width:0}.list-group+.panel-footer{border-top-width:0}.panel>.table,.panel>.table-responsive>.table,.panel>.panel-collapse>.table{margin-bottom:0}.panel>.table caption,.panel>.table-responsive>.table caption,.panel>.panel-collapse>.table caption{padding-right:15px;padding-left:15px}.panel>.table:first-child,.panel>.table-responsive:first-child>.table:first-child{border-top-left-radius:3px;border-top-right-radius:3px}.panel>.table:first-child>thead:first-child>tr:first-child,.panel>.table-responsive:first-child>.table:first-child>thead:first-child>tr:first-child,.panel>.table:first-child>tbody:first-child>tr:first-child,.panel>.table-responsive:first-child>.table:first-child>tbody:first-child>tr:first-child{border-top-left-radius:3px;border-top-right-radius:3px}.panel>.table:first-child>thead:first-child>tr:first-child td:first-child,.panel>.table-responsive:first-child>.table:first-child>thead:first-child>tr:first-child td:first-child,.panel>.table:first-child>tbody:first-child>tr:first-child td:first-child,.panel>.table-responsive:first-child>.table:first-child>tbody:first-child>tr:first-child td:first-child,.panel>.table:first-child>thead:first-child>tr:first-child th:first-child,.panel>.table-responsive:first-child>.table:first-child>thead:first-child>tr:first-child th:first-child,.panel>.table:first-child>tbody:first-child>tr:first-child th:first-child,.panel>.table-responsive:first-child>.table:first-child>tbody:first-child>tr:first-child th:first-child{border-top-left-radius:3px}.panel>.table:first-child>thead:first-child>tr:first-child td:last-child,.panel>.table-responsive:first-child>.table:first-child>thead:first-child>tr:first-child td:last-child,.panel>.table:first-child>tbody:first-child>tr:first-child td:last-child,.panel>.table-responsive:first-child>.table:first-child>tbody:first-child>tr:first-child td:last-child,.panel>.table:first-child>thead:first-child>tr:first-child th:last-child,.panel>.table-responsive:first-child>.table:first-child>thead:first-child>tr:first-child th:last-child,.panel>.table:first-child>tbody:first-child>tr:first-child th:last-child,.panel>.table-responsive:first-child>.table:first-child>tbody:first-child>tr:first-child th:last-child{border-top-right-radius:3px}.panel>.table:last-child,.panel>.table-responsive:last-child>.table:last-child{border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel>.table:last-child>tbody:last-child>tr:last-child,.panel>.table-responsive:last-child>.table:last-child>tbody:last-child>tr:last-child,.panel>.table:last-child>tfoot:last-child>tr:last-child,.panel>.table-responsive:last-child>.table:last-child>tfoot:last-child>tr:last-child{border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel>.table:last-child>tbody:last-child>tr:last-child td:first-child,.panel>.table-responsive:last-child>.table:last-child>tbody:last-child>tr:last-child td:first-child,.panel>.table:last-child>tfoot:last-child>tr:last-child td:first-child,.panel>.table-responsive:last-child>.table:last-child>tfoot:last-child>tr:last-child td:first-child,.panel>.table:last-child>tbody:last-child>tr:last-child th:first-child,.panel>.table-responsive:last-child>.table:last-child>tbody:last-child>tr:last-child th:first-child,.panel>.table:last-child>tfoot:last-child>tr:last-child th:first-child,.panel>.table-responsive:last-child>.table:last-child>tfoot:last-child>tr:last-child th:first-child{border-bottom-left-radius:3px}.panel>.table:last-child>tbody:last-child>tr:last-child td:last-child,.panel>.table-responsive:last-child>.table:last-child>tbody:last-child>tr:last-child td:last-child,.panel>.table:last-child>tfoot:last-child>tr:last-child td:last-child,.panel>.table-responsive:last-child>.table:last-child>tfoot:last-child>tr:last-child td:last-child,.panel>.table:last-child>tbody:last-child>tr:last-child th:last-child,.panel>.table-responsive:last-child>.table:last-child>tbody:last-child>tr:last-child th:last-child,.panel>.table:last-child>tfoot:last-child>tr:last-child th:last-child,.panel>.table-responsive:last-child>.table:last-child>tfoot:last-child>tr:last-child th:last-child{border-bottom-right-radius:3px}.panel>.panel-body+.table,.panel>.panel-body+.table-responsive,.panel>.table+.panel-body,.panel>.table-responsive+.panel-body{border-top:1px solid #ddd}.panel>.table>tbody:first-child>tr:first-child th,.panel>.table>tbody:first-child>tr:first-child td{border-top:0}.panel>.table-bordered,.panel>.table-responsive>.table-bordered{border:0}.panel>.table-bordered>thead>tr>th:first-child,.panel>.table-responsive>.table-bordered>thead>tr>th:first-child,.panel>.table-bordered>tbody>tr>th:first-child,.panel>.table-responsive>.table-bordered>tbody>tr>th:first-child,.panel>.table-bordered>tfoot>tr>th:first-child,.panel>.table-responsive>.table-bordered>tfoot>tr>th:first-child,.panel>.table-bordered>thead>tr>td:first-child,.panel>.table-responsive>.table-bordered>thead>tr>td:first-child,.panel>.table-bordered>tbody>tr>td:first-child,.panel>.table-responsive>.table-bordered>tbody>tr>td:first-child,.panel>.table-bordered>tfoot>tr>td:first-child,.panel>.table-responsive>.table-bordered>tfoot>tr>td:first-child{border-left:0}.panel>.table-bordered>thead>tr>th:last-child,.panel>.table-responsive>.table-bordered>thead>tr>th:last-child,.panel>.table-bordered>tbody>tr>th:last-child,.panel>.table-responsive>.table-bordered>tbody>tr>th:last-child,.panel>.table-bordered>tfoot>tr>th:last-child,.panel>.table-responsive>.table-bordered>tfoot>tr>th:last-child,.panel>.table-bordered>thead>tr>td:last-child,.panel>.table-responsive>.table-bordered>thead>tr>td:last-child,.panel>.table-bordered>tbody>tr>td:last-child,.panel>.table-responsive>.table-bordered>tbody>tr>td:last-child,.panel>.table-bordered>tfoot>tr>td:last-child,.panel>.table-responsive>.table-bordered>tfoot>tr>td:last-child{border-right:0}.panel>.table-bordered>thead>tr:first-child>td,.panel>.table-responsive>.table-bordered>thead>tr:first-child>td,.panel>.table-bordered>tbody>tr:first-child>td,.panel>.table-responsive>.table-bordered>tbody>tr:first-child>td,.panel>.table-bordered>thead>tr:first-child>th,.panel>.table-responsive>.table-bordered>thead>tr:first-child>th,.panel>.table-bordered>tbody>tr:first-child>th,.panel>.table-responsive>.table-bordered>tbody>tr:first-child>th{border-bottom:0}.panel>.table-bordered>tbody>tr:last-child>td,.panel>.table-responsive>.table-bordered>tbody>tr:last-child>td,.panel>.table-bordered>tfoot>tr:last-child>td,.panel>.table-responsive>.table-bordered>tfoot>tr:last-child>td,.panel>.table-bordered>tbody>tr:last-child>th,.panel>.table-responsive>.table-bordered>tbody>tr:last-child>th,.panel>.table-bordered>tfoot>tr:last-child>th,.panel>.table-responsive>.table-bordered>tfoot>tr:last-child>th{border-bottom:0}.panel>.table-responsive{margin-bottom:0;border:0}.panel-group{margin-bottom:20px}.panel-group .panel{margin-bottom:0;border-radius:4px}.panel-group .panel+.panel{margin-top:5px}.panel-group .panel-heading{border-bottom:0}.panel-group .panel-heading+.panel-collapse>.panel-body,.panel-group .panel-heading+.panel-collapse>.list-group{border-top:1px solid #ddd}.panel-group .panel-footer{border-top:0}.panel-group .panel-footer+.panel-collapse .panel-body{border-bottom:1px solid #ddd}.panel-default{border-color:#ddd}.panel-default>.panel-heading{color:#333;background-color:#f5f5f5;border-color:#ddd}.panel-default>.panel-heading+.panel-collapse>.panel-body{border-top-color:#ddd}.panel-default>.panel-heading .badge{color:#f5f5f5;background-color:#333}.panel-default>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#ddd}.panel-primary{border-color:#337ab7}.panel-primary>.panel-heading{color:#fff;background-color:#337ab7;border-color:#337ab7}.panel-primary>.panel-heading+.panel-collapse>.panel-body{border-top-color:#337ab7}.panel-primary>.panel-heading .badge{color:#337ab7;background-color:#fff}.panel-primary>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#337ab7}.panel-success{border-color:#d6e9c6}.panel-success>.panel-heading{color:#3c763d;background-color:#dff0d8;border-color:#d6e9c6}.panel-success>.panel-heading+.panel-collapse>.panel-body{border-top-color:#d6e9c6}.panel-success>.panel-heading .badge{color:#dff0d8;background-color:#3c763d}.panel-success>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#d6e9c6}.panel-info{border-color:#bce8f1}.panel-info>.panel-heading{color:#31708f;background-color:#d9edf7;border-color:#bce8f1}.panel-info>.panel-heading+.panel-collapse>.panel-body{border-top-color:#bce8f1}.panel-info>.panel-heading .badge{color:#d9edf7;background-color:#31708f}.panel-info>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#bce8f1}.panel-warning{border-color:#faebcc}.panel-warning>.panel-heading{color:#8a6d3b;background-color:#fcf8e3;border-color:#faebcc}.panel-warning>.panel-heading+.panel-collapse>.panel-body{border-top-color:#faebcc}.panel-warning>.panel-heading .badge{color:#fcf8e3;background-color:#8a6d3b}.panel-warning>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#faebcc}.panel-danger{border-color:#ebccd1}.panel-danger>.panel-heading{color:#a94442;background-color:#f2dede;border-color:#ebccd1}.panel-danger>.panel-heading+.panel-collapse>.panel-body{border-top-color:#ebccd1}.panel-danger>.panel-heading .badge{color:#f2dede;background-color:#a94442}.panel-danger>.panel-footer+.panel-collapse>.panel-body{border-bottom-color:#ebccd1}.embed-responsive{position:relative;display:block;height:0;padding:0;overflow:hidden}.embed-responsive .embed-responsive-item,.embed-responsive iframe,.embed-responsive embed,.embed-responsive object,.embed-responsive video{position:absolute;top:0;bottom:0;left:0;width:100%;height:100%;border:0}.embed-responsive.embed-responsive-16by9{padding-bottom:56.25%}.embed-responsive.embed-responsive-4by3{padding-bottom:75%}.well{min-height:20px;padding:19px;margin-bottom:20px;background-color:#f5f5f5;border:1px solid #e3e3e3;border-radius:4px;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,.05);box-shadow:inset 0 1px 1px rgba(0,0,0,.05)}.well blockquote{border-color:#ddd;border-color:rgba(0,0,0,.15)}.well-lg{padding:24px;border-radius:6px}.well-sm{padding:9px;border-radius:3px}.close{float:right;font-size:21px;font-weight:700;line-height:1;color:#000;text-shadow:0 1px 0 #fff;filter:alpha(opacity=20);opacity:.2}.close:hover,.close:focus{color:#000;text-decoration:none;cursor:pointer;filter:alpha(opacity=50);opacity:.5}button.close{-webkit-appearance:none;padding:0;cursor:pointer;background:0 0;border:0}.modal-open{overflow:hidden}.modal{position:fixed;top:0;right:0;bottom:0;left:0;z-index:1040;display:none;overflow:hidden;-webkit-overflow-scrolling:touch;outline:0}.modal.fade .modal-dialog{-webkit-transition:-webkit-transform .3s ease-out;-o-transition:-o-transform .3s ease-out;transition:transform .3s ease-out;-webkit-transform:translate(0,-25%);-ms-transform:translate(0,-25%);-o-transform:translate(0,-25%);transform:translate(0,-25%)}.modal.in .modal-dialog{-webkit-transform:translate(0,0);-ms-transform:translate(0,0);-o-transform:translate(0,0);transform:translate(0,0)}.modal-open .modal{overflow-x:hidden;overflow-y:auto}.modal-dialog{position:relative;width:auto;margin:10px}.modal-content{position:relative;background-color:#fff;-webkit-background-clip:padding-box;background-clip:padding-box;border:1px solid #999;border:1px solid rgba(0,0,0,.2);border-radius:6px;outline:0;-webkit-box-shadow:0 3px 9px rgba(0,0,0,.5);box-shadow:0 3px 9px rgba(0,0,0,.5)}.modal-backdrop{position:absolute;top:0;right:0;left:0;background-color:#000}.modal-backdrop.fade{filter:alpha(opacity=0);opacity:0}.modal-backdrop.in{filter:alpha(opacity=50);opacity:.5}.modal-header{min-height:16.43px;padding:15px;border-bottom:1px solid #e5e5e5}.modal-header .close{margin-top:-2px}.modal-title{margin:0;line-height:1.42857143}.modal-body{position:relative;padding:15px}.modal-footer{padding:15px;text-align:right;border-top:1px solid #e5e5e5}.modal-footer .btn+.btn{margin-bottom:0;margin-left:5px}.modal-footer .btn-group .btn+.btn{margin-left:-1px}.modal-footer .btn-block+.btn-block{margin-left:0}.modal-scrollbar-measure{position:absolute;top:-9999px;width:50px;height:50px;overflow:scroll}@media (min-width:768px){.modal-dialog{width:600px;margin:30px auto}.modal-content{-webkit-box-shadow:0 5px 15px rgba(0,0,0,.5);box-shadow:0 5px 15px rgba(0,0,0,.5)}.modal-sm{width:300px}}@media (min-width:992px){.modal-lg{width:900px}}.tooltip{position:absolute;z-index:1070;display:block;font-family:""Helvetica Neue"",Helvetica,Arial,sans-serif;font-size:12px;font-weight:400;line-height:1.4;visibility:visible;filter:alpha(opacity=0);opacity:0}.tooltip.in{filter:alpha(opacity=90);opacity:.9}.tooltip.top{padding:5px 0;margin-top:-3px}.tooltip.right{padding:0 5px;margin-left:3px}.tooltip.bottom{padding:5px 0;margin-top:3px}.tooltip.left{padding:0 5px;margin-left:-3px}.tooltip-inner{max-width:200px;padding:3px 8px;color:#fff;text-align:center;text-decoration:none;background-color:#000;border-radius:4px}.tooltip-arrow{position:absolute;width:0;height:0;border-color:transparent;border-style:solid}.tooltip.top .tooltip-arrow{bottom:0;left:50%;margin-left:-5px;border-width:5px 5px 0;border-top-color:#000}.tooltip.top-left .tooltip-arrow{right:5px;bottom:0;margin-bottom:-5px;border-width:5px 5px 0;border-top-color:#000}.tooltip.top-right .tooltip-arrow{bottom:0;left:5px;margin-bottom:-5px;border-width:5px 5px 0;border-top-color:#000}.tooltip.right .tooltip-arrow{top:50%;left:0;margin-top:-5px;border-width:5px 5px 5px 0;border-right-color:#000}.tooltip.left .tooltip-arrow{top:50%;right:0;margin-top:-5px;border-width:5px 0 5px 5px;border-left-color:#000}.tooltip.bottom .tooltip-arrow{top:0;left:50%;margin-left:-5px;border-width:0 5px 5px;border-bottom-color:#000}.tooltip.bottom-left .tooltip-arrow{top:0;right:5px;margin-top:-5px;border-width:0 5px 5px;border-bottom-color:#000}.tooltip.bottom-right .tooltip-arrow{top:0;left:5px;margin-top:-5px;border-width:0 5px 5px;border-bottom-color:#000}.popover{position:absolute;top:0;left:0;z-index:1060;display:none;max-width:276px;padding:1px;font-family:""Helvetica Neue"",Helvetica,Arial,sans-serif;font-size:14px;font-weight:400;line-height:1.42857143;text-align:left;white-space:normal;background-color:#fff;-webkit-background-clip:padding-box;background-clip:padding-box;border:1px solid #ccc;border:1px solid rgba(0,0,0,.2);border-radius:6px;-webkit-box-shadow:0 5px 10px rgba(0,0,0,.2);box-shadow:0 5px 10px rgba(0,0,0,.2)}.popover.top{margin-top:-10px}.popover.right{margin-left:10px}.popover.bottom{margin-top:10px}.popover.left{margin-left:-10px}.popover-title{padding:8px 14px;margin:0;font-size:14px;background-color:#f7f7f7;border-bottom:1px solid #ebebeb;border-radius:5px 5px 0 0}.popover-content{padding:9px 14px}.popover>.arrow,.popover>.arrow:after{position:absolute;display:block;width:0;height:0;border-color:transparent;border-style:solid}.popover>.arrow{border-width:11px}.popover>.arrow:after{content:"""";border-width:10px}.popover.top>.arrow{bottom:-11px;left:50%;margin-left:-11px;border-top-color:#999;border-top-color:rgba(0,0,0,.25);border-bottom-width:0}.popover.top>.arrow:after{bottom:1px;margin-left:-10px;content:"" "";border-top-color:#fff;border-bottom-width:0}.popover.right>.arrow{top:50%;left:-11px;margin-top:-11px;border-right-color:#999;border-right-color:rgba(0,0,0,.25);border-left-width:0}.popover.right>.arrow:after{bottom:-10px;left:1px;content:"" "";border-right-color:#fff;border-left-width:0}.popover.bottom>.arrow{top:-11px;left:50%;margin-left:-11px;border-top-width:0;border-bottom-color:#999;border-bottom-color:rgba(0,0,0,.25)}.popover.bottom>.arrow:after{top:1px;margin-left:-10px;content:"" "";border-top-width:0;border-bottom-color:#fff}.popover.left>.arrow{top:50%;right:-11px;margin-top:-11px;border-right-width:0;border-left-color:#999;border-left-color:rgba(0,0,0,.25)}.popover.left>.arrow:after{right:1px;bottom:-10px;content:"" "";border-right-width:0;border-left-color:#fff}.carousel{position:relative}.carousel-inner{position:relative;width:100%;overflow:hidden}.carousel-inner>.item{position:relative;display:none;-webkit-transition:.6s ease-in-out left;-o-transition:.6s ease-in-out left;transition:.6s ease-in-out left}.carousel-inner>.item>img,.carousel-inner>.item>a>img{line-height:1}@media all and (transform-3d),(-webkit-transform-3d){.carousel-inner>.item{-webkit-transition:-webkit-transform .6s ease-in-out;-o-transition:-o-transform .6s ease-in-out;transition:transform .6s ease-in-out;-webkit-backface-visibility:hidden;backface-visibility:hidden;-webkit-perspective:1000;perspective:1000}.carousel-inner>.item.next,.carousel-inner>.item.active.right{left:0;-webkit-transform:translate3d(100%,0,0);transform:translate3d(100%,0,0)}.carousel-inner>.item.prev,.carousel-inner>.item.active.left{left:0;-webkit-transform:translate3d(-100%,0,0);transform:translate3d(-100%,0,0)}.carousel-inner>.item.next.left,.carousel-inner>.item.prev.right,.carousel-inner>.item.active{left:0;-webkit-transform:translate3d(0,0,0);transform:translate3d(0,0,0)}}.carousel-inner>.active,.carousel-inner>.next,.carousel-inner>.prev{display:block}.carousel-inner>.active{left:0}.carousel-inner>.next,.carousel-inner>.prev{position:absolute;top:0;width:100%}.carousel-inner>.next{left:100%}.carousel-inner>.prev{left:-100%}.carousel-inner>.next.left,.carousel-inner>.prev.right{left:0}.carousel-inner>.active.left{left:-100%}.carousel-inner>.active.right{left:100%}.carousel-control{position:absolute;top:0;bottom:0;left:0;width:15%;font-size:20px;color:#fff;text-align:center;text-shadow:0 1px 2px rgba(0,0,0,.6);filter:alpha(opacity=50);opacity:.5}.carousel-control.left{background-image:-webkit-linear-gradient(left,rgba(0,0,0,.5) 0,rgba(0,0,0,.0001) 100%);background-image:-o-linear-gradient(left,rgba(0,0,0,.5) 0,rgba(0,0,0,.0001) 100%);background-image:-webkit-gradient(linear,left top,right top,from(rgba(0,0,0,.5)),to(rgba(0,0,0,.0001)));background-image:linear-gradient(to right,rgba(0,0,0,.5) 0,rgba(0,0,0,.0001) 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#80000000', endColorstr='#00000000', GradientType=1);background-repeat:repeat-x}.carousel-control.right{right:0;left:auto;background-image:-webkit-linear-gradient(left,rgba(0,0,0,.0001) 0,rgba(0,0,0,.5) 100%);background-image:-o-linear-gradient(left,rgba(0,0,0,.0001) 0,rgba(0,0,0,.5) 100%);background-image:-webkit-gradient(linear,left top,right top,from(rgba(0,0,0,.0001)),to(rgba(0,0,0,.5)));background-image:linear-gradient(to right,rgba(0,0,0,.0001) 0,rgba(0,0,0,.5) 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#00000000', endColorstr='#80000000', GradientType=1);background-repeat:repeat-x}.carousel-control:hover,.carousel-control:focus{color:#fff;text-decoration:none;filter:alpha(opacity=90);outline:0;opacity:.9}.carousel-control .icon-prev,.carousel-control .icon-next,.carousel-control .glyphicon-chevron-left,.carousel-control .glyphicon-chevron-right{position:absolute;top:50%;z-index:5;display:inline-block}.carousel-control .icon-prev,.carousel-control .glyphicon-chevron-left{left:50%;margin-left:-10px}.carousel-control .icon-next,.carousel-control .glyphicon-chevron-right{right:50%;margin-right:-10px}.carousel-control .icon-prev,.carousel-control .icon-next{width:20px;height:20px;margin-top:-10px;font-family:serif}.carousel-control .icon-prev:before{content:'\2039'}.carousel-control .icon-next:before{content:'\203a'}.carousel-indicators{position:absolute;bottom:10px;left:50%;z-index:15;width:60%;padding-left:0;margin-left:-30%;text-align:center;list-style:none}.carousel-indicators li{display:inline-block;width:10px;height:10px;margin:1px;text-indent:-999px;cursor:pointer;background-color:#000 \9;background-color:rgba(0,0,0,0);border:1px solid #fff;border-radius:10px}.carousel-indicators .active{width:12px;height:12px;margin:0;background-color:#fff}.carousel-caption{position:absolute;right:15%;bottom:20px;left:15%;z-index:10;padding-top:20px;padding-bottom:20px;color:#fff;text-align:center;text-shadow:0 1px 2px rgba(0,0,0,.6)}.carousel-caption .btn{text-shadow:none}@media screen and (min-width:768px){.carousel-control .glyphicon-chevron-left,.carousel-control .glyphicon-chevron-right,.carousel-control .icon-prev,.carousel-control .icon-next{width:30px;height:30px;margin-top:-15px;font-size:30px}.carousel-control .glyphicon-chevron-left,.carousel-control .icon-prev{margin-left:-15px}.carousel-control .glyphicon-chevron-right,.carousel-control .icon-next{margin-right:-15px}.carousel-caption{right:20%;left:20%;padding-bottom:30px}.carousel-indicators{bottom:20px}}.clearfix:before,.clearfix:after,.dl-horizontal dd:before,.dl-horizontal dd:after,.container:before,.container:after,.container-fluid:before,.container-fluid:after,.row:before,.row:after,.form-horizontal .form-group:before,.form-horizontal .form-group:after,.btn-toolbar:before,.btn-toolbar:after,.btn-group-vertical>.btn-group:before,.btn-group-vertical>.btn-group:after,.nav:before,.nav:after,.navbar:before,.navbar:after,.navbar-header:before,.navbar-header:after,.navbar-collapse:before,.navbar-collapse:after,.pager:before,.pager:after,.panel-body:before,.panel-body:after,.modal-footer:before,.modal-footer:after{display:table;content:"" ""}.clearfix:after,.dl-horizontal dd:after,.container:after,.container-fluid:after,.row:after,.form-horizontal .form-group:after,.btn-toolbar:after,.btn-group-vertical>.btn-group:after,.nav:after,.navbar:after,.navbar-header:after,.navbar-collapse:after,.pager:after,.panel-body:after,.modal-footer:after{clear:both}.center-block{display:block;margin-right:auto;margin-left:auto}.pull-right{float:right!important}.pull-left{float:left!important}.hide{display:none!important}.show{display:block!important}.invisible{visibility:hidden}.text-hide{font:0/0 a;color:transparent;text-shadow:none;background-color:transparent;border:0}.hidden{display:none!important;visibility:hidden!important}.affix{position:fixed}@-ms-viewport{width:device-width}.visible-xs,.visible-sm,.visible-md,.visible-lg{display:none!important}.visible-xs-block,.visible-xs-inline,.visible-xs-inline-block,.visible-sm-block,.visible-sm-inline,.visible-sm-inline-block,.visible-md-block,.visible-md-inline,.visible-md-inline-block,.visible-lg-block,.visible-lg-inline,.visible-lg-inline-block{display:none!important}@media (max-width:767px){.visible-xs{display:block!important}table.visible-xs{display:table}tr.visible-xs{display:table-row!important}th.visible-xs,td.visible-xs{display:table-cell!important}}@media (max-width:767px){.visible-xs-block{display:block!important}}@media (max-width:767px){.visible-xs-inline{display:inline!important}}@media (max-width:767px){.visible-xs-inline-block{display:inline-block!important}}@media (min-width:768px) and (max-width:991px){.visible-sm{display:block!important}table.visible-sm{display:table}tr.visible-sm{display:table-row!important}th.visible-sm,td.visible-sm{display:table-cell!important}}@media (min-width:768px) and (max-width:991px){.visible-sm-block{display:block!important}}@media (min-width:768px) and (max-width:991px){.visible-sm-inline{display:inline!important}}@media (min-width:768px) and (max-width:991px){.visible-sm-inline-block{display:inline-block!important}}@media (min-width:992px) and (max-width:1199px){.visible-md{display:block!important}table.visible-md{display:table}tr.visible-md{display:table-row!important}th.visible-md,td.visible-md{display:table-cell!important}}@media (min-width:992px) and (max-width:1199px){.visible-md-block{display:block!important}}@media (min-width:992px) and (max-width:1199px){.visible-md-inline{display:inline!important}}@media (min-width:992px) and (max-width:1199px){.visible-md-inline-block{display:inline-block!important}}@media (min-width:1200px){.visible-lg{display:block!important}table.visible-lg{display:table}tr.visible-lg{display:table-row!important}th.visible-lg,td.visible-lg{display:table-cell!important}}@media (min-width:1200px){.visible-lg-block{display:block!important}}@media (min-width:1200px){.visible-lg-inline{display:inline!important}}@media (min-width:1200px){.visible-lg-inline-block{display:inline-block!important}}@media (max-width:767px){.hidden-xs{display:none!important}}@media (min-width:768px) and (max-width:991px){.hidden-sm{display:none!important}}@media (min-width:992px) and (max-width:1199px){.hidden-md{display:none!important}}@media (min-width:1200px){.hidden-lg{display:none!important}}.visible-print{display:none!important}@media print{.visible-print{display:block!important}table.visible-print{display:table}tr.visible-print{display:table-row!important}th.visible-print,td.visible-print{display:table-cell!important}}.visible-print-block{display:none!important}@media print{.visible-print-block{display:block!important}}.visible-print-inline{display:none!important}@media print{.visible-print-inline{display:inline!important}}.visible-print-inline-block{display:none!important}@media print{.visible-print-inline-block{display:inline-block!important}}@media print{.hidden-print{display:none!important}}";

            }
            public class jquery_1_12_4_min_js
            {
                public static readonly string TextContent = ((Func<string>)(() =>
                {
                    byte[] Result = null;
                    {
                        var ZipMemmory = new System.IO.MemoryStream(new byte[] { 220, 189, 105, 151, 227, 198, 145, 054, 250, 125, 126, 069, 017, 238, 161, 128, 102, 022, 171, 216, 090, 222, 049, 216, 040, 158, 086, 183, 100, 073, 214, 102, 181, 172, 197, 044, 202, 007, 027, 073, 084, 113, 043, 146, 213, 139, 138, 244, 111, 127, 227, 137, 200, 076, 036, 022, 118, 203, 051, 115, 239, 185, 231, 202, 238, 034, 128, 220, 051, 035, 035, 035, 034, 099, 185, 120, 220, 057, 187, 249, 219, 125, 190, 125, 123, 246, 106, 208, 031, 060, 233, 127, 116, 118, 056, 243, 211, 192, 124, 252, 124, 125, 191, 202, 226, 125, 177, 094, 209, 247, 155, 059, 124, 235, 175, 183, 179, 139, 069, 145, 230, 171, 093, 126, 246, 248, 226, 063, 058, 211, 251, 085, 138, 028, 126, 172, 146, 224, 193, 091, 039, 055, 121, 186, 247, 162, 104, 255, 118, 147, 175, 167, 103, 203, 117, 118, 191, 200, 187, 221, 019, 009, 253, 252, 205, 102, 189, 221, 239, 070, 213, 215, 040, 238, 103, 235, 244, 126, 153, 175, 246, 163, 132, 106, 238, 092, 006, 097, 217, 080, 240, 080, 076, 253, 078, 153, 037, 216, 207, 183, 235, 215, 103, 171, 252, 245, 217, 103, 219, 237, 122, 235, 123, 186, 255, 219, 252, 238, 190, 216, 230, 187, 179, 248, 236, 117, 177, 202, 040, 207, 235, 098, 063, 167, 055, 083, 210, 011, 134, 219, 124, 127, 191, 093, 157, 081, 043, 193, 049, 228, 191, 190, 071, 163, 206, 167, 197, 042, 207, 188, 142, 233, 174, 148, 031, 201, 079, 184, 159, 023, 059, 085, 029, 249, 171, 120, 123, 150, 070, 227, 137, 202, 156, 206, 171, 060, 074, 251, 059, 076, 151, 154, 210, 083, 186, 094, 165, 241, 094, 205, 232, 113, 115, 191, 155, 171, 057, 061, 080, 133, 249, 155, 239, 166, 170, 136, 030, 142, 234, 038, 042, 250, 251, 245, 203, 253, 182, 088, 205, 212, 045, 189, 204, 227, 221, 119, 175, 087, 223, 111, 215, 155, 124, 187, 127, 171, 022, 200, 180, 140, 060, 089, 044, 079, 173, 162, 106, 039, 244, 096, 048, 019, 171, 254, 116, 069, 149, 023, 123, 078, 057, 170, 117, 116, 241, 219, 248, 122, 119, 125, 255, 249, 103, 159, 127, 126, 253, 230, 217, 229, 164, 119, 168, 189, 063, 186, 152, 169, 013, 101, 059, 095, 238, 206, 047, 212, 093, 116, 113, 238, 143, 175, 179, 248, 252, 247, 073, 112, 049, 043, 212, 182, 189, 177, 132, 122, 252, 247, 013, 245, 239, 121, 188, 203, 253, 224, 056, 068, 203, 209, 170, 191, 217, 174, 247, 107, 204, 094, 244, 032, 160, 019, 046, 021, 077, 192, 110, 191, 189, 079, 247, 235, 109, 184, 082, 187, 124, 145, 243, 163, 231, 169, 069, 190, 154, 237, 231, 225, 165, 218, 175, 159, 109, 183, 241, 219, 114, 185, 109, 067, 121, 063, 141, 023, 011, 031, 115, 079, 227, 153, 229, 251, 010, 072, 152, 161, 223, 047, 022, 157, 040, 030, 093, 094, 197, 035, 228, 028, 199, 061, 252, 244, 165, 254, 073, 040, 223, 038, 097, 181, 050, 172, 198, 203, 125, 156, 222, 086, 170, 196, 146, 038, 052, 146, 101, 190, 157, 229, 156, 181, 239, 012, 192, 015, 084, 092, 130, 015, 013, 055, 127, 245, 029, 195, 120, 196, 208, 145, 032, 239, 062, 127, 035, 175, 230, 069, 037, 071, 149, 199, 233, 188, 181, 235, 125, 164, 112, 059, 084, 051, 173, 115, 188, 105, 203, 198, 213, 217, 014, 251, 212, 189, 120, 227, 087, 001, 050, 081, 169, 205, 030, 203, 064, 233, 019, 192, 032, 160, 122, 025, 030, 091, 230, 183, 086, 113, 222, 143, 055, 155, 197, 091, 221, 159, 237, 140, 001, 122, 135, 010, 166, 197, 118, 183, 063, 085, 065, 126, 231, 095, 082, 158, 069, 252, 206, 044, 231, 003, 202, 147, 223, 181, 076, 183, 179, 090, 042, 141, 122, 113, 207, 199, 082, 038, 225, 165, 157, 235, 090, 063, 211, 171, 232, 178, 219, 077, 174, 210, 209, 152, 023, 055, 157, 076, 194, 241, 004, 213, 175, 178, 147, 163, 180, 139, 117, 056, 052, 215, 085, 224, 033, 156, 169, 029, 225, 162, 144, 054, 048, 253, 168, 221, 134, 167, 141, 222, 248, 225, 168, 104, 185, 222, 236, 169, 141, 136, 119, 154, 126, 118, 218, 195, 112, 104, 159, 208, 188, 103, 138, 182, 063, 237, 122, 059, 137, 227, 203, 201, 225, 064, 059, 121, 030, 013, 104, 223, 219, 207, 102, 216, 055, 081, 103, 048, 156, 002, 143, 037, 235, 245, 034, 143, 087, 037, 214, 156, 117, 187, 254, 077, 052, 171, 084, 054, 215, 149, 245, 122, 129, 106, 160, 217, 217, 225, 064, 104, 096, 247, 185, 233, 215, 044, 056, 028, 252, 025, 161, 145, 128, 090, 143, 162, 130, 234, 155, 009, 192, 206, 207, 207, 131, 097, 113, 053, 031, 162, 034, 066, 176, 178, 147, 252, 188, 210, 082, 016, 160, 095, 217, 089, 065, 219, 049, 136, 163, 217, 056, 155, 208, 042, 229, 248, 153, 117, 162, 040, 069, 247, 186, 093, 252, 160, 213, 239, 023, 113, 177, 146, 121, 166, 083, 133, 026, 198, 110, 042, 118, 188, 193, 233, 067, 016, 140, 252, 132, 254, 079, 195, 037, 244, 024, 119, 187, 101, 098, 028, 140, 098, 172, 098, 104, 191, 187, 117, 113, 042, 013, 025, 205, 071, 102, 238, 253, 027, 154, 100, 170, 052, 124, 181, 046, 178, 179, 075, 221, 027, 206, 066, 095, 013, 240, 204, 202, 133, 243, 031, 232, 180, 137, 009, 159, 135, 250, 188, 240, 122, 254, 178, 247, 077, 188, 159, 247, 183, 248, 188, 244, 131, 160, 191, 205, 055, 139, 056, 205, 253, 139, 235, 023, 132, 029, 061, 047, 080, 197, 238, 135, 060, 206, 222, 134, 157, 075, 149, 227, 180, 169, 192, 112, 253, 036, 194, 062, 094, 173, 215, 027, 023, 016, 143, 170, 092, 143, 150, 013, 238, 153, 079, 180, 136, 052, 056, 172, 035, 087, 163, 167, 038, 228, 191, 102, 162, 014, 135, 150, 010, 098, 164, 052, 074, 255, 044, 071, 215, 105, 156, 217, 237, 198, 017, 157, 093, 114, 196, 161, 196, 183, 180, 236, 219, 034, 109, 217, 164, 200, 107, 015, 042, 223, 076, 110, 199, 093, 063, 218, 146, 231, 155, 120, 187, 203, 063, 095, 172, 227, 189, 159, 004, 189, 001, 109, 084, 084, 251, 217, 114, 179, 127, 043, 043, 217, 172, 154, 225, 062, 001, 124, 197, 129, 174, 117, 096, 170, 231, 210, 014, 028, 180, 148, 102, 194, 224, 112, 048, 219, 160, 227, 204, 193, 225, 016, 247, 087, 235, 044, 255, 145, 094, 101, 083, 200, 140, 080, 082, 217, 210, 126, 251, 022, 196, 069, 236, 034, 132, 110, 183, 115, 043, 072, 052, 086, 158, 243, 221, 011, 156, 020, 183, 064, 121, 244, 041, 143, 058, 108, 094, 190, 155, 122, 101, 075, 071, 034, 004, 008, 217, 091, 060, 077, 095, 208, 247, 069, 127, 253, 122, 245, 057, 240, 107, 208, 152, 136, 051, 219, 139, 036, 112, 167, 201, 128, 182, 192, 061, 045, 123, 114, 056, 056, 089, 143, 010, 141, 159, 090, 119, 090, 241, 081, 220, 243, 188, 176, 129, 057, 048, 141, 014, 040, 154, 175, 163, 098, 124, 163, 043, 015, 038, 229, 076, 135, 038, 157, 118, 229, 098, 157, 196, 139, 207, 094, 197, 139, 178, 081, 034, 023, 018, 236, 099, 002, 152, 037, 189, 208, 198, 140, 105, 023, 230, 233, 203, 116, 091, 108, 246, 014, 020, 083, 070, 074, 161, 178, 206, 000, 002, 031, 163, 072, 227, 101, 190, 000, 141, 209, 054, 148, 216, 110, 212, 141, 242, 136, 124, 241, 202, 157, 123, 167, 182, 188, 009, 179, 252, 091, 170, 033, 108, 165, 097, 004, 050, 144, 014, 192, 054, 207, 004, 225, 095, 175, 095, 027, 194, 006, 019, 091, 253, 210, 056, 202, 045, 029, 072, 068, 224, 037, 096, 113, 007, 224, 122, 192, 090, 165, 180, 179, 004, 189, 015, 211, 171, 108, 152, 009, 142, 077, 244, 040, 129, 063, 051, 133, 031, 052, 211, 025, 004, 201, 054, 143, 111, 143, 249, 130, 136, 108, 139, 112, 227, 247, 151, 048, 160, 064, 171, 128, 153, 126, 215, 162, 211, 146, 251, 088, 248, 114, 162, 214, 064, 111, 032, 059, 110, 243, 026, 009, 230, 016, 184, 004, 091, 227, 201, 176, 142, 056, 104, 160, 022, 053, 007, 035, 067, 049, 165, 202, 219, 049, 134, 112, 193, 007, 196, 087, 028, 132, 051, 025, 071, 074, 148, 078, 160, 082, 218, 216, 171, 102, 155, 160, 098, 208, 106, 134, 169, 076, 152, 234, 159, 155, 189, 048, 055, 100, 077, 076, 185, 120, 055, 100, 180, 060, 150, 110, 072, 137, 000, 076, 071, 140, 203, 151, 241, 027, 255, 082, 101, 189, 052, 008, 211, 240, 114, 152, 093, 165, 195, 084, 102, 063, 197, 172, 018, 088, 038, 068, 051, 208, 036, 218, 125, 150, 030, 229, 225, 124, 064, 179, 129, 145, 180, 206, 068, 207, 054, 071, 139, 077, 132, 190, 093, 223, 215, 243, 098, 065, 131, 191, 202, 130, 120, 156, 247, 122, 147, 040, 025, 211, 114, 079, 048, 136, 020, 167, 082, 032, 025, 236, 049, 149, 096, 017, 171, 089, 045, 088, 074, 149, 081, 174, 176, 175, 104, 161, 026, 243, 131, 145, 243, 028, 081, 015, 136, 249, 152, 082, 087, 102, 182, 043, 116, 196, 119, 210, 225, 236, 106, 058, 156, 210, 136, 179, 168, 067, 252, 205, 120, 074, 185, 002, 149, 081, 195, 243, 110, 055, 103, 066, 138, 191, 090, 060, 146, 215, 073, 079, 103, 037, 136, 146, 153, 081, 019, 115, 106, 203, 194, 183, 076, 190, 029, 126, 118, 053, 027, 206, 168, 185, 060, 066, 107, 051, 034, 012, 168, 180, 018, 080, 161, 237, 053, 151, 022, 243, 096, 104, 161, 123, 038, 208, 253, 222, 002, 186, 127, 083, 077, 152, 210, 112, 231, 096, 004, 238, 139, 044, 028, 040, 194, 185, 111, 090, 065, 150, 182, 201, 084, 023, 109, 128, 035, 045, 190, 079, 020, 198, 056, 153, 040, 058, 212, 084, 028, 209, 204, 084, 200, 037, 162, 052, 104, 235, 106, 102, 193, 018, 066, 234, 009, 077, 096, 212, 164, 045, 099, 221, 179, 068, 168, 074, 101, 056, 062, 191, 094, 065, 000, 050, 058, 235, 163, 235, 052, 113, 206, 015, 014, 038, 252, 246, 122, 042, 051, 148, 012, 144, 215, 235, 038, 037, 219, 003, 149, 241, 034, 222, 211, 114, 237, 238, 055, 096, 155, 195, 005, 081, 117, 045, 056, 251, 229, 219, 101, 178, 094, 048, 077, 054, 093, 141, 229, 173, 095, 236, 243, 109, 076, 135, 021, 081, 071, 141, 079, 152, 004, 230, 069, 188, 079, 133, 000, 061, 035, 082, 032, 201, 183, 103, 114, 224, 159, 153, 233, 057, 227, 061, 203, 157, 056, 251, 033, 159, 125, 246, 102, 115, 038, 104, 064, 168, 031, 221, 176, 199, 036, 243, 222, 247, 206, 136, 122, 170, 046, 080, 049, 246, 198, 114, 128, 156, 121, 189, 164, 231, 077, 188, 073, 003, 201, 210, 238, 054, 205, 237, 074, 010, 164, 211, 033, 180, 227, 009, 200, 121, 000, 031, 032, 110, 187, 253, 237, 161, 063, 108, 161, 169, 210, 218, 233, 063, 234, 012, 194, 146, 102, 162, 084, 125, 134, 122, 043, 030, 116, 005, 090, 146, 043, 240, 026, 231, 003, 134, 216, 035, 186, 178, 143, 026, 084, 072, 073, 242, 171, 185, 042, 212, 141, 186, 085, 011, 181, 084, 043, 181, 086, 027, 069, 199, 145, 218, 169, 189, 186, 143, 188, 093, 241, 251, 239, 139, 220, 235, 013, 030, 155, 165, 084, 175, 092, 225, 193, 107, 218, 105, 111, 232, 223, 219, 104, 022, 019, 183, 249, 187, 252, 060, 147, 159, 079, 219, 185, 241, 024, 125, 167, 165, 094, 068, 157, 203, 064, 017, 236, 060, 143, 006, 079, 159, 126, 056, 080, 047, 136, 230, 175, 139, 019, 062, 003, 202, 248, 060, 250, 172, 191, 089, 111, 212, 095, 240, 011, 169, 196, 023, 230, 225, 075, 122, 016, 225, 197, 087, 181, 198, 012, 214, 073, 169, 119, 149, 173, 111, 113, 107, 044, 056, 053, 177, 056, 117, 088, 226, 212, 191, 070, 094, 058, 207, 211, 219, 060, 059, 136, 008, 128, 030, 226, 221, 219, 085, 122, 136, 239, 247, 235, 041, 013, 127, 199, 079, 116, 044, 189, 061, 128, 113, 222, 174, 023, 187, 067, 150, 079, 243, 237, 033, 043, 118, 113, 178, 160, 002, 243, 034, 203, 242, 213, 161, 216, 017, 174, 058, 044, 136, 196, 062, 044, 239, 023, 251, 098, 179, 200, 015, 052, 186, 213, 129, 142, 195, 108, 189, 090, 188, 061, 104, 033, 016, 181, 149, 082, 066, 230, 169, 175, 035, 111, 124, 125, 253, 230, 201, 229, 245, 245, 254, 250, 122, 123, 125, 189, 186, 190, 158, 078, 060, 245, 077, 228, 249, 163, 240, 154, 254, 235, 031, 040, 195, 235, 243, 201, 097, 252, 027, 101, 188, 188, 060, 167, 191, 241, 229, 036, 232, 121, 234, 219, 200, 187, 190, 030, 123, 189, 175, 123, 222, 099, 223, 235, 125, 211, 243, 002, 042, 164, 223, 199, 143, 127, 123, 116, 232, 252, 107, 050, 138, 002, 253, 101, 020, 126, 224, 151, 149, 254, 134, 223, 015, 038, 193, 227, 224, 131, 195, 181, 087, 079, 184, 246, 144, 114, 237, 029, 116, 189, 193, 065, 215, 114, 125, 077, 189, 251, 046, 162, 003, 219, 054, 120, 125, 237, 251, 254, 191, 095, 117, 112, 168, 167, 248, 001, 013, 117, 050, 057, 120, 189, 111, 169, 230, 199, 193, 161, 079, 249, 174, 209, 180, 250, 062, 002, 088, 202, 190, 246, 169, 031, 052, 122, 111, 070, 091, 248, 111, 238, 119, 239, 055, 238, 099, 143, 043, 254, 077, 087, 058, 009, 076, 043, 084, 163, 164, 063, 210, 133, 127, 104, 041, 252, 088, 201, 015, 037, 191, 108, 075, 246, 199, 087, 189, 127, 161, 139, 244, 018, 216, 172, 063, 086, 178, 070, 038, 043, 117, 096, 242, 001, 141, 247, 241, 200, 157, 061, 110, 251, 239, 110, 137, 239, 002, 245, 083, 189, 049, 154, 220, 071, 148, 239, 231, 232, 225, 203, 023, 097, 037, 237, 079, 122, 234, 041, 245, 249, 215, 207, 094, 190, 172, 166, 210, 064, 203, 244, 031, 159, 253, 165, 154, 042, 073, 135, 241, 227, 009, 146, 159, 253, 248, 227, 015, 097, 173, 221, 111, 003, 245, 253, 203, 207, 254, 254, 226, 187, 122, 002, 117, 242, 249, 023, 095, 126, 093, 235, 076, 232, 051, 096, 179, 024, 230, 000, 065, 203, 097, 181, 159, 227, 223, 057, 094, 130, 115, 063, 037, 002, 035, 059, 172, 167, 231, 064, 092, 026, 092, 244, 252, 228, 175, 104, 111, 172, 179, 140, 214, 107, 220, 035, 008, 015, 252, 235, 235, 236, 113, 176, 058, 148, 016, 171, 019, 244, 059, 037, 247, 008, 028, 236, 100, 050, 104, 120, 005, 141, 004, 194, 137, 218, 072, 177, 019, 254, 074, 211, 240, 072, 103, 089, 229, 121, 182, 123, 046, 162, 175, 250, 216, 080, 157, 044, 108, 088, 246, 042, 191, 059, 204, 104, 076, 050, 162, 114, 128, 213, 049, 208, 011, 237, 200, 044, 024, 113, 215, 157, 142, 249, 163, 104, 252, 027, 245, 253, 145, 238, 226, 081, 253, 018, 093, 160, 087, 197, 106, 115, 191, 215, 200, 230, 128, 206, 196, 132, 030, 014, 201, 253, 126, 191, 094, 005, 143, 046, 010, 245, 043, 229, 155, 095, 103, 120, 252, 007, 004, 165, 191, 061, 076, 122, 215, 015, 215, 187, 199, 215, 227, 085, 188, 047, 094, 229, 103, 215, 175, 047, 212, 035, 169, 237, 079, 254, 024, 216, 129, 166, 197, 191, 126, 077, 127, 105, 245, 245, 007, 170, 075, 253, 051, 186, 024, 211, 168, 046, 084, 028, 071, 023, 180, 025, 175, 047, 102, 042, 137, 043, 160, 198, 027, 143, 246, 093, 022, 159, 079, 039, 015, 003, 245, 201, 145, 007, 049, 058, 200, 008, 105, 019, 242, 000, 000, 179, 105, 028, 181, 018, 098, 145, 119, 249, 134, 142, 204, 243, 079, 062, 254, 248, 195, 079, 012, 101, 004, 162, 142, 072, 136, 020, 226, 178, 171, 108, 036, 167, 117, 127, 186, 093, 047, 159, 207, 227, 237, 115, 098, 107, 252, 172, 199, 037, 130, 176, 053, 241, 234, 106, 112, 121, 248, 248, 227, 039, 127, 254, 068, 013, 046, 159, 124, 216, 205, 014, 031, 127, 242, 225, 019, 200, 243, 178, 216, 165, 119, 150, 144, 243, 130, 087, 254, 066, 083, 060, 159, 069, 095, 010, 137, 243, 170, 207, 192, 247, 045, 085, 183, 011, 084, 245, 237, 179, 177, 251, 110, 068, 178, 150, 041, 215, 044, 113, 078, 199, 232, 023, 209, 003, 215, 027, 126, 166, 115, 141, 170, 231, 207, 095, 116, 179, 177, 210, 205, 038, 068, 080, 181, 082, 233, 177, 067, 164, 107, 202, 156, 142, 166, 146, 212, 014, 134, 150, 200, 078, 233, 108, 058, 030, 045, 177, 049, 141, 121, 194, 233, 036, 151, 186, 166, 116, 152, 203, 081, 190, 230, 035, 252, 053, 206, 217, 004, 252, 122, 190, 125, 097, 142, 236, 055, 081, 050, 074, 236, 136, 194, 063, 131, 072, 206, 176, 038, 116, 208, 026, 234, 179, 227, 112, 216, 016, 086, 012, 104, 213, 222, 116, 187, 127, 150, 159, 001, 191, 154, 099, 147, 089, 159, 014, 081, 193, 190, 143, 138, 043, 141, 029, 014, 073, 248, 042, 128, 136, 163, 219, 005, 083, 077, 036, 044, 081, 045, 043, 181, 009, 152, 085, 026, 232, 122, 253, 117, 244, 136, 121, 109, 208, 234, 056, 154, 167, 209, 122, 060, 152, 112, 158, 063, 071, 104, 139, 175, 083, 252, 027, 162, 188, 102, 249, 254, 179, 069, 142, 202, 063, 125, 251, 101, 230, 079, 131, 160, 210, 145, 155, 062, 017, 170, 017, 017, 201, 230, 163, 144, 230, 055, 068, 013, 011, 175, 074, 121, 094, 179, 132, 242, 117, 075, 085, 221, 238, 158, 248, 181, 027, 250, 125, 095, 061, 232, 208, 122, 252, 100, 098, 210, 013, 148, 101, 202, 237, 226, 238, 211, 183, 063, 198, 051, 048, 234, 024, 153, 226, 030, 242, 224, 062, 156, 080, 027, 105, 053, 231, 115, 066, 035, 059, 097, 240, 147, 019, 041, 239, 109, 205, 230, 196, 104, 168, 171, 096, 232, 250, 119, 059, 034, 061, 059, 207, 198, 196, 075, 159, 121, 019, 026, 124, 231, 142, 150, 245, 174, 191, 207, 119, 204, 013, 203, 090, 240, 154, 018, 196, 168, 093, 020, 015, 205, 084, 057, 082, 170, 228, 132, 208, 033, 120, 240, 111, 101, 093, 158, 237, 009, 122, 008, 099, 229, 190, 087, 100, 068, 028, 140, 110, 163, 091, 203, 186, 199, 177, 034, 164, 242, 168, 235, 005, 097, 210, 223, 213, 051, 171, 219, 232, 062, 080, 219, 104, 070, 253, 033, 214, 109, 107, 182, 196, 034, 250, 073, 186, 121, 027, 140, 188, 063, 121, 189, 219, 208, 027, 211, 194, 124, 064, 079, 061, 239, 131, 137, 167, 247, 011, 004, 195, 219, 241, 124, 018, 045, 048, 196, 222, 093, 236, 227, 045, 024, 238, 168, 166, 155, 117, 177, 242, 009, 087, 005, 180, 029, 254, 105, 198, 220, 237, 174, 099, 063, 233, 111, 008, 201, 174, 246, 216, 235, 001, 129, 042, 102, 107, 023, 000, 103, 052, 166, 249, 117, 159, 239, 135, 094, 234, 235, 160, 103, 180, 159, 119, 060, 193, 130, 016, 222, 006, 015, 199, 105, 177, 162, 109, 254, 246, 225, 150, 192, 230, 030, 075, 184, 205, 151, 235, 087, 121, 109, 082, 104, 251, 234, 202, 011, 191, 148, 008, 253, 077, 121, 143, 006, 056, 175, 120, 047, 151, 027, 028, 100, 180, 072, 233, 193, 205, 218, 207, 137, 159, 098, 203, 091, 150, 142, 065, 051, 197, 208, 131, 171, 140, 176, 013, 145, 174, 095, 243, 252, 117, 187, 025, 245, 152, 088, 159, 100, 028, 247, 119, 243, 098, 186, 247, 003, 226, 035, 199, 156, 119, 018, 229, 166, 047, 073, 217, 228, 060, 118, 101, 086, 227, 251, 009, 081, 233, 196, 218, 219, 244, 034, 118, 239, 157, 082, 058, 164, 246, 185, 134, 063, 223, 203, 138, 087, 094, 048, 044, 103, 144, 088, 032, 008, 198, 154, 130, 068, 051, 089, 238, 018, 096, 206, 202, 055, 061, 125, 207, 129, 140, 005, 115, 128, 221, 118, 144, 223, 077, 092, 069, 162, 154, 131, 059, 208, 052, 226, 074, 179, 034, 233, 200, 009, 062, 178, 126, 076, 075, 241, 069, 188, 202, 022, 249, 056, 029, 231, 019, 194, 175, 101, 109, 183, 149, 218, 008, 109, 198, 132, 142, 083, 194, 117, 144, 186, 088, 108, 041, 239, 137, 243, 238, 255, 139, 160, 121, 125, 191, 077, 243, 047, 113, 075, 122, 056, 060, 039, 226, 230, 095, 113, 253, 027, 035, 217, 010, 162, 050, 050, 150, 148, 250, 186, 162, 227, 254, 101, 145, 044, 008, 245, 178, 208, 199, 225, 074, 206, 007, 086, 210, 050, 026, 132, 132, 254, 109, 143, 023, 238, 066, 185, 194, 073, 061, 132, 019, 219, 213, 176, 155, 076, 111, 048, 055, 137, 121, 231, 075, 080, 026, 169, 051, 191, 203, 255, 081, 253, 190, 211, 000, 241, 169, 066, 199, 240, 091, 112, 162, 189, 149, 219, 030, 065, 161, 219, 164, 001, 211, 168, 151, 040, 055, 137, 184, 088, 233, 015, 046, 175, 099, 136, 091, 204, 194, 211, 090, 170, 089, 052, 173, 130, 193, 140, 192, 128, 086, 062, 154, 142, 103, 019, 224, 065, 064, 065, 212, 241, 051, 252, 224, 153, 078, 104, 252, 207, 118, 105, 093, 217, 011, 196, 204, 183, 220, 186, 199, 173, 232, 158, 224, 231, 152, 070, 083, 130, 003, 145, 123, 224, 082, 124, 138, 247, 098, 247, 203, 055, 095, 055, 249, 113, 150, 080, 198, 245, 195, 051, 014, 044, 167, 173, 091, 176, 151, 185, 035, 239, 139, 031, 191, 249, 186, 138, 151, 195, 014, 164, 129, 220, 106, 190, 055, 181, 180, 240, 254, 016, 144, 197, 163, 102, 107, 225, 043, 123, 005, 037, 071, 054, 206, 222, 153, 003, 236, 179, 122, 119, 070, 254, 042, 154, 169, 053, 033, 130, 090, 130, 218, 068, 157, 169, 191, 010, 148, 159, 035, 049, 159, 198, 196, 249, 254, 084, 228, 175, 003, 008, 244, 136, 111, 166, 006, 176, 123, 242, 126, 156, 101, 159, 017, 105, 189, 255, 186, 216, 237, 115, 234, 207, 168, 249, 009, 218, 014, 139, 117, 076, 167, 068, 022, 171, 206, 032, 008, 115, 108, 101, 194, 114, 156, 011, 021, 058, 175, 116, 100, 173, 202, 236, 144, 220, 242, 190, 103, 020, 188, 139, 010, 007, 120, 092, 209, 124, 106, 206, 205, 136, 136, 114, 213, 137, 107, 199, 153, 077, 038, 004, 142, 026, 219, 150, 252, 100, 221, 116, 134, 228, 171, 076, 016, 153, 193, 152, 207, 215, 075, 193, 152, 116, 076, 234, 230, 154, 036, 003, 120, 072, 013, 192, 205, 086, 237, 073, 031, 253, 067, 014, 181, 213, 041, 154, 065, 074, 130, 196, 057, 209, 197, 117, 165, 139, 004, 116, 116, 194, 222, 171, 078, 173, 066, 212, 069, 100, 067, 203, 087, 255, 190, 222, 077, 052, 054, 242, 179, 062, 237, 149, 172, 255, 229, 139, 154, 104, 006, 116, 069, 203, 078, 170, 211, 118, 221, 238, 166, 068, 054, 053, 090, 173, 188, 082, 074, 071, 227, 020, 183, 233, 071, 072, 043, 167, 197, 098, 159, 111, 171, 045, 218, 029, 102, 079, 219, 036, 038, 158, 165, 148, 213, 182, 173, 089, 147, 154, 001, 078, 062, 030, 131, 208, 215, 039, 170, 029, 220, 255, 066, 179, 050, 200, 147, 216, 197, 246, 068, 078, 201, 230, 055, 233, 161, 157, 017, 080, 150, 175, 226, 197, 125, 174, 251, 172, 116, 095, 137, 237, 143, 218, 097, 119, 212, 038, 168, 123, 223, 026, 057, 197, 079, 210, 188, 033, 083, 159, 148, 222, 032, 159, 098, 043, 059, 062, 113, 059, 069, 200, 060, 143, 046, 009, 107, 158, 172, 028, 199, 040, 237, 017, 190, 023, 121, 048, 167, 233, 148, 111, 041, 002, 156, 211, 169, 131, 186, 052, 253, 158, 218, 089, 202, 012, 237, 051, 061, 154, 249, 097, 177, 073, 125, 134, 028, 170, 252, 191, 057, 073, 078, 013, 155, 250, 076, 149, 244, 122, 057, 031, 068, 006, 211, 216, 239, 240, 071, 136, 247, 114, 135, 215, 103, 017, 028, 139, 095, 219, 212, 205, 221, 188, 034, 028, 138, 211, 034, 242, 158, 198, 103, 066, 061, 223, 019, 245, 124, 245, 244, 034, 190, 122, 042, 018, 135, 242, 243, 057, 196, 141, 031, 156, 045, 119, 068, 159, 173, 095, 167, 241, 134, 070, 153, 071, 031, 080, 238, 245, 070, 036, 220, 090, 030, 202, 223, 046, 228, 035, 061, 200, 231, 043, 079, 197, 205, 181, 246, 198, 213, 234, 126, 163, 178, 019, 139, 219, 186, 221, 059, 089, 028, 015, 194, 201, 073, 084, 202, 037, 033, 039, 188, 102, 137, 085, 107, 165, 166, 039, 101, 085, 135, 131, 169, 170, 148, 128, 142, 066, 222, 011, 007, 017, 251, 156, 170, 171, 200, 254, 021, 201, 248, 219, 106, 163, 180, 246, 114, 161, 150, 018, 183, 148, 041, 147, 090, 075, 198, 127, 226, 230, 122, 143, 091, 138, 246, 255, 212, 239, 065, 064, 195, 167, 077, 109, 121, 079, 080, 222, 066, 109, 005, 195, 058, 111, 005, 144, 036, 230, 071, 068, 209, 220, 021, 023, 058, 146, 160, 150, 125, 133, 051, 078, 121, 047, 078, 077, 019, 210, 163, 172, 109, 237, 184, 164, 072, 204, 172, 136, 249, 212, 164, 229, 043, 022, 144, 183, 077, 154, 073, 082, 094, 104, 228, 232, 039, 106, 121, 172, 194, 055, 148, 100, 074, 170, 254, 227, 016, 243, 021, 096, 211, 044, 193, 112, 228, 059, 147, 223, 108, 160, 093, 180, 054, 073, 135, 195, 186, 255, 058, 079, 110, 139, 253, 055, 213, 188, 072, 088, 174, 127, 111, 249, 186, 110, 203, 185, 171, 125, 196, 142, 172, 173, 088, 218, 167, 145, 164, 107, 218, 133, 000, 086, 206, 031, 237, 172, 182, 006, 179, 076, 170, 124, 031, 239, 058, 216, 029, 060, 182, 173, 030, 091, 039, 242, 212, 119, 128, 133, 187, 232, 206, 078, 188, 035, 173, 187, 211, 076, 238, 001, 212, 196, 214, 114, 207, 149, 060, 091, 055, 079, 098, 102, 100, 221, 079, 215, 075, 048, 093, 134, 012, 252, 126, 189, 043, 208, 241, 064, 237, 033, 170, 113, 178, 173, 246, 113, 177, 218, 005, 163, 054, 017, 214, 159, 043, 092, 210, 040, 174, 147, 131, 033, 184, 169, 164, 202, 224, 089, 190, 038, 098, 137, 096, 199, 239, 100, 034, 104, 202, 028, 029, 152, 142, 159, 218, 166, 071, 229, 035, 241, 082, 097, 124, 170, 235, 196, 164, 125, 210, 061, 153, 074, 069, 155, 130, 056, 190, 188, 151, 019, 036, 137, 042, 098, 001, 164, 056, 023, 239, 157, 203, 161, 229, 097, 213, 167, 081, 050, 106, 212, 019, 187, 087, 074, 184, 226, 082, 151, 067, 017, 135, 118, 078, 246, 233, 188, 147, 156, 074, 178, 199, 213, 040, 035, 210, 035, 106, 227, 017, 168, 065, 191, 041, 119, 011, 070, 167, 167, 032, 009, 194, 129, 026, 116, 049, 235, 162, 058, 248, 034, 007, 253, 156, 103, 088, 161, 083, 133, 184, 161, 108, 132, 241, 173, 160, 168, 084, 105, 144, 062, 190, 130, 240, 236, 149, 138, 131, 209, 249, 032, 076, 036, 087, 114, 042, 023, 117, 111, 016, 222, 142, 190, 242, 111, 169, 192, 057, 126, 168, 079, 151, 225, 071, 221, 012, 165, 007, 109, 011, 116, 106, 098, 083, 171, 220, 080, 046, 027, 083, 013, 206, 235, 044, 026, 199, 019, 168, 004, 036, 019, 145, 086, 210, 200, 173, 104, 143, 071, 100, 059, 077, 253, 202, 241, 050, 109, 235, 032, 010, 231, 174, 088, 080, 139, 013, 134, 105, 020, 015, 075, 126, 222, 129, 159, 089, 255, 126, 037, 130, 151, 020, 185, 146, 246, 092, 115, 055, 151, 230, 085, 161, 021, 024, 069, 115, 104, 094, 100, 189, 094, 009, 007, 212, 036, 235, 052, 114, 074, 168, 179, 189, 066, 151, 231, 230, 121, 016, 226, 070, 062, 008, 087, 196, 116, 198, 006, 227, 181, 095, 198, 178, 008, 025, 146, 021, 249, 003, 045, 166, 178, 136, 197, 159, 141, 197, 104, 131, 067, 035, 230, 141, 089, 204, 107, 201, 222, 031, 149, 023, 125, 240, 104, 128, 003, 095, 053, 048, 051, 145, 068, 044, 154, 076, 172, 104, 146, 208, 106, 103, 043, 088, 039, 097, 242, 198, 021, 086, 210, 023, 150, 207, 201, 134, 218, 057, 202, 102, 144, 175, 028, 014, 045, 168, 022, 160, 106, 176, 145, 022, 093, 151, 031, 044, 166, 177, 162, 025, 035, 225, 015, 030, 142, 229, 012, 037, 106, 037, 211, 067, 080, 100, 206, 172, 171, 075, 158, 041, 131, 145, 090, 103, 247, 061, 179, 100, 244, 222, 169, 026, 176, 168, 181, 042, 222, 093, 152, 065, 159, 142, 098, 087, 168, 085, 083, 075, 128, 170, 013, 209, 156, 047, 100, 150, 220, 156, 170, 150, 051, 024, 229, 124, 145, 208, 217, 024, 034, 180, 170, 176, 071, 205, 078, 071, 211, 208, 229, 164, 105, 077, 054, 163, 026, 159, 068, 059, 004, 170, 042, 077, 246, 132, 023, 114, 218, 223, 109, 242, 180, 152, 022, 121, 054, 154, 010, 127, 018, 178, 072, 015, 227, 103, 053, 213, 010, 247, 212, 048, 152, 120, 249, 150, 102, 250, 205, 025, 231, 084, 103, 247, 171, 109, 158, 174, 103, 171, 226, 247, 060, 059, 203, 223, 108, 182, 249, 110, 007, 101, 213, 051, 175, 023, 203, 148, 222, 175, 010, 034, 026, 094, 066, 248, 210, 020, 129, 056, 236, 005, 111, 106, 194, 039, 004, 059, 196, 215, 165, 251, 023, 247, 208, 158, 038, 218, 106, 167, 110, 035, 141, 031, 095, 238, 065, 137, 128, 245, 098, 101, 003, 255, 018, 036, 009, 018, 252, 079, 003, 181, 048, 204, 007, 049, 123, 227, 041, 152, 015, 062, 049, 198, 083, 192, 051, 150, 136, 015, 241, 105, 016, 056, 194, 200, 088, 235, 104, 179, 236, 073, 017, 190, 051, 248, 132, 165, 156, 080, 165, 202, 033, 199, 161, 153, 252, 017, 250, 249, 045, 250, 027, 145, 231, 049, 226, 155, 058, 007, 047, 070, 050, 021, 105, 062, 144, 212, 225, 240, 103, 249, 025, 240, 171, 048, 222, 013, 117, 183, 062, 046, 027, 249, 002, 116, 181, 183, 040, 209, 253, 200, 170, 107, 049, 053, 195, 151, 157, 076, 056, 014, 227, 033, 062, 184, 114, 203, 180, 023, 177, 026, 175, 185, 057, 248, 080, 154, 254, 200, 197, 150, 210, 211, 159, 176, 244, 146, 175, 156, 055, 190, 232, 226, 058, 146, 146, 145, 061, 066, 133, 009, 210, 044, 065, 022, 187, 232, 193, 145, 109, 135, 031, 095, 042, 033, 130, 191, 223, 229, 247, 217, 058, 156, 199, 138, 177, 075, 248, 179, 042, 065, 029, 058, 216, 096, 238, 240, 187, 205, 023, 124, 081, 026, 062, 120, 087, 094, 248, 144, 021, 219, 208, 043, 145, 176, 167, 141, 006, 160, 195, 075, 200, 168, 153, 078, 159, 123, 246, 243, 054, 127, 085, 172, 239, 119, 122, 244, 149, 178, 255, 058, 149, 233, 120, 084, 244, 233, 115, 022, 022, 132, 015, 124, 203, 222, 038, 124, 024, 015, 038, 017, 254, 212, 004, 007, 042, 030, 127, 056, 033, 002, 128, 254, 018, 042, 024, 127, 196, 127, 063, 134, 126, 171, 163, 032, 169, 179, 130, 097, 097, 024, 124, 002, 024, 228, 130, 184, 023, 193, 003, 223, 018, 040, 011, 200, 234, 035, 218, 045, 114, 129, 255, 206, 190, 084, 240, 133, 242, 086, 251, 185, 052, 064, 073, 166, 166, 015, 131, 145, 238, 157, 217, 208, 244, 122, 057, 065, 199, 063, 154, 068, 061, 031, 063, 035, 116, 025, 143, 159, 080, 182, 065, 016, 062, 121, 236, 123, 184, 089, 151, 202, 062, 100, 109, 221, 044, 051, 111, 001, 202, 126, 044, 101, 255, 207, 132, 186, 255, 095, 141, 012, 033, 126, 008, 185, 212, 090, 060, 026, 109, 133, 182, 157, 211, 065, 243, 180, 153, 105, 118, 012, 168, 253, 220, 231, 057, 208, 215, 067, 168, 099, 132, 141, 024, 242, 128, 070, 200, 025, 085, 167, 060, 076, 187, 221, 191, 075, 118, 072, 180, 009, 134, 103, 126, 010, 035, 048, 121, 177, 070, 083, 062, 177, 156, 086, 040, 125, 158, 004, 231, 230, 153, 021, 140, 169, 161, 008, 127, 236, 028, 038, 024, 049, 053, 150, 058, 095, 220, 213, 250, 144, 104, 087, 000, 180, 128, 016, 244, 056, 222, 047, 113, 106, 151, 255, 139, 220, 164, 164, 095, 237, 229, 076, 069, 032, 243, 111, 171, 031, 083, 239, 068, 251, 164, 217, 175, 183, 250, 002, 210, 202, 176, 197, 218, 194, 081, 058, 240, 127, 179, 026, 052, 148, 085, 052, 013, 160, 041, 129, 073, 197, 077, 122, 075, 191, 018, 089, 131, 022, 180, 150, 150, 098, 023, 231, 133, 214, 238, 189, 098, 182, 186, 136, 077, 203, 127, 189, 128, 247, 218, 145, 054, 076, 109, 239, 042, 215, 154, 201, 126, 054, 087, 019, 145, 062, 223, 253, 204, 177, 198, 018, 029, 231, 124, 004, 222, 014, 243, 022, 038, 035, 063, 239, 001, 169, 123, 242, 097, 004, 058, 051, 013, 077, 250, 040, 239, 240, 235, 111, 250, 149, 192, 015, 058, 128, 185, 133, 179, 052, 008, 189, 199, 101, 162, 155, 112, 069, 180, 161, 247, 200, 077, 019, 112, 042, 097, 081, 154, 250, 151, 206, 002, 053, 200, 094, 110, 161, 232, 123, 160, 195, 128, 241, 070, 189, 210, 131, 219, 215, 195, 033, 183, 112, 106, 106, 238, 013, 184, 238, 158, 119, 238, 133, 144, 220, 019, 116, 053, 209, 141, 177, 064, 210, 074, 012, 017, 099, 023, 166, 211, 074, 176, 039, 018, 222, 131, 154, 141, 251, 253, 252, 035, 220, 021, 123, 090, 137, 136, 123, 098, 166, 023, 007, 094, 166, 167, 104, 212, 004, 155, 078, 199, 229, 022, 028, 128, 071, 079, 010, 233, 071, 069, 039, 050, 154, 082, 187, 179, 145, 231, 156, 120, 094, 203, 041, 112, 087, 101, 059, 182, 208, 095, 062, 117, 081, 166, 118, 081, 167, 032, 226, 119, 078, 156, 118, 103, 128, 163, 251, 142, 079, 232, 169, 033, 039, 054, 193, 195, 210, 050, 012, 203, 104, 057, 222, 076, 192, 142, 206, 071, 203, 211, 251, 111, 027, 098, 228, 203, 058, 093, 075, 245, 175, 163, 013, 077, 213, 106, 193, 026, 164, 208, 007, 088, 119, 187, 149, 225, 028, 237, 254, 135, 106, 067, 052, 158, 141, 238, 156, 019, 063, 188, 235, 099, 250, 249, 121, 162, 102, 221, 238, 014, 189, 187, 083, 011, 234, 215, 061, 097, 068, 031, 063, 108, 179, 117, 027, 045, 198, 075, 077, 125, 125, 249, 002, 073, 149, 119, 206, 115, 019, 221, 018, 041, 205, 042, 039, 171, 232, 006, 088, 048, 138, 094, 119, 187, 055, 116, 156, 208, 108, 172, 240, 244, 100, 162, 150, 120, 186, 115, 244, 113, 198, 171, 137, 157, 142, 094, 015, 148, 048, 253, 159, 166, 133, 218, 160, 082, 209, 101, 000, 145, 204, 102, 189, 241, 089, 137, 164, 058, 019, 221, 110, 175, 071, 012, 192, 146, 185, 200, 007, 052, 031, 141, 095, 211, 226, 238, 039, 067, 177, 100, 176, 148, 203, 142, 048, 051, 077, 252, 255, 051, 067, 131, 092, 133, 141, 033, 254, 248, 056, 254, 205, 021, 215, 003, 229, 097, 252, 247, 134, 096, 038, 103, 079, 039, 169, 204, 087, 213, 116, 099, 127, 030, 229, 060, 012, 226, 184, 246, 255, 009, 053, 153, 075, 226, 234, 047, 050, 024, 079, 029, 091, 078, 222, 082, 170, 207, 212, 048, 083, 107, 059, 158, 164, 012, 226, 071, 033, 136, 232, 067, 141, 125, 113, 232, 008, 194, 217, 250, 034, 149, 168, 125, 169, 064, 040, 125, 107, 016, 064, 227, 027, 049, 007, 146, 027, 254, 108, 000, 181, 248, 113, 172, 098, 069, 072, 053, 153, 040, 183, 173, 154, 150, 177, 031, 215, 249, 033, 247, 146, 057, 046, 077, 011, 166, 017, 051, 074, 039, 174, 150, 179, 232, 043, 028, 082, 227, 025, 211, 060, 025, 174, 150, 083, 252, 240, 151, 099, 208, 118, 172, 162, 058, 194, 150, 072, 205, 065, 030, 202, 220, 132, 015, 171, 245, 062, 156, 183, 009, 126, 113, 197, 045, 038, 220, 243, 166, 254, 072, 121, 185, 129, 233, 168, 142, 001, 152, 205, 170, 137, 205, 162, 204, 176, 253, 185, 026, 079, 128, 070, 107, 182, 033, 208, 168, 033, 102, 110, 006, 053, 026, 166, 084, 230, 024, 078, 130, 159, 105, 080, 029, 012, 244, 200, 203, 243, 152, 073, 026, 149, 129, 103, 070, 245, 176, 119, 084, 252, 145, 095, 137, 159, 098, 184, 198, 173, 020, 173, 065, 125, 140, 045, 218, 006, 174, 156, 034, 113, 152, 111, 092, 053, 106, 222, 251, 068, 045, 113, 131, 018, 082, 205, 138, 253, 196, 101, 117, 032, 177, 226, 235, 018, 112, 093, 116, 158, 129, 115, 181, 103, 094, 140, 051, 015, 237, 046, 226, 213, 236, 068, 155, 090, 091, 041, 102, 074, 225, 020, 000, 115, 121, 006, 095, 213, 236, 099, 237, 144, 104, 040, 094, 012, 179, 245, 025, 235, 136, 224, 046, 137, 107, 170, 235, 094, 189, 089, 046, 066, 036, 160, 003, 245, 052, 249, 110, 053, 222, 137, 206, 172, 054, 007, 213, 147, 088, 204, 011, 074, 018, 054, 198, 241, 109, 184, 215, 186, 128, 180, 174, 018, 019, 148, 002, 082, 154, 170, 125, 188, 173, 088, 193, 187, 074, 143, 235, 052, 022, 113, 109, 249, 140, 125, 057, 175, 220, 099, 202, 073, 063, 016, 211, 182, 034, 035, 038, 110, 189, 110, 181, 170, 135, 020, 111, 077, 164, 049, 084, 243, 079, 165, 175, 250, 113, 010, 006, 080, 075, 165, 033, 092, 226, 038, 063, 103, 125, 254, 067, 249, 236, 131, 226, 236, 116, 128, 023, 088, 012, 029, 247, 231, 219, 156, 184, 217, 127, 209, 135, 056, 097, 173, 030, 182, 222, 230, 251, 137, 118, 114, 217, 220, 094, 048, 182, 039, 070, 086, 191, 190, 063, 051, 177, 144, 250, 230, 168, 149, 182, 255, 131, 090, 061, 009, 250, 079, 116, 175, 054, 098, 032, 174, 105, 099, 140, 075, 116, 146, 185, 061, 059, 042, 243, 212, 222, 055, 087, 065, 203, 125, 179, 021, 240, 116, 168, 178, 066, 061, 136, 028, 054, 180, 149, 042, 255, 144, 028, 129, 077, 091, 013, 048, 061, 253, 164, 205, 194, 086, 250, 208, 210, 219, 142, 061, 093, 250, 220, 058, 155, 021, 207, 243, 056, 203, 183, 109, 099, 251, 085, 111, 086, 059, 167, 048, 066, 198, 004, 182, 101, 254, 165, 037, 179, 104, 053, 253, 015, 151, 201, 209, 141, 050, 224, 230, 124, 074, 142, 138, 053, 211, 155, 118, 196, 245, 170, 078, 181, 073, 045, 160, 134, 178, 126, 088, 089, 049, 247, 001, 193, 075, 013, 063, 048, 033, 029, 128, 209, 049, 101, 234, 162, 066, 227, 091, 097, 229, 224, 063, 051, 073, 132, 230, 025, 061, 214, 146, 029, 137, 232, 056, 057, 031, 032, 079, 126, 087, 207, 081, 114, 080, 099, 216, 069, 166, 189, 036, 076, 057, 231, 171, 124, 213, 172, 205, 177, 238, 025, 038, 108, 210, 019, 061, 009, 226, 250, 029, 127, 076, 229, 215, 089, 246, 174, 226, 131, 247, 020, 095, 052, 134, 082, 049, 105, 140, 108, 095, 135, 231, 231, 032, 128, 134, 166, 154, 172, 082, 205, 236, 015, 087, 211, 235, 101, 079, 147, 246, 090, 088, 197, 197, 000, 056, 241, 072, 145, 003, 238, 119, 214, 220, 250, 097, 027, 103, 197, 026, 182, 255, 188, 249, 147, 245, 027, 060, 079, 009, 119, 227, 119, 067, 140, 236, 235, 245, 054, 195, 115, 177, 140, 103, 248, 120, 012, 074, 170, 044, 153, 068, 011, 232, 108, 150, 213, 237, 238, 147, 101, 001, 209, 150, 218, 230, 068, 065, 053, 243, 047, 037, 191, 081, 154, 219, 064, 103, 245, 184, 137, 029, 239, 042, 070, 067, 102, 087, 246, 184, 066, 142, 177, 016, 096, 019, 131, 170, 002, 037, 118, 155, 067, 172, 091, 019, 135, 107, 002, 210, 053, 089, 139, 126, 055, 018, 005, 066, 026, 183, 230, 080, 075, 070, 151, 225, 173, 149, 211, 014, 137, 178, 081, 005, 040, 166, 027, 052, 110, 196, 111, 134, 202, 009, 030, 232, 148, 233, 248, 121, 244, 131, 232, 159, 207, 001, 251, 062, 182, 200, 060, 154, 235, 074, 114, 008, 104, 052, 163, 124, 056, 204, 003, 085, 104, 137, 046, 213, 010, 253, 050, 248, 134, 160, 026, 094, 218, 026, 160, 089, 072, 156, 185, 214, 183, 085, 083, 201, 254, 032, 034, 239, 084, 108, 221, 185, 082, 135, 124, 059, 227, 187, 254, 178, 081, 203, 154, 203, 074, 176, 253, 169, 153, 198, 000, 029, 254, 153, 136, 074, 167, 207, 055, 244, 042, 035, 193, 147, 159, 243, 056, 222, 219, 139, 153, 210, 247, 049, 068, 125, 158, 104, 029, 215, 101, 169, 054, 246, 182, 051, 060, 215, 233, 225, 124, 084, 074, 222, 130, 240, 119, 090, 170, 034, 176, 115, 127, 044, 129, 226, 046, 054, 007, 128, 032, 072, 002, 078, 215, 238, 193, 243, 134, 233, 085, 050, 076, 096, 002, 220, 099, 099, 087, 185, 032, 040, 149, 117, 108, 077, 219, 184, 106, 100, 146, 208, 225, 185, 133, 198, 047, 225, 057, 071, 088, 011, 238, 132, 200, 245, 055, 229, 205, 089, 034, 231, 078, 041, 013, 000, 069, 060, 045, 101, 247, 108, 227, 172, 217, 198, 196, 185, 130, 182, 119, 067, 177, 046, 081, 019, 023, 204, 164, 039, 006, 038, 137, 111, 154, 050, 064, 206, 154, 085, 183, 212, 077, 103, 170, 174, 197, 222, 048, 087, 069, 227, 167, 187, 005, 097, 193, 013, 165, 051, 131, 151, 024, 006, 175, 032, 000, 072, 042, 012, 094, 229, 157, 243, 016, 112, 023, 168, 182, 219, 157, 091, 006, 117, 014, 081, 175, 123, 179, 009, 049, 228, 028, 034, 082, 106, 006, 185, 163, 091, 197, 223, 154, 253, 117, 148, 120, 119, 021, 141, 217, 146, 021, 171, 204, 152, 021, 140, 197, 077, 045, 109, 246, 142, 049, 206, 039, 058, 099, 203, 241, 031, 066, 096, 090, 182, 184, 143, 091, 176, 041, 193, 131, 049, 125, 031, 230, 218, 151, 129, 112, 016, 236, 013, 198, 185, 097, 176, 245, 220, 199, 174, 032, 202, 212, 005, 086, 009, 070, 220, 084, 101, 081, 002, 236, 077, 036, 166, 223, 137, 117, 070, 195, 038, 218, 194, 042, 001, 161, 164, 254, 148, 235, 129, 035, 027, 115, 253, 163, 110, 088, 255, 001, 047, 180, 175, 028, 111, 047, 182, 011, 175, 156, 046, 056, 044, 085, 070, 021, 130, 161, 163, 186, 179, 136, 242, 208, 172, 040, 194, 080, 157, 092, 190, 229, 248, 134, 252, 065, 069, 053, 090, 227, 201, 138, 044, 043, 098, 161, 004, 253, 089, 071, 051, 051, 150, 013, 110, 105, 104, 018, 097, 077, 252, 216, 083, 243, 082, 139, 131, 198, 019, 206, 153, 059, 188, 139, 096, 004, 212, 153, 210, 000, 070, 155, 144, 102, 106, 163, 150, 196, 087, 163, 122, 181, 141, 210, 017, 129, 163, 063, 029, 197, 225, 154, 056, 250, 096, 052, 158, 132, 179, 240, 142, 117, 220, 137, 130, 135, 115, 011, 201, 073, 203, 126, 019, 081, 225, 173, 090, 209, 139, 127, 163, 048, 177, 072, 184, 141, 110, 170, 128, 112, 011, 230, 115, 065, 128, 124, 203, 051, 186, 029, 175, 232, 009, 252, 231, 157, 126, 090, 016, 029, 098, 239, 186, 064, 159, 203, 003, 026, 160, 074, 111, 173, 006, 076, 181, 190, 173, 212, 119, 035, 139, 112, 071, 111, 084, 209, 048, 103, 106, 072, 148, 238, 110, 168, 059, 199, 247, 020, 167, 061, 151, 143, 190, 162, 249, 093, 004, 225, 018, 159, 136, 043, 132, 141, 254, 248, 006, 061, 156, 225, 007, 221, 147, 093, 188, 229, 001, 067, 141, 122, 180, 053, 087, 126, 107, 101, 234, 015, 194, 045, 045, 229, 072, 247, 096, 070, 019, 085, 004, 161, 049, 058, 153, 193, 041, 136, 163, 140, 254, 186, 138, 066, 021, 031, 134, 037, 076, 018, 075, 223, 055, 023, 092, 099, 190, 083, 000, 134, 007, 232, 206, 032, 103, 177, 073, 056, 047, 009, 150, 103, 080, 018, 192, 076, 157, 096, 157, 153, 242, 156, 227, 066, 067, 045, 078, 100, 250, 010, 174, 045, 152, 039, 214, 025, 009, 190, 092, 161, 073, 185, 207, 059, 240, 009, 133, 235, 249, 078, 020, 221, 096, 083, 224, 134, 036, 040, 033, 237, 086, 103, 015, 023, 250, 161, 116, 090, 038, 098, 131, 252, 056, 025, 078, 175, 138, 097, 161, 189, 098, 084, 199, 090, 232, 177, 006, 212, 001, 234, 042, 225, 160, 037, 029, 203, 193, 100, 104, 012, 181, 082, 075, 129, 056, 185, 245, 052, 203, 125, 043, 190, 234, 163, 144, 138, 210, 198, 146, 153, 206, 163, 094, 175, 160, 150, 243, 097, 046, 045, 087, 218, 205, 077, 187, 021, 233, 024, 237, 198, 226, 138, 032, 066, 186, 193, 143, 056, 253, 172, 044, 187, 056, 031, 004, 198, 233, 130, 062, 141, 105, 077, 248, 066, 171, 056, 127, 034, 085, 142, 104, 043, 134, 158, 119, 116, 188, 059, 025, 019, 033, 090, 246, 171, 162, 219, 125, 093, 086, 089, 016, 162, 161, 243, 253, 042, 151, 175, 086, 060, 110, 191, 242, 225, 027, 028, 151, 134, 190, 053, 231, 056, 247, 176, 004, 176, 055, 085, 019, 024, 043, 106, 113, 028, 135, 092, 225, 190, 185, 142, 098, 212, 173, 148, 129, 213, 033, 109, 118, 066, 149, 187, 200, 187, 244, 212, 062, 034, 132, 065, 155, 234, 030, 059, 235, 085, 116, 163, 222, 000, 213, 176, 046, 174, 081, 071, 134, 250, 046, 021, 087, 111, 163, 215, 189, 072, 152, 018, 104, 175, 084, 028, 092, 029, 014, 253, 129, 250, 061, 122, 099, 246, 036, 214, 229, 086, 124, 140, 137, 146, 017, 193, 247, 109, 048, 220, 017, 108, 253, 222, 237, 106, 151, 096, 139, 232, 205, 120, 007, 003, 048, 090, 054, 070, 013, 221, 238, 034, 120, 088, 195, 245, 200, 225, 176, 104, 104, 037, 173, 032, 034, 245, 023, 032, 129, 058, 027, 035, 233, 184, 163, 021, 089, 227, 090, 026, 162, 121, 127, 129, 146, 043, 053, 135, 181, 156, 076, 036, 225, 014, 033, 139, 208, 155, 215, 209, 219, 224, 008, 111, 094, 080, 037, 184, 067, 107, 221, 238, 246, 252, 092, 209, 020, 236, 077, 118, 198, 089, 219, 094, 180, 083, 148, 017, 253, 221, 114, 151, 108, 115, 137, 052, 119, 231, 239, 213, 061, 166, 054, 040, 239, 243, 183, 087, 151, 090, 098, 188, 035, 100, 180, 167, 177, 029, 014, 247, 252, 215, 199, 079, 244, 185, 040, 121, 020, 180, 115, 238, 129, 114, 238, 131, 163, 193, 035, 133, 186, 039, 020, 075, 231, 005, 117, 229, 222, 174, 035, 245, 206, 122, 135, 001, 136, 086, 020, 038, 168, 030, 171, 144, 032, 099, 163, 035, 239, 085, 160, 246, 199, 082, 235, 030, 103, 077, 016, 078, 077, 190, 121, 052, 021, 125, 051, 234, 099, 043, 229, 174, 053, 046, 216, 001, 204, 051, 135, 110, 239, 208, 000, 229, 038, 112, 198, 054, 146, 037, 228, 025, 085, 041, 026, 240, 052, 034, 200, 134, 015, 028, 130, 104, 136, 054, 173, 078, 069, 152, 155, 167, 033, 085, 075, 237, 189, 193, 089, 136, 019, 114, 106, 213, 006, 162, 216, 081, 245, 046, 074, 133, 130, 170, 213, 050, 159, 184, 232, 170, 227, 013, 035, 106, 243, 046, 005, 211, 176, 117, 132, 217, 164, 014, 071, 043, 219, 012, 078, 033, 209, 015, 203, 089, 250, 015, 130, 109, 109, 016, 189, 208, 106, 107, 080, 091, 107, 231, 190, 151, 136, 002, 179, 006, 079, 136, 126, 253, 242, 005, 240, 128, 127, 203, 247, 006, 129, 102, 214, 173, 065, 133, 216, 227, 184, 198, 103, 027, 236, 037, 139, 144, 110, 248, 158, 158, 017, 018, 043, 052, 070, 165, 001, 134, 127, 107, 176, 155, 203, 118, 104, 161, 104, 018, 160, 191, 001, 165, 168, 142, 085, 176, 203, 135, 043, 190, 200, 174, 200, 250, 084, 137, 092, 110, 012, 099, 033, 132, 186, 025, 231, 177, 136, 126, 238, 187, 038, 245, 198, 224, 146, 152, 178, 218, 033, 095, 208, 186, 162, 159, 024, 109, 001, 241, 188, 029, 200, 034, 186, 173, 032, 086, 080, 206, 203, 072, 006, 051, 094, 240, 025, 060, 141, 150, 239, 028, 147, 054, 244, 188, 049, 231, 096, 187, 193, 167, 054, 125, 189, 049, 039, 115, 161, 006, 024, 163, 017, 237, 051, 230, 188, 129, 125, 078, 221, 238, 022, 192, 162, 114, 115, 113, 164, 193, 203, 039, 044, 050, 039, 088, 090, 007, 068, 014, 178, 054, 021, 001, 085, 135, 096, 251, 221, 086, 167, 084, 209, 081, 185, 106, 070, 209, 189, 177, 101, 244, 002, 163, 100, 164, 149, 119, 217, 012, 229, 094, 053, 085, 149, 162, 078, 135, 032, 022, 002, 220, 138, 062, 231, 009, 131, 159, 193, 059, 212, 099, 219, 109, 058, 091, 020, 209, 045, 145, 095, 181, 047, 128, 160, 052, 250, 224, 079, 098, 093, 224, 041, 239, 079, 034, 120, 042, 101, 126, 053, 137, 019, 242, 131, 247, 037, 022, 054, 022, 249, 211, 129, 101, 173, 243, 188, 152, 205, 247, 135, 215, 069, 182, 159, 123, 170, 093, 082, 068, 104, 072, 020, 212, 194, 186, 038, 154, 242, 236, 149, 112, 085, 126, 069, 167, 203, 019, 177, 225, 042, 117, 217, 026, 026, 219, 173, 067, 099, 065, 219, 005, 027, 056, 056, 131, 169, 170, 207, 243, 094, 240, 216, 007, 163, 247, 158, 113, 075, 086, 059, 112, 093, 242, 212, 056, 137, 056, 023, 065, 095, 231, 180, 160, 175, 156, 011, 099, 111, 199, 138, 086, 167, 022, 078, 123, 089, 171, 117, 171, 084, 186, 215, 061, 251, 107, 163, 079, 226, 239, 172, 177, 002, 016, 005, 065, 242, 059, 170, 205, 120, 200, 158, 183, 218, 117, 002, 051, 071, 039, 048, 115, 117, 002, 009, 127, 199, 071, 168, 057, 174, 120, 215, 071, 123, 118, 137, 185, 217, 070, 251, 082, 027, 076, 127, 026, 019, 165, 036, 046, 054, 055, 091, 043, 093, 090, 185, 234, 127, 230, 133, 010, 151, 095, 041, 139, 184, 190, 053, 074, 118, 236, 085, 235, 151, 111, 190, 166, 029, 065, 031, 249, 145, 062, 089, 253, 206, 189, 125, 100, 213, 203, 251, 019, 062, 047, 248, 144, 043, 221, 123, 154, 075, 147, 152, 133, 026, 129, 184, 080, 040, 229, 219, 134, 177, 119, 190, 104, 106, 101, 197, 150, 059, 059, 184, 032, 021, 036, 168, 143, 188, 216, 158, 204, 217, 145, 136, 170, 083, 098, 205, 241, 164, 069, 180, 094, 183, 101, 142, 059, 044, 128, 078, 077, 205, 142, 222, 221, 107, 051, 161, 140, 098, 043, 024, 157, 200, 184, 139, 223, 158, 026, 119, 034, 240, 063, 114, 049, 186, 242, 071, 225, 211, 235, 139, 235, 193, 213, 001, 254, 069, 222, 082, 142, 254, 248, 183, 240, 079, 215, 227, 235, 190, 154, 060, 126, 116, 081, 074, 130, 126, 055, 243, 005, 255, 173, 174, 031, 179, 196, 094, 075, 173, 250, 240, 035, 087, 209, 247, 001, 047, 097, 084, 055, 140, 127, 065, 056, 023, 132, 070, 108, 122, 228, 115, 055, 105, 040, 243, 182, 212, 083, 229, 112, 202, 194, 077, 183, 107, 220, 193, 183, 086, 237, 216, 086, 041, 124, 132, 241, 234, 007, 075, 156, 242, 139, 093, 155, 119, 181, 012, 151, 215, 218, 003, 041, 173, 025, 177, 079, 210, 139, 163, 169, 232, 004, 096, 225, 014, 213, 185, 025, 035, 136, 242, 194, 213, 154, 240, 051, 084, 165, 096, 214, 036, 002, 034, 115, 114, 177, 002, 076, 201, 097, 201, 054, 170, 107, 095, 179, 070, 210, 104, 156, 193, 126, 050, 172, 102, 193, 245, 176, 012, 034, 105, 027, 068, 021, 148, 216, 103, 179, 227, 099, 216, 127, 096, 149, 203, 054, 221, 059, 190, 186, 102, 207, 115, 016, 011, 107, 130, 192, 089, 128, 082, 037, 203, 076, 121, 221, 153, 052, 054, 134, 158, 114, 231, 002, 130, 197, 213, 068, 075, 231, 090, 140, 200, 208, 085, 154, 139, 192, 125, 030, 059, 210, 046, 165, 086, 090, 178, 234, 022, 146, 041, 000, 100, 033, 191, 035, 038, 138, 234, 142, 167, 175, 006, 035, 131, 085, 160, 121, 149, 242, 193, 107, 232, 077, 206, 108, 222, 070, 149, 055, 118, 079, 017, 135, 049, 252, 073, 106, 045, 190, 150, 217, 173, 181, 246, 187, 118, 112, 205, 100, 101, 103, 192, 147, 221, 126, 009, 250, 206, 130, 151, 001, 059, 217, 109, 187, 055, 235, 232, 156, 077, 053, 058, 226, 041, 045, 009, 135, 018, 161, 237, 133, 049, 249, 061, 138, 046, 250, 051, 245, 169, 184, 030, 034, 140, 224, 063, 037, 236, 112, 253, 243, 164, 119, 021, 140, 127, 187, 154, 060, 062, 104, 119, 068, 143, 217, 251, 208, 243, 200, 058, 126, 111, 133, 117, 034, 174, 196, 073, 174, 011, 001, 044, 078, 130, 142, 217, 179, 150, 078, 010, 210, 164, 051, 090, 206, 220, 116, 030, 111, 159, 237, 137, 186, 038, 146, 250, 170, 242, 201, 240, 177, 196, 130, 151, 206, 248, 174, 162, 015, 071, 099, 017, 003, 176, 206, 194, 036, 252, 212, 056, 163, 081, 108, 041, 146, 019, 097, 221, 237, 026, 210, 024, 020, 093, 210, 023, 127, 241, 035, 136, 205, 210, 064, 131, 077, 016, 054, 028, 117, 039, 054, 141, 089, 003, 227, 212, 134, 232, 234, 051, 130, 203, 125, 188, 074, 049, 128, 213, 008, 187, 059, 076, 148, 235, 200, 157, 094, 216, 057, 049, 200, 015, 046, 169, 018, 163, 076, 198, 091, 186, 197, 213, 078, 198, 139, 172, 222, 200, 138, 113, 107, 077, 207, 212, 137, 184, 195, 204, 217, 179, 104, 080, 065, 193, 236, 156, 156, 200, 238, 145, 126, 032, 150, 139, 222, 100, 084, 172, 193, 152, 043, 254, 050, 116, 022, 134, 229, 112, 081, 086, 055, 208, 206, 225, 027, 007, 220, 239, 212, 037, 119, 089, 191, 173, 095, 192, 009, 084, 238, 056, 207, 121, 102, 039, 201, 113, 173, 030, 013, 120, 199, 130, 101, 178, 092, 166, 235, 167, 062, 202, 084, 101, 107, 069, 049, 191, 031, 045, 241, 102, 103, 202, 175, 020, 051, 149, 074, 246, 106, 099, 065, 088, 119, 173, 217, 166, 045, 154, 246, 225, 185, 239, 237, 072, 255, 242, 174, 240, 087, 068, 232, 216, 115, 191, 212, 089, 039, 076, 093, 235, 165, 125, 084, 149, 110, 197, 230, 009, 158, 045, 173, 131, 091, 063, 214, 104, 235, 056, 124, 238, 092, 168, 097, 003, 169, 103, 017, 116, 076, 121, 243, 189, 144, 173, 039, 051, 189, 059, 064, 047, 145, 094, 255, 190, 218, 023, 139, 003, 155, 009, 095, 168, 207, 162, 007, 086, 164, 163, 028, 124, 055, 040, 010, 047, 059, 060, 131, 070, 224, 187, 065, 042, 134, 235, 189, 097, 005, 149, 067, 077, 167, 013, 147, 175, 076, 223, 224, 040, 197, 096, 114, 119, 157, 078, 035, 233, 172, 021, 073, 139, 063, 082, 144, 073, 014, 146, 038, 214, 104, 177, 222, 229, 174, 167, 254, 042, 169, 163, 173, 191, 092, 159, 252, 184, 152, 083, 179, 200, 034, 046, 034, 158, 027, 167, 203, 136, 235, 041, 093, 235, 243, 204, 135, 151, 229, 109, 001, 059, 068, 102, 088, 201, 038, 067, 008, 178, 065, 044, 013, 107, 118, 091, 236, 094, 201, 042, 043, 012, 032, 007, 158, 141, 102, 162, 056, 163, 117, 100, 235, 134, 232, 039, 078, 098, 246, 050, 076, 195, 178, 119, 208, 149, 027, 183, 026, 094, 159, 150, 023, 043, 046, 161, 203, 002, 025, 214, 097, 160, 230, 091, 149, 057, 070, 045, 222, 142, 075, 122, 068, 239, 012, 133, 034, 188, 019, 012, 157, 098, 176, 029, 196, 202, 161, 094, 245, 080, 231, 238, 118, 245, 131, 051, 047, 035, 189, 254, 196, 250, 248, 001, 071, 050, 128, 197, 174, 057, 049, 224, 197, 070, 197, 089, 086, 091, 209, 019, 199, 189, 059, 190, 074, 156, 011, 066, 056, 196, 239, 074, 097, 118, 086, 075, 085, 126, 090, 143, 143, 225, 086, 074, 233, 190, 113, 045, 093, 011, 177, 016, 214, 222, 013, 236, 066, 108, 234, 122, 119, 253, 092, 186, 154, 173, 207, 132, 160, 055, 238, 218, 136, 222, 170, 210, 245, 229, 173, 189, 118, 083, 251, 208, 162, 180, 098, 116, 069, 154, 054, 177, 137, 182, 084, 115, 209, 189, 054, 150, 210, 123, 188, 109, 148, 247, 176, 031, 118, 238, 064, 003, 155, 155, 049, 193, 041, 133, 245, 122, 049, 168, 035, 010, 078, 104, 105, 003, 051, 080, 081, 027, 014, 142, 130, 055, 078, 229, 173, 235, 072, 235, 186, 009, 032, 078, 014, 161, 165, 250, 119, 101, 063, 209, 194, 251, 199, 236, 182, 195, 131, 070, 077, 127, 096, 170, 234, 090, 223, 040, 186, 147, 151, 214, 117, 121, 005, 115, 201, 114, 130, 017, 010, 035, 112, 004, 003, 028, 073, 197, 226, 230, 214, 242, 174, 028, 001, 153, 013, 242, 110, 101, 047, 140, 116, 000, 125, 045, 166, 091, 246, 090, 051, 210, 199, 203, 106, 239, 152, 019, 154, 079, 226, 102, 216, 026, 069, 134, 102, 155, 017, 022, 141, 093, 239, 136, 176, 060, 169, 108, 089, 118, 214, 028, 079, 074, 042, 206, 185, 243, 113, 130, 190, 036, 037, 041, 237, 241, 244, 086, 148, 248, 063, 014, 248, 098, 147, 072, 232, 140, 232, 181, 058, 126, 202, 248, 134, 211, 050, 089, 025, 223, 117, 056, 200, 030, 178, 107, 255, 051, 086, 038, 230, 124, 014, 182, 064, 206, 023, 165, 232, 013, 130, 089, 058, 176, 095, 229, 091, 214, 099, 210, 181, 056, 052, 125, 096, 072, 217, 191, 068, 023, 215, 047, 123, 023, 179, 114, 223, 127, 081, 110, 215, 007, 043, 003, 215, 059, 091, 091, 203, 250, 127, 097, 049, 170, 059, 067, 004, 055, 236, 183, 029, 231, 152, 074, 008, 019, 060, 039, 190, 053, 161, 182, 118, 021, 131, 062, 226, 228, 154, 088, 025, 045, 134, 229, 033, 124, 084, 218, 228, 211, 094, 015, 203, 025, 199, 119, 211, 231, 008, 209, 082, 059, 102, 113, 103, 179, 038, 234, 018, 118, 247, 212, 129, 161, 185, 239, 029, 034, 123, 240, 144, 070, 051, 035, 192, 213, 056, 172, 215, 155, 063, 053, 231, 074, 048, 029, 207, 205, 253, 088, 138, 019, 033, 005, 041, 201, 154, 140, 108, 003, 185, 095, 111, 190, 091, 125, 030, 047, 118, 162, 040, 099, 138, 177, 042, 076, 112, 164, 009, 201, 151, 235, 237, 091, 086, 064, 161, 015, 138, 163, 167, 228, 044, 181, 077, 113, 059, 204, 062, 252, 111, 162, 135, 202, 025, 080, 106, 250, 234, 059, 244, 164, 082, 055, 141, 113, 102, 206, 198, 082, 043, 246, 012, 046, 044, 030, 244, 066, 036, 181, 232, 066, 021, 082, 046, 197, 022, 016, 224, 192, 013, 048, 017, 053, 028, 238, 197, 158, 183, 033, 107, 155, 026, 238, 185, 164, 023, 140, 159, 110, 024, 121, 101, 184, 059, 011, 142, 142, 143, 116, 165, 059, 090, 248, 026, 158, 096, 100, 008, 183, 117, 045, 227, 050, 224, 098, 061, 180, 183, 220, 146, 024, 113, 017, 220, 131, 155, 211, 055, 145, 152, 049, 068, 075, 004, 083, 035, 168, 078, 033, 168, 038, 198, 133, 154, 159, 159, 159, 031, 077, 219, 117, 082, 205, 030, 251, 174, 204, 097, 202, 100, 137, 037, 033, 154, 154, 154, 149, 149, 096, 037, 039, 093, 189, 150, 013, 182, 197, 189, 138, 102, 114, 179, 195, 102, 169, 149, 220, 045, 075, 220, 153, 030, 213, 098, 237, 158, 212, 078, 069, 160, 077, 015, 135, 027, 163, 018, 235, 155, 198, 081, 160, 181, 178, 078, 206, 042, 137, 249, 207, 197, 190, 018, 232, 162, 196, 218, 057, 067, 098, 202, 219, 147, 077, 002, 180, 090, 241, 200, 168, 023, 211, 234, 079, 074, 240, 002, 089, 232, 044, 040, 234, 110, 233, 233, 077, 223, 052, 090, 015, 063, 229, 148, 107, 237, 111, 118, 180, 040, 228, 198, 013, 245, 243, 002, 222, 195, 183, 173, 106, 191, 227, 177, 183, 205, 119, 235, 197, 043, 008, 151, 179, 245, 138, 126, 028, 108, 002, 151, 103, 105, 126, 038, 187, 014, 162, 103, 157, 055, 243, 038, 010, 005, 217, 131, 167, 242, 166, 049, 097, 222, 247, 148, 187, 097, 013, 094, 046, 183, 090, 239, 139, 233, 091, 015, 231, 221, 122, 006, 243, 234, 090, 089, 083, 108, 130, 073, 245, 224, 083, 134, 015, 195, 044, 122, 032, 190, 118, 223, 054, 101, 041, 097, 177, 197, 235, 248, 237, 174, 109, 225, 251, 024, 150, 179, 181, 250, 232, 174, 223, 152, 213, 253, 220, 061, 035, 141, 115, 076, 155, 173, 196, 205, 102, 054, 029, 103, 129, 237, 152, 066, 095, 251, 205, 162, 042, 235, 039, 226, 090, 038, 242, 242, 241, 148, 240, 223, 196, 111, 052, 059, 131, 075, 188, 214, 016, 100, 067, 029, 018, 170, 172, 015, 220, 219, 178, 216, 229, 192, 067, 250, 145, 105, 099, 153, 091, 102, 035, 048, 223, 129, 204, 003, 088, 075, 094, 067, 061, 015, 120, 199, 218, 016, 164, 082, 103, 046, 039, 061, 015, 144, 231, 077, 184, 093, 246, 203, 145, 150, 181, 074, 236, 191, 153, 196, 049, 177, 093, 130, 107, 067, 021, 139, 023, 205, 160, 204, 012, 170, 135, 031, 223, 021, 170, 206, 002, 041, 004, 177, 033, 001, 048, 113, 092, 229, 065, 152, 245, 055, 005, 107, 121, 098, 113, 084, 115, 142, 227, 114, 142, 167, 176, 235, 034, 172, 062, 254, 112, 050, 204, 100, 090, 233, 036, 162, 131, 064, 205, 121, 046, 179, 204, 157, 229, 052, 154, 031, 085, 050, 030, 252, 022, 079, 168, 156, 065, 009, 244, 229, 009, 191, 003, 037, 004, 042, 231, 009, 153, 068, 045, 048, 213, 058, 085, 249, 040, 011, 219, 119, 108, 053, 063, 245, 203, 236, 112, 118, 161, 102, 102, 012, 215, 160, 108, 082, 010, 033, 116, 078, 244, 008, 238, 239, 094, 207, 243, 054, 061, 112, 168, 057, 054, 002, 107, 056, 252, 050, 097, 204, 129, 056, 196, 126, 007, 188, 100, 033, 002, 166, 176, 009, 255, 040, 014, 029, 200, 134, 186, 194, 009, 066, 213, 126, 206, 064, 131, 016, 137, 166, 153, 107, 122, 170, 199, 114, 035, 038, 178, 209, 197, 048, 099, 075, 144, 098, 052, 211, 096, 201, 120, 014, 013, 132, 231, 231, 211, 195, 097, 102, 192, 211, 126, 039, 144, 224, 139, 115, 118, 196, 113, 053, 096, 254, 185, 096, 189, 094, 057, 119, 050, 088, 198, 085, 094, 111, 043, 175, 086, 038, 000, 001, 064, 109, 046, 240, 169, 156, 014, 247, 205, 221, 065, 232, 007, 244, 183, 244, 014, 194, 235, 045, 014, 078, 217, 064, 051, 187, 129, 104, 000, 214, 007, 095, 125, 036, 040, 128, 131, 192, 110, 014, 077, 017, 126, 041, 034, 017, 022, 247, 068, 109, 251, 068, 146, 156, 110, 009, 054, 051, 144, 085, 034, 121, 027, 154, 109, 160, 184, 200, 207, 113, 177, 015, 007, 106, 190, 094, 100, 146, 080, 161, 012, 071, 186, 098, 228, 234, 245, 066, 253, 230, 119, 224, 238, 124, 219, 200, 206, 046, 114, 058, 151, 163, 243, 115, 167, 024, 075, 180, 184, 106, 040, 094, 217, 023, 246, 234, 219, 065, 246, 110, 183, 146, 255, 234, 146, 178, 125, 089, 153, 149, 076, 141, 087, 028, 112, 101, 202, 209, 177, 102, 179, 092, 059, 087, 128, 116, 011, 032, 086, 251, 234, 123, 092, 027, 156, 235, 035, 113, 061, 157, 218, 047, 065, 141, 161, 254, 138, 118, 107, 214, 244, 001, 202, 122, 086, 160, 163, 106, 126, 064, 095, 124, 247, 141, 182, 212, 250, 122, 029, 103, 112, 019, 246, 087, 088, 199, 183, 230, 021, 023, 160, 127, 013, 112, 001, 137, 251, 114, 215, 065, 040, 247, 134, 143, 169, 116, 030, 175, 102, 185, 174, 167, 150, 203, 214, 080, 234, 072, 253, 149, 250, 235, 055, 059, 124, 056, 072, 115, 044, 107, 134, 197, 194, 222, 088, 114, 224, 154, 029, 254, 034, 089, 033, 089, 102, 249, 037, 218, 005, 067, 242, 021, 228, 023, 122, 073, 003, 092, 000, 085, 128, 168, 132, 051, 185, 139, 234, 124, 009, 113, 211, 151, 081, 005, 001, 156, 172, 095, 247, 072, 019, 177, 110, 010, 180, 073, 235, 254, 185, 232, 253, 101, 186, 093, 047, 022, 001, 251, 142, 253, 177, 088, 230, 235, 123, 200, 091, 184, 088, 096, 221, 129, 055, 071, 030, 052, 063, 157, 092, 166, 102, 070, 051, 195, 162, 169, 151, 213, 253, 184, 158, 088, 166, 090, 046, 091, 137, 092, 125, 234, 016, 121, 105, 100, 046, 184, 153, 005, 182, 038, 095, 141, 177, 059, 030, 127, 152, 007, 048, 083, 001, 141, 041, 179, 236, 083, 095, 150, 160, 220, 075, 015, 220, 134, 205, 076, 067, 201, 167, 123, 207, 184, 186, 118, 195, 182, 057, 243, 057, 085, 031, 211, 206, 173, 044, 251, 241, 232, 219, 091, 195, 047, 045, 254, 072, 248, 062, 173, 134, 083, 120, 124, 095, 243, 165, 213, 215, 016, 222, 175, 160, 082, 038, 151, 195, 101, 076, 062, 104, 222, 017, 040, 124, 173, 022, 068, 242, 047, 138, 085, 254, 041, 078, 201, 111, 113, 127, 251, 117, 252, 150, 058, 001, 094, 108, 213, 032, 101, 196, 060, 116, 152, 086, 004, 249, 174, 043, 218, 100, 141, 237, 203, 188, 032, 155, 229, 237, 223, 194, 031, 142, 159, 080, 129, 054, 125, 017, 190, 221, 107, 079, 144, 178, 253, 116, 183, 099, 215, 054, 222, 070, 107, 158, 132, 113, 066, 040, 231, 126, 159, 015, 147, 245, 022, 022, 091, 151, 067, 086, 254, 160, 095, 209, 005, 161, 007, 098, 060, 233, 047, 166, 058, 060, 255, 051, 253, 183, 121, 003, 231, 026, 174, 071, 067, 034, 152, 170, 014, 014, 085, 187, 135, 078, 233, 196, 239, 235, 245, 018, 163, 168, 247, 137, 040, 013, 068, 246, 009, 101, 010, 135, 075, 058, 025, 139, 021, 181, 108, 123, 182, 033, 088, 166, 189, 021, 014, 054, 111, 116, 047, 241, 132, 234, 194, 129, 119, 122, 238, 227, 232, 067, 190, 165, 037, 148, 072, 096, 241, 051, 010, 130, 152, 240, 083, 167, 063, 209, 128, 157, 022, 187, 238, 207, 115, 214, 191, 105, 144, 159, 237, 051, 076, 192, 032, 046, 106, 063, 147, 208, 161, 224, 254, 001, 172, 218, 111, 109, 204, 018, 145, 018, 078, 027, 185, 007, 071, 077, 036, 106, 136, 251, 166, 197, 169, 045, 132, 076, 047, 226, 125, 060, 118, 236, 225, 196, 065, 068, 205, 003, 021, 194, 227, 058, 070, 011, 214, 153, 249, 064, 098, 159, 066, 027, 034, 069, 112, 043, 190, 084, 211, 199, 081, 171, 243, 013, 235, 130, 087, 125, 171, 175, 025, 031, 228, 142, 241, 241, 245, 241, 112, 061, 054, 207, 019, 092, 048, 126, 023, 093, 248, 227, 103, 231, 255, 064, 076, 232, 242, 168, 249, 222, 081, 060, 176, 113, 040, 027, 030, 222, 109, 236, 146, 140, 134, 119, 238, 245, 074, 063, 102, 223, 041, 239, 028, 026, 184, 053, 139, 062, 190, 151, 172, 117, 153, 168, 155, 134, 072, 039, 213, 040, 035, 242, 246, 219, 123, 070, 215, 052, 240, 203, 144, 184, 050, 066, 125, 250, 117, 016, 122, 152, 121, 121, 099, 255, 051, 189, 180, 231, 201, 107, 047, 013, 191, 053, 254, 102, 070, 250, 106, 240, 171, 151, 223, 125, 203, 178, 011, 007, 135, 173, 250, 232, 185, 030, 171, 040, 161, 167, 090, 033, 101, 248, 031, 071, 203, 137, 217, 105, 249, 091, 105, 081, 232, 004, 236, 132, 202, 029, 079, 001, 123, 024, 103, 183, 207, 149, 168, 168, 194, 031, 177, 109, 225, 026, 189, 224, 108, 045, 198, 026, 182, 153, 031, 202, 072, 037, 084, 247, 055, 144, 172, 027, 091, 116, 162, 098, 089, 217, 004, 224, 199, 086, 022, 102, 045, 016, 107, 156, 134, 202, 062, 159, 194, 024, 193, 198, 071, 048, 178, 008, 241, 167, 219, 101, 181, 129, 091, 120, 086, 184, 101, 051, 008, 216, 064, 221, 078, 120, 244, 068, 233, 216, 171, 184, 172, 229, 246, 199, 106, 057, 222, 018, 173, 099, 170, 141, 180, 105, 122, 025, 223, 046, 156, 019, 209, 074, 117, 178, 013, 205, 237, 132, 242, 061, 028, 195, 007, 025, 112, 136, 045, 176, 222, 028, 203, 120, 198, 029, 039, 006, 091, 169, 055, 090, 126, 133, 128, 114, 196, 245, 148, 225, 128, 233, 013, 126, 005, 109, 199, 171, 073, 252, 137, 029, 000, 205, 216, 022, 066, 229, 108, 079, 130, 175, 246, 129, 237, 121, 102, 145, 188, 004, 170, 028, 054, 007, 022, 198, 228, 233, 144, 164, 180, 215, 039, 081, 027, 096, 038, 035, 182, 243, 079, 038, 074, 014, 076, 145, 250, 052, 202, 006, 008, 116, 060, 083, 083, 199, 216, 231, 165, 179, 163, 202, 021, 205, 180, 093, 130, 093, 069, 226, 250, 156, 085, 156, 131, 139, 025, 219, 005, 159, 132, 246, 145, 141, 166, 096, 068, 195, 055, 228, 034, 020, 030, 225, 067, 136, 063, 050, 064, 145, 234, 105, 193, 088, 048, 130, 126, 170, 086, 165, 023, 121, 051, 174, 208, 109, 183, 169, 203, 012, 206, 025, 229, 163, 241, 133, 028, 219, 217, 029, 148, 146, 123, 120, 202, 128, 228, 196, 009, 002, 024, 184, 022, 068, 078, 012, 009, 237, 251, 123, 140, 107, 112, 009, 151, 057, 234, 252, 013, 222, 072, 027, 027, 196, 218, 046, 029, 253, 148, 086, 075, 151, 180, 035, 081, 127, 147, 177, 178, 181, 032, 207, 015, 130, 024, 002, 167, 250, 112, 085, 073, 020, 126, 088, 195, 204, 196, 171, 116, 104, 157, 037, 246, 241, 200, 169, 143, 167, 071, 111, 113, 196, 023, 041, 153, 013, 153, 243, 007, 168, 139, 160, 230, 240, 193, 131, 176, 034, 223, 159, 121, 184, 238, 245, 242, 101, 146, 103, 250, 217, 132, 056, 012, 009, 221, 018, 178, 013, 095, 060, 249, 063, 207, 095, 124, 250, 201, 103, 231, 207, 062, 251, 228, 197, 249, 096, 144, 078, 207, 255, 252, 201, 167, 255, 117, 254, 209, 071, 031, 125, 252, 241, 135, 031, 127, 116, 073, 255, 121, 044, 113, 228, 154, 091, 053, 172, 098, 087, 255, 136, 251, 050, 118, 087, 030, 027, 185, 124, 083, 028, 046, 177, 243, 055, 182, 217, 206, 170, 117, 086, 088, 219, 031, 012, 110, 211, 162, 214, 023, 245, 188, 054, 231, 075, 237, 095, 241, 159, 127, 160, 058, 197, 060, 213, 063, 255, 080, 149, 156, 247, 024, 084, 053, 158, 026, 109, 148, 177, 061, 177, 031, 204, 221, 234, 044, 098, 157, 136, 082, 249, 116, 232, 030, 071, 162, 201, 226, 092, 111, 232, 219, 015, 070, 233, 083, 209, 241, 154, 058, 151, 201, 004, 117, 255, 148, 052, 190, 193, 219, 229, 025, 014, 161, 029, 248, 044, 022, 241, 055, 020, 233, 103, 227, 084, 236, 200, 240, 208, 135, 207, 102, 117, 201, 156, 131, 117, 087, 038, 231, 158, 220, 201, 184, 187, 037, 211, 178, 210, 143, 105, 111, 124, 047, 230, 109, 048, 033, 012, 160, 162, 217, 218, 007, 158, 037, 035, 137, 209, 015, 205, 008, 206, 114, 019, 203, 018, 035, 135, 190, 209, 035, 022, 049, 013, 188, 141, 180, 008, 045, 222, 095, 144, 003, 051, 135, 211, 017, 186, 011, 117, 054, 221, 203, 056, 112, 092, 173, 183, 173, 119, 245, 218, 184, 217, 068, 089, 168, 236, 161, 192, 131, 001, 134, 187, 251, 252, 062, 111, 064, 092, 069, 113, 054, 070, 084, 121, 182, 186, 155, 190, 129, 119, 045, 046, 002, 241, 169, 153, 078, 246, 051, 003, 077, 191, 078, 038, 209, 064, 077, 076, 250, 081, 037, 079, 069, 091, 004, 177, 229, 179, 242, 154, 132, 131, 122, 149, 131, 205, 242, 102, 191, 130, 007, 014, 201, 133, 078, 104, 254, 137, 189, 204, 223, 107, 063, 055, 142, 168, 010, 065, 117, 172, 049, 049, 122, 192, 217, 190, 088, 175, 111, 119, 214, 039, 078, 101, 033, 242, 178, 158, 227, 016, 234, 203, 070, 142, 028, 233, 216, 031, 078, 133, 025, 193, 038, 095, 017, 161, 035, 145, 086, 075, 053, 062, 113, 221, 178, 148, 085, 048, 223, 148, 175, 161, 148, 017, 093, 017, 025, 001, 011, 200, 014, 157, 123, 188, 193, 248, 078, 131, 005, 120, 016, 113, 058, 125, 109, 015, 025, 173, 231, 159, 115, 120, 165, 016, 217, 204, 114, 202, 084, 129, 125, 083, 015, 114, 101, 114, 090, 150, 094, 151, 100, 082, 097, 007, 106, 168, 101, 179, 224, 128, 154, 106, 082, 042, 065, 104, 234, 248, 165, 109, 233, 164, 243, 079, 204, 221, 106, 067, 201, 133, 025, 052, 196, 012, 198, 172, 042, 236, 127, 085, 223, 072, 079, 137, 208, 212, 011, 110, 080, 148, 141, 068, 192, 174, 221, 240, 053, 108, 221, 008, 085, 112, 177, 091, 110, 088, 005, 013, 189, 067, 148, 094, 088, 068, 209, 113, 150, 019, 244, 063, 043, 145, 148, 240, 082, 110, 169, 022, 128, 125, 223, 214, 108, 169, 004, 007, 235, 246, 111, 239, 172, 071, 003, 170, 108, 003, 088, 188, 182, 009, 197, 093, 083, 166, 129, 202, 171, 114, 024, 065, 240, 180, 005, 092, 061, 164, 185, 187, 033, 206, 207, 051, 056, 200, 115, 229, 106, 083, 133, 152, 214, 180, 055, 078, 047, 156, 062, 213, 097, 120, 163, 055, 169, 019, 201, 200, 162, 001, 056, 152, 082, 113, 005, 134, 003, 225, 211, 025, 078, 129, 203, 123, 061, 165, 223, 024, 052, 231, 165, 137, 229, 220, 007, 083, 238, 072, 030, 090, 088, 077, 226, 041, 119, 115, 234, 227, 237, 207, 219, 120, 195, 092, 237, 206, 029, 027, 244, 184, 228, 166, 192, 218, 202, 015, 099, 008, 098, 172, 165, 106, 169, 073, 251, 239, 008, 024, 070, 255, 127, 149, 047, 156, 075, 232, 129, 243, 100, 253, 230, 124, 087, 252, 014, 073, 130, 214, 197, 192, 167, 225, 249, 114, 253, 251, 169, 180, 019, 159, 141, 196, 034, 193, 226, 252, 219, 002, 139, 164, 050, 168, 246, 153, 013, 244, 032, 184, 116, 228, 125, 140, 185, 137, 163, 015, 059, 053, 121, 070, 083, 118, 081, 034, 148, 163, 145, 041, 252, 136, 000, 164, 231, 147, 017, 248, 248, 236, 241, 117, 255, 016, 092, 103, 061, 122, 025, 231, 159, 077, 056, 129, 094, 015, 193, 133, 142, 162, 086, 141, 138, 203, 049, 092, 057, 000, 108, 016, 029, 002, 223, 235, 253, 136, 120, 195, 227, 248, 252, 247, 255, 156, 060, 182, 097, 093, 127, 138, 198, 222, 143, 235, 013, 189, 253, 128, 181, 165, 223, 079, 215, 251, 253, 122, 073, 015, 095, 067, 082, 055, 081, 063, 159, 138, 078, 077, 199, 033, 148, 131, 112, 203, 202, 062, 163, 104, 213, 036, 080, 003, 207, 048, 188, 106, 117, 028, 173, 197, 154, 151, 110, 032, 157, 082, 230, 240, 139, 049, 232, 047, 163, 148, 013, 008, 073, 060, 065, 120, 250, 172, 233, 123, 245, 140, 102, 254, 126, 235, 187, 030, 038, 156, 043, 006, 233, 071, 162, 088, 125, 162, 136, 176, 111, 111, 032, 192, 072, 217, 111, 174, 207, 025, 036, 008, 058, 113, 051, 035, 015, 254, 041, 017, 068, 226, 054, 170, 037, 017, 034, 161, 004, 152, 098, 119, 187, 189, 034, 128, 031, 091, 086, 119, 182, 045, 136, 215, 015, 098, 173, 111, 169, 102, 054, 217, 126, 184, 137, 110, 014, 007, 188, 042, 115, 145, 126, 027, 245, 010, 008, 116, 178, 245, 217, 020, 118, 189, 094, 255, 099, 079, 221, 094, 068, 083, 058, 182, 024, 080, 184, 175, 183, 189, 027, 163, 096, 002, 015, 154, 196, 088, 082, 183, 047, 208, 040, 036, 064, 083, 220, 061, 204, 044, 149, 008, 098, 135, 170, 037, 166, 028, 085, 067, 171, 019, 122, 039, 163, 219, 158, 143, 223, 222, 032, 120, 156, 142, 159, 076, 194, 030, 254, 066, 089, 136, 000, 245, 030, 074, 228, 055, 112, 038, 179, 143, 183, 251, 232, 150, 158, 008, 140, 035, 168, 254, 228, 028, 251, 252, 215, 218, 013, 153, 009, 123, 174, 157, 118, 180, 249, 117, 128, 217, 142, 019, 250, 050, 114, 116, 064, 130, 007, 040, 040, 176, 160, 100, 014, 198, 049, 013, 126, 229, 106, 231, 042, 037, 030, 076, 193, 207, 014, 085, 109, 061, 073, 086, 184, 113, 086, 109, 168, 220, 107, 101, 236, 031, 130, 163, 160, 223, 176, 230, 168, 095, 154, 182, 152, 128, 135, 065, 008, 031, 031, 244, 114, 130, 125, 185, 145, 018, 056, 210, 152, 114, 128, 057, 039, 095, 190, 089, 215, 020, 009, 059, 239, 131, 203, 142, 081, 022, 102, 186, 001, 124, 153, 043, 147, 228, 184, 163, 200, 071, 113, 120, 051, 050, 253, 008, 194, 098, 148, 176, 155, 098, 092, 253, 077, 143, 028, 121, 152, 182, 159, 241, 044, 116, 096, 095, 067, 028, 158, 248, 081, 116, 193, 070, 065, 033, 172, 130, 056, 196, 240, 111, 143, 014, 215, 023, 148, 249, 038, 126, 021, 031, 242, 116, 025, 007, 187, 116, 091, 108, 246, 148, 025, 113, 135, 127, 187, 222, 245, 046, 016, 118, 216, 139, 147, 100, 123, 160, 229, 043, 232, 168, 062, 196, 196, 125, 210, 223, 123, 170, 247, 144, 100, 197, 033, 141, 087, 175, 226, 221, 129, 133, 030, 248, 179, 040, 118, 251, 003, 046, 106, 010, 004, 092, 047, 226, 197, 122, 118, 152, 022, 051, 068, 022, 162, 217, 193, 227, 253, 054, 063, 076, 215, 235, 125, 190, 061, 136, 239, 175, 195, 124, 182, 093, 223, 111, 014, 075, 218, 176, 244, 103, 123, 123, 088, 230, 072, 093, 197, 175, 014, 235, 251, 061, 226, 046, 111, 138, 116, 143, 114, 134, 048, 057, 236, 114, 158, 237, 195, 238, 126, 073, 037, 222, 030, 246, 116, 118, 046, 112, 175, 178, 047, 150, 249, 225, 021, 245, 113, 237, 149, 251, 060, 117, 130, 098, 038, 149, 080, 148, 144, 013, 010, 054, 053, 024, 226, 243, 109, 060, 099, 180, 042, 178, 195, 042, 174, 053, 065, 072, 140, 034, 086, 045, 025, 198, 163, 236, 022, 180, 052, 213, 234, 212, 207, 234, 083, 007, 099, 121, 148, 054, 123, 162, 210, 102, 041, 019, 081, 168, 098, 005, 121, 118, 246, 116, 065, 100, 192, 197, 213, 083, 054, 085, 189, 122, 122, 161, 127, 141, 225, 231, 069, 252, 193, 085, 204, 129, 165, 184, 252, 025, 171, 171, 127, 096, 224, 229, 003, 216, 079, 046, 104, 112, 124, 051, 245, 243, 188, 032, 006, 120, 019, 167, 057, 139, 197, 043, 198, 146, 086, 122, 180, 232, 239, 065, 026, 068, 039, 131, 228, 237, 133, 114, 176, 017, 099, 251, 243, 253, 114, 241, 050, 223, 018, 104, 192, 139, 084, 231, 100, 065, 012, 164, 094, 238, 227, 231, 011, 194, 251, 145, 247, 052, 036, 224, 160, 209, 241, 143, 220, 160, 213, 166, 135, 018, 168, 112, 138, 236, 108, 207, 072, 187, 184, 079, 192, 036, 019, 069, 103, 032, 015, 220, 051, 003, 007, 025, 161, 125, 241, 001, 013, 084, 207, 219, 052, 160, 214, 205, 007, 201, 099, 115, 171, 170, 017, 170, 009, 012, 126, 245, 134, 102, 222, 060, 099, 074, 087, 107, 238, 185, 041, 207, 190, 255, 042, 125, 179, 222, 123, 043, 214, 161, 181, 158, 196, 239, 130, 004, 054, 113, 106, 137, 045, 197, 056, 160, 037, 217, 196, 191, 082, 246, 169, 153, 071, 199, 155, 218, 055, 034, 083, 241, 156, 112, 057, 089, 145, 250, 112, 078, 013, 206, 076, 155, 157, 017, 190, 038, 228, 249, 168, 095, 063, 042, 087, 252, 068, 231, 242, 194, 013, 020, 089, 015, 001, 105, 115, 006, 134, 134, 201, 226, 232, 065, 156, 042, 134, 227, 129, 242, 076, 032, 181, 037, 077, 109, 177, 089, 016, 208, 155, 167, 015, 096, 006, 093, 006, 072, 155, 168, 069, 062, 163, 113, 074, 169, 105, 145, 047, 050, 154, 019, 201, 083, 190, 017, 061, 079, 139, 032, 121, 150, 241, 070, 146, 249, 097, 002, 165, 238, 120, 041, 073, 114, 070, 073, 170, 121, 134, 165, 027, 109, 048, 201, 032, 219, 147, 211, 245, 035, 037, 111, 195, 241, 019, 155, 246, 148, 055, 144, 206, 194, 143, 078, 214, 116, 189, 104, 201, 107, 051, 082, 050, 163, 086, 041, 109, 223, 220, 182, 168, 031, 031, 054, 202, 239, 183, 186, 189, 237, 085, 075, 163, 255, 212, 016, 026, 214, 246, 242, 104, 124, 009, 095, 190, 158, 055, 225, 177, 253, 242, 148, 016, 155, 212, 195, 015, 147, 227, 048, 035, 106, 108, 179, 231, 078, 068, 242, 076, 203, 163, 232, 073, 112, 008, 030, 112, 050, 224, 193, 116, 150, 159, 229, 240, 224, 116, 076, 029, 151, 152, 243, 107, 086, 034, 249, 060, 174, 137, 245, 056, 250, 198, 031, 143, 009, 059, 058, 129, 136, 196, 243, 082, 016, 182, 215, 084, 015, 124, 054, 106, 137, 133, 102, 106, 208, 215, 060, 226, 085, 003, 068, 192, 084, 252, 005, 187, 090, 220, 068, 220, 014, 181, 147, 148, 076, 130, 222, 178, 111, 027, 092, 192, 057, 170, 227, 025, 034, 052, 077, 141, 015, 066, 171, 017, 062, 085, 052, 009, 025, 083, 136, 070, 055, 221, 074, 010, 014, 135, 004, 108, 188, 163, 125, 158, 224, 162, 074, 171, 146, 199, 019, 197, 238, 066, 202, 043, 245, 184, 197, 144, 199, 244, 044, 069, 068, 016, 234, 089, 198, 070, 153, 194, 228, 166, 202, 155, 045, 214, 073, 188, 248, 236, 085, 188, 240, 148, 116, 088, 146, 216, 171, 151, 155, 026, 004, 076, 252, 205, 136, 204, 120, 122, 232, 254, 105, 116, 253, 186, 055, 188, 080, 115, 188, 050, 036, 092, 020, 229, 178, 074, 064, 235, 127, 024, 247, 158, 218, 091, 132, 111, 173, 232, 013, 074, 181, 206, 084, 029, 117, 144, 155, 083, 078, 195, 140, 139, 066, 181, 084, 235, 146, 190, 220, 068, 041, 124, 037, 074, 108, 199, 045, 013, 119, 125, 181, 029, 110, 197, 016, 106, 070, 067, 222, 078, 224, 100, 006, 179, 201, 126, 080, 155, 020, 232, 044, 008, 204, 140, 222, 169, 050, 050, 239, 136, 248, 255, 112, 086, 170, 105, 204, 228, 146, 023, 217, 031, 138, 136, 104, 233, 077, 149, 105, 109, 231, 239, 136, 252, 245, 031, 009, 043, 048, 131, 046, 187, 108, 181, 160, 025, 253, 099, 073, 059, 099, 124, 003, 247, 221, 113, 223, 236, 085, 085, 056, 231, 212, 018, 228, 250, 138, 055, 239, 247, 068, 021, 136, 234, 254, 044, 232, 045, 065, 184, 079, 041, 249, 210, 088, 206, 076, 225, 242, 045, 042, 074, 004, 206, 192, 219, 066, 033, 116, 187, 177, 029, 148, 013, 058, 104, 006, 002, 166, 154, 207, 002, 202, 163, 251, 015, 007, 042, 129, 234, 104, 234, 033, 120, 152, 069, 030, 227, 023, 230, 123, 014, 135, 185, 173, 108, 100, 113, 036, 165, 160, 227, 149, 196, 203, 176, 008, 011, 215, 086, 099, 042, 074, 159, 229, 126, 170, 202, 216, 049, 030, 103, 015, 220, 070, 110, 214, 241, 148, 160, 084, 147, 043, 221, 110, 231, 182, 089, 011, 170, 118, 153, 231, 091, 104, 005, 153, 245, 046, 092, 091, 012, 122, 115, 252, 086, 195, 129, 162, 246, 170, 226, 090, 138, 020, 149, 202, 042, 073, 195, 034, 218, 148, 179, 046, 028, 203, 169, 089, 157, 129, 033, 236, 118, 055, 213, 218, 026, 068, 011, 095, 156, 194, 098, 156, 112, 196, 157, 178, 084, 067, 017, 007, 012, 235, 090, 112, 021, 221, 141, 183, 218, 169, 082, 198, 122, 126, 090, 049, 125, 070, 060, 015, 116, 220, 115, 196, 167, 224, 126, 056, 000, 141, 027, 098, 203, 113, 207, 106, 028, 247, 012, 078, 015, 169, 205, 042, 144, 207, 112, 213, 201, 252, 006, 245, 097, 014, 007, 071, 232, 050, 220, 004, 058, 125, 041, 036, 094, 147, 118, 208, 050, 051, 074, 091, 088, 158, 212, 116, 194, 112, 167, 133, 184, 034, 219, 052, 040, 110, 022, 112, 157, 082, 198, 104, 113, 163, 042, 186, 076, 236, 148, 021, 190, 166, 139, 021, 187, 084, 077, 017, 157, 193, 235, 037, 202, 095, 192, 147, 070, 042, 247, 240, 184, 052, 174, 146, 076, 169, 080, 075, 156, 041, 119, 040, 022, 220, 231, 104, 018, 069, 002, 012, 012, 243, 138, 026, 199, 109, 044, 172, 027, 047, 203, 065, 136, 145, 131, 033, 033, 153, 131, 091, 032, 199, 109, 254, 246, 066, 045, 117, 222, 229, 250, 126, 071, 060, 209, 186, 088, 129, 093, 210, 086, 146, 052, 186, 251, 067, 070, 172, 003, 253, 089, 111, 130, 067, 186, 040, 210, 219, 011, 181, 210, 101, 244, 152, 248, 231, 192, 127, 137, 042, 078, 022, 247, 091, 104, 102, 172, 057, 211, 248, 183, 254, 228, 113, 000, 217, 079, 223, 239, 247, 130, 067, 112, 081, 243, 030, 219, 084, 031, 184, 115, 062, 015, 042, 206, 064, 069, 173, 194, 202, 079, 042, 014, 190, 181, 090, 004, 161, 247, 154, 139, 074, 215, 203, 034, 031, 020, 106, 094, 101, 249, 075, 047, 016, 013, 225, 108, 202, 087, 099, 240, 102, 167, 140, 098, 069, 080, 074, 004, 146, 128, 235, 159, 115, 253, 112, 151, 079, 077, 148, 198, 128, 090, 080, 202, 002, 001, 019, 001, 006, 247, 032, 184, 103, 049, 117, 133, 058, 001, 151, 033, 013, 037, 018, 228, 038, 242, 196, 230, 213, 175, 078, 087, 032, 026, 229, 245, 207, 163, 187, 216, 110, 160, 078, 233, 197, 212, 170, 224, 068, 114, 215, 063, 139, 114, 149, 183, 042, 181, 250, 162, 187, 025, 067, 037, 182, 085, 203, 253, 168, 114, 086, 149, 032, 044, 135, 031, 086, 074, 224, 119, 163, 065, 193, 177, 139, 154, 082, 122, 209, 144, 132, 028, 090, 219, 135, 229, 052, 089, 124, 239, 161, 211, 162, 007, 057, 204, 113, 119, 093, 183, 020, 173, 196, 138, 113, 078, 090, 120, 236, 090, 171, 013, 187, 160, 179, 119, 053, 204, 099, 111, 017, 131, 117, 110, 053, 086, 011, 158, 174, 194, 124, 161, 193, 023, 214, 038, 155, 227, 151, 203, 080, 210, 218, 080, 020, 205, 212, 086, 186, 183, 019, 193, 141, 121, 019, 159, 170, 112, 027, 041, 085, 006, 172, 085, 098, 222, 090, 166, 214, 161, 244, 236, 210, 174, 180, 014, 184, 214, 030, 021, 133, 090, 118, 166, 046, 052, 137, 241, 179, 099, 114, 064, 028, 201, 190, 089, 100, 101, 110, 251, 052, 130, 101, 101, 109, 228, 083, 196, 225, 024, 153, 068, 036, 126, 213, 049, 085, 163, 179, 093, 205, 235, 074, 014, 115, 118, 182, 182, 214, 199, 040, 224, 087, 172, 218, 214, 209, 093, 004, 109, 125, 034, 097, 124, 232, 240, 075, 109, 090, 206, 209, 055, 126, 170, 002, 181, 102, 199, 124, 166, 143, 236, 220, 039, 094, 140, 215, 019, 216, 060, 082, 037, 208, 128, 097, 173, 134, 025, 206, 023, 120, 032, 190, 233, 039, 197, 042, 099, 205, 171, 195, 097, 173, 078, 150, 093, 148, 170, 049, 015, 236, 186, 120, 173, 214, 052, 071, 092, 199, 157, 232, 011, 100, 074, 175, 104, 152, 042, 172, 089, 040, 043, 168, 204, 218, 134, 185, 114, 253, 216, 132, 108, 140, 125, 194, 201, 141, 246, 160, 016, 040, 048, 168, 076, 128, 132, 027, 237, 131, 171, 207, 226, 087, 090, 240, 101, 052, 163, 222, 097, 177, 229, 009, 068, 221, 210, 142, 238, 249, 250, 158, 224, 248, 082, 221, 000, 125, 223, 111, 096, 116, 198, 015, 165, 243, 154, 141, 186, 133, 251, 154, 206, 128, 106, 104, 114, 167, 163, 022, 125, 217, 053, 129, 057, 162, 040, 085, 084, 096, 161, 048, 087, 211, 136, 245, 122, 148, 019, 038, 142, 055, 168, 003, 075, 130, 095, 211, 242, 002, 039, 183, 158, 042, 003, 236, 213, 015, 145, 204, 028, 112, 201, 104, 105, 108, 206, 106, 067, 235, 245, 212, 037, 187, 064, 053, 222, 013, 149, 089, 057, 217, 186, 152, 016, 098, 205, 135, 090, 153, 176, 105, 024, 247, 135, 183, 178, 214, 049, 097, 035, 206, 218, 198, 102, 129, 177, 221, 148, 184, 203, 062, 001, 232, 055, 117, 064, 191, 017, 095, 195, 115, 007, 214, 111, 028, 088, 159, 107, 088, 159, 191, 011, 214, 131, 135, 197, 059, 064, 061, 027, 045, 170, 160, 190, 168, 130, 250, 050, 186, 229, 220, 108, 194, 057, 231, 072, 125, 181, 088, 104, 215, 215, 253, 192, 235, 025, 184, 163, 055, 058, 050, 251, 143, 175, 113, 093, 002, 141, 001, 031, 079, 136, 142, 006, 235, 207, 104, 217, 036, 068, 103, 068, 207, 018, 201, 217, 129, 011, 080, 132, 175, 234, 155, 029, 067, 103, 023, 251, 021, 188, 103, 079, 028, 006, 121, 207, 017, 140, 202, 080, 066, 022, 238, 169, 175, 004, 065, 146, 207, 113, 106, 225, 061, 126, 236, 137, 118, 093, 167, 252, 206, 123, 193, 192, 203, 020, 054, 138, 110, 153, 026, 000, 157, 159, 019, 024, 010, 080, 116, 187, 230, 201, 222, 230, 011, 201, 217, 089, 150, 122, 048, 068, 202, 231, 241, 054, 035, 218, 015, 217, 205, 179, 041, 176, 081, 022, 251, 234, 061, 181, 114, 205, 002, 224, 150, 175, 204, 097, 116, 008, 176, 000, 090, 090, 143, 003, 124, 141, 003, 252, 054, 048, 075, 042, 197, 081, 178, 007, 224, 096, 096, 005, 068, 215, 053, 189, 110, 089, 089, 070, 106, 052, 077, 052, 110, 245, 061, 129, 081, 143, 227, 247, 105, 236, 094, 245, 035, 238, 018, 035, 188, 015, 244, 046, 136, 198, 180, 094, 196, 223, 222, 069, 183, 050, 088, 227, 220, 046, 128, 167, 053, 000, 086, 066, 219, 164, 076, 179, 043, 199, 025, 236, 155, 003, 192, 225, 088, 156, 122, 071, 203, 136, 157, 085, 102, 010, 183, 120, 185, 163, 087, 244, 095, 181, 247, 206, 074, 179, 068, 119, 189, 198, 009, 133, 209, 223, 149, 250, 067, 084, 191, 056, 068, 222, 070, 119, 078, 155, 212, 253, 173, 213, 246, 216, 234, 045, 004, 003, 038, 167, 104, 232, 005, 079, 047, 187, 093, 070, 096, 119, 208, 205, 115, 228, 115, 240, 127, 064, 187, 099, 213, 151, 229, 188, 083, 077, 090, 013, 046, 122, 084, 066, 139, 243, 163, 116, 045, 154, 142, 158, 132, 031, 042, 103, 014, 162, 109, 137, 195, 233, 251, 182, 076, 112, 050, 141, 090, 119, 225, 246, 189, 187, 048, 148, 000, 202, 184, 225, 039, 126, 088, 147, 099, 136, 180, 203, 033, 112, 224, 225, 084, 063, 070, 057, 164, 172, 250, 166, 137, 053, 015, 043, 042, 060, 010, 154, 181, 170, 137, 089, 238, 004, 179, 076, 105, 199, 045, 204, 236, 195, 151, 173, 126, 180, 158, 041, 083, 217, 000, 226, 218, 018, 110, 074, 059, 144, 133, 126, 122, 159, 112, 004, 091, 214, 083, 020, 131, 127, 168, 149, 179, 204, 226, 038, 170, 098, 170, 195, 225, 078, 153, 005, 191, 233, 221, 097, 087, 131, 071, 119, 252, 158, 012, 139, 097, 237, 203, 070, 142, 129, 002, 210, 129, 098, 136, 168, 097, 196, 184, 212, 028, 020, 001, 084, 116, 190, 165, 149, 066, 023, 249, 235, 195, 097, 169, 171, 146, 142, 193, 147, 234, 177, 116, 140, 075, 141, 111, 196, 051, 046, 140, 173, 225, 205, 136, 248, 142, 120, 198, 001, 131, 094, 238, 215, 196, 243, 101, 004, 074, 178, 023, 162, 245, 213, 096, 116, 227, 160, 090, 140, 101, 134, 027, 080, 057, 055, 138, 114, 035, 178, 027, 134, 177, 020, 155, 148, 039, 011, 229, 144, 045, 076, 032, 226, 216, 119, 022, 108, 025, 134, 120, 126, 005, 136, 250, 050, 165, 219, 253, 198, 047, 056, 232, 167, 089, 249, 074, 017, 251, 085, 140, 231, 019, 118, 050, 002, 055, 012, 050, 124, 063, 208, 190, 235, 184, 243, 119, 138, 215, 011, 099, 212, 233, 223, 075, 110, 140, 016, 154, 096, 011, 043, 111, 193, 202, 155, 103, 221, 222, 070, 238, 134, 168, 085, 097, 000, 208, 051, 136, 178, 168, 207, 057, 065, 079, 115, 237, 105, 251, 099, 044, 208, 080, 192, 131, 220, 055, 170, 038, 009, 122, 199, 246, 003, 168, 195, 055, 006, 046, 059, 214, 052, 175, 103, 212, 048, 111, 043, 092, 090, 230, 217, 076, 195, 145, 237, 194, 153, 112, 173, 090, 186, 089, 112, 159, 022, 111, 252, 138, 175, 003, 209, 016, 031, 195, 105, 121, 211, 148, 242, 166, 092, 090, 113, 096, 086, 093, 093, 033, 157, 039, 250, 194, 186, 190, 163, 108, 234, 195, 081, 130, 028, 176, 087, 168, 184, 220, 011, 178, 093, 185, 226, 206, 045, 086, 238, 133, 238, 251, 225, 080, 121, 149, 110, 105, 157, 035, 217, 125, 015, 115, 219, 156, 166, 172, 118, 110, 046, 117, 003, 170, 220, 066, 248, 148, 008, 128, 068, 067, 120, 124, 010, 194, 031, 098, 104, 007, 096, 155, 232, 142, 077, 133, 230, 079, 203, 122, 016, 150, 206, 052, 055, 078, 157, 010, 191, 092, 046, 243, 172, 064, 172, 228, 182, 154, 099, 007, 021, 114, 129, 242, 181, 141, 030, 032, 106, 085, 026, 161, 003, 048, 154, 097, 194, 160, 136, 046, 042, 232, 196, 140, 250, 126, 125, 162, 075, 202, 099, 034, 206, 079, 164, 056, 172, 047, 117, 111, 003, 013, 195, 122, 072, 069, 077, 147, 061, 054, 187, 040, 051, 094, 040, 216, 064, 182, 186, 147, 148, 120, 166, 112, 070, 008, 055, 031, 214, 145, 053, 237, 142, 221, 222, 172, 088, 183, 091, 125, 175, 172, 160, 138, 075, 104, 053, 179, 121, 090, 181, 215, 120, 225, 072, 170, 228, 013, 235, 021, 008, 194, 007, 112, 001, 113, 056, 007, 042, 172, 090, 138, 244, 086, 028, 161, 136, 212, 169, 216, 125, 027, 127, 075, 195, 146, 216, 079, 001, 252, 179, 200, 227, 211, 129, 185, 209, 239, 068, 226, 010, 175, 130, 122, 197, 135, 149, 241, 036, 090, 105, 164, 176, 129, 197, 216, 180, 006, 230, 136, 213, 086, 245, 001, 144, 201, 053, 193, 229, 112, 206, 177, 144, 122, 196, 002, 194, 113, 009, 196, 002, 021, 207, 133, 202, 074, 251, 017, 182, 028, 084, 015, 253, 064, 033, 217, 225, 156, 070, 043, 058, 131, 184, 071, 218, 013, 086, 193, 254, 038, 180, 107, 069, 073, 146, 208, 128, 227, 098, 098, 110, 095, 137, 034, 227, 010, 173, 095, 239, 097, 230, 072, 068, 037, 112, 013, 000, 035, 044, 202, 005, 201, 142, 118, 109, 231, 079, 147, 246, 236, 220, 152, 045, 145, 104, 021, 230, 057, 172, 169, 102, 240, 208, 080, 245, 146, 133, 208, 099, 014, 201, 081, 074, 076, 170, 001, 023, 246, 098, 212, 000, 083, 006, 237, 231, 234, 013, 235, 220, 017, 173, 061, 156, 035, 132, 103, 237, 099, 052, 143, 150, 250, 052, 157, 138, 107, 189, 062, 011, 214, 068, 029, 117, 081, 075, 186, 205, 223, 074, 002, 132, 010, 121, 052, 135, 102, 222, 102, 103, 028, 086, 209, 163, 049, 120, 208, 041, 065, 088, 038, 193, 188, 223, 033, 146, 102, 192, 049, 121, 149, 047, 072, 088, 113, 048, 135, 185, 071, 012, 223, 052, 208, 003, 183, 242, 169, 146, 086, 049, 143, 032, 250, 183, 169, 022, 167, 225, 004, 087, 114, 083, 047, 169, 046, 172, 217, 018, 054, 177, 226, 056, 188, 191, 204, 247, 241, 095, 243, 183, 124, 019, 171, 159, 213, 092, 187, 245, 025, 205, 173, 195, 045, 162, 252, 195, 152, 213, 047, 055, 187, 208, 139, 023, 123, 202, 119, 150, 048, 241, 178, 059, 075, 225, 047, 113, 001, 128, 062, 075, 247, 219, 005, 146, 042, 024, 241, 076, 244, 066, 206, 024, 047, 124, 079, 124, 035, 020, 114, 185, 169, 051, 118, 046, 158, 103, 058, 031, 019, 162, 248, 044, 093, 061, 131, 082, 199, 203, 125, 188, 220, 156, 189, 034, 122, 004, 177, 119, 210, 185, 231, 152, 131, 040, 179, 156, 016, 072, 149, 043, 164, 123, 009, 231, 146, 103, 248, 243, 156, 198, 122, 070, 201, 248, 135, 231, 090, 021, 053, 119, 159, 142, 210, 153, 049, 085, 229, 134, 121, 050, 249, 201, 132, 153, 233, 155, 202, 071, 229, 035, 065, 179, 110, 005, 225, 206, 143, 202, 001, 041, 211, 047, 065, 031, 103, 242, 067, 179, 183, 040, 104, 090, 126, 209, 191, 191, 158, 077, 183, 235, 165, 094, 217, 051, 081, 224, 251, 069, 255, 254, 122, 070, 024, 052, 255, 133, 255, 254, 122, 182, 075, 183, 121, 190, 250, 069, 255, 254, 122, 182, 095, 235, 082, 239, 031, 158, 019, 180, 139, 070, 033, 029, 097, 084, 233, 180, 061, 172, 205, 001, 055, 109, 002, 057, 208, 120, 165, 215, 172, 064, 101, 065, 171, 078, 088, 034, 048, 107, 221, 174, 151, 093, 047, 224, 178, 069, 233, 058, 203, 202, 122, 062, 235, 136, 239, 216, 116, 023, 234, 128, 154, 033, 118, 063, 092, 006, 231, 146, 075, 202, 056, 185, 220, 015, 080, 205, 229, 218, 127, 181, 181, 255, 090, 169, 253, 199, 245, 166, 082, 057, 191, 215, 234, 046, 243, 056, 239, 144, 228, 226, 036, 118, 225, 150, 105, 057, 191, 246, 145, 208, 075, 185, 035, 217, 249, 182, 158, 000, 152, 175, 105, 064, 050, 246, 112, 184, 089, 228, 221, 045, 224, 053, 232, 034, 012, 204, 019, 250, 251, 097, 248, 017, 253, 125, 018, 094, 010, 048, 233, 131, 059, 124, 128, 109, 053, 162, 209, 010, 251, 128, 139, 010, 029, 097, 243, 161, 193, 195, 090, 211, 021, 004, 182, 136, 065, 175, 010, 034, 068, 246, 192, 145, 209, 151, 095, 233, 200, 238, 012, 092, 025, 189, 170, 200, 075, 060, 125, 149, 224, 029, 021, 238, 015, 090, 155, 116, 234, 140, 042, 205, 162, 132, 118, 252, 137, 071, 110, 202, 181, 132, 104, 180, 179, 190, 223, 123, 080, 022, 167, 243, 241, 093, 045, 057, 151, 123, 066, 112, 234, 107, 046, 098, 091, 173, 174, 015, 245, 132, 027, 150, 248, 013, 226, 097, 018, 245, 026, 071, 164, 120, 174, 117, 200, 170, 060, 188, 199, 163, 155, 094, 102, 229, 197, 030, 004, 009, 073, 078, 199, 119, 126, 191, 146, 101, 114, 009, 154, 170, 051, 146, 210, 047, 169, 016, 054, 144, 025, 130, 036, 043, 086, 184, 048, 103, 033, 162, 095, 251, 210, 151, 214, 089, 075, 200, 150, 131, 121, 153, 218, 021, 203, 251, 069, 197, 195, 079, 197, 081, 181, 149, 211, 058, 071, 017, 076, 038, 088, 084, 017, 171, 098, 247, 082, 151, 207, 248, 222, 107, 088, 099, 035, 252, 076, 168, 003, 024, 159, 156, 224, 131, 210, 006, 233, 119, 020, 115, 121, 043, 238, 137, 090, 253, 071, 140, 234, 029, 110, 245, 028, 129, 185, 105, 115, 040, 193, 054, 103, 039, 198, 044, 087, 119, 195, 138, 007, 009, 016, 121, 045, 114, 126, 040, 054, 112, 064, 229, 204, 048, 092, 085, 191, 019, 184, 008, 097, 235, 127, 025, 072, 235, 113, 001, 048, 170, 248, 015, 150, 204, 035, 159, 029, 211, 236, 075, 159, 183, 149, 021, 053, 030, 111, 153, 213, 212, 100, 012, 127, 104, 206, 114, 100, 181, 031, 236, 039, 007, 133, 052, 019, 101, 206, 074, 120, 001, 141, 062, 218, 196, 225, 157, 241, 137, 044, 109, 170, 068, 068, 238, 012, 030, 114, 245, 163, 221, 240, 217, 003, 056, 146, 033, 152, 087, 081, 074, 121, 237, 011, 063, 032, 038, 042, 142, 210, 022, 220, 029, 087, 228, 066, 098, 240, 167, 223, 028, 135, 185, 015, 142, 083, 230, 208, 000, 101, 115, 220, 212, 097, 213, 198, 125, 201, 247, 119, 048, 081, 200, 080, 005, 202, 166, 147, 170, 230, 138, 012, 079, 077, 255, 038, 102, 183, 000, 117, 030, 103, 212, 100, 122, 194, 234, 196, 195, 255, 158, 170, 113, 065, 255, 078, 079, 154, 035, 211, 125, 233, 232, 012, 118, 243, 114, 247, 106, 045, 025, 231, 128, 021, 022, 012, 062, 036, 153, 110, 147, 051, 036, 098, 067, 075, 228, 106, 155, 206, 127, 167, 175, 239, 088, 014, 221, 105, 233, 077, 091, 190, 119, 165, 105, 151, 115, 205, 145, 048, 150, 017, 111, 174, 076, 109, 097, 173, 182, 161, 199, 207, 132, 046, 182, 158, 016, 097, 139, 060, 126, 149, 155, 207, 116, 054, 040, 125, 129, 174, 179, 235, 055, 041, 160, 095, 116, 017, 147, 196, 071, 081, 221, 213, 102, 077, 118, 049, 137, 030, 042, 167, 088, 162, 140, 200, 139, 030, 133, 229, 105, 056, 164, 074, 075, 223, 251, 053, 034, 130, 025, 027, 203, 212, 091, 101, 112, 166, 186, 088, 202, 239, 058, 211, 087, 018, 093, 084, 080, 008, 177, 127, 134, 185, 039, 138, 203, 138, 031, 218, 047, 140, 149, 046, 003, 203, 198, 035, 135, 082, 239, 139, 094, 004, 219, 047, 084, 070, 168, 019, 162, 007, 190, 053, 251, 035, 039, 049, 029, 133, 075, 047, 128, 023, 005, 198, 084, 141, 107, 102, 097, 126, 251, 255, 148, 138, 065, 163, 111, 160, 234, 109, 062, 120, 170, 062, 093, 137, 037, 171, 032, 175, 045, 155, 075, 236, 169, 095, 209, 153, 163, 207, 058, 110, 051, 059, 104, 032, 200, 193, 039, 233, 149, 081, 205, 075, 029, 155, 221, 084, 121, 186, 101, 200, 246, 220, 238, 218, 148, 246, 190, 197, 230, 179, 221, 081, 108, 044, 088, 175, 085, 060, 254, 195, 150, 237, 020, 093, 080, 171, 168, 188, 198, 168, 037, 104, 087, 169, 078, 032, 112, 022, 042, 105, 057, 123, 121, 107, 109, 040, 003, 223, 118, 161, 086, 016, 182, 183, 008, 104, 045, 119, 055, 255, 131, 069, 213, 119, 051, 146, 199, 206, 082, 032, 032, 037, 010, 054, 045, 032, 037, 009, 167, 065, 234, 086, 243, 226, 220, 107, 027, 231, 123, 228, 151, 164, 093, 167, 066, 218, 105, 213, 103, 247, 163, 211, 106, 009, 120, 128, 133, 124, 187, 127, 043, 237, 247, 255, 105, 188, 025, 185, 107, 097, 149, 164, 249, 128, 173, 146, 098, 166, 060, 186, 099, 028, 202, 255, 243, 230, 030, 202, 091, 168, 040, 099, 156, 090, 074, 110, 027, 016, 223, 214, 094, 163, 146, 250, 162, 182, 053, 051, 040, 027, 041, 215, 218, 212, 110, 109, 046, 029, 210, 182, 217, 037, 033, 089, 089, 077, 135, 074, 183, 247, 173, 186, 249, 134, 102, 089, 146, 114, 077, 156, 109, 148, 040, 211, 131, 250, 054, 178, 041, 237, 173, 116, 106, 192, 009, 137, 155, 115, 192, 201, 235, 143, 230, 162, 229, 221, 227, 174, 194, 184, 179, 029, 203, 222, 217, 237, 120, 002, 057, 059, 003, 118, 200, 060, 113, 191, 242, 174, 142, 149, 064, 152, 024, 176, 116, 193, 085, 062, 142, 028, 236, 254, 110, 020, 109, 089, 146, 119, 238, 210, 214, 093, 104, 150, 065, 117, 218, 055, 146, 236, 078, 205, 213, 241, 140, 242, 089, 042, 012, 165, 229, 246, 132, 217, 115, 153, 178, 054, 163, 103, 119, 246, 026, 075, 147, 040, 139, 184, 171, 215, 010, 193, 113, 088, 063, 072, 147, 073, 011, 062, 016, 170, 094, 072, 143, 170, 168, 065, 031, 159, 102, 121, 161, 011, 061, 196, 021, 106, 083, 089, 035, 022, 167, 018, 202, 201, 170, 224, 165, 230, 050, 232, 013, 078, 032, 193, 127, 171, 217, 243, 193, 016, 126, 053, 202, 202, 115, 246, 116, 215, 198, 186, 056, 093, 113, 046, 165, 089, 143, 251, 216, 048, 052, 119, 137, 048, 107, 189, 168, 023, 126, 087, 186, 086, 224, 239, 071, 181, 094, 053, 085, 058, 078, 101, 087, 024, 247, 122, 058, 109, 103, 162, 084, 014, 057, 057, 083, 109, 085, 034, 023, 095, 044, 240, 026, 161, 108, 230, 210, 043, 136, 002, 080, 187, 181, 017, 213, 181, 204, 185, 214, 205, 044, 157, 210, 243, 250, 094, 207, 073, 010, 203, 036, 014, 075, 175, 195, 115, 100, 246, 114, 066, 137, 216, 189, 069, 065, 080, 071, 228, 205, 069, 091, 083, 086, 142, 218, 205, 105, 092, 113, 035, 082, 138, 185, 011, 235, 200, 021, 198, 009, 039, 065, 041, 155, 251, 025, 195, 239, 212, 220, 119, 164, 209, 157, 118, 039, 121, 082, 161, 206, 221, 142, 088, 242, 132, 209, 077, 067, 150, 209, 136, 182, 112, 170, 062, 195, 145, 099, 149, 088, 162, 239, 084, 167, 093, 061, 182, 058, 036, 208, 142, 004, 202, 016, 113, 109, 021, 050, 068, 090, 187, 096, 185, 240, 219, 199, 209, 197, 217, 205, 223, 096, 222, 112, 157, 245, 034, 207, 031, 241, 165, 250, 129, 094, 002, 239, 098, 166, 238, 227, 138, 033, 240, 083, 074, 039, 182, 027, 241, 175, 198, 215, 215, 187, 139, 171, 137, 054, 254, 125, 005, 069, 127, 127, 212, 129, 106, 235, 033, 129, 230, 234, 226, 192, 206, 110, 014, 243, 237, 161, 088, 206, 014, 162, 004, 011, 003, 048, 152, 002, 198, 007, 182, 168, 009, 124, 107, 191, 200, 081, 139, 130, 235, 139, 171, 139, 089, 161, 094, 163, 054, 081, 037, 062, 060, 101, 211, 214, 131, 024, 193, 021, 234, 013, 037, 233, 115, 027, 097, 143, 070, 225, 248, 183, 104, 114, 136, 232, 217, 024, 011, 244, 003, 202, 246, 022, 106, 175, 112, 251, 117, 125, 225, 247, 031, 007, 023, 234, 119, 177, 127, 124, 252, 180, 003, 045, 216, 241, 243, 023, 207, 126, 124, 118, 061, 062, 156, 159, 007, 007, 124, 152, 092, 079, 240, 124, 069, 057, 030, 209, 176, 159, 197, 176, 027, 200, 002, 245, 105, 028, 061, 139, 255, 136, 169, 118, 169, 079, 251, 060, 174, 010, 154, 043, 209, 001, 068, 035, 062, 168, 152, 109, 052, 227, 078, 036, 174, 254, 187, 183, 223, 114, 052, 129, 119, 218, 226, 209, 226, 227, 144, 170, 088, 150, 085, 049, 090, 189, 219, 186, 032, 100, 255, 182, 235, 047, 170, 177, 228, 153, 113, 208, 126, 006, 034, 029, 069, 133, 131, 018, 197, 070, 031, 038, 232, 121, 023, 094, 079, 075, 054, 156, 138, 062, 115, 172, 051, 223, 106, 117, 047, 125, 255, 101, 003, 110, 140, 012, 103, 050, 030, 076, 066, 035, 000, 170, 219, 186, 005, 110, 173, 159, 235, 153, 213, 023, 111, 110, 092, 086, 087, 103, 173, 234, 006, 168, 212, 095, 083, 179, 168, 164, 017, 166, 236, 202, 087, 235, 177, 241, 117, 097, 096, 252, 245, 153, 043, 082, 053, 043, 181, 077, 089, 221, 152, 053, 196, 231, 129, 220, 223, 193, 088, 122, 014, 021, 240, 090, 208, 251, 042, 057, 148, 042, 228, 129, 101, 205, 081, 110, 107, 161, 254, 091, 115, 060, 246, 112, 084, 218, 153, 088, 224, 040, 077, 255, 165, 110, 251, 052, 108, 012, 059, 144, 072, 219, 037, 145, 086, 179, 026, 233, 084, 077, 242, 186, 093, 087, 189, 007, 134, 213, 118, 058, 068, 157, 058, 195, 248, 114, 163, 219, 087, 213, 230, 098, 079, 114, 070, 155, 107, 152, 052, 150, 203, 177, 214, 051, 070, 000, 226, 236, 047, 097, 171, 009, 185, 255, 196, 101, 165, 255, 002, 237, 245, 117, 004, 038, 014, 111, 248, 025, 190, 004, 161, 099, 124, 147, 142, 042, 065, 091, 089, 215, 195, 154, 122, 130, 088, 055, 207, 065, 197, 138, 020, 231, 150, 181, 137, 097, 130, 149, 186, 183, 164, 178, 246, 163, 168, 141, 148, 118, 051, 078, 126, 244, 064, 152, 076, 233, 120, 213, 062, 009, 029, 170, 153, 039, 037, 214, 190, 212, 026, 042, 169, 068, 066, 106, 242, 112, 249, 137, 219, 123, 165, 069, 186, 252, 203, 035, 101, 235, 055, 025, 169, 173, 087, 236, 203, 184, 226, 157, 121, 140, 235, 137, 097, 025, 149, 052, 133, 047, 064, 109, 078, 192, 239, 172, 241, 228, 218, 154, 070, 213, 192, 164, 046, 120, 125, 017, 151, 180, 067, 066, 027, 065, 040, 211, 049, 156, 227, 013, 197, 113, 130, 099, 081, 069, 192, 094, 177, 169, 090, 159, 015, 212, 029, 199, 071, 100, 117, 081, 199, 192, 254, 078, 180, 068, 015, 135, 053, 194, 132, 052, 180, 233, 239, 088, 055, 170, 052, 050, 237, 118, 223, 024, 053, 055, 027, 240, 177, 174, 190, 110, 180, 085, 225, 231, 247, 206, 007, 002, 193, 156, 066, 115, 228, 206, 209, 033, 160, 173, 206, 128, 192, 113, 070, 190, 136, 057, 028, 048, 211, 075, 220, 161, 053, 171, 173, 222, 096, 227, 195, 180, 190, 102, 206, 210, 025, 040, 182, 255, 207, 163, 091, 023, 241, 098, 179, 181, 090, 012, 081, 085, 240, 065, 142, 203, 089, 033, 070, 010, 029, 123, 037, 143, 253, 091, 199, 006, 230, 069, 012, 036, 083, 024, 012, 177, 190, 090, 014, 151, 132, 033, 102, 017, 077, 041, 173, 215, 134, 077, 001, 086, 098, 088, 235, 207, 224, 203, 000, 036, 227, 028, 216, 076, 044, 144, 010, 024, 255, 205, 202, 026, 217, 077, 169, 118, 039, 176, 156, 208, 010, 045, 003, 065, 093, 162, 083, 086, 140, 077, 091, 231, 131, 250, 024, 165, 131, 005, 237, 053, 232, 139, 065, 239, 192, 116, 166, 064, 085, 109, 246, 056, 150, 223, 155, 085, 045, 253, 208, 063, 043, 154, 186, 081, 176, 014, 243, 249, 210, 026, 180, 113, 078, 089, 254, 190, 093, 176, 118, 151, 126, 150, 068, 152, 052, 150, 181, 248, 104, 138, 096, 023, 010, 041, 142, 085, 021, 094, 237, 134, 020, 045, 096, 227, 012, 244, 247, 024, 110, 056, 232, 160, 165, 201, 023, 005, 103, 107, 227, 097, 129, 250, 075, 227, 133, 211, 026, 006, 194, 102, 052, 025, 085, 034, 134, 134, 177, 154, 150, 118, 143, 089, 148, 195, 037, 208, 112, 010, 071, 232, 135, 003, 251, 134, 119, 220, 167, 186, 094, 002, 097, 136, 025, 176, 095, 122, 023, 049, 165, 149, 233, 200, 106, 211, 014, 005, 188, 169, 020, 117, 150, 177, 082, 071, 197, 198, 043, 011, 170, 129, 172, 076, 068, 056, 215, 174, 175, 213, 245, 159, 157, 167, 087, 052, 079, 079, 031, 013, 174, 158, 094, 060, 122, 114, 229, 073, 052, 183, 006, 199, 224, 248, 141, 148, 125, 030, 189, 211, 191, 010, 128, 204, 197, 179, 226, 029, 077, 034, 247, 114, 180, 183, 206, 189, 222, 198, 222, 083, 175, 231, 058, 168, 165, 030, 140, 166, 117, 083, 242, 208, 255, 052, 174, 224, 224, 210, 138, 255, 211, 184, 050, 029, 211, 232, 211, 074, 032, 038, 058, 211, 252, 250, 161, 086, 183, 196, 151, 069, 116, 163, 238, 014, 170, 031, 106, 189, 015, 244, 129, 158, 179, 191, 191, 057, 126, 153, 086, 176, 032, 130, 115, 126, 070, 032, 210, 235, 205, 130, 140, 158, 186, 221, 191, 112, 200, 123, 254, 008, 085, 066, 014, 074, 199, 181, 204, 163, 249, 225, 032, 021, 176, 193, 146, 212, 217, 082, 215, 140, 192, 237, 243, 178, 022, 086, 136, 102, 242, 166, 180, 093, 146, 030, 057, 232, 036, 179, 001, 100, 024, 166, 050, 213, 041, 186, 093, 054, 145, 118, 033, 043, 154, 235, 253, 161, 166, 226, 137, 107, 213, 226, 091, 209, 217, 030, 090, 219, 239, 146, 097, 192, 248, 132, 189, 137, 180, 251, 072, 117, 027, 185, 070, 250, 132, 058, 106, 220, 124, 185, 143, 224, 216, 036, 096, 255, 039, 112, 101, 075, 124, 213, 055, 128, 102, 118, 109, 154, 033, 220, 060, 251, 097, 188, 193, 102, 099, 186, 197, 016, 087, 101, 104, 076, 251, 101, 073, 124, 220, 168, 198, 098, 065, 004, 030, 086, 073, 018, 116, 126, 102, 073, 018, 212, 092, 138, 083, 241, 166, 110, 009, 131, 180, 220, 007, 102, 117, 226, 101, 132, 254, 105, 254, 047, 108, 164, 178, 017, 163, 209, 146, 106, 225, 222, 179, 245, 242, 155, 120, 085, 108, 194, 047, 232, 016, 225, 107, 197, 182, 013, 250, 165, 101, 204, 117, 040, 129, 154, 105, 070, 051, 035, 075, 044, 218, 067, 194, 253, 042, 089, 090, 082, 202, 027, 195, 145, 068, 219, 230, 156, 250, 074, 144, 157, 145, 249, 198, 157, 150, 111, 220, 207, 149, 241, 004, 027, 218, 195, 013, 107, 095, 190, 040, 149, 192, 173, 117, 191, 118, 136, 007, 200, 021, 183, 136, 175, 190, 136, 107, 066, 175, 074, 223, 053, 097, 107, 069, 087, 218, 115, 117, 203, 183, 063, 215, 063, 025, 038, 227, 185, 157, 181, 097, 221, 191, 198, 241, 040, 113, 230, 254, 063, 209, 053, 194, 175, 249, 118, 255, 041, 075, 100, 177, 021, 043, 081, 230, 208, 081, 017, 214, 254, 219, 253, 108, 092, 023, 212, 062, 212, 027, 182, 194, 133, 120, 186, 111, 213, 186, 248, 223, 111, 174, 018, 024, 029, 077, 055, 130, 096, 025, 172, 020, 179, 162, 174, 070, 044, 114, 031, 009, 013, 125, 014, 086, 242, 080, 015, 169, 094, 059, 161, 099, 014, 215, 108, 162, 068, 186, 211, 091, 061, 089, 042, 073, 067, 227, 184, 098, 087, 243, 167, 224, 009, 033, 238, 137, 123, 002, 157, 199, 004, 176, 045, 157, 173, 178, 172, 169, 229, 160, 117, 092, 156, 153, 048, 152, 157, 065, 024, 107, 071, 083, 240, 245, 024, 135, 034, 232, 097, 234, 172, 213, 005, 025, 019, 135, 142, 119, 213, 163, 194, 057, 252, 007, 177, 130, 064, 160, 222, 222, 108, 204, 144, 018, 166, 207, 092, 159, 137, 053, 111, 184, 142, 169, 106, 037, 242, 177, 061, 174, 045, 161, 177, 103, 130, 204, 241, 128, 209, 022, 202, 186, 243, 058, 118, 226, 001, 214, 188, 139, 056, 116, 067, 028, 072, 122, 195, 011, 001, 229, 137, 221, 060, 157, 044, 030, 027, 159, 009, 241, 187, 124, 038, 076, 068, 197, 189, 234, 017, 001, 241, 142, 183, 111, 025, 208, 134, 153, 214, 188, 213, 243, 147, 202, 252, 212, 005, 011, 126, 013, 190, 018, 134, 047, 085, 225, 033, 017, 148, 254, 210, 113, 008, 127, 076, 244, 134, 168, 033, 220, 248, 157, 200, 083, 207, 107, 053, 168, 154, 185, 147, 031, 091, 217, 222, 059, 054, 102, 069, 028, 232, 198, 056, 173, 006, 156, 037, 076, 004, 163, 159, 250, 200, 024, 037, 104, 039, 150, 186, 047, 218, 121, 133, 009, 136, 172, 098, 227, 116, 151, 111, 017, 100, 084, 063, 174, 067, 079, 158, 060, 131, 101, 241, 073, 063, 122, 202, 197, 004, 161, 190, 139, 050, 095, 159, 049, 242, 241, 024, 007, 121, 102, 002, 016, 120, 212, 115, 038, 163, 229, 098, 190, 022, 003, 051, 110, 006, 039, 230, 208, 120, 236, 162, 205, 013, 104, 072, 092, 079, 036, 082, 154, 052, 130, 157, 239, 220, 241, 181, 042, 027, 141, 101, 246, 254, 020, 002, 027, 066, 057, 126, 090, 026, 098, 231, 176, 083, 206, 247, 142, 039, 048, 153, 229, 150, 104, 150, 095, 197, 234, 175, 113, 244, 000, 224, 160, 017, 195, 061, 164, 167, 062, 253, 238, 197, 175, 230, 197, 113, 087, 248, 181, 043, 229, 161, 014, 215, 029, 133, 016, 204, 247, 205, 060, 083, 034, 123, 214, 096, 143, 197, 112, 028, 200, 129, 034, 075, 047, 137, 086, 250, 171, 181, 155, 224, 228, 183, 228, 141, 190, 113, 068, 114, 136, 053, 245, 215, 152, 230, 208, 022, 097, 041, 184, 238, 140, 246, 196, 040, 018, 006, 164, 124, 021, 071, 244, 143, 200, 102, 034, 239, 037, 188, 233, 025, 255, 021, 087, 151, 209, 007, 151, 031, 156, 137, 139, 074, 060, 137, 103, 079, 060, 094, 092, 121, 213, 222, 215, 212, 069, 217, 074, 250, 043, 230, 199, 043, 097, 081, 015, 135, 202, 071, 067, 143, 004, 182, 060, 237, 190, 215, 091, 066, 016, 062, 246, 033, 007, 171, 102, 127, 106, 166, 251, 095, 197, 118, 002, 002, 197, 195, 068, 168, 211, 148, 253, 211, 124, 011, 049, 176, 248, 234, 188, 080, 223, 197, 053, 047, 151, 218, 179, 229, 168, 179, 121, 019, 136, 123, 203, 158, 241, 110, 249, 125, 220, 112, 107, 104, 061, 076, 018, 105, 171, 037, 132, 083, 113, 072, 048, 131, 014, 122, 044, 078, 025, 065, 136, 150, 143, 081, 002, 157, 117, 248, 097, 022, 168, 138, 181, 199, 104, 183, 180, 147, 027, 021, 089, 213, 144, 163, 250, 027, 092, 205, 213, 102, 113, 216, 234, 028, 195, 242, 117, 039, 092, 211, 221, 156, 242, 158, 065, 120, 252, 070, 218, 015, 030, 110, 234, 078, 084, 167, 139, 117, 188, 015, 225, 172, 117, 184, 038, 228, 092, 208, 241, 013, 031, 148, 139, 190, 126, 139, 188, 075, 122, 167, 141, 101, 074, 234, 239, 208, 016, 216, 237, 062, 071, 233, 168, 211, 113, 170, 229, 079, 202, 124, 128, 091, 103, 120, 136, 034, 218, 109, 081, 108, 034, 207, 241, 182, 234, 081, 166, 170, 003, 178, 246, 034, 232, 012, 187, 031, 102, 142, 240, 037, 242, 084, 235, 113, 058, 087, 045, 123, 122, 174, 138, 250, 052, 212, 188, 217, 254, 215, 230, 205, 187, 252, 217, 090, 071, 176, 151, 218, 073, 236, 057, 242, 192, 023, 108, 195, 079, 046, 070, 233, 248, 158, 035, 052, 089, 161, 101, 111, 032, 237, 164, 065, 188, 100, 103, 180, 145, 087, 025, 140, 249, 012, 169, 137, 243, 253, 155, 245, 239, 159, 158, 072, 250, 153, 061, 226, 218, 212, 210, 153, 250, 066, 017, 073, 177, 040, 112, 085, 241, 069, 145, 101, 249, 234, 059, 214, 081, 111, 011, 034, 169, 233, 152, 110, 023, 234, 181, 196, 112, 218, 110, 252, 160, 043, 120, 095, 025, 130, 234, 077, 241, 038, 095, 124, 195, 083, 195, 158, 099, 223, 087, 036, 213, 069, 190, 055, 211, 247, 158, 252, 009, 206, 086, 233, 205, 191, 209, 202, 172, 094, 010, 234, 231, 239, 043, 052, 175, 132, 090, 187, 213, 059, 146, 205, 235, 091, 118, 238, 178, 178, 186, 069, 160, 026, 027, 174, 197, 107, 177, 000, 095, 221, 049, 177, 243, 213, 066, 021, 171, 155, 021, 175, 242, 118, 079, 197, 241, 253, 126, 109, 156, 021, 015, 028, 040, 197, 051, 003, 232, 127, 106, 240, 254, 248, 242, 063, 061, 088, 181, 068, 115, 004, 178, 074, 163, 025, 007, 178, 195, 089, 248, 124, 189, 220, 016, 212, 102, 047, 117, 104, 170, 069, 212, 252, 236, 179, 217, 157, 055, 248, 079, 156, 039, 254, 066, 140, 209, 224, 070, 126, 030, 121, 079, 054, 188, 031, 205, 215, 165, 157, 102, 058, 192, 189, 143, 156, 068, 233, 008, 127, 162, 124, 252, 102, 231, 106, 089, 046, 105, 228, 113, 095, 211, 106, 097, 039, 131, 173, 194, 249, 166, 110, 105, 067, 252, 129, 011, 062, 117, 091, 091, 156, 063, 178, 088, 255, 047, 185, 152, 190, 244, 108, 239, 220, 233, 184, 173, 250, 143, 190, 244, 236, 164, 233, 047, 003, 120, 148, 166, 245, 228, 032, 018, 140, 139, 017, 055, 189, 177, 132, 183, 129, 187, 064, 092, 055, 096, 181, 234, 054, 171, 132, 094, 221, 235, 072, 200, 008, 008, 091, 025, 233, 160, 090, 054, 182, 248, 129, 056, 170, 157, 111, 221, 128, 078, 217, 091, 070, 173, 104, 013, 019, 090, 103, 134, 091, 250, 151, 193, 115, 097, 198, 015, 123, 121, 210, 094, 013, 181, 227, 197, 027, 215, 001, 024, 209, 017, 006, 065, 098, 210, 158, 175, 023, 139, 120, 179, 035, 248, 218, 229, 184, 130, 006, 206, 189, 149, 206, 181, 093, 174, 194, 129, 230, 109, 089, 135, 093, 234, 119, 044, 133, 093, 052, 119, 244, 092, 135, 088, 250, 124, 193, 039, 005, 143, 218, 169, 217, 025, 248, 045, 088, 153, 119, 204, 100, 163, 046, 154, 250, 101, 205, 081, 216, 209, 250, 253, 254, 033, 086, 047, 099, 245, 035, 123, 129, 162, 109, 119, 216, 162, 196, 033, 097, 207, 220, 007, 028, 083, 193, 163, 139, 097, 115, 205, 071, 254, 015, 113, 212, 100, 044, 146, 218, 157, 178, 099, 082, 110, 073, 073, 240, 015, 235, 141, 196, 104, 132, 091, 193, 064, 008, 231, 042, 072, 065, 103, 252, 101, 157, 164, 170, 073, 165, 013, 005, 085, 122, 178, 039, 098, 244, 007, 045, 157, 077, 071, 092, 235, 247, 090, 079, 143, 239, 183, 168, 214, 195, 001, 193, 074, 053, 083, 074, 140, 033, 156, 104, 116, 187, 214, 026, 099, 086, 081, 171, 109, 010, 186, 217, 201, 144, 227, 077, 091, 248, 161, 206, 162, 095, 063, 163, 096, 020, 241, 157, 227, 030, 239, 091, 163, 048, 039, 097, 092, 230, 026, 073, 193, 154, 112, 089, 172, 036, 236, 220, 020, 047, 241, 027, 121, 041, 191, 059, 095, 077, 185, 104, 134, 017, 234, 058, 204, 183, 204, 045, 147, 043, 167, 212, 052, 040, 205, 068, 103, 163, 089, 056, 235, 121, 132, 228, 194, 191, 089, 235, 101, 131, 160, 221, 053, 117, 239, 014, 220, 108, 255, 243, 117, 113, 022, 064, 014, 199, 025, 091, 081, 205, 057, 204, 044, 205, 239, 156, 221, 045, 184, 179, 215, 249, 177, 062, 125, 011, 057, 005, 226, 254, 246, 126, 037, 166, 009, 212, 032, 108, 206, 224, 171, 142, 019, 177, 129, 228, 049, 170, 014, 128, 191, 209, 081, 044, 105, 222, 148, 086, 155, 072, 018, 246, 044, 159, 140, 188, 065, 190, 244, 066, 076, 239, 092, 214, 020, 199, 077, 015, 222, 217, 077, 129, 204, 173, 025, 001, 078, 026, 083, 075, 100, 020, 014, 079, 207, 061, 234, 255, 094, 081, 250, 120, 032, 200, 108, 033, 021, 098, 063, 016, 159, 082, 090, 072, 205, 012, 036, 178, 250, 230, 041, 074, 130, 019, 126, 191, 142, 204, 190, 252, 068, 251, 056, 094, 108, 230, 241, 181, 063, 254, 045, 152, 060, 190, 134, 174, 203, 207, 244, 081, 019, 218, 215, 187, 199, 080, 133, 145, 068, 164, 253, 194, 027, 031, 232, 227, 192, 232, 145, 120, 155, 243, 116, 156, 199, 147, 160, 015, 159, 225, 191, 158, 224, 128, 250, 165, 091, 255, 127, 016, 039, 107, 073, 010, 175, 164, 084, 095, 021, 187, 034, 041, 022, 224, 001, 188, 057, 083, 137, 158, 050, 168, 207, 240, 185, 234, 017, 021, 166, 161, 018, 123, 255, 018, 029, 036, 252, 136, 083, 008, 075, 242, 115, 174, 015, 100, 014, 106, 245, 207, 056, 026, 123, 066, 143, 082, 179, 223, 209, 063, 162, 091, 233, 239, 114, 007, 183, 188, 167, 066, 082, 104, 048, 180, 139, 144, 036, 198, 166, 152, 149, 195, 146, 154, 057, 049, 095, 211, 199, 219, 103, 123, 255, 018, 036, 200, 223, 233, 204, 215, 178, 162, 094, 172, 237, 148, 007, 224, 035, 255, 025, 087, 013, 120, 083, 113, 092, 020, 083, 194, 056, 157, 244, 018, 085, 175, 190, 100, 178, 211, 164, 225, 098, 149, 149, 080, 198, 184, 254, 184, 228, 173, 163, 171, 158, 095, 205, 248, 018, 008, 247, 038, 179, 009, 187, 211, 151, 061, 138, 200, 034, 142, 214, 161, 183, 094, 100, 150, 195, 103, 087, 209, 149, 163, 065, 033, 150, 028, 149, 032, 152, 180, 076, 059, 098, 159, 053, 142, 021, 226, 234, 037, 096, 109, 037, 161, 219, 253, 217, 103, 119, 050, 239, 104, 085, 125, 019, 251, 089, 169, 082, 026, 004, 236, 003, 015, 229, 020, 046, 063, 221, 118, 059, 185, 227, 048, 170, 086, 075, 062, 074, 067, 017, 091, 100, 142, 204, 002, 023, 186, 152, 172, 025, 095, 073, 159, 152, 146, 196, 105, 165, 049, 000, 175, 229, 107, 203, 012, 036, 035, 061, 077, 094, 040, 117, 185, 023, 173, 118, 253, 178, 164, 106, 101, 246, 171, 113, 085, 085, 222, 202, 141, 190, 137, 247, 140, 123, 253, 075, 149, 209, 081, 125, 142, 096, 115, 151, 065, 208, 243, 051, 241, 092, 133, 016, 016, 097, 082, 214, 153, 039, 173, 030, 107, 035, 232, 025, 250, 217, 072, 115, 147, 008, 003, 039, 148, 159, 023, 140, 062, 010, 061, 198, 249, 130, 179, 006, 028, 141, 252, 114, 248, 209, 213, 116, 056, 237, 069, 079, 002, 077, 126, 104, 061, 021, 127, 214, 179, 033, 051, 210, 222, 079, 236, 127, 234, 018, 006, 035, 042, 131, 030, 189, 174, 212, 228, 061, 047, 195, 107, 104, 146, 197, 171, 148, 049, 117, 119, 154, 249, 117, 071, 057, 059, 066, 182, 163, 131, 186, 024, 129, 132, 211, 139, 182, 154, 203, 143, 157, 070, 175, 223, 089, 179, 153, 248, 153, 227, 200, 184, 182, 074, 200, 025, 185, 051, 022, 187, 225, 081, 194, 184, 070, 121, 069, 230, 172, 114, 152, 105, 130, 163, 146, 133, 170, 004, 033, 177, 089, 060, 232, 137, 076, 089, 066, 114, 121, 005, 039, 078, 218, 107, 036, 099, 156, 060, 122, 041, 186, 007, 116, 022, 083, 178, 147, 170, 205, 168, 069, 164, 227, 030, 124, 185, 213, 120, 033, 014, 013, 103, 164, 239, 244, 199, 176, 207, 008, 142, 153, 243, 141, 071, 089, 065, 030, 057, 244, 058, 188, 131, 092, 218, 160, 111, 061, 003, 108, 032, 100, 218, 064, 011, 222, 093, 002, 062, 239, 220, 160, 133, 187, 157, 054, 099, 055, 082, 157, 234, 001, 102, 148, 238, 044, 025, 200, 099, 245, 116, 102, 043, 120, 212, 097, 083, 189, 129, 023, 194, 212, 232, 168, 108, 008, 148, 240, 033, 094, 021, 075, 182, 233, 250, 146, 206, 002, 126, 096, 151, 029, 236, 156, 149, 014, 148, 101, 249, 058, 045, 022, 139, 239, 116, 055, 240, 186, 200, 223, 252, 101, 187, 126, 109, 158, 095, 114, 080, 036, 113, 229, 106, 079, 017, 122, 067, 240, 223, 047, 236, 219, 186, 172, 064, 008, 116, 126, 160, 115, 115, 181, 195, 035, 129, 202, 250, 053, 063, 253, 254, 037, 060, 102, 240, 019, 002, 243, 192, 218, 154, 122, 253, 061, 091, 243, 063, 136, 188, 203, 011, 075, 233, 213, 200, 051, 079, 052, 169, 188, 034, 242, 002, 243, 186, 183, 139, 054, 189, 108, 081, 176, 254, 176, 166, 145, 240, 095, 181, 247, 216, 136, 220, 172, 068, 145, 067, 180, 086, 034, 103, 022, 150, 238, 194, 082, 008, 128, 114, 071, 199, 115, 027, 140, 198, 188, 071, 116, 020, 206, 009, 050, 230, 162, 244, 104, 022, 152, 067, 209, 056, 175, 243, 073, 073, 221, 164, 006, 026, 177, 023, 104, 249, 189, 130, 031, 045, 233, 076, 080, 060, 099, 001, 056, 110, 216, 104, 100, 193, 040, 015, 011, 170, 016, 189, 153, 090, 111, 169, 078, 176, 083, 118, 117, 154, 071, 058, 212, 077, 138, 203, 155, 156, 240, 037, 043, 086, 075, 148, 158, 028, 081, 188, 188, 021, 131, 008, 194, 178, 241, 133, 031, 248, 007, 141, 172, 076, 146, 174, 043, 237, 049, 249, 215, 140, 189, 051, 047, 099, 239, 004, 077, 249, 031, 112, 190, 028, 079, 151, 172, 103, 107, 157, 159, 149, 242, 063, 143, 029, 110, 193, 048, 129, 070, 061, 207, 183, 048, 108, 082, 029, 031, 051, 177, 171, 205, 004, 144, 118, 202, 142, 249, 246, 172, 228, 159, 225, 008, 099, 099, 123, 046, 111, 226, 002, 223, 224, 026, 136, 097, 169, 005, 038, 078, 047, 178, 085, 142, 253, 223, 090, 222, 234, 098, 034, 202, 172, 089, 195, 075, 088, 061, 151, 203, 047, 049, 104, 053, 018, 131, 162, 013, 157, 146, 219, 101, 188, 208, 147, 207, 065, 091, 031, 197, 156, 233, 017, 071, 034, 022, 122, 130, 102, 149, 061, 228, 058, 024, 105, 170, 085, 232, 047, 225, 096, 231, 243, 098, 005, 065, 125, 078, 208, 066, 243, 031, 178, 067, 250, 242, 010, 105, 236, 137, 220, 148, 040, 061, 193, 222, 147, 198, 085, 143, 051, 182, 168, 142, 149, 092, 195, 135, 116, 244, 139, 198, 171, 205, 240, 081, 004, 121, 098, 068, 237, 156, 010, 163, 239, 049, 212, 127, 196, 170, 073, 166, 235, 227, 037, 227, 080, 150, 230, 217, 026, 233, 236, 090, 250, 192, 011, 010, 183, 073, 124, 178, 088, 082, 033, 145, 244, 145, 070, 202, 153, 250, 111, 028, 056, 080, 052, 012, 194, 203, 064, 219, 110, 106, 196, 102, 000, 130, 167, 198, 074, 223, 091, 176, 182, 238, 202, 207, 122, 110, 064, 074, 085, 185, 165, 081, 141, 121, 210, 042, 102, 026, 213, 232, 215, 128, 085, 241, 070, 253, 203, 193, 099, 103, 161, 133, 107, 232, 063, 026, 208, 129, 226, 133, 009, 035, 126, 207, 107, 204, 144, 061, 053, 116, 157, 042, 171, 049, 108, 108, 101, 083, 096, 051, 211, 206, 003, 173, 053, 242, 152, 199, 241, 237, 173, 066, 111, 112, 121, 249, 056, 033, 198, 004, 013, 016, 206, 128, 123, 076, 221, 053, 130, 063, 251, 228, 121, 195, 084, 007, 116, 087, 126, 114, 021, 013, 180, 192, 027, 092, 165, 167, 125, 250, 067, 031, 120, 106, 239, 176, 127, 018, 165, 066, 185, 242, 172, 104, 253, 112, 136, 248, 134, 078, 186, 180, 100, 136, 233, 132, 029, 117, 118, 178, 114, 150, 124, 211, 153, 232, 167, 210, 201, 080, 165, 057, 090, 204, 041, 028, 059, 245, 114, 125, 151, 106, 087, 209, 149, 220, 017, 059, 009, 255, 156, 013, 161, 181, 106, 093, 221, 068, 128, 249, 193, 178, 095, 197, 010, 199, 225, 185, 225, 194, 094, 198, 106, 028, 027, 202, 078, 130, 179, 149, 049, 074, 091, 250, 000, 158, 184, 173, 011, 044, 154, 061, 209, 003, 223, 001, 012, 033, 015, 202, 186, 060, 054, 094, 126, 167, 224, 069, 236, 027, 062, 005, 050, 038, 200, 047, 229, 132, 044, 038, 156, 238, 207, 101, 128, 255, 081, 214, 025, 094, 030, 091, 182, 238, 187, 107, 057, 098, 035, 009, 245, 227, 142, 094, 219, 151, 139, 072, 143, 224, 203, 136, 242, 232, 081, 139, 247, 052, 073, 218, 188, 136, 182, 216, 041, 238, 001, 063, 137, 250, 092, 009, 252, 174, 034, 042, 095, 075, 195, 155, 100, 212, 226, 099, 060, 117, 252, 250, 132, 240, 015, 245, 145, 182, 038, 200, 169, 230, 159, 198, 217, 004, 213, 227, 086, 250, 112, 160, 191, 231, 079, 248, 247, 210, 185, 026, 060, 170, 111, 173, 150, 068, 005, 057, 112, 207, 112, 090, 069, 089, 018, 212, 085, 215, 234, 199, 211, 105, 085, 146, 154, 024, 008, 003, 001, 075, 002, 239, 234, 078, 116, 112, 227, 219, 076, 008, 108, 039, 158, 119, 174, 089, 187, 233, 056, 033, 046, 108, 098, 145, 029, 222, 132, 156, 048, 067, 153, 026, 090, 214, 018, 030, 080, 043, 046, 195, 219, 177, 030, 177, 137, 154, 119, 100, 059, 183, 102, 164, 098, 184, 059, 152, 019, 205, 216, 004, 016, 098, 208, 197, 185, 032, 148, 240, 230, 069, 214, 118, 063, 165, 243, 064, 255, 110, 061, 155, 045, 218, 180, 244, 008, 073, 175, 161, 059, 225, 134, 085, 214, 129, 149, 209, 176, 175, 021, 238, 208, 128, 121, 174, 155, 124, 253, 044, 141, 140, 086, 242, 107, 202, 153, 087, 041, 202, 081, 105, 075, 001, 199, 172, 194, 074, 154, 011, 168, 252, 053, 037, 148, 158, 055, 250, 056, 114, 157, 124, 068, 232, 255, 248, 058, 207, 087, 209, 044, 081, 110, 190, 170, 135, 014, 074, 068, 185, 022, 103, 209, 112, 146, 043, 035, 096, 223, 230, 202, 058, 063, 139, 082, 099, 028, 183, 195, 117, 100, 046, 246, 165, 120, 046, 099, 148, 136, 125, 158, 232, 077, 069, 137, 241, 242, 128, 192, 129, 090, 087, 238, 181, 060, 112, 016, 070, 093, 221, 042, 139, 050, 121, 228, 088, 131, 211, 026, 181, 151, 150, 212, 030, 017, 088, 247, 219, 166, 178, 140, 140, 114, 035, 240, 111, 187, 091, 058, 093, 195, 025, 008, 207, 077, 252, 087, 043, 072, 186, 101, 074, 055, 159, 054, 253, 168, 182, 247, 171, 010, 032, 232, 171, 246, 247, 053, 230, 078, 064, 063, 187, 023, 222, 071, 251, 150, 091, 211, 148, 068, 102, 206, 198, 206, 092, 078, 140, 202, 092, 189, 224, 227, 088, 093, 170, 065, 123, 154, 113, 075, 199, 181, 026, 149, 059, 154, 095, 223, 204, 234, 121, 057, 251, 001, 157, 166, 229, 091, 181, 190, 221, 062, 223, 104, 253, 037, 247, 083, 105, 107, 033, 014, 035, 077, 253, 162, 028, 164, 067, 200, 210, 164, 242, 223, 119, 078, 170, 077, 231, 162, 132, 186, 026, 208, 235, 000, 169, 155, 166, 220, 250, 162, 007, 235, 193, 169, 070, 247, 232, 181, 177, 081, 023, 152, 215, 066, 159, 093, 013, 112, 009, 158, 203, 159, 199, 177, 044, 153, 009, 016, 161, 051, 011, 195, 173, 211, 070, 213, 172, 161, 097, 192, 252, 088, 071, 001, 224, 239, 032, 038, 224, 113, 071, 228, 195, 224, 054, 070, 073, 200, 206, 087, 106, 029, 036, 044, 252, 134, 103, 213, 214, 223, 248, 130, 080, 144, 039, 187, 094, 235, 164, 195, 028, 232, 210, 226, 123, 182, 060, 004, 090, 071, 193, 044, 232, 235, 208, 162, 215, 202, 080, 056, 173, 023, 243, 030, 100, 039, 084, 149, 197, 180, 174, 213, 162, 150, 207, 076, 063, 060, 212, 199, 092, 027, 008, 054, 033, 127, 168, 088, 083, 180, 117, 208, 120, 129, 097, 044, 243, 000, 202, 038, 110, 183, 129, 160, 121, 126, 141, 131, 187, 153, 214, 255, 248, 156, 197, 118, 233, 154, 086, 236, 049, 063, 126, 255, 101, 112, 241, 196, 113, 003, 230, 113, 089, 015, 077, 077, 223, 068, 013, 144, 084, 118, 129, 160, 235, 195, 145, 076, 009, 099, 038, 234, 038, 145, 032, 044, 114, 084, 028, 128, 197, 015, 192, 221, 136, 189, 114, 075, 105, 101, 128, 232, 071, 078, 204, 149, 069, 226, 210, 044, 052, 083, 063, 022, 075, 120, 174, 113, 015, 136, 185, 049, 073, 166, 083, 123, 158, 068, 218, 043, 083, 041, 005, 091, 038, 213, 008, 217, 015, 090, 007, 133, 166, 033, 143, 036, 098, 106, 034, 210, 067, 162, 038, 242, 097, 222, 139, 158, 156, 039, 065, 026, 253, 052, 206, 039, 042, 027, 027, 065, 095, 047, 157, 068, 244, 102, 069, 118, 244, 106, 003, 150, 036, 028, 241, 213, 144, 225, 153, 190, 136, 138, 043, 122, 101, 043, 043, 140, 115, 109, 110, 252, 187, 164, 191, 199, 169, 003, 183, 184, 009, 187, 003, 014, 140, 027, 077, 055, 201, 123, 076, 164, 040, 046, 054, 137, 156, 176, 126, 051, 103, 044, 232, 100, 059, 002, 049, 207, 017, 220, 147, 226, 172, 183, 066, 051, 167, 011, 235, 164, 065, 160, 084, 172, 215, 248, 196, 095, 131, 102, 217, 088, 038, 228, 174, 034, 127, 249, 217, 231, 144, 073, 101, 192, 121, 111, 250, 006, 075, 233, 005, 196, 081, 240, 018, 210, 033, 004, 102, 221, 141, 179, 206, 185, 180, 220, 034, 138, 230, 180, 083, 056, 017, 126, 154, 202, 023, 054, 172, 152, 235, 056, 224, 211, 098, 139, 139, 187, 242, 197, 013, 233, 093, 150, 033, 198, 025, 071, 191, 042, 191, 244, 122, 106, 217, 143, 023, 175, 227, 183, 059, 023, 068, 218, 190, 149, 133, 206, 207, 149, 009, 021, 175, 187, 170, 039, 248, 112, 112, 251, 192, 084, 006, 049, 252, 117, 197, 106, 223, 048, 231, 080, 074, 035, 078, 074, 056, 116, 214, 080, 099, 230, 008, 238, 148, 166, 011, 058, 096, 198, 027, 251, 172, 202, 199, 095, 156, 231, 095, 039, 098, 081, 082, 227, 203, 213, 109, 100, 131, 062, 223, 140, 156, 201, 119, 239, 060, 014, 007, 104, 046, 150, 247, 015, 225, 141, 210, 252, 013, 138, 221, 154, 171, 001, 151, 131, 022, 177, 157, 040, 020, 075, 086, 014, 160, 254, 045, 124, 230, 126, 029, 191, 165, 125, 070, 165, 116, 029, 132, 097, 171, 245, 143, 054, 154, 133, 012, 055, 229, 245, 073, 133, 161, 098, 043, 060, 051, 052, 106, 164, 028, 103, 100, 111, 194, 154, 209, 219, 033, 201, 109, 091, 047, 167, 116, 089, 043, 020, 059, 157, 169, 116, 083, 006, 110, 202, 175, 110, 202, 147, 201, 049, 112, 012, 121, 217, 044, 009, 230, 213, 025, 205, 126, 034, 082, 184, 092, 108, 111, 244, 253, 035, 039, 073, 040, 105, 065, 095, 152, 197, 156, 163, 025, 249, 119, 035, 140, 037, 135, 152, 147, 247, 001, 151, 147, 103, 068, 042, 056, 028, 058, 091, 199, 063, 221, 022, 010, 179, 096, 239, 138, 213, 125, 062, 188, 131, 127, 164, 053, 220, 237, 109, 187, 221, 045, 115, 045, 037, 009, 159, 233, 240, 015, 055, 081, 169, 057, 094, 143, 239, 176, 014, 002, 103, 141, 125, 007, 072, 234, 176, 016, 240, 252, 155, 117, 186, 017, 075, 170, 135, 237, 200, 044, 004, 205, 003, 204, 069, 017, 010, 065, 190, 004, 097, 219, 054, 087, 112, 253, 011, 017, 152, 201, 022, 117, 238, 002, 117, 055, 194, 017, 098, 040, 243, 101, 063, 131, 134, 176, 235, 181, 001, 169, 134, 080, 087, 205, 116, 033, 067, 026, 113, 041, 044, 106, 049, 145, 202, 206, 214, 129, 203, 226, 172, 033, 094, 059, 058, 043, 185, 014, 102, 017, 161, 217, 187, 017, 230, 050, 188, 036, 162, 124, 073, 040, 024, 041, 080, 177, 192, 071, 008, 036, 153, 146, 187, 099, 091, 076, 144, 208, 230, 139, 254, 117, 238, 071, 008, 191, 152, 173, 141, 055, 156, 016, 021, 203, 243, 077, 245, 096, 017, 116, 090, 026, 192, 199, 130, 151, 093, 193, 101, 202, 204, 158, 064, 019, 238, 072, 085, 201, 020, 078, 229, 002, 132, 035, 025, 073, 098, 004, 206, 149, 182, 144, 113, 052, 142, 238, 079, 077, 124, 016, 100, 168, 009, 053, 051, 145, 098, 010, 111, 205, 130, 076, 068, 148, 155, 105, 091, 053, 136, 027, 109, 217, 108, 082, 118, 115, 026, 072, 111, 225, 054, 077, 090, 165, 126, 193, 199, 118, 148, 107, 008, 068, 135, 163, 220, 137, 120, 214, 060, 070, 248, 092, 186, 227, 016, 002, 034, 215, 049, 060, 038, 075, 110, 095, 228, 211, 124, 187, 133, 135, 204, 150, 125, 173, 059, 085, 048, 065, 115, 228, 136, 049, 101, 034, 118, 102, 096, 130, 171, 013, 205, 193, 153, 068, 115, 066, 180, 160, 013, 136, 175, 112, 174, 025, 111, 100, 009, 065, 035, 244, 110, 044, 189, 127, 014, 127, 157, 081, 122, 081, 126, 065, 004, 247, 105, 052, 056, 207, 248, 178, 185, 136, 110, 228, 156, 045, 045, 061, 052, 023, 110, 190, 019, 243, 013, 253, 010, 191, 180, 057, 156, 211, 222, 218, 023, 211, 183, 208, 187, 167, 201, 024, 223, 208, 210, 099, 065, 006, 087, 180, 057, 138, 081, 026, 210, 201, 182, 205, 119, 235, 197, 171, 220, 102, 153, 176, 243, 169, 035, 097, 120, 246, 143, 189, 044, 008, 032, 196, 023, 120, 172, 157, 074, 187, 158, 014, 168, 211, 196, 085, 056, 223, 058, 151, 234, 065, 219, 018, 126, 198, 036, 030, 028, 062, 011, 177, 023, 054, 248, 201, 035, 002, 060, 024, 079, 093, 090, 247, 167, 200, 119, 097, 098, 063, 126, 039, 044, 075, 152, 042, 059, 103, 161, 157, 085, 051, 081, 097, 106, 231, 076, 201, 084, 132, 112, 199, 046, 182, 110, 120, 175, 196, 137, 041, 221, 172, 114, 026, 013, 250, 006, 156, 209, 142, 025, 100, 121, 236, 087, 070, 192, 004, 143, 078, 144, 001, 216, 009, 182, 075, 162, 227, 188, 018, 037, 197, 158, 025, 157, 006, 141, 184, 020, 118, 058, 201, 168, 182, 134, 033, 227, 075, 027, 090, 142, 189, 228, 176, 001, 037, 161, 092, 107, 209, 098, 215, 055, 149, 245, 029, 056, 078, 055, 252, 230, 018, 015, 020, 246, 099, 115, 097, 021, 161, 161, 032, 196, 119, 032, 229, 202, 103, 205, 185, 005, 172, 059, 199, 139, 204, 189, 032, 204, 113, 219, 058, 035, 065, 149, 166, 171, 236, 040, 075, 223, 221, 168, 088, 153, 226, 150, 204, 171, 120, 023, 200, 216, 067, 164, 056, 029, 115, 041, 177, 027, 225, 092, 116, 203, 156, 016, 112, 214, 136, 189, 002, 190, 121, 171, 075, 242, 197, 070, 102, 221, 095, 179, 209, 213, 173, 090, 037, 136, 013, 081, 105, 200, 140, 129, 121, 101, 004, 018, 043, 095, 077, 248, 035, 046, 066, 044, 001, 180, 147, 182, 190, 005, 231, 066, 025, 216, 199, 149, 039, 017, 043, 220, 155, 208, 237, 218, 145, 117, 037, 077, 220, 123, 211, 150, 121, 015, 228, 024, 209, 095, 241, 108, 198, 149, 174, 151, 027, 224, 148, 160, 063, 141, 139, 133, 201, 129, 103, 139, 127, 244, 055, 121, 131, 228, 231, 153, 185, 119, 045, 221, 141, 220, 037, 234, 193, 208, 223, 225, 003, 017, 224, 225, 248, 148, 075, 161, 190, 179, 037, 056, 205, 076, 221, 047, 068, 255, 009, 179, 168, 254, 110, 052, 036, 020, 027, 089, 076, 142, 074, 087, 094, 147, 044, 086, 230, 055, 134, 063, 013, 068, 254, 136, 132, 003, 008, 099, 034, 061, 077, 000, 177, 097, 221, 152, 039, 174, 251, 088, 145, 096, 197, 202, 101, 036, 008, 175, 087, 095, 057, 002, 073, 245, 019, 081, 198, 018, 015, 041, 017, 139, 080, 013, 128, 225, 120, 157, 076, 202, 247, 090, 191, 147, 081, 021, 255, 155, 058, 136, 061, 175, 038, 240, 142, 054, 022, 082, 004, 253, 068, 251, 183, 202, 079, 225, 111, 183, 233, 213, 106, 228, 034, 073, 170, 251, 193, 172, 118, 008, 189, 026, 248, 081, 017, 179, 117, 103, 010, 137, 131, 046, 049, 090, 108, 112, 166, 100, 077, 116, 236, 153, 207, 075, 180, 066, 223, 143, 246, 106, 202, 226, 191, 136, 129, 120, 061, 157, 142, 046, 195, 242, 190, 213, 090, 074, 091, 137, 085, 249, 024, 150, 143, 056, 096, 133, 047, 198, 112, 119, 035, 231, 121, 092, 230, 066, 180, 037, 251, 189, 020, 013, 138, 008, 038, 147, 061, 129, 011, 029, 126, 208, 097, 051, 124, 253, 026, 009, 139, 069, 076, 232, 034, 131, 022, 152, 158, 020, 085, 062, 186, 199, 106, 013, 089, 080, 153, 000, 021, 175, 023, 089, 041, 189, 066, 101, 186, 201, 085, 063, 203, 133, 063, 098, 014, 081, 127, 039, 216, 200, 142, 085, 057, 249, 052, 038, 166, 104, 125, 218, 235, 026, 239, 021, 109, 098, 248, 115, 192, 108, 136, 213, 142, 080, 151, 134, 130, 004, 089, 006, 082, 129, 183, 036, 157, 145, 070, 073, 033, 057, 170, 088, 251, 118, 211, 105, 167, 238, 140, 163, 058, 161, 028, 007, 236, 225, 136, 167, 214, 151, 156, 176, 179, 175, 145, 160, 180, 051, 100, 132, 085, 024, 131, 050, 139, 159, 151, 049, 189, 181, 251, 207, 098, 085, 236, 230, 124, 041, 150, 048, 210, 132, 181, 141, 005, 156, 089, 095, 210, 163, 025, 252, 159, 076, 245, 034, 177, 243, 233, 082, 202, 061, 211, 162, 072, 153, 091, 157, 073, 205, 130, 250, 081, 087, 217, 021, 077, 179, 085, 241, 023, 060, 180, 014, 082, 025, 131, 039, 068, 108, 154, 222, 052, 002, 197, 198, 172, 092, 000, 103, 110, 064, 045, 198, 161, 027, 238, 060, 181, 003, 056, 167, 091, 068, 024, 002, 182, 212, 120, 114, 194, 199, 155, 116, 130, 117, 139, 180, 180, 144, 234, 233, 121, 229, 177, 227, 241, 220, 051, 254, 223, 057, 190, 165, 024, 200, 228, 148, 158, 113, 136, 021, 252, 229, 206, 019, 044, 250, 051, 137, 255, 110, 002, 224, 137, 227, 131, 102, 198, 219, 196, 040, 009, 149, 133, 228, 156, 159, 090, 100, 120, 126, 062, 012, 166, 040, 002, 076, 172, 061, 162, 090, 201, 102, 183, 203, 073, 102, 079, 129, 012, 230, 015, 128, 048, 089, 086, 132, 202, 130, 069, 199, 212, 132, 015, 204, 021, 044, 167, 059, 009, 027, 253, 213, 246, 070, 204, 150, 199, 178, 246, 173, 178, 183, 142, 009, 018, 020, 233, 137, 125, 215, 172, 178, 115, 095, 103, 182, 064, 199, 142, 205, 220, 122, 136, 130, 083, 190, 202, 084, 079, 170, 115, 157, 141, 178, 146, 032, 098, 146, 223, 192, 037, 173, 151, 017, 122, 104, 163, 105, 044, 048, 171, 013, 203, 204, 202, 175, 227, 057, 008, 038, 159, 073, 057, 173, 137, 076, 107, 034, 211, 170, 141, 250, 049, 155, 201, 196, 066, 059, 235, 065, 224, 067, 057, 155, 168, 197, 206, 100, 194, 051, 041, 018, 184, 075, 034, 125, 018, 054, 093, 207, 088, 001, 026, 127, 117, 103, 043, 047, 014, 134, 050, 032, 111, 006, 117, 012, 042, 058, 019, 154, 087, 087, 194, 141, 043, 225, 211, 235, 154, 019, 218, 160, 020, 166, 178, 201, 100, 168, 127, 221, 003, 169, 114, 133, 036, 034, 101, 090, 184, 150, 123, 173, 244, 132, 215, 082, 094, 094, 131, 204, 150, 009, 044, 163, 017, 203, 066, 238, 156, 156, 139, 213, 221, 130, 250, 247, 002, 126, 055, 041, 147, 150, 038, 040, 254, 248, 247, 013, 127, 226, 254, 235, 079, 063, 202, 141, 027, 062, 235, 097, 210, 180, 018, 246, 253, 114, 085, 042, 167, 073, 029, 071, 254, 254, 221, 253, 222, 073, 224, 154, 036, 065, 087, 084, 166, 233, 234, 142, 239, 183, 038, 110, 162, 118, 051, 202, 196, 032, 106, 030, 158, 064, 035, 116, 123, 053, 033, 152, 222, 214, 081, 047, 155, 250, 027, 176, 077, 181, 084, 182, 020, 230, 014, 083, 027, 070, 137, 009, 247, 088, 130, 064, 197, 144, 021, 225, 073, 054, 110, 098, 192, 042, 061, 063, 039, 192, 026, 038, 086, 140, 167, 101, 210, 028, 131, 174, 020, 023, 059, 116, 105, 205, 105, 171, 116, 196, 080, 042, 104, 199, 220, 059, 196, 008, 014, 025, 150, 057, 080, 165, 174, 136, 029, 164, 191, 138, 023, 209, 224, 067, 085, 230, 174, 112, 179, 112, 162, 089, 048, 194, 206, 247, 095, 234, 236, 190, 157, 148, 106, 053, 129, 169, 151, 233, 115, 167, 150, 088, 020, 183, 108, 249, 002, 058, 111, 226, 041, 193, 148, 096, 010, 034, 034, 136, 090, 191, 014, 063, 185, 036, 102, 055, 222, 237, 195, 039, 244, 096, 165, 248, 031, 093, 094, 234, 179, 155, 246, 080, 252, 054, 170, 242, 113, 142, 118, 213, 244, 077, 133, 090, 001, 199, 150, 132, 064, 076, 169, 062, 023, 156, 163, 034, 045, 065, 198, 057, 139, 043, 114, 123, 184, 254, 028, 102, 167, 198, 100, 114, 229, 226, 227, 163, 005, 072, 026, 010, 238, 218, 221, 057, 107, 124, 183, 090, 165, 230, 205, 004, 227, 172, 130, 099, 243, 188, 211, 164, 077, 251, 159, 035, 036, 117, 178, 126, 190, 199, 115, 084, 095, 210, 069, 188, 219, 065, 234, 070, 024, 135, 059, 230, 218, 103, 157, 157, 137, 127, 206, 043, 099, 168, 165, 141, 177, 158, 198, 103, 115, 162, 144, 163, 015, 046, 226, 015, 174, 226, 167, 023, 241, 213, 083, 030, 216, 025, 223, 237, 125, 096, 188, 038, 195, 084, 156, 014, 236, 244, 132, 025, 086, 204, 254, 045, 085, 082, 235, 017, 059, 136, 084, 165, 235, 101, 244, 201, 029, 053, 193, 207, 123, 235, 052, 122, 078, 214, 178, 075, 219, 230, 066, 130, 075, 005, 095, 058, 013, 082, 026, 235, 017, 246, 237, 076, 064, 202, 203, 022, 198, 023, 084, 234, 194, 184, 011, 156, 085, 058, 201, 025, 068, 047, 017, 051, 241, 045, 043, 213, 021, 191, 019, 115, 224, 093, 196, 226, 121, 188, 090, 000, 185, 060, 241, 169, 078, 227, 250, 110, 021, 117, 058, 218, 129, 032, 171, 127, 149, 094, 002, 167, 214, 075, 032, 037, 228, 004, 081, 152, 210, 078, 167, 177, 156, 226, 207, 221, 228, 080, 185, 141, 080, 135, 179, 146, 171, 124, 097, 063, 076, 109, 226, 187, 096, 178, 190, 016, 220, 059, 143, 047, 063, 033, 112, 167, 076, 098, 155, 156, 212, 134, 038, 249, 002, 227, 036, 017, 019, 122, 106, 081, 197, 195, 054, 234, 227, 167, 159, 076, 126, 174, 149, 075, 027, 003, 183, 109, 018, 093, 092, 111, 047, 102, 106, 071, 015, 227, 235, 055, 079, 046, 175, 247, 215, 219, 235, 213, 245, 116, 210, 187, 152, 013, 043, 116, 060, 021, 108, 187, 195, 199, 145, 101, 093, 232, 178, 198, 110, 221, 059, 135, 117, 201, 085, 099, 193, 218, 104, 028, 077, 209, 230, 195, 134, 147, 032, 150, 132, 102, 208, 184, 179, 196, 071, 170, 140, 194, 007, 080, 030, 188, 177, 234, 235, 164, 124, 068, 227, 245, 154, 044, 025, 125, 239, 033, 161, 148, 176, 106, 021, 115, 237, 129, 176, 205, 041, 148, 241, 061, 067, 197, 098, 054, 013, 011, 248, 088, 162, 022, 029, 173, 005, 029, 152, 179, 254, 185, 221, 181, 232, 132, 111, 185, 181, 110, 108, 226, 104, 009, 039, 246, 118, 095, 229, 202, 044, 184, 137, 208, 039, 171, 158, 007, 218, 041, 163, 149, 098, 085, 058, 147, 183, 244, 036, 127, 103, 055, 102, 205, 110, 176, 087, 084, 040, 111, 148, 157, 008, 032, 189, 068, 128, 050, 217, 075, 173, 074, 088, 070, 079, 111, 155, 176, 107, 027, 019, 048, 152, 230, 045, 053, 142, 191, 028, 080, 178, 058, 243, 204, 021, 183, 043, 033, 212, 221, 230, 234, 206, 184, 017, 216, 058, 172, 037, 160, 085, 019, 181, 211, 046, 184, 253, 048, 157, 217, 081, 103, 206, 056, 246, 148, 236, 247, 102, 075, 086, 126, 012, 080, 142, 172, 159, 034, 037, 103, 148, 224, 008, 214, 115, 103, 229, 051, 254, 112, 174, 111, 084, 076, 040, 204, 203, 043, 004, 088, 156, 142, 208, 161, 144, 131, 107, 078, 009, 208, 254, 111, 115, 215, 218, 220, 182, 145, 101, 191, 207, 175, 160, 048, 094, 021, 096, 181, 040, 201, 241, 135, 089, 104, 096, 108, 226, 216, 137, 103, 226, 216, 177, 157, 157, 204, 080, 092, 023, 000, 130, 020, 037, 074, 084, 072, 074, 148, 070, 228, 127, 223, 123, 238, 237, 039, 000, 058, 217, 217, 218, 170, 173, 084, 044, 162, 209, 232, 247, 227, 062, 207, 061, 073, 013, 153, 173, 166, 025, 101, 201, 137, 246, 207, 235, 020, 174, 058, 211, 211, 169, 072, 243, 042, 065, 119, 139, 043, 091, 213, 102, 051, 205, 180, 231, 067, 112, 188, 228, 123, 149, 061, 094, 244, 216, 202, 241, 236, 109, 127, 243, 158, 249, 207, 152, 062, 240, 048, 165, 204, 187, 205, 102, 207, 003, 105, 242, 179, 192, 039, 097, 005, 147, 238, 027, 120, 022, 061, 138, 157, 060, 109, 072, 217, 092, 196, 239, 154, 005, 119, 170, 099, 086, 150, 022, 194, 105, 178, 211, 110, 149, 079, 007, 055, 168, 227, 204, 143, 029, 093, 038, 190, 226, 090, 252, 197, 038, 226, 047, 006, 213, 245, 132, 085, 033, 026, 123, 199, 045, 104, 093, 024, 175, 208, 017, 218, 245, 226, 240, 132, 109, 200, 071, 014, 249, 181, 130, 018, 077, 140, 201, 001, 077, 172, 077, 043, 196, 197, 065, 244, 023, 094, 230, 189, 019, 031, 201, 165, 049, 235, 025, 021, 174, 234, 237, 054, 096, 034, 228, 128, 245, 110, 207, 161, 010, 004, 041, 193, 041, 048, 108, 090, 115, 004, 248, 214, 206, 254, 047, 183, 064, 184, 153, 215, 109, 104, 201, 100, 002, 074, 014, 076, 106, 016, 200, 221, 029, 199, 022, 094, 097, 141, 236, 013, 185, 243, 044, 107, 221, 154, 122, 095, 229, 008, 011, 150, 106, 160, 093, 168, 181, 005, 228, 188, 084, 183, 165, 186, 043, 005, 224, 112, 193, 155, 081, 208, 212, 213, 090, 155, 111, 232, 102, 111, 204, 192, 037, 079, 128, 053, 094, 102, 045, 042, 064, 061, 032, 145, 047, 185, 240, 110, 065, 161, 095, 052, 166, 188, 230, 122, 119, 091, 044, 058, 099, 227, 046, 006, 123, 007, 108, 188, 251, 200, 227, 211, 131, 131, 170, 213, 174, 080, 193, 229, 052, 254, 056, 146, 225, 167, 050, 022, 247, 020, 250, 243, 012, 127, 244, 158, 233, 012, 172, 022, 076, 130, 137, 192, 163, 173, 052, 025, 055, 111, 012, 033, 091, 128, 020, 010, 155, 148, 006, 076, 053, 238, 046, 158, 018, 235, 195, 016, 235, 137, 098, 137, 112, 031, 060, 169, 113, 015, 206, 111, 203, 116, 085, 038, 094, 176, 226, 042, 055, 071, 073, 174, 227, 159, 120, 099, 130, 105, 072, 107, 119, 085, 213, 254, 029, 065, 027, 212, 186, 113, 080, 145, 249, 040, 229, 157, 227, 045, 043, 234, 200, 001, 168, 154, 074, 010, 153, 216, 066, 052, 038, 058, 023, 033, 254, 020, 186, 128, 081, 227, 192, 047, 141, 121, 200, 040, 055, 248, 146, 012, 164, 101, 186, 155, 074, 144, 190, 142, 237, 133, 080, 242, 062, 245, 099, 099, 223, 100, 140, 054, 018, 194, 212, 105, 202, 204, 089, 218, 243, 006, 056, 245, 045, 140, 090, 052, 086, 201, 070, 123, 177, 206, 011, 136, 164, 146, 189, 182, 058, 215, 097, 120, 026, 066, 205, 088, 066, 062, 104, 069, 246, 112, 002, 218, 223, 015, 077, 072, 018, 237, 062, 155, 141, 007, 053, 002, 099, 143, 068, 027, 115, 243, 122, 122, 207, 242, 249, 074, 237, 152, 230, 042, 201, 031, 168, 248, 123, 098, 043, 247, 214, 165, 077, 099, 077, 049, 240, 243, 006, 190, 238, 057, 210, 172, 223, 097, 116, 080, 037, 195, 204, 100, 186, 054, 051, 080, 049, 093, 218, 198, 151, 191, 047, 137, 032, 016, 086, 254, 182, 108, 159, 111, 001, 187, 200, 242, 205, 198, 194, 162, 053, 209, 217, 200, 112, 172, 247, 238, 121, 174, 194, 110, 211, 167, 191, 209, 135, 010, 129, 246, 016, 176, 203, 028, 216, 237, 145, 090, 206, 111, 023, 085, 173, 103, 224, 232, 108, 077, 196, 110, 210, 041, 003, 186, 043, 181, 095, 144, 093, 151, 167, 173, 134, 211, 198, 226, 108, 161, 198, 194, 185, 042, 089, 189, 001, 196, 136, 082, 162, 146, 015, 106, 043, 030, 173, 244, 055, 121, 099, 127, 243, 085, 175, 115, 143, 113, 017, 165, 029, 085, 085, 158, 035, 143, 222, 040, 059, 135, 168, 076, 134, 157, 117, 096, 042, 077, 207, 098, 239, 072, 209, 011, 252, 075, 083, 220, 185, 155, 184, 033, 113, 008, 161, 158, 209, 121, 178, 002, 188, 095, 169, 143, 014, 142, 081, 073, 021, 115, 165, 171, 029, 011, 073, 235, 125, 130, 227, 146, 129, 172, 170, 196, 027, 217, 112, 233, 240, 123, 166, 230, 058, 162, 056, 184, 005, 086, 049, 154, 180, 244, 176, 004, 103, 096, 104, 076, 113, 206, 042, 219, 119, 037, 045, 210, 210, 221, 193, 119, 101, 127, 058, 162, 025, 229, 216, 044, 248, 091, 205, 231, 139, 209, 178, 091, 117, 117, 218, 156, 163, 184, 179, 087, 101, 146, 088, 191, 108, 110, 076, 174, 255, 166, 070, 152, 099, 009, 033, 009, 223, 214, 229, 185, 100, 078, 178, 142, 210, 003, 168, 017, 214, 051, 143, 167, 068, 094, 086, 186, 018, 223, 083, 075, 230, 105, 171, 252, 229, 160, 157, 097, 235, 209, 148, 133, 020, 221, 083, 102, 039, 088, 056, 089, 224, 118, 202, 084, 059, 018, 074, 012, 106, 148, 177, 165, 105, 251, 174, 249, 151, 090, 171, 022, 187, 248, 180, 219, 108, 251, 238, 017, 043, 098, 190, 125, 204, 116, 109, 089, 144, 032, 017, 081, 194, 053, 046, 130, 136, 022, 107, 224, 110, 000, 095, 204, 097, 044, 183, 186, 104, 222, 238, 047, 104, 109, 017, 227, 168, 233, 169, 127, 106, 162, 073, 098, 186, 008, 201, 180, 049, 145, 008, 054, 050, 165, 027, 209, 112, 050, 029, 245, 181, 206, 095, 108, 144, 001, 073, 033, 233, 132, 211, 241, 055, 072, 039, 049, 073, 254, 050, 233, 244, 062, 044, 198, 007, 095, 181, 007, 112, 129, 048, 041, 221, 154, 009, 208, 223, 044, 004, 040, 012, 022, 180, 242, 112, 058, 040, 085, 083, 227, 037, 092, 059, 027, 068, 086, 171, 007, 255, 026, 145, 213, 219, 073, 051, 185, 030, 176, 164, 146, 105, 038, 231, 118, 000, 139, 012, 143, 032, 250, 253, 004, 079, 193, 062, 171, 191, 147, 184, 065, 102, 137, 019, 111, 072, 151, 162, 020, 247, 233, 223, 199, 251, 082, 118, 246, 189, 141, 060, 115, 020, 246, 026, 123, 195, 241, 069, 078, 142, 147, 244, 159, 165, 145, 162, 089, 027, 191, 205, 230, 235, 118, 034, 007, 168, 090, 212, 080, 090, 031, 158, 128, 104, 209, 163, 003, 159, 237, 057, 028, 222, 001, 251, 250, 154, 126, 041, 145, 091, 194, 005, 222, 202, 047, 197, 147, 050, 148, 198, 217, 216, 104, 003, 017, 192, 169, 104, 185, 168, 058, 182, 180, 063, 230, 095, 216, 109, 147, 112, 039, 063, 119, 238, 155, 086, 126, 199, 251, 215, 179, 156, 055, 172, 220, 142, 177, 044, 124, 060, 087, 207, 060, 187, 108, 112, 248, 097, 216, 020, 255, 041, 204, 040, 116, 105, 219, 051, 161, 163, 182, 255, 085, 053, 161, 230, 074, 047, 024, 008, 248, 234, 098, 244, 238, 122, 246, 000, 144, 151, 226, 254, 007, 222, 203, 152, 173, 122, 054, 211, 080, 049, 250, 233, 189, 054, 076, 167, 079, 230, 107, 122, 117, 141, 244, 249, 076, 255, 186, 093, 214, 111, 139, 027, 250, 193, 128, 164, 223, 008, 222, 129, 050, 120, 007, 175, 244, 017, 223, 100, 110, 205, 094, 018, 193, 087, 032, 081, 098, 137, 029, 207, 149, 022, 152, 218, 121, 162, 015, 172, 152, 053, 162, 031, 115, 110, 150, 156, 136, 223, 176, 236, 209, 200, 029, 143, 038, 094, 052, 168, 050, 136, 043, 110, 246, 130, 172, 074, 246, 171, 221, 134, 108, 228, 104, 244, 018, 239, 118, 201, 041, 045, 176, 167, 053, 156, 245, 196, 145, 201, 110, 102, 017, 171, 087, 075, 026, 077, 021, 177, 047, 136, 044, 213, 075, 237, 246, 102, 196, 115, 093, 112, 206, 005, 130, 197, 056, 075, 029, 216, 216, 024, 144, 028, 017, 160, 078, 065, 232, 179, 221, 241, 075, 104, 234, 213, 040, 059, 097, 041, 143, 111, 086, 014, 231, 087, 184, 192, 058, 017, 215, 055, 034, 226, 074, 030, 225, 083, 040, 229, 017, 131, 049, 152, 048, 215, 224, 028, 246, 233, 067, 246, 157, 021, 248, 226, 209, 065, 038, 079, 167, 231, 198, 177, 023, 049, 092, 232, 228, 226, 008, 042, 060, 210, 149, 025, 105, 117, 078, 043, 049, 064, 235, 214, 128, 105, 255, 199, 099, 237, 213, 242, 027, 195, 189, 183, 075, 232, 044, 026, 072, 244, 198, 244, 037, 138, 254, 063, 077, 144, 060, 118, 077, 019, 164, 090, 035, 014, 012, 047, 165, 216, 055, 234, 095, 159, 054, 081, 227, 054, 167, 205, 051, 091, 211, 067, 097, 076, 068, 090, 010, 237, 210, 015, 075, 132, 240, 075, 098, 184, 226, 054, 133, 214, 108, 007, 051, 151, 164, 077, 243, 181, 110, 193, 191, 153, 118, 175, 149, 113, 040, 238, 055, 211, 014, 246, 027, 014, 185, 221, 206, 161, 178, 008, 121, 009, 134, 051, 077, 108, 250, 163, 024, 197, 233, 170, 152, 186, 232, 152, 238, 018, 238, 194, 152, 030, 004, 237, 090, 074, 075, 136, 203, 171, 131, 126, 065, 056, 226, 122, 094, 106, 187, 104, 075, 048, 020, 012, 082, 032, 227, 103, 226, 075, 101, 174, 003, 014, 234, 073, 044, 135, 062, 127, 182, 119, 236, 231, 207, 044, 095, 208, 211, 041, 175, 245, 140, 034, 182, 170, 048, 212, 172, 197, 216, 093, 128, 128, 014, 232, 248, 173, 203, 047, 108, 084, 090, 146, 101, 134, 181, 085, 096, 109, 133, 203, 125, 164, 151, 123, 231, 034, 231, 029, 208, 185, 208, 237, 098, 102, 137, 165, 177, 212, 062, 062, 053, 038, 219, 222, 173, 022, 033, 120, 106, 143, 099, 167, 246, 116, 064, 213, 158, 137, 164, 218, 155, 205, 139, 081, 111, 081, 047, 137, 196, 232, 137, 000, 183, 119, 123, 205, 137, 028, 039, 184, 055, 042, 103, 242, 131, 067, 151, 035, 056, 169, 252, 186, 189, 145, 191, 152, 170, 158, 141, 118, 222, 051, 001, 206, 123, 046, 024, 122, 207, 005, 064, 239, 073, 052, 216, 158, 092, 192, 061, 023, 116, 155, 203, 053, 193, 183, 241, 131, 138, 175, 023, 139, 057, 237, 023, 220, 147, 247, 043, 058, 118, 110, 035, 207, 005, 189, 203, 068, 034, 228, 216, 029, 235, 220, 034, 200, 143, 101, 111, 176, 206, 093, 163, 221, 087, 122, 153, 155, 104, 148, 101, 210, 138, 162, 114, 142, 014, 126, 033, 098, 166, 235, 049, 196, 201, 174, 211, 008, 052, 083, 088, 228, 245, 111, 113, 248, 205, 230, 149, 152, 118, 191, 178, 134, 022, 234, 053, 180, 132, 249, 145, 250, 142, 254, 198, 042, 217, 196, 103, 131, 205, 035, 253, 217, 110, 134, 201, 038, 226, 048, 146, 209, 217, 025, 238, 239, 225, 230, 236, 108, 128, 223, 071, 229, 248, 122, 177, 194, 227, 237, 224, 108, 084, 028, 142, 191, 062, 124, 061, 124, 124, 190, 077, 158, 070, 103, 203, 167, 105, 190, 065, 120, 201, 205, 184, 160, 045, 195, 070, 087, 155, 195, 060, 206, 247, 142, 207, 070, 201, 217, 232, 000, 049, 037, 251, 244, 119, 147, 160, 236, 250, 213, 112, 112, 112, 056, 204, 057, 129, 149, 146, 076, 247, 254, 229, 227, 187, 031, 003, 144, 077, 168, 030, 251, 072, 005, 125, 139, 191, 146, 207, 069, 070, 115, 105, 049, 024, 050, 233, 052, 182, 000, 015, 116, 109, 014, 084, 121, 103, 192, 001, 092, 032, 188, 218, 174, 243, 239, 202, 208, 185, 159, 189, 188, 029, 091, 205, 142, 127, 217, 113, 162, 142, 217, 067, 164, 096, 205, 025, 248, 014, 186, 116, 247, 198, 135, 123, 053, 238, 032, 098, 070, 115, 123, 036, 070, 250, 099, 000, 091, 176, 037, 009, 047, 176, 056, 122, 115, 077, 076, 057, 157, 039, 104, 121, 218, 131, 240, 006, 028, 052, 119, 129, 056, 156, 054, 196, 168, 026, 241, 101, 008, 135, 179, 166, 117, 159, 005, 220, 067, 103, 057, 142, 067, 209, 255, 246, 221, 219, 247, 040, 107, 145, 067, 244, 090, 175, 123, 094, 018, 219, 078, 008, 044, 197, 098, 126, 245, 145, 075, 003, 091, 141, 021, 127, 116, 127, 053, 067, 240, 075, 234, 151, 124, 245, 117, 005, 168, 228, 095, 180, 093, 101, 244, 118, 074, 187, 117, 057, 031, 175, 250, 224, 195, 222, 189, 021, 227, 130, 229, 195, 117, 149, 069, 060, 229, 017, 061, 099, 035, 211, 107, 200, 059, 092, 244, 135, 202, 152, 225, 248, 066, 138, 006, 242, 051, 205, 200, 046, 187, 004, 110, 239, 130, 007, 207, 243, 215, 107, 013, 039, 085, 043, 163, 169, 042, 241, 070, 253, 158, 022, 246, 031, 251, 079, 159, 028, 169, 055, 088, 226, 131, 124, 127, 152, 124, 206, 006, 255, 181, 063, 124, 122, 164, 254, 194, 012, 120, 255, 105, 158, 164, 131, 222, 217, 106, 008, 128, 072, 094, 235, 079, 147, 179, 069, 254, 228, 104, 114, 165, 254, 106, 120, 244, 146, 206, 151, 077, 113, 115, 131, 255, 015, 151, 171, 249, 162, 152, 212, 155, 254, 193, 033, 111, 212, 037, 188, 076, 198, 116, 190, 110, 232, 040, 217, 172, 167, 035, 196, 253, 077, 169, 210, 031, 244, 231, 223, 189, 250, 180, 249, 254, 213, 215, 223, 194, 253, 245, 045, 210, 206, 142, 206, 142, 142, 212, 143, 252, 122, 112, 182, 166, 130, 134, 007, 041, 054, 005, 094, 240, 190, 059, 059, 202, 255, 056, 124, 250, 031, 180, 083, 228, 119, 074, 173, 162, 023, 105, 140, 024, 180, 027, 250, 239, 072, 189, 043, 225, 188, 249, 158, 255, 253, 137, 206, 250, 167, 071, 145, 113, 038, 141, 158, 210, 212, 124, 040, 179, 111, 075, 230, 232, 212, 199, 050, 251, 081, 059, 187, 125, 104, 008, 009, 229, 122, 180, 148, 249, 039, 159, 050, 015, 237, 132, 218, 075, 207, 026, 150, 162, 190, 083, 227, 229, 202, 114, 240, 176, 146, 240, 046, 110, 146, 141, 085, 098, 200, 037, 035, 020, 143, 014, 004, 150, 209, 034, 083, 098, 033, 143, 044, 028, 037, 109, 131, 167, 145, 018, 039, 169, 130, 061, 233, 224, 072, 107, 108, 208, 043, 172, 224, 198, 059, 214, 107, 086, 190, 063, 215, 207, 022, 053, 194, 152, 046, 049, 012, 009, 110, 225, 247, 165, 007, 046, 001, 061, 035, 222, 079, 237, 201, 001, 168, 041, 054, 168, 228, 155, 174, 096, 036, 170, 065, 192, 020, 235, 079, 046, 178, 115, 109, 126, 188, 203, 052, 247, 098, 179, 025, 111, 054, 245, 224, 098, 152, 143, 243, 189, 120, 154, 093, 024, 169, 086, 138, 200, 016, 068, 004, 224, 098, 118, 230, 245, 023, 137, 154, 224, 031, 184, 022, 037, 106, 106, 213, 180, 126, 102, 248, 145, 001, 116, 146, 125, 008, 246, 247, 039, 188, 024, 092, 191, 255, 179, 233, 199, 006, 061, 211, 069, 113, 255, 177, 094, 173, 168, 109, 203, 254, 120, 086, 172, 180, 191, 016, 000, 174, 125, 215, 073, 103, 208, 064, 003, 075, 147, 031, 215, 244, 151, 206, 065, 009, 148, 241, 072, 071, 031, 070, 029, 047, 125, 105, 164, 239, 212, 132, 091, 207, 015, 094, 251, 183, 110, 039, 101, 096, 033, 009, 179, 186, 100, 132, 055, 219, 057, 077, 199, 080, 151, 104, 158, 166, 232, 234, 180, 047, 067, 227, 161, 118, 213, 026, 093, 240, 106, 122, 165, 209, 001, 216, 236, 225, 067, 189, 188, 161, 078, 213, 223, 019, 147, 077, 023, 102, 164, 163, 043, 030, 126, 146, 160, 189, 098, 124, 193, 176, 155, 018, 209, 022, 049, 036, 057, 176, 029, 254, 181, 232, 132, 143, 083, 059, 027, 147, 228, 180, 036, 142, 253, 114, 075, 057, 209, 022, 250, 170, 074, 198, 220, 044, 241, 184, 180, 133, 085, 162, 185, 154, 234, 128, 196, 212, 055, 186, 215, 217, 205, 103, 194, 176, 071, 120, 049, 100, 007, 062, 093, 034, 004, 033, 163, 108, 066, 243, 006, 071, 212, 145, 153, 232, 113, 030, 143, 247, 164, 227, 251, 251, 174, 033, 000, 023, 067, 008, 057, 035, 017, 181, 195, 251, 075, 115, 145, 059, 031, 112, 044, 247, 075, 127, 104, 245, 238, 226, 113, 000, 218, 182, 027, 010, 191, 193, 201, 197, 096, 210, 020, 014, 132, 029, 162, 195, 004, 024, 235, 050, 041, 134, 045, 098, 168, 088, 248, 082, 241, 012, 188, 158, 214, 179, 209, 082, 066, 211, 085, 131, 142, 116, 090, 068, 009, 007, 242, 027, 225, 206, 071, 019, 095, 179, 031, 001, 100, 046, 153, 159, 000, 227, 083, 219, 133, 132, 253, 009, 149, 087, 061, 170, 149, 181, 050, 198, 196, 136, 133, 185, 164, 097, 024, 105, 012, 089, 206, 200, 145, 247, 178, 011, 098, 003, 153, 025, 131, 159, 026, 237, 030, 254, 169, 246, 038, 046, 022, 223, 005, 175, 009, 200, 011, 029, 081, 120, 078, 067, 165, 145, 226, 092, 017, 231, 152, 079, 091, 010, 063, 049, 155, 008, 055, 142, 028, 217, 234, 097, 138, 127, 216, 175, 131, 225, 227, 144, 071, 093, 218, 025, 069, 169, 137, 183, 188, 038, 156, 019, 213, 079, 104, 068, 006, 209, 234, 124, 049, 095, 047, 163, 097, 082, 102, 019, 200, 254, 185, 099, 184, 252, 229, 089, 095, 188, 051, 011, 189, 188, 092, 193, 129, 034, 184, 071, 021, 255, 073, 039, 249, 044, 141, 126, 156, 247, 100, 010, 113, 149, 245, 198, 068, 024, 096, 081, 082, 087, 086, 115, 140, 194, 214, 176, 155, 166, 156, 229, 109, 085, 213, 224, 091, 048, 244, 105, 185, 245, 096, 051, 011, 038, 025, 210, 099, 069, 172, 201, 234, 237, 124, 196, 090, 008, 246, 094, 092, 021, 236, 197, 232, 031, 054, 233, 227, 237, 098, 150, 126, 040, 021, 235, 123, 035, 186, 039, 035, 053, 093, 254, 064, 164, 234, 044, 253, 171, 022, 112, 126, 068, 040, 108, 058, 247, 056, 074, 042, 048, 041, 111, 022, 115, 084, 206, 001, 028, 113, 164, 128, 254, 016, 200, 076, 222, 207, 159, 184, 040, 088, 103, 079, 133, 226, 061, 186, 063, 092, 175, 215, 135, 176, 248, 059, 164, 234, 088, 072, 085, 143, 078, 193, 024, 044, 000, 001, 245, 243, 167, 215, 135, 127, 138, 084, 065, 093, 130, 015, 038, 059, 127, 253, 084, 074, 152, 065, 033, 139, 136, 058, 156, 094, 071, 018, 098, 076, 082, 240, 051, 082, 247, 120, 014, 106, 186, 154, 169, 158, 165, 164, 212, 197, 146, 209, 161, 189, 012, 072, 209, 057, 046, 138, 187, 066, 071, 168, 220, 154, 182, 083, 237, 040, 243, 232, 172, 164, 063, 103, 229, 145, 084, 073, 143, 248, 123, 036, 229, 209, 019, 254, 210, 091, 136, 107, 252, 141, 035, 031, 071, 038, 145, 168, 161, 072, 247, 194, 036, 065, 105, 097, 154, 101, 210, 064, 131, 074, 011, 244, 022, 198, 008, 112, 019, 163, 084, 200, 067, 033, 014, 123, 220, 103, 012, 180, 060, 162, 020, 240, 170, 150, 104, 215, 233, 232, 121, 234, 136, 089, 186, 084, 221, 133, 034, 243, 109, 230, 234, 030, 168, 164, 091, 187, 034, 110, 119, 168, 059, 202, 156, 238, 044, 190, 182, 194, 155, 010, 162, 130, 148, 210, 195, 084, 014, 017, 137, 004, 023, 025, 150, 104, 154, 119, 048, 123, 165, 196, 079, 139, 226, 154, 186, 189, 088, 033, 241, 189, 078, 108, 248, 181, 182, 220, 208, 028, 157, 099, 156, 103, 216, 032, 090, 035, 158, 052, 145, 053, 102, 238, 066, 189, 189, 129, 079, 081, 133, 216, 194, 179, 190, 238, 242, 102, 051, 083, 115, 247, 072, 069, 251, 048, 054, 087, 253, 139, 095, 111, 235, 197, 003, 016, 176, 174, 152, 097, 064, 080, 079, 117, 019, 248, 085, 171, 095, 233, 241, 101, 049, 155, 001, 171, 019, 078, 085, 215, 021, 241, 195, 196, 022, 047, 000, 031, 177, 200, 160, 011, 043, 086, 183, 203, 151, 084, 044, 135, 111, 067, 172, 118, 181, 194, 063, 183, 068, 157, 221, 101, 081, 085, 208, 039, 048, 040, 083, 235, 236, 017, 066, 231, 135, 143, 188, 177, 143, 085, 235, 158, 236, 128, 016, 162, 083, 232, 025, 029, 101, 183, 114, 169, 093, 038, 143, 151, 064, 127, 049, 018, 150, 191, 104, 066, 115, 146, 036, 151, 131, 178, 029, 129, 142, 232, 131, 103, 195, 109, 153, 093, 210, 193, 031, 190, 217, 122, 156, 012, 180, 139, 108, 118, 087, 110, 209, 166, 175, 103, 179, 176, 089, 093, 097, 119, 184, 081, 249, 036, 181, 050, 252, 015, 112, 214, 089, 174, 090, 029, 241, 053, 169, 065, 019, 012, 213, 114, 011, 123, 177, 108, 005, 139, 131, 021, 155, 039, 020, 138, 053, 096, 165, 246, 241, 085, 224, 203, 023, 211, 081, 253, 086, 147, 024, 093, 010, 015, 020, 050, 179, 068, 072, 086, 152, 111, 221, 228, 116, 143, 045, 067, 009, 060, 123, 113, 155, 088, 036, 132, 034, 089, 064, 210, 048, 192, 191, 010, 010, 039, 161, 047, 122, 107, 227, 225, 090, 012, 214, 122, 206, 135, 065, 032, 054, 218, 009, 037, 022, 123, 135, 046, 099, 179, 185, 179, 238, 215, 240, 229, 229, 140, 176, 220, 123, 136, 143, 149, 233, 231, 022, 237, 185, 177, 014, 236, 235, 196, 057, 024, 254, 010, 017, 153, 162, 106, 229, 042, 200, 214, 226, 149, 187, 022, 158, 140, 030, 225, 130, 171, 102, 125, 218, 240, 025, 007, 190, 229, 159, 155, 205, 135, 018, 192, 152, 078, 182, 244, 061, 091, 149, 218, 199, 183, 165, 226, 003, 255, 032, 058, 058, 098, 075, 103, 214, 044, 084, 253, 171, 122, 117, 062, 031, 001, 215, 082, 212, 015, 051, 155, 034, 089, 040, 167, 165, 100, 012, 195, 239, 146, 152, 097, 072, 118, 243, 036, 081, 052, 212, 022, 073, 180, 049, 137, 197, 093, 126, 059, 191, 162, 035, 159, 249, 125, 195, 056, 113, 251, 027, 188, 147, 010, 178, 103, 123, 241, 030, 053, 104, 036, 020, 001, 119, 003, 206, 076, 207, 244, 211, 051, 142, 196, 043, 064, 189, 209, 249, 106, 117, 147, 050, 147, 067, 185, 242, 232, 079, 199, 081, 026, 061, 127, 254, 021, 209, 161, 192, 246, 160, 220, 141, 108, 092, 090, 144, 143, 107, 071, 007, 017, 236, 217, 187, 019, 157, 036, 215, 178, 025, 038, 159, 030, 145, 140, 079, 231, 194, 012, 016, 006, 025, 070, 084, 088, 032, 240, 068, 081, 063, 227, 188, 084, 051, 034, 027, 215, 137, 146, 157, 174, 087, 202, 250, 116, 106, 066, 029, 163, 086, 185, 147, 213, 084, 144, 100, 233, 216, 227, 187, 255, 224, 128, 105, 126, 142, 083, 108, 100, 091, 017, 159, 136, 112, 145, 113, 147, 042, 127, 194, 152, 003, 080, 065, 022, 075, 077, 153, 103, 123, 063, 104, 026, 064, 178, 066, 172, 203, 179, 016, 228, 226, 093, 166, 251, 167, 223, 031, 100, 241, 235, 210, 034, 141, 070, 251, 052, 102, 121, 148, 028, 232, 238, 106, 253, 181, 060, 241, 020, 034, 166, 115, 166, 093, 245, 100, 197, 190, 113, 159, 059, 160, 210, 055, 180, 084, 159, 156, 124, 206, 162, 131, 087, 240, 092, 075, 199, 007, 157, 213, 068, 054, 007, 091, 254, 143, 013, 017, 196, 014, 250, 062, 085, 196, 004, 240, 186, 223, 060, 168, 226, 232, 205, 248, 208, 228, 057, 252, 056, 165, 163, 058, 082, 173, 047, 089, 212, 074, 036, 213, 151, 010, 249, 145, 118, 036, 032, 190, 170, 243, 200, 229, 166, 086, 197, 110, 225, 184, 113, 196, 147, 071, 066, 177, 235, 034, 246, 155, 151, 150, 116, 215, 020, 048, 082, 042, 040, 037, 081, 093, 031, 124, 205, 164, 086, 228, 239, 089, 230, 104, 102, 125, 077, 132, 013, 194, 055, 195, 124, 231, 155, 003, 077, 203, 135, 201, 121, 164, 136, 110, 253, 169, 060, 136, 078, 123, 191, 102, 199, 253, 099, 006, 225, 077, 082, 087, 012, 059, 216, 159, 090, 202, 158, 006, 066, 174, 149, 164, 163, 189, 056, 092, 244, 107, 118, 057, 229, 184, 239, 018, 179, 243, 035, 209, 188, 130, 081, 111, 031, 069, 177, 113, 165, 214, 106, 150, 100, 050, 136, 178, 135, 236, 038, 210, 103, 109, 114, 074, 055, 049, 255, 140, 108, 067, 030, 245, 113, 154, 158, 104, 242, 252, 068, 089, 151, 247, 147, 109, 178, 166, 250, 227, 153, 105, 196, 069, 246, 051, 136, 024, 189, 083, 249, 054, 094, 247, 221, 109, 158, 157, 096, 103, 206, 027, 123, 144, 195, 143, 014, 168, 113, 195, 214, 222, 158, 137, 044, 015, 243, 176, 018, 223, 044, 004, 085, 143, 207, 179, 093, 096, 108, 166, 039, 145, 206, 014, 236, 073, 251, 109, 034, 081, 101, 111, 169, 025, 023, 244, 061, 241, 006, 075, 245, 096, 056, 147, 123, 161, 029, 248, 154, 075, 152, 153, 233, 221, 159, 062, 196, 135, 039, 010, 202, 113, 190, 220, 248, 009, 156, 137, 165, 217, 034, 015, 244, 243, 033, 182, 136, 159, 018, 076, 110, 169, 086, 234, 078, 221, 171, 135, 172, 058, 133, 245, 200, 045, 181, 252, 054, 123, 166, 206, 025, 219, 217, 247, 055, 059, 071, 160, 071, 109, 203, 194, 000, 157, 017, 145, 065, 254, 184, 149, 047, 142, 243, 231, 068, 011, 093, 210, 175, 236, 217, 049, 141, 193, 087, 199, 199, 047, 232, 010, 251, 234, 248, 057, 204, 158, 020, 166, 252, 046, 251, 091, 025, 207, 104, 150, 129, 158, 113, 151, 253, 130, 135, 059, 122, 188, 076, 212, 101, 030, 055, 246, 253, 061, 221, 135, 029, 018, 136, 031, 104, 075, 219, 157, 078, 007, 227, 125, 215, 017, 145, 221, 211, 139, 238, 239, 177, 163, 237, 103, 122, 123, 083, 118, 154, 088, 105, 040, 117, 013, 098, 071, 220, 031, 114, 130, 230, 028, 053, 203, 196, 061, 072, 117, 127, 036, 117, 117, 101, 026, 146, 198, 015, 217, 029, 211, 019, 053, 145, 142, 119, 114, 106, 174, 232, 007, 047, 073, 026, 150, 189, 021, 068, 108, 171, 236, 065, 193, 047, 026, 184, 219, 084, 130, 230, 042, 105, 160, 152, 075, 063, 198, 005, 101, 168, 018, 026, 051, 243, 147, 237, 166, 016, 029, 228, 001, 116, 000, 141, 213, 077, 000, 177, 114, 165, 006, 180, 076, 212, 122, 152, 164, 055, 062, 198, 202, 021, 214, 236, 131, 090, 013, 093, 161, 160, 160, 226, 005, 008, 094, 061, 153, 193, 106, 191, 204, 101, 189, 107, 078, 053, 229, 167, 087, 210, 070, 044, 127, 170, 121, 153, 162, 184, 095, 025, 239, 205, 171, 132, 210, 168, 164, 184, 177, 113, 094, 234, 109, 104, 055, 207, 225, 161, 185, 240, 088, 018, 221, 117, 221, 205, 217, 227, 194, 144, 181, 107, 166, 100, 089, 228, 191, 211, 224, 114, 098, 204, 039, 085, 196, 252, 085, 194, 223, 124, 100, 054, 113, 151, 167, 129, 124, 163, 199, 128, 174, 040, 205, 084, 134, 070, 038, 048, 100, 082, 209, 205, 124, 217, 097, 146, 215, 212, 098, 133, 094, 208, 161, 164, 150, 069, 107, 212, 227, 017, 188, 211, 085, 101, 057, 034, 225, 121, 028, 226, 011, 051, 122, 133, 240, 245, 165, 050, 103, 115, 090, 139, 196, 160, 082, 230, 156, 099, 080, 137, 233, 242, 061, 056, 108, 011, 223, 000, 141, 189, 110, 254, 231, 250, 174, 152, 253, 076, 055, 114, 151, 215, 131, 212, 233, 087, 037, 034, 004, 091, 157, 025, 011, 197, 087, 188, 039, 045, 056, 177, 242, 004, 058, 099, 180, 048, 005, 204, 104, 210, 192, 184, 088, 047, 138, 027, 132, 094, 246, 107, 255, 087, 173, 030, 116, 089, 161, 197, 131, 049, 116, 208, 142, 118, 214, 072, 076, 099, 221, 114, 084, 059, 223, 212, 053, 233, 215, 191, 034, 082, 147, 013, 204, 124, 106, 178, 133, 166, 071, 065, 168, 121, 083, 184, 042, 155, 017, 213, 005, 026, 152, 001, 147, 218, 193, 225, 141, 233, 186, 075, 113, 086, 236, 069, 144, 238, 130, 006, 217, 176, 222, 002, 016, 028, 240, 034, 024, 128, 055, 215, 215, 117, 167, 043, 199, 239, 048, 037, 104, 140, 037, 023, 213, 030, 205, 047, 216, 014, 088, 243, 000, 056, 194, 025, 145, 011, 028, 206, 181, 074, 041, 175, 220, 036, 037, 105, 233, 069, 040, 079, 164, 245, 157, 230, 124, 065, 187, 079, 119, 047, 133, 170, 189, 020, 202, 208, 237, 049, 073, 165, 170, 219, 235, 176, 178, 080, 233, 043, 019, 013, 228, 148, 150, 227, 139, 181, 215, 022, 211, 001, 004, 198, 134, 197, 128, 179, 187, 177, 241, 195, 197, 247, 208, 133, 132, 164, 122, 005, 137, 037, 128, 214, 254, 123, 025, 183, 204, 101, 109, 092, 022, 023, 087, 170, 013, 098, 233, 068, 208, 255, 048, 225, 199, 246, 118, 067, 221, 227, 056, 177, 187, 104, 207, 216, 210, 020, 045, 215, 009, 070, 091, 052, 176, 135, 220, 054, 128, 246, 009, 166, 161, 117, 219, 115, 197, 020, 129, 197, 222, 214, 154, 041, 104, 183, 001, 003, 081, 164, 177, 013, 059, 214, 164, 131, 250, 015, 034, 254, 198, 073, 024, 075, 233, 207, 217, 049, 006, 197, 143, 167, 196, 073, 123, 197, 174, 240, 158, 041, 143, 202, 086, 053, 090, 194, 209, 227, 102, 117, 071, 083, 246, 058, 219, 140, 050, 088, 006, 245, 164, 204, 142, 254, 237, 217, 241, 209, 068, 125, 134, 074, 127, 112, 054, 124, 114, 164, 138, 010, 062, 192, 249, 217, 053, 037, 151, 149, 104, 034, 197, 252, 193, 088, 035, 079, 175, 160, 198, 164, 235, 183, 094, 177, 254, 146, 237, 146, 171, 234, 075, 118, 204, 151, 245, 195, 164, 190, 078, 142, 166, 110, 145, 140, 170, 134, 146, 161, 005, 128, 175, 111, 033, 079, 187, 142, 048, 055, 143, 068, 004, 124, 054, 086, 172, 073, 078, 251, 012, 113, 032, 080, 218, 065, 052, 136, 136, 176, 111, 202, 225, 172, 021, 110, 157, 151, 032, 231, 015, 162, 097, 164, 106, 193, 158, 072, 172, 108, 031, 080, 005, 250, 203, 061, 014, 110, 065, 031, 163, 017, 035, 193, 234, 010, 065, 102, 202, 196, 214, 087, 115, 113, 037, 112, 128, 185, 068, 205, 036, 103, 227, 182, 216, 136, 238, 190, 001, 240, 088, 194, 087, 141, 179, 160, 076, 242, 050, 054, 238, 178, 037, 236, 122, 232, 030, 028, 024, 215, 209, 097, 038, 114, 232, 159, 063, 188, 001, 101, 065, 107, 154, 227, 218, 031, 068, 196, 059, 118, 188, 041, 019, 022, 197, 088, 021, 151, 208, 088, 161, 228, 147, 013, 197, 124, 077, 158, 207, 213, 123, 224, 147, 216, 053, 133, 150, 051, 106, 064, 172, 240, 250, 053, 243, 021, 132, 129, 145, 051, 133, 029, 034, 148, 243, 091, 182, 003, 239, 001, 097, 210, 136, 086, 138, 001, 047, 049, 203, 014, 092, 235, 098, 062, 189, 142, 137, 075, 118, 194, 158, 039, 068, 173, 028, 068, 205, 059, 151, 046, 174, 041, 219, 031, 119, 197, 149, 214, 146, 011, 129, 105, 055, 025, 165, 095, 009, 067, 138, 251, 073, 187, 078, 208, 206, 075, 080, 059, 234, 201, 201, 089, 107, 027, 004, 103, 048, 002, 044, 050, 231, 225, 170, 237, 225, 232, 208, 212, 000, 087, 157, 119, 042, 175, 189, 224, 070, 192, 240, 097, 204, 245, 129, 060, 093, 198, 081, 234, 188, 123, 247, 247, 171, 074, 246, 067, 224, 101, 142, 088, 160, 101, 101, 054, 010, 205, 188, 062, 187, 197, 057, 115, 179, 247, 015, 243, 042, 225, 112, 196, 094, 223, 124, 208, 027, 223, 163, 254, 052, 144, 180, 138, 179, 160, 231, 060, 095, 037, 185, 056, 206, 087, 029, 142, 243, 143, 232, 067, 042, 174, 049, 074, 124, 075, 010, 059, 163, 005, 017, 174, 176, 161, 096, 234, 051, 237, 200, 090, 117, 103, 101, 034, 086, 147, 124, 193, 026, 190, 063, 055, 228, 061, 163, 190, 007, 006, 041, 249, 142, 233, 213, 026, 164, 124, 090, 209, 254, 115, 001, 198, 137, 167, 170, 095, 252, 041, 063, 071, 042, 157, 112, 084, 227, 006, 244, 240, 006, 172, 253, 006, 103, 157, 008, 134, 054, 218, 167, 025, 135, 161, 055, 025, 043, 145, 127, 224, 235, 205, 006, 037, 111, 211, 243, 138, 015, 223, 026, 168, 148, 227, 138, 163, 127, 084, 077, 109, 058, 181, 159, 134, 187, 128, 201, 031, 109, 169, 087, 034, 057, 011, 030, 033, 191, 023, 243, 183, 200, 223, 111, 198, 125, 157, 195, 106, 142, 171, 100, 092, 013, 138, 161, 062, 000, 056, 080, 007, 075, 174, 230, 139, 101, 182, 183, 055, 065, 196, 201, 053, 221, 234, 047, 023, 053, 093, 011, 043, 218, 002, 075, 198, 132, 173, 208, 162, 025, 183, 136, 179, 169, 073, 101, 078, 009, 203, 085, 007, 244, 021, 091, 025, 249, 050, 077, 200, 087, 081, 139, 094, 070, 167, 070, 015, 088, 251, 017, 093, 044, 027, 062, 038, 134, 186, 148, 062, 171, 243, 236, 224, 160, 174, 112, 106, 077, 056, 242, 113, 092, 138, 160, 182, 100, 009, 094, 041, 178, 006, 060, 209, 158, 229, 005, 002, 123, 249, 229, 114, 061, 095, 140, 064, 168, 082, 033, 162, 213, 098, 049, 248, 152, 015, 107, 047, 145, 089, 093, 047, 129, 030, 079, 075, 043, 108, 223, 223, 159, 244, 155, 034, 250, 174, 180, 216, 125, 130, 058, 131, 126, 143, 006, 209, 047, 135, 090, 016, 084, 143, 014, 065, 052, 069, 144, 201, 118, 166, 103, 209, 047, 111, 127, 248, 126, 181, 186, 209, 047, 052, 142, 049, 055, 123, 228, 236, 038, 068, 228, 055, 105, 139, 152, 198, 106, 196, 081, 030, 233, 187, 137, 008, 075, 202, 064, 052, 039, 246, 029, 130, 099, 006, 242, 213, 219, 225, 035, 051, 244, 083, 117, 129, 209, 134, 251, 042, 157, 010, 224, 236, 039, 158, 084, 131, 117, 225, 090, 244, 073, 107, 233, 124, 104, 153, 055, 069, 227, 114, 205, 025, 153, 229, 023, 123, 073, 182, 017, 100, 108, 208, 231, 123, 097, 065, 104, 191, 017, 096, 177, 149, 195, 133, 088, 206, 076, 052, 091, 222, 006, 145, 152, 244, 125, 253, 035, 007, 054, 007, 061, 145, 133, 233, 034, 046, 154, 218, 114, 144, 166, 165, 069, 151, 180, 048, 179, 040, 218, 142, 233, 176, 043, 205, 246, 134, 085, 135, 055, 097, 249, 201, 179, 103, 095, 217, 144, 109, 207, 142, 159, 039, 233, 056, 147, 138, 242, 103, 199, 199, 233, 243, 227, 231, 219, 139, 253, 253, 058, 230, 129, 082, 028, 241, 173, 173, 085, 226, 075, 068, 175, 205, 188, 057, 132, 121, 008, 110, 148, 164, 157, 003, 199, 131, 155, 085, 041, 142, 136, 166, 250, 133, 104, 030, 058, 227, 253, 093, 188, 013, 232, 109, 156, 046, 236, 114, 229, 069, 177, 041, 250, 225, 210, 242, 029, 174, 236, 135, 211, 206, 015, 191, 100, 190, 247, 253, 167, 079, 239, 163, 196, 047, 044, 080, 091, 090, 109, 184, 112, 208, 090, 237, 237, 116, 214, 170, 023, 168, 181, 119, 164, 215, 213, 085, 103, 250, 253, 161, 123, 019, 104, 191, 117, 109, 071, 103, 037, 081, 160, 040, 117, 131, 140, 137, 036, 179, 202, 059, 208, 083, 179, 194, 089, 023, 211, 205, 084, 010, 155, 255, 010, 183, 030, 176, 189, 182, 238, 138, 177, 250, 225, 216, 074, 009, 002, 238, 206, 144, 090, 133, 232, 007, 216, 041, 092, 052, 005, 123, 039, 112, 157, 014, 245, 068, 194, 123, 100, 034, 127, 040, 116, 181, 153, 216, 105, 053, 015, 220, 206, 250, 216, 064, 198, 043, 211, 129, 002, 142, 088, 248, 012, 046, 046, 194, 015, 134, 104, 162, 227, 169, 105, 062, 217, 125, 052, 179, 009, 107, 007, 106, 145, 017, 018, 153, 197, 014, 139, 182, 162, 047, 169, 047, 197, 048, 130, 029, 156, 140, 145, 068, 227, 029, 195, 029, 045, 160, 063, 149, 195, 124, 206, 023, 088, 086, 118, 110, 136, 192, 064, 026, 244, 057, 109, 098, 183, 171, 054, 155, 035, 124, 075, 148, 140, 145, 116, 107, 208, 168, 050, 056, 189, 184, 053, 095, 170, 134, 141, 125, 191, 224, 129, 165, 077, 252, 013, 008, 150, 096, 168, 033, 038, 236, 056, 006, 088, 154, 181, 105, 193, 009, 080, 133, 242, 019, 154, 005, 079, 216, 209, 181, 173, 217, 239, 095, 090, 215, 218, 221, 108, 010, 088, 129, 099, 184, 036, 214, 042, 206, 146, 179, 060, 206, 179, 253, 205, 147, 100, 115, 150, 159, 229, 071, 167, 193, 198, 131, 216, 239, 038, 141, 042, 173, 222, 023, 107, 141, 027, 163, 237, 111, 135, 117, 186, 168, 004, 151, 142, 133, 144, 012, 254, 062, 063, 136, 062, 139, 002, 202, 039, 061, 161, 190, 198, 028, 119, 110, 000, 212, 193, 006, 029, 055, 145, 207, 163, 181, 226, 114, 150, 125, 206, 100, 096, 045, 047, 043, 051, 083, 180, 010, 146, 060, 162, 127, 057, 002, 108, 120, 252, 151, 090, 217, 196, 081, 066, 075, 095, 059, 196, 190, 012, 206, 069, 230, 055, 109, 118, 064, 033, 187, 058, 089, 125, 071, 132, 015, 254, 058, 058, 253, 156, 010, 149, 158, 048, 042, 086, 160, 021, 066, 224, 135, 126, 048, 158, 013, 150, 045, 124, 009, 167, 253, 032, 033, 134, 080, 040, 072, 081, 231, 121, 201, 161, 071, 233, 031, 075, 217, 094, 086, 208, 019, 194, 230, 059, 109, 012, 088, 217, 208, 078, 154, 129, 051, 170, 067, 157, 093, 184, 064, 166, 072, 156, 097, 157, 222, 178, 098, 119, 051, 204, 218, 180, 239, 196, 089, 068, 195, 081, 105, 093, 044, 123, 215, 243, 085, 015, 043, 137, 181, 011, 019, 026, 002, 092, 110, 254, 144, 100, 034, 102, 230, 032, 004, 053, 108, 011, 234, 160, 228, 073, 102, 029, 025, 182, 106, 212, 129, 231, 239, 034, 165, 074, 084, 008, 231, 115, 028, 215, 192, 115, 064, 113, 204, 083, 115, 231, 195, 161, 175, 026, 067, 137, 149, 012, 051, 093, 216, 241, 077, 196, 221, 215, 206, 012, 002, 038, 016, 033, 201, 066, 204, 073, 054, 118, 049, 128, 236, 065, 230, 199, 046, 100, 219, 035, 198, 224, 107, 010, 218, 217, 165, 172, 195, 138, 190, 008, 172, 232, 059, 253, 163, 180, 009, 016, 014, 255, 146, 131, 092, 010, 241, 159, 221, 139, 121, 000, 131, 014, 003, 146, 122, 224, 002, 016, 230, 131, 178, 113, 238, 034, 214, 111, 050, 068, 168, 248, 139, 034, 166, 045, 041, 177, 177, 137, 102, 049, 072, 171, 212, 111, 234, 172, 030, 199, 024, 157, 185, 170, 023, 147, 058, 134, 188, 193, 023, 215, 105, 193, 207, 076, 208, 076, 217, 232, 254, 212, 254, 234, 234, 118, 023, 044, 240, 204, 134, 055, 158, 237, 192, 051, 245, 045, 156, 036, 098, 016, 180, 129, 158, 083, 155, 219, 119, 047, 014, 079, 216, 094, 066, 027, 095, 020, 218, 158, 244, 092, 025, 160, 242, 004, 200, 131, 038, 249, 088, 225, 185, 041, 048, 009, 205, 172, 210, 178, 003, 019, 188, 100, 013, 068, 244, 254, 221, 199, 079, 088, 211, 214, 191, 198, 240, 053, 129, 058, 000, 039, 076, 067, 035, 032, 150, 123, 218, 110, 049, 105, 068, 039, 001, 108, 152, 091, 242, 084, 058, 114, 199, 035, 090, 219, 209, 159, 071, 211, 187, 023, 145, 021, 111, 123, 139, 140, 113, 201, 224, 211, 029, 143, 068, 128, 107, 246, 009, 045, 134, 144, 019, 159, 180, 068, 182, 193, 176, 019, 121, 235, 025, 195, 130, 254, 133, 107, 255, 144, 035, 017, 105, 219, 033, 171, 207, 113, 246, 020, 202, 041, 155, 084, 067, 083, 229, 235, 188, 002, 109, 152, 114, 186, 224, 182, 055, 119, 211, 173, 041, 100, 174, 249, 098, 040, 172, 167, 191, 039, 147, 212, 120, 176, 163, 110, 085, 205, 132, 078, 199, 216, 162, 190, 250, 060, 167, 005, 036, 192, 121, 045, 113, 065, 244, 172, 110, 029, 101, 124, 085, 053, 116, 005, 127, 163, 001, 159, 175, 033, 056, 044, 210, 127, 015, 196, 196, 185, 003, 039, 153, 214, 107, 200, 186, 132, 010, 144, 047, 082, 022, 000, 139, 172, 150, 225, 039, 068, 168, 187, 027, 167, 192, 153, 246, 185, 088, 074, 055, 243, 037, 011, 214, 096, 204, 146, 049, 228, 225, 021, 076, 224, 034, 208, 035, 211, 074, 135, 096, 138, 141, 156, 220, 228, 206, 162, 069, 061, 043, 064, 146, 195, 110, 152, 248, 114, 105, 069, 044, 112, 229, 186, 104, 086, 026, 170, 169, 075, 152, 113, 172, 088, 117, 145, 209, 005, 089, 046, 231, 179, 219, 149, 196, 120, 002, 034, 235, 244, 158, 145, 167, 178, 203, 132, 207, 075, 141, 045, 038, 016, 025, 106, 064, 156, 206, 016, 158, 122, 234, 002, 238, 019, 051, 219, 142, 024, 103, 232, 168, 015, 224, 112, 224, 165, 162, 130, 036, 141, 039, 097, 184, 106, 132, 077, 009, 034, 088, 079, 145, 212, 218, 178, 044, 135, 044, 077, 248, 135, 042, 000, 083, 063, 135, 010, 088, 131, 249, 245, 025, 102, 058, 190, 194, 095, 121, 058, 060, 199, 191, 007, 019, 151, 005, 013, 225, 060, 248, 161, 159, 041, 023, 254, 224, 054, 140, 224, 069, 056, 097, 080, 195, 028, 098, 003, 132, 064, 209, 245, 094, 037, 018, 142, 062, 190, 018, 044, 018, 079, 154, 056, 111, 078, 111, 242, 037, 044, 075, 071, 245, 179, 002, 170, 075, 151, 196, 123, 068, 074, 237, 219, 229, 099, 096, 181, 075, 131, 174, 038, 062, 152, 143, 000, 078, 061, 086, 051, 029, 154, 215, 226, 105, 002, 139, 022, 160, 219, 129, 022, 132, 225, 162, 028, 250, 227, 184, 073, 224, 043, 079, 129, 002, 201, 106, 030, 123, 240, 099, 246, 088, 175, 187, 131, 253, 242, 209, 188, 227, 029, 012, 217, 170, 140, 118, 024, 093, 069, 220, 098, 094, 027, 007, 012, 034, 056, 169, 255, 046, 061, 100, 196, 101, 019, 223, 048, 057, 004, 033, 199, 005, 208, 019, 175, 011, 238, 163, 044, 038, 243, 229, 047, 205, 047, 017, 002, 209, 251, 020, 143, 248, 118, 155, 064, 013, 108, 086, 167, 079, 226, 054, 021, 163, 188, 055, 155, 163, 058, 178, 040, 165, 218, 203, 198, 110, 011, 217, 067, 035, 127, 191, 230, 224, 133, 118, 140, 066, 026, 107, 177, 173, 204, 238, 123, 173, 103, 163, 011, 201, 075, 229, 059, 217, 225, 034, 097, 050, 229, 078, 225, 064, 215, 054, 156, 058, 143, 041, 015, 163, 217, 201, 156, 083, 066, 039, 211, 152, 073, 236, 100, 006, 007, 175, 100, 208, 058, 242, 097, 128, 092, 070, 061, 057, 178, 121, 184, 236, 067, 219, 065, 009, 091, 248, 009, 119, 192, 158, 153, 012, 189, 129, 164, 248, 102, 086, 142, 064, 173, 121, 020, 229, 247, 247, 127, 036, 051, 111, 141, 150, 083, 221, 237, 133, 232, 081, 050, 068, 108, 155, 104, 142, 200, 142, 211, 148, 213, 202, 065, 121, 230, 114, 216, 108, 126, 042, 182, 033, 130, 186, 093, 082, 240, 107, 176, 171, 045, 082, 118, 149, 074, 186, 094, 191, 173, 056, 213, 034, 022, 063, 250, 187, 225, 054, 147, 211, 022, 232, 185, 003, 060, 111, 135, 126, 118, 050, 207, 140, 239, 166, 211, 230, 009, 082, 231, 227, 156, 173, 119, 199, 057, 080, 241, 083, 183, 159, 155, 027, 027, 241, 203, 224, 170, 198, 036, 108, 076, 217, 237, 070, 139, 171, 156, 201, 065, 215, 087, 090, 165, 181, 170, 242, 058, 245, 210, 063, 129, 255, 075, 184, 008, 132, 241, 098, 180, 249, 230, 001, 199, 135, 108, 019, 049, 031, 020, 003, 095, 047, 109, 034, 192, 006, 026, 035, 082, 128, 131, 158, 223, 208, 166, 154, 189, 215, 051, 165, 066, 158, 222, 002, 096, 209, 006, 224, 048, 231, 196, 095, 191, 043, 044, 250, 027, 115, 006, 238, 242, 161, 034, 037, 218, 184, 131, 112, 074, 188, 121, 021, 005, 106, 106, 000, 164, 020, 111, 128, 084, 227, 074, 181, 113, 234, 229, 163, 063, 216, 240, 228, 140, 191, 029, 029, 020, 070, 168, 148, 194, 190, 056, 141, 230, 183, 043, 078, 222, 054, 080, 203, 121, 206, 071, 254, 156, 187, 121, 109, 142, 033, 120, 000, 047, 026, 128, 061, 114, 057, 188, 072, 197, 030, 091, 018, 167, 165, 022, 071, 031, 019, 076, 052, 213, 219, 217, 017, 203, 205, 229, 020, 104, 079, 219, 068, 014, 084, 137, 187, 087, 079, 036, 167, 041, 117, 110, 200, 148, 080, 233, 040, 033, 102, 120, 155, 215, 136, 141, 161, 086, 246, 097, 032, 192, 140, 037, 173, 034, 124, 175, 234, 224, 201, 100, 144, 029, 105, 050, 132, 079, 174, 242, 196, 076, 039, 123, 071, 203, 238, 070, 199, 038, 137, 013, 166, 043, 054, 123, 136, 121, 066, 124, 079, 062, 210, 249, 137, 198, 050, 171, 179, 233, 000, 015, 108, 164, 093, 214, 082, 134, 028, 021, 112, 104, 193, 156, 083, 183, 215, 173, 079, 026, 031, 140, 199, 246, 011, 004, 181, 033, 130, 115, 210, 029, 092, 166, 077, 245, 234, 120, 052, 184, 117, 059, 191, 178, 223, 176, 221, 066, 099, 245, 228, 094, 245, 209, 211, 167, 145, 054, 084, 065, 002, 021, 013, 011, 245, 167, 145, 134, 205, 227, 033, 000, 060, 066, 007, 151, 207, 031, 105, 242, 088, 050, 022, 215, 163, 143, 245, 108, 044, 140, 032, 109, 131, 111, 192, 076, 071, 230, 075, 047, 110, 017, 147, 010, 251, 251, 242, 183, 095, 092, 141, 204, 239, 056, 018, 109, 048, 226, 190, 168, 118, 149, 215, 154, 166, 185, 198, 245, 118, 241, 019, 114, 170, 057, 126, 063, 113, 107, 245, 122, 254, 114, 126, 061, 038, 214, 110, 149, 117, 209, 246, 253, 039, 056, 240, 153, 038, 126, 146, 205, 043, 009, 063, 163, 203, 178, 111, 244, 227, 053, 189, 190, 222, 002, 018, 195, 165, 225, 179, 107, 036, 039, 167, 127, 248, 111, });
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
            public class TextInput_balloon_css
            {
                public static readonly string TextContent =
        @".balloon {
    display: inline-block;
    width: 215px;
    padding: 10px 0 10px 15px;
    font-family: ""Open Sans"", sans;
    font-weight: 400;
    color: #377D6A;
    background: #efefef;
    border: 0;
    border-radius: 3px;
    outline: 0;
    text-indent: 60px;
    transition: all .3s ease-in-out;
}

    .balloon::-webkit-input-placeholder {
        color: #efefef;
        text-indent: 0;
        font-weight: 300;
    }

    .balloon + label {
        display: inline-block;
        position: absolute;
        top: 8px;
        left: 0;
        bottom: 8px;
        padding: 5px 15px;
        color: #032429;
        font-size: 11px;
        font-weight: 700;
        text-transform: uppercase;
        text-shadow: 0 1px 0 rgba(19, 74, 70, 0);
        transition: all .3s ease-in-out;
        border-radius: 3px;
        background: rgba(122, 184, 147, 0);
    }

        .balloon + label:after {
            position: absolute;
            content: """";
            width: 0;
            height: 0;
            top: 100%;
            left: 50%;
            margin-left: -3px;
            border-left: 3px solid transparent;
            border-right: 3px solid transparent;
            border-top: 3px solid rgba(122, 184, 147, 0);
            transition: all .3s ease-in-out;
        }

    .balloon:focus,
    .balloon:active {
        color: #377D6A;
        text-indent: 0;
        background: #fff;
    }

        .balloon:focus::-webkit-input-placeholder,
        .balloon:active::-webkit-input-placeholder {
            color: #aaa;
        }

        .balloon:focus + label,
        .balloon:active + label {
            color: #fff;
            text-shadow: 0 1px 0 rgba(19, 74, 70, 0.4);
            background: #7ab893;
            transform: translateY(-40px);
        }

            .balloon:focus + label:after,
            .balloon:active + label:after {
                border-top: 4px solid #7ab893;
            }
";

            }
            public class TextInput_Container_css
            {
                public static readonly string TextContent =
        @".TextInput_Container {
    position: relative;
    margin: 30px 10px;
    display: inline-block;
}
";

            }
            public class w3_css
            {
                public static readonly string TextContent =
        @"/* W3.CSS 4.13 June 2019 by Jan Egil and Borge Refsnes */
html{box-sizing:border-box}*,*:before,*:after{box-sizing:inherit}
/* Extract from normalize.css by Nicolas Gallagher and Jonathan Neal git.io/normalize */
html{-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}body{margin:0}
article,aside,details,figcaption,figure,footer,header,main,menu,nav,section{display:block}summary{display:list-item}
audio,canvas,progress,video{display:inline-block}progress{vertical-align:baseline}
audio:not([controls]){display:none;height:0}[hidden],template{display:none}
a{background-color:transparent}a:active,a:hover{outline-width:0}
abbr[title]{border-bottom:none;text-decoration:underline;text-decoration:underline dotted}
b,strong{font-weight:bolder}dfn{font-style:italic}mark{background:#ff0;color:#000}
small{font-size:80%}sub,sup{font-size:75%;line-height:0;position:relative;vertical-align:baseline}
sub{bottom:-0.25em}sup{top:-0.5em}figure{margin:1em 40px}img{border-style:none}
code,kbd,pre,samp{font-family:monospace,monospace;font-size:1em}hr{box-sizing:content-box;height:0;overflow:visible}
button,input,select,textarea,optgroup{font:inherit;margin:0}optgroup{font-weight:bold}
button,input{overflow:visible}button,select{text-transform:none}
button,[type=button],[type=reset],[type=submit]{-webkit-appearance:button}
button::-moz-focus-inner,[type=button]::-moz-focus-inner,[type=reset]::-moz-focus-inner,[type=submit]::-moz-focus-inner{border-style:none;padding:0}
button:-moz-focusring,[type=button]:-moz-focusring,[type=reset]:-moz-focusring,[type=submit]:-moz-focusring{outline:1px dotted ButtonText}
fieldset{border:1px solid #c0c0c0;margin:0 2px;padding:.35em .625em .75em}
legend{color:inherit;display:table;max-width:100%;padding:0;white-space:normal}textarea{overflow:auto}
[type=checkbox],[type=radio]{padding:0}
[type=number]::-webkit-inner-spin-button,[type=number]::-webkit-outer-spin-button{height:auto}
[type=search]{-webkit-appearance:textfield;outline-offset:-2px}
[type=search]::-webkit-search-decoration{-webkit-appearance:none}
::-webkit-file-upload-button{-webkit-appearance:button;font:inherit}
/* End extract */
html,body{font-family:Verdana,sans-serif;font-size:15px;line-height:1.5}html{overflow-x:hidden}
h1{font-size:36px}h2{font-size:30px}h3{font-size:24px}h4{font-size:20px}h5{font-size:18px}h6{font-size:16px}.w3-serif{font-family:serif}
h1,h2,h3,h4,h5,h6{font-family:""Segoe UI"",Arial,sans-serif;font-weight:400;margin:10px 0}.w3-wide{letter-spacing:4px}
hr{border:0;border-top:1px solid #eee;margin:20px 0}
.w3-image{max-width:100%;height:auto}img{vertical-align:middle}a{color:inherit}
.w3-table,.w3-table-all{border-collapse:collapse;border-spacing:0;width:100%;display:table}.w3-table-all{border:1px solid #ccc}
.w3-bordered tr,.w3-table-all tr{border-bottom:1px solid #ddd}.w3-striped tbody tr:nth-child(even){background-color:#f1f1f1}
.w3-table-all tr:nth-child(odd){background-color:#fff}.w3-table-all tr:nth-child(even){background-color:#f1f1f1}
.w3-hoverable tbody tr:hover,.w3-ul.w3-hoverable li:hover{background-color:#ccc}.w3-centered tr th,.w3-centered tr td{text-align:center}
.w3-table td,.w3-table th,.w3-table-all td,.w3-table-all th{padding:8px 8px;display:table-cell;text-align:left;vertical-align:top}
.w3-table th:first-child,.w3-table td:first-child,.w3-table-all th:first-child,.w3-table-all td:first-child{padding-left:16px}
.w3-btn,.w3-button{border:none;display:inline-block;padding:8px 16px;vertical-align:middle;overflow:hidden;text-decoration:none;color:inherit;background-color:inherit;text-align:center;cursor:pointer;white-space:nowrap}
.w3-btn:hover{box-shadow:0 8px 16px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19)}
.w3-btn,.w3-button{-webkit-touch-callout:none;-webkit-user-select:none;-khtml-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}   
.w3-disabled,.w3-btn:disabled,.w3-button:disabled{cursor:not-allowed;opacity:0.3}.w3-disabled *,:disabled *{pointer-events:none}
.w3-btn.w3-disabled:hover,.w3-btn:disabled:hover{box-shadow:none}
.w3-badge,.w3-tag{background-color:#000;color:#fff;display:inline-block;padding-left:8px;padding-right:8px;text-align:center}.w3-badge{border-radius:50%}
.w3-ul{list-style-type:none;padding:0;margin:0}.w3-ul li{padding:8px 16px;border-bottom:1px solid #ddd}.w3-ul li:last-child{border-bottom:none}
.w3-tooltip,.w3-display-container{position:relative}.w3-tooltip .w3-text{display:none}.w3-tooltip:hover .w3-text{display:inline-block}
.w3-ripple:active{opacity:0.5}.w3-ripple{transition:opacity 0s}
.w3-input{padding:8px;display:block;border:none;border-bottom:1px solid #ccc;width:100%}
.w3-select{padding:9px 0;width:100%;border:none;border-bottom:1px solid #ccc}
.w3-dropdown-click,.w3-dropdown-hover{position:relative;display:inline-block;cursor:pointer}
.w3-dropdown-hover:hover .w3-dropdown-content{display:block}
.w3-dropdown-hover:first-child,.w3-dropdown-click:hover{background-color:#ccc;color:#000}
.w3-dropdown-hover:hover > .w3-button:first-child,.w3-dropdown-click:hover > .w3-button:first-child{background-color:#ccc;color:#000}
.w3-dropdown-content{cursor:auto;color:#000;background-color:#fff;display:none;position:absolute;min-width:160px;margin:0;padding:0;z-index:1}
.w3-check,.w3-radio{width:24px;height:24px;position:relative;top:6px}
.w3-sidebar{height:100%;width:200px;background-color:#fff;position:fixed!important;z-index:1;overflow:auto}
.w3-bar-block .w3-dropdown-hover,.w3-bar-block .w3-dropdown-click{width:100%}
.w3-bar-block .w3-dropdown-hover .w3-dropdown-content,.w3-bar-block .w3-dropdown-click .w3-dropdown-content{min-width:100%}
.w3-bar-block .w3-dropdown-hover .w3-button,.w3-bar-block .w3-dropdown-click .w3-button{width:100%;text-align:left;padding:8px 16px}
.w3-main,#main{transition:margin-left .4s}
.w3-modal{z-index:3;display:none;padding-top:100px;position:fixed;left:0;top:0;width:100%;height:100%;overflow:auto;background-color:rgb(0,0,0);background-color:rgba(0,0,0,0.4)}
.w3-modal-content{margin:auto;background-color:#fff;position:relative;padding:0;outline:0;width:600px}
.w3-bar{width:100%;overflow:hidden}.w3-center .w3-bar{display:inline-block;width:auto}
.w3-bar .w3-bar-item{padding:8px 16px;float:left;width:auto;border:none;display:block;outline:0}
.w3-bar .w3-dropdown-hover,.w3-bar .w3-dropdown-click{position:static;float:left}
.w3-bar .w3-button{white-space:normal}
.w3-bar-block .w3-bar-item{width:100%;display:block;padding:8px 16px;text-align:left;border:none;white-space:normal;float:none;outline:0}
.w3-bar-block.w3-center .w3-bar-item{text-align:center}.w3-block{display:block;width:100%}
.w3-responsive{display:block;overflow-x:auto}
.w3-container:after,.w3-container:before,.w3-panel:after,.w3-panel:before,.w3-row:after,.w3-row:before,.w3-row-padding:after,.w3-row-padding:before,
.w3-cell-row:before,.w3-cell-row:after,.w3-clear:after,.w3-clear:before,.w3-bar:before,.w3-bar:after{content:"""";display:table;clear:both}
.w3-col,.w3-half,.w3-third,.w3-twothird,.w3-threequarter,.w3-quarter{float:left;width:100%}
.w3-col.s1{width:8.33333%}.w3-col.s2{width:16.66666%}.w3-col.s3{width:24.99999%}.w3-col.s4{width:33.33333%}
.w3-col.s5{width:41.66666%}.w3-col.s6{width:49.99999%}.w3-col.s7{width:58.33333%}.w3-col.s8{width:66.66666%}
.w3-col.s9{width:74.99999%}.w3-col.s10{width:83.33333%}.w3-col.s11{width:91.66666%}.w3-col.s12{width:99.99999%}
@media (min-width:601px){.w3-col.m1{width:8.33333%}.w3-col.m2{width:16.66666%}.w3-col.m3,.w3-quarter{width:24.99999%}.w3-col.m4,.w3-third{width:33.33333%}
.w3-col.m5{width:41.66666%}.w3-col.m6,.w3-half{width:49.99999%}.w3-col.m7{width:58.33333%}.w3-col.m8,.w3-twothird{width:66.66666%}
.w3-col.m9,.w3-threequarter{width:74.99999%}.w3-col.m10{width:83.33333%}.w3-col.m11{width:91.66666%}.w3-col.m12{width:99.99999%}}
@media (min-width:993px){.w3-col.l1{width:8.33333%}.w3-col.l2{width:16.66666%}.w3-col.l3{width:24.99999%}.w3-col.l4{width:33.33333%}
.w3-col.l5{width:41.66666%}.w3-col.l6{width:49.99999%}.w3-col.l7{width:58.33333%}.w3-col.l8{width:66.66666%}
.w3-col.l9{width:74.99999%}.w3-col.l10{width:83.33333%}.w3-col.l11{width:91.66666%}.w3-col.l12{width:99.99999%}}
.w3-rest{overflow:hidden}.w3-stretch{margin-left:-16px;margin-right:-16px}
.w3-content,.w3-auto{margin-left:auto;margin-right:auto}.w3-content{max-width:980px}.w3-auto{max-width:1140px}
.w3-cell-row{display:table;width:100%}.w3-cell{display:table-cell}
.w3-cell-top{vertical-align:top}.w3-cell-middle{vertical-align:middle}.w3-cell-bottom{vertical-align:bottom}
.w3-hide{display:none!important}.w3-show-block,.w3-show{display:block!important}.w3-show-inline-block{display:inline-block!important}
@media (max-width:1205px){.w3-auto{max-width:95%}}
@media (max-width:600px){.w3-modal-content{margin:0 10px;width:auto!important}.w3-modal{padding-top:30px}
.w3-dropdown-hover.w3-mobile .w3-dropdown-content,.w3-dropdown-click.w3-mobile .w3-dropdown-content{position:relative}	
.w3-hide-small{display:none!important}.w3-mobile{display:block;width:100%!important}.w3-bar-item.w3-mobile,.w3-dropdown-hover.w3-mobile,.w3-dropdown-click.w3-mobile{text-align:center}
.w3-dropdown-hover.w3-mobile,.w3-dropdown-hover.w3-mobile .w3-btn,.w3-dropdown-hover.w3-mobile .w3-button,.w3-dropdown-click.w3-mobile,.w3-dropdown-click.w3-mobile .w3-btn,.w3-dropdown-click.w3-mobile .w3-button{width:100%}}
@media (max-width:768px){.w3-modal-content{width:500px}.w3-modal{padding-top:50px}}
@media (min-width:993px){.w3-modal-content{width:900px}.w3-hide-large{display:none!important}.w3-sidebar.w3-collapse{display:block!important}}
@media (max-width:992px) and (min-width:601px){.w3-hide-medium{display:none!important}}
@media (max-width:992px){.w3-sidebar.w3-collapse{display:none}.w3-main{margin-left:0!important;margin-right:0!important}.w3-auto{max-width:100%}}
.w3-top,.w3-bottom{position:fixed;width:100%;z-index:1}.w3-top{top:0}.w3-bottom{bottom:0}
.w3-overlay{position:fixed;display:none;width:100%;height:100%;top:0;left:0;right:0;bottom:0;background-color:rgba(0,0,0,0.5);z-index:2}
.w3-display-topleft{position:absolute;left:0;top:0}.w3-display-topright{position:absolute;right:0;top:0}
.w3-display-bottomleft{position:absolute;left:0;bottom:0}.w3-display-bottomright{position:absolute;right:0;bottom:0}
.w3-display-middle{position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);-ms-transform:translate(-50%,-50%)}
.w3-display-left{position:absolute;top:50%;left:0%;transform:translate(0%,-50%);-ms-transform:translate(-0%,-50%)}
.w3-display-right{position:absolute;top:50%;right:0%;transform:translate(0%,-50%);-ms-transform:translate(0%,-50%)}
.w3-display-topmiddle{position:absolute;left:50%;top:0;transform:translate(-50%,0%);-ms-transform:translate(-50%,0%)}
.w3-display-bottommiddle{position:absolute;left:50%;bottom:0;transform:translate(-50%,0%);-ms-transform:translate(-50%,0%)}
.w3-display-container:hover .w3-display-hover{display:block}.w3-display-container:hover span.w3-display-hover{display:inline-block}.w3-display-hover{display:none}
.w3-display-position{position:absolute}
.w3-circle{border-radius:50%}
.w3-round-small{border-radius:2px}.w3-round,.w3-round-medium{border-radius:4px}.w3-round-large{border-radius:8px}.w3-round-xlarge{border-radius:16px}.w3-round-xxlarge{border-radius:32px}
.w3-row-padding,.w3-row-padding>.w3-half,.w3-row-padding>.w3-third,.w3-row-padding>.w3-twothird,.w3-row-padding>.w3-threequarter,.w3-row-padding>.w3-quarter,.w3-row-padding>.w3-col{padding:0 8px}
.w3-container,.w3-panel{padding:0.01em 16px}.w3-panel{margin-top:16px;margin-bottom:16px}
.w3-code,.w3-codespan{font-family:Consolas,""courier new"";font-size:16px}
.w3-code{width:auto;background-color:#fff;padding:8px 12px;border-left:4px solid #4CAF50;word-wrap:break-word}
.w3-codespan{color:crimson;background-color:#f1f1f1;padding-left:4px;padding-right:4px;font-size:110%}
.w3-card,.w3-card-2{box-shadow:0 2px 5px 0 rgba(0,0,0,0.16),0 2px 10px 0 rgba(0,0,0,0.12)}
.w3-card-4,.w3-hover-shadow:hover{box-shadow:0 4px 10px 0 rgba(0,0,0,0.2),0 4px 20px 0 rgba(0,0,0,0.19)}
.w3-spin{animation:w3-spin 2s infinite linear}@keyframes w3-spin{0%{transform:rotate(0deg)}100%{transform:rotate(359deg)}}
.w3-animate-fading{animation:fading 10s infinite}@keyframes fading{0%{opacity:0}50%{opacity:1}100%{opacity:0}}
.w3-animate-opacity{animation:opac 0.8s}@keyframes opac{from{opacity:0} to{opacity:1}}
.w3-animate-top{position:relative;animation:animatetop 0.4s}@keyframes animatetop{from{top:-300px;opacity:0} to{top:0;opacity:1}}
.w3-animate-left{position:relative;animation:animateleft 0.4s}@keyframes animateleft{from{left:-300px;opacity:0} to{left:0;opacity:1}}
.w3-animate-right{position:relative;animation:animateright 0.4s}@keyframes animateright{from{right:-300px;opacity:0} to{right:0;opacity:1}}
.w3-animate-bottom{position:relative;animation:animatebottom 0.4s}@keyframes animatebottom{from{bottom:-300px;opacity:0} to{bottom:0;opacity:1}}
.w3-animate-zoom {animation:animatezoom 0.6s}@keyframes animatezoom{from{transform:scale(0)} to{transform:scale(1)}}
.w3-animate-input{transition:width 0.4s ease-in-out}.w3-animate-input:focus{width:100%!important}
.w3-opacity,.w3-hover-opacity:hover{opacity:0.60}.w3-opacity-off,.w3-hover-opacity-off:hover{opacity:1}
.w3-opacity-max{opacity:0.25}.w3-opacity-min{opacity:0.75}
.w3-greyscale-max,.w3-grayscale-max,.w3-hover-greyscale:hover,.w3-hover-grayscale:hover{filter:grayscale(100%)}
.w3-greyscale,.w3-grayscale{filter:grayscale(75%)}.w3-greyscale-min,.w3-grayscale-min{filter:grayscale(50%)}
.w3-sepia{filter:sepia(75%)}.w3-sepia-max,.w3-hover-sepia:hover{filter:sepia(100%)}.w3-sepia-min{filter:sepia(50%)}
.w3-tiny{font-size:10px!important}.w3-small{font-size:12px!important}.w3-medium{font-size:15px!important}.w3-large{font-size:18px!important}
.w3-xlarge{font-size:24px!important}.w3-xxlarge{font-size:36px!important}.w3-xxxlarge{font-size:48px!important}.w3-jumbo{font-size:64px!important}
.w3-left-align{text-align:left!important}.w3-right-align{text-align:right!important}.w3-justify{text-align:justify!important}.w3-center{text-align:center!important}
.w3-border-0{border:0!important}.w3-border{border:1px solid #ccc!important}
.w3-border-top{border-top:1px solid #ccc!important}.w3-border-bottom{border-bottom:1px solid #ccc!important}
.w3-border-left{border-left:1px solid #ccc!important}.w3-border-right{border-right:1px solid #ccc!important}
.w3-topbar{border-top:6px solid #ccc!important}.w3-bottombar{border-bottom:6px solid #ccc!important}
.w3-leftbar{border-left:6px solid #ccc!important}.w3-rightbar{border-right:6px solid #ccc!important}
.w3-section,.w3-code{margin-top:16px!important;margin-bottom:16px!important}
.w3-margin{margin:16px!important}.w3-margin-top{margin-top:16px!important}.w3-margin-bottom{margin-bottom:16px!important}
.w3-margin-left{margin-left:16px!important}.w3-margin-right{margin-right:16px!important}
.w3-padding-small{padding:4px 8px!important}.w3-padding{padding:8px 16px!important}.w3-padding-large{padding:12px 24px!important}
.w3-padding-16{padding-top:16px!important;padding-bottom:16px!important}.w3-padding-24{padding-top:24px!important;padding-bottom:24px!important}
.w3-padding-32{padding-top:32px!important;padding-bottom:32px!important}.w3-padding-48{padding-top:48px!important;padding-bottom:48px!important}
.w3-padding-64{padding-top:64px!important;padding-bottom:64px!important}
.w3-left{float:left!important}.w3-right{float:right!important}
.w3-button:hover{color:#000!important;background-color:#ccc!important}
.w3-transparent,.w3-hover-none:hover{background-color:transparent!important}
.w3-hover-none:hover{box-shadow:none!important}
/* Colors */
.w3-amber,.w3-hover-amber:hover{color:#000!important;background-color:#ffc107!important}
.w3-aqua,.w3-hover-aqua:hover{color:#000!important;background-color:#00ffff!important}
.w3-blue,.w3-hover-blue:hover{color:#fff!important;background-color:#2196F3!important}
.w3-light-blue,.w3-hover-light-blue:hover{color:#000!important;background-color:#87CEEB!important}
.w3-brown,.w3-hover-brown:hover{color:#fff!important;background-color:#795548!important}
.w3-cyan,.w3-hover-cyan:hover{color:#000!important;background-color:#00bcd4!important}
.w3-blue-grey,.w3-hover-blue-grey:hover,.w3-blue-gray,.w3-hover-blue-gray:hover{color:#fff!important;background-color:#607d8b!important}
.w3-green,.w3-hover-green:hover{color:#fff!important;background-color:#4CAF50!important}
.w3-light-green,.w3-hover-light-green:hover{color:#000!important;background-color:#8bc34a!important}
.w3-indigo,.w3-hover-indigo:hover{color:#fff!important;background-color:#3f51b5!important}
.w3-khaki,.w3-hover-khaki:hover{color:#000!important;background-color:#f0e68c!important}
.w3-lime,.w3-hover-lime:hover{color:#000!important;background-color:#cddc39!important}
.w3-orange,.w3-hover-orange:hover{color:#000!important;background-color:#ff9800!important}
.w3-deep-orange,.w3-hover-deep-orange:hover{color:#fff!important;background-color:#ff5722!important}
.w3-pink,.w3-hover-pink:hover{color:#fff!important;background-color:#e91e63!important}
.w3-purple,.w3-hover-purple:hover{color:#fff!important;background-color:#9c27b0!important}
.w3-deep-purple,.w3-hover-deep-purple:hover{color:#fff!important;background-color:#673ab7!important}
.w3-red,.w3-hover-red:hover{color:#fff!important;background-color:#f44336!important}
.w3-sand,.w3-hover-sand:hover{color:#000!important;background-color:#fdf5e6!important}
.w3-teal,.w3-hover-teal:hover{color:#fff!important;background-color:#009688!important}
.w3-yellow,.w3-hover-yellow:hover{color:#000!important;background-color:#ffeb3b!important}
.w3-white,.w3-hover-white:hover{color:#000!important;background-color:#fff!important}
.w3-black,.w3-hover-black:hover{color:#fff!important;background-color:#000!important}
.w3-grey,.w3-hover-grey:hover,.w3-gray,.w3-hover-gray:hover{color:#000!important;background-color:#9e9e9e!important}
.w3-light-grey,.w3-hover-light-grey:hover,.w3-light-gray,.w3-hover-light-gray:hover{color:#000!important;background-color:#f1f1f1!important}
.w3-dark-grey,.w3-hover-dark-grey:hover,.w3-dark-gray,.w3-hover-dark-gray:hover{color:#fff!important;background-color:#616161!important}
.w3-pale-red,.w3-hover-pale-red:hover{color:#000!important;background-color:#ffdddd!important}
.w3-pale-green,.w3-hover-pale-green:hover{color:#000!important;background-color:#ddffdd!important}
.w3-pale-yellow,.w3-hover-pale-yellow:hover{color:#000!important;background-color:#ffffcc!important}
.w3-pale-blue,.w3-hover-pale-blue:hover{color:#000!important;background-color:#ddffff!important}
.w3-text-amber,.w3-hover-text-amber:hover{color:#ffc107!important}
.w3-text-aqua,.w3-hover-text-aqua:hover{color:#00ffff!important}
.w3-text-blue,.w3-hover-text-blue:hover{color:#2196F3!important}
.w3-text-light-blue,.w3-hover-text-light-blue:hover{color:#87CEEB!important}
.w3-text-brown,.w3-hover-text-brown:hover{color:#795548!important}
.w3-text-cyan,.w3-hover-text-cyan:hover{color:#00bcd4!important}
.w3-text-blue-grey,.w3-hover-text-blue-grey:hover,.w3-text-blue-gray,.w3-hover-text-blue-gray:hover{color:#607d8b!important}
.w3-text-green,.w3-hover-text-green:hover{color:#4CAF50!important}
.w3-text-light-green,.w3-hover-text-light-green:hover{color:#8bc34a!important}
.w3-text-indigo,.w3-hover-text-indigo:hover{color:#3f51b5!important}
.w3-text-khaki,.w3-hover-text-khaki:hover{color:#b4aa50!important}
.w3-text-lime,.w3-hover-text-lime:hover{color:#cddc39!important}
.w3-text-orange,.w3-hover-text-orange:hover{color:#ff9800!important}
.w3-text-deep-orange,.w3-hover-text-deep-orange:hover{color:#ff5722!important}
.w3-text-pink,.w3-hover-text-pink:hover{color:#e91e63!important}
.w3-text-purple,.w3-hover-text-purple:hover{color:#9c27b0!important}
.w3-text-deep-purple,.w3-hover-text-deep-purple:hover{color:#673ab7!important}
.w3-text-red,.w3-hover-text-red:hover{color:#f44336!important}
.w3-text-sand,.w3-hover-text-sand:hover{color:#fdf5e6!important}
.w3-text-teal,.w3-hover-text-teal:hover{color:#009688!important}
.w3-text-yellow,.w3-hover-text-yellow:hover{color:#d2be0e!important}
.w3-text-white,.w3-hover-text-white:hover{color:#fff!important}
.w3-text-black,.w3-hover-text-black:hover{color:#000!important}
.w3-text-grey,.w3-hover-text-grey:hover,.w3-text-gray,.w3-hover-text-gray:hover{color:#757575!important}
.w3-text-light-grey,.w3-hover-text-light-grey:hover,.w3-text-light-gray,.w3-hover-text-light-gray:hover{color:#f1f1f1!important}
.w3-text-dark-grey,.w3-hover-text-dark-grey:hover,.w3-text-dark-gray,.w3-hover-text-dark-gray:hover{color:#3a3a3a!important}
.w3-border-amber,.w3-hover-border-amber:hover{border-color:#ffc107!important}
.w3-border-aqua,.w3-hover-border-aqua:hover{border-color:#00ffff!important}
.w3-border-blue,.w3-hover-border-blue:hover{border-color:#2196F3!important}
.w3-border-light-blue,.w3-hover-border-light-blue:hover{border-color:#87CEEB!important}
.w3-border-brown,.w3-hover-border-brown:hover{border-color:#795548!important}
.w3-border-cyan,.w3-hover-border-cyan:hover{border-color:#00bcd4!important}
.w3-border-blue-grey,.w3-hover-border-blue-grey:hover,.w3-border-blue-gray,.w3-hover-border-blue-gray:hover{border-color:#607d8b!important}
.w3-border-green,.w3-hover-border-green:hover{border-color:#4CAF50!important}
.w3-border-light-green,.w3-hover-border-light-green:hover{border-color:#8bc34a!important}
.w3-border-indigo,.w3-hover-border-indigo:hover{border-color:#3f51b5!important}
.w3-border-khaki,.w3-hover-border-khaki:hover{border-color:#f0e68c!important}
.w3-border-lime,.w3-hover-border-lime:hover{border-color:#cddc39!important}
.w3-border-orange,.w3-hover-border-orange:hover{border-color:#ff9800!important}
.w3-border-deep-orange,.w3-hover-border-deep-orange:hover{border-color:#ff5722!important}
.w3-border-pink,.w3-hover-border-pink:hover{border-color:#e91e63!important}
.w3-border-purple,.w3-hover-border-purple:hover{border-color:#9c27b0!important}
.w3-border-deep-purple,.w3-hover-border-deep-purple:hover{border-color:#673ab7!important}
.w3-border-red,.w3-hover-border-red:hover{border-color:#f44336!important}
.w3-border-sand,.w3-hover-border-sand:hover{border-color:#fdf5e6!important}
.w3-border-teal,.w3-hover-border-teal:hover{border-color:#009688!important}
.w3-border-yellow,.w3-hover-border-yellow:hover{border-color:#ffeb3b!important}
.w3-border-white,.w3-hover-border-white:hover{border-color:#fff!important}
.w3-border-black,.w3-hover-border-black:hover{border-color:#000!important}
.w3-border-grey,.w3-hover-border-grey:hover,.w3-border-gray,.w3-hover-border-gray:hover{border-color:#9e9e9e!important}
.w3-border-light-grey,.w3-hover-border-light-grey:hover,.w3-border-light-gray,.w3-hover-border-light-gray:hover{border-color:#f1f1f1!important}
.w3-border-dark-grey,.w3-hover-border-dark-grey:hover,.w3-border-dark-gray,.w3-hover-border-dark-gray:hover{border-color:#616161!important}
.w3-border-pale-red,.w3-hover-border-pale-red:hover{border-color:#ffe7e7!important}.w3-border-pale-green,.w3-hover-border-pale-green:hover{border-color:#e7ffe7!important}
.w3-border-pale-yellow,.w3-hover-border-pale-yellow:hover{border-color:#ffffcc!important}.w3-border-pale-blue,.w3-hover-border-pale-blue:hover{border-color:#e7ffff!important}";

            }

        }

    }
}

