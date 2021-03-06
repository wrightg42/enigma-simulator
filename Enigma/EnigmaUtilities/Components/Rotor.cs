﻿// Rotor.cs
// <copyright file="Rotor.cs"> This code is protected under the MIT License. </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using EnigmaUtilities.Components.EventArgs;

namespace EnigmaUtilities.Components
{
    /// <summary>
    /// An implementation of the rotor component for the enigma machine.
    /// </summary>
    public class Rotor : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rotor" /> class.
        /// </summary>
        /// <param name="ringSetting"> The ring setting of the rotor. </param>
        /// <param name="rotorSetting"> The position of the rotor. </param>
        /// <param name="wiring"> The wirings of the alphabet. </param>
        /// <param name="turnNotches"> The letters that will turn the next rotor. </param>
        /// <param name="rotorNumber"> The identification number for the rotor. </param>
        public Rotor(int ringSetting, int rotorSetting, string wiring, string turnNotches, int rotorNumber)
        {
            // Set the ring and rotor settings
            this.RingSetting = ringSetting;
            this.RotorSetting = rotorSetting;
            this.OriginalRotorSetting = rotorSetting;

            // Change the notches and wiring to lower case
            wiring = wiring.ToLower();
            turnNotches = turnNotches.ToLower();

            // Create the dictionary of encrypted letters
            this.EncryptionKeys = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                this.EncryptionKeys.Add(i.ToChar(), wiring[i]);
            }

            // Get the list of nothces that turn the next rotor
            this.TunringNotches = new int[turnNotches.Length];
            for (int i = 0; i < turnNotches.Length; i++)
            {
                this.TunringNotches[i] = turnNotches[i].ToInt();
            }

            this.RotorNumber = rotorNumber;
        }

        /// <summary>
        /// Fires when the turning notch it activated.
        /// </summary>
        public event EventHandler<RotorNotchActivatedEventArgs> RotorNotchActivated;

        /// <summary>
        /// Gets or sets the current ring setting for the rotor.
        /// </summary>
        public int RingSetting { get; protected set; }

        /// <summary>
        /// Gets or sets the current position of the rotor.
        /// </summary>
        public int RotorSetting { get; protected set; }

        /// <summary>
        /// Gets the rotor setting from an anticlockwise perspective.
        /// </summary>
        public int AntiClockwiseRotorSetting
        {
            get
            {
                return 26 - this.RotorSetting;
            }
        }

        /// <summary>
        /// Gets or sets the letter that will turn the next rotor.
        /// </summary>
        public int[] TunringNotches { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the encryption is happening right to left or not.
        /// </summary>
        public bool RightToLeft { get; set; }

        /// <summary>
        /// Gets or sets the rotor number.
        /// </summary>
        public int RotorNumber { get; set; }

        /// <summary>
        /// Gets or sets the original rotor setting.
        /// </summary>
        protected int OriginalRotorSetting { get; set; }

        /// <summary>
        /// Encrypts a letter with the current rotor settings.
        /// </summary>
        /// <param name="c"> The character to encrypt. </param>
        /// <returns> The encrypted character. </returns>
        public override char Encrypt(char c)
        {
            // Get entry point into rotor
            int charVal = c.ToInt() - this.RingSetting - this.AntiClockwiseRotorSetting;

            // Encrypt letter
            if (this.RightToLeft)
            {
                charVal = this.EncryptionKeys[charVal.ToChar()].ToInt();
            }
            else
            {
                charVal = this.EncryptionKeys.First(e => e.Value == charVal.ToChar()).Key.ToInt();
            }

            // Return the exit point from rotor
            return (charVal + this.RingSetting + this.AntiClockwiseRotorSetting).ToChar();
        }

        /// <summary>
        /// Rotates the rotor.
        /// </summary>
        public void Rotate()
        {
            // Turn anti-clockwise and keep in alphabet range
            this.RotorSetting++;
            this.RotorSetting = Resources.Mod(this.RotorSetting, 26);

            // Run the notch activated event if needed
            foreach (int turningNotch in this.TunringNotches)
            {
                if (turningNotch == this.RotorSetting)
                {
                    this.OnRotorNotchActivate(this, new RotorNotchActivatedEventArgs(this.RotorNumber));
                }
            }
        }

        /// <summary>
        /// Resets the rotor to its starting position.
        /// </summary>
        public void Reset()
        {
            this.RotorSetting = this.OriginalRotorSetting;
        }

        /// <summary>
        /// Fires the rotor notch activated event.
        /// </summary>
        /// <param name="origin"> The origin on the event. </param>
        /// <param name="e"> The event arguments. </param>
        protected void OnRotorNotchActivate(object origin, RotorNotchActivatedEventArgs e)
        {
            EventHandler<RotorNotchActivatedEventArgs> handler = this.RotorNotchActivated;

            if (handler != null)
            {
                handler(origin, e);
            }
        }
    }
}
