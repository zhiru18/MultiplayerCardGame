﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGameClient.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index(int gameTableId) 
        {
            GameServiceAcces gameServiceAcces = new GameServiceAcces();
            GameTable gameTable = gameServiceAcces.GetGameTable(gameTableId);
            Game game= gameServiceAcces.StartGame(gametable);
            return View(game);
        }
    }
}