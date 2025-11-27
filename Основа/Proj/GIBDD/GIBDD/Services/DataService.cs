using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;
using System.Windows;

namespace GIBDD.Services
{
    /// <summary>
    /// Универсальный сервис для работы с данными через Entity Framework
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class DataService<T> where T : class
    {
        private GibddDbContext _context = new GibddDbContext();

        /// <summary>
        /// Получает все записи указанного типа из базы данных
        /// </summary>
        /// <returns>Список всех записей</returns>
        public List<T> GetAll() => _context.Set<T>().ToList();

        /// <summary>
        /// Добавляет новую запись в базу данных
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
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

        /// <summary>
        /// Обновляет существующую запись в базе данных
        /// </summary>
        /// <param name="item">Обновляемая сущность</param>
        public void Update(T item)
        {
            try
            {
                // Создаем новый контекст чтобы избежать конфликта отслеживания
                using (var newContext = new GibddDbContext())
                {
                    newContext.Set<T>().Update(item);
                    newContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет запись из базы данных
        /// </summary>
        /// <param name="item">Удаляемая сущность</param>
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

        /// <summary>
        /// Выполняет универсальный поиск по строке
        /// </summary>
        /// <param name="keyword">Ключевое слово для поиска</param>
        /// <returns>Список найденных записей</returns>
        public List<T> Search(string keyword)
        {
            var items = _context.Set<T>().ToList();
            return items.Where(x => x.ToString().Contains(keyword)).ToList();
        }
    }
}
