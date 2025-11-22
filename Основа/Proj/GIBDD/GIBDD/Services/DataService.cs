using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;

namespace GIBDD.Services
{public class DataService<T> where T : class
{
    private GibddDbContext _context = new GibddDbContext();
    
    public List<T> GetAll() => _context.Set<T>().ToList();
    
    public void Add(T item)
    {
        _context.Set<T>().Add(item);
        _context.SaveChanges();
    }
    
    public void Update(T item)
    {
        _context.Set<T>().Update(item);
        _context.SaveChanges();
    }
    
    public void Delete(T item)
    {
        _context.Set<T>().Remove(item);
        _context.SaveChanges();
    }
    
    // Универсальный поиск по строке
    public List<T> Search(string keyword)
    {
        var items = _context.Set<T>().ToList();
        return items.Where(x => x.ToString().Contains(keyword)).ToList();
    }
}
}
