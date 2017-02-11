using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ворд = Microsoft.Office.Interop;


namespace CourseWork
{
    public partial class MainForm : Form
    {
        string login;
        bool isActivatedAdmin = false;
        bool isActivatedNonAdmin = false;

        public MainForm()
        {
            InitializeComponent();
            сменитьУчетнуюЗаписьToolStripMenuItem_Click(new object(), new EventArgs());
        }

        #region Просмотреть
        #region Сотрудники
        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewEmoloyees);
            using (Entities myEntities = new Entities())
            {

                var allPositions =
                      from x in myEntities.EMPLOYEE
                      select new
                      {
                          Должность = x.POSITION,
                      };

                if (comboBox2.DataSource == null)
                {
                    comboBox2.DataSource = allPositions.ToList();
                    comboBox2.DisplayMember = "Должность";
                }

                var allEmployees =
                    from x in myEntities.EMPLOYEE
                    select new
                    {
                        Фамилия = x.LAST_NAME,
                        Имя = x.FIRST_NAME,
                        Отчество = x.PATRONYMIC,
                        Дата_рождения = x.BIRTHDATE,
                        Зарплата = x.SALARY,
                        Должность = x.POSITION,
                        Адрес = x.ADDRESS,
                        Телефон = x.PHONE,
                        Логин = x.LOGIN
                    };


                if (checkBox2.Checked)
                {
                    var allEmployeesAdmins =
                        from x in myEntities.EMPLOYEE
                        where x.ISADMIN == "1"
                        select new
                        {
                            Фамилия = x.LAST_NAME,
                            Имя = x.FIRST_NAME,
                            Отчество = x.PATRONYMIC,
                            Дата_рождения = x.BIRTHDATE,
                            Зарплата = x.SALARY,
                            Должность = x.POSITION,
                            Адрес = x.ADDRESS,
                            Телефон = x.PHONE,
                            Логин = x.LOGIN
                        };

                    allEmployees = allEmployees.Intersect(allEmployeesAdmins);

                }

                if (checkBox3.Checked)
                {
                    var allEmployeesNyPosition =
                           from x in myEntities.EMPLOYEE
                           where x.POSITION == comboBox2.Text
                           select new
                           {
                               Фамилия = x.LAST_NAME,
                               Имя = x.FIRST_NAME,
                               Отчество = x.PATRONYMIC,
                               Дата_рождения = x.BIRTHDATE,
                               Зарплата = x.SALARY,
                               Должность = x.POSITION,
                               Адрес = x.ADDRESS,
                               Телефон = x.PHONE,
                               Логин = x.LOGIN
                           };

                    allEmployees = allEmployees.Intersect(allEmployeesNyPosition);
                }

                viewEmployeesDataGridView.DataSource = allEmployees.ToList();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            сотрудникиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            сотрудникиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            сотрудникиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectedLastName = viewEmployeesDataGridView.SelectedCells[0].Value.ToString();
            string selectedName = viewEmployeesDataGridView.SelectedCells[1].Value.ToString();
            string selectedPatronymic = viewEmployeesDataGridView.SelectedCells[2].Value.ToString();
            string selectedBirthDate = viewEmployeesDataGridView.SelectedCells[3].Value.ToString();
            string selectedSalary = viewEmployeesDataGridView.SelectedCells[4].Value.ToString();
            string selectedPosition = viewEmployeesDataGridView.SelectedCells[5].Value.ToString();
            string selectedAddress = viewEmployeesDataGridView.SelectedCells[6].Value.ToString();
            string selectedPhone = viewEmployeesDataGridView.SelectedCells[7].Value.ToString();
            string selectedLogin = viewEmployeesDataGridView.SelectedCells[8].Value.ToString();

            DialogResult dialogResult = MessageBox.Show("Удаление может привести к непредвиденным последствиям. Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (Entities myEntities = new Entities())
                    {
                        EMPLOYEE selEmployee =
                              (from employee in myEntities.EMPLOYEE
                               where employee.LAST_NAME == selectedLastName &&
                               employee.FIRST_NAME == selectedName &&
                               employee.PATRONYMIC == selectedPatronymic &&
                                   // employee.BIRTHDATE==selectedBirthDate&&
                              employee.POSITION == selectedPosition &&
                              employee.ADDRESS == selectedAddress &&
                              employee.PHONE == selectedPhone &&
                              employee.LOGIN == selectedLogin
                               select employee).SingleOrDefault();

                        myEntities.EMPLOYEE.Remove(selEmployee);
                        myEntities.SaveChanges();
                        сотрудникиToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                }

                catch
                {
                    MessageBox.Show("Произошла непредвиденная ошибка удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        #endregion

        #region Заказы
        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewOrder);
            using (Entities myEntities = new Entities())
            {
                var allOrders =
                    from order in myEntities.ORDERS
                    join tour in myEntities.TOURS on order.ID_TOUR equals tour.ID_TOURS
                    join employee in myEntities.EMPLOYEE on order.ID_EMPLOYEE equals employee.ID_EMPLOYEE
                    join client in myEntities.CLIENT on order.ID_CLIENT equals client.ID_CLIENT
                    join servive in myEntities.SERVICES on tour.ID_SERVIS equals servive.ID_SERVIS
                    join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                    orderby employee.FIRST_NAME
                    select new
                    {
                        Оформивший_сотрудник = employee.FIRST_NAME + " " + employee.LAST_NAME,
                        Купивший_клиент = client.FIRST_NAME + " " + client.LAST_NAME,
                        Страна = country.NAME,
                        Дата_отправления = tour.DISPATCH_DATE,
                        Дата_прибытия = tour.ARRIVAL_DATE,
                        Цена = tour.TOUR_PRICE
                    };

                viewOrderDataGridView.DataSource = allOrders.ToList();
            }

        }
        #endregion

        #region Туры
        private void турыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewTours);
            using (Entities myEntities = new Entities())
            {
                // страны
                if (comboBox1.DataSource == null)
                {
                    var allCountries =
                        from country in myEntities.COUNTRY
                        orderby country.NAME
                        select new
                        {
                            Страна = country.NAME
                        };

                    comboBox1.DataSource = allCountries.ToList();
                    comboBox1.DisplayMember = "Страна";
                }

                var allTours =
                    from tour in myEntities.TOURS
                    join servive in myEntities.SERVICES on tour.ID_SERVIS equals servive.ID_SERVIS
                    join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                    join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                    orderby country.NAME
                    select new
                    {
                        Страна = country.NAME,
                        Дата_отправления = tour.DISPATCH_DATE,
                        Дата_прибытия = tour.ARRIVAL_DATE,
                        Цена = tour.TOUR_PRICE,
                        Транспорт = transport.NAME,
                        Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                        Проживание = servive.RESIDENCE,
                        Условия_проживания = servive.NUTRITION
                    };

                var allToursByCountry =
                   from tour in myEntities.TOURS
                   join servive in myEntities.SERVICES on tour.ID_SERVIS equals servive.ID_SERVIS
                   join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                   join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                   where country.NAME == comboBox1.Text
                   orderby country.NAME
                   select new
                   {
                       Страна = country.NAME,
                       Дата_отправления = tour.DISPATCH_DATE,
                       Дата_прибытия = tour.ARRIVAL_DATE,
                       Цена = tour.TOUR_PRICE,
                       Транспорт = transport.NAME,
                       Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                       Проживание = servive.RESIDENCE,
                       Условия_проживания = servive.NUTRITION
                   };

                if (checkBox1.Checked)
                    allToursDataGridView.DataSource = allToursByCountry.ToList();
                else
                    allToursDataGridView.DataSource = allTours.ToList();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            турыToolStripMenuItem_Click(new object(), new EventArgs());
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            турыToolStripMenuItem_Click(new object(), new EventArgs());
        }
        #endregion

        #region Услуги
        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewServices);
            using (Entities myEntities = new Entities())
            {
                if (comboBox3.DataSource == null || comboBox4.DataSource == null)
                {
                    // проживание
                    BindingSource BSserviceAddResidance = new BindingSource();
                    BSserviceAddResidance.DataSource = new List<string>() { "одноместное", "двухместное", "трехъместное", "четырехъместное" };
                    comboBox4.DataSource = BSserviceAddResidance;

                    // условия проживания
                    BindingSource BSserviceAddNutrition = new BindingSource();
                    BSserviceAddNutrition.DataSource = new List<string>() { "bed & breakfast", "Half Board", "Full Board" };
                    comboBox3.DataSource = BSserviceAddNutrition;
                }


                var allServices =
                    from servive in myEntities.SERVICES
                    join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                    orderby country.NAME
                    select new
                    {
                        Страна = country.NAME,
                        Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                        Проживание = servive.RESIDENCE,
                        Условия_проживания = servive.NUTRITION,
                        Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                    };

                if (checkBox4.Checked)
                {
                    var allServicesByVisa =
                       from servive in myEntities.SERVICES
                       join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                       where servive.VISA_SERVICE == "1"
                       orderby country.NAME
                       select new
                       {
                           Страна = country.NAME,
                           Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                           Проживание = servive.RESIDENCE,
                           Условия_проживания = servive.NUTRITION,
                           Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                       };
                    allServices = allServices.Intersect(allServicesByVisa);
                }

                if (checkBox5.Checked)
                {
                    var allServicesByExcursions =
                       from servive in myEntities.SERVICES
                       join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                       where servive.EXCURSIONS == "1"
                       orderby country.NAME
                       select new
                       {
                           Страна = country.NAME,
                           Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                           Проживание = servive.RESIDENCE,
                           Условия_проживания = servive.NUTRITION,
                           Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                       };
                    allServices = allServices.Intersect(allServicesByExcursions);
                }

                if (checkBox6.Checked)
                {
                    var allServicesByNutrition =
                       from servive in myEntities.SERVICES
                       join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                       where servive.NUTRITION == comboBox3.Text
                       orderby country.NAME
                       select new
                       {
                           Страна = country.NAME,
                           Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                           Проживание = servive.RESIDENCE,
                           Условия_проживания = servive.NUTRITION,
                           Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                       };
                    allServices = allServices.Intersect(allServicesByNutrition);
                }

                if (checkBox7.Checked)
                {
                    var allServicesByResidance =
                       from servive in myEntities.SERVICES
                       join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                       where servive.RESIDENCE == comboBox4.Text
                       orderby country.NAME
                       select new
                       {
                           Страна = country.NAME,
                           Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                           Проживание = servive.RESIDENCE,
                           Условия_проживания = servive.NUTRITION,
                           Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                       };
                    allServices = allServices.Intersect(allServicesByResidance);
                }

                allServicesDataGridView.DataSource = allServices.ToList();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            услугиToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //string selcountry = allServicesDataGridView.SelectedCells[0].Value.ToString();
            //string selvisa = allServicesDataGridView.SelectedCells[0].Value.ToString() == "Присутствует" ? "1" : "0";
            //string selresidance = allServicesDataGridView.SelectedCells[0].Value.ToString();
            //string selnutrition = allServicesDataGridView.SelectedCells[0].Value.ToString();
            //string selexcursions = allServicesDataGridView.SelectedCells[0].Value.ToString() == "Включены" ? "1" : "0";

            //using (Entities myEntities = new Entities())
            //{
            //    decimal serviceID =
            //             (from servive in myEntities.SERVICES
            //             join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
            //             where country.NAME == selcountry &&
            //             servive.VISA_SERVICE == selvisa &&
            //             servive.RESIDENCE == selresidance &&
            //             servive.NUTRITION == selnutrition &&
            //             servive.EXCURSIONS == selexcursions
            //             select servive.ID_SERVIS).SingleOrDefault();


            //    decimal countryID=
            //        (




            //        )


            //    SERVICES servicetoDelete= new SERVICES
            //    {
            //    ID_SERVIS=serviceID,
            //    ID_COUNTRY=


            //    }


            //    myEntities.SERVICES.Remove(servicetoDelete);
            //    myEntities.SaveChanges();
        }

        #endregion

        #region Страны
        private void страныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewCountries);
            using (Entities myEntities = new Entities())
            {
                var allCountries =
                    from country in myEntities.COUNTRY
                    orderby country.NAME
                    select new
                    {
                        Страна = country.NAME,
                    };

                AllCountriesDataGridView.DataSource = allCountries.ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string selectedCountry = AllCountriesDataGridView.SelectedCells[0].Value.ToString();
            DialogResult dialogResult = MessageBox.Show("Удаление может привести к непредвиденным последствиям. Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (Entities myEntities = new Entities())
                    {
                        COUNTRY selCountry =
                            (from country in myEntities.COUNTRY
                             where country.NAME == selectedCountry
                             select country).SingleOrDefault();

                        myEntities.COUNTRY.Remove(selCountry);
                        myEntities.SaveChanges();
                        страныToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                }

                catch
                {
                    MessageBox.Show("Произошла непредвиденная ошибка удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }


        #endregion

        #region Транспорт
        private void транспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewTransport);
            using (Entities myEntities = new Entities())
            {
                var allTransport =
                    from transport in myEntities.TRANSPORT
                    orderby transport.NAME
                    select new
                    {
                        Вид_транспорта = transport.NAME
                    };

                AllTransportDataGridView.DataSource = allTransport.ToList();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selectedTransportName = AllTransportDataGridView.SelectedCells[0].Value.ToString();

            DialogResult dialogResult = MessageBox.Show("Удаление может привести к непредвиденным последствиям. Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (Entities myEntities = new Entities())
                    {
                        TRANSPORT selTransport =
                              (from transport in myEntities.TRANSPORT
                               where transport.NAME == selectedTransportName
                               select transport).SingleOrDefault();

                        myEntities.TRANSPORT.Remove(selTransport);
                        myEntities.SaveChanges();
                        транспортToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                }

                catch
                {
                    MessageBox.Show("Произошла непредвиденная ошибка удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        #endregion

        #region Клиенты
        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewClients);
            using (Entities myEntities = new Entities())
            {
                var allClients =
                    from client in myEntities.CLIENT
                    orderby client.LAST_NAME
                    select new
                    {
                        Фамилия = client.LAST_NAME,
                        Имя = client.FIRST_NAME,
                        Отчество = client.PATRONYMIC,
                        Адрес = client.ADDRESS,
                        Телефон = client.PHONE
                    };

                allClientsDataGridView.DataSource = allClients.ToList();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string selectedLastName = allClientsDataGridView.SelectedCells[0].Value.ToString();
            string selectedName = allClientsDataGridView.SelectedCells[1].Value.ToString();
            string selectedPatronymic = allClientsDataGridView.SelectedCells[2].Value.ToString();
            string selectedAddress = allClientsDataGridView.SelectedCells[3].Value.ToString();
            string selectedPhone = allClientsDataGridView.SelectedCells[4].Value.ToString();

            DialogResult dialogResult = MessageBox.Show("Удаление может привести к непредвиденным последствиям. Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (Entities myEntities = new Entities())
                    {
                        CLIENT selClient =
                              (from client in myEntities.CLIENT
                               where client.LAST_NAME == selectedLastName &&
                               client.FIRST_NAME == selectedName &&
                               client.PATRONYMIC == selectedPatronymic &&
                               client.ADDRESS == selectedAddress &&
                               client.PHONE == selectedPhone
                               select client).SingleOrDefault();

                        myEntities.CLIENT.Remove(selClient);
                        myEntities.SaveChanges();
                        клиентыToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                }

                catch
                {
                    MessageBox.Show("Произошла непредвиденная ошибка удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        #endregion

        #region Сменить учетную запись
        private void сменитьУчетнуюЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ViewChangeAcount);
            if (isActivatedAdmin || isActivatedNonAdmin)
            {
                ExitAcountButton.Enabled = true;
                EnterAcountButton.Enabled = false;
            }

            else
            {
                ExitAcountButton.Enabled = false;
                EnterAcountButton.Enabled = true;
            }

        }
        private void ExitAcountButton_Click(object sender, EventArgs e)
        {
            ExitAcountButton.Enabled = false;
            EnterAcountButton.Enabled = true;
            isActivatedAdmin = false;
            isActivatedNonAdmin = false;

        }
        private void EnterAcountButton_Click(object sender, EventArgs e)
        {
            using (Entities myEntities = new Entities())
            {
                decimal searchedLoginPasswordA =
                    (from employee in myEntities.EMPLOYEE
                     where employee.LOGIN == LoginTextBox.Text && employee.PASSWORD == PasswordTextBox.Text && employee.ISADMIN == "1"
                     select employee.ID_EMPLOYEE).SingleOrDefault();

                decimal searchedLoginPasswordNA =
                    (from employee in myEntities.EMPLOYEE
                     where employee.LOGIN == LoginTextBox.Text && employee.PASSWORD == PasswordTextBox.Text && employee.ISADMIN == "0"
                     select employee.ID_EMPLOYEE).SingleOrDefault();

                if (searchedLoginPasswordA == 0 && searchedLoginPasswordNA == 0)
                {
                    MessageBox.Show("Неверная комбинация логина и пароля!", "Вход не выполнен", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (searchedLoginPasswordA != 0)
                {
                    isActivatedNonAdmin = false;
                    isActivatedAdmin = true;
                    тураToolStripMenuItem_Click(new object(), new EventArgs());
                    MessageBox.Show("Пароль и логин введены верно", "Вход выполнен как администратор", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    login = LoginTextBox.Text;
                    добавитьToolStripMenuItem.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                }

                else
                {
                    isActivatedAdmin = false;
                    isActivatedNonAdmin = true;
                    тураToolStripMenuItem_Click(new object(), new EventArgs());
                    MessageBox.Show("Пароль и логин введены верно", "Вход выполнен как менеджер", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    login = LoginTextBox.Text;
                    добавитьToolStripMenuItem.Enabled = false;

                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                }

            };

        }

        #endregion
        #endregion

        #region Добавить

        # region Добавить - Сотрудника
        private void сотрудникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(AddEmployee);
        }
        private void empAddbutton_Click(object sender, EventArgs e)
        {

            #region проверка ввода

            try
            {
                bool allInformation = true;

                foreach (var symbol in empAddLastNameTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                foreach (var symbol in empAddFirstNameTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                foreach (var symbol in empAddPatronymicTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                if (empAddBirthdayDateTimePicker.Value.Year >= DateTime.Now.Year - 10) allInformation = false;

                foreach (var symbol in empAddSalaryTextBox.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                if (Convert.ToDecimal(empAddSalaryTextBox.Text) <= 0) allInformation = false;

                foreach (var symbol in empAddPhoneTextBox.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                if (IsEmpty(empAddLastNameTextBox, empAddFirstNameTextBox, empAddPatronymicTextBox,
                    empAddSalaryTextBox, empAddPositionTextBox, empAddAddressTextBox,
                    empAddPhoneTextBox, EmpAddLoginTextBox, empAddPasswordTextBox)) allInformation = false;

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            try
            {
                using (Entities myEntities = new Entities())
                {
                    EMPLOYEE employee = new EMPLOYEE
                    {
                        FIRST_NAME = empAddFirstNameTextBox.Text,
                        LAST_NAME = empAddLastNameTextBox.Text,
                        PATRONYMIC = empAddPatronymicTextBox.Text,
                        BIRTHDATE = Convert.ToDateTime(empAddBirthdayDateTimePicker.Value),
                        SALARY = Convert.ToDecimal(empAddSalaryTextBox.Text),
                        POSITION = empAddPositionTextBox.Text,
                        ADDRESS = empAddAddressTextBox.Text,
                        PHONE = empAddPhoneTextBox.Text,
                        LOGIN = EmpAddLoginTextBox.Text,
                        PASSWORD = empAddPasswordTextBox.Text,
                        ISADMIN = EmpAddIsAdminCheckBox.Checked == true ? "1" : "0"
                    };

                    myEntities.EMPLOYEE.Add(employee);
                    myEntities.SaveChanges();
                }
                MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        #endregion

        #region Добавить - Тур
        private void турToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(AddTour);

            using (Entities myEntities = new Entities())
            {
                var allTransport =
                    from transport in myEntities.TRANSPORT
                    orderby transport.NAME
                    select new
                    {
                        Вид_транспорта = transport.NAME
                    };

                tourAddTransportComboBox1.DataSource = allTransport.ToList();
                tourAddTransportComboBox1.DisplayMember = "Вид_транспорта";


                var allServices =
                   from servive in myEntities.SERVICES
                   join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                   orderby country.NAME
                   select new
                   {
                       Страна = country.NAME,
                       Визовое_обслуживание = servive.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                       Проживание = servive.RESIDENCE,
                       Условия_проживания = servive.NUTRITION,
                       Экскурсии = servive.EXCURSIONS == "1" ? "Включены" : "Не включены",
                   };

                tourAddServiceDataGridView.DataSource = allServices.ToList();

            }

        }
        private void tourAddAddButton_Click(object sender, EventArgs e)
        {

            #region проверка ввода

            try
            {
                bool allInformation = true;

                //   if (tourAddArivalDatedateTimePicker.Value >= DateTime.Now) allInformation = false;
                //  if (tourAddDispatchDatedateTimePicker.Value >= DateTime.Now) allInformation = false;

                foreach (var symbol in tourAddTextBoxStayLength.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                foreach (var symbol in tourAddAmountPeopletextBox.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                foreach (var symbol in tourAddTourPriceTextBox.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                if (IsEmpty(tourAddTextBoxStayLength, tourAddAmountPeopletextBox, tourAddTourPriceTextBox
                    )) allInformation = false;

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            try
            {
                using (Entities myEntities = new Entities())
                {

                    string selectedCountry = tourAddServiceDataGridView.SelectedCells[0].Value.ToString();
                    string selectedVisaService = tourAddServiceDataGridView.SelectedCells[1].Value.ToString();
                    string selectedResidence = tourAddServiceDataGridView.SelectedCells[2].Value.ToString() == "Присутствует" ? "1" : "0";
                    string selectedNutrition = tourAddServiceDataGridView.SelectedCells[3].Value.ToString();
                    string selectedExcursions = tourAddServiceDataGridView.SelectedCells[4].Value.ToString() == "Включены" ? "1" : "0";

                    decimal serviceID =
                        (from country in myEntities.COUNTRY
                         join service in myEntities.SERVICES on country.ID_COUNTRY equals service.ID_COUNTRY
                         where country.NAME == selectedCountry
                         select service.ID_SERVIS).SingleOrDefault();

                    decimal transportID =
                        (from transport in myEntities.TRANSPORT
                         where transport.NAME == tourAddTransportComboBox1.Text
                         select transport.ID_TRANSPORT).SingleOrDefault();

                    TOURS tour = new TOURS
                    {
                        ARRIVAL_DATE = Convert.ToDateTime(tourAddArivalDatedateTimePicker.Value),
                        DISPATCH_DATE = Convert.ToDateTime(tourAddDispatchDatedateTimePicker.Value),
                        ID_SERVIS = serviceID,
                        ID_TRANSPORT = transportID,
                        PERSON_NUMBER = Convert.ToDecimal(tourAddAmountPeopletextBox.Text),
                        STAY_LENGTH = Convert.ToDecimal(tourAddAmountPeopletextBox.Text),
                        TOUR_PRICE = Convert.ToDecimal(tourAddTourPriceTextBox.Text)
                    };

                    myEntities.TOURS.Add(tour);
                    myEntities.SaveChanges();

                }
                MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            СleanControlls(tourAddTextBoxStayLength, tourAddAmountPeopletextBox, tourAddTourPriceTextBox);
        }
        private void tourAddAddTransportButton_Click(object sender, EventArgs e)
        {
            транспортToolStripMenuItem_Click_1(new object(), new EventArgs());
        }
        private void tourAddServiceAddButton_Click(object sender, EventArgs e)
        {
            услугуToolStripMenuItem_Click(new object(), new EventArgs());
        }
        #endregion

        #region Добавить - Услугу

        private void услугуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(AddService);
            using (Entities myEntities = new Entities())
            {
                var allCountries =
                    from country in myEntities.COUNTRY
                    orderby country.NAME
                    select new
                    {
                        Страна = country.NAME
                    };

                serviceAddCountryComboBox.DataSource = allCountries.ToList();
                serviceAddCountryComboBox.DisplayMember = "Страна";

                // визовое обслуживание
                BindingSource BSserviceAddCountry = new BindingSource();
                BSserviceAddCountry.DataSource = new List<string>() { "Присутствует", "Отсутствует" };
                serviceAddVisaServiceComboBox.DataSource = BSserviceAddCountry;

                // проживание
                BindingSource BSserviceAddResidance = new BindingSource();
                BSserviceAddResidance.DataSource = new List<string>() { "одноместное", "двухместное", "трехъместное", "четырехъместное" };
                serviceAddResidanceComboBox.DataSource = BSserviceAddResidance;

                // условия проживания
                BindingSource BSserviceAddNutrition = new BindingSource();
                BSserviceAddNutrition.DataSource = new List<string>() { "bed & breakfast", "Half Board", "Full Board" };
                serviceAddNutritionComboBox.DataSource = BSserviceAddNutrition;

                // экскурсии
                BindingSource BSserviceAddExursions = new BindingSource();
                BSserviceAddExursions.DataSource = new List<string>() { "Включены", "Не включены" };
                serviceAddExursionsComboBox.DataSource = BSserviceAddExursions;
            }

        }
        private void serviceAddAddButtonButton_Click(object sender, EventArgs e)
        {
            странуToolStripMenuItem_Click(new object(), new EventArgs());
        }
        private void serviceAddAddButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Entities myEntities = new Entities())
                {
                    decimal selectedCountyID =
                            (from country in myEntities.COUNTRY
                             where country.NAME == serviceAddCountryComboBox.Text
                             select country.ID_COUNTRY).SingleOrDefault();


                    SERVICES service = new SERVICES
                    {
                        EXCURSIONS = serviceAddExursionsComboBox.Text == "Включены" ? "1" : "2",
                        VISA_SERVICE = serviceAddVisaServiceComboBox.Text == "Присутствует" ? "1" : "0",
                        ID_COUNTRY = selectedCountyID,
                        NUTRITION = serviceAddNutritionComboBox.Text,
                        RESIDENCE = serviceAddResidanceComboBox.Text
                    };

                    myEntities.SERVICES.Add(service);
                    myEntities.SaveChanges();
                }
                MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }

        #endregion

        #region Добавить - Транспорт
        private void транспортToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PanelController(AddTransport);
        }
        private void AddTransportAddButton_Click(object sender, EventArgs e)
        {
            #region проверка ввода

            try
            {
                bool allInformation = true;


                foreach (var symbol in AddTransportNameTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                if (IsEmpty(AddTransportNameTextBox)) allInformation = false;

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion


            try
            {
                using (Entities myEntities = new Entities())
                {
                    TRANSPORT transport = new TRANSPORT
                    {
                        NAME = AddTransportNameTextBox.Text
                    };
                    myEntities.TRANSPORT.Add(transport);
                    myEntities.SaveChanges();
                }
                MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            СleanControlls(AddTransportNameTextBox);
        }
        #endregion

        #region Добавить - Клиента
        private void клиентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(AddClient);
        }
        private void ClientAddAddButton_Click(object sender, EventArgs e)
        {
            #region проверка ввода

            try
            {
                bool allInformation = true;

                foreach (var symbol in ClientAddLastNameTextBox1.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                foreach (var symbol in ClientAddFirstNameTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                foreach (var symbol in ClientAddPatronymicTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                foreach (var symbol in ClientAddPhoneTextBox.Text)
                    if (!char.IsDigit(symbol)) allInformation = false;

                if (IsEmpty(ClientAddLastNameTextBox1, ClientAddFirstNameTextBox,
                    ClientAddPatronymicTextBox, ClientAddAddressTextBox, ClientAddPhoneTextBox
                    )) allInformation = false;

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion



            try
            {
                using (Entities myEntities = new Entities())
                {
                    CLIENT client = new CLIENT
                    {
                        ADDRESS = ClientAddAddressTextBox.Text,
                        FIRST_NAME = ClientAddFirstNameTextBox.Text,
                        LAST_NAME = ClientAddLastNameTextBox1.Text,
                        PATRONYMIC = ClientAddPatronymicTextBox.Text,
                        PHONE = ClientAddPhoneTextBox.Text
                    };

                    myEntities.CLIENT.Add(client);
                    myEntities.SaveChanges();

                }
                MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

            СleanControlls(ClientAddLastNameTextBox1, ClientAddFirstNameTextBox, ClientAddPatronymicTextBox, ClientAddAddressTextBox, ClientAddPhoneTextBox);
        }

        #endregion

        #region Добавить - Страну
        private void странуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(AddCountry);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region проверка ввода

            try
            {
                bool allInformation = true;

                foreach (var symbol in CountryAddNameTextBox.Text)
                    if (!char.IsLetter(symbol)) allInformation = false;

                if (IsEmpty(CountryAddNameTextBox)) allInformation = false;

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            try
            {
                using (Entities myEntities = new Entities())
                {
                    COUNTRY country = new COUNTRY
                    {
                        NAME = CountryAddNameTextBox.Text,
                    };

                    myEntities.COUNTRY.Add(country);
                    myEntities.SaveChanges();
                    MessageBox.Show("Добавление выполнено", "Запись успешно добавлена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка добавления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            СleanControlls(CountryAddNameTextBox);
        }

        #endregion

        #endregion

        #region Поиск - Тура/Сделать заказ
        private void тураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(SearchTour);




            using (Entities myEntities = new Entities())
            {
                // страны
                var allCountries =
                    from country in myEntities.COUNTRY
                    orderby country.NAME
                    select new
                    {
                        Страна = country.NAME
                    };

                TourSearchCountryComboBox.DataSource = allCountries.ToList();
                TourSearchCountryComboBox.DisplayMember = "Страна";

                // транспорт
                var allTransport =
                    from transport in myEntities.TRANSPORT
                    orderby transport.NAME
                    select new
                    {
                        Вид_транспорта = transport.NAME
                    };

                TourSearchTransportComboBox.DataSource = allTransport.ToList();
                TourSearchTransportComboBox.DisplayMember = "Вид_транспорта";

                // страна
                BindingSource BSserviceAddCountry = new BindingSource();
                BSserviceAddCountry.DataSource = new List<string>() { "Присутствует", "Отсутствует" };
                TourSearchVisaServiceComboBox.DataSource = BSserviceAddCountry;

                // проживание
                BindingSource BSserviceAddResidance = new BindingSource();
                BSserviceAddResidance.DataSource = new List<string>() { "одноместное", "двухместное", "трехъместное", "четырехъместное" };
                TourSearchResidanceСomboBox.DataSource = BSserviceAddResidance;

                // условия проживания
                BindingSource BSserviceAddNutrition = new BindingSource();
                BSserviceAddNutrition.DataSource = new List<string>() { "bed & breakfast", "Half Board", "Full Board" };
                TourSearchNutritionComboBox.DataSource = BSserviceAddNutrition;

                // экскурсии
                BindingSource BSserviceAddExursions = new BindingSource();
                BSserviceAddExursions.DataSource = new List<string>() { "Включены", "Не включены" };
                TourSearchExcursionsTextBox.DataSource = BSserviceAddExursions;

                var allClients =
                    from client in myEntities.CLIENT
                    orderby client.LAST_NAME
                    select client.LAST_NAME + " " + client.FIRST_NAME + " " + client.PATRONYMIC;
                TourSearchClientComboBox.DataSource = allClients.ToList();
                //TourSearchClientComboBox.DisplayMember = "Страна";
            }


            TourSearchSearchButton_Click(new object(), new EventArgs());

        }

        private void TourSearchSearchButton_Click(object sender, EventArgs e)
        {
            #region проверка ввода

            try
            {
                bool allInformation = true;


                if (TourAddPriceCheckBox.Checked)
                {
                    foreach (var symbol in TourSearchPriceTextBox.Text)
                        if (!char.IsDigit(symbol)) allInformation = false;

                    if (IsEmpty(TourSearchPriceTextBox)) allInformation = false;

                    if (Convert.ToDecimal(TourSearchPriceTextBox.Text) <= 0) allInformation = false;

                }


                if (TourSearchAmountOfDaysCheckBox.Checked)
                {
                    foreach (var symbol in TourSearchDaysAmountTextBox.Text)
                        if (!char.IsDigit(symbol)) allInformation = false;

                    if (IsEmpty(TourSearchDaysAmountTextBox)) allInformation = false;

                    if (Convert.ToDecimal(TourSearchDaysAmountTextBox.Text) <= 0) allInformation = false;
                }

                if (allInformation == false)
                {
                    MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            }

            catch
            {
                MessageBox.Show("Произошла ошибка формата введенных данных!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            using (Entities myEntities = new Entities())
            {
                var result =
                 from tour in myEntities.TOURS
                 join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                 join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                 join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                 //  from country in myEntities.COUNTRY
                 // join service in myEntities.SERVICES on country.ID_COUNTRY equals service.ID_COUNTRY
                 //join tour in myEntities.TOURS on service.ID_SERVIS equals tour.ID_TOURS
                 //join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                 select new
                 {
                     Страна = country.NAME,
                     Дата_отправления = tour.DISPATCH_DATE,
                     Дата_прибытия = tour.ARRIVAL_DATE,
                     Цена = tour.TOUR_PRICE,
                     Транспорт = transport.NAME,
                     Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                     Проживание = service.RESIDENCE,
                     Условия_проживания = service.NUTRITION,
                     Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                     Количетво_оставшихся_мест = tour.PERSON_NUMBER
                 };

                // по стране
                if (TourAddCountryCheckBox.Checked)
                {
                    var tourByCountry =
                          from tour in myEntities.TOURS
                          join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                          join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                          join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where country.NAME == TourSearchCountryComboBox.Text
                        select new
                        {
                            Страна = country.NAME,
                            Дата_отправления = tour.DISPATCH_DATE,
                            Дата_прибытия = tour.ARRIVAL_DATE,
                            Цена = tour.TOUR_PRICE,
                            Транспорт = transport.NAME,
                            Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                            Проживание = service.RESIDENCE,
                            Условия_проживания = service.NUTRITION,
                            Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                            Количетво_оставшихся_мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByCountry);
                }

                // по транспорту
                if (TourAddTransportCheckBox.Checked)
                {

                    var tourByTransport =
                          from tour in myEntities.TOURS
                          join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                          join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                          join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where transport.NAME == TourSearchTransportComboBox.Text
                        select new
                        {
                            Страна = country.NAME,
                            Дата_отправления = tour.DISPATCH_DATE,
                            Дата_прибытия = tour.ARRIVAL_DATE,
                            Цена = tour.TOUR_PRICE,
                            Транспорт = transport.NAME,
                            Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                            Проживание = service.RESIDENCE,
                            Условия_проживания = service.NUTRITION,
                            Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                            Количетво_оставшихся_мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByTransport);
                }

                // по визовому обслуживанию
                if (TourAddVisaServiceCheckBox.Checked)
                {
                    var tourByVisaService =
                         from tour in myEntities.TOURS
                         join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                         join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                         join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where service.VISA_SERVICE == (TourSearchVisaServiceComboBox.Text == "Присутствует" ? "1" : "0")
                        select new
                        {
                            Страна = country.NAME,
                            Дата_отправления = tour.DISPATCH_DATE,
                            Дата_прибытия = tour.ARRIVAL_DATE,
                            Цена = tour.TOUR_PRICE,
                            Транспорт = transport.NAME,
                            Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                            Проживание = service.RESIDENCE,
                            Условия_проживания = service.NUTRITION,
                            Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                            Количетво_оставшихся_мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByVisaService);
                }

                // по проживанию
                if (TourAddResidanceCheckBox.Checked)
                {

                    var tourByResidance =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.RESIDENCE == TourSearchResidanceСomboBox.Text
                           select new
                           {
                               Страна = country.NAME,
                               Дата_отправления = tour.DISPATCH_DATE,
                               Дата_прибытия = tour.ARRIVAL_DATE,
                               Цена = tour.TOUR_PRICE,
                               Транспорт = transport.NAME,
                               Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                               Проживание = service.RESIDENCE,
                               Условия_проживания = service.NUTRITION,
                               Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                               Количетво_оставшихся_мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByResidance);

                }

                // по условиям проживания
                if (TourAddNutritionCheckBox.Checked)
                {

                    var tourByNutrition =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.NUTRITION == TourSearchNutritionComboBox.Text
                           select new
                           {
                               Страна = country.NAME,
                               Дата_отправления = tour.DISPATCH_DATE,
                               Дата_прибытия = tour.ARRIVAL_DATE,
                               Цена = tour.TOUR_PRICE,
                               Транспорт = transport.NAME,
                               Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                               Проживание = service.RESIDENCE,
                               Условия_проживания = service.NUTRITION,
                               Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                               Количетво_оставшихся_мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByNutrition);

                }


                if (TourAddPriceCheckBox.Checked)
                {
                    decimal amount = Convert.ToDecimal(TourSearchPriceTextBox.Text);

                    if (TourSearchPriceMoreRadioButton.Checked)
                    {
                        // более
                        var tourByPrice =
                                from tour in myEntities.TOURS
                                join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                                join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                                join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                               where tour.TOUR_PRICE >= amount
                               select new
                               {
                                   Страна = country.NAME,
                                   Дата_отправления = tour.DISPATCH_DATE,
                                   Дата_прибытия = tour.ARRIVAL_DATE,
                                   Цена = tour.TOUR_PRICE,
                                   Транспорт = transport.NAME,
                                   Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                                   Проживание = service.RESIDENCE,
                                   Условия_проживания = service.NUTRITION,
                                   Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                                   Количетво_оставшихся_мест = tour.PERSON_NUMBER
                               };
                        result = result.Intersect(tourByPrice);
                    }

                    else
                    {
                        // менее
                        var tourByPrice =
                              from tour in myEntities.TOURS
                              join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                              join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                              join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                             where tour.TOUR_PRICE <= amount
                             select new
                             {
                                 Страна = country.NAME,
                                 Дата_отправления = tour.DISPATCH_DATE,
                                 Дата_прибытия = tour.ARRIVAL_DATE,
                                 Цена = tour.TOUR_PRICE,
                                 Транспорт = transport.NAME,
                                 Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                                 Проживание = service.RESIDENCE,
                                 Условия_проживания = service.NUTRITION,
                                 Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                                 Количетво_оставшихся_мест = tour.PERSON_NUMBER
                             };
                        result = result.Intersect(tourByPrice);
                    }

                }

                // по дате прибытия
                if (TourSearchDispatchDateCheckBox.Checked)
                {

                    var tourByDispatchDate =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where tour.DISPATCH_DATE == TourSearchArivalDateDateTimePicker.Value
                           select new
                           {
                               Страна = country.NAME,
                               Дата_отправления = tour.DISPATCH_DATE,
                               Дата_прибытия = tour.ARRIVAL_DATE,
                               Цена = tour.TOUR_PRICE,
                               Транспорт = transport.NAME,
                               Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                               Проживание = service.RESIDENCE,
                               Условия_проживания = service.NUTRITION,
                               Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                               Количетво_оставшихся_мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByDispatchDate);

                }

                // по дате отправления
                if (TourSearchDispatchDateCheckBox.Checked)
                {

                    var tourByDispatchDate =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where tour.DISPATCH_DATE == TourSearchDispatchDateDateTimePicker.Value
                           select new
                           {
                               Страна = country.NAME,
                               Дата_отправления = tour.DISPATCH_DATE,
                               Дата_прибытия = tour.ARRIVAL_DATE,
                               Цена = tour.TOUR_PRICE,
                               Транспорт = transport.NAME,
                               Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                               Проживание = service.RESIDENCE,
                               Условия_проживания = service.NUTRITION,
                               Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                               Количетво_оставшихся_мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByDispatchDate);

                }

                // по экскурсиям
                if (TourSearchExcursionsСheckBox.Checked)
                {

                    var tourByExcursions =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.EXCURSIONS == (TourSearchExcursionsTextBox.Text == "Включены" ? "1" : "0")
                           select new
                           {
                               Страна = country.NAME,
                               Дата_отправления = tour.DISPATCH_DATE,
                               Дата_прибытия = tour.ARRIVAL_DATE,
                               Цена = tour.TOUR_PRICE,
                               Транспорт = transport.NAME,
                               Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                               Проживание = service.RESIDENCE,
                               Условия_проживания = service.NUTRITION,
                               Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                               Количетво_оставшихся_мест = tour.PERSON_NUMBER

                           };
                    result = result.Intersect(tourByExcursions);

                }


                // по длительности пребывания
                if (TourSearchAmountOfDaysCheckBox.Checked)
                {
                    decimal amount = Convert.ToDecimal(TourSearchDaysAmountTextBox.Text);
                    // более
                    if (ToutSearchAmountOfDaysMoreRadioButton.Checked)
                    {
                        var tourByAmountOfDays =
                                from tour in myEntities.TOURS
                                join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                                join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                                join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                               where tour.STAY_LENGTH >= amount
                               select new
                               {
                                   Страна = country.NAME,
                                   Дата_отправления = tour.DISPATCH_DATE,
                                   Дата_прибытия = tour.ARRIVAL_DATE,
                                   Цена = tour.TOUR_PRICE,
                                   Транспорт = transport.NAME,
                                   Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                                   Проживание = service.RESIDENCE,
                                   Условия_проживания = service.NUTRITION,
                                   Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                                   Количетво_оставшихся_мест = tour.PERSON_NUMBER

                               };
                        result = result.Intersect(tourByAmountOfDays);

                    }

                        // менее
                    else
                    {
                        var tourByAmountOfDays =
                                    from tour in myEntities.TOURS
                                    join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                                    join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                                    join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                                   where tour.STAY_LENGTH <= amount
                                   select new
                                   {
                                       Страна = country.NAME,
                                       Дата_отправления = tour.DISPATCH_DATE,
                                       Дата_прибытия = tour.ARRIVAL_DATE,
                                       Цена = tour.TOUR_PRICE,
                                       Транспорт = transport.NAME,
                                       Визовое_обслуживание = service.VISA_SERVICE == "1" ? "Присутствует" : "Отсутствует",
                                       Проживание = service.RESIDENCE,
                                       Условия_проживания = service.NUTRITION,
                                       Экскурсии = service.EXCURSIONS == "1" ? "Включены" : "Не включены",
                                       Количетво_оставшихся_мест = tour.PERSON_NUMBER

                                   };
                        result = result.Intersect(tourByAmountOfDays);
                    }
                }

                TourSearchSearchedToursDataGridView.DataSource = result.ToList();

                if (TourSearchSearchedToursDataGridView.Rows.Count != 0)
                    TourSearchSearchedToursDataGridView.Rows[0].Selected = true;
            }
        }

        private void TourSearchAddClientButton_Click(object sender, EventArgs e)
        {
            клиентаToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void TourSearchSellTourButton_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedCountry = TourSearchSearchedToursDataGridView.SelectedCells[0].Value.ToString();
                DateTime selectedArivalDate = Convert.ToDateTime(TourSearchSearchedToursDataGridView.SelectedCells[1].Value);
                DateTime selectedDispatchDate = Convert.ToDateTime(TourSearchSearchedToursDataGridView.SelectedCells[2].Value);
                decimal selectedPrice = Convert.ToDecimal(TourSearchSearchedToursDataGridView.SelectedCells[3].Value.ToString());
                string selectedTransport = TourSearchSearchedToursDataGridView.SelectedCells[4].Value.ToString();
                string selectedVisaService = TourSearchSearchedToursDataGridView.SelectedCells[5].Value.ToString() == "Присутствует" ? "1" : "0";
                string selectedResidence = TourSearchSearchedToursDataGridView.SelectedCells[6].Value.ToString();
                string selectedNutrition = TourSearchSearchedToursDataGridView.SelectedCells[7].Value.ToString();
                string selectedExcursions = TourSearchSearchedToursDataGridView.SelectedCells[8].Value.ToString() == "Включены" ? "1" : "0";
                decimal selectedAmountTours = Convert.ToDecimal(TourSearchSearchedToursDataGridView.SelectedCells[9].Value.ToString());

                if (selectedAmountTours == 0)
                {
                    MessageBox.Show("Путевок не осталось!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (Entities myEntities = new Entities())
                {
                    decimal selectedTourID =
                        (from tour in myEntities.TOURS
                         join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                         join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                         join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                         where country.NAME == selectedCountry &&
                             //   tour.ARRIVAL_DATE == selectedArivalDate &&
                             //   tour.DISPATCH_DATE == selectedDispatchDate &&
                         tour.TOUR_PRICE == selectedPrice &&
                         transport.NAME == selectedTransport &&
                         service.VISA_SERVICE == selectedVisaService &&
                         service.RESIDENCE == selectedResidence &&
                         service.NUTRITION == selectedNutrition &&
                         service.EXCURSIONS == selectedExcursions
                         select tour.ID_TOURS).SingleOrDefault();

                    decimal priceOfTour =
                        (from tour in myEntities.TOURS
                         join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                         join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                         join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                         where country.NAME == selectedCountry &&
                             //   tour.ARRIVAL_DATE == selectedArivalDate &&
                             //   tour.DISPATCH_DATE == selectedDispatchDate &&
                         tour.TOUR_PRICE == selectedPrice &&
                         transport.NAME == selectedTransport &&
                         service.VISA_SERVICE == selectedVisaService &&
                         service.RESIDENCE == selectedResidence &&
                         service.NUTRITION == selectedNutrition &&
                         service.EXCURSIONS == selectedExcursions
                         select tour.TOUR_PRICE).SingleOrDefault();


                    decimal employeeID =
                        (from employee in myEntities.EMPLOYEE
                         where employee.LOGIN == login
                         select employee.ID_EMPLOYEE).SingleOrDefault();

                    List<string> clientFullName = new List<string>(TourSearchClientComboBox.Text.Split(new char[] { ' ' }));
                    string LastName = clientFullName[0];
                    string firstName = clientFullName[1];
                    string patronymic = clientFullName[2];

                    decimal clientID =
                        (from client in myEntities.CLIENT
                         where client.LAST_NAME == LastName &&
                         client.FIRST_NAME == firstName &&
                         client.PATRONYMIC == patronymic
                         select client.ID_CLIENT).SingleOrDefault();

                    ORDERS order = new ORDERS
                    {
                        ID_TOUR = selectedTourID,
                        ID_EMPLOYEE = employeeID,
                        ID_CLIENT = clientID,
                        REGISTRATION_DATE = DateTime.Now,
                        SALE_PRICE = priceOfTour,

                    };

                    myEntities.ORDERS.Add(order);
                    myEntities.SaveChanges();

                }
                MessageBox.Show("Путевка продана!", "Операция продажи успешно выполнена", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch
            {
                MessageBox.Show("Произошла ошибка продажи!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

            TourSearchSearchButton_Click(new object(), new EventArgs());
        }

        #endregion

        #region Отчет - Продажи за месяц
        private void продажиЗаПоследнийМесяцToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(ReportLastMonthReport);

            LastMonthReportDateTimePicker.Format = DateTimePickerFormat.Custom;
            LastMonthReportDateTimePicker.CustomFormat = "MM/yyyy";
        }

        private void LastMonthReportButton_Click(object sender, EventArgs e)
        {

            using (Entities myEntities = new Entities())
            {
                decimal summOfOrdersMoney =
                   (from order in myEntities.ORDERS
                    join tour in myEntities.TOURS on order.ID_TOUR equals tour.ID_TOURS
                    join employee in myEntities.EMPLOYEE on order.ID_EMPLOYEE equals employee.ID_EMPLOYEE
                    join client in myEntities.CLIENT on order.ID_CLIENT equals client.ID_CLIENT
                    join servive in myEntities.SERVICES on tour.ID_SERVIS equals servive.ID_SERVIS
                    join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                    orderby employee.FIRST_NAME
                    where order.REGISTRATION_DATE.Month == LastMonthReportDateTimePicker.Value.Month &&
                    order.REGISTRATION_DATE.Year == LastMonthReportDateTimePicker.Value.Year
                    select order.SALE_PRICE).Sum().GetValueOrDefault();


                var allOrders =
                    from order in myEntities.ORDERS
                    join tour in myEntities.TOURS on order.ID_TOUR equals tour.ID_TOURS
                    join employee in myEntities.EMPLOYEE on order.ID_EMPLOYEE equals employee.ID_EMPLOYEE
                    join client in myEntities.CLIENT on order.ID_CLIENT equals client.ID_CLIENT
                    join servive in myEntities.SERVICES on tour.ID_SERVIS equals servive.ID_SERVIS
                    join country in myEntities.COUNTRY on servive.ID_COUNTRY equals country.ID_COUNTRY
                    orderby employee.FIRST_NAME
                    where order.REGISTRATION_DATE.Month == LastMonthReportDateTimePicker.Value.Month &&
                    order.REGISTRATION_DATE.Year == LastMonthReportDateTimePicker.Value.Year
                    select new
                    {
                        Оформивший_сотрудник = employee.FIRST_NAME + " " + employee.LAST_NAME,
                        Купивший_клиент = client.FIRST_NAME + " " + client.LAST_NAME,
                        Страна = country.NAME,
                        Дата_отправления = tour.DISPATCH_DATE,
                        Дата_прибытия = tour.ARRIVAL_DATE,
                        Цена = tour.TOUR_PRICE,
                        Окончательная_цена = order.SALE_PRICE
                    };


                var Ворд1 = new Ворд.Word.Application();
                Ворд1.Visible = true;
                Ворд1.Documents.Add();  // Открываем новый документ
                //  Ворд1.Selection.TypeText("Список квартир в Ленинском районе");
                // Создаем таблицу из 9 строк и 2 столбцов;
                // автоподбор ширины столбцов - по
                // содержимому ячеек (wdAutoFitContent)
                CultureInfo culture = CultureInfo.CurrentCulture;

                Ворд1.Selection.TypeText("Отчет за " + LastMonthReportDateTimePicker.Value.ToString("MMMM", culture) + " " + LastMonthReportDateTimePicker.Value.Year);
                Ворд1.ActiveDocument.Tables.Add(Ворд1.Selection.Range, allOrders.Count() + 1, 7,
                Ворд.Word.WdDefaultTableBehavior.wdWord9TableBehavior,
                Ворд.Word.WdAutoFitBehavior.wdAutoFitContent);

                // Заполнять ячейки таблицы можно так
                int i = 2;
                foreach (var order in allOrders)
                {
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 1).Range.InsertAfter(order.Оформивший_сотрудник);
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 2).Range.InsertAfter(order.Купивший_клиент);
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 3).Range.InsertAfter(order.Страна);
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 4).Range.InsertAfter(order.Дата_отправления.ToShortDateString());
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 5).Range.InsertAfter(order.Дата_прибытия.ToShortDateString());
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 6).Range.InsertAfter(order.Цена.ToString());
                    Ворд1.ActiveDocument.Tables[1].Cell(i, 7).Range.InsertAfter(order.Окончательная_цена.ToString());
                    i++;
                }
                Ворд1.ActiveDocument.Tables[1].Cell(1, 1).Range.InsertAfter("Продал");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 2).Range.InsertAfter("Клиент");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 3).Range.InsertAfter("Страна");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 4).Range.InsertAfter("Дата отправления");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 5).Range.InsertAfter("Дата прибытия");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 6).Range.InsertAfter("Цена");
                Ворд1.ActiveDocument.Tables[1].Cell(1, 7).Range.InsertAfter("Окончательная цена");

                // Перевести курсор (Selection) за пределы таблицы
                Ворд1.Selection.MoveDown(Ворд.Word.WdUnits.wdLine, 12);
                Ворд1.Selection.MoveDown(Ворд.Word.WdUnits.wdLine, 12);

                Ворд1.Selection.TypeText("Продано сумму: " + summOfOrdersMoney + " рублей");
            }
        }


        #endregion

        #region О программе

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelController(HelpAbout);
        }

        #endregion

        void PanelController(Panel forShowing)
        {
            foreach (Control c in this.Controls)
            {
                if (isActivatedAdmin || isActivatedNonAdmin || forShowing == ViewChangeAcount)
                {
                    if (c is Panel) c.Visible = false;
                    if (c == forShowing) c.Visible = true;
                }
            }
        }

        void СleanControlls(params Control[] controlls)
        {
            foreach (var controll in controlls)
            {

                if (controll is TextBox)
                {
                    controll.Text = "";
                }

                if (controll is CheckBox)
                {
                    (controll as CheckBox).Checked = false;
                }

            }

        }

        bool IsEmpty(params TextBox[] controlls)
        {
            foreach (var controll in controlls)
            {
                if (controll.Text.Trim().Length == 0)
                    return true;
            }
            return false;
        }



    }
}
