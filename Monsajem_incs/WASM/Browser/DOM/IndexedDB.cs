using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;

namespace WebAssembly.Browser.DOM
{

    [Export("indexedDB", typeof(JSObject))]
    public static class IndexedDB
    {
        private static JSObject Obj = ((Func<JSObject>)(() => 
        {
            return (JSObject) Runtime.GetGlobalObject("indexedDB");
        }))();

        [Export("open")]
        public async static Task<IDBDatabase> Open(
            string Name,int Version,Func<IDBDatabase,Task> OnUpgradeNeeded)
        {
            IDBDatabase Result=null;
            var RQ = new IDBRequest((JSObject)Obj.Invoke("open", Name,Version));
            var UpgradeNeeded = new Task(() => { });
            Action<JSObject> _OnUpgradeNeeded = async (c) =>
            {
                Result = new IDBDatabase((JSObject)RQ.Result);
                await OnUpgradeNeeded(Result);
                UpgradeNeeded.Start();
            };
            RQ.OnUpgradeNeeded += _OnUpgradeNeeded;
            await RQ.MakeTask();
            Result = new IDBDatabase((JSObject)RQ.Result);
            if (Result.Version != Version)
                await UpgradeNeeded;
            RQ.OnUpgradeNeeded -= _OnUpgradeNeeded;
            return Result;
        }

        public static Task DeleteDatabase(string Name)
        {
            return new IDBRequest((JSObject)Obj.Invoke("deleteDatabase",Name)).MakeTask();
        }
    }

    [Export("IDBDatabase", typeof(JSObject))]
    public class IDBDatabase:DOMObject
    {
        public IDBDatabase(JSObject obj) :base(obj)
        {
        }

        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }

        [Export("version")]
        public int Version { get => GetProperty<int>("version"); }

        [Export("createObjectStore")]
        public IDBObjectStore CreateObjectStore(string Name) => 
            InvokeMethod<IDBObjectStore>("createObjectStore",Name);

        [Export("createObjectStore")]
        public IDBObjectStore CreateObjectStore(string Name,object options) =>
            InvokeMethod<IDBObjectStore>("createObjectStore", Name,options);

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName) =>
            InvokeMethod<IDBTransaction>("transaction", storeName,"readwrite");

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName, string mode) =>
            InvokeMethod<IDBTransaction>("transaction", storeName, mode);

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName,string mode, object options) =>
            InvokeMethod<IDBTransaction>("transaction", storeName,mode, options);

        [Export("close")]
        public void Close() => ManagedJSObject.Invoke("close");
    }

    [Export("IDBObjectStore", typeof(JSObject))]
    public class IDBObjectStore : DOMObject
    {
        public IDBObjectStore(JSObject obj) : base(obj)
        {

        }

        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }

        [Export("transaction")]
        public string Transaction { get => GetProperty<string>("transaction"); }

        [Export("add")]
        public IDBRequest Add(object value, string Key) =>InvokeMethod<IDBRequest>("add", value, Key);

        [Export("add")]
        public IDBRequest Add(object value) =>InvokeMethod<IDBRequest>("add", value);

        [Export("clear")]
        public Task Clear() => InvokeMethod<IDBRequest>("clear").MakeTask();

        [Export("count")]
        public async Task<int> Count() =>(int)(await InvokeMethod<IDBRequest>("count").MakeTask()).Result;

        [Export("createIndex")]
        public IDBIndex CreateIndex(string indexName,string keyPath) => InvokeMethod<IDBIndex>("createIndex", indexName,keyPath);

        [Export("createIndex")]
        public IDBIndex CreateIndex(string indexName, string keyPath,object objectParameters) => InvokeMethod<IDBIndex>("createIndex", indexName, keyPath, objectParameters);

        [Export("delete")]
        public IDBRequest Delete(object KeyRange) => InvokeMethod<IDBRequest>("delete", KeyRange);

        [Export("deleteIndex")]
        public void DeleteIndex(string indexName) => ManagedJSObject.Invoke("deleteIndex", indexName);

        [Export("get")]
        public async Task<object> Get(string key) => (await InvokeMethod<IDBRequest>("get", key).MakeTask()).Result;

        [Export("get")]
        public async Task<t> Get<t>(string key) =>(t) await Get(key);

        [Export("getKey")]
        public IDBRequest GetKey(string key) => InvokeMethod<IDBRequest>("getKey", key);

        [Export("getAll")]
        public IDBRequest GetAll() => InvokeMethod<IDBRequest>("getAll");

        [Export("getAll")]
        public IDBRequest GetAll(object query) => InvokeMethod<IDBRequest>("getAll", query);

        [Export("getAll")]
        public IDBRequest GetAll(object query, int count) => InvokeMethod<IDBRequest>("getAll", query, count);

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys() => InvokeMethod<IDBRequest>("getAllKeys");

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys(object query) => InvokeMethod<IDBRequest>("getAllKeys", query);

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys(object query, int count) => InvokeMethod<IDBRequest>("getAllKeys", query, count);

        [Export("index")]
        public IDBIndex Index(string name) => InvokeMethod<IDBIndex>("index",name);

        [Export("openCursor")]
        public IDBRequest OpenCursor() => InvokeMethod<IDBRequest>("openCursor");

        [Export("openCursor")]
        public IDBRequest OpenCursor(object query) => InvokeMethod<IDBRequest>("openCursor", query);

        [Export("openCursor")]
        public IDBRequest OpenCursor(object query, int count) => InvokeMethod<IDBRequest>("openCursor", query, count);

        [Export("put")]
        public IDBRequest Put(object item) => InvokeMethod<IDBRequest>("put", item);

        [Export("put")]
        public IDBRequest Put(object item, string key) => InvokeMethod<IDBRequest>("put", item, key);
    }

    [Export("IDBIndex", typeof(JSObject))]
    public class IDBIndex : DOMObject
    {
        public IDBIndex(JSObject handle) : base(handle)
        {
        }

        [Export("isAutoLocale")]
        public bool IsAutoLocale { get => GetProperty<bool>("isAutoLocale"); }

        [Export("locale")]
        public JSObject Locale { get => GetProperty<JSObject>("locale"); }

        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }

        [Export("objectStore")]
        public object ObjectStore { get => GetProperty<object>("objectStore"); }

        [Export("keyPath")]
        public object KeyPath { get => GetProperty<object>("keyPath"); }

        [Export("multiEntry")]
        public bool MultiEntry { get => GetProperty<bool>("multiEntry"); }

        [Export("unique")]
        public object Unique { get => GetProperty<object>("unique"); }

        [Export("count")]
        public IDBRequest Count(string Key) => InvokeMethod<IDBRequest>("count", Key);

        [Export("count")]
        public IDBRequest Count() => InvokeMethod<IDBRequest>("count");

        [Export("get")]
        public IDBRequest Get(string Key) => InvokeMethod<IDBRequest>("get", Key);

        [Export("getKey")]
        public IDBRequest GetKey(string Key) => InvokeMethod<IDBRequest>("getKey", Key);

        [Export("getAll")]
        public IDBRequest GetAll() => InvokeMethod<IDBRequest>("getAll");

        [Export("getAll")]
        public IDBRequest GetAll(object query) => InvokeMethod<IDBRequest>("getAll", query);

        [Export("getAll")]
        public IDBRequest GetAll(object query,int count) => InvokeMethod<IDBRequest>("getAll", query, count);

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys() => InvokeMethod<IDBRequest>("getAllKeys");

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys(object query) => InvokeMethod<IDBRequest>("getAllKeys", query);

        [Export("getAllKeys")]
        public IDBRequest GetAllKeys(object query, int count) => InvokeMethod<IDBRequest>("getAllKeys", query, count);

        [Export("openCursor")]
        public IDBRequest OpenCursor() => InvokeMethod<IDBRequest>("openCursor");

        [Export("openCursor")]
        public IDBRequest OpenCursor(object range) => InvokeMethod<IDBRequest>("openCursor", range);

        [Export("openCursor")]
        public IDBRequest OpenCursor(object range, object direction) => InvokeMethod<IDBRequest>("openCursor", range, direction);

        [Export("openKeyCursor")]
        public IDBRequest OpenKeyCursor() => InvokeMethod<IDBRequest>("openKeyCursor");

        [Export("openKeyCursor")]
        public IDBRequest OpenKeyCursor(object range) => InvokeMethod<IDBRequest>("openKeyCursor", range);

        [Export("openKeyCursor")]
        public IDBRequest OpenKeyCursor(object range, object direction) => InvokeMethod<IDBRequest>("openKeyCursor", range, direction);

    }

    [Export("IDBRequest", typeof(JSObject))]
    public class IDBRequest:EventTarget
    {
        public IDBRequest(JSObject handle) : base(handle)
        {
            SetProperty("onerror", (Action<JSObject>)onerror);
            SetProperty("onsuccess", (Action<JSObject>)onsuccess);
            SetProperty("onupgradeneeded", (Action<JSObject>)onupgradeneeded);
        }

        public event Action<JSObject> OnError;
        public event Action<JSObject> OnSuccess;
        public event Action<JSObject> OnUpgradeNeeded;
        private void onerror(JSObject e)=>OnError?.Invoke(e);
        private void onsuccess(JSObject e)=>OnSuccess?.Invoke(e);
        private void onupgradeneeded(JSObject e) => OnUpgradeNeeded?.Invoke(e);

        public async Task<IDBRequest> MakeTask()
        {
            var Completed = new Task(()=> { });
            var Error = false;
            Action<JSObject> OnError=null;
            OnError= (c) =>
            {
                this.OnError -= OnError;
                Error =true;
                Completed.Start();
            };
            this.OnError += OnError;

            Action<JSObject> OnSuccess = null;
            OnSuccess = (c) =>
            {
                this.OnSuccess -= OnSuccess;
                Completed.Start();
            };
            this.OnSuccess += OnSuccess;

            await Completed;
            if(Error)
                throw new Exception("Error");

            return this;
        }

        public JSObject Error { get => GetProperty<JSObject>("error"); }
        public object Result { get => GetProperty<object>("result"); }
        public JSObject Source { get => GetProperty<JSObject>("source"); }
        public JSObject ReadyState { get => GetProperty<JSObject>("readyState"); }
        public JSObject Transaction { get => GetProperty<JSObject>("transaction"); }

        public t GetResult<t>() => (t) Result;

    }

    [Export("IDBRequest", typeof(JSObject))]
    public class IDBTransaction : DOMObject
    {
        public IDBTransaction(JSObject handle) : base(handle)
        {
            SetProperty("onabort", (Action<JSObject>)onabort);
            SetProperty("oncomplete", (Action<JSObject>)oncomplete);
            SetProperty("onerror", (Action<JSObject>)onerror);
        }

        public event Action<JSObject> OnError;
        public event Action<JSObject> OnComplete;
        public event Action<JSObject> OnAbort;
        private void onerror(JSObject e) => OnError?.Invoke(e);
        private void oncomplete(JSObject e) => OnComplete?.Invoke(e);
        private void onabort(JSObject e) => OnAbort?.Invoke(e);

        public IDBObjectStore ObjectStore(string Name) => InvokeMethod<IDBObjectStore>("objectStore",Name);
        public void Abort() => ManagedJSObject.Invoke("abort");
        public async Task Commit()
        {
            var Completed = new Task(() => { });
            var Status =0;
            Action<JSObject> OnError = null;
            OnError = (c) =>
            {
                this.OnError -= OnError;
                Status = 0;
                Completed?.Start();
                Completed = null;
            };
            this.OnError += OnError;

            Action<JSObject> OnAbort = null;
            OnAbort = (c) =>
            {
                this.OnComplete -= OnAbort;
                Status = 1;
                Completed?.Start();
                Completed = null;
            };
            this.OnAbort += OnAbort;

            Action<JSObject> OnComplete = null;
            OnComplete = (c) =>
            {
                this.OnComplete -= OnComplete;
                Status = 2;
                Completed?.Start();
                Completed = null;
            };
            this.OnComplete += OnComplete;

            await Completed;
            if (Status==0)
                throw new Exception("Error");
            if(Status == 1)
                throw new Exception("Aborted");
        }

        public IDBDatabase DB { get => GetProperty<IDBDatabase>("db"); }
        public string Durability { get => GetProperty<string>("durability"); }
        public JSObject Error { get => GetProperty<JSObject>("error"); }
        public JSObject Mode { get => GetProperty<JSObject>("mode"); }
        public JSObject ObjectStoreNames { get => GetProperty<JSObject>("objectStoreNames"); }

    }
}