namespace EnergyMetering

open System
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

[<Measure>] type h
[<Measure>] type kW = W

type EnergyMeterReading =
    {
    Timestamp: DateTimeOffset
    ConsumedActiveEnergy: Nullable<decimal<kW h>>
    Power: decimal<kW>
    Voltage: decimal<V>
    Current: decimal<A>
    }

type MeteringState =
    {
    mutable ConsumedActiveEnergy: decimal<kW h>
    mutable LastEnergyMeterReading: EnergyMeterReading option
    }
    static member Create() = { ConsumedActiveEnergy = 0m<kW h>; LastEnergyMeterReading = None }

module MeterComputation =
    let private averageEnergy reading1 reading2 : decimal<kW h> =
        let tDelta = (decimal)(reading1.Timestamp - reading2.Timestamp).TotalHours * 1m<h>
        let averagePower = (reading1.Power + reading2.Power) / 2m
        averagePower * tDelta
        
    let private calculateAverageEnergy state reading =
        match state.LastEnergyMeterReading with
        | Some lastReading -> averageEnergy reading lastReading
        | None -> 0m<kW h>

    let Aggregate (state: MeteringState) (reading: EnergyMeterReading) =
        let consumedActiveEnergy =
            match Option.ofNullable reading.ConsumedActiveEnergy with
            | Some consumedActiveEnergy -> consumedActiveEnergy
            | None -> state.ConsumedActiveEnergy + calculateAverageEnergy state reading

        state.ConsumedActiveEnergy <- consumedActiveEnergy
        state.LastEnergyMeterReading <- Some reading