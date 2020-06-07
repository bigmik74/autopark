using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace autopark
{
    public partial class Form1 : Form
    {
        // объявляем список автобусов в паркинге и список маршрутов
        private BindingList<Autobus> parking;        
        private BindingList<Autobus> route;

        // кн. удаления строки
        private DataGridViewButtonColumn btnDelete;

        /// <summary>
        /// это конструктор формы, 
        /// срабаывает при инстанцировании экземпляра класса
        /// </summary>
        public Form1()
        {
            // инициализация видимых компонентов на форме
            InitializeComponent();
            // инстанцируем списки
            this.parking = new BindingList<Autobus>();
            this.route = new BindingList<Autobus>();            

            // инстанс кнопки удаления строки
            btnDelete = new DataGridViewButtonColumn();
            {
                btnDelete.Name = "btnDelete";
                btnDelete.HeaderText = "";
                btnDelete.Text = "Удалить";
                btnDelete.UseColumnTextForButtonValue = true; //dont forget this line                
            }
            // вызываем метод наполнения паркинга начальными данными
            FillParking();

        }

        /// <summary>
        /// метод заполнения паркинга начальными данными
        /// </summary>
        private void FillParking()
        {
            this.parking.Add(new Autobus { Number = 1, DriverFIO = "Иванов И.И.", RouteNumber = 1 });
            this.parking.Add(new Autobus { Number = 2, DriverFIO = "Петров П.П.", RouteNumber = 2 });
            this.parking.Add(new Autobus { Number = 3, DriverFIO = "Сидоров С.С.", RouteNumber = 3 });
        }

        /// <summary>
        /// это загрузчик формы, срабатывает после инициализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // после того как инициализировались видимые компоненты
            // привязываем гриды к спискам
            //this.parkingSource.DataSource = this.parking;
            dgvPark.DataSource = parking;
            dgvRoute.DataSource = route;
            AddDelButton();
        }

        private void AddDelButton()
        {
            if (dgvPark.Columns.Contains(btnDelete))
            {
                dgvPark.Columns.Remove(dgvPark.Columns[btnDelete.Name]);
            }
            dgvPark.Columns.Add(btnDelete);
        }

        /// <summary>
        /// обработчик кн. добавить в паркинг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.parking.Add(new Autobus());
            dgvPark.Rows[dgvPark.Rows.Count - 1].Selected = true;
        }

        /// <summary>
        /// обработчик в гриде паркинга на нажатие кн. удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPark_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not in our 
            if (e.ColumnIndex == dgvPark.Columns[btnDelete.Name].Index && e.RowIndex >= 0 && e.RowIndex < dgvPark.Rows.Count - 1)
            {
                dgvPark.Rows.RemoveAt(e.RowIndex);
            }
        }

        /// <summary>
        /// отправляем автобус на маршрут
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInRoute_Click(object sender, EventArgs e)
        {
            AutobusMove(dgvPark, dgvRoute);
        }

        /// <summary>
        /// возвращаем автобус с маршрута в паркинг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInPark_Click(object sender, EventArgs e)
        {
            AutobusMove(dgvRoute, dgvPark);
        }

        private void AutobusMove(DataGridView source, DataGridView dest)
        {
            // бежим циклом по выделенным ячейкам
            foreach (DataGridViewCell cell in source.SelectedCells)
            {
                if (cell.RowIndex >= source.Rows.Count - 1)
                    continue;
                // находим строку
                var row = source.Rows[cell.RowIndex];
                // получаем данные автобуса
                Autobus a = (row.DataBoundItem as Autobus);
                // добавляем в назначение
                (dest.DataSource as BindingList<Autobus>).Add(a);
                source.Rows.Remove(row); // убираем из источника
            }
        }

        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbFind.Text))
            {
                dgvPark.ClearSelection();

                var rows = dgvPark.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Number"].Value != null && 
                        r.Cells["Number"].Value.ToString().Equals(tbFind.Text));

                if (rows == null || rows.Count() == 0)
                    return;

                DataGridViewRow row = rows.First();

                if (row.Index > -1)
                    dgvPark.Rows[row.Index].Selected = true;
            }                
        }
    }
}
