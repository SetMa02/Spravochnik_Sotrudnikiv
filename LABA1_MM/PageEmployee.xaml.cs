using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Services.Client;

namespace LABA1_MM
{
    /// <summary>
    /// Логика взаимодействия для PageEmployee.xaml
    /// </summary>
    public partial class PageEmployee : Page
    {
        public static LabaEmployee_MMEntities DataEntitiesEmployee { get; set; }
        ObservableCollection<Employee> ListEmployee;
        private bool isDirty = true;
        public PageEmployee()
        {
            DataEntitiesEmployee = new LabaEmployee_MMEntities();
            InitializeComponent();
            ListEmployee = new ObservableCollection<Employee>();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteEmployee();
            DataGridEmployee.IsReadOnly = true;
            isDirty = true;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            Employee employee = Employee.CreateEmployee(-1, "none", "none", "none", 0);
            try
            {
                DataEntitiesEmployee.Employees.Add(employee);
                ListEmployee.Add(employee);
                DataGridEmployee.ScrollIntoView(employee);
                DataGridEmployee.SelectedIndex = DataGridEmployee.Items.Count - 1;
                DataGridEmployee.Focus();
                DataGridEmployee.IsReadOnly = false;
                isDirty = false;
            }
            catch (DataServiceRequestException ex)
            {
                throw new ApplicationException("Ошибка добавления нового сотрудника в контекст данных" + ex.ToString());
            }
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_CanExecute_2(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !isDirty;
            DataEntitiesEmployee.SaveChanges();
            isDirty = true;
            DataGridEmployee.IsReadOnly = true;
        }

        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            BorderFind.Visibility = System.Windows.Visibility.Visible;
            isDirty = false;
        }

        private void CommandBinding_CanExecute_3(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void CommandBinding_Executed_4(object sender, ExecutedRoutedEventArgs e)
        {
            DataGridEmployee.IsReadOnly = false;
            DataGridEmployee.BeginEdit();
            isDirty = false;
        }

        private void CommandBinding_CanExecute_4(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void CommandBinding_Executed_5(object sender, ExecutedRoutedEventArgs e)
        {
            Employee emp = DataGridEmployee.SelectedItem as Employee;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить сотрудника: " + emp.Surname.Trim() + " "
                    + emp.Name.Trim() + " " + emp.Pantonymic.Trim(), "Предупреждение", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesEmployee.Employees.Remove(emp);
                    DataGridEmployee.SelectedIndex = DataGridEmployee.SelectedIndex == 0 ? 1 : DataGridEmployee.SelectedIndex - 1;
                    ListEmployee.Remove(emp);
                    DataEntitiesEmployee.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления");
            }
        }

        private void CommandBinding_CanExecute_5(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmployee();
            DataGridEmployee.SelectedIndex = 0;
        }
        private void GetEmployee()
        {
            var employees = DataEntitiesEmployee.Employees;
            var queryEmployee = from employee in employees
                                orderby employee.Surname
                                select employee;
            foreach (Employee emp in queryEmployee)
            {
                ListEmployee.Add(emp);
            }
            DataGridEmployee.ItemsSource = ListEmployee;
        }
        private void RewriteEmployee()
        {
            DataEntitiesEmployee = new LabaEmployee_MMEntities();
            ListEmployee.Clear();
            GetEmployee();
        }

        private void TextBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonFindSurname.IsEnabled = true;
            ButtonFindTitle.IsEnabled = false;
            ComboBoxTitle.SelectedIndex = -1;
        }

        private void ButtonFindSurname_Click(object sender, RoutedEventArgs e)
        {
            string surname = TextBoxSurname.Text;
            DataEntitiesEmployee = new LabaEmployee_MMEntities();
            ListEmployee.Clear();
            var employees = DataEntitiesEmployee.Employees;
            var queryEmployee = from employee in employees
                                where employee.Surname == surname
                                select employee;
            foreach(Employee emp in queryEmployee)
            {
                ListEmployee.Add(emp);
            }
            if (ListEmployee.Count > 0)
            {
                DataGridEmployee.ItemsSource = ListEmployee;
                ButtonFindSurname.IsEnabled = true;
                ButtonFindTitle.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Сотрудник с фамилией \n" + surname + "\n не найден",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboBoxTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonFindTitle.IsEnabled = true;
            ButtonFindSurname.IsEnabled = false;
            TextBoxSurname.Text = "";
        }

        private void ButtonFindTitle_Click(object sender, RoutedEventArgs e)
        {
            DataEntitiesEmployee = new LabaEmployee_MMEntities();
            ListEmployee.Clear();

            Title title = ComboBoxTitle.SelectedItem as Title;
            var employees = DataEntitiesEmployee.Employees;
            var queryEmployee = from employee in employees
                                where employee.TitleID == title.ID
                                orderby employee.Surname
                                select employee;
            foreach (Employee emp in queryEmployee)
            {
                ListEmployee.Add(emp);
            }
                DataGridEmployee.ItemsSource = ListEmployee;
        }

        private void CommandBinding_Executed_6(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteEmployee();
            DataGridEmployee.IsReadOnly = true;
            isDirty = true;
            BorderFind.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
