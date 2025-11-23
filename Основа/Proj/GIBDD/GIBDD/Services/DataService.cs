using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;
using System.Windows;

namespace GIBDD.Services
{
    public class DataService<T> where T : class
    {
        private GibddDbContext _context = new GibddDbContext();

        public List<T> GetAll() => _context.Set<T>().ToList();

        public void Add(T item)
        {
            try
            {
                _context.Set<T>().Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}");
            }
        }

        public void Update(T item)
        {
            try
            {
                // Отсоединяем все сущности и обновляем
                _context.ChangeTracker.Clear();
                _context.Set<T>().Update(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
            }
        }

        public void Delete(T item)
        {
            try
            {
                _context.Set<T>().Remove(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}. \n Каскадное удаление записей не предусмотрено, проверьте наличие связанных данных");
            }
        }

        // Универсальный поиск по строке
        public List<T> Search(string keyword)
        {
            var items = _context.Set<T>().ToList();
            return items.Where(x => x.ToString().Contains(keyword)).ToList();
        }
    }
}
