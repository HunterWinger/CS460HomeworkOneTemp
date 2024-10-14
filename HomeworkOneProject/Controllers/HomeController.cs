using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeworkOneProject.Models;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace HomeworkOneProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult TeamBuilder(TeamInfoModel t)
    {
        // First check if initialList is empty, if so then we can't do anything else so return error
        if (string.IsNullOrEmpty(t.initialList))
        {
            ModelState.AddModelError("initialList", "List of team names cannot be empty, please enter names one per line.");
        }
        else if (!string.IsNullOrEmpty(t.initialList))
        {
            // Set our member list to an empty list of strings to hold our spliced names.
            t.memberList = new List<string>();
            // Take our initial string, split on each newline character and place into our list of names. If we find an empty entry, then we get rid of it.
            t.memberList.AddRange(t.initialList.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));

            // Validate each name in the memberList
            foreach (string name in t.memberList)
            {
                if (!IsValidName(name))
                {
                    ModelState.AddModelError(nameof(t.initialList), $"Invalid name: '{name}'. Please ensure you only use letters, spaces, and the characters , . - _ '");
                }
            }
        }


        if (t.teamSize < 2 || t.teamSize > 10)
        {
            ModelState.AddModelError("teamSize", "Please enter a team size that is between 2 and 10");
        }

        // If our model is invalid, then return the current view with model
        if (!ModelState.IsValid)
        {
            /** More error debugging
            foreach (var error in ModelState.Values)
            {
                foreach (var err in error.Errors)
                {
                    Console.WriteLine(err.ErrorMessage);
                }
            }
            */
            // There is a model error, return view with model
            return View("Index", t);
        }
        // Form our teams using the function defined later in this file
        t.teamList = MakeTeams(t.memberList, t.teamSize);
        t.colorPalette = new List<string> { "#4E6BA6", "#938FB8", "#D8B5BE", "#398AA2", "#1E7590", "#FD9A4D", "#FCB274", "#FBCA9A", "#FCDFAA", "#FCF3B9" };
        // Success! Proceed to TeamBuilder page with model
        return View("TeamBuilder", t);
    }

    /**
    * Takes a name and uses a regex pattern match to return true or false if the name has non-permitted characters
    * Trim the name to remove whitespace before validating
    */
    static bool IsValidName(string name)
    {
        name = name.Trim();
        // Take name, check if it matches the following regex. Return true or false.
        string pattern = @"^[a-zA-Z ,.\-_'’\n]+$";
        return Regex.IsMatch(name, pattern);
    }

    public List<List<string>> MakeTeams(List<string> names, int teamSize)
    {
        //Make List to hold List of names for teams
        List<List<string>> teamLists = new List<List<string>>();

        // Create a new list to store a randomized version of the names we took from the home page, this allows us to easily use LINQ seen below to create teams.
        Random random = new Random();
        // Note: This is fine for this use case, in future use something in-place to save space with larger data sets
        List<string> randomizedNames = names.OrderBy(x => random.Next()).ToList();
        // Loop through the list of randomized names start at 0 and increase by team size. Allows us to use LINQ to pick members while skipping those we've already picked.
        for (int i = 0; i < names.Count; i += teamSize)
        {
            // Create a team by skipping those we've already picked (using i) and taking a new teamSize of those, then use ToList()
            List<string> team = names.Skip(i).Take(teamSize).ToList();
            teamLists.Add(team);
        }

        return teamLists;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
