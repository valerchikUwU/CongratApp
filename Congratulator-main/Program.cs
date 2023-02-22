using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ConsoleApp3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string date_now = Convert.ToString(DateTime.Now.Day) + '.';
                string month_now = Convert.ToString(DateTime.Now.Month);
                date_now += month_now.Length == 2 ? month_now : '0' + month_now;
                string this_month = month_now.Length == 2 ? month_now : '0' + month_now;

                var persons = db.Persons.ToList();

                Console.WriteLine("Сегодня день рождения у следующих людей:");
                foreach (Person u in persons)
                {
                    if (u.Date.Contains(date_now))
                    {
                        Console.WriteLine($"{u.Name} - {u.Age}, {u.Date}");
                    }
                }
                Console.WriteLine();

                Console.WriteLine("В этом месяце день рождения у следующих людей:");
                foreach (Person u in persons)
                {
                    int date_b = Convert.ToInt32(Convert.ToString(u.Date[0])) * 10 + Convert.ToInt32(Convert.ToString(u.Date[1]));
                    if (u.Date.Contains(this_month) && date_b >= DateTime.Now.Day)
                    {
                        Console.WriteLine($"{u.Name} - {u.Age}, {u.Date}");
                    }
                }
            }
            Console.ReadKey();

            Menu menu = new Menu();
            bool fl = true;

            while (fl)
            {
                int n = menu.Print();
                switch (n)
                {
                    case 0:
                        fl = false;
                        Console.WriteLine("Завершение работы");
                        break;

                    case 1:
                        menu.Add();
                        break;

                    case 2:
                        menu.Delete();
                        break;

                    case 3:
                        menu.Update();
                        break;

                    case 4:
                        menu.Get_data();
                        break;

                    case 5:
                        menu.Get_All();
                        break;
                    case 6:
                        menu.Closest();
                        break;

                    default:
                        fl = false;
                        Console.WriteLine("Завершение работы");
                        break;

                }


            }

        }
    }
    public class Person
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Date { get; set; }

    }

    public class Menu
    {
        public int Print()
        {

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1-Добавить информацию о новом ДР");
            Console.WriteLine("2-Удалить информацию о существующем ДР");
            Console.WriteLine("3-Редактировать информацию о существующем ДР");
            Console.WriteLine("4-Получить информацию о ДР по имени/дате/возрасту");
            Console.WriteLine("5-Получить полный список всех ДР");
            Console.WriteLine("6-Получить полный список всех ближайших ДР");
            Console.WriteLine("0-Завершение");
            Console.WriteLine();

            return Convert.ToInt32(Console.ReadLine());
        }

        public void Add()
        {
            Console.WriteLine("Введите имя: ");
            string? name = Console.ReadLine();
            Console.WriteLine("Введите дату рождения в формате дд.мм.гггг : ");
            string? date = Console.ReadLine();
            int year = (Convert.ToInt32(Convert.ToString(date[6]))) * 1000 + (Convert.ToInt32(Convert.ToString(date[7]))) * 100 + (Convert.ToInt32(Convert.ToString(date[8]))) * 10 + Convert.ToInt32(Convert.ToString(date[9]));
            int age = 2022 - year;
            try
            {
                if (age < 0 || age > 150) throw new Exception("Введено недопустимое значение для возраста.");
                else
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        Person pers = new Person { Name = name, Age = age, Date = date };
                        db.Persons.Add(pers);
                        db.SaveChanges();
                        Console.WriteLine("День рождения успешно сохранен");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Delete()
        {
            // Удаление
            Console.WriteLine("Введите имя для удаления записи: ");
            string? del_name = Console.ReadLine();
            Console.WriteLine($"Введите дату рождения {del_name} для удаления записи: ");
            string? del_date = Console.ReadLine();
            using (ApplicationContext db = new ApplicationContext())
            {
                var persons = db.Persons.ToList();
                foreach (Person u in persons)
                {
                    if (u.Name == del_name && u.Date == del_date)
                    {
                        db.Persons.Remove(u);
                        db.SaveChanges();
                    }

                }

            }
            Console.WriteLine("День рождения успешно удалён");
            Console.WriteLine();
        }

        public void Update()
        {
            Console.WriteLine("Нажмите 1, если хотите изменить дату рождения или 2, если хотите изменить имя");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите текущее имя для редактирования записи: ");
            string? red_name = Console.ReadLine();
            Console.WriteLine($"Введите текущую дату рождения {red_name} для редактирования записи: ");
            string? red_date = Console.ReadLine();
            using (ApplicationContext db = new ApplicationContext())
            {
                var persons = db.Persons.ToList();
                foreach (Person u in persons)
                {
                    if (u.Name == red_name && u.Date == red_date)
                    {
                        try
                        {
                            if (a == 1)
                            {
                                Console.WriteLine("Введите новую дату рождения в формате дд.мм.гггг: ");
                                string new_date = Console.ReadLine();
                                int new_year = (Convert.ToInt32(Convert.ToString(new_date[6]))) * 1000 + (Convert.ToInt32(Convert.ToString(new_date[7]))) * 100 + (Convert.ToInt32(Convert.ToString(new_date[8]))) * 10 + Convert.ToInt32(Convert.ToString(new_date[9]));
                                int new_age = 2022 - new_year;
                                u.Date = new_date;
                                db.Persons.Update(u);
                                db.SaveChanges();
                                u.Age = new_age;
                                db.Persons.Update(u);
                                db.SaveChanges();
                                Console.WriteLine("Запись успешно изменена.");
                            }
                            else if (a == 2)
                            {
                                Console.WriteLine("Введите новое имя: ");
                                string new_name = Console.ReadLine();
                                u.Name = new_name;
                                db.Persons.Update(u);
                                db.SaveChanges();
                                Console.WriteLine("Запись успешно изменена.");
                            }
                            else
                            {
                                throw new Exception("Введено недопустимое значение.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                }
            }
            Console.WriteLine();
        }

        public void Get_data()
        {
            Console.WriteLine("Введите 1-для поиска записи по имени, 2-по дате рождения, 3-по возрасту");
            int s = Convert.ToInt32(Console.ReadLine());
            using (ApplicationContext db = new ApplicationContext())
            {
                if (s == 1)
                {
                    Console.WriteLine("Введите имя для поиска записи: ");
                    string? s_name = Console.ReadLine();

                    var persons = db.Persons.ToList();
                    foreach (Person u in persons)
                    {
                        if (u.Name == s_name)
                        {
                            Console.WriteLine($"{u.Name} - {u.Age} лет,  {u.Date}");
                        }

                    }
                }
                else if (s == 2)
                {
                    Console.WriteLine("Введите дату рождения для поиска записи: ");
                    string? s_date = Console.ReadLine();

                    var persons = db.Persons.ToList();
                    foreach (Person u in persons)
                    {
                        if (u.Date == s_date)
                        {
                            Console.WriteLine($"{u.Name} - {u.Age} лет,  {u.Date}");
                        }

                    }
                }
                else if (s == 3)
                {
                    Console.WriteLine("Введите возраст для поиска записи: ");
                    int s_age = Convert.ToInt32(Console.ReadLine());

                    var persons = db.Persons.ToList();
                    foreach (Person u in persons)
                    {
                        if (u.Age == s_age)
                        {
                            Console.WriteLine($"{u.Name} - {u.Age} лет,  {u.Date}");
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Введено недопустимое значение");
                }
            }
            Console.WriteLine();
        }

        public void Get_All()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var persons = db.Persons.ToList();
                Console.WriteLine("Список дней рождений:");
                foreach (Person u in persons)
                {
                    Console.WriteLine($"{u.Name} - {u.Age} лет,  {u.Date}");
                }
            }
            Console.ReadKey();
        }


        public void Closest()
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                DateTime date = DateTime.Now;
                int personDay = 0;
                int currentDay = Int32.Parse(date.ToShortDateString()[..^8]);
                var persons = db.Persons.ToList();
                Console.WriteLine("Список ближайших дней рождений:");
                foreach (Person u in persons)
                {
                    personDay = Int32.Parse(u.Date[..^8]);
                    if (personDay - currentDay <= 3 && personDay - currentDay >= 0)
                    Console.WriteLine($"{u.Name} - {u.Age} лет,  {u.Date}");
                }
            }
            Console.ReadKey();
        }



    }


}


