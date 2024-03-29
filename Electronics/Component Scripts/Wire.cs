using Godot;
using System;


/*

TODO:
- Implement length-based resistance and voltage drop

*/
namespace Electric
{
    public partial class Wire : Component
    {
        public Terminal A = new Terminal();
        public Terminal B = new Terminal();
        
        public Wire()
        {
            A.OnPower += (power) => {Transmit(power, A, B);};
            B.OnPower += (power) => {Transmit(power, B, A);};
        }

        void Transmit(Power power, Terminal from, Terminal to)
        {
            Power p = new Power(power);
            p.Resistance++;
            p.Volts--;
            to.Connection.Feed(p);
        }

    }
}