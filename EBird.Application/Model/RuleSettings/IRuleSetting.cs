using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.RuleSettings;

namespace EBird.Api.RuleSettings
{
    public interface IRuleSetting
    {
        public RuleSettingModel RuleSettingModel{ get; } 
    }
}