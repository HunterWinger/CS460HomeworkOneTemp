using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace HomeworkOneProject.Models;

public class TeamInfoModel
{
    public string? initialList { get; set; }
    public List<string>? memberList { get; set; }
    public List<List<string>>? teamList { get; set; }
    public int teamSize { get; set; }
    public List<string>? colorPalette { get; set; }
}