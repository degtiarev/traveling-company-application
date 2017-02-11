using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WedInterface
{
    public partial class SearchPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    DropDownList1.DataSource = allCountries.ToList();
                    DropDownList1.DataTextField = "Страна";
                    DropDownList1.DataBind();

                    // транспорт
                    var allTransport =
                       from transport in myEntities.TRANSPORT
                       orderby transport.NAME
                       select new
                       {
                           Вид_транспорта = transport.NAME
                       };

                    DropDownList2.DataSource = allTransport.ToList();
                    DropDownList2.DataTextField = "Вид_транспорта";
                    DropDownList2.DataBind();


                    // проживание
                    List<string> residanceType = new List<string>() { "одноместное", "двухместное", "трехъместное", "четырехъместное" };
                    DropDownList4.DataSource = residanceType;
                    DropDownList4.DataBind();

                    // условия проживания
                    List<string> nutritionType = new List<string>() { "bed & breakfast", "Half Board", "Full Board" };
                    DropDownList5.DataSource = nutritionType;
                    DropDownList5.DataBind();


                    var allClients =
                        from client in myEntities.CLIENT
                        orderby client.LAST_NAME
                        select client.LAST_NAME + " " + client.FIRST_NAME + " " + client.PATRONYMIC;
                    DropDownList6.DataSource = allClients.ToList();
                    DropDownList6.DataBind();

                    Button1_Click(new object(), new EventArgs());
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (Entities myEntities = new Entities())
            {

                var result =
                     from tour in myEntities.TOURS
                     join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                     join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                     join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
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
                        Мест = tour.PERSON_NUMBER
                    };

                // по стране
                if (CheckBox1.Checked)
                {
                    var tourByCountry =
                          from tour in myEntities.TOURS
                          join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                          join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                          join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where country.NAME == DropDownList1.SelectedItem.Value
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
                            Мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByCountry);
                }

                // по транспорту
                if (CheckBox2.Checked)
                {

                    var tourByTransport =
  from tour in myEntities.TOURS
  join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
  join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
  join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where transport.NAME == DropDownList2.SelectedItem.Value
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
                            Мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByTransport);
                }

                // по визовому обслуживанию
                if (CheckBox3.Checked)
                {
                    var tourByVisaService =
                         from tour in myEntities.TOURS
                         join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                         join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                         join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                        where service.VISA_SERVICE == "1"
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
                            Мест = tour.PERSON_NUMBER
                        };
                    result = result.Intersect(tourByVisaService);
                }

                // по проживанию
                if (CheckBox4.Checked)
                {

                    var tourByResidance =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.RESIDENCE == DropDownList4.SelectedItem.Value
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
                               Мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByResidance);

                }

                // по условиям проживания
                if (CheckBox5.Checked)
                {

                    var tourByNutrition =
                           from tour in myEntities.TOURS
                           join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                           join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                           join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.NUTRITION == DropDownList5.SelectedItem.Value
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
                               Мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByNutrition);

                }

                // стоимость
                if (CheckBox6.Checked)
                {
                    decimal amount = Convert.ToDecimal(TextBox1.Text);

                    if (RadioButton1.Checked)
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
                                   Мест = tour.PERSON_NUMBER
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
                                 Мест = tour.PERSON_NUMBER
                             };
                        result = result.Intersect(tourByPrice);
                    }

                }

                // по дате прибытия
                if (CheckBox7.Checked)
                {

                    var tourByDispatchDate =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where tour.DISPATCH_DATE.Year == Calendar1.SelectedDate.Year &&
                           tour.DISPATCH_DATE.Month == Calendar1.SelectedDate.Month &&
                           tour.DISPATCH_DATE.Day == Calendar1.SelectedDate.Day
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
                               Мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByDispatchDate);

                }

                // по дате отправления
                if (CheckBox8.Checked)
                {

                    var tourByDispatchDate =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where tour.DISPATCH_DATE.Year == Calendar2.SelectedDate.Year &&
                           tour.DISPATCH_DATE.Month == Calendar2.SelectedDate.Month &&
                           tour.DISPATCH_DATE.Day == Calendar2.SelectedDate.Day
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
                               Мест = tour.PERSON_NUMBER
                           };
                    result = result.Intersect(tourByDispatchDate);

                }

                // по экскурсиям
                if (CheckBox9.Checked)
                {

                    var tourByExcursions =
                            from tour in myEntities.TOURS
                            join service in myEntities.SERVICES on tour.ID_SERVIS equals service.ID_SERVIS
                            join country in myEntities.COUNTRY on service.ID_COUNTRY equals country.ID_COUNTRY
                            join transport in myEntities.TRANSPORT on tour.ID_TRANSPORT equals transport.ID_TRANSPORT
                           where service.EXCURSIONS == "1"
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
                               Мест = tour.PERSON_NUMBER

                           };
                    result = result.Intersect(tourByExcursions);

                }


                // по длительности пребывания
                if (CheckBox10.Checked)
                {
                    decimal amount = Convert.ToDecimal(TextBox2.Text);
                    // более
                    if (RadioButton3.Checked)
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
                                   Мест = tour.PERSON_NUMBER

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
                                       Мест = tour.PERSON_NUMBER

                                   };
                        result = result.Intersect(tourByAmountOfDays);
                    }
                }

                GridView1.DataSource = result.ToList();
                GridView1.DataBind();
            }


        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int num = Convert.ToInt32(e.CommandArgument);

                Session["country"] = GridView1.Rows[num].Cells[1].Text;
                Session["dispatchDate"] = GridView1.Rows[num].Cells[2].Text;
                Session["arivalDate"] = GridView1.Rows[num].Cells[3].Text;
                Session["price"] = GridView1.Rows[num].Cells[4].Text;
                Session["transport"] = GridView1.Rows[num].Cells[5].Text;
                Session["visa"] = GridView1.Rows[num].Cells[6].Text;
                Session["residance"] = GridView1.Rows[num].Cells[7].Text;
                Session["nutrition"] = GridView1.Rows[num].Cells[8].Text;
                Session["excursions"] = GridView1.Rows[num].Cells[9].Text;
                Session["spots"] = GridView1.Rows[num].Cells[10].Text;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedCountry = Session["country"].ToString();
                DateTime selectedArivalDate = Convert.ToDateTime(Session["dispatchDate"]);
                DateTime selectedDispatchDate = Convert.ToDateTime(Session["arivalDate"]);
                decimal selectedPrice = Convert.ToDecimal(Session["price"]);
                string selectedTransport = Session["transport"].ToString();
                string selectedVisaService = Session["visa"].ToString() == "Присутствует" ? "1" : "0";
                string selectedResidence = Session["residance"].ToString();
                string selectedNutrition = Session["nutrition"].ToString();
                string selectedExcursions = Session["excursions"].ToString() == "Включены" ? "1" : "0";
                string login = Session["login"].ToString();


                using (Entities myEntities = new Entities())
                {
                    decimal selectedTourID =
                        (from country in myEntities.COUNTRY
                         join service in myEntities.SERVICES on country.ID_COUNTRY equals service.ID_COUNTRY
                         join tour in myEntities.TOURS on service.ID_SERVIS equals tour.ID_TOURS
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
                        (from country in myEntities.COUNTRY
                         join service in myEntities.SERVICES on country.ID_COUNTRY equals service.ID_COUNTRY
                         join tour in myEntities.TOURS on service.ID_SERVIS equals tour.ID_TOURS
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

                    List<string> clientFullName = new List<string>(DropDownList6.SelectedItem.Value.Split(new char[] { ' ' }));
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
                Response.Write("<script>alert('Тур успешно продан');</script>");
            }

            catch
            {
                Response.Write("<script>alert('Возникла неизвестная ошибка продажи');</script>");
            }

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Session["login"] = "";
            Response.Redirect("MainForm.aspx");
        }


    }
}