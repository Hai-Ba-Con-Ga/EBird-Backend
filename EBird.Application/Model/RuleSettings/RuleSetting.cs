using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.RuleSettings;
using Newtonsoft.Json;

namespace EBird.Api.RuleSettings
{
    public class RuleSetting : IRuleSetting
    {
        private static readonly string _pathFile = "rulesettings.json";
        private RuleSettingModel _ruleSettingModel;

        public RuleSetting()
        {
            LoadData();
        }

        public RuleSettingModel RuleSettingModel => _ruleSettingModel;

        public void LoadData()
        {
            if (!File.Exists(_pathFile))
            {
                throw new Exception("Rule setting file is not found");
            }

            string jsonContent = File.ReadAllText(_pathFile);
            
            _ruleSettingModel = JsonConvert.DeserializeObject<RuleSettingModel>(jsonContent);

            if (_ruleSettingModel == null)
            {
                throw new Exception("Rule setting file is load failed");
            }
            Console.WriteLine($"---------------Text: {_ruleSettingModel.ScoringWeight.KWeightGroup}");
        }
    }
}