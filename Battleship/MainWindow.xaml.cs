using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Battleship.ViewModel;
using Battleship.Model;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HumanPlayer _humanPlayer;
        ComputerPlayer _computerPlayer;

        HumanGridVM _humanGrid;
        ComputerGridVM _computerGrid;

        public static MainWindow _mainWindow;


        public MainWindow()
        {
            _humanPlayer = new HumanPlayer();
            _computerPlayer = new ComputerPlayer();
            _humanGrid = new HumanGridVM(_humanPlayer, _computerPlayer);
            _computerGrid = new ComputerGridVM(_humanPlayer, _computerPlayer);

            InitializeComponent();
            _mainWindow = this;
            humanGrid.DataContext = _humanGrid;
            computerGrid.DataContext = _computerGrid;
            UpdateTb(0,16);
        }

        public void UpdateTb(int shots, int enemyUnits)
        {
            infoTB.Text = "Shots: " + shots + Environment.NewLine + "Enemy Units: " + enemyUnits;
        }

        private void ExecutedNewGame(object sender, ExecutedRoutedEventArgs e)
        {
            _humanPlayer.Reset();
            _computerPlayer.Reset();            
        }

        private void ExecutedAutomatedGame(object sender, ExecutedRoutedEventArgs e)
        {
            ExecutedNewGame(sender, e);
            while (!_computerGrid.Clicked(null, true))
            { }
        }

        private void ExecutedExit(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
