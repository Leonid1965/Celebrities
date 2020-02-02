using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Celebrities.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Celebrities.Services
{
    public class DataManager : IDataManager
    {
        IConfiguration config;
        public DataManager(IConfiguration config)
        {
            this.config = config;
 
        }

        public async Task<Dictionary<string, Celebrity>> AddSelebrity(Celebrity celebrity)
        {
            Dictionary<string, Celebrity> celebrityDic = await GetAllCelebrities();
            if (string.IsNullOrEmpty(celebrity.Id))
                celebrity.Id = Guid.NewGuid().ToString();

            celebrityDic.Add(celebrity.Id, celebrity);
            string jsonFilePath = config["Celebrities:Path"];
            await System.IO.File.WriteAllTextAsync(jsonFilePath, JsonConvert.SerializeObject(celebrityDic));

            return celebrityDic;
        }

        public async Task<Dictionary<string, Celebrity>> DeleteSelebrity(string Id)
        {
            Dictionary<string, Celebrity> celebrityDic = await GetAllCelebrities();
            if (celebrityDic.ContainsKey(Id))
            {
                celebrityDic.Remove(Id);
                string jsonFilePath = config["Celebrities:Path"];
                await System.IO.File.WriteAllTextAsync(jsonFilePath, JsonConvert.SerializeObject(celebrityDic));
            }
            return celebrityDic;

        }

        public async Task<Dictionary<string, Celebrity>> GetAllCelebrities()
        {
                string jsonFilePath = config["Celebrities:Path"];
                string data = await System.IO.File.ReadAllTextAsync(jsonFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, Celebrity>>(data);
        }

        public async Task<Celebrity> GetSelebrity(string Id)
        {
            Dictionary<string, Celebrity> celebrityDic = await GetAllCelebrities();
            Celebrity celebrity = null;
            if (celebrityDic.ContainsKey(Id))
                celebrity = celebrityDic[Id];
            return celebrity;
        }

        public async Task<Dictionary<string, Celebrity>> UpdateSelebrity(Celebrity celebrity)
        {
            Dictionary<string, Celebrity> celebrityDic = await GetAllCelebrities();
            if (celebrityDic.ContainsKey(celebrity.Id))
            {
                celebrityDic[celebrity.Id] = celebrity;
                string jsonFilePath = config["Celebrities:Path"];
                await System.IO.File.WriteAllTextAsync(jsonFilePath, JsonConvert.SerializeObject(celebrityDic));
            }
            else
                throw new Exception("Celebrity Id is not found.");

            return celebrityDic;
        }

   
    }
}
