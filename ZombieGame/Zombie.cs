using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Lucas Ghigli
// 08/28/2022
// Zombie Infestation Game
// Zombie.cs

namespace ZombieGame
{
    class Zombie
    {
        private int x;
        private int y;
        private bool isBloodSucker;
        private int id;
        public static List<Zombie> Zombies;
        private List<Human> humansinfected=new List<Human>();

        public Zombie(bool isBloodSucker, int id,int x,int y)
        {
            this.IsBloodSucker = isBloodSucker;
            this.Id = id;
            this.x = x;
            this.y = y;
        }
        
        public static void addzombies(int rows, int cols, int num, string[,] table)
        {
            bool isBloodSucker = true;
            Zombies = new List<Zombie>();
            Random R = new Random();
            for (int i = 0; i < num; i++)
            {
                int row = R.Next(0, rows);
                int col = R.Next(0, cols);
                while (table[row, col] != "" && table[row, col] != null)
                {
                    row = R.Next(0, rows);
                    col = R.Next(0, cols);
                }
                table[row, col] = $"H{i + 1}";
                int rand = R.Next(0, 10);
                if (rand % 2 == 0)
                    isBloodSucker = false;
                Zombie Z = new Zombie(isBloodSucker,i+1,row,col);
                Zombies.Add(Z);
                table[row, col] = $"Z{i + 1}";
            }
        }
        public static Zombie getZombieById(int id)
        {
            foreach(Zombie Z in Zombie.Zombies)
            {
                if (Z.Id == id)
                    return Z;
            }
            return null;
        }

        public static List<Zombie> GetZombies()
        {
            return Zombies;
        }

       

        public bool IsBloodSucker { get => isBloodSucker; set => isBloodSucker = value; }
        public int Id { get => id; set => id = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public List<Human> Humansinfected { get => humansinfected; set => humansinfected = value; }
    }
}
