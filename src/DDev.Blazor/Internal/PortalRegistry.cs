namespace DDev.Blazor.Internal;

internal class PortalRegistry
{
    private readonly HashSet<PortalOutlet> _outlets = new();
    private readonly HashSet<PortalSource> _sources = new();
    private int _counter = 0;

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

    public void Add(PortalSource source)
    {
        // If outlet has not yet been created, this will return null.
        source.Outlet = _outlets.FirstOrDefault(o => o.Name == source.Name);
        source.IdInOutlet = _counter++;

        if (source.Outlet?.Sources.Contains(source) is false)
            source.Outlet?.Sources.Add(source);

        source.NotifyStateHasChanged();
    }

    public void Remove(PortalOutlet outlet)
    {
        if (_outlets.Remove(outlet) is false)
            return;

        outlet.Sources.ForEach(p => p.Outlet = null);
        outlet.Sources.Clear();
    }

    public void Remove(PortalSource source)
    {
        if (_sources.Remove(source) is false)
            return;

        source.Outlet?.Sources.Remove(source);
        source.Outlet = null;
        source.IdInOutlet = 0;
    }
}