using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

// Lucas Ghigli
// 08/28/2022
// Zombie Infestation Game
// Human.cs

namespace ZombieGame
{
    class Human
    {
        private int age; //Age of the Human.
        private string occupation; //Occupation of the Human.
        private DateTime birthday; //Birthdate of the Human.
        private string character;
        private bool isInfected = false;
        private int x;
        private int y;
        public static List<Human> Humans;
        private static int humanCount;

        private int infectedIteration;

        public Human(int age, string occupation, DateTime birthday,string character,int x,int y)
        {
            this.Age = age;
            this.Occupation = occupation;
            this.Birthday = birthday;
            this.Character = character;
            this.x = x;
            this.y = y;
        }

        public static Tuple<int, DateTime> GenerateRandomAgeAndBirthday(Random random)
        {
            int age = random.Next(1, 30); // Generate random age between 1 and 99
            DateTime birthday = DateTime.Now.AddYears(-age); // Subtract age from current date to get birthday
            return Tuple.Create(age, birthday);
        }
        public static void addHumans(int rows,int cols,int num,string [,] table)
        {
            Humans = new List<Human>();
            Random R = new Random();
            for(int i = 0; i < num; i++)
            {
                int row = R.Next(0,rows);
                int col = R.Next(0,cols);
                while(table[row, col] != "" && table[row, col] != null)
                {
                    row = R.Next(0, rows);
                    col = R.Next(0, cols);
                }
                int rand = R.Next(0,4);
                table[row, col] = $"H{i + 1}";
                Tuple<int, DateTime> ageAndBirthday = GenerateRandomAgeAndBirthday(R);
                int age = ageAndBirthday.Item1;
                DateTime birthday = ageAndBirthday.Item2;
                string character = table[row, col];
                string occupation;
                if (rand == 0 || rand == 1)
                    occupation = "Student";
                else
                    occupation = "Teacher";
                Human H = new Human(age,occupation,birthday,character,row,col);
                Humans.Add(H);
            }
        }

        public static void ViewAgeOfHumans()
        {
            Window myWindow = new Window();
            ScrollViewer SV = new ScrollViewer();
            myWindow.Title = "Object Information";
            myWindow.Width = 400;
            myWindow.Height = 300;
            int i = 1;
            StackPanel stackPanel = new StackPanel();
            foreach (Human H in Humans)
            {
                TextBlock myTextBlock = new TextBlock();

                myTextBlock.Text = $"Human {i} age: " +H.age.ToString()+$", Birthday: {H.birthday} , Occupation: {H.occupation}" ;
                myTextBlock.Margin = new Thickness(0, 10, 0, 10);
                stackPanel.Children.Add(myTextBlock);
                i += 1;
            }
            TextBlock percentage = new TextBlock();
            float percent = humanCount * 100 / Humans.Count;
            percentage.Text = $"Percentage of Humans survived = {percent}";
            percentage.Margin = new Thickness(0, 10, 0, 10);
            stackPanel.Children.Add(percentage);
            stackPanel.CanVerticallyScroll = true;
            SV.Content = stackPanel;
            myWindow.Content = SV;
            myWindow.Show();

        }
        public static List<Human> GetHumans()
        {
            return Humans;
        }
        public int Age { get => age; set => age = value; }
        public string Occupation { get => occupation; set => occupation = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Character { get => character; set => character = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int InfectedIteration { get => infectedIteration; set => infectedIteration = value; }
        public static int HumanCount { get => humanCount; set => humanCount = value; }
        public bool IsInfected { get => isInfected; set => isInfected = value; }
    }
}
