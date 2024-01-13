using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Electric
{
    public abstract partial class Component : Node
    {
    }

    public struct Power
    {
        static int LatestGeneratedID=0;
        public static int GenerateNewID()
        {
            LatestGeneratedID++;
            return LatestGeneratedID;
        }
        public Power(int timestamp, int volts, string source, int resistance, int rate, int phaseOffset)
        {
            LatestTimestamp = timestamp;
            Volts = volts;
            Source = source;
            Resistance = resistance;
            Rate = rate;
            PhaseOffset = phaseOffset;
        }
        
        // Clone
        public Power(Power p) {
            this = new Power(p.LatestTimestamp, p.Volts, p.Source, p.Resistance, p.Rate, p.PhaseOffset);
        }

        public int LatestTimestamp = 0;
        public int Volts = 0;
        public string Source = "";
        public int Resistance = 1;
        public int Rate = 0;
        public int PhaseOffset = 0;
    }

    public class Terminal
    {
        public Connection Connection = new Connection();
        public Power Power { get {return Connection.Power; }}

        public Terminal()
        {
            Connection.Terminals.Add(this);
        }

        public void Connect(Terminal t)
        {
            Connection.Terminals.UnionWith(t.Connection.Terminals);
            t.Connection = Connection;
            // Connection = t.Connection; // Let's see if this propagates correctly?
            if (OnConnect != null) 
                OnConnect(Power.GenerateNewID());
            if (OnPower != null)
                OnPower(Power.GenerateNewID());
        }
        
        public void Disconnect(Terminal t)
        {
            Connection.Terminals.Remove(t);
            t.Connection = new Connection();
            t.Connection.Terminals.Add(t);

            if (OnDisconnect != null)
                OnDisconnect(Power.GenerateNewID());
        }

        public void Disconnect()
        {
            Disconnect(this);
        }


        public Action<int> OnConnect;
        public Action<int> OnDisconnect;
        public Action<int> OnPower;
        
    }

    public class Connection
    {
        public HashSet<Terminal> Terminals = new HashSet<Terminal>();
        public Power Power;

        public void RecievePower(Power p)
        {
            // Console.WriteLine("Recieving power...");
            if (p.LatestTimestamp > Power.LatestTimestamp || p.Resistance < Power.Resistance ) {
                // Console.WriteLine($"Passed the test: {p.LatestTimestamp} - {Power.LatestTimestamp} = {p.LatestTimestamp - Power.LatestTimestamp}, {p.Resistance - Power.Resistance} Sending power to {Terminals.Count} Terminals.");
                Power = p;

                // Console.WriteLine($"Power = p, so timestamp diff is now {p.LatestTimestamp - Power.LatestTimestamp}");

                foreach (Terminal t in Terminals) {
                    if (t.OnPower != null)
                        t.OnPower(p.LatestTimestamp);
                }
            }

        }
    }
}