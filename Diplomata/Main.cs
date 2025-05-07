using Godot;
using System;

namespace Diplomata
{
    public partial class Main : Node
    {

        public static Main Instance { get; private set; }

        private Database _database;

        public Database Database {
            get {
                if (_database == null) {
                    _database = new Database();
                    _database.Database.EnsureCreated();
                }
                return _database;
            }
        }

        public override void _Ready()
        {
            Instance = this;
        }
    }
}
