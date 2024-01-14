using Electric;
using Godot;
using System;

public partial class ElectricDemo : Node
{
    public ElectricDemo()
    {
        
        log("Initializing components...");
        DCSource source = new DCSource();
        Wire wire1 = new Wire();
        Wire wire2 = new Wire();
        Meter meter = new Meter();
        log("Done.");

        log("Connecting Source Positive to wire1 A");
        source.Positive.Connect(wire1.A);
        log("Connecting wire1 B to meter A");
        wire1.B.Connect(meter.A);

        log("Connecting meter B to wire2 B");
        meter.B.Connect(wire2.B);
        log("Connecting wire2 A to source Negative");
        wire2.A.Connect(source.Negative);

        log("Pulsing"); 
        source.Pulse();

        log("Reading"); 
        meter.Read();
    }

    public void log(string str)
    {
        Console.WriteLine(str);
    }
}
