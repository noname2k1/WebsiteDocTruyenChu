using DatabaseProvider;
using DBIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebsiteDocTruyenChu.DTOs;
using static WebsiteDocTruyenChu.Controllers.ServiceController;

namespace WebsiteDocTruyenChu.Helpers
{
    public class StaticMethods
    {
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public static bool CheckAdminPageAccess(UserDTO user)
        {
            if (user == null) return false;
            return user.Role == StaticVariables.ROLE_ADMIN || user.Role == StaticVariables.ROLE_MOD;
        }

        public static T RequestBodyConverter<T>(HttpRequestBase Request)
        {
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            System.Diagnostics.Debug.WriteLine(json);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}