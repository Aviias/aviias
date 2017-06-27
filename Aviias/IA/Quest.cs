﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    [Serializable]
    public class Quest
    {
        Dictionary <string, int> _reward;
        int _type;
        int _idStartNpc;
        public NPC _startNpc;
        int _idEndNpc;
        string _spitch;
        int _id;
        internal Dictionary<string, int> _goal;
        [field: NonSerialized]
        Random rand = new Random();

        public Quest(int type, int idStartNpc, int idEndNpc, NPC startNPC)
        {
            _type = type;
            _id = ++_id;
            _idStartNpc = idStartNpc;
            _idEndNpc = idEndNpc;
            _goal = new Dictionary<string, int>(4);
            _reward = new Dictionary<string, int>(2);
             _goal.Add("dirt", 8);
            _startNpc = startNPC;
            CreateSpitch();
            CreateReward();
        }

        internal bool CheckGoal(Player player, int npcId)
        {
            if (_type == 0)
            {
                foreach (KeyValuePair<Ressource, int> entry in player.Inventory)
                {
                    if (_goal.ContainsKey(entry.Key.Name) && entry.Value > _goal["dirt"]) return true;
                }
                return false;
            }
          /*  else
            {
                if (_idEndNpc == npcId)
                {
                    return true;
                }
            }*/
            return false;
        }

        void CreateReward()
        {
            _reward.Add("dirt", rand.Next(0, 30));
            _reward.Add("stone", rand.Next(0, 20));
            _reward.Add("heal_potion", 1);
        }

        void CreateSpitch()
        {
            if (_type == 0)
            {
                string goal = "Salut toi ! ";
                goal += "Ramene moi ces ressources ";
                goal += "et ne demande pas pourquoi.\n";
                foreach (KeyValuePair<string, int> entry in _goal)
                {
                    goal += entry.Key + " x " + entry.Value + "\n";
                }
                _spitch = goal;
            }
            else
            {
                string goal = "Salut toi !\n";
                goal += "Peux-tu aller voir d ";
                goal += "pour moi ?\n";
                _spitch = goal;
            }
        }

        internal void GetReward(Player player)
        {
            foreach(KeyValuePair<string, int> entry in _reward)
            {
                 player._inv.AddInventory(entry.Value, entry.Key);
            }
        }

        public string Spitch => _spitch;

        public int EndNpc => _idEndNpc;

        public int StartNpc => _idStartNpc;

        public int Type => _type;
    }
}
