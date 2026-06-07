# Adapters for Shortest Paths (.NET)

C# / .NET 10 adapter library providing three implementations of Yen's algorithm for finding the K shortest loopless paths between two nodes in a weighted directed graph.

This is the .NET port of the Java project [adapters-shortest-paths](https://github.com/TomasJohansson/adapters-shortest-paths).

## Quick Start

```csharp
using Programmerare.ShortestPaths.Core.Api;
using Programmerare.ShortestPaths.Adapter.YanQi;
using static Programmerare.ShortestPaths.Core.Impl.VertexImpl;   // CreateVertex
using static Programmerare.ShortestPaths.Core.Impl.WeightImpl;   // CreateWeight
using static Programmerare.ShortestPaths.Core.Impl.EdgeImpl;    // CreateEdge
using static Programmerare.ShortestPaths.Core.Impl.GraphImpl;   // CreateGraph

Vertex a = CreateVertex("A");
Vertex b = CreateVertex("B");
Vertex c = CreateVertex("C");
Vertex d = CreateVertex("D");

IList<Edge> edges = new List<Edge> {
    CreateEdge(a, b, CreateWeight(5)),
    CreateEdge(a, c, CreateWeight(6)),
    CreateEdge(b, c, CreateWeight(7)),
    CreateEdge(b, d, CreateWeight(8)),
    CreateEdge(c, d, CreateWeight(9))
};

Graph graph = CreateGraph(edges);

PathFinderFactory factory = new PathFinderFactoryYanQi();
// Alternative: new PathFinderFactoryBsmock()
// Alternative: new PathFinderFactoryQuikGraph()

PathFinder pathFinder = factory.CreatePathFinder(graph);
IList<Path> paths = pathFinder.FindShortestPaths(a, d, maxNumberOfPaths: 5);

foreach (Path path in paths) {
    Console.WriteLine($"{path.TotalWeightForPath.WeightValue} via {string.Join(" -> ", path.EdgesForPath.Select(e => $"{e.StartVertex.VertexId}->{e.EndVertex.VertexId}"))}");
}
// Output:
// 13 via A -> B -> D
// 15 via A -> C -> D
// 21 via A -> B -> C -> D
```

## Build & Test

```bash
dotnet restore adapters-shortest-paths-dotnet.sln
dotnet build  adapters-shortest-paths-dotnet.sln
dotnet test   adapters-shortest-paths-dotnet.sln
```

All 95 tests pass (0 failed, 0 skipped). No mono or .NET Framework runtime required.

## Architecture

```
Client  ──▶  Core  ◀── Adapter (Bsmock / YanQi / QuikGraph)
              │              │
              │              ▼
              │          Adaptee (third-party algorithm source, translated from Java)
              ▼
          OptimizedPriorityQueue (NuGet)
```

| Project | Description | License |
|---|---|---|
| **Core** | Public API interfaces (`PathFinderFactory`, `PathFinder`, `Graph`, `Edge`, `Vertex`, `Weight`, `Path`) and default implementations | MIT |
| **Adapter.Bsmock** | Adapter using Brandon Smock's K-shortest-paths implementation | Apache 2.0 |
| **Adapter.YanQi** | Adapter using Yan Qi's K-shortest-paths implementation | Apache 2.0 |
| **Adapter.QuikGraph** | Adapter using QuikGraph graph library | MS-PL |
| **Adaptee.Bsmock** | Translated Java source from [bsmock/k-shortest-paths](https://github.com/bsmock/k-shortest-paths) | Apache 2.0 |
| **Adaptee.YanQi** | Translated Java source from [yan-qi/k-shortest-paths-java-version](https://github.com/yan-qi/k-shortest-paths-java-version) | Apache 2.0 |
| **Test** | NUnit test suite — same assertions run against all three adapters and compare results | MIT |
| **Parallel-Test** | Runs all three implementations in parallel for performance comparison | MIT |

All projects target **net10.0** (single target framework).

## Implementations

Three interchangeable implementations of Yen's algorithm are available:

| Factory | Algorithm Source | Notes |
|---|---|---|
| `PathFinderFactoryBsmock` | [bsmock/k-shortest-paths](https://github.com/bsmock/k-shortest-paths) | Uses `OptimizedPriorityQueue` for priority queue operations |
| `PathFinderFactoryYanQi` | [yan-qi/k-shortest-paths-java-version](https://github.com/yan-qi/k-shortest-paths-java-version) | Vertices use integer IDs internally; string IDs are mapped automatically |
| `PathFinderFactoryQuikGraph` | [QuikGraph](https://github.com/KeRNeLith/QuikGraph) | Wrapper around the QuikGraph library |

## .NET Version

This project targets **.NET 10** (single target framework `net10.0`).

Previously it supported .NET Framework 2.0, 4.0, and 4.7.2 with a polyfill layer (`Adaptees.Common`) providing `java.util` collection types for .NET 2.0 compatibility. That polyfill layer has been removed — all collections now use standard BCL types (`List<T>`, `Dictionary<TKey,TValue>`, etc.) and `OptimizedPriorityQueue` 5.1 from NuGet.

## License

- **Core** — MIT
- **Adapter.YanQi / Adaptee.YanQi** — Apache 2.0 (based on Yan Qi's Java implementation)
- **Adapter.Bsmock / Adaptee.Bsmock** — Apache 2.0 (based on Brandon Smock's Java implementation)
- **Adapter.QuikGraph** — MS-PL (based on QuikGraph)

The bundled NuGet package `Programmerare.ShortestPaths` should be considered Apache 2.0 since it contains Apache-licensed code.

See `NOTICE.txt` in the repository root for the full license summary.

## Related Projects

- **Java version**: [adapters-shortest-paths](https://github.com/TomasJohansson/adapters-shortest-paths) — the original Java project with five implementations (Bsmock, YanQi, JGraphT, Mulavito, ReneArgento)
- **YanQi algorithm source**: [k-shortest-paths-java-version](https://github.com/yan-qi/k-shortest-paths-java-version) — Yan Qi's original Java implementation
- **Bsmock algorithm source**: [k-shortest-paths](https://github.com/bsmock/k-shortest-paths) — Brandon Smock's original Java implementation
