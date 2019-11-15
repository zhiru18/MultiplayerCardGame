﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Data.Data;
using Server.Model.Model;

namespace UserManagementServiceTest {
    [TestClass]
    public class CGUserDBTest {
        CGUserDB cgUserDB;
        [TestMethod]
        public void GetAllTest() {
            cgUserDB = new CGUserDB();
            List<CGUser> cgUsers = (List<CGUser>)cgUserDB.GetAll();
            Assert.IsTrue(cgUsers.Count > 0);
        }
        [TestMethod]
        public void GetByIdTest() {
            cgUserDB = new CGUserDB();
            CGUser manny = cgUserDB.GetById("12345678qwerty");
            Assert.AreEqual("12345678qwerty", manny.Id);
        }
        [TestMethod]
        public void GetUserByUserNameTest() {
            cgUserDB = new CGUserDB();
            CGUser anders = cgUserDB.GetUserByUsername("Anders");
            Assert.AreEqual("Anders", anders.UserName);
        }
        [TestMethod]
        public void InsertTest() {
            cgUserDB = new CGUserDB();
            var anders = new CGUser("qwerty12345678", "Anders", "Anders@anders.com", "sufhesfaoiuh", CGUser.UserType.PLAYER, CGUser.UserStatus.STUNNED);
            cgUserDB.Insert(anders);
            var anders2 = cgUserDB.GetById("qwerty12345678");
            Assert.AreEqual(anders.UserName, anders2.UserName);
        }
        [TestMethod]
        public void UpdateTest() {
            cgUserDB = new CGUserDB();
            var anders = new CGUser("qwerty12345678", "Anders", "Anders@anders.com", "sufhesfaoiuh", CGUser.UserType.PLAYER, CGUser.UserStatus.INGAME);
            cgUserDB.Update(anders);
            var anders2 = cgUserDB.GetById("qwerty12345678");
            Assert.AreEqual(anders.userStatus, anders2.userStatus);
        }
    }
}
