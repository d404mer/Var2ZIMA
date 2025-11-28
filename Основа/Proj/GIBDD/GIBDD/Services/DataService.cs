using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GIBDD.Services
{
    /// <summary>
    /// Универсальный сервис для работы с данными через Entity Framework
    /// 
    /// НАЗНАЧЕНИЕ: Один сервис для всех CRUD операций с любой сущностью
    /// 
    /// ИСПОЛЬЗОВАНИЕ:
    /// var service = new DataService&lt;Driver&gt;();
    /// service.Add(driver);    // CREATE
    /// var list = service.GetAll(); // READ
    /// service.Update(driver); // UPDATE
    /// service.Delete(driver); // DELETE
    /// 
    /// ПАТТЕРН: Каждый метод создает новый контекст БД для избежания кэширования
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class DataService<T> where T : class
    {

        /// <summary>
        /// Получает все записи указанного типа из базы данных
        /// </summary>
        /// <returns>Список всех записей</returns>
        public List<T> GetAll()
        {
            Debug.WriteLine($"[DataService<{typeof(T).Name}>] GetAll called");
            try
            {
                // Создаем новый контекст для получения свежих данных из БД
                // Используем AsNoTracking() чтобы избежать кэширования
                using (var context = new GibddDbContext())
                {
                    var list = context.Set<T>().AsNoTracking().ToList();
                    Debug.WriteLine($"[DataService<{typeof(T).Name}>] GetAll returned {list.Count} items");
                    return list;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] GetAll exception: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Добавляет новую запись в базу данных
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(T item)
        {
            Debug.WriteLine($"[DataService<{typeof(T).Name}>] Add called. Item: {item}");
            try
            {
                // Создаем новый контекст для добавления
                using (var context = new GibddDbContext())
                {
                    context.Set<T>().Add(item);
                    context.SaveChanges();
                }
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Add succeeded.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Add exception: {ex}");
                MessageBox.Show($"Ошибка при добавлении записи. Подробности смотрите в Debug Output.");
                throw; // Пробрасываем исключение дальше
            }
        }

        /// <summary>
        /// Обновляет существующую запись в базе данных
        /// </summary>
        /// <param name="item">Обновляемая сущность</param>
        public void Update(T item)
        {
            Debug.WriteLine($"[DataService<{typeof(T).Name}>] Update called. Item: {item}");
            try
            {
                // Создаем новый контекст чтобы избежать конфликта отслеживания
                using (var newContext = new GibddDbContext())
                {
                    newContext.Set<T>().Update(item);
                    newContext.SaveChanges();
                }
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Update succeeded.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Update exception: {ex}");
                MessageBox.Show($"Ошибка при обновлении записи. Подробности смотрите в Debug Output.");
            }
        }

        /// <summary>
        /// Удаляет запись из базы данных
        /// </summary>
        /// <param name="item">Удаляемая сущность</param>
        public void Delete(T item)
        {
            Debug.WriteLine($"[DataService<{typeof(T).Name}>] Delete called. Item: {item}");
            try
            {
                // Создаем новый контекст для удаления
                using (var context = new GibddDbContext())
                {
                    // Присоединяем сущность к контексту перед удалением
                    context.Set<T>().Attach(item);
                    context.Set<T>().Remove(item);
                    context.SaveChanges();
                }
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Delete succeeded.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Delete exception: {ex}");
                MessageBox.Show($"Ошибка при удалении записи. Подробности смотрите в Debug Output.\nКаскадное удаление записей не предусмотрено, проверьте наличие связанных данных.");
                throw; // Пробрасываем исключение дальше
            }
        }

        /// <summary>
        /// Выполняет универсальный поиск по строке
        /// </summary>
        /// <param name="keyword">Ключевое слово для поиска</param>
        /// <returns>Список найденных записей</returns>
        public List<T> Search(string keyword)
        {
            Debug.WriteLine($"[DataService<{typeof(T).Name}>] Search called. Keyword: '{keyword}'");
            try
            {
                // Создаем новый контекст для поиска
                using (var context = new GibddDbContext())
                {
                    var items = context.Set<T>().AsNoTracking().ToList();
                    var result = items.Where(x => x?.ToString()?.Contains(keyword) == true).ToList();
                    Debug.WriteLine($"[DataService<{typeof(T).Name}>] Search found {result.Count} items.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DataService<{typeof(T).Name}>] Search exception: {ex}");
                throw;
            }
        }
    }
}
