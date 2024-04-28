using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using WebAssembly.Browser.MonsajemDomHelpers;

namespace WebAssembly.Browser.DOM
{

    [Export("indexedDB", typeof(IJSInProcessObjectReference))]
    public static class IndexedDB
    {
        private static IJSInProcessObjectReference Obj = ((Func<IJSInProcessObjectReference>)(() =>
        {
            return js.JsGetValue("indexedDB");
        }))();

        [Export("open")]
        public async static Task<IDBDatabase> Open(
            string Name, int Version, Func<IDBDatabase, Task> OnUpgradeNeeded)
        {
            IDBDatabase Result = null;
            var RQ = new IDBRequest((IJSInProcessObjectReference)Obj.Invoke<object>("open", Name, Version));
            var UpgradeNeeded = new Task(() => { });
            async void _OnUpgradeNeeded(IJSInProcessObjectReference c)
            {
                Result = new IDBDatabase((IJSInProcessObjectReference)RQ.Result);
                await OnUpgradeNeeded(Result);
                UpgradeNeeded.Start();
            }
            RQ.OnUpgradeNeeded += _OnUpgradeNeeded;
            _ = await RQ.MakeTask();
            Result = new IDBDatabase((IJSInProcessObjectReference)RQ.Result);
            if (Result.Version != Version)
                await UpgradeNeeded;
            RQ.OnUpgradeNeeded -= _OnUpgradeNeeded;
            return Result;
        }

        public static Task DeleteDatabase(string Name)
        {
            return new IDBRequest((IJSInProcessObjectReference)Obj.Invoke<Task>("deleteDatabase", Name)).MakeTask();
        }
    }

    [Export("IDBDatabase", typeof(IJSInProcessObjectReference))]
    public class IDBDatabase : DOMObject
    {
        public IDBDatabase(IJSInProcessObjectReference obj) : base(obj)
        {
        }

        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }

        [Export("version")]
        public int Version { get => GetProperty<int>("version"); }

        [Export("createObjectStore")]
        public IDBObjectStore CreateObjectStore(string Name) =>
            InvokeMethod<IDBObjectStore>("createObjectStore", Name);

        [Export("createObjectStore")]
        public IDBObjectStore CreateObjectStore(string Name, object options) =>
            InvokeMethod<IDBObjectStore>("createObjectStore", Name, options);

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName) =>
            InvokeMethod<IDBTransaction>("transaction", storeName, "readwrite");

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName, string mode) =>
            InvokeMethod<IDBTransaction>("transaction", storeName, mode);

        [Export("transaction")]
        public IDBTransaction Transaction(string storeName, string mode, object options) =>
            InvokeMethod<IDBTransaction>("transaction", storeName, mode, options);

        [Export("close")]
        public void Close() => ManagedJSObject.InvokeVoid("close");
    }

    [Export("IDBObjectStore", typeof(IJSInProcessObjectReference))]
    public class IDBObjectStore : DOMObject
    {
        public IDBObjectStore(IJSInProcessObjectReference obj) : base(obj)
        {

        }

        [Export("name")]
        public string Name { get => GetProperty<string>("name"); }

        [Export("transaction")]
        public string Transaction { get => GetProperty<string>("transaction"); }

        [Export("add")]
        public IDBRequest Add(object value, string Key) => InvokeMethod<IDBRequest>("add", value, Key);

        [Export("add")]
        public IDBRequest Add(object value) => InvokeMethod<IDBRequest>("add", value);

        [Export("clear")]
        public Task Clear() => InvokeMethod<IDBRequest>("clear").MakeTask();

        [Export("count")]
        public async Task<int> Count() => (int)(await InvokeMethod<IDBRequest>("count").MakeTask()).Result;

        [Export("createIndex")]
        public IDBIndex CreateIndex(string indexName, string keyPath) => InvokeMethod<IDBIndex>("createIndex", indexName, keyPath);

        [Export("createIndex")]
        public IDBIndex CreateIndex(string indexName, string keyPath, object objectParameters) => InvokeMethod<IDBIndex>("createIndex", indexName, keyPath, objectParameters);

        [Export("delete")]
        public Task Delete(object KeyRange) => InvokeMethod<IDBRequest>("delete", KeyRange).MakeTask();

        [Export("deleteIndex")]
        public void DeleteIndex(string indexName) => ManagedJSObject.InvokeVoid("deleteIndex", indexName);

        [Export("get")]
        public async Task<object> Get(string key) => (await InvokeMethod<IDBRequest>("get", key).MakeTask()).Result;

        [Export("get")]
        public async Task<t> Get<t>(string key) => (t)await Get(key);

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
        public IDBIndex Index(string name) => InvokeMethod<IDBIndex>("index", name);

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

    [Export("IDBIndex", typeof(IJSInProcessObjectReference))]
    public class IDBIndex : DOMObject
    {
        public IDBIndex(IJSInProcessObjectReference handle) : base(handle)
        {
        }

        [Export("isAutoLocale")]
        public bool IsAutoLocale { get => GetProperty<bool>("isAutoLocale"); }

        [Export("locale")]
        public IJSInProcessObjectReference Locale { get => GetProperty<IJSInProcessObjectReference>("locale"); }

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
        public IDBRequest GetAll(object query, int count) => InvokeMethod<IDBRequest>("getAll", query, count);

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

    [Export("IDBRequest", typeof(IJSInProcessObjectReference))]
    public class IDBRequest : EventTarget
    {
        public IDBRequest(IJSInProcessObjectReference handle) : base(handle)
        {
            SetProperty("onerror", onerror);
            SetProperty("onsuccess", onsuccess);
            SetProperty("onupgradeneeded", onupgradeneeded);
        }

        public event Action<IJSInProcessObjectReference> OnError;
        public event Action<IJSInProcessObjectReference> OnSuccess;
        public event Action<IJSInProcessObjectReference> OnUpgradeNeeded;
        private void onerror(IJSInProcessObjectReference e) => OnError?.Invoke(e);
        private void onsuccess(IJSInProcessObjectReference e) => OnSuccess?.Invoke(e);
        private void onupgradeneeded(IJSInProcessObjectReference e) => OnUpgradeNeeded?.Invoke(e);

        public async Task<IDBRequest> MakeTask()
        {
            var Completed = new Task(() => { });
            var Error = false;
            void OnError(IJSInProcessObjectReference c)
            {
                this.OnError -= OnError;
                Error = true;
                Completed.Start();
            }

            this.OnError += OnError;

            void OnSuccess(IJSInProcessObjectReference c)
            {
                this.OnSuccess -= OnSuccess;
                Completed.Start();
            }

            this.OnSuccess += OnSuccess;

            await Completed;
            return Error ? throw new Exception("Error") : this;
        }

        public IJSInProcessObjectReference Error { get => GetProperty<IJSInProcessObjectReference>("error"); }
        public object Result { get => GetProperty<object>("result"); }
        public IJSInProcessObjectReference Source { get => GetProperty<IJSInProcessObjectReference>("source"); }
        public IJSInProcessObjectReference ReadyState { get => GetProperty<IJSInProcessObjectReference>("readyState"); }
        public IJSInProcessObjectReference Transaction { get => GetProperty<IJSInProcessObjectReference>("transaction"); }

        public t GetResult<t>() => (t)Result;

    }

    [Export("IDBRequest", typeof(IJSInProcessObjectReference))]
    public class IDBTransaction : DOMObject
    {
        public IDBTransaction(IJSInProcessObjectReference handle) : base(handle)
        {
            SetProperty("onabort", onabort);
            SetProperty("oncomplete", oncomplete);
            SetProperty("onerror", onerror);
        }

        public event Action<IJSInProcessObjectReference> OnError;
        public event Action<IJSInProcessObjectReference> OnComplete;
        public event Action<IJSInProcessObjectReference> OnAbort;
        private void onerror(IJSInProcessObjectReference e) => OnError?.Invoke(e);
        private void oncomplete(IJSInProcessObjectReference e) => OnComplete?.Invoke(e);
        private void onabort(IJSInProcessObjectReference e) => OnAbort?.Invoke(e);

        public IDBObjectStore ObjectStore(string Name) => InvokeMethod<IDBObjectStore>("objectStore", Name);
        public void Abort() => ManagedJSObject.InvokeVoid("abort");
        public async Task Commit()
        {
            var Completed = new Task(() => { });
            var Status = 0;
            void OnError(IJSInProcessObjectReference c)
            {
                this.OnError -= OnError;
                Status = 0;
                Completed?.Start();
                Completed = null;
            }

            this.OnError += OnError;

            void OnAbort(IJSInProcessObjectReference c)
            {
                this.OnComplete -= OnAbort;
                Status = 1;
                Completed?.Start();
                Completed = null;
            }

            this.OnAbort += OnAbort;

            void OnComplete(IJSInProcessObjectReference c)
            {
                this.OnComplete -= OnComplete;
                Status = 2;
                Completed?.Start();
                Completed = null;
            }

            this.OnComplete += OnComplete;

            await Completed;
            if (Status == 0)
                throw new Exception("Error");
            if (Status == 1)
                throw new Exception("Aborted");
        }

        public IDBDatabase DB { get => GetProperty<IDBDatabase>("db"); }
        public string Durability { get => GetProperty<string>("durability"); }
        public IJSInProcessObjectReference Error { get => GetProperty<IJSInProcessObjectReference>("error"); }
        public IJSInProcessObjectReference Mode { get => GetProperty<IJSInProcessObjectReference>("mode"); }
        public IJSInProcessObjectReference ObjectStoreNames { get => GetProperty<IJSInProcessObjectReference>("objectStoreNames"); }

    }
}