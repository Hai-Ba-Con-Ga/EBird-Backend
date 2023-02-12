using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Api.RuleSettings;
using EBird.Application.Model.RuleSettings;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class ScoringService : IScoringService
    {
        private static readonly int _win = 1;
        private static readonly int _lose = 0;
        private ScoringWeight _weight;
        private int _vip = 1;

        public ScoringService(IRuleSetting settings)
        {
            _weight = settings.RuleSettingModel.ScoringWeight;
        }

        private double GetEloDifference(double seftElo, double competitorElo)
        {
            return 1 / (1 + 10 * ((competitorElo - seftElo) / 400));
        }

        private double GetWinEloInRoom(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightRoom * (_win + eloDifference)) * _vip;
        }

        private double GetWinEloInGroup(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightGroup * (_win + eloDifference)) * _vip;
        }

        private double GetLoseElo(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightRoom * (_lose + eloDifference));
        }

        public Dictionary<string,double> GetResutlEloInRoom(double winnerElo, double loserElo)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            winnerElo = GetWinEloInRoom(GetEloDifference(winnerElo, loserElo), winnerElo);

            result.Add("winnerElo", winnerElo);

            loserElo = GetLoseElo(GetEloDifference(loserElo, winnerElo), loserElo);

            result.Add("loserElo", loserElo);

            return result;
        }

        public Dictionary<string, double> GetResutlEloInGroup(double winnerElo, double loserElo)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            winnerElo = GetWinEloInGroup(GetEloDifference(winnerElo, loserElo), winnerElo);

            result.Add("winnerElo", winnerElo);

            loserElo = GetLoseElo(GetEloDifference(loserElo, winnerElo), loserElo);

            result.Add("loserElo", loserElo);

            return result;
        }
    }
}