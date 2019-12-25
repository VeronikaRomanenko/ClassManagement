using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClassManagement {
	public partial class BookingForm : Form {
        StepSchedulerEntities db;

        public BookingForm(DateTime date, int timeIndex, List<string> audits, int auditIndex)
        {
            InitializeComponent();
            db = new StepSchedulerEntities();
            comboBox1.Items.Add("9:00");          //заполнение данных на форме
            comboBox1.Items.Add("10:30");
            comboBox1.Items.Add("12:00");
            comboBox1.Items.Add("13:30");
            comboBox1.Items.Add("15:00");
            comboBox1.Items.Add("16:30");
            comboBox1.Items.Add("18:00");
            comboBox1.Items.Add("19:30");
            comboBox1.SelectedIndex = timeIndex - 1;
            comboBox3.Items.AddRange(audits.ToArray());
            comboBox3.SelectedIndex = auditIndex;
            comboBox2.Items.Add("Консультация");
            comboBox2.Items.Add("Мероприятие");
            comboBox2.SelectedIndex = 0;
            dateTimePicker1.Value = date;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value < DateTime.Now)           //проверки
            {
                MessageBox.Show("Нельзя бронировать на прошедшее время");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Поле \"Преподаватель\" не может быть пустым");
                return;
            }
            Requests request = new Requests();                  //создание нового запроса на бронирование соответственно данным с формы
            request.ClassRoomId = comboBox3.SelectedIndex;
            var teacher = db.Users.Where(x => x.Login == textBox2.Text);
            if (teacher.Count(x => true) != 1)
            {
                MessageBox.Show("Некорректный логин");
                return;
            }
            request.UserId = teacher.Single().UserId;
            request.ClassDate = dateTimePicker1.Value;
            request.LessonNumber = comboBox1.SelectedIndex;
            if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("");
                return;
            }
            request.CountOfVisitors = (int)numericUpDown1.Value;
            request.EventDescription = textBox1.Text;
            db.Requests.Add(request);           //сохранение запроса в базе
            db.SaveChanges();
            MessageBox.Show("Заявка на бронирование успешно отправлена");
            Close();
        }
    }
}
