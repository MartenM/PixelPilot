using System.Text;
using Microsoft.Extensions.Primitives;

namespace PixelPilot.PixelHttpClient.Responses.Collections;

/// <summary>
/// Allows for building complex queries against some collections.
/// </summary>
public class QueryArgumentBuilder
{
    private List<(string, dynamic)>? _filters;
    private string? _sortBy;
    private bool _sortAscending = true;
    
    public QueryArgumentBuilder()
    {
        
    }

    /// <summary>
    /// Add a filter for a specific field
    /// </summary>
    /// <param name="key">Key of the field</param>
    /// <param name="value">Value of the field</param>
    /// <returns>The builder</returns>
    public QueryArgumentBuilder AddFilter(string key, dynamic value)
    {
        if (_filters == null) _filters = new();
        _filters.Add((key, value));
        return this;
    }

    /// <summary>
    /// Sort by a specific field
    /// </summary>
    /// <param name="key">Key of the field</param>
    /// <returns>The builder</returns>
    public QueryArgumentBuilder SortBy(string key)
    {
        _sortBy = key;
        return this;
    }
    
    /// <summary>
    /// Sets sorting method to ascending.
    /// Note this is the default.
    /// </summary>
    /// <returns>The builder</returns>
    public QueryArgumentBuilder SortAscending()
    {
        _sortAscending = true;
        return this;
    }
    
    /// <summary>
    /// Set sorting to descending.
    /// </summary>
    /// <returns>The builder</returns>
    public QueryArgumentBuilder SortDescending()
    {
        _sortAscending = false;
        return this;
    }

    /// <summary>
    /// Convert the builder to a string represenation.
    /// </summary>
    /// <returns></returns>
    public string Build()
    {
        StringBuilder sb = new();
        if (_filters != null)
        {
            sb.Append($"&filter={ConstructFilter(_filters)}");
        }

        if (_sortBy != null)
        {
            sb.Append($"&sort={(_sortAscending ? "" : "-")}{_sortBy}");
        }

        return sb.ToString();
    }
    
    /// <summary>
    /// Constructs the filter based on the input
    /// </summary>
    /// <param name="filters">Key, value pairs of filter entries</param>
    /// <returns></returns>
    /// <exception cref="PixelApiException">When the filter is mis-used.</exception>
    private static string ConstructFilter(List<(string, dynamic)> filters)
    {
        StringBuilder filterBuilder = new();
        for (int i = 0; i < filters.Count; i++)
        {
            var fe = filters[i];
            if (fe.Item2.GetType().Equals(typeof(string)))
            {
                filterBuilder.Append($"{fe.Item1}=\"{fe.Item2}\"");
            }
            else if (fe.Item2.GetType().Equals(typeof(bool)))
            {
                filterBuilder.Append($"{fe.Item1}={(fe.Item2 ? "true" : "false")}");
            }
            else
            {
                filterBuilder.Append($"{fe.Item1}={fe.Item2}");
            }
            
            
            if (filters.Count - 1 == i) continue;
            
            // Append the AND
            throw new PixelApiException("Currently it's not possible to filter on multiple fields!");
        }

        return filterBuilder.ToString();
    }
}