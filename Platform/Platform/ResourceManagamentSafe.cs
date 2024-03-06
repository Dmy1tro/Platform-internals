using System;
using System.Collections.Generic;

namespace Platform
{
    public class Item { }

    class SystemIO : IDisposable
    {
        public ICollection<Item> LoadItems() => new List<Item>();

        public void SaveItems(ICollection<Item> items) { }

        public void UpdateItems(ICollection<Item> item) { }

        public void Dispose()
        {
            Console.WriteLine("Disposing....");
        }
    }

    public class ResourceManagamentSafe
    {
        public ICollection<Item> GetItems()
        {
            using var systemIO = new SystemIO();

            var items = systemIO.LoadItems();

            return items;
        }

        public void AddItems(ICollection<Item> items)
        {
            using var systemIO = new SystemIO();

            systemIO.SaveItems(items);
        }

        public void UpdateItems(ICollection<Item> items)
        {
            using var systemIO = new SystemIO();

            systemIO.UpdateItems(items);
        }
    }

    public class ResourceManagamentUnsafe
    {
        private SystemIO _systemIO;

        public void Open()
        {
            _systemIO = new SystemIO();
        }

        public ICollection<Item> GetItems()
        {
            var items = _systemIO.LoadItems();

            return items;
        }

        public void AddItems(ICollection<Item> items)
        {
            _systemIO.SaveItems(items);
        }

        public void UpdateItems(ICollection<Item> items)
        {
            _systemIO.UpdateItems(items);
        }

        public void Close()
        {
            _systemIO.Dispose();
        }
    }
}
