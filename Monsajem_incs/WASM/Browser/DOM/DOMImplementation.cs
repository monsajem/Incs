using Microsoft.JSInterop;

namespace WebAssembly.Browser.DOM
{



    [Export("DOMImplementation", typeof(IJSInProcessObjectReference))]
    public sealed class DOMImplementation : DOMObject
    {
        internal DOMImplementation(IJSInProcessObjectReference handle) : base(handle) { }

        [Export("createDocument")]
        public Document CreateDocument(string namespaceURI, string qualifiedName, DocumentType doctype)
        {
            return InvokeMethod<Document>("createDocument", namespaceURI, qualifiedName, doctype);
        }

        [Export("createDocumentType")]
        public DocumentType CreateDocumentType(string qualifiedName, string publicId, string systemId)
        {
            return InvokeMethod<DocumentType>("createDocumentType", qualifiedName, publicId, systemId);
        }

        [Export("createHTMLDocument")]
        public Document CreateHTMLDocument(string title)
        {
            return InvokeMethod<Document>("createHTMLDocument", title);
        }

    }
}