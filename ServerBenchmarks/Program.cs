using BenchmarkDotNet.Running;

BenchmarkSwitcher
    .FromAssembly(System.Reflection.Assembly.GetExecutingAssembly())
    .Run(args);

// // * Legends *
// Mean: Arithmetic mean of all measurements
//   Error     : Half of 99.9% confidence interval
//   StdDev    : Standard deviation of all measurements
//   Gen0      : GC Generation 0 collects per 1000 operations
//   Gen1      : GC Generation 1 collects per 1000 operations
//   Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
//   1 ns      : 1 Nanosecond(0.000000001 sec)