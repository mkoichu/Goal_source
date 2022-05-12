using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if PLAYGROUND_DISABLED   //- COMMENT THIS AFTER INSTALL
//#if !PLAYGROUND_DISABLED    //- UNCOMMENT THIS AFTER INSTALL

using Newtonsoft.Json;
using Firebase.Firestore;

using Tamarin.Common;
using Tamarin.FirebaseX;
public class FirebaseExamples : Singleton<FirebaseExamples>
{
    Text result;
    Button button;
    FirebaseAPI firebase;

    public FirestoreModel test;

    async void Start()
    {
        button = transform.Find("TEST").GetComponent<Button>();
        result = transform.Find("RESULT/Viewport/Content/Text").GetComponent<Text>();
        result.text = "To play around with the API. open the file: /Assets/Tamarin/Examples/FirebaseExamples.cs";

        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        firebase = FirebaseAPI.Instance;

        //Uncomment functions you want to play around with.
        button.onClick.AddListener(async () =>
        {
            await Waiter.ForSeconds(1);
            //await TestAuth();
            //await TestConfig();
            //await TestStorage();
            //await TestRealtime();
            //await TestFirestore();
            //await TestFunctions();
            //await TestAnalytics();
            //await TestMessaging();
            //await TestLinks();
        });
    }

    async Task<bool> TestAuth()
    {
        firebase.auth.AuthChanged((user) =>
        {
            result.text += $"\n\n AUTH LISTENER: \n {JsonConvert.SerializeObject(user)}";
        });

        FirebaseUser user = await firebase.auth.SignInWithAnon();
        result.text += $"\n\n ANON AUTH: \n {JsonConvert.SerializeObject(user)}";

        user = await firebase.auth.RegisterWithEmail("test@test.com", "Test0000tesT");
        result.text += $"\n\n REG AUTH: \n {JsonConvert.SerializeObject(user)}";

        firebase.auth.SignOut();
        await firebase.auth.Reload();

        user = await firebase.auth.SignInWithEmail("test@test.com", "Test0000tesT");
        result.text += $"\n\n EMAIL AUTH: \n {JsonConvert.SerializeObject(user)}";
        await firebase.auth.UpdateProfile("TEST NAME", new Uri("https://test.com/test.jpg"));
        await firebase.auth.VerifyEmail();

        await firebase.auth.Reload();
        result.text += $"\n\n USER: \n {JsonConvert.SerializeObject(firebase.auth.User)}";

        var token = await firebase.auth.GetToken(false);
        result.text += $"\n\n TOKEN: \n {JsonConvert.SerializeObject(token)}";
        return true;
    }

    async Task<bool> TestFirestore()
    {
        firebase.firestore.QueryListenAs<FirestoreModel>("test", "testId", (res) =>
        {
            result.text += $"\n\n FIRESTORE LISTENER: \n {JsonConvert.SerializeObject(res)}";
        });

        var query = new List<FirestoreQuery> { new FirestoreQuery("test", "orderBy", null), new FirestoreQuery(null, "limit", 10) };
        firebase.firestore.QueryListenAs<FirestoreModel>("test", query, (res) =>
        {
            result.text += $"\n\n FIRESTORE QUERY LISTENER: \n {JsonConvert.SerializeObject(res)}";
        });

        var data = new FirestoreModel() { test = "test" };
        await firebase.firestore.SetAsync("test", "testId", data);
        await firebase.firestore.SetAsync("test", "testId2", data);
        await firebase.firestore.UpdateAsync("test", "testId", new Dictionary<string, object> { { "test", "A" } });
        await firebase.firestore.UpdateAsync("test", "testId2", new Dictionary<string, object> { { "test", Firebase.Firestore.FieldValue.ArrayUnion(new Dictionary<string, object> { { "xxx", "yyy" } }) } });

        var res = await firebase.firestore.QueryAs<FirestoreModel>("test", "testId1");
        result.text += $"\n\n FIRESTORE: \n {JsonConvert.SerializeObject(res)}";

        var res2 = await firebase.firestore.QueryAs<FirestoreModel>("test", "testId2");
        result.text += $"\n\n FIRESTORE: \n {JsonConvert.SerializeObject(res2)}";

        return true;
    }

    async Task<bool> TestRealtime()
    {
        firebase.database.QueryListenAsync<Dictionary<string, object>>("testQuery", (res) =>
        {
            result.text += $"\n\n REALTIME LISTENER: \n {JsonConvert.SerializeObject(res)}";
        });

        var query = new List<DatabaseQuery> { new DatabaseQuery("limitToLast", 1) };
        firebase.database.QueryListenAsync<Dictionary<string, object>>("testQuery", (res) =>
        {
            result.text += $"\n\n REALTIME QUERY LISTENER: \n {JsonConvert.SerializeObject(res)}";
        }, query);

        var data = new RealtimeModel() { test = "test" };
        await firebase.database.SetRawAsync("test", data);
        await firebase.database.UpdateAsync("test", new Dictionary<string, object> { { "test", "test updated" } });

        await firebase.database.SetAsync("testQuery", new Dictionary<string, object> { { "test1", "set 1" } });
        await firebase.database.UpdateAsync("testQuery", new Dictionary<string, object> { { "test1", "set 1" }, { "test2", "set 2" }, { "test3", "set 3" }, });

        var res = await firebase.database.QueryAsync<RealtimeModel>("test");
        result.text += $"\n\n REALTIME: \n {JsonConvert.SerializeObject(res)}";

        var resx = await firebase.database.QueryAsync<RealtimeModel>("test/something");
        result.text += $"\n\n REALTIME A: \n {JsonConvert.SerializeObject(resx)}";

        var res2 = await firebase.database.QueryAsync<Dictionary<string, object>>("testQuery");
        result.text += $"\n\n REALTIME B QUERY: \n {JsonConvert.SerializeObject(res2)}";

        return true;
    }

    async Task<bool> TestStorage()
    {
        var buffer = await TestImage();

        var upload = await firebase.storage.Upload("/test/test.jpg", buffer, "image/jpeg");
        result.text += $"\n\n STORAGE A UPLOADED: \n {upload}";

        var url = await firebase.storage.DownloadUrl("/test/test.jpg");
        result.text += $"\n\n STORAGE A DOWNLOAD URL: \n {url}";

        var upload2 = await firebase.storage.Upload("gs://***.appspot.com", "/test/test.jpg", buffer, "image/jpeg");
        result.text += $"\n\n STORAGE B UPLOADED: \n {upload}";

        var url2 = await firebase.storage.DownloadUrl("gs://***.appspot.com", "/test/test.jpg");
        result.text += $"\n\n STORAGE B DOWNLOAD URL: \n {url}";

        return true;
    }

    async Task<bool> TestFunctions()
    {
        var res = await firebase.functions.HttpsCall<Dictionary<string, object>>("test", new Dictionary<string, object> { { "foo", "bar" } });
        result.text += $"\n\n FUNCTIONS A: \n {JsonConvert.SerializeObject(res)}";

        var res2 = await firebase.functions.HttpsCall<Dictionary<string, object>>("test", "europe-west1", new Dictionary<string, object> { { "foo", "bar" } });
        result.text += $"\n\n FUNCTIONS B: \n {JsonConvert.SerializeObject(res2)}";

        return true;
    }

    public async Task<bool> TestAnalytics()
    {
        firebase.analytics.Setup(true, 300);
        firebase.analytics.LogEvent("page_view", "something", "something");
        firebase.analytics.LogEvent("TEST_EVENT");
        firebase.analytics.LogEvent("TEST_EVENTA", "test", 1.0f);
        result.text += $"\n\n ANALYTICS SENT";

        await Waiter.ForSeconds(1);
        return true;
    }

    async Task<bool> TestLinks()
    {
        var link = await firebase.links.GetShortLinkAsync("https://linkyoureceive", "https://yourprefixlink", "com.yourpackage.id", "com.yourpackage.id");
        result.text += $"\n\n SHORTLINK : \n {link}";

        firebase.links.OnLinkReceived((link) =>
        {
            result.text += $"\n\n LINK LISTENER : \n {link}";
        });
        return true;
    }

    public async Task<bool> TestMessaging()
    {
        firebase.messaging.Setup((token) =>
        {
            result.text += $"\n\n MESSAGING: \n {token}";
        });

        firebase.messaging.Subscribe("xxx");
        firebase.messaging.OnReceived((message) =>
        {
            result.text += $"\n\n MESSAGING ONRECEIVED: \n {JsonConvert.SerializeObject(message)}";
        });

        await Waiter.ForSeconds(1);
        return true;
    }

    async Task<bool> TestConfig()
    {
        var conf = new Dictionary<string, object> { { "TEST1", "TEST1VALUE" }, { "TEST2", "TEST2VALUE" } };

        await firebase.config.Setup(conf, 3600000);
        await firebase.config.FetchActivate();

        var res = await firebase.config.GetValue("TEST1");
        result.text += $"\n\n REMOTE CONFIG: \n {res}";

        var res2 = await firebase.config.GetValue("TEST2");
        result.text += $"\n\n REMOTE CONFIG2: \n {res2}";

        return true;
    }

    public async Task<byte[]> TestImage()
    {
        await new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();
        UnityEngine.Object.Destroy(tex);
        return bytes;
    }

}

[Serializable, FirestoreData]
public class FirestoreModel
{
    public string docId { get; set; }   //<- receive the docId automatically

    [FirestoreProperty] public object test { get; set; }
    [FirestoreProperty] public DateTimeOffset time { get; set; }
    [FirestoreProperty] public object time1 { get; set; }
    [field: SerializeField][FirestoreProperty] public string time2 { get; set; }
    [FirestoreProperty] public object point { get; set; }
    [FirestoreProperty] public FirestoreModel2 xxx { get; set; }
}

[FirestoreData]
public class FirestoreModel2
{
    [FirestoreProperty] public DateTimeOffset time { get; set; }
}

public class RealtimeModel
{
    public string test { get; set; }
}
#endif