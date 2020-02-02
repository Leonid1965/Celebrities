using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Celebrities.Models;
using Celebrities.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Celebrities.Controllers
{

    public class CelebrityController : Controller
    {

        private readonly ILogger<CelebrityController> _logger;
        private readonly ICelebrityScraping _celebrityScraping;
        private readonly IDataManager _dataManager;
        private readonly IConfiguration _config;

        public CelebrityController(ILogger<CelebrityController> logger, ICelebrityScraping celebrityScraping, IDataManager dataManager, IConfiguration config)
        {
            _logger = logger;
            _celebrityScraping = celebrityScraping;
            _dataManager = dataManager;
            _config = config;
        }

        //http://localhost:49175/celebrity/all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Celebrity>>> All()
        {

            Dictionary<string, Celebrity> celebrityDic = null;
            string jsonFilePath = _config["Celebrities:Path"];

            if (!System.IO.File.Exists(jsonFilePath))
                celebrityDic = await _celebrityScraping.CreateDataFile();
            else
                celebrityDic = await _dataManager.GetAllCelebrities();

            return celebrityDic.Values;
        }

        //http://localhost:49175/celebrity/delete?id=1
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Celebrity>>> Delete(string id)
        {
            Dictionary<string, Celebrity> celebrityDic = await _dataManager.DeleteSelebrity(id);

            return celebrityDic.Values;
        }


        //http://localhost:49175/celebrity/reset
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Celebrity>>> Reset()
        {
            _celebrityScraping.DeleteDataFile();
            Dictionary<string, Celebrity> celebrityDic = await _celebrityScraping.CreateDataFile();

            return celebrityDic.Values;
        }

        //http://localhost:49175/celebrity/add
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Celebrity>>> Add(string name, DateTime? birthday, string sex, string profession, string imageUrl)
        {
            Celebrity celebrity = new Celebrity { Name = name, Birthday = birthday, Gender = sex, Profession = profession, ImageUrl = imageUrl};
            Dictionary<string, Celebrity> celebrityDic = await _dataManager.AddSelebrity(celebrity);

            return celebrityDic.Values;
        }

        //http://localhost:49175/celebrity/update
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Celebrity>>> Update(string id, string name, DateTime? birthday, string sex, string profession, string imageUrl)
        {

            Celebrity celebrity = new Celebrity { Id = id, Name = name, Birthday = birthday, Gender = sex, Profession = profession, ImageUrl = imageUrl };
            Dictionary<string, Celebrity> celebrityDic = await _dataManager.UpdateSelebrity(celebrity);

            return celebrityDic.Values;
        }

    }
}
