using EnergyMetering;

var reading1 = new EnergyMeterReading(DateTimeOffset.Now - TimeSpan.FromSeconds(10), 10m, 10m, 230m, 10m/230);
var reading2 = new EnergyMeterReading(DateTimeOffset.Now, null, 5m, 230m, 5m/230);
var reading3 = new EnergyMeterReading(DateTimeOffset.Now + TimeSpan.FromSeconds(10), null, 100m, 230m, 100m/230);

var state = MeteringState.Create();

MeterComputation.Aggregate(state, reading1);
Log.info($"{state.ConsumedActiveEnergy}");
MeterComputation.Aggregate(state, reading2);
Log.info($"{state.ConsumedActiveEnergy}");
MeterComputation.Aggregate(state, reading3);
Log.info($"{state.ConsumedActiveEnergy}");

