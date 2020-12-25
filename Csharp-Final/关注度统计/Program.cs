using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace csharp_final_part3
{
    class Program
    {
        static void reset_location(int year)
        {
            //获取给定年度全部文件名
            StreamReader sr_1 = new StreamReader("E:\\Csharp作业\\ALL\\All_urls\\" + year + ".txt");
            string temp_url;
            
            while((temp_url=sr_1.ReadLine())!=null)
            {
                //读取对应文件，获得时间
                string source_file = "E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir\\" + temp_url.Replace("/", "") + ".txt";
                if(!File.Exists(source_file))
                {
                    continue;
                }
                StreamReader sr_2 = new StreamReader("E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir\\" + temp_url.Replace("/", "") + ".txt");
                string target_file;
                char[] charArray = new Char[] { '\r','\n' };
                string[] final_str = sr_2.ReadToEnd().ToString().Split(charArray, StringSplitOptions.RemoveEmptyEntries);
                int len = final_str.Length;
                string[] time = final_str[len-1].Split(new Char[] {';'})[0].Split(new Char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                //根据文件时间是否合规分类
                if (time.Length > 1 && time.Length <= 3 && !time[1].Equals("2020"))
                {
                    target_file = "E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir\\" + time[1];
                    if(!Directory.Exists(target_file))
                    {
                        Directory.CreateDirectory(target_file);
                    }
                    Console.WriteLine(time[1]);
                }
                else
                {
                    target_file = "E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir\\unranked";
                    if (!Directory.Exists(target_file))
                    {
                        Directory.CreateDirectory(target_file);
                    }
                }
                sr_2.Close();
                target_file = target_file + "\\" + temp_url.Replace("/", "") + ".txt";
                File.Move(source_file, target_file);
                // Console.WriteLine(temp_url);
                //Console.WriteLine(time);
                
            }
            sr_1.Close();
        }
        static void Main(string[] args)
        {
            reset_location(2002);
            Console.Read();
        }
    }
}
