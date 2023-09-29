namespace DDev.Blazor.Internal;

/// <summary>
/// Links <see cref="PortalOutlet"/> with <see cref="PortalSource"/>.
/// </summary>
internal class PortalRegistry
{
    private readonly HashSet<PortalOutlet> _outlets = new();
    private readonly HashSet<PortalSource> _sources = new();
    private int _counter = 0;

    /// <summary>
    /// Add <paramref name="outlet"/> to the register. If <paramref name="outlet"/> has already been added, nothing happens.
    /// </summary>
    /// <exception cref="InvalidOperationException">If another portal with the same name already exists.</exception>
    public void Add(PortalOutlet outlet)
    {
        if (_outlets.Add(outlet) is false)
            return;

        if (_outlets.Any(o => o.Name == outlet.Name && o != outlet))
            throw new InvalidOperationException($"An outlet with name {outlet.Name} already exists.");

        foreach (var source in _sources)
        {
            if (source.Name == outlet.Name)
            {
                outlet.Sources.Add(source);
                source.Outlet = outlet;
                source.IdInOutlet = _counter++;
            }
        }

        outlet.NotifyStateHasChanged();
    }

    /// <summary>
    /// Adds the portal to the register. If the portal has already been added, it's moved to the end of its outlet.
    /// </summary>
    public void Add(PortalSource source)
    {
        // If outlet has not yet been created, this will return null.
        source.Outlet = _outlets.FirstOrDefault(o => o.Name == source.Name);
        source.IdInOutlet = _counter++;

        if (source.Outlet?.Sources.Contains(source) is false)
            source.Outlet?.Sources.Add(source);

        source.NotifyStateHasChanged();
    }

    /// <summary>
    /// Removes an outlet from the register. All sources are removed from the outlet and no new sources will be added to the outlet.
    /// </summary>
    public void Remove(PortalOutlet outlet)
    {
        if (_outlets.Remove(outlet) is false)
            return;

        outlet.Sources.ForEach(p => p.Outlet = null);
        outlet.Sources.Clear();
    }

    /// <summary>
    /// Removes a source from the register and from its outlet.
    /// </summary>
    /// <param name="source"></param>
    public void Remove(PortalSource source)
    {
        if (_sources.Remove(source) is false)
            return;

        source.Outlet?.Sources.Remove(source);
        source.Outlet = null;
        source.IdInOutlet = 0;
    }
}