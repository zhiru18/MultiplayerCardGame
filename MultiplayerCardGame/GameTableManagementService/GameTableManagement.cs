﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;
using Server.Converters.DataContractConverters;
using Server.Data.Data;
using Server.DataContracts.DataContracts;
using Server.Model.Model;
using Server.Services.GameTableManagementService.Contracts;
using Server.Services.UserManagementService;

namespace Server.Services.GameTableManagementService {
    /* This service takes care of creating, deleting, updating and getting GameTables out of the database. 
     * Furthermore it allows users to join a table  
     */
    public class GameTableManagement : IGameTableManagementService {
        IGameTableDBIF gameTableDB = new GameTableDB();
        UserManagement userManagement = new UserManagement();
        public GameTable CreateGameTable(CGUser user, string tableName) {
            if (user == null || tableName == null) {
                throw new ArgumentNullException();
            } else {
                GameTableModel tableModel = new GameTableModel() {
                    TableName = tableName,
                    DeckId = 2,
                    seats = 4
                };
                gameTableDB.Insert(tableModel);
                GameTable table = GameTableConverter.ConvertFromGameTableModelToGameTable(gameTableDB.GetGameTableByTableName(tableName));
                return JoinGameTable(user, table);
            }
        }

        public bool DeleteGameTable(int id) {
            if (id == 0) {
                throw new ArgumentException();
            } else {
                bool res = false;
                GameTable table = GameTableConverter.ConvertFromGameTableModelToGameTable(gameTableDB.GetById(id));
                if (table != null) {
                    gameTableDB.Delete(GameTableConverter.ConvertFromGameTableToGameTableModel(table));
                    res = true;
                }
                return res;
            }
        }

        public IEnumerable<GameTable> GetAll() {
            return GameTableConverter.ConvertFromListOfGameTableModelToListOfGameTable((List<GameTableModel>)gameTableDB.GetAll());
        }

        public GameTable GetGameTableById(int id) {
            if (id == 0) {
                throw new ArgumentException();
            } else {
                return GameTableConverter.ConvertFromGameTableModelToGameTable(gameTableDB.GetById(id));
            }
        }
        /* This method is called when a user want to join a GameTable. 
         * It checks wether or notthe table is already full and if it is the user cannot join the table.
         * if a user is successfull in joining a table, a seat is subtracted from the seat count,
         * such that the it can keep track of how many users have joined the table
         */
        public GameTable JoinGameTable(CGUser user, GameTable chosenTable) {
            if (user == null || chosenTable == null) {
                throw new ArgumentNullException();
            } else {
                GameTable databaseTable = null;
                try {
                    using (TransactionScope scope = new TransactionScope()) {
                        CGUserModel userModel = CGUserConverter.ConvertFromCGUserToCGUserModel(user);
                        if (userModel.TableID != 0 && userModel.TableID != chosenTable.Id) {
                            GameTableModel modelTable = gameTableDB.GetById(userModel.TableID);
                            if(modelTable != null) {
                                gameTableDB.UpdateGameTableSeats(modelTable, -1);
                            }
                        }
                        databaseTable = GameTableConverter.ConvertFromGameTableModelToGameTable(gameTableDB.GetById(chosenTable.Id));
                        if (chosenTable.seats == databaseTable.seats && databaseTable.Users.Count < 4) {
                            userManagement.UpdateUserTableId(user, databaseTable.Id);
                            databaseTable.Users.Add(user);
                            UpdateGameTableSeats(databaseTable, 1);
                        }
                        scope.Complete();
                    }
                } catch (TransactionAbortedException tae) {

                    throw;
                }
                return databaseTable;
            }
        }

        public GameTable GetGameTableByTableName(string name) {
            if (name == null) {
                throw new ArgumentNullException();
            } else {
                return GameTableConverter.ConvertFromGameTableModelToGameTable(gameTableDB.GetGameTableByTableName(name));
            }
        }

        public void UpdateGameTableSeats(GameTable gameTable, int amount) {
            gameTableDB.UpdateGameTableSeats(GameTableConverter.ConvertFromGameTableToGameTableModel(gameTable), amount);
        }
    }
}
