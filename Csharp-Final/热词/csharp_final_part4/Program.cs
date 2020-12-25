using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCloudSharp;
using System.IO;
namespace csharp_final_part4
{
    class Program
    {
        static List<List<String>> get_all_list(string File_Folder)
        {
            DirectoryInfo target_dir = new DirectoryInfo(File_Folder);
            FileSystemInfo[] fsinfos = target_dir.GetFileSystemInfos();
            List<List<string>> total_list = new List<List<string>>();
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                StreamReader sr = new StreamReader(fsinfo.FullName);
                char[] charArray = new Char[] { ' ', '\r', '\n', ',', ':', ';', '.', '(', ')', '±', '=','%' };
                List<string> temp = sr.ReadToEnd().Split(charArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                sr.Close();
                if (temp.Count() >= 20)
                    total_list.Add(temp);
            }
            return total_list;
        }
        static double IDF(List<List<String>> total_list, string target)
        {
            double IDF = 0;
            double all_num = total_list.Count(); //所有文件数量
            double target_num = 0; //包含target文件的数量
            foreach (List<string> word_list in total_list)
            {
                if (word_list.Contains(target))
                {
                    target_num++;
                }
            }
            IDF = Math.Log(all_num / (target_num + 1));
            return IDF;
        }
        static List<Dictionary<string, double>> TF_IDF(List<List<string>> total_list)
        {
            List<Dictionary<string, double>> final_result = new List<Dictionary<string, double>>();
            foreach (List<string> word_list in total_list)
            {
                Dictionary<string, double> temp = new Dictionary<string, double>();
                double temp_total_num = word_list.Count();
                foreach (string word in word_list)
                {
                    double temp_target_num = 0;
                    foreach (string str in word_list)
                    {
                        if (word.Equals(str))
                        {
                            temp_target_num++;
                        }
                    }
                    double tmp_TF = temp_target_num / temp_total_num;
                    double tmp_IDF = IDF(total_list, word);
                    double tmp_TF_IDF = tmp_IDF * tmp_TF;
                    if (!temp.ContainsKey(word))
                    {
                        temp.Add(word, tmp_TF_IDF);
                    }
                }
                final_result.Add(temp.OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value));
            }
            return final_result;
        }
        static void Func(string year, string month)
        {
            string File_Folder = "E:\\Csharp作业\\ALL\\All_Articles\\"+year+"ArticlesDir\\"+month;
            List<List<string>> total_list = get_all_list(File_Folder);
            List<Dictionary<string, double>> result = TF_IDF(total_list);

            List<string> words = new List<string>();
            List<int> frequencies = new List<int>();
            var mask = "E:\\Csharp作业\\back.jpg";
            var wc = new WordCloud(3000, 3000, mask: System.Drawing.Image.FromFile(mask), allowVerical: true, fontname: "结直肠癌");

            foreach (Dictionary<string, double> dic in result)
            {

                words.Add(dic.ElementAt(0).Key);
                frequencies.Add(Convert.ToInt32(dic.ElementAt(0).Value * 100));


            }
            foreach (KeyValuePair<string, double> kvp in result[3])
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            var image = wc.Draw(words, frequencies);
            image.Save("E:\\Csharp作业\\ALL\\All_clouds\\" + year + "Clouds\\" + year +month+".jpeg");
            Console.WriteLine(month+"输出完毕");
        }

        static void Main(string[] args)
        {
            int i,j;
            string[] years = new string[21];
            for (i = 0; i < 21; i++)
            {
                years[i] = (2000 + i).ToString();
                Console.WriteLine(years[i]);
            }
            string[] month = new string[13];
            month[1] = "Jan";
            month[2] = "Feb";
            month[3] = "Mar";
            month[4] = "Apr";
            month[5] = "May";
            month[6] = "Jun";
            month[7] = "Jul";
            month[8] = "Aug";
            month[9] = "Sep";
            month[10] = "Oct";
            month[11] = "Nov";
            month[12] = "Dec";
            for (j = 2; j <= 2; j++)
            {
                Console.WriteLine(years[j] + "开始了");
                for (i = 10; i <= 12; i++)
                {
                    Console.WriteLine(month[i]+"开始了");
                    Func(years[j], month[i]);
                }
                Console.WriteLine(years[j] + "结束了");
            }
            Console.Read();
        }
    }

}
