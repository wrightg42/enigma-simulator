﻿// MachineSetup.xaml.cs
// <copyright file="MachineSetup.xaml.cs"> This code is protected under the MIT License. </copyright>
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnigmaUtilities;
using EnigmaUtilities.Components;

namespace Enigma
{
    /// <summary>
    /// Interaction logic for MachineSetup.xaml
    /// </summary>
    public partial class MachineSetup : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MachineSetup" /> class.
        /// </summary>
        public MachineSetup()
        {
            this.InitializeComponent();

            // Tell the program the default data
            this.FourRotors = false;
            this.RingSettingsActive = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ring settings are currently being viewed.
        /// </summary>
        public bool RingSettingsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are four rotors or not.
        /// </summary>
        public bool FourRotors { get; set; }

        /// <summary>
        /// Begins encryption using the current enigma settings.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Begin_Click(object sender, RoutedEventArgs e)
        {
            // Use settings to create enigma machine instance
            ////EnigmaMachine em = new EnigmaMachine(...);

            // Create encryption window
            ////EnigmaWriter ew = new EnigmaWriter(em);

            // Show encryptor, hide settings
            ////ew.Show();
            this.Close();
        }

        /// <summary>
        /// Changes which rotor setup is being viewed.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void SwitchRotorView_Click(object sender, RoutedEventArgs e)
        {
            // Get which rotor is the first rotro
            int minRotor = this.FourRotors ? 0 : 1;

            if (this.RingSettingsActive)
            {
                // Switch from ring settings to rotor positions
                for (int i = 0; i < 8; i++)
                {
                    if (i < 4 + minRotor)
                    {
                        this.RotorSettingsGrid.ColumnDefinitions[i].Width = new GridLength(0);
                    }
                    else
                    {
                        this.RotorSettingsGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                    }
                }

                // Tell the program it has switched
                this.RingSettingsActive = !this.RingSettingsActive;
                this.SwitchRotorView.Content = "  View Ring Settings  ";
            }
            else
            {
                // Switch from rotor positions to ring settings
                for (int i = 0; i < 8; i++)
                {
                    if (i < 4 && i >= minRotor)
                    {
                        this.RotorSettingsGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                    }
                    else
                    {
                        this.RotorSettingsGrid.ColumnDefinitions[i].Width = new GridLength(0);
                    }
                }

                // Tell the program it has switched
                this.RingSettingsActive = !this.RingSettingsActive;
                this.SwitchRotorView.Content = "  View Rotor Positions  ";
            }
        }

        /// <summary>
        /// Changes values in the program when the four rotor check box has changed.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void FourRotorCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Change the value indicating if there are four rotors
            this.FourRotors = !this.FourRotors;

            // Display correct rotors
            if (this.FourRotors)
            {
                int currentFourthRotorIndex = this.RingSettingsActive ? 0 : 4; // Get the currently displayed fourth rotor
                this.RotorSettingsGrid.ColumnDefinitions[currentFourthRotorIndex].Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                this.RotorSettingsGrid.ColumnDefinitions[0].Width = new GridLength(0);
                this.RotorSettingsGrid.ColumnDefinitions[4].Width = new GridLength(0);
            }
        }
    }
}
