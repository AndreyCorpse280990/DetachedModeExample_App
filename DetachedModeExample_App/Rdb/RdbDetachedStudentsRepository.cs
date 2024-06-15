using DetachedModeExample_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App.Rdb
{
    internal class RdbDetachedStudentsRepository : IDetachedStudentsRepository
    {
        // ПОЛЯ
        private SqlDataAdapter dataAdapter; // инструмент для получения/отправки данных в БД
        private DataSet dataSet;            // табличная структура для хранения реляционного состояния в приложении
        //
        private const string TABLE_NAME = "students_t"; // имя используемой таблицы

        // КОНСТРКУТОР
        public RdbDetachedStudentsRepository()
        {
            // 1. создадим объект подключения к БД (не открывая)
            SqlConnection connection = SqlHelper.CreateDbConnection();
            // 2. зададим select-запрос для заполнения данными
            string selectQueryString = @"SELECT id, last_name_f, first_name_f, rate_f, grants_f FROM students_t;"; 
            // 3. создадим объект SqlDataAdapter
            dataAdapter = new SqlDataAdapter(selectQueryString, connection);
            // 4. задать запросы для insert/update/delete
            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter); // в момент такого созадния в dataAdpter будут записаны команды для INSERT/UPDATE/DELETE на основе его SELECT-команды
            // 
            dataSet = null;
        }

        // ФИНАЛИЗАТОР
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // МЕТОДЫ ВЫТЯГИВАНИЯ И ОТПРАВКИ ДАННЫХ В БД
        public void Pull()
        {
            // в момент вызова этого методы dataAdpter подключится к БД, выполнит заданную команду select
            // и заполнит данные в dataSet, после чего отключится от БД
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, TABLE_NAME);
        }

        public void Push()
        {
            CheckDataSetFilled();
            // в этот момент datAdapter подключится к БД, выполнит необходимые команды INSERT/UPDATE/DELETE
            // на основе изменений данных в DataSet, и тем самым синхронизирует БД с текущим состоянием DataSet
            dataAdapter.Update(dataSet, TABLE_NAME);
            // после операции синхронизации БД синхронизировать DataSet с БД
            Pull();
        }

        // МЕТОДЫ РАБОТЫ С ДАННЫМИ В ОТСОЕДИНЕННОМ РЕЖИМЕ
        public Student Get(int id)
        {
            CheckDataSetFilled();
            // если заполнен, то задача такая: реляционные данные в DataSet отобразить в объектные List<Student>
            // 1. получим таблицу из DataSet
            DataTable studentsTable = dataSet.Tables[TABLE_NAME];
            // 2. пройдемся по строкам таблицы и считаем их в объекты
            foreach (DataRow studentRow in studentsTable.Rows)
            {
                if (studentRow.RowState == DataRowState.Deleted)
                {
                    continue; // пропуск удаленных строк (помечены удаленными, но еще не удалены физически)
                }
                int studentId = Convert.ToInt32(studentRow[0]);
                if (studentId == id)
                {
                    return ParseStudentFromRow(studentRow);
                }
            }
            throw new InvalidOperationException($"There are no student with id '{id}'");
        }

        public List<Student> GetAll()
        {
            CheckDataSetFilled();
            // если заполнен, то задача такая: реляционные данные в DataSet отобразить в объектные List<Student>
            // 1. получим таблицу из DataSet
            DataTable studentsTable = dataSet.Tables[TABLE_NAME];
            // 2. пройдемся по строкам таблицы и считаем их в объекты
            List<Student> students = new List<Student>();
            foreach (DataRow studentRow in studentsTable.Rows)
            {
                if (studentRow.RowState == DataRowState.Deleted)
                {
                    continue; // пропуск удаленных строк (помечены удаленными, но еще не удалены физически)
                }
                Student student = ParseStudentFromRow(studentRow);
                students.Add(student);
            }
            return students;
        }

        public void Add(Student student)
        {
            CheckDataSetFilled();
            // 1. получим таблицу студентов из DataSet
            DataTable studentsTable = dataSet.Tables[TABLE_NAME];
            // 2. добавляем новую строку с данными студента
            studentsTable.Rows.Add(0, student.LastName, student.FirstName, student.Rate, student.Grants);
        }

        public void Delete(int id)
        {
            CheckDataSetFilled();
            // идея - найти нужную строку в DataTable и пометить ее удаленной
            // 1. получим таблицу студентов из DataSet
            DataTable studentsTable = dataSet.Tables[TABLE_NAME];
            // 2. найти строку для удаления
            DataRow deletedRow = null;
            foreach (DataRow row in studentsTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    continue; // пропуск удаленных строк (помечены удаленными, но еще не удалены физически)
                }
                int studentId = Convert.ToInt32(row[0]);
                if (studentId == id)
                {
                    deletedRow = row;
                    break;
                }
            }
            if (deletedRow == null)
            {
                throw new InvalidOperationException($"There are no student with id '{id}'");
            }
            // 3. если такая строка таблицы была найден, то надо записать информацию о том, что она была удалена
            deletedRow.Delete();
        }

        public void Update(Student student)
        {
            CheckDataSetFilled();
            DataTable studentsTable = dataSet.Tables[TABLE_NAME];
            DataRow updatedRow = null;
            foreach (DataRow row in studentsTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    continue; // пропуск удаленных строк (помечены удаленными, но еще не удалены физически)
                }
                int studentId = Convert.ToInt32(row[0]);
                if (studentId == student.Id)
                {
                    updatedRow = row;
                    break;
                }
            }
            if (updatedRow == null)
            {
                throw new InvalidOperationException($"There are no student with id '{student.Id}'");
            }
            updatedRow[1] = student.LastName;
            updatedRow[2] = student.FirstName;
            updatedRow[3] = student.Rate;
            updatedRow[4] = student.Grants;
        }

        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        private Student ParseStudentFromRow(DataRow row)
        {
            int id = Convert.ToInt32(row[0]);
            string lastName = Convert.ToString(row[1]);
            string firstName = Convert.ToString(row[2]);
            int rate = Convert.ToInt32(row[3]);
            decimal? grants = null;
            if (!row.IsNull(4))
            {
                grants = Convert.ToDecimal(row[4]);
            }
            return new Student(id, lastName, firstName, rate, grants);
        }

        private void CheckDataSetFilled()
        {
            // проверить был ли уже заполнен DataSet
            if (dataSet == null)
            {
                throw new InvalidOperationException("The data set is not filled");
            }
        }
    }
}
