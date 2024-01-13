using Godot;
using System;
using System.ComponentModel;

namespace Electric
{
    public partial class DCSource : Component
    {
        public Terminal Positive = new Terminal();
        public Terminal Negative = new Terminal();

        public void Pulse() {
            int timestamp = Power.GenerateNewID();
            string sourceName = "dc source";
            
            Console.WriteLine($"Pulse stamp: {timestamp}");

            Power power_positive = new Power(timestamp, 100, sourceName, 1, 0, 0);
            Power power_negative = new Power(timestamp, 0,   sourceName, 1, 0, 0);

            Positive.Connection.RecievePower(power_positive);
            Negative.Connection.RecievePower(power_negative);
        }
    }
}
