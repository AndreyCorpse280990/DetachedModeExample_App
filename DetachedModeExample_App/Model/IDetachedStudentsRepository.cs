using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App.Model
{
    // IDetachedStudentsRepository - интерфейс операций со студентами в некотором хранилище,
    // работающий в отсоединенном режиме (с необходимость загружать и обновлять данные)
    internal interface IDetachedStudentsRepository : IStudentsRepository
    {
        // Pull - метод вытягивания данных из БД, при котором репозиторий заполняется
        void Pull();

        // Push - метод отправки данных из приложения в БД, БД синхронизируется с репозиторием
        void Push();
    }
}
