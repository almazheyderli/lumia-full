using Lumbia.Bussiness.Exceptions;
using Lumbia.Bussiness.Service.Abstracts;
using Lumbia.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lumbia.Areas.Admin.Controllers
{ 
[Area("Admin")]
    public class TeamController : Controller
    {
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    public IActionResult Index()
        {
        var teams = _teamService.GetAllTeams();
            return View(teams);
        }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Team team)
    {
      if(!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            _teamService.AddTeam(team);
        }
        catch(FileSizeException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
        }
        catch (FileContentException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return RedirectToAction(nameof(Index));
    }
       

    public IActionResult Delete(int id)
    {
        var team=_teamService.GetTeam(x=>x.Id==id);
        if (team == null)
        {
            return NotFound();
        }
        _teamService.RemoveTeam(team.Id);
        return RedirectToAction(nameof(Index));
    }
     public IActionResult Update(int id)
    {
        var oldTeam = _teamService.GetTeam(x => x.Id == id);
        if (oldTeam == null)
        {
            return NotFound();
        }
        return View(oldTeam);

    }
    [HttpPost]
    public IActionResult Update(int id,Team team)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        _teamService.UpdateTeam(team.Id, team);
        return RedirectToAction(nameof(Index));
    }
    }
}
