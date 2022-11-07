using System.Net;
using FastMember;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace web_crawler_tele.Services;

public class CrawlingDataService
{
    public async void GetDataFromWeb()
    {
        string url = "http://q.isentric.com:8001/FtpPull/CheckQServlet";

        List<List<string>> table = LoadHtmlAsync(url).Result.DocumentNode.SelectNodes("//table[@bordercolor='#0000FF']")
                 .Descendants("tr")
                 .Where(tr => tr.Elements("td").Count() > 1)
                 .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                 .ToList();

        foreach (var item in table)
        {
            System.Console.WriteLine(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)));
        }

    }

    public async Task<HtmlDocument> LoadHtmlAsync(string url)
    {
        HttpClient httpClient = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        var html = await httpClient.GetStringAsync(url);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        return htmlDoc;
    }
}

