using Godot;
using System;

namespace Electric
{
    public partial class Wire : Component
    {
        public Terminal A = new Terminal();
        public Terminal B = new Terminal();
        
        public Wire()
        {
            A.OnPower += (timestamp) => {Transmit(timestamp, A, B);};
            B.OnPower += (timestamp) => {Transmit(timestamp, B, A);};
        }

        void Transmit(int timestamp, Terminal from, Terminal to)
        {
            Console.WriteLine("Transmitting!");
            Power p = new Power(from.Power);
            p.Resistance++;
            p.Volts--;
            to.Connection.RecievePower(from.Connection.Power);
        }

    }
}