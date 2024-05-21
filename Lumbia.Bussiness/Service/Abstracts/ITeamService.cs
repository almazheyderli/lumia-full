using Lumbia.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lumbia.Bussiness.Service.Abstracts
{
    public  interface  ITeamService
    {
       void AddTeam(Team team);  
        void UpdateTeam(int id, Team team); 
        void RemoveTeam (int id);
        Team GetTeam(Func<Team, bool>? func = null);
        List<Team> GetAllTeams(Func<Team,bool>? func=null);
    }
}
