using BlankVk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlankVk
{
    public partial class FormBlank : ContentPage
    {
        GetDataFromVK gdv = new GetDataFromVK();
        IEnumerable<VKData> CountryList;
        IEnumerable<VKData> CityList;
        IEnumerable<VKDataUnivers> UniverList;
        public FormBlank()
        {
            InitializeComponent();
            UserSurName.IsEnabled = false;
            Country.IsEnabled = false;
            City.IsEnabled = false;
            listviewCity.IsVisible = false;
            listviewUniver.IsVisible = false;
            Univer.IsEnabled = false;
            ButtonFillBlank.IsEnabled = false;

            GetCountries();

            ErrorUserName.IsVisible = false;
            ErrorUserSurName.IsVisible = false;
            ErrorCity.IsVisible = false;
            ErrorUniver.IsVisible = false;
        }

        private async Task GetCountries()
        {
            CountryList = await gdv.GetCountries();
            foreach (var i in CountryList)
            {
                Country.Items.Add(i.title);
            }
        }


        //ввод имени пользователя
        void TextChangedUserName(object s,TextChangedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(UserName.Text))
            {
                bool digit = false;
               foreach(char i in UserName.Text)
                {
                    if(!char.IsLetter(i))
                    {
                        digit = true;
                        break;                     
                    }
                }
               if(digit)
                {
                    UserSurName.IsEnabled = false;
                    ErrorUserName.Text = "Имя должно содержать только буквы!";
                    ErrorUserName.IsVisible = true;
                }
               else
                {
                    UserSurName.IsEnabled = true;
                    ErrorUserName.IsVisible = false;
                }
               
            }
            else
            {
                UserSurName.IsEnabled = false;
                ErrorUserName.Text = "Введите имя!";
                ErrorUserName.IsVisible = true;
            }
        }
 //ввод фамилии пользователя
       void TextChangedUserSurName(object s, TextChangedEventArgs e)
        {
             if (!String.IsNullOrWhiteSpace(UserSurName.Text))
                {
                    bool digit = false;
                    foreach (char i in UserSurName.Text)
                    {
                        if (!char.IsLetter(i))
                        {
                            digit = true;
                            break;
                        }
                    }
                    if (digit)
                    {
                        Country.IsEnabled = false;
                        ErrorUserSurName.Text = "Фамилия должна содержать только буквы!";
                        ErrorUserSurName.IsVisible = true;
                    }
                    else
                    {
                        Country.IsEnabled = true;
                        ErrorUserSurName.IsVisible = false;
                    }

                }
                else
                {
                    Country.IsEnabled = false;
                    ErrorUserSurName.Text = "Введите фамилию!";
                    ErrorUserSurName.IsVisible = true;
                }
          }
//выбор страны
       async void SelectedPickerCountry(object s, EventArgs e)
        {
            if (City.Text !=null )
            {
                City.Text = "";
                listviewCity.ItemsSource = null;
            }
            if(Univer.Text!=null)
            {
                Univer.Text = "";
                listviewUniver.ItemsSource = null;
            }

            City.IsEnabled = true;
            CityList = await gdv.GetCities(CountryList.Where(x=>x.title == Country.Items[Country.SelectedIndex]).FirstOrDefault().cid);
            listviewCity.ItemsSource = CityList;
        }


//ввод города
       async void TextChangedCity(object s, EventArgs e)
        {

            if (Univer.Text!=null)
            {
                Univer.Text = "";
                listviewUniver.ItemsSource = null;
            }

            await GetCityList();
            if (!String.IsNullOrWhiteSpace(City.Text))
            {
                bool digit = false;
                foreach (char i in City.Text)
                {
                    if (!char.IsLetter(i))
                    {
                        if(i!='-' && i!='(' && i!=')' && i!=' ')
                        {
                            digit = true;
                            break;
                        }                      
                    }
                }
                if (digit)
                {
                    Univer.IsEnabled = false;
                    ErrorCity.Text = "Название города должно содержать только буквы(и символ \"-\" )!";
                    ErrorCity.IsVisible = true;
                }
                else
                {
                    Univer.IsEnabled = true;
                    ErrorCity.IsVisible = false;
                }

            }
            else
            {
                Univer.IsEnabled = false;
                ErrorCity.Text = "Введите название города!";
                ErrorCity.IsVisible = true;
              
            }
            listviewCity.IsVisible = true;

        }
//метод получения списка городов в зависимости от ввода текста в поле City
        private async Task GetCityList()
        {
            List<VKData> dt = new List<VKData>();
            foreach (var i in CityList)
            {
                if (i.title.StartsWith(City.Text))
                    dt.Add(i);
            }
            listviewCity.ItemsSource = dt;
        }
//выбор города из списка
       async void SelectedlistviewCity(object s, SelectedItemChangedEventArgs e)
        {
            City.Text =  ((VKData)e.SelectedItem).title;
            listviewCity.IsVisible = false;
            UniverList = await gdv.GetUnivers(((VKData)e.SelectedItem).cid);
        }


//ввод университета
       async void TextChangedUniver(object s, TextChangedEventArgs e)
        {
            await GetUniverList();

            if (!String.IsNullOrWhiteSpace(Univer.Text))
            {
                ButtonFillBlank.IsEnabled = true;
                ErrorUniver.IsVisible = false;
            }
            else
            {
                ButtonFillBlank.IsEnabled = false;
                ErrorUniver.Text = "Введите название ВУЗа";
                ErrorUniver.IsVisible = true;
            }
           
            listviewUniver.IsVisible = true;

        }
        //метод получения списка вузов в зависимости от ввода текста в поле Univer
        private async Task GetUniverList()
        {
            List<VKDataUnivers> dt = new List<VKDataUnivers>();
            foreach (var i in UniverList)
            {
                if (i.title.StartsWith(Univer.Text))
                    dt.Add(i);
            }
            listviewUniver.ItemsSource = dt;
        }
        //выбор Вуза из списка
       void SelectedlistviewUniver(object s, SelectedItemChangedEventArgs e)
        {
            Univer.Text = ((VKDataUnivers)e.SelectedItem).title;
            listviewUniver.IsVisible = false;
       }

        async void BtClickFillBlank(object s, EventArgs e)
        {
            try
            {
                ModelUser model = new ModelUser()
                {
                    Name = UserName.Text,
                    SurName = UserSurName.Text,                 
                    Country = Country.Items[Country.SelectedIndex].ToString(),
                    City = City.Text,
                    Univer = Univer.Text
                };
                DataBlank db = new DataBlank(model);
                await Navigation.PushModalAsync(db);
            }
            catch(Exception er)
            {
              await DisplayAlert("Error", er.Message, "Ok");
            }
        }

    }
}
