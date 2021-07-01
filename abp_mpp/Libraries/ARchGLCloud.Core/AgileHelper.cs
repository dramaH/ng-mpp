using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ARchGLCloud.Core
{
    public class AgileHelper
    {
        /// <summary>  
        /// 该方法用于生成指定位数的随机数  
        /// </summary>  
        /// <param name="VcodeNum">参数是随机数的位数</param>  
        /// <returns>返回一个随机数字符串</returns>  
        public static string GenerateCaptcha(int VCodeNum, bool onlyNumber = true)
        {
            if (onlyNumber)
            {
                string number = "0,1,2,3,4,5,6,7,8,9";
                var numberArray = number.Split(new Char[] { ',' });
                string code = "";//产生的随机数  
                int temp = -1;

                Random rand = new Random();
                //采用一个简单的算法以保证生成随机数的不同  
                for (int i = 1; i < VCodeNum + 1; i++)
                {
                    if (temp != -1)
                    {
                        rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                    }
                    int t = rand.Next(10);//获取随机数  
                    if (temp != -1 && temp == t)
                    {
                        return GenerateCaptcha(VCodeNum);//如果获取的随机数重复，则递归调用  
                    }

                    temp = t;//把本次产生的随机数记录起来  
                    code += numberArray[t];//随机数的位数加一  
                }

                return code;
            }
            else
            {
                //验证码可以显示的字符集合  
                string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
                string code = "";//产生的随机数  
                int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

                Random rand = new Random();
                //采用一个简单的算法以保证生成随机数的不同  
                for (int i = 1; i < VCodeNum + 1; i++)
                {
                    if (temp != -1)
                    {
                        rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                    }
                    int t = rand.Next(62);//获取随机数  
                    if (temp != -1 && temp == t)
                    {
                        return GenerateCaptcha(VCodeNum);//如果获取的随机数重复，则递归调用  
                    }

                    temp = t;//把本次产生的随机数记录起来  
                    code += VcArray[t];//随机数的位数加一  
                }

                return code;
            }
        }
        /// <summary>
        /// 判断是否是手机浏览器
        /// </summary>
        /// <param name="userAgent">用户代理</param>
        /// <returns></returns>
        public static bool IsMobile(string userAgent)
        {
            Regex regex = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regex.IsMatch(userAgent);
        }
        /// <summary>
        /// 判断是否为iOS移动设备
        /// </summary>
        /// <param name="userAgent">用户代理</param>
        /// <returns></returns>
        public static bool IsiOS(string userAgent)
        {
            var regex = new Regex("(iPhone|iPad|iPod|iOS)", RegexOptions.IgnoreCase);
            return regex.IsMatch(userAgent);
        }
        /// <summary>
        /// 判断是否为Android设备
        /// </summary>
        /// <param name="userAgent">用户代理</param>
        /// <returns></returns>
        public static bool IsAndroid(string userAgent)
        {
            var regex = new Regex("(Android)", RegexOptions.IgnoreCase);
            return regex.IsMatch(userAgent);
        }
    }
}
