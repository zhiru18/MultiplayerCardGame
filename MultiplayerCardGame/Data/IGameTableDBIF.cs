﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model.Model;

namespace Server.Data.Data {
    public interface IGameTableDBIF : IGeneralDBIF<GameTable> {
        GameTable GetById(int id);
    }
}
