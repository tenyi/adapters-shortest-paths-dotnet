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
using Programmerare.ShortestPaths.Core.Api.Generics;
using System.Collections.Generic;

namespace Programmerare.ShortestPaths.Utils;

public enum SelectionStrategyWhenEdgesAreDuplicated {
    FIRST_IN_LIST_OF_EDGES,
    LAST_IN_LIST_OF_EDGES,
    SMALLEST_WEIGHT,
    LARGEST_WEIGHT
}

public interface SelectionStrategy<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {
    E Reduce(IList<E> edges);
}

public class SelectionStrategyFirst<E, V, W> : SelectionStrategy<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {
    public E Reduce(IList<E> edges) {
        return edges[0];
    }
}

public class SelectionStrategyLast<E, V, W> : SelectionStrategy<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {
    public E Reduce(IList<E> edges) {
        return edges[edges.Count - 1];
    }
}

public class SelectionStrategySmallestWeight<E, V, W> : SelectionStrategy<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {
    public E Reduce(IList<E> edges) {
        double weightMin = double.MaxValue;
        E edgeToReturn = default!;
        foreach (E edge in edges) {
            double w = edge.EdgeWeight.WeightValue;
            if (w < weightMin) {
                weightMin = w;
                edgeToReturn = edge;
            }
        }
        return edgeToReturn;
    }
}

public class SelectionStrategyLargestWeight<E, V, W> : SelectionStrategy<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {
    public E Reduce(IList<E> edges) {
        double weightMax = double.MinValue;
        E edgeToReturn = default(E); // null in the Java version
        foreach (E edge in edges) {
            double w = edge.EdgeWeight.WeightValue;
            if (w > weightMax) {
                weightMax = w;
                edgeToReturn = edge;
            }
        }
        return edgeToReturn;
    }
}

public sealed class EdgeUtility<E, V, W>
    where E : EdgeGenerics<V, W>
    where V : Vertex
    where W : Weight {

    private IDictionary<SelectionStrategyWhenEdgesAreDuplicated, SelectionStrategy<E, V, W>>? tableLookupMapForSelectionStrategies;

    private EdgeUtility() {
    }

    public static EdgeUtility<E, V, W> Create<E, V, W>()
        where E : EdgeGenerics<V, W>
        where V : Vertex
        where W : Weight {
        return new EdgeUtility<E, V, W>();
    }

    private IDictionary<SelectionStrategyWhenEdgesAreDuplicated, SelectionStrategy<E, V, W>> GetTableLookupMapForSelectionStrategies() {
        if (tableLookupMapForSelectionStrategies == null) {
            tableLookupMapForSelectionStrategies = new Dictionary<SelectionStrategyWhenEdgesAreDuplicated, SelectionStrategy<E, V, W>> {
                [SelectionStrategyWhenEdgesAreDuplicated.FIRST_IN_LIST_OF_EDGES] = new SelectionStrategyFirst<E, V, W>(),
                [SelectionStrategyWhenEdgesAreDuplicated.LAST_IN_LIST_OF_EDGES] = new SelectionStrategyLast<E, V, W>(),
                [SelectionStrategyWhenEdgesAreDuplicated.SMALLEST_WEIGHT] = new SelectionStrategySmallestWeight<E, V, W>(),
                [SelectionStrategyWhenEdgesAreDuplicated.LARGEST_WEIGHT] = new SelectionStrategyLargestWeight<E, V, W>(),
            };
        }
        return tableLookupMapForSelectionStrategies;
    }

    public IList<E> GetEdgesWithoutDuplicates(
        IList<E> edges,
        SelectionStrategyWhenEdgesAreDuplicated selectionStrategyWhenEdgesAreDuplicated
    ) {
        IDictionary<string, IList<E>> map = GetMap(edges);
        return GetReduced(edges, map, GetTableLookupMapForSelectionStrategies()[selectionStrategyWhenEdgesAreDuplicated]);
    }

    private IList<E> GetReduced(
        IList<E> edges,
        IDictionary<string, IList<E>> map,
        SelectionStrategy<E, V, W> selectionStrategy
    ) {
        IList<E> edgesToReturn = new List<E>();
        foreach (E edge in edges) {
            string key = edge.EdgeId;
            if (map.TryGetValue(key, out IList<E>? list)) {
                E reduce = selectionStrategy.Reduce(list);
                edgesToReturn.Add(reduce);
                map.Remove(key);
            }
        }
        return edgesToReturn;
    }

    private IDictionary<string, IList<E>> GetMap(IList<E> edges) {
        IDictionary<string, IList<E>> map = new Dictionary<string, IList<E>>();
        foreach (E edge in edges) {
            string key = edge.EdgeId;
            if (!map.TryGetValue(key, out IList<E>? list)) {
                list = new List<E>();
                map.Add(key, list);
            }
            list.Add(edge);
        }
        return map;
    }
}
