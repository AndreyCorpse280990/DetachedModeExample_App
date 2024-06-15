using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App.Model
{
    // IStudentsRepository - интерфейс, описывающий операции со студентами
    // в некотором хранилище (репозитории)
    internal interface IStudentsRepository : IDisposable
    {
        // Add - добавить нового студента
        void Add(Student student);

        // GetAll - получить список всех студентов
        List<Student> GetAll();

        // Get - получение студента по id
        Student Get(int id);

        // Update - обновить данные студента
        void Update(Student student);

        // Delete - удаление студента по id
        void Delete(int id);
    }
}
