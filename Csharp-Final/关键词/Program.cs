using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCloudSharp;
using System.IO;
using System.Text.RegularExpressions;
namespace csharp_final_part4
{
    class Program
    {
        static List<String> get_all_list(string File_Folder)
        {
            //目标文件夹
            int i;
            DirectoryInfo target_dir = new DirectoryInfo(File_Folder);
            DirectoryInfo[] son_dirs = new DirectoryInfo[3];
            //目标文件夹系统信息
            son_dirs = target_dir.GetDirectories();
            FileSystemInfo[][] Season_fsinfos = new FileSystemInfo[3][];
            for (i = 0; i < 3; i++)
            {
                Season_fsinfos[i] = son_dirs[i].GetFiles();
            }
            //总列表
            List<string> total_list = new List<string>();
            //遍历目标文件夹的每个文件
            foreach (FileSystemInfo[] Month_fsinfos in Season_fsinfos)
            {
                foreach (FileSystemInfo fsinfo in Month_fsinfos)
                {
                    //指向每个文件的文件阅读器
                    StreamReader sr = new StreamReader(fsinfo.FullName);
                    //文件名里的分割字符
                    //char[] charArray = new Char[] { ' ', '\r', '\n', ',', ':', ';', '.', '(', ')', '±', '=', '%' };
                    //把文件中的每个单词都输入到temp表中
                    //List<string> temp = sr.ReadToEnd().Split(charArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                    //输入流关闭
                    String[] stringArray = new String[] {"\r", "\n", ",", ";", "Keywords", ":"," ","s:" };
                    string temp = sr.ReadToEnd().ToString();
                    i = temp.LastIndexOf("Keywords");
                    if (i != -1)
                        temp = temp.Substring(i, 60);
                    else
                        continue;
                    List<string> liststr = temp.Split(stringArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                    sr.Close();
                    //如果文章单词数目大于20就把该文章的temp表加入list
                    if (total_list.Count <= 1000)
                        total_list.AddRange(liststr);
                }
            }
            return total_list;
        }

        static void Func(string year, string season)
        {
            int flag = 1;
            string File_Folder = "E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir\\" + season;
            List<string> total_list = get_all_list(File_Folder);
            //单词表
            List<string> words = new List<string>();
            //频率表
            List<int> frequencies = new List<int>();
            var mask = "E:\\Csharp作业\\back.jpg";
            var wc = new WordCloud(3000, 3000, mask: System.Drawing.Image.FromFile(mask), allowVerical: true, fontname: "结直肠癌");
            //遍历result里面的字典
            char[] trimChars = { '\n', ' ','\r'};
            foreach (string str in total_list)
            {
                if (!words.Contains(str)&&str.Length>2)
                {
                    words.Add(str.Trim(trimChars));
                    frequencies.Add(1);
                }
            }
            foreach (string str in words)
            {
                Console.WriteLine(str);
            }
            var image = wc.Draw(words, frequencies);
            image.Save("E:\\Csharp作业\\ALL\\All_clouds\\" + year + "Clouds\\Keywords" + year + season + ".jpeg");
        }
        static void Main(string[] args)
        {
            int i, j;
            Console.WriteLine("123123123");
            string[] years = new string[21];
            for (i = 0; i < 21; i++)
            {
                years[i] = (2000 + i).ToString();
                Console.WriteLine(years[i]);
            }
            string[] season = new string[5];
            season[1] = "_Spring";
            season[2] = "_Summer";
            season[3] = "_Autumn";
            season[4] = "_Winter";
            for (j = 15; j <= 19; j++)
            {
                Console.WriteLine(years[j] + "开始了");
                for (i = 1; i <= 4; i++)
                {
                    Console.WriteLine(season[i] + "开始了");
                    Func(years[j], season[i]);
                }
                Console.WriteLine(years[j] + "结束了");
            }
            Console.Read();
        }
    }
}
