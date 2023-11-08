using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;

// Lucas Ghigli
// 08/28/2022
// Zombie Infestation Game
//MainWindow.xaml.cs

namespace ZombieGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StreamWriter Starting; //Script for the starting point.
        StreamWriter Movements; //Script for the Movements for Humans and Zombies.
        StreamWriter Infecting; //Script for the Zombies infestation.
        StreamWriter InfectedBy; //Script for the Humans that are infected by the Zombies.
        StreamWriter FinalResults;
        int rows = 0, cols = 0, NumZombies = 0, NumHumans = 0, numIterations = 0;
        bool open=false;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (open == false)
            {
                Starting = new StreamWriter("JustHappyToBeHere_StartingConfiguration.txt", false);
                Movements = new StreamWriter("JustHappyToBeHere_Movements.txt", false);
                Infecting = new StreamWriter("JustHappyToBeHere_DoingTheInfecting.txt", false);
                InfectedBy = new StreamWriter("JustHappyToBeHere_InfectedBy.txt", false);
                FinalResults = new StreamWriter("JustHappyToBeHere_FinalResults.txt",false);
                open = true;
            }
            
            lvTable.Items.Clear();

            // Creation of the table with the number of rows and columns from the text boxes.
            try
            {
                if (!int.TryParse(txtRows.Text, out rows) || !int.TryParse(txtCols.Text, out cols))
                {
                    MessageBox.Show("Please enter valid numbers for rows and columns.");
                    return;
                }
                else
                {
                    rows = int.Parse(txtRows.Text);
                    cols = int.Parse(txtCols.Text);
                }
                if (!int.TryParse(txtNumZombies.Text, out NumZombies) || !int.TryParse(txtNumHumans.Text, out NumHumans))
                {
                    MessageBox.Show("Please enter valid numbers for number of zombies and humans.");
                    return;
                }
                else
                {
                    NumHumans = int.Parse(txtNumHumans.Text);
                    NumZombies = int.Parse(txtNumZombies.Text);
                }
                if (!int.TryParse(txtNumZombies.Text, out numIterations))
                {
                    MessageBox.Show("Please enter valid numbers for number of iterations.");
                    return;
                }
                else
                    numIterations = int.Parse(NumIterations.Text);
            }
            catch(Exception ex)
            {
                if (open == true)
                {
                    Starting.Close();
                    Movements.Close();
                    InfectedBy.Close();
                    Infecting.Close();
                }
                open = false;
                MessageBox.Show(ex.Message);
            }
                
            // Clear any existing columns from the list view
            gridView.Columns.Clear();
            // Create new columns based on user input
            for (int i = 1; i <= cols; i++)
            {
                GridViewColumn col = new GridViewColumn();
                col.Header = "Column " + i;
                col.DisplayMemberBinding = new Binding("[" + (i - 1) + "]");
                gridView.Columns.Add(col);
            }

            string[,] table = new string[rows, cols];
            Zombie.addzombies(rows,cols,NumZombies,table);
            Human.addHumans(rows,cols,NumHumans,table);
            Human.HumanCount = Human.Humans.Count;

            Grid.startingConfig(Starting);
            
            // Populate the list with sample data
            
            UpdateTable(table, rows, cols,numIterations);
            open = false;
            
        }

        private void lvTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void UpdateTable(string [,] table,int rows,int cols,int numIterations)
        {
            int iter = 0;
            while (Human.HumanCount > 0 && numIterations>0)
            {
                for (int i = 0; i < rows; i++)
                {
                    string[] line = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        line[j] = table[i, j];
                    }
                    lvTable.Items.Add(line);
                }
                await Task.Delay(500);
                List<Human> Prevhumans = Human.GetHumans();
                List<Zombie> Prevzombies = Zombie.GetZombies();
                Grid.moveAllCharacters(table, rows, cols,iter);
                List<Human> Afterhumans = Human.GetHumans();
                List<Zombie> Afterzombies = Zombie.GetZombies();

                Grid.WriteMovements(Movements,Prevhumans,Afterhumans,Prevzombies,Afterzombies,iter);
                lvTable.Items.Clear();
                iter += 1;
                numIterations -= 1;
            }
            for (int i = 0; i < rows; i++)
            {
                string[] line = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                    line[j] = table[i, j];
                }
                lvTable.Items.Add(line);
            }
            Movements.Close();
            Grid.Infecting(Infecting);
            Grid.InfectedBy(InfectedBy);
            Human.ViewAgeOfHumans();
            Grid.WriteFinalResults(FinalResults,NumZombies,NumHumans,iter);
        }
    }
}
