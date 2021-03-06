﻿// Reflector.cs
// <copyright file="Reflector.cs"> This code is protected under the MIT License. </copyright>
using System.Collections.Generic;

namespace EnigmaUtilities.Components
{
    /// <summary>
    /// An implementation of the rotor component for the enigma machine.
    /// </summary>
    public class Reflector : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector" /> class.
        /// </summary>
        /// <param name="wiring"> The wirings of the alphabet. </param>
        public Reflector(string wiring)
        {
            // Make sure the wiring is in lower case
            wiring = wiring.ToLower();

            // Create the dictionary of the wiring
            this.EncryptionKeys = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                this.EncryptionKeys.Add(i.ToChar(), wiring[i]);
            }
        }

        /// <summary>
        /// Encrypts a letter with the current reflector settings. 
        /// </summary>
        /// <param name="c"> The character to encrypt. </param>
        /// <returns> The encrypted character. </returns>
        public override char Encrypt(char c)
        {
            return this.EncryptionKeys[c];
        }
    }
}
