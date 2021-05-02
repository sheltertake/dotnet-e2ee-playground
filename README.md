# dotnet-e2ee-playground

This repo contains an implementation of encrypt/decrypt given a KEY.
I'm not an expert of cryptography. 

## Prerequisite

 - dotnet 5 sdk
## How to

 - git clone https://github.com/sheltertake/dotnet-e2ee-playground.git
 - cd .\dotnet-e2ee-playground\
 - dotnet test

```cmd
PS C:\temp\github> git clone https://github.com/sheltertake/dotnet-e2ee-playground.git
Cloning into 'dotnet-e2ee-playground'...

PS C:\temp\github> cd .\dotnet-e2ee-playground\
PS C:\temp\github\dotnet-e2ee-playground> dotnet test
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    27, Skipped:     0, Total:    27, Duration: 37 ms - E2eePlaygroundUnitTests.dll (net5.0)
```

## How to benchmark

 - dotnet run --project .\tests\E2eeLibraryBenchmark\E2eeLibraryBenchmark.csproj -c Release
 - 
```cmd
PS C:\temp\github\dotnet-e2ee-playground> dotnet run --project .\tests\E2eeLibraryBenchmark\E2eeLibraryBenchmark.csproj -c Release
Available Benchmarks:
  #0 Benchmarks1000LineText
  #1 Benchmarks100LineText
  #2 Benchmarks1MillionLineText
  #3 BenchmarkSimple
  #4 BenchmarksLineText


You should select the target benchmark(s). Please, print a number of a benchmark (e.g. `0`) or a contained benchmark caption (e.g. `Benchmarks1000LineText`).
If you want to select few, please separate them with space ` ` (e.g. `1 2 3`).
You can also provide the class name in console arguments by using --filter. (e.g. `--filter *Benchmarks1000LineText*`).

```

## Implementation

Library contains extensions methods for string and for bytes. In dotnet string are immutable. If you have bytes (eg: http requests) it's better use bytes. If you use the bytes version you don't add memory traffic. 
Otherwise yes and it can be huge.

There is a unsafe version of Encrypt/Decrypt in the benchmark project. I don't know why dotnet doesn't provide a ReplaceAt helper to modify a specific byte of a string with another byte. It allows when you walk bytes buffer. Not when you want walk a string.


```csharp
public static string Encrypt(this string message, string key)
{
    return EncryptDecryptToString(message, key);
}
public static byte[] Encrypt(this byte[] message, string key)
{
    return EncryptDecrypt(message, key);
}
public static string Decrypt(this string message, string key)
{
    return EncryptDecryptToString(message, key, false);
}
public static byte[] Decrypt(this byte[] message, string key)
{
    return EncryptDecrypt(message, key, false);
}
```

The function shift each character between 32 and 125 ASCII codes.
The number of shifts are based on key (sum of char codes), optimized avoiding useless full roundtrip 32->125.
The modulo operator give us the number of shifts without the useless complete roundtrip in the 32-125 segment.
Constants are used to optimize the code as best I can. In this particular use case, readability is less important than performance. If the world has to be encrypted/decrypted each cpu clock and each byte in the ram are important. Also for environment.
Tricky bits here are acceptable.  
The row key.Select(x => (int)x).Sum() probably can be optimized as well, but is not an hot-spot of this code. 
The hot-spot is the for-cycle where I iterate each byte to override it with the encrypted/decrypted char code byte.

The body of EncryptDecrypt function below:

```csharp
// wrapper class omitted
private const byte MAX_CHAR_CODE_125 = 125;
private const byte MIN_CHAR_CODE_32 = 32;
private const byte NUM_CHAR_ALLOWED_94 = MAX_CHAR_CODE_125 - MIN_CHAR_CODE_32 + 1;
private const byte LOWER_SEGMENT_BOUND_31 = MIN_CHAR_CODE_32 - 1;
private const byte UPPER_SEGMENT_BOUND_126 = MAX_CHAR_CODE_125 + 1;

private static byte[] EncryptDecrypt(byte[] bytes, string key, bool leftDirection = true)
{
    int sumKeyToAsciBytes = key.Select(x => (int)x).Sum();
    int restToSum = sumKeyToAsciBytes % NUM_CHAR_ALLOWED_94;
    int limit = bytes.Length;

    for (int i = 0; i < limit; i++)
    {
        if (leftDirection)
        {
            bytes[i] = (byte)(bytes[i] + restToSum);
            if (bytes[i] > MAX_CHAR_CODE_125)
                bytes[i] = (byte)(LOWER_SEGMENT_BOUND_31 + (bytes[i] - MAX_CHAR_CODE_125));

        }
        else
        {
            bytes[i] = (byte)(bytes[i] - restToSum);
            if (bytes[i] < MIN_CHAR_CODE_32)
                bytes[i] = (byte)(UPPER_SEGMENT_BOUND_126 - (MIN_CHAR_CODE_32 - bytes[i]));
        }

    }
    return bytes;
}
```

## Considerations

The final version is the best version I think, also if also the unsafe one (in benchmark project EncryptDecryptV5.cs) is valuable.
I'd study better what are implications of unsafe projects. Honestly I don't know what unsafe brings to consumers programs.
If you use bytes in input almost no traffic at all will be produced in the RAM.
If you use string at least 1 allocation will be done. So if you encrypt 1 giga, 1 giga will be added in memory causing memory traffic and cycles of GC. When thruput is important GC cycles should be avoided because in dotnet GC thread reduce the app thread one  

## Versions improvements

Before performance optimizations I saved each attempt in the benchmark project. The baseline of benchmark is the V1, the first implementation.

NB: seconds are relative to my cpu/ram/ssd. In new M1 performance are 2x more or less.

 - EncryptDecryptV1.cs - 6.2s per 1 million lines to encrypt/decrypt
   - the code is not optimized yet. It contains some attempt of optimization.
   - doesn't use chunks and reverse. It seems useless, but my experience is very limited. I can wrong.
   - it use string.Concat to join string[] and char[]. I discovered after that new string{string[]} and new string{char[]} are better.
   - it use List<> that is not performing (4 capacity at beginning, each time list is full is doubled creating a new [] and wasting your memory).
   - encrypt decrypt are two different functions also if they are very similar. I developed them in TDD and optimized only later. 
 - EncryptDecryptV2.cs - 4.497s per million
   - contains first attempt of reduce traffic memory with ReadOnlySpan<char>
   - char[] shiftedChars = new char[message.Length] replace List and its bad performance for this scenario
   - encrypt decrypt merged because the difference is only the direction of string walk
 - EncryptDecryptV3.cs - 1.762s per million
   - new string(shiftedChars) replace string.Concat(shiftedChars). In the 1 million lines benchmark the difference is 1,762.9 vs 4,497.5 ms of v2 and also memory traffic is very lower. string.Concat probably have an implementation similar to List.Add (capacity 4/8/16....)
 - EncryptDecryptV4.cs - 1.275s per million
   - tried string.Create introduced in c#7.2
   - RAM 2x improvement (from 1_221_002_216 to 610_455_184)
 - EncryptDecryptV5.cs - 0.959s per million + 560 bytes of RAM!!! FERRARI!
   - unsafe + fixed (char* ptr = message)
 - EncryptDecryptV6.cs - 1,602s per million (string) + 0.938s (bytes) -> 464B Of RAM from bytes | 915_775_496 for string :(
   - not confident with unsafe solution I tried to create 2 overloads. Usually you should have streams not strings. The string is a final overhead. Bytes should run in memory and should be editable always with the index. 
   - Before use unsafe code I'd study better. In my opinion replace a char with another one is a super-safe code. I don't know how the framework doesn't provide a ReplaceAt(index) extension method.
 - EncryptDecryptV7.cs - 1,779s per million (string) + 0.964s (bytes) -> same memory traffic
   - in string overload I replaced Encoding.ASCII.GetString with string.Create. In million lines benchmark seems changed almost anything. Probably GetString use string.Create or probably is not this the case to use this new functionality.
 - EncryptDecrypt FINAL - 1,557s per million (string) + 0.894s (bytes) -> same memory traffic
   - some opinionated optimization of the body of the cycle (0.2s per million)

 - EncryptDecryptV1ChunksReverse.cs 
   - contains the code with double reverse and chunks. I wrote a test to ensure to have reason in consider these steps instructions traps and not real steps to be implemented. But honestly my experience with crypto is 2 days. So I can be wrong.

## Branches

 - I wrote code in TDD in different branches and every time I reached my goal I squash the branch and all the commits in the main branch. 
 - I didn't delete the original branch to leave all the commits, also initials where I didn't find quickly solution also if the very first implementation where already logically [correct](https://github.com/sheltertake/dotnet-e2ee-playground/commit/55902bb9b64cd66c2aa0323c2072e00b998f7a30). The issue was numCharAllowed = 125 - 32. Stupid error that make fail test and make me think I wrong something I don't know (key.size is not the length?), what does mean shift upwards? rotate chars in chunks? if means rotate how is possible overflow 32-125? debugging code I discovered that reverse twice could be a trap. And then I thought also chunks seems useless. But I can wrong. 


## Benchmarks results


 - Simple string enc/dec 
 
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method | N |       Mean |    Error |   StdDev |  StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------- |-- |-----------:|---------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|------:|-----:|-------:|------:|------:|----------:|
| BEncryptDecryptV1BaselineString | 0 | 1,038.3 ns | 14.63 ns | 12.97 ns | 3.47 ns | 1,022.9 ns | 1,026.8 ns | 1,038.2 ns | 1,047.2 ns | 1,065.1 ns |   963,079.0 |  1.00 |    8 | 0.4425 |     - |     - |     928 B |
|         BEncryptDecryptV2String | 0 |   635.5 ns |  1.52 ns |  1.42 ns | 0.37 ns |   633.0 ns |   634.7 ns |   635.6 ns |   636.6 ns |   638.1 ns | 1,573,448.8 |  0.61 |    7 | 0.2060 |     - |     - |     432 B |
|         BEncryptDecryptV3String | 0 |   458.5 ns |  2.27 ns |  1.90 ns | 0.53 ns |   456.8 ns |   457.4 ns |   457.9 ns |   458.2 ns |   462.6 ns | 2,181,092.5 |  0.44 |    3 | 0.1760 |     - |     - |     368 B |
|         BEncryptDecryptV4String | 0 |   469.1 ns |  1.42 ns |  1.18 ns | 0.33 ns |   467.1 ns |   468.6 ns |   469.4 ns |   469.7 ns |   471.4 ns | 2,131,864.2 |  0.45 |    4 | 0.2217 |     - |     - |     464 B |
|   BEncryptDecryptV5StringUnsafe | 0 |   417.8 ns |  1.44 ns |  1.20 ns | 0.33 ns |   415.8 ns |   417.1 ns |   417.9 ns |   418.7 ns |   419.8 ns | 2,393,365.4 |  0.40 |    2 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV6String | 0 |   508.9 ns |  3.52 ns |  3.12 ns | 0.83 ns |   504.9 ns |   506.5 ns |   508.1 ns |   510.5 ns |   515.4 ns | 1,964,883.9 |  0.49 |    6 | 0.1678 |     - |     - |     352 B |
|               BEncryptDecryptV6 | 0 |   418.4 ns |  1.72 ns |  1.61 ns | 0.42 ns |   416.0 ns |   417.4 ns |   418.2 ns |   419.6 ns |   421.4 ns | 2,389,802.2 |  0.40 |    2 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV7String | 0 |   515.5 ns |  2.71 ns |  2.54 ns | 0.65 ns |   510.5 ns |   513.8 ns |   515.8 ns |   517.4 ns |   519.5 ns | 1,939,816.7 |  0.50 |    6 | 0.1678 |     - |     - |     352 B |
|               BEncryptDecryptV7 | 0 |   405.6 ns |  3.37 ns |  2.82 ns | 0.78 ns |   403.2 ns |   403.6 ns |   404.7 ns |   406.2 ns |   411.3 ns | 2,465,343.1 |  0.39 |    1 | 0.0839 |     - |     - |     176 B |
|           BEncryptDecryptString | 0 |   481.6 ns |  1.76 ns |  1.47 ns | 0.41 ns |   479.5 ns |   480.7 ns |   481.4 ns |   482.6 ns |   484.6 ns | 2,076,417.5 |  0.46 |    5 | 0.1678 |     - |     - |     352 B |
|            BEncryptDecryptBytes | 0 |   408.5 ns |  1.13 ns |  1.00 ns | 0.27 ns |   407.3 ns |   407.6 ns |   408.5 ns |   409.3 ns |   410.0 ns | 2,447,747.8 |  0.39 |    1 | 0.0839 |     - |     - |     176 B |

 - 1 M of lines

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method |       N |       Mean |    Error |   StdDev |  StdErr |        Min |         Q1 |     Median |         Q3 |        Max |   Op/s | Ratio | Rank |     Gen 0 |     Gen 1 |     Gen 2 |    Allocated |
|-------------------------------- |-------- |-----------:|---------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|-----------:|-------:|------:|-----:|----------:|----------:|----------:|-------------:|
| BEncryptDecryptV1BaselineString | 1000000 | 6,225.1 ms | 11.61 ms |  9.70 ms | 2.69 ms | 6,213.5 ms | 6,218.0 ms | 6,221.2 ms | 6,232.7 ms | 6,242.5 ms | 0.1606 |  1.00 |    9 | 2000.0000 | 2000.0000 | 2000.0000 | 4897150160 B |
|         BEncryptDecryptV2String | 1000000 | 4,497.5 ms | 18.45 ms | 14.40 ms | 4.16 ms | 4,478.4 ms | 4,483.0 ms | 4,499.1 ms | 4,510.2 ms | 4,517.0 ms | 0.2223 |  0.72 |    8 | 2000.0000 | 2000.0000 | 2000.0000 | 3360053256 B |
|         BEncryptDecryptV3String | 1000000 | 1,762.9 ms |  4.43 ms |  4.14 ms | 1.07 ms | 1,758.2 ms | 1,759.1 ms | 1,762.6 ms | 1,765.8 ms | 1,772.5 ms | 0.5673 |  0.28 |    7 |         - |         - |         - | 1221002216 B |
|         BEncryptDecryptV4String | 1000000 | 1,275.5 ms |  2.47 ms |  2.19 ms | 0.58 ms | 1,271.5 ms | 1,274.0 ms | 1,275.5 ms | 1,276.9 ms | 1,279.1 ms | 0.7840 |  0.20 |    4 |         - |         - |         - |  610455184 B |
|   BEncryptDecryptV5StringUnsafe | 1000000 |   959.5 ms |  3.19 ms |  2.98 ms | 0.77 ms |   954.0 ms |   957.8 ms |   959.7 ms |   961.0 ms |   964.5 ms | 1.0422 |  0.15 |    3 |         - |         - |         - |        560 B |
|         BEncryptDecryptV6String | 1000000 | 1,602.5 ms | 16.73 ms | 15.65 ms | 4.04 ms | 1,585.4 ms | 1,590.4 ms | 1,593.9 ms | 1,616.6 ms | 1,630.4 ms | 0.6240 |  0.26 |    6 |         - |         - |         - |  915775496 B |
|               BEncryptDecryptV6 | 1000000 |   938.4 ms |  3.47 ms |  3.07 ms | 0.82 ms |   932.9 ms |   936.4 ms |   938.8 ms |   939.7 ms |   944.2 ms | 1.0657 |  0.15 |    2 |         - |         - |         - |        464 B |
|         BEncryptDecryptV7String | 1000000 | 1,779.6 ms | 30.31 ms | 25.31 ms | 7.02 ms | 1,750.7 ms | 1,769.4 ms | 1,773.5 ms | 1,777.6 ms | 1,843.0 ms | 0.5619 |  0.29 |    7 |         - |         - |         - |  915780400 B |
|               BEncryptDecryptV7 | 1000000 |   964.9 ms |  3.21 ms |  2.85 ms | 0.76 ms |   961.3 ms |   963.0 ms |   963.8 ms |   966.6 ms |   970.7 ms | 1.0364 |  0.16 |    3 |         - |         - |         - |        464 B |
|           BEncryptDecryptString | 1000000 | 1,557.0 ms | 17.66 ms | 16.52 ms | 4.27 ms | 1,536.1 ms | 1,543.2 ms | 1,554.7 ms | 1,569.9 ms | 1,591.5 ms | 0.6423 |  0.25 |    5 |         - |         - |         - |  915714704 B |
|            BEncryptDecryptBytes | 1000000 |   894.9 ms |  3.02 ms |  2.52 ms | 0.70 ms |   890.7 ms |   893.7 ms |   894.4 ms |   895.2 ms |   899.7 ms | 1.1174 |  0.14 |    1 |         - |         - |         - |        464 B |


 - 1 K of lines

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method |    N |       Mean |    Error |   StdDev |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |    Op/s | Ratio | Rank |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|-------------------------------- |----- |-----------:|---------:|---------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|--------:|------:|-----:|---------:|---------:|---------:|----------:|
| BEncryptDecryptV1BaselineString | 1000 | 4,523.7 μs | 66.70 μs | 55.70 μs | 15.45 μs | 4,422.6 μs | 4,480.8 μs | 4,525.8 μs | 4,556.5 μs | 4,648.3 μs |   221.1 |  1.00 |    8 | 796.8750 | 664.0625 | 664.0625 | 2708608 B |
|         BEncryptDecryptV2String | 1000 | 3,153.4 μs | 41.28 μs | 32.23 μs |  9.30 μs | 3,113.5 μs | 3,139.7 μs | 3,145.0 μs | 3,151.6 μs | 3,241.6 μs |   317.1 |  0.70 |    7 | 328.1250 | 328.1250 | 328.1250 | 1221192 B |
|         BEncryptDecryptV3String | 1000 | 1,421.5 μs |  5.12 μs |  4.79 μs |  1.24 μs | 1,413.2 μs | 1,418.7 μs | 1,422.2 μs | 1,425.0 μs | 1,428.4 μs |   703.5 |  0.31 |    4 | 333.9844 | 332.0313 | 332.0313 | 1223816 B |
|         BEncryptDecryptV4String | 1000 | 1,394.6 μs |  3.89 μs |  3.45 μs |  0.92 μs | 1,388.6 μs | 1,392.4 μs | 1,394.1 μs | 1,397.6 μs | 1,399.8 μs |   717.1 |  0.31 |    3 | 164.0625 | 164.0625 | 164.0625 |  610460 B |
|   BEncryptDecryptV5StringUnsafe | 1000 |   936.8 μs |  2.27 μs |  2.01 μs |  0.54 μs |   933.6 μs |   934.8 μs |   937.1 μs |   938.4 μs |   939.5 μs | 1,067.5 |  0.21 |    2 |        - |        - |        - |     176 B |
|         BEncryptDecryptV6String | 1000 | 1,487.4 μs |  8.82 μs |  8.25 μs |  2.13 μs | 1,471.3 μs | 1,482.9 μs | 1,488.1 μs | 1,491.9 μs | 1,504.7 μs |   672.3 |  0.33 |    5 | 259.7656 | 257.8125 | 257.8125 |  912502 B |
|               BEncryptDecryptV6 | 1000 |   930.7 μs |  3.55 μs |  3.15 μs |  0.84 μs |   924.2 μs |   928.6 μs |   930.6 μs |   932.4 μs |   937.6 μs | 1,074.5 |  0.21 |    2 |        - |        - |        - |     176 B |
|         BEncryptDecryptV7String | 1000 | 1,587.6 μs |  5.75 μs |  5.38 μs |  1.39 μs | 1,579.2 μs | 1,583.4 μs | 1,588.1 μs | 1,592.0 μs | 1,595.1 μs |   629.9 |  0.35 |    6 | 261.7188 | 259.7656 | 259.7656 |  912890 B |
|               BEncryptDecryptV7 | 1000 |   926.7 μs |  3.30 μs |  3.09 μs |  0.80 μs |   919.0 μs |   925.1 μs |   927.1 μs |   928.8 μs |   931.0 μs | 1,079.1 |  0.20 |    2 |        - |        - |        - |     176 B |
|           BEncryptDecryptString | 1000 | 1,425.0 μs |  6.02 μs |  5.63 μs |  1.45 μs | 1,416.9 μs | 1,420.8 μs | 1,424.2 μs | 1,429.0 μs | 1,434.3 μs |   701.7 |  0.32 |    4 | 261.7188 | 259.7656 | 259.7656 |  917609 B |
|            BEncryptDecryptBytes | 1000 |   887.8 μs |  1.78 μs |  1.39 μs |  0.40 μs |   885.2 μs |   887.1 μs |   887.7 μs |   888.8 μs |   890.0 μs | 1,126.3 |  0.20 |    1 |        - |        - |        - |     176 B |


 - 1 line

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=5.0.202
  [Host]     : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET Core 5.0.5 (CoreCLR 5.0.521.16609, CoreFX 5.0.521.16609), X64 RyuJIT


```
|                          Method | N |       Mean |    Error |   StdDev |   StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------- |-- |-----------:|---------:|---------:|---------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|------:|-----:|-------:|------:|------:|----------:|
| BEncryptDecryptV1BaselineString | 1 | 4,717.3 ns | 77.99 ns | 69.13 ns | 18.48 ns | 4,639.8 ns | 4,665.5 ns | 4,695.0 ns | 4,745.8 ns | 4,852.2 ns |   211,986.4 |  1.00 |    9 | 1.7395 |     - |     - |    3648 B |
|         BEncryptDecryptV2String | 1 | 3,031.5 ns | 24.34 ns | 21.57 ns |  5.77 ns | 3,006.6 ns | 3,016.6 ns | 3,026.4 ns | 3,042.2 ns | 3,077.1 ns |   329,871.6 |  0.64 |    8 | 0.7782 |     - |     - |    1632 B |
|         BEncryptDecryptV3String | 1 | 1,144.7 ns |  8.94 ns |  8.36 ns |  2.16 ns | 1,130.4 ns | 1,138.3 ns | 1,143.3 ns | 1,150.3 ns | 1,159.7 ns |   873,570.7 |  0.24 |    6 | 0.6962 |     - |     - |    1456 B |
|         BEncryptDecryptV4String | 1 |   998.8 ns |  3.59 ns |  3.00 ns |  0.83 ns |   993.4 ns |   996.6 ns |   998.8 ns | 1,000.4 ns | 1,004.5 ns | 1,001,185.3 |  0.21 |    5 | 0.4501 |     - |     - |     944 B |
|   BEncryptDecryptV5StringUnsafe | 1 |   957.6 ns |  5.20 ns |  4.87 ns |  1.26 ns |   948.5 ns |   954.8 ns |   958.8 ns |   960.9 ns |   965.9 ns | 1,044,280.6 |  0.20 |    4 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV6String | 1 | 1,126.5 ns |  5.22 ns |  4.63 ns |  1.24 ns | 1,120.2 ns | 1,123.4 ns | 1,124.4 ns | 1,131.0 ns | 1,134.5 ns |   887,701.5 |  0.24 |    6 | 0.5569 |     - |     - |    1168 B |
|               BEncryptDecryptV6 | 1 |   937.7 ns |  7.24 ns |  6.42 ns |  1.72 ns |   930.6 ns |   932.9 ns |   935.8 ns |   940.8 ns |   949.8 ns | 1,066,466.2 |  0.20 |    3 | 0.0839 |     - |     - |     176 B |
|         BEncryptDecryptV7String | 1 | 1,681.3 ns |  4.98 ns |  4.42 ns |  1.18 ns | 1,674.3 ns | 1,677.9 ns | 1,681.0 ns | 1,684.9 ns | 1,688.6 ns |   594,784.1 |  0.36 |    7 | 0.6657 |     - |     - |    1392 B |
|               BEncryptDecryptV7 | 1 |   895.7 ns |  3.55 ns |  3.15 ns |  0.84 ns |   890.0 ns |   894.3 ns |   895.9 ns |   897.0 ns |   901.0 ns | 1,116,450.2 |  0.19 |    2 | 0.0839 |     - |     - |     176 B |
|           BEncryptDecryptString | 1 | 1,134.1 ns |  8.86 ns |  7.85 ns |  2.10 ns | 1,124.8 ns | 1,129.8 ns | 1,132.3 ns | 1,137.3 ns | 1,149.9 ns |   881,720.4 |  0.24 |    6 | 0.5722 |     - |     - |    1200 B |
|            BEncryptDecryptBytes | 1 |   844.2 ns | 10.41 ns |  8.69 ns |  2.41 ns |   835.4 ns |   838.1 ns |   841.8 ns |   849.2 ns |   866.5 ns | 1,184,483.6 |  0.18 |    1 | 0.0839 |     - |     - |     176 B |
