using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Api.RuleSettings;
using EBird.Application.Model.RuleSettings;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class ScoringService
    {
        private static readonly int _win = 1;
        private static readonly int _lose = 0;
        private static ScoringWeight _weight = new ScoringWeight(){KWeightRoom = 64, KWeightGroup = 128};
        private static int _vip = 1;

        public ScoringService()
        {
            // _weight = settings.RuleSettingModel.ScoringWeight;
        }

        private static double GetEloDifference(double seftElo, double competitorElo)
        {
            return 1 / (1 + Math.Pow(10, ((competitorElo - seftElo) / 400)));
        }

        private static double GetWinEloInRoom(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightRoom * (_win + eloDifference)) * _vip;
        }

        private static double GetWinEloInGroup(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightGroup * (_win + eloDifference)) * _vip;
        }

        private static double GetLoseElo(double eloDifference, double currentElo)
        {
            return currentElo + (_weight.KWeightRoom * (_lose + eloDifference));
        }

        public static Dictionary<string,double> GetResutlEloInRoom(double winnerElo, double loserElo)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            winnerElo = GetWinEloInRoom(GetEloDifference(winnerElo, loserElo), winnerElo);

            result.Add("winnerElo", winnerElo);

            loserElo = GetLoseElo(GetEloDifference(loserElo, winnerElo), loserElo);

            result.Add("loserElo", loserElo);

            return result;
        }

        public static Dictionary<string, double> GetResutlEloInGroup(double winnerElo, double loserElo)
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