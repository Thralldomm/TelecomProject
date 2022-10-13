using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace tele
{
     
    public partial class MainWindow : Window
    {
        public string kod;

       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, RoutedEventArgs e)
        {
            //здесь я добавила объекты в базу данных
            /*using(var context = new MyDbContext())
            {
                var users = new List<Users>
                {
                    new Users()
                    {
                        Number = "11111111",
                        Password = "11111",
                        Role = " Руководитель отдела по работе с клиентами"
                    },
                     new Users()
                    {
                        Number = "22222222",
                        Password = "22222",
                        Role = "Менеджер по работе с клиентами"
                    },
                     new Users()
                    {
                        Number = "33333333",
                        Password = "33333",
                        Role = "Руководитель отдела технической поддержки "
                    },
                      new Users()
                    {
                        Number = "44444444",
                        Password = "44444",
                        Role = "Специалист технической поддержки"
                    },
                       new Users()
                    {
                        Number = "55555555",
                        Password = "55555",
                        Role = "Бухгалтер"
                    },
                        new Users()
                    {
                        Number = "66666666",
                        Password = "66666",
                        Role = "Директор по развитию"
                    },
                    new Users()
                    {
                        Number = "77777777",
                        Password = "77777",
                        Role = "Сотрудник технического департамента"
                    },

                };

                context.User.AddRange(users);
                context.SaveChanges();
                 
            }*/
            
        }


        /// <summary>
        /// генерируем код
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns> 
        public string CreateCode(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890#$&*<>";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }


        /// <summary>
        /// проверяем вводимый номер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //вводим номер 
            //должны проверить, если номер равен 8 знакам
            //если да, то проверяем, есть ли номер в базе данных
            //если он есть, то проходим дальше
            //если нет- - сообщение ошибке

            if (e.Key == Key.Enter)//если нажали Enter 
            { 

                if (NumberTextBox.Text.Length == 8)//если длина текста 8
                {
                    message_textblock.Text = "";

                    using (var context = new MyDbContext())//подключаемся к нашему классу - таблице в базе данных
                    {
                        if (context.User.Any(o => o.Number == NumberTextBox.Text))//если мы нашли введенный номер в таблице, проходим дальше
                        {
                            PassTextBox.IsReadOnly = false;//открываем бокс для ввода пароля
                            CodeTextBox.IsReadOnly = false;
                            PassTextBox.Focus();//устанавливаем курсор
                            
                        }
                        else message_textblock.Text = "wrong number";
                    }
                     
                }
                else message_textblock.Text = "short number";

            }
        }


        /// <summary>
        /// проверяем вводимый пароль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//если нажали Enter 
            {

                if (PassTextBox.Text.Length == 5)//если длина текста 5
                {
                    message_textblock.Text = "";

                    using (var context = new MyDbContext())//подключаемся к нашему классу - таблице в базе данных
                    {
                        if (context.User.Any(o => o.Password == PassTextBox.Text))//если мы нашли введенный номер в таблице, проходим дальше
                        { 
                            CodeTextBox.Focus();//устанавливаем курсор
                            
                            kod = CreateCode(8);
                            load_image.IsEnabled = false;
                            MessageBox.Show(kod);
                            button2.IsEnabled = true;
                        }
                        else message_textblock.Text = "wrong number";
                    }

                }
                else message_textblock.Text = "short number";

            }
        }


        /// <summary>
        /// таймер
        /// </summary> 
        
        public async void disableLoadImage( )
        {
            load_image.IsEnabled = false;
            await Task.Delay(TimeSpan.FromSeconds(10));
            
            load_image.IsEnabled = true;
            message_textblock.Text = "Время вышло, нажмите на кнопку, генерирующую код";
             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
           //if(e.Key != Key.Enter)
                disableLoadImage();

             if (e.Key == Key.Enter)//если нажали Enter 
            {
                if (CodeTextBox.Text == kod) //если введенный код верен, то поздравляем
                {
                    MessageBox.Show("well done");

                }//если нет, то выводим сообщение об ошибке и очищаем кодТекстБокс и включаем имэдж и запускаем таймер
                else
                {
                    MessageBox.Show("Неправильный код, нажмите на синиюю кнопочку");
                    CodeTextBox.Clear();
                    load_image.IsEnabled = true;
                    //disableLoadImage();
                }
            }
        }
     
        /// <summary>
        /// Нажали на кнопку Отмена - очищаем все
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            NumberTextBox.Clear();
            PassTextBox.Clear();
            CodeTextBox.Clear();

            PassTextBox.IsReadOnly = true;
            CodeTextBox.IsReadOnly = true;

        }

        /// <summary>
        /// нажали на кнопку "новый код"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            kod = CreateCode(8);
            MessageBox.Show(kod);
            disableLoadImage();
        }


        /// <summary>
        /// нажали на кнопку "вход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //если нажали на кнопку вход, то нужно остановить таймер
            //не знаю как
             

            if (CodeTextBox.Text != kod)
            { 
                MessageBox.Show("Неправильный код, нажмите на синиюю кнопочку");
                CodeTextBox.Clear();
                load_image.IsEnabled = true;

            }
            else MessageBox.Show("okay");
            
        }

     
    }
}

