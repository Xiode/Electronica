/*

Thom's Electrical Circuit Simulator

*/

using Godot;
using System;
using System.Collections.Generic;

namespace Electric
{
    public abstract partial class Component : Node
    {
        // Not sure if this will be useful, but might be in the future! 
        // All Electrical Component scripts should inherit from this
    }


    /// <summary>
    /// Each Connection holds one of these. It represents the
    /// state of the circuit at the point of that Connection.
    /// When a Component causes a state change, a new one of
    /// these is generated, and it is passed along to each
    /// connected Terminal.
    /// </summary>
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

    /// <summary>
    /// Represents a physical point of connection on a Component.
    /// Has trigger Actions for when something is connected or
    /// disconnected, as well as when there is a change of Power.
    /// </summary>
    public class Terminal
    {
        public Connection Connection = new Connection();
        public Power Power { get {return Connection.Power; }}

        public Terminal()
        {
            Connection.Terminals.Add(this);
        }

        // Todo: Check for faults, etc etc
        public void Connect(Terminal t)
        {
            Power oldPower = t.Power;

            Connection.Terminals.UnionWith(t.Connection.Terminals);
            t.Connection = Connection;

            Connection.Feed(oldPower);

            if (OnConnect != null) 
                OnConnect(Power.GenerateNewID());
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
        public Action<Power> OnPower;
        
    }

    public class Connection
    {
        public HashSet<Terminal> Terminals = new HashSet<Terminal>();
        public Power Power;

        public void Feed(Power p)
        {
            // If the new Power is either newer or of lower resistance, replace it!
            if (p.LatestTimestamp > Power.LatestTimestamp || p.Resistance < Power.Resistance) {
                foreach (Terminal t in Terminals) {
                    if (t.OnPower != null)
                        t.OnPower(p);
                }

                Power = p;
            }

        }
    }
}