using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Lucas Ghigli
// 08/28/2022
// Zombie Infestation Game
// ZombieGame.java

namespace ZombieGame
{
    class Grid
    {
        int x;
        int y;
        int z;

        public Grid(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Z { get => z; set => z = value; }

        public static void moveAllCharacters(string[,] array, int rows, int cols,int iter)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (array[i, j] != "" && array[i, j] != null)
                    {
                        MoveCharacter(array, ref i, ref j,ref iter);
                    }
                }
            }
        }
        public static void MoveCharacter(string[,] array, ref int row, ref int col,ref int iter)
        {
            // Get the dimensions of the array
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);

            // Determine the valid directions to move in
            List<string> directions = new List<string>();
            if (row > 0) directions.Add("up");
            if (row < numRows - 1) directions.Add("down");
            if (col > 0) directions.Add("left");
            if (col < numCols - 1) directions.Add("right");

            // If there are no valid directions, return without moving
            if (directions.Count == 0) return;

            // Select a random direction
            Random rand = new Random();
            string direction = directions[rand.Next(directions.Count)];

            // Move the character in the selected direction
            string value = array[row, col];

            int r= row;
            int c=col;
            switch (direction)
            {
                case "up":
                    row--;
                    break;
                case "down":
                    row++;
                    break;
                case "left":
                    col--;
                    break;
                case "right":
                    col++;
                    break;
            }
            if (array[row, col] != "" && array[row,col] != null && array[row, col].Contains("H") && value.Contains("Z"))
            {
                foreach (Human Hum in Human.Humans) 
                {
                    if (Hum.Character == array[row, col])
                    {
                        Hum.InfectedIteration = iter;
                        Hum.IsInfected = true;
                        foreach(Zombie Z1 in Zombie.Zombies)
                            if(Z1.Id== int.Parse(value[1].ToString()))
                                Z1.Humansinfected.Add(Hum);
                    }
                }
                Human.HumanCount -= 1;
                array[row, col] = $"Z{Zombie.Zombies.Count + 1}";
                Random random = new Random();
                int R = random.Next(0, 10);
                bool x=true;
                if (R % 2 == 0)
                    x = false;
                Zombie Z = new Zombie(x, Zombie.Zombies.Count + 1,row,col);
                Zombie.Zombies.Add(Z);
            }
            else if(array[row, col] != "" && array[row, col] != null && array[row, col].Contains("Z") && value.Contains("H"))
            {
                foreach (Human Hum in Human.Humans)
                {
                    if (Hum.Character == value)
                    {
                        Hum.IsInfected = true;
                        Hum.InfectedIteration = iter;
                        foreach (Zombie Z1 in Zombie.Zombies)
                            if (Z1.Id == int.Parse(array[row,col][1].ToString()))
                                Z1.Humansinfected.Add(Hum);
                    }
                }
                Human.HumanCount -= 1;
                array[r, c] = $"Z{Zombie.Zombies.Count + 1}";
                Random random = new Random();
                int R = random.Next(0, 10);
                bool x = true;
                if (R % 2 == 0)
                    x = false;
                Zombie Z = new Zombie(x, Zombie.Zombies.Count + 1, row, col);
                Zombie.Zombies.Add(Z);
            }
            else if(array[row, col] == "" || array[row, col] == null)
            {
                array[r, c] = "";
                array[row, col] = value;
                if (value.Contains("H"))
                {
                    foreach (Human Hum in Human.Humans)
                    {
                        if (Hum.Character == value)
                        {
                            Hum.X = row;
                            Hum.Y = col;
                        }
                    }
                }
                if (value.Contains("Z"))
                {
                    foreach (Zombie Z in Zombie.Zombies)
                    {
                        if (Z.Id == int.Parse(value[1].ToString()))
                        {
                            Z.X = row;
                            Z.Y = col;
                        }
                    }
                }
            }
        }
        public static void startingConfig(StreamWriter Starting)
        {
            for (int i=0;i<Human.Humans.Count;i++)
                Starting.WriteLine($"Human {i+1} starts at {Human.Humans[i].X},{Human.Humans[i].Y}");
            Starting.WriteLine();
            for (int i = 0; i < Zombie.Zombies.Count; i++)
                Starting.WriteLine($"Zombie {i+1} starts at {Zombie.Zombies[i].X},{Zombie.Zombies[i].Y}");
            Starting.Close();
        }
        public static void WriteMovements(StreamWriter Movements,List<Human> H1, List<Human> H2,List<Zombie> Z1, List<Zombie> Z2,int iter)
        {
            Movements.WriteLine($"Iteration {iter}");
            for (int i = 0; i < Human.Humans.Count; i++)
                Movements.WriteLine($"From {H1[i].X},{H1[i].Y} Human {i+1} is now at {H2[i].X},{H2[i].Y}");
            Movements.WriteLine();
            for (int i = 0; i < Zombie.Zombies.Count; i++)
                Movements.WriteLine($"From {Z1[i].X},{Z1[i].Y} Zombie {i + 1} is now at {Z2[i].X},{Z2[i].Y}");
            Movements.WriteLine();
            Movements.WriteLine("_________________________________________________");
            Movements.Flush();
        }
        public static void Infecting(StreamWriter Infecting)
        {
            foreach(Zombie Z in Zombie.Zombies)
            {
                Infecting.Write($"Zombie {Z.Id}: ");
                foreach (Human H in Z.Humansinfected)
                    Infecting.Write($"Infected Human {H.Character[1]} at iteration {H.InfectedIteration},");
                Infecting.WriteLine();
            }
            Infecting.Close();
        }
        public static void InfectedBy(StreamWriter InfectedBy)
        {
            int i = 0;
            foreach (Human H in Human.Humans)
            {
                foreach(Human Infected in Zombie.Zombies[i].Humansinfected)
                    if(Infected.Character==H.Character)
                        InfectedBy.WriteLine($"Human {H.Character[1]} infected by: Zombie {Zombie.Zombies[i].Id} at iteration {H.InfectedIteration}");
            }
            InfectedBy.Close();
        }
        static float GetHumanPercentage(string Type)
        {
            int numStudents=0;
            int survived = 0;
            float percentage;
            if(Type == "")
                percentage = 100 * Human.HumanCount / Human.Humans.Count;
            else
            {
                
                foreach(Human H in Human.Humans)
                {
                    if (H.Occupation == "Student")
                    {
                        numStudents += 1;
                        if (H.IsInfected == false)
                            survived += 1;
                    }
                    
                }
                percentage = 100 * survived / numStudents;
            }
            return percentage;
        }

        static float getZombiePercentage(bool isBloodSucker)
        {
            int count = 0;
            float percentage;
            foreach(Zombie Z in Zombie.Zombies)
            {
                if (isBloodSucker == true)
                {
                    if (Z.IsBloodSucker)
                        count += 1;
                }
                else
                {
                    if (Z.IsBloodSucker == false)
                        count += 1;
                }
            }
            percentage = count * 100 / Zombie.Zombies.Count;
            return percentage;
        }
        public static void WriteFinalResults(StreamWriter FinalResults,int numZombies,int numHumans,int iterations)
        {
            float SurvivedHumans = GetHumanPercentage("");
            float survivedStudents = GetHumanPercentage("Student");
            float BloodSuckerZombies = getZombiePercentage(true);
            float BrainSuckerZombies = getZombiePercentage(false);
            FinalResults.WriteLine($"Number of Zombies at the start = {numZombies}, at the end = {Zombie.Zombies.Count}");
            FinalResults.WriteLine($"Number of Humans at the start = {numHumans}, at the end = {Human.HumanCount}");
            FinalResults.WriteLine($"The percentage of Humans that survived = {SurvivedHumans}");
            FinalResults.WriteLine($"The percentage of Humans that remained students = {survivedStudents}");
            FinalResults.WriteLine($"The percentage of Zombies that are Blood Suckers = {BloodSuckerZombies}");
            FinalResults.WriteLine($"The percentage of Zombies that are Brain Suckers = {BrainSuckerZombies}");
            FinalResults.WriteLine($"The number of iterations = {iterations}");
            FinalResults.Close();
        }
    }
}
