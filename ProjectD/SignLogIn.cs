using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using HttpCommunication;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace ProjectD
{
    [Activity(Label = "登录", MainLauncher = true)]
    public class SignLogIn : Activity
    {
        List<String> Users = new List<String> { };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.SignLogin);
            Button SignInButton = FindViewById<Button>(Resource.Id.SignIn);
            Button LogInButton = FindViewById<Button>(Resource.Id.LogIn);
            EditText PassWordText = FindViewById<EditText>(Resource.Id.PassWord);
            AutoCompleteTextView UserNameText = FindViewById<AutoCompleteTextView>(Resource.Id.autocomplete_country);
            /*获取数据库文件路径*/
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            /*创建链接*/
            var db = new SQLiteConnection(dbPath);
            /*创建数据表*/
            db.CreateTable<UserTable>();
            /*自动补全用户名功能在此*/
            foreach (UserTable Count in db.Table<UserTable>().ToList())
            {
                string user = Count.UserName;
                Users.Add(user);
            }
            Users.Add("aaa");
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, Users);
            UserNameText.Adapter = adapter;
            /*自动补全功能启动完毕*/

            /*登录按钮按下执行的东西*/
            SignInButton.Click += (s, arg) =>
            {
                //JsonObject JSONresult;
                if (!SignInButton.Text.Equals("登录中"))
                {
                    this.RunOnUiThread(() =>
                    {
                        SignInButton.Text = "登陆中";
                    });
                    Task startupWork = new Task(() => { Signin(UserNameText.Text,PassWordText.Text,SignInButton); });
                    startupWork.Start();
                    /*
                    var getuser = new HttpPost();
                    var result = getuser.CheckUserByJson("http://www.nakago.cc/User/CheckUser", UserNameText.Text, PassWordText.Text);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                    //if (result.StartsWith("{") || result.StartsWith("[")) JSONresult = (JsonObject)JsonObject.Parse(result);//判断是否是Json数据
                    //else JSONresult = (JsonObject)JsonObject.Parse("{\"Success\": false,\"Reason\":\"" + result+"\"}");//不是则改成json数据
                    //if (JSONresult["Success"] == true)//判断json数据中的Success项
                    //{
                        if (result.Equals("true"))//判断字符串是否一样
                        {
                            Task startupWork = new Task(() => { ToMain(); });
                            Android.Widget.Toast.MakeText(this, "登录成功", Android.Widget.ToastLength.Short).Show();
                            var table = db.Table<UserTable>();//查找本地数据库是否有此用户
                            foreach (var user in table)
                            {
                                if (user.UserName.Equals(UserNameText.Text) && user.PassWord.Equals(PassWordText.Text))
                                {
                                    startupWork.Start();
                                    return;//查到存在则退出
                                }
                            }
                            var data = new UserTable();//没找到就会执行到这
                            data.UserName = UserNameText.Text;
                            data.PassWord = PassWordText.Text;
                            db.Insert(data);//向本地数据库插入一个用户
                            startupWork.Start();
                        }
                        else
                        {
                            Android.Widget.Toast.MakeText(this, "登录失败:用户名或密码错误"+ result, Android.Widget.ToastLength.Short).Show();
                        }
                    //}
                    //else
                    //{
                        //Android.Widget.Toast.MakeText(this, "登录失败:"+result, Android.Widget.ToastLength.Short).Show();
                    //}
                    */
                }
                else
                {
                    Android.Widget.Toast.MakeText(this, "请输入用户名", Android.Widget.ToastLength.Short).Show();
                }
            };
            /*注册按钮按下执行的东西*/
            LogInButton.Click += (s, arg) =>
            {
                //JsonObject JSONresult;
                if (!LogInButton.Text.Equals("注册中"))
                {
                    this.RunOnUiThread(() =>
                    {
                        LogInButton.Text = "注册中";
                    });
                    Task startupWork = new Task(() => { Login(UserNameText.Text, PassWordText.Text, LogInButton); });
                    startupWork.Start();
                }
                /*
                var postuser = new HttpPost();
                if (UserNameText.Text.Length > 0)
                {
                    var result = postuser.PostUserByJson("http://www.nakago.cc/User/PostUser", UserNameText.Text, PassWordText.Text);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                    //if (result.StartsWith("{") || result.StartsWith("[")) JSONresult = (JsonObject)JsonObject.Parse(result);//判断是否是Json数据
                    //else JSONresult = (JsonObject)JsonObject.Parse("{\"Success\": false,\"Reason\":\"" + result + "\"}");
                    //if (JSONresult["Success"] == true)
                    if (result.Equals("true"))
                    {
                        Task startupWork = new Task(() => { ToMain(); });
                        Android.Widget.Toast.MakeText(this, "注册成功", Android.Widget.ToastLength.Short).Show();
                        var data = new UserTable();
                        data.UserName = UserNameText.Text;
                        data.PassWord = PassWordText.Text;
                        db.Insert(data);
                        startupWork.Start();
                    }
                    else
                    {
                        Android.Widget.Toast.MakeText(this, "注册失败:"+ result, Android.Widget.ToastLength.Short).Show();
                    }
                }
                */
            };
        }
        async void Login(string name, string password, Button LogInButton)
        {
            await Task.Delay(10);
            /*获取数据库文件路径*/
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            /*创建链接*/
            var db = new SQLiteConnection(dbPath);
            /*创建数据表*/
            db.CreateTable<UserTable>();
            if (name.Length > 0)
            {
                var postuser = new HttpPost();
                var result = postuser.PostUserByJson("http://www.nakago.cc/User/PostUser", name, password);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                if (result.Equals("true"))
                {
                    this.RunOnUiThread(() =>
                    {
                        LogInButton.Text = "注册";
                        Task startupWork = new Task(() => { ToMain(name); });
                        Android.Widget.Toast.MakeText(this, "注册成功", Android.Widget.ToastLength.Short).Show();
                        var data = new UserTable();
                        data.UserName = name;
                        data.PassWord = password;
                        db.Insert(data);
                        startupWork.Start();
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        LogInButton.Text = "注册";
                        Android.Widget.Toast.MakeText(this, "注册失败:" + result, Android.Widget.ToastLength.Short).Show();
                    });
                }
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    LogInButton.Text = "注册";
                    Android.Widget.Toast.MakeText(this, "注册失败:" + "请输入用户名", Android.Widget.ToastLength.Short).Show();
                });
            }
        }
        async void Signin(string name,string password , Button SignInButton)
        {
            await Task.Delay(10);
            /*获取数据库文件路径*/
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            /*创建链接*/
            var db = new SQLiteConnection(dbPath);
            /*创建数据表*/
            db.CreateTable<UserTable>();
            if (name.Length > 0)
            {
                var getuser = new HttpPost();
                var result = getuser.CheckUserByJson("http://www.nakago.cc/User/CheckUser", name, password);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                if (result.Equals("true"))//判断字符串是否一样
                {
                    this.RunOnUiThread(() =>
                    {
                        SignInButton.Text = "登陆";
                        Task startupWork = new Task(() => { ToMain(name); });
                        Android.Widget.Toast.MakeText(this, "登录成功", Android.Widget.ToastLength.Short).Show();
                        var table = db.Table<UserTable>();//查找本地数据库是否有此用户
                        foreach (var user in table)
                        {
                            if (user.UserName.Equals(name) && user.PassWord.Equals(password))
                            {
                                startupWork.Start();
                                return;//查到存在则退出
                            }
                        }
                        var data = new UserTable();//没找到就会执行到这
                        data.UserName = name;
                        data.PassWord = password;
                        db.Insert(data);//向本地数据库插入一个用户
                        startupWork.Start();
                    });
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        SignInButton.Text = "登陆";
                        Android.Widget.Toast.MakeText(this, "登录失败:用户名或密码错误" + result, Android.Widget.ToastLength.Short).Show();
                    });
                }
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    SignInButton.Text = "登陆";
                    Android.Widget.Toast.MakeText(this, "登录失败:请输入用户名", Android.Widget.ToastLength.Short).Show();
                });
            }
        }
        async void ToMain(string name)
        {
            await Task.Delay(500);
            var MyActive = new Intent(this, typeof(MainActivity));
            MyActive.PutExtra("User", name);
            StartActivity(MyActive);
        }
    }
    /*数据库格式声明*/
    public class UserTable
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}