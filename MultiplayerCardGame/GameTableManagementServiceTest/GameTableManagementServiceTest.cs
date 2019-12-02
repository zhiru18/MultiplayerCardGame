﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Data.Data;
using Server.Model.Model;
using Server.Services.GameTableManagementService;

namespace Tests.GameTableManagementServiceTest {
    [TestClass]
    public class GameTableManagementServiceTest {
        [TestMethod]
        public void JoinGameTableTest() {
            GameTableManagement gameTableManagement = new GameTableManagement();
            ICGUserDBIF userDB = new CGUserDB();
            GameTableModel table = gameTableManagement.GetGameTableById(2);
            CGUser cGUser = userDB.GetById("ce2b4942-69bc-4423-a2c0-2b28f53e7817");
            gameTableManagement.JoinGameTable(cGUser, table);
            // TODO: Fix this test.
            //Assert.IsFalse(succeeded);

        }
    }
}
