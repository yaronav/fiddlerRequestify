using System;
using System.Net;
using System.Text;

namespace requestify
{
    public static class CodeGenerator
    {
        public static string GeneratePythonCode(Fiddler.Session session)
        {

            var requestMethod = session.RequestMethod.ToLower();
            var fullUrl = session.fullUrl;

            StringBuilder sb = new StringBuilder();

            WriteLine(sb,"import requests");

            WriteLine(sb,"headers = {}");
            foreach (var header in session.RequestHeaders)
            {
                if (header.Name.ToLower() == "cookie")
                {
                    // cookies will be handled differently
                    continue;
                }
                WriteLine(sb, string.Format($"headers['{header.Name}'] = '{header.Value}'", header.Name, header.Value));
            }


            WriteLine(sb,"cookies = {}" );

            if (session.RequestHeaders["Cookie"] != null)
            {

                WriteLine(sb, "jar = requests.cookies.RequestsCookieJar()");

                CookieContainer cookieContainer = new CookieContainer();
                var uri = new Uri(session.fullUrl);

                // .Net CookieContainer has a stupid bug where it expects comma instead of semi-column as a delimiter
                cookieContainer.SetCookies(uri, session.RequestHeaders["Cookie"].Replace(";", ","));

                foreach (Cookie cookie in cookieContainer.GetCookies(uri))
                {
                    string name = cookie.Name;
                    string value = cookie.Value;
                    WriteLine(sb, string.Format($"jar.set('{name}','{value}')", name, value));
                }
            }

            if (requestMethod == "post")
            {                
                var payload = UnicodeEncoding.UTF8.GetString(session.RequestBody);
                WriteLine(sb, string.Format($"data = b'{payload}'", payload));
                
            }
            else
            {
                WriteLine(sb, "data = {}");
            }

            WriteLine(sb, string.Format($"requests.{requestMethod}('{fullUrl}',headers = headers,cookies = cookies,data=data)", requestMethod, fullUrl));
            return sb.ToString().TrimEnd();
        }



        private static void WriteLine(StringBuilder sb, string code)
        {
            sb.Append(code + Environment.NewLine);
        }
    }
}

