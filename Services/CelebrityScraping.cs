using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Celebrities.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Celebrities.Services
{
    public class CelebrityScraping : ICelebrityScraping
    {
        IConfiguration config;
        public CelebrityScraping(IConfiguration config)
        {
            this.config = config;
 
        }


        public async Task<Dictionary<string, Celebrity>> CreateDataFile()
        {
            string jsonFilePath = config["Celebrities:Path"];
            string url = config["Celebrities:Url"];
            Dictionary<string, Celebrity> celebrityDic = new Dictionary<string, Celebrity>();
            Celebrity celebrity = null;

            HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(url);


            HtmlNode htmlNode = null; HtmlNode item = null;
            string description;

            var nodes = doc.DocumentNode.SelectNodes("//div[@class='lister-item mode-detail']").ToList();
            //
            for (int i = 0; i < nodes.Count; i++)
            {
                item = nodes[i];
                celebrity = new Celebrity
                {
                    Id = Guid.NewGuid().ToString()
                };

                celebrity.Name = item.SelectNodes("//h3[@class='lister-item-header']").FirstOrDefault().ChildNodes["a"].InnerText.Trim();

                htmlNode = item.SelectNodes("//p[@class='text-muted text-small']").FirstOrDefault();
                description = htmlNode.NextSibling.NextSibling.InnerText.ToLower();

                celebrity.Profession = htmlNode.InnerText.Split(new string[] { "\r\n", "|" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                SetGender(celebrity, description);

                SetBirthday(celebrity, description);
                celebrity.ImageUrl = item.SelectNodes("//div[@class='lister-item-image']").FirstOrDefault().ChildNodes["a"].ChildNodes["img"].Attributes["src"].Value;

                celebrityDic.Add(celebrity.Id, celebrity);
                item.Remove(); //correct document, error is found.
            }

            await System.IO.File.WriteAllTextAsync(jsonFilePath, JsonConvert.SerializeObject(celebrityDic));
            return celebrityDic;
        }

        private static void SetGender(Celebrity celebrity, string description)
        {
            if (celebrity.Profession.ToLower() == "actor")
                celebrity.Gender = "male";
            else
                if (celebrity.Profession.ToLower() == "actress")
                    celebrity.Gender = "female";
                else
                {
                    int startDateInd = description.IndexOf(" he ");
                    if (startDateInd != -1)
                        celebrity.Gender = "male";
                    else
                    {
                        startDateInd = description.IndexOf(" his ");
                        if (startDateInd != -1)
                            celebrity.Gender = "male";
                        else
                        {
                            startDateInd = description.IndexOf(" her ");
                            if (startDateInd != -1)
                                celebrity.Gender = "female";
                            else
                            {
                                startDateInd = description.IndexOf(" she ");
                                if (startDateInd != -1)
                                    celebrity.Gender = "female";
                            }
                        }
                    }
                }
        }

        private static void SetBirthday(Celebrity celebrity, string description)
        {
            int startDateInd = description.IndexOf("was born on");
            if (startDateInd != -1)
            {
                startDateInd = startDateInd + 11;
                int EndDateInd = description.IndexOf("in", startDateInd + 10) - 1;
                if ((EndDateInd - startDateInd + 1) <= 20)
                    celebrity.Birthday = Convert.ToDateTime(description.Substring(startDateInd, (EndDateInd - startDateInd + 1)));
            }
        }


        public void DeleteDataFile()
        {
            string jsonFilePath = config["Celebrities:Path"];
            if (File.Exists(jsonFilePath))
                File.Delete(jsonFilePath);
        }
    }
}
