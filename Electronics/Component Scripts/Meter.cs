using Electric;
using Godot;
using System;
using System.ComponentModel;

namespace Electric
{
    public partial class Meter : Component
    {
        public Terminal A = new Terminal();
        public Terminal B = new Terminal();

        public void Read()
        {
            Console.WriteLine($"V: {A.Power.Volts - B.Power.Volts}");
        }
    }
}