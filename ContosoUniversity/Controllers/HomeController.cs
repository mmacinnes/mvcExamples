﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        
        if (User.Identity?.IsAuthenticated == true) 
        {
            ViewBag.UserName = User.Identity.Name;
        }
        else
        {
            ViewBag.UserName = "";
        }

        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
