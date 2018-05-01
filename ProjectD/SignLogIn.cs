using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using HttpCommunication;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;

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
                JsonObject JSONresult;
                string username = "", password = "";
                if (UserNameText.Text.Length > 0)
                {
                    var getuser = new HttpPost();
                    var result = getuser.CheckUserByJson("http://www.nakago.cc/User/CheckUser", UserNameText.Text, PassWordText.Text);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                    //if (result.StartsWith("{") || result.StartsWith("[")) JSONresult = (JsonObject)JsonObject.Parse(result);//判断是否是Json数据
                    //else JSONresult = (JsonObject)JsonObject.Parse("{\"Success\": false,\"Reason\":\"" + result+"\"}");//不是则改成json数据
                    //if (JSONresult["Success"] == true)//判断json数据中的Success项
                    //{
                        if (result.Equals("true"))//判断字符串是否一样
                        {
                            Android.Widget.Toast.MakeText(this, "登录成功", Android.Widget.ToastLength.Short).Show();
                            var table = db.Table<UserTable>();//查找本地数据库是否有此用户
                            foreach (var user in table)
                            {
                                if (user.UserName.Equals(UserNameText.Text) && user.PassWord.Equals(PassWordText.Text))
                                {
                                    Android.Widget.Toast.MakeText(this, "登录成功", Android.Widget.ToastLength.Short).Show();
                                    return;//查到存在则退出
                                }
                            }
                            var data = new UserTable();//没找到就会执行到这
                            data.UserName = UserNameText.Text;
                            data.PassWord = PassWordText.Text;
                            db.Insert(data);//向本地数据库插入一个用户
                        }
                        else
                        {
                            Android.Widget.Toast.MakeText(this, "登录失败:用户名或密码错误", Android.Widget.ToastLength.Short).Show();
                        }
                    //}
                    //else
                    //{
                        //Android.Widget.Toast.MakeText(this, "登录失败:"+result, Android.Widget.ToastLength.Short).Show();
                    //}
                }
            };
            /*注册按钮按下执行的东西*/
            LogInButton.Click += (s, arg) =>
            {
                JsonObject JSONresult;
                var postuser = new HttpPost();
                if (UserNameText.Text.Length > 0)
                {
                    var result = postuser.PostUserByJson("http://www.nakago.cc/User/PostUser", UserNameText.Text, PassWordText.Text);//这里是网站地址（字符串）,和传入的参数（字符串）顺便得到一个服务器的返回数据
                    //if (result.StartsWith("{") || result.StartsWith("[")) JSONresult = (JsonObject)JsonObject.Parse(result);//判断是否是Json数据
                    //else JSONresult = (JsonObject)JsonObject.Parse("{\"Success\": false,\"Reason\":\"" + result + "\"}");
                    //if (JSONresult["Success"] == true)
                    if (result.Equals("true"))
                    {
                        Android.Widget.Toast.MakeText(this, "注册成功", Android.Widget.ToastLength.Short).Show();
                        var data = new UserTable();
                        data.UserName = UserNameText.Text;
                        data.PassWord = PassWordText.Text;
                        db.Insert(data);
                    }
                    else
                    {
                        Android.Widget.Toast.MakeText(this, "注册失败:"+ result, Android.Widget.ToastLength.Short).Show();
                    }
                }
            };
        }
    }
    /*数据库格式声明*/
    public class UserTable
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
}