using Lumbia.Bussiness.Exceptions;
using Lumbia.Bussiness.Service.Abstracts;
using Lumbia.Core.Models;
using Lumbia.Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbia.Bussiness.Service.Concretes
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

      
        public void AddTeam(Team team)
        {
            if (team.ImgFile== null)
            {
                throw new ArgumentNullException("ImgFile", "empty file");
            }
            if (team.ImgFile.Length > 2097125)
            {
                throw new FileSizeException("ImgFile", "olcu boyukdur");
            }
            if (!team.ImgFile.ContentType.Contains("image/"))
                {
                throw new FileContentException("ImgFile", "zehmet olmasa sekil daxil edin");
            }
            string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + team.ImgFile.FileName;
            using (FileStream stream =new FileStream(path, FileMode.Create))
            {
                team.ImgFile.CopyTo(stream);
            }
            team.ImgUrl = team.ImgFile.FileName;
           _teamRepository.Add(team);
            _teamRepository.Commit();
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _teamRepository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _teamRepository.Get(func);
        }
        public void RemoveTeam(int id)
        {
            var team=_teamRepository.Get(x=>x.Id == id);
            if(team == null)
            {
                throw new Exception(); 
            }
            string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + team.ImgUrl;
            if(!File.Exists(path))
            {
                throw new TeamNotFoundException("ImgUrl", "null");
            }
            File.Delete(path);
            _teamRepository.Remove(team);
            _teamRepository.Commit();
        }
        public void UpdateTeam(int id, Team team)
        {
            var oldTeam=_teamRepository.Get(x=>x.Id == id);
            if(oldTeam == null)
            {
                throw new NullReferenceException();
            }
            if (team.ImgFile != null)
            {
                string filename = team.ImgFile.FileName;
                string path = _webHostEnvironment.WebRootPath+ @"/Upload/Service/" + team.ImgFile.FileName;
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    team.ImgFile.CopyTo(stream);
                }
                FileInfo fileInfo = new FileInfo(path + oldTeam.ImgUrl);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                oldTeam.ImgUrl = filename;
            }
            oldTeam.Name = team.Name;
            oldTeam.Description = team.Description;
            oldTeam.Position=team.Position;
            _teamRepository.Commit();
        }
    }
}

