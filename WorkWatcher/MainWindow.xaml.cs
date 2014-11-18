using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WorkWatcher
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string Conn = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
        string Conn = ConfigurationManager.ConnectionStrings["TestSQL"].ConnectionString;  // Testing Purpose
        private Stopwatch LogTime_TimeWatch = new Stopwatch();
        private Stopwatch ProjectTime_TimeWatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            Username = Username.Split('\\')[1].ToString();
            UserNameValueLbl.Content = (Username == "") ? "No User" : Username;

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("Icon.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate(object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            SqlConnection connect = new SqlConnection(Conn);
            connect.Open();
            try
            {
                string sql = @"select PID,ProjectName from tbProjects";

                SqlCommand cmd = new SqlCommand(sql, connect);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    string Value = rd["PID"].ToString();
                    string Text = rd["ProjectName"].ToString();
                    ComboBoxItem item = new ComboBoxItem { Text = Text, Tag = Value };
                    ComboBoxTest.Items.Add(item);

                }
                rd.Close();
                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connect.Close();
            }

            LogOutBtn.Visibility = Visibility.Hidden;
            LogResultLbl.Visibility = Visibility.Visible;

        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Tag { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (ComboBoxTest.SelectedItem!= null)
            {
                string Result = "";
                if (LogBtn.Content.ToString() == "Login")
                {


                    if (UserNameValueLbl.Content.ToString() != "No User")
                    {

                        string username = UserNameValueLbl.Content.ToString();
                        string Project_Name = ComboBoxTest.SelectedValue.ToString();
                        //SqlConnection connect = new SqlConnection(Conn);
                        //connect.Open();
                        //try
                        //{
                        //    string sql = @"sp_LoggedUserCheck";

                        //    SqlCommand cmd = new SqlCommand(sql, connect);
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Add(new SqlParameter("@LogName", username));
                        //    cmd.Parameters.Add(new SqlParameter("@Ldate", DateTime.Now.Date));

                        //    Result = cmd.ExecuteScalar().ToString();

                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //finally
                        //{
                        //    connect.Close();
                        //}

                        Result = "User logged in";

                        if (Result != "User is not Logged Out")
                        {
                            LogBtn.Background = new SolidColorBrush(Colors.LightPink);
                            LogBtn.Content = "End Project";
                            LogResultLbl.Content = "Logged in";
                            ProjectResultLbl.Content = " Started";
                            ComboBoxTest.IsEnabled = false;
                            LogTime_TimeWatch = new Stopwatch();
                            LogTime_TimeWatch.Start();
                            ProjectTime_TimeWatch = new Stopwatch();
                            ProjectTime_TimeWatch.Start();
                            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                            dispatcherTimer.Start();
                            System.Windows.Threading.DispatcherTimer dispatcherTimerProject = new System.Windows.Threading.DispatcherTimer();
                            dispatcherTimerProject.Tick += new EventHandler(dispatcherTimerProject_Tick);
                            dispatcherTimerProject.Interval = new TimeSpan(0, 0, 1);
                            dispatcherTimerProject.Start();
                            LogOutBtn.Visibility = Visibility.Hidden;
                            LogResultLbl.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            LogResultLbl.Content = Result.Replace("User", username);
                            LogResultBottomLbl.Content = "Please Log out from the Existing App or Contact ADMIN";
                        }
                    }

                }
                else if (LogBtn.Content.ToString() == "Start Project")
                {


                    if (UserNameValueLbl.Content.ToString() != "No User")
                    {

                        string username = UserNameValueLbl.Content.ToString();
                        string Project_Name = ComboBoxTest.SelectedValue.ToString();
                        //SqlConnection connect = new SqlConnection(Conn);
                        //connect.Open();
                        //try
                        //{
                        //    string sql = @"sp_LoggedUserCheck";

                        //    SqlCommand cmd = new SqlCommand(sql, connect);
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Add(new SqlParameter("@LogName", username));
                        //    cmd.Parameters.Add(new SqlParameter("@Ldate", DateTime.Now.Date));

                        //    Result = cmd.ExecuteScalar().ToString();

                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //finally
                        //{
                        //    connect.Close();
                        //}

                        Result = "User logged in";

                        if (Result != "User is not Logged Out")
                        {
                            LogBtn.Background = new SolidColorBrush(Colors.LightPink);
                            LogBtn.Content = "End Project";
                            ProjectResultLbl.Content = " Started";
                            ComboBoxTest.IsEnabled = false;
                            ProjectTime_TimeWatch = new Stopwatch();
                            ProjectTime_TimeWatch.Start();
                            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                            dispatcherTimer.Start();
                            LogOutBtn.Visibility = Visibility.Hidden;
                            LogResultLbl.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            LogResultLbl.Content = Result.Replace("User", username);
                            LogResultBottomLbl.Content = "Please Log out from the Existing App or Contact ADMIN";
                        }
                    }

                }
                else if (LogBtn.Content.ToString() == "End Project")
                {

                    if (UserNameValueLbl.Content.ToString() != "No User")
                    {
                        string username = UserNameValueLbl.Content.ToString();
                        //SqlConnection connect = new SqlConnection(Conn);
                        //connect.Open();
                        //try
                        //{
                        //    string sql = @"sp_LoggedUserCheckOut";

                        //    SqlCommand cmd = new SqlCommand(sql, connect);
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Add(new SqlParameter("@LogName", username));
                        //    cmd.Parameters.Add(new SqlParameter("@Ldate", DateTime.Now.Date));

                        //    Result = cmd.ExecuteScalar().ToString();

                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //finally
                        //{
                        //    connect.Close();
                        //}

                        if (Result != "User has more than 1 session Logged in")
                        {

                            LogBtn.ClearValue(Button.BackgroundProperty);
                            LogBtn.Content = "Start Project";
                            ProjectResultLbl.Content = ""; ;
                            //ProjectResultLbl.Content = Result.Replace("User", username);
                            //LogResultBottomLbl.Content = "Don't work Hard!! WORK SMART";
                            ComboBoxTest.IsEnabled = true;
                            ProjectTime_TimeWatch.Stop();
                            LogOutBtn.Visibility = Visibility.Visible;
                            LogResultLbl.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            LogResultLbl.Content = Result.Replace("User", username);
                            LogResultBottomLbl.Content = "Please Contact ADMIN";
                        }


                    }

                }
            }
            else {
                LogResultBottomLbl.Content = "Please Select a Project to Login";
            }
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = LogTime_TimeWatch.Elapsed;
            LoginTimeLbl.Content = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }
        private void dispatcherTimerProject_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = ProjectTime_TimeWatch.Elapsed;
            ProjectTimeLbl.Content = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            string Result = "";
            if (LogBtn.Content.ToString() != "Login")
            {
                Result = "Please Log out before Exit";
                LogResultBottomLbl.Content = Result;
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }

           
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            string Result = "";
            if (LogBtn.Content.ToString() == "Start Project")
            {


                if (UserNameValueLbl.Content.ToString() != "No User")
                {

                    string username = UserNameValueLbl.Content.ToString();
                    string Project_Name = ComboBoxTest.SelectedValue.ToString();
                    //SqlConnection connect = new SqlConnection(Conn);
                    //connect.Open();
                    //try
                    //{
                    //    string sql = @"sp_LoggedUserCheck";

                    //    SqlCommand cmd = new SqlCommand(sql, connect);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@LogName", username));
                    //    cmd.Parameters.Add(new SqlParameter("@Ldate", DateTime.Now.Date));

                    //    Result = cmd.ExecuteScalar().ToString();

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //finally
                    //{
                    //    connect.Close();
                    //}

                    //Result = "User logged in";

                    if (Result != "User has more than 1 session Logged in")
                    {
                        LogOutBtn.Visibility = Visibility.Hidden;
                        LogBtn.ClearValue(Button.BackgroundProperty);
                        LogBtn.Content = "Login";
                        ProjectResultLbl.Content = ""; ;
                        //ProjectResultLbl.Content = Result.Replace("User", username);
                        //LogResultBottomLbl.Content = "Don't work Hard!! WORK SMART";
                        ComboBoxTest.IsEnabled = true;
                        LogOutBtn.Visibility = Visibility.Hidden;
                        LogTime_TimeWatch.Stop();
                    }
                    else
                    {
                        LogResultLbl.Content = Result.Replace("User", username);
                        LogResultBottomLbl.Content = "Please Contact ADMIN";
                    }
                   
                   
                }

            }
        }



    }
}
