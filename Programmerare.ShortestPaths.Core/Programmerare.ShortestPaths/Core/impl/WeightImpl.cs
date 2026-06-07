/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in this "core" project is licensed with MIT.
* Other projects within this Visual Studio solution may be released with other licenses e.g. Apache.
* Please find more information in the files "License.txt" and "NOTICE.txt"
* in the project root directory and/or in the solution root directory.
* It should also be possible to find more license information at this URL:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
*/

using Programmerare.ShortestPaths.Core.Api;
using System;

namespace Programmerare.ShortestPaths.Core.Impl;

/// <summary>
/// Default implementation of <see cref="Weight"/>.
/// Use the static factory method <see cref="CreateWeight"/> to construct instances.
/// </summary>
public sealed class WeightImpl : Weight {

    private readonly double value;

    public static Weight CreateWeight(
        double value
    ) {
        return new WeightImpl(value);
    }

    private WeightImpl(double value) {
        this.value = value;
    }

    public double WeightValue => value;

    public override string ToString() {
        return "WeightImpl [value=" + value + "]";
    }

    public override int GetHashCode() {
        // The original Java code used a 32-bit shift (value ^ (value >>> 32)) which
        // C# does not have. The lower 32 bits of the IEEE 754 bit pattern are used
        // for hashing, which is good enough for the small set of edges in a graph.
        int result = 1;
        long temp = BitConverter.DoubleToInt64Bits(value);
        result = (int)temp;
        return result;
    }

    public override bool Equals(object obj) {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (obj is not WeightImpl other)
            return false;
        if (BitConverter.DoubleToInt64Bits(value) != BitConverter.DoubleToInt64Bits(other.value))
            return false;
        return true;
    }

    public const double SMALL_DELTA_VALUE_FOR_WEIGHT_COMPARISONS = 0.0000000001;

    public string RenderToString() {
        return ToString();
    }

    public Weight Create(double value) {
        return CreateWeight(value);
    }
}
