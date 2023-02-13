using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IScoringService
    {
       public Dictionary<string, double> GetResutlEloInRoom(double winnerElo, double loserElo);
       public Dictionary<string, double> GetResutlEloInGroup(double winnerElo, double loserElo);
    }
}