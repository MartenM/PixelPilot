using System.Text;

namespace PixelPilot.Api.Responses.Collections;

/// <summary>
/// Allows for building complex queries against some collections.
/// </summary>
public class QueryArgumentBuilder
{
    private static readonly HashSet<string> SupportedOperators = new() { "=", "!=", ">", ">=", "<", "<=", "~", "!~" };

    private List<(string, string, dynamic)>? _filters;
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
        return AddFilter(key, "=", value);
    }

    /// <summary>
    /// Add a filter for a specific field using a comparison operator.
    /// Multiple filters are combined with AND.
    /// </summary>
    /// <param name="key">Key of the field</param>
    /// <param name="op">Comparison operator: =, !=, &gt;, &gt;=, &lt;, &lt;=, ~ (like) or !~ (not like)</param>
    /// <param name="value">Value of the field</param>
    /// <returns>The builder</returns>
    /// <exception cref="PixelApiException">When the operator is not supported.</exception>
    public QueryArgumentBuilder AddFilter(string key, string op, dynamic value)
    {
        if (!SupportedOperators.Contains(op))
            throw new PixelApiException($"Unsupported filter operator '{op}'.");

        if (_filters == null) _filters = new();
        _filters.Add((key, op, value));
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
            sb.Append($"&filter={Uri.EscapeDataString(ConstructFilter(_filters))}");
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
    /// <param name="filters">Key, operator, value entries</param>
    /// <returns></returns>
    private static string ConstructFilter(List<(string, string, dynamic)> filters)
    {
        StringBuilder filterBuilder = new();
        for (int i = 0; i < filters.Count; i++)
        {
            var (key, op, value) = filters[i];
            if (value.GetType().Equals(typeof(string)))
            {
                filterBuilder.Append($"{key}{op}\"{value}\"");
            }
            else if (value.GetType().Equals(typeof(bool)))
            {
                filterBuilder.Append($"{key}{op}{(value ? "true" : "false")}");
            }
            else
            {
                filterBuilder.Append($"{key}{op}{value}");
            }

            if (filters.Count - 1 == i) continue;

            filterBuilder.Append(" && ");
        }

        return filterBuilder.ToString();
    }
}