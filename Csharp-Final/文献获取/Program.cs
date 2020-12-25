using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace csharp_final_part1
{
    class Program
    {
        static void GetFiles(WebClient webClient,int year)
        {
            string path_p1 = "https://pubmed.ncbi.nlm.nih.gov/?term=colorectal+cancer&filter=years.";
            string path_p2 = "&timeline=expanded";
            string weburl = path_p1 + year + "-" + year + path_p2;//对应年份的结直肠癌资料地址
            Stream stream = webClient.OpenRead(weburl);
            StreamReader reader = new StreamReader(stream);
            string htmlcode = reader.ReadToEnd();//网页源代码生成的字符串
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlcode);//html树

            //至此已获取网页源代码，并将网页结构组织好
            //确定对应年份文献一共有多少页
            int total_num = 0;
            string totalnum_xpath = "//span[@class=\"total-pages\"]";
            HtmlNodeCollection num_nodes = htmlDoc.DocumentNode.SelectNodes(totalnum_xpath);
            foreach(HtmlNode node in num_nodes)
            {
                string temp_total = node.InnerText.Replace(",", "");
                total_num = Int32.Parse(temp_total);
                Console.WriteLine(total_num);
            }

            //目前total_num就是对应年份的文献总页数
            //开始逐页读取对应文献的位置，并将其保存到对应年份的txt文件中
            string url_xpath = "//a[@class=\"docsum-title\"]";
            string file_path = "E:\\Csharp作业\\ALL\\All_urls\\"+year+".txt";
            StreamWriter sw = File.CreateText(file_path);
            if(total_num>1000)
            {
                total_num = 1000;
            }
            for (int i=1;i<=total_num;i++)
            {
                string temp_weburl = weburl + "&page=" + i;
                Stream temp_stream = webClient.OpenRead(temp_weburl);
                StreamReader temp_reader = new StreamReader(temp_stream);
                string temp_htmlcode = temp_reader.ReadToEnd();//网页源代码生成的字符串
                HtmlAgilityPack.HtmlDocument temp_htmlDoc = new HtmlAgilityPack.HtmlDocument();
                temp_htmlDoc.LoadHtml(temp_htmlcode);//html树
                HtmlNodeCollection url_nodes = temp_htmlDoc.DocumentNode.SelectNodes(url_xpath);
                foreach (HtmlNode node in url_nodes)
                {
                    HtmlAttribute url_Href = node.Attributes["href"];
                    string temp_url = url_Href.Value;
                    sw.WriteLine(temp_url);
                }
                Console.WriteLine(i + "页写入完成");
            }
            sw.Close();
            Console.WriteLine("第" + year + "写入完成！");
        }
        static void GetArticles(WebClient webClient,int year)
        {
            string file_path = "E:\\Csharp作业\\ALL\\All_urls\\" + year + ".txt";
            string Article_Path = "E:\\Csharp作业\\ALL\\All_Articles\\" + year + "ArticlesDir";
            string target_head = "https://pubmed.ncbi.nlm.nih.gov";
            string key_xpath = "//div[@class=\"abstract\"]";
            string time_path = "//span[@class=\"cit\"] | /html/body/div[6]/main/header/div[1]/div[3]/div[2]/span[1]/time | //div[@class=\"citation\"]";
            // 
            //获取存有文章位置的txt文件，并创建一个新的文件夹
            if (!Directory.Exists(Article_Path))
            {
                Directory.CreateDirectory(Article_Path);
            }
            StreamReader sr = new StreamReader(file_path);
            string url_tail;
            while((url_tail=sr.ReadLine())!=null)
            {
                string url = target_head + url_tail;//目标位置

                //重复前面的操作
                Stream stream = webClient.OpenRead(url);
                StreamReader reader = new StreamReader(stream);
                string htmlcode = reader.ReadToEnd();//网页源代码生成的字符串
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlcode);//html树

                //准备写入文件的操作
                url_tail = url_tail.Replace("/", "");
                StreamWriter sw = File.CreateText(Article_Path + "\\" + url_tail + ".txt");
                HtmlNodeCollection article_nodes = htmlDoc.DocumentNode.SelectNodes(key_xpath);
                HtmlNodeCollection time_nodes = htmlDoc.DocumentNode.SelectNodes(time_path);
                foreach(HtmlNode node in article_nodes)
                {
                    string keywords = node.InnerText;
                    sw.Write(keywords);
                    //Console.WriteLine(keywords);
                }
                foreach(HtmlNode node in time_nodes)
                {
                    string time = node.InnerText;
                    sw.Write(time);
                    //Console.WriteLine(time);
                    break;
                }
                sw.Close();
                Console.WriteLine(url_tail + "读取完成！");
            }
            sr.Close();
        }
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            //GetFiles(webClient, 2015);
            GetArticles(webClient,2017); 
        }
    }
}
